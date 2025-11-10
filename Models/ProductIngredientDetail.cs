namespace RestaurantPMS.Models
{
    public class ProductIngredientDetail
    {
        public int RowId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public DateTime FechaCreacionProducto { get; set; }
        public bool ProductoActivo { get; set; }

        public int IngredienteId { get; set; }
        public decimal CantidadIngrediente { get; set; }
        public string UnidadUsada { get; set; }
        public string NombreIngrediente { get; set; }
        public string UnidadIngrediente { get; set; }
        public DateTime FechaCreacionIngrediente { get; set; }
    }
}
