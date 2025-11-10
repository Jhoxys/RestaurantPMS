namespace RestaurantPMS.Models
{
    /// <summary>
    /// Esto es un DTO (Data Transfer Object) para manejar el inventario,
    /// Lo mismo que el view model pero sin la paginación.
    /// Mas lejible para el repositorio.
    /// </summary>
    public class InventoryDTO
    {
        public IEnumerable<InventarioProducto>? producto { get; set; }
        public IEnumerable<Base>? categorias { get; set; }
        public IEnumerable<Base>? proveedores { get; set; }
    }
}
