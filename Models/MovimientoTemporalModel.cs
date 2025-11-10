using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    // Clase para mapear la lista de productos serializada desde el cliente
    public class MovimientoTemporalModel
    {
        /// <summary>
        /// ID del producto en el inventario (ID > 0) o un ID temporal (ID < 0).
        /// </summary>
        public int ID_Producto { get; set; }

        [Required]
        public string Nombre_Producto { get; set; }

        [Required]
        public string Unidad { get; set; }

        [Required]
        public decimal Costo_Unitario { get; set; }

        [Required]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Tipo de movimiento (ENTRADA o SALIDA).
        /// </summary>
        [Required]
        public string Tipo_Movimiento { get; set; }

        // -------------------------------------------------------------
        // Propiedades de Referencia/Registro (Relevantes si ID_Producto < 0 o para Logging)
        // -------------------------------------------------------------

        /// <summary>
        /// ID numérico de la categoría. Es nullable para productos existentes.
        /// </summary>
        public int? CategoriaId { get; set; }

        /// <summary>
        /// ID numérico del proveedor. Es nullable para productos existentes.
        /// </summary>
        public int? ProveedorId { get; set; }

        // Propiedades auxiliares para productos nuevos (solo usados por el repositorio)
        public decimal Stock_Minimo { get; set; }
        public string Categoria_Tipo { get; set; }
        public string Nombre_Proveedor { get; set; }


    }
}