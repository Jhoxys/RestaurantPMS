using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    public class RawProduct
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Brand { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImagenFileName { get; set; } = ""; // Nombre del archivo de imagen

        public DateTime CreatedAt { get; set; }

        // Propiedad de navegación para la relación muchos a muchos con Ingredientes
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
