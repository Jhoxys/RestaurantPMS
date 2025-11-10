using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantPMS.Models;
using RestaurantPMS.Service;
using RestaurantPMS.ViewModels;
using System;
using System.Data;


namespace RestaurantPMS.Repository
{
    public class RestauranteRepository
    {
        private readonly DapperContext _context;

        public RestauranteRepository(DapperContext context)
        {
            _context = context;
        }

        #region Get 
        public IEnumerable<ReporteDetalladoIngredientesRecetaView> GetAllAsync()
        {
            var query = "SELECT * FROM [dbo].[VW_GET_ReporteDetalladoIngredientesReceta]";
            using (var connection = _context.CreateConnection())
            {
                var recetas = connection.Query<ReporteDetalladoIngredientesRecetaView>(query).ToList();
                return recetas;
            }
        }
        public IEnumerable<VistaResumenRecetas> GetVistaResumenRecetas()
        {
            var query = "select * from [dbo].[VistaResumenRecetas]";
            using (var connection = _context.CreateConnection())
            {
                var recetas = connection.Query<VistaResumenRecetas>(query).ToList();
                return recetas;
            }
        }

        public IEnumerable<ProductoMov> GetInventoryTemp()
        {
            var query = "select * from [dbo].[Vista_InventarioActualMov_Resultado] order by 4";
            using (var connection = _context.CreateConnection())
            {
                var inventory = connection.Query<ProductoMov>(query).ToList();
                return inventory;
            }
        }
        public IEnumerable<InventarioProducto> GetInventory()
        {
            var query = "select distinct * from [dbo].[Vista_InventarioActual_Resultado] ORDER BY 4 ";
            using (var connection = _context.CreateConnection())
            {
                var inventory = connection.Query<InventarioProducto>(query).ToList();
                return inventory;
            }
        }
        public IEnumerable<Base> GetCategories()
        {
            // Asumo que tienes una tabla de Categorias con ID y un campo de nombre o tipo.
            var query = "SELECT distinct [CategoriaId] as [Key] ,[Nombre]   as [Value] FROM [dbo].[Categoria] \r\n";
            using (var connection = _context.CreateConnection())
            {
                // NOTA: Dapper mapeará los resultados a la clase CategoriaSelect
                var categorias = connection.Query<Base>(query).ToList();
                return categorias;
            }
        }

        // ⚠️ MÉTODO FALTANTE 2: Obtener Proveedores para el Dropdown
        public IEnumerable<Base> GetProviders()
        {
            // Asumo que tienes una tabla de Proveedores con ID y Nombre.
            var query = "SELECT [ProveedorId]  AS [Key], [Nombre] AS [Value] FROM Proveedor WHERE Activo = 1 ORDER BY Nombre";
            using (var connection = _context.CreateConnection())
            {
                var proveedores = connection.Query<Base>(query).ToList();
                return proveedores;
            }
        }

        #endregion

        #region Set
        public Base UpsertProduct(Producto producto)
        {
            var baseResult = new Base();
            // 1. Dapper mapea automáticamente las propiedades de 'producto' a los parámetros del SP.
            var parameters = new DynamicParameters(producto);

            using (var connection = _context.CreateConnection())
            {
                // 2. Ejecutamos el SP y usamos Query<Base> para mapear el resultado de la cláusula OUTPUT.
                // El resultado será una colección (Enumerable) de objetos Base.
                baseResult = connection.Query<Base>("sp_UpsertProducto_Params", parameters, commandType: CommandType.StoredProcedure
            ).FirstOrDefault();

                // Devolvemos el objeto Base que contiene el ID y la acción.
            }
            return baseResult ?? new Base();
        }


