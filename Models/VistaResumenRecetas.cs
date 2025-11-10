namespace RestaurantPMS.Models
{
    public class VistaResumenRecetas
    {
        // Mapea la columna de la clave de la receta (identificador principal)
        public int RecetaId { get; set; }

        // Mapea el nombre de la receta
        public string? NombreReceta { get; set; }

        // Mapea el precio al que se vende el plato
        public decimal PrecioVenta { get; set; }

        // Mapea el costo total de los ingredientes (CostoPromedio en la tabla Receta)
        public decimal CostoTotalIngredientes { get; set; }

        // Mapea la ganancia en pesos/dólares (PrecioVenta - CostoTotalIngredientes)
        public decimal GananciaBrutaMonto { get; set; }

        // Mapea el margen de ganancia calculado como porcentaje
        public decimal MargenGananciaPorcentaje { get; set; }
    }
}
