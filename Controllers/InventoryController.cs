using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Models;
using RestaurantPMS.Repository;
using RestaurantPMS.Service;
using RestaurantPMS.ViewModels; // InventarioViewModel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using X.PagedList; // Paginación
using X.PagedList.Extensions;

namespace RestaurantPMS.Controllers
{
    public class InventoryController : Controller
    {
        private readonly RestauranteRepository _repo;

        public InventoryController(DapperContext context, RestauranteRepository repo)
        {
            _repo = repo;
        }

        // ==========================================================
        // MÉTODO PRINCIPAL: INDEX (Maneja la Paginación AJAX)
        // ==========================================================
        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;

            // 1. Obtener todos los datos
            var inventoryData = _repo.GetInventory();
            var cat = _repo.GetCategories();
            var pro = _repo.GetProviders();

            // Si el DTO o la lista de productos es nula, inicializar con listas vacías
            var products = inventoryData.ToList() ?? new List<InventarioProducto>();

            // 2. Aplicar la paginación
            var dataPaginated = products.ToPagedList(pageNumber, pageSize);

            // 3. Construir el ViewModel
            var viewModel = new InventarioViewModel
            {
                ProductosInventario = dataPaginated,
                CategoriasDisponibles = cat,
                ProveedoresDisponibles = pro,
                NuevoProducto = new AgregarProductoModel() // Inicializa el modelo del formulario vacío
            };

            // 4. Manejar la Solicitud (AJAX vs. Carga Completa)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || page.HasValue)
            {
                // Devolvemos la vista parcial sin el _Layout
                return PartialView("_ListadoProductos", viewModel);
            }

            // Si es la carga inicial de la página, devolvemos la vista completa
            return View(viewModel);
        }