        public int InsertProductMovement(ProductoMov movimiento)
        {

            movimiento.TransactionNumber = string.IsNullOrWhiteSpace(movimiento.TransactionNumber)
            ? new ServiceHelper(_context).GenerateTransactionNumber()
            : movimiento.TransactionNumber;

            var spName = "sp_InsertarProductoMovimiento";

            var parameters = new DynamicParameters();
            parameters.Add("@TransactionNumber", movimiento.TransactionNumber);
            parameters.Add("@ProductoId", movimiento.ProductoId);
            parameters.Add("@Nombre", movimiento.Nombre);
            parameters.Add("@UnidadMedida", movimiento.UnidadMedida);
            parameters.Add("@CostoUnitario", movimiento.CostoUnitario);
            parameters.Add("@StockActual", movimiento.StockActual);
            parameters.Add("@CategoryId", movimiento.CategoryId);
            parameters.Add("@ProvaiderId", movimiento.ProvaiderId);
            parameters.Add("@StokMin", movimiento.StokMin);
            parameters.Add("@Estatus", movimiento.Estatus);



            using (var connection = _context.CreateConnection())
            {
                int nuevoId = connection.ExecuteScalar<int>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return nuevoId;
            }
        }

        public int UpsertTransactionHeader(TransactionRequestModel headerRequest)
        {
            var spName = "USP_UpsertTransactionHeader";

            // Opcional: Puedes usar DynamicParameters o pasar el objeto headerRequest directamente.
            // Usaremos DynamicParameters solo para demostrar la limpieza del código.
            var parameters = new DynamicParameters();
            parameters.Add("@TransactionId", headerRequest.TransactionId);
            parameters.Add("@TipoMovimiento", headerRequest.TipoMovimiento);
            parameters.Add("@UsuarioId", headerRequest.UsuarioId);
            parameters.Add("@Observaciones", headerRequest.Observaciones);
            parameters.Add("@Estatus", headerRequest.Estatus);

            using (var connection = _context.CreateConnection())
            {
                // 1. CAMBIO CLAVE: Usar ExecuteScalar<int> para capturar el valor del SELECT final.
                // ExecuteScalar devuelve el valor de la primera columna de la primera fila.
                int transactionId = connection.ExecuteScalar<int>(
                    sql: spName, // Dapper asume que el nombre del parámetro es el mismo que en el SP
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                // 2. Devolver el ID capturado
                return transactionId;
            }
        }
        public int UpsertTransactionDetalle(int transactionId, TransactionDetalleRequestModel detalle)
        {
            var spName = "USP_UpsertTransactionDetalleSingle";

            var parameters = new DynamicParameters();

            // Claves
            parameters.Add("@TransactionId", transactionId);
            // Si DetalleId es 0 o NULL en el modelo, Dapper lo envía correctamente.
            parameters.Add("@DetalleId", detalle.DetalleId == 0 ? (int?)null : detalle.DetalleId);
            parameters.Add("@ProductoId", detalle.ProductoId);

            // Datos del movimiento
            parameters.Add("@Cantidad", detalle.Cantidad);
            parameters.Add("@UnidadMedida", detalle.UnidadMedida);
            parameters.Add("@CostoUnitario", detalle.CostoUnitario);

            // ⛔ Eliminamos el parámetro de salida @ResultDetalleId,
            // ya que el SP debe devolverlo con SELECT para usar ExecuteScalar.

            using (var connection = _context.CreateConnection())
            {
                // ⭐ CAMBIO CLAVE: Usamos ExecuteScalar<int> (síncrono)
                // Esto captura el valor devuelto por la sentencia SELECT del SP.
                int detalleId = connection.ExecuteScalar<int>(
                    sql: spName,
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Devolvemos el ID capturado
                return detalleId;
            }
        }
        public int UpsertProducto(Producto producto)
        {
            var spName = "USP_UpsertProducto";

            var parameters = new DynamicParameters();

            // 1. Parámetros de Entrada
            // Si ProductoId es 0, lo enviamos como NULL para activar la inserción en el MERGE
            parameters.Add("@ProductoId", producto.ProductoId == 0 ? (int?)null : producto.ProductoId);
            parameters.Add("@Nombre", producto.Nombre);
            parameters.Add("@UnidadMedida", producto.UnidadMedida);
            parameters.Add("@CostoUnitario", producto.CostoUnitario);
            parameters.Add("@StockActual", producto.StockActual);
            parameters.Add("@CategoryId", producto.CategoryId);
            parameters.Add("@ProvaiderId", producto.ProvaiderId);
            parameters.Add("@StokMin", producto.StokMin);

            // 2. Eliminamos el parámetro de salida @ResultProductoId de C#
            // El SP ya no lo tiene y usa SELECT.

            using (var connection = _context.CreateConnection())
            {
                // CAMBIO CLAVE: Usamos ExecuteScalar<int> para capturar el resultado del SELECT
                int productoId = connection.ExecuteScalar<int>(
                     sql: spName,
                     param: parameters,
                     commandType: CommandType.StoredProcedure
                 );

                // 3. Devolvemos el ID capturado directamente
                return productoId;
            }
        }
        public decimal AjustarStockProducto(int productoId, decimal cantidadAjuste, string tipoMovimiento)
        {
            var spName = "USP_AjustarStockProducto";
            var parameters = new DynamicParameters();

            // 1. Determinar el signo del ajuste basado en el tipo de movimiento.
            // Si es "SALIDA", la cantidad se vuelve negativa. De lo contrario, es positiva (ENTRADA).
            /*decimal ajusteFinal = (tipoMovimiento.ToUpper() == "SALIDA")
                                    ? -cantidadAjuste
                                    : cantidadAjuste;*/

            parameters.Add("@ProductoId", productoId);
            parameters.Add("@CantidadAjuste", cantidadAjuste);
            parameters.Add("@TipoMovimiento", tipoMovimiento);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    // 3. Ejecuta el SP. El SP debe devolver el nuevo StockActual.
                    decimal nuevoStock = connection.ExecuteScalar<decimal>(
                        sql: spName,
                        param: parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return nuevoStock;
                }
                catch (Exception ex)
                {
                    // Manejo de errores de base de datos
                    // Loguea la excepción (opcional, pero recomendado)
                    throw new Exception($"Error al ajustar el stock del Producto ID {productoId}. SQL SP: {spName}", ex);
                }
            }
        }

        /// <summary>
        /// Obtiene el StockActual de un producto específico.
        /// </summary>
        /// <param name="productoId">El ID del producto a consultar.</param>
        /// <returns>El valor decimal del StockActual. Devuelve 0 si el producto no existe.</returns>
        public decimal GetStockActual(int productoId)
        {
            var spName = "USP_GetStockActualProducto";

            var parameters = new DynamicParameters();
            parameters.Add("@ProductoId", productoId);

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    // ExecuteScalar<decimal> es ideal para obtener un único valor (el stock)
                    decimal stock = connection.ExecuteScalar<decimal>(
                        sql: spName,
                        param: parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Si el productoId no se encuentra, ExecuteScalar podría devolver el valor por defecto (0 para decimal)
                    return stock;
                }
                catch (Exception ex)
                {
                    // Es buena práctica registrar el error
                    // _logger.LogError(ex, "Error al obtener stock actual del producto {Id}.", productoId);
                    throw new Exception($"Error al consultar el stock del Producto ID {productoId}.", ex);
                }
            }
        }
        // RestauranteRepository.cs - CÓDIGO CORRECTO (Lógica General)
        public TransaccionDetalleViewModel GetDetalleTransaccion(int transactionId)
        {
            // 1. Obtener los datos del encabezado
            var encabezadoDB = transactionId;

            // 2. Obtener los datos de los detalles (lista de filas)
            var detallesDB = new List<dynamic>
            {
                new { Nombre = "Producto A", Cantidad = 2m, Unidad = "kg", Costo = 50m },
                new { Nombre = "Producto B", Cantidad = 1.5m, Unidad = "ltr", Costo = 30m },
                // ... (Más detalles)
            };

            // 3. Crear y mapear el ViewModel del encabezado (sin la lista aún)
            var viewModel = new TransaccionDetalleViewModel
            {
                TransactionId = encabezadoDB,
                TipoMovimiento = "SALIDA, ENTRADA",
                // ... (Otras propiedades del encabezado) ...
            };

            // 4. ⭐ CORRECCIÓN: Usar LINQ o un bucle para mapear CADA DETALLE ⭐
            // Cada elemento 'd' en 'detallesDB' DEBE ser mapeado al tipo de item.
            viewModel.Detalles = detallesDB.Select(d => new DetalleTransaccionItemViewModel
            {
                NombreProducto = d.Nombre,
                Cantidad = d.Cantidad,
                UnidadMedida = d.Unidad,
                CostoUnitario = d.Costo,
                // El CostoTotal es una propiedad calculada en el ViewModel
            }).ToList();

            return viewModel;
        }


        #endregion

    } 
}




