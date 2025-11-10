namespace RestaurantPMS.Models
{
    public class ReporteDetalladoIngredientesRecetaView
    {
        public string? Ingrediente { get; set; }
        public decimal Cantidad { get; set; }
        public string? UnidadMedida { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoPorReceta { get; set; }
        public int ProductoId { get; set; }
        public int RecetaDetalleId { get; set; }
        public int RecetaId { get; set; }
        public string Receta { get; set; }
    }
}
