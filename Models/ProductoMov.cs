namespace RestaurantPMS.Models
{
    using System;

    public class ProductoMov
    {
        // Clave primaria e Identity: Captura el valor devuelto por el SP al insertar.
        public int ProductoMovId { get; set; }

        // Campos de la transacción y referencias
        public string TransactionNumber { get; set; }
        public int ProductoId { get; set; }

        // Copia de las propiedades del producto en el momento del movimiento
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal StockActual { get; set; }

        // Fechas y Referencias
        public DateTime? FechaCreacion { get; set; } // Puede ser nulo si no se maneja en el SP de inserción
        public int? CategoryId { get; set; }
        public int? ProvaiderId { get; set; }
        public decimal? StokMin { get; set; }

        // Estado del movimiento (e.g., 'Entrada', 'Salida', 'Ajuste')
        public string Estatus { get; set; }
    }
}
