// /ViewModels/TransaccionDetalleViewModel.cs
using System.Linq;
using System;
using System.Collections.Generic;
using RestaurantPMS.ViewModels;

namespace RestaurantPMS.ViewModels
{
    public class TransaccionDetalleViewModel
    {
        // -----------------------------------
        // Encabezado de la Transacción
        // -----------------------------------
        public int TransactionId { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime FechaTransaccion { get; set; } = DateTime.Now;
        public string Estatus { get; set; } = string.Empty;
        public string Observaciones { get; set; }

 
             // ... (Propiedades de Encabezado) ...

            // ⭐ ESTO CORRIGE EL CS1061 DE UnidadMedida, Cantidad, etc. ⭐
        public List<DetalleTransaccionItemViewModel> Detalles { get; set; } = new List<DetalleTransaccionItemViewModel>();

        // Propiedad calculada (debe sumar CostoTotal, no TransactionId)
        public decimal GranTotal => Detalles.Sum(d => d.CostoTotal);
        


    } 
}