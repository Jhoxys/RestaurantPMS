using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    public class TransactionRequestModel
    {
        // ==========================================================
        // PROPIEDADES DEL ENCABEZADO (TransactionHeader)
        // Coinciden con los parámetros de USP_UpsertTransactionHeader
        // ==========================================================

        /// <summary>
        /// ID de la transacción. NULL o 0 para una nueva inserción.
        /// </summary>
        public int? TransactionId { get; set; }

        [Required]
        [StringLength(10)]
        public string TipoMovimiento { get; set; } // 'ENTRADA' o 'SALIDA'

        public int? UsuarioId { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        [Required]
        [StringLength(20)]
        public string Estatus { get; set; } = "PENDIENTE";


        // ==========================================================
        // PROPIEDADES DE LOS DETALLES (Lista de TransactionDetalle)
        // Se usaría para la iteración y llamada a USP_UpsertTransactionDetalleSingle
        // ==========================================================

        /// <summary>
        /// Lista de los ítems (detalles) a insertar o actualizar en la transacción.
        /// </summary>
        [Required]
        public List<TransactionDetalleRequestModel> Detalles { get; set; } = new List<TransactionDetalleRequestModel>();
    }

    /// <summary>
    /// Modelo de datos para cada ítem de detalle dentro de la transacción.
    /// </summary>
    public class TransactionDetalleRequestModel
    {
        /// <summary>
        /// ID del detalle. NULL o 0 para un nuevo ítem (inserción).
        /// </summary>
        public int? DetalleId { get; set; }

        /// <summary>
        /// ID del producto afectado.
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        /// Cantidad movida.
        /// </summary>
        public decimal Cantidad { get; set; }

        [Required]
        [StringLength(50)]
        public string UnidadMedida { get; set; }

        /// <summary>
        /// Costo unitario al momento del movimiento.
        /// </summary>
        public decimal CostoUnitario { get; set; }

        // Propiedad auxiliar para manejar el ID temporal del cliente si es necesario, 
        // aunque no se envía a la BD.
        public int? ClientTemporaryId { get; set; }
    }
}
