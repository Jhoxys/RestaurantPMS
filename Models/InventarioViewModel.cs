using X.PagedList; // Para la paginación
using System.Collections.Generic;
using RestaurantPMS.Models;
using System.ComponentModel.DataAnnotations; // Para referenciar la clase InventarioProducto


namespace RestaurantPMS.ViewModels
{

    public class InventarioViewModel
    {
        // 1. LISTA PAGINADA
        public IPagedList<InventarioProducto>? ProductosInventario { get; set; }

        public IPagedList<ProductoMov>? ProductosInventarioMov { get; set; }

        // 2. MODELO DEL FORMULARIO
        public AgregarProductoModel NuevoProducto { get; set; } = new AgregarProductoModel();

        // 3. LISTAS DE SELECCIÓN (Usando tu clase Base con Key/Value)
        public IEnumerable<Base>? CategoriasDisponibles { get; set; }
        public IEnumerable<Base>? ProveedoresDisponibles { get; set; }
    }

    // ... (La clase AgregarProductoModel permanece sin cambios, ya que maneja la entrada del formulario) ...
    public class AgregarProductoModel
    {
        public string TransactionNumber { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Nombre_Producto { get; set; }

        // CategoriaId se llenará con Base.Key
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaId { get; set; }

        // ProveedorId se llenará con Base.Key (opcional)
        public int? ProveedorId { get; set; }

        // ... (resto de las propiedades de stock y costo) ...
        [Required(ErrorMessage = "El stock actual es obligatorio.")]
        public decimal Stock_Actual { get; set; }
        [Required(ErrorMessage = "El stock mínimo es obligatorio.")]
        public decimal Stock_Minimo { get; set; }
        [Required(ErrorMessage = "La unidad es obligatoria.")]
        public string Unidad { get; set; }
        [Required(ErrorMessage = "El costo es obligatorio.")]
        public decimal Costo_Unitario { get; set; }
    }
}
