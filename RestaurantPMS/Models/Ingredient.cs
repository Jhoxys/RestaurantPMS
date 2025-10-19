// Models/Ingredient.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using RestaurantPMS.Models.RestaurantInventorySystem.Models;
using BestStoreMVC.Models.RestaurantInventorySystem.Models;
//using RestaurantPMS.Models.RestaurantInventorySystem.Models; // Asegúrate de incluir esto

namespace RestaurantPMS.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = ""; // Ej: "Harina", "Carne de Res"

        [Required, MaxLength(50)]
        public string UnitOfMeasure { get; set; } = ""; // Ej: "kg", "gramos", "litros", "unidades"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Propiedad de navegación para la relación muchos a muchos con Productos
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
		public ICollection<Recipe> Recipes { get; set; }
		public ICollection<InventoryMovement> Movements { get; set; }
	}
}



