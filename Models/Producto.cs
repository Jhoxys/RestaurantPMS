namespace RestaurantPMS.Models
{
    public class Producto
    {
        public int? ProductoId { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal StockActual { get; set; }
        public int? CategoryId { get; set; }
        public int? ProvaiderId { get; set; }
        public decimal? StokMin { get; set; }
    }
}
