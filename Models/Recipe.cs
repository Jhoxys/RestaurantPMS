using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
	namespace RestaurantInventorySystem.Models
	{
		public class Recipe
		{
			public int Id { get; set; }
			public int ProductId { get; set; }
			public int IngredientId { get; set; }

			[Range(0.01, double.MaxValue)]
			public decimal RequiredQuantity { get; set; }

			public Product Product { get; set; }
			public Ingredient Ingredient { get; set; }
		}
	}
}
