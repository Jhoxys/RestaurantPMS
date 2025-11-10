namespace RestaurantPMS.Models
{
    public class InventarioProducto
    {
        // Propiedades de Identificación
        public string TransactionNumber { get; set; }
        public int ID_Producto { get; set; }
        public int CategoriaId { get; set; }
        public int ProveedorId { get; set; }

        // Propiedades del Producto y Stock
        public string Nombre_Producto { get; set; }
        public decimal Stock_Actual { get; set; }
        public string Unidad { get; set; }
        public decimal Stock_Minimo { get; set; }
        public decimal Costo_Unitario { get; set; }

        // Propiedades de Categoría
        public string Categoria_Nombre { get; set; }
        public string Categoria_Tipo { get; set; } // Ejemplo: 'Ingrediente', 'Bebidas'

        // Propiedad de Proveedor
        public string Nombre_Proveedor { get; set; }

        // --- Propiedades de Ayuda para el Formato en la Vista (.cshtml) ---

        /// <summary>
        /// Obtiene el stock actual formateado con su unidad (ej: "150 Kg").
        /// </summary>
        public string StockCompleto => $"{Stock_Actual:N0} {Unidad}";

        /// <summary>
        /// Obtiene el stock mínimo formateado con su unidad (ej: "Min: 30 Kg").
        /// </summary>
        public string StockMinimoCompleto => $"Min: {Stock_Minimo:N0} {Unidad}";

        /// <summary>
        /// Obtiene el costo unitario formateado como moneda (ej: "$320.00 c/u").
        /// </summary>
        // Nota: :C usa el símbolo de moneda local. Si solo quieres '$', usa :N2 y concatena.
        public string CostoFormateado => $"${Costo_Unitario:N2} c/u";

    }
}
