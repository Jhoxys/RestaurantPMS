// Models/ProductIngredient.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantPMS.Models
{
    public class ProductIngredient
    {
        public int Id { get; set; } // Clave primaria única para la relación
        // Clave primaria compuesta
        public int ProductId { get; set; }
        public int IngredientId { get; set; }

        // Propiedades de navegación a las entidades relacionadas
        public Product Product { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Para asegurar precisión en la base de datos
        public decimal Quantity { get; set; } // Cantidad del ingrediente necesaria para este producto

        [Required, MaxLength(50)]
        public string UnitOfMeasureUsed { get; set; } = ""; // Ej: "gramos", "ml"
    }
}