        // ==========================================================
        // MÉTODO DE PROCESAMIENTO: GuardarMovimientos (Transacción Final)
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken] // Recomendado para métodos POST que modifican datos
        public IActionResult GuardarMovimientos([FromForm] string MovimientosJson)
        {
            // Usamos el ID del usuario directamente en el controlador (deberías obtenerlo de la sesión/Claims)
            const int USUARIO_ID = 1;
            int productoIdGenerado = 0;

            // --- Validación Inicial de JSON ---
            if (string.IsNullOrEmpty(MovimientosJson))
            {
                TempData["Error"] = "❌ No se recibieron movimientos para procesar. La lista estaba vacía.";
                return RedirectToAction(nameof(Index));
            }

            List<MovimientoTemporalModel> movimientos;
            try
            {
                // 1. Deserializar el JSON
                movimientos = JsonSerializer.Deserialize<List<MovimientoTemporalModel>>(
                    MovimientosJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (movimientos == null || !movimientos.Any())
                {
                    TempData["Error"] = "❌ El JSON de movimientos está vacío o es inválido.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (JsonException)
            {
                TempData["Error"] = "❌ Error de formato (JSON inválido) al procesar los datos de la transacción.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Loguea la excepción (es buena práctica)
                // _logger.LogError(ex, "Error al deserializar movimientos.");
                TempData["Error"] = $"⛔ Ocurrió un error inesperado al deserializar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            // --- Lógica Central de Movimiento ---
            string tipoMovimiento = movimientos.First().Tipo_Movimiento;
            var encabezadoId = 0;

            try
            {
                // El repositorio debe manejar la transacción de la DB (BeginTransaction, Commit, Rollback)
                // Esto simplifica el código del controlador y garantiza la atomicidad.

                // 1. Crear/Actualizar el Encabezado de la Transacción (Estado PENDIENTE)
                encabezadoId = _repo.UpsertTransactionHeader(new TransactionRequestModel()
                {
                    TransactionId = 0,
                    TipoMovimiento = tipoMovimiento,
                    UsuarioId = USUARIO_ID,
                    Observaciones = $"Transacción generada desde interfaz de inventario ({tipoMovimiento})",
                    Estatus = "PENDIENTE"
                });

                foreach (var m in movimientos)
                {
                    // 2. Manejar la creación de nuevos productos si ID_Producto es <= 0
                    if (m.ID_Producto <= 0)
                    {
                        var nuevoProducto = new Producto
                        {
                            Nombre = m.Nombre_Producto,
                            CategoryId = m.CategoriaId ?? 0,
                            ProvaiderId = m.ProveedorId ?? 0,
                            StockActual = (tipoMovimiento == "ENTRADA" ? m.Cantidad : 0), // Solo se inicializa stock en ENTRADA
                            StokMin = m.Stock_Minimo,
                            UnidadMedida = m.Unidad,
                            CostoUnitario = m.Costo_Unitario
                        };
                        productoIdGenerado = _repo.UpsertProducto(nuevoProducto);
                    }
                    // Determinar el ID final, ya sea existente o recién creado
                    var productoIdFinal = m.ID_Producto > 0 ? m.ID_Producto : productoIdGenerado;

                    // 3. Validar Stock Suficiente para SALIDA (Lógica de Negocio)
                    if (tipoMovimiento == "SALIDA")
                    {
                        // **Añadir validación real de stock aquí**
                        var stockActual = _repo.GetStockActual(productoIdFinal);
                        if (stockActual < m.Cantidad)
                        {
                            // Si el repositorio maneja la transacción, debe hacer rollback en caso de falla.
                            throw new InvalidOperationException($"Stock insuficiente para {m.Nombre_Producto}. Disponible: {stockActual}, Requerido: {m.Cantidad}.");
                        }
                    }

                    // 4. Registrar el Detalle de la Transacción
                    var detalle = new TransactionDetalleRequestModel
                    {
                        DetalleId = 0,
                        ProductoId = productoIdFinal,
                        Cantidad = m.Cantidad,
                        UnidadMedida = m.Unidad,
                        CostoUnitario = m.Costo_Unitario
                    };
                    _repo.UpsertTransactionDetalle(encabezadoId, detalle);

                    // 5. Ajustar el Stock Final
                    // La función AjustarStockProducto debe manejar la suma (ENTRADA) o resta (SALIDA)
                    // de la cantidad de stock, respetando el TipoMovimiento.
                    _repo.AjustarStockProducto(productoIdFinal, m.Cantidad, tipoMovimiento);

                }

                // 6. Marcar el Encabezado como COMPLETADO
                _repo.UpsertTransactionHeader(new TransactionRequestModel()
                {
                    TransactionId = encabezadoId,
                    TipoMovimiento = tipoMovimiento,
                    UsuarioId = USUARIO_ID,
                    Observaciones = "Transacción finalizada con éxito",
                    Estatus = "COMPLETADO"
                });

                // Retorno de éxito
                TempData["Success"] = $"✅ Transacción de {tipoMovimiento} de **{movimientos.Count}** ítems registrada con éxito.";
                var resultado = true;

            }
            catch (InvalidOperationException ex) // Para errores de negocio (ej. falta de stock)
            {
                // Marcar la transacción como fallida/cancelada (opcional, pero recomendado)
               // if (encabezadoId > 0) _repo.MarcarTransaccionFallida(encabezadoId);
                TempData["Error"] = $"⚠️ Error de negocio: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Marcar la transacción como fallida/cancelada (opcional, pero recomendado)
                //if (encabezadoId > 0) _repo.MarcarTransaccionFallida(encabezadoId);

                // Loguea la excepción para diagnóstico
                // _logger.LogError(ex, "Error al guardar movimientos.");
                TempData["Error"] = $"⛔ Ocurrió un error inesperado al procesar los movimientos: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================================================================
        // MÉTODO POST: AgregarItem (Adición de Ítem Temporal - Debería ser AJAX)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarItem(AgregarProductoModel nuevoProducto)
        {
            // NOTA: Este método POST es solo para validación del formulario de "Agregar Ítem Temporal".
            // La lógica de la UI (añadir a listaMovimiento) ocurre en JavaScript.

            // Asumiendo que 'RestauranteRepository' tiene los métodos GetCategories/GetProviders
            // para recargar la vista en caso de fallo de validación.

            if (!ModelState.IsValid)
            {
                var pageNumber = 1;
                var pageSize = 8;

                // Recarga de datos para mostrar la vista con errores de validación
                var inventoryData = _repo.GetInventory();
                var cat = _repo.GetCategories();
                var pro = _repo.GetProviders();
                var products = inventoryData.ToList() ?? new List<InventarioProducto>();
                var dataPaginated = products.ToPagedList(pageNumber, pageSize);

                var viewModel = new InventarioViewModel
                {
                    ProductosInventario = dataPaginated,
                    CategoriasDisponibles = cat,
                    ProveedoresDisponibles = pro,
                    NuevoProducto = nuevoProducto // Devolvemos el modelo con los errores
                };

                TempData["Warning"] = "⚠️ Corrija los errores del formulario para agregar el producto temporal.";
                return View("Index", viewModel);
            }

            // El JS maneja la adición del ítem temporal a la lista. 
            // Si el formulario se envía por POST, asumimos que la lógica de negocio
            // debe ser re-evaluada o que esto era solo un punto de validación.

            // Si decides usar este método para agregar directamente a una tabla temporal de BD,
            // puedes implementar la lógica aquí (pero no es lo que el JS está haciendo actualmente).

            TempData["Success"] = "✅ Ítem temporal validado con éxito. Por favor, finalice la transacción.";

            // Redirigir al Index (patrón PRG)
            return RedirectToAction("Index");
        }

    }
}