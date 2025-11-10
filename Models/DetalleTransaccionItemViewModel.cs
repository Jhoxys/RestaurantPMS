using RestaurantPMS.ViewModels;
using System;
using System.Collections.Generic;

namespace RestaurantPMS.ViewModels

{
    // Este modelo representa una sola línea del recibo
    public class DetalleTransaccionItemViewModel
    {
        public string NombreProducto { get; set; } // Nombre para mostrar en el PDF
        public decimal Cantidad { get; set; }      // Propiedad que causaba el CS1061
        public string UnidadMedida { get; set; }
        public decimal CostoUnitario { get; set; }

        // Propiedad calculada:
        public decimal CostoTotal => Cantidad * CostoUnitario;
    }
}