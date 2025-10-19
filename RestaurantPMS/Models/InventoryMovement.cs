using RestaurantPMS.Models;
using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
	namespace RestaurantInventorySystem.Models
	{
		public enum MovementType
		{
			Purchase,
			Consumption
		}

		public class InventoryMovement
		{
			public int Id { get; set; }
			public int IngredientId { get; set; }
			public MovementType MovementType { get; set; }

			[Range(0.01, double.MaxValue)]
			public decimal Quantity { get; set; }
			public DateTime Date { get; set; }
			public int? ProductId { get; set; }
			public string Notes { get; set; }

			public Ingredient Ingredient { get; set; }
			public Product Product { get; set; }
		}
	}
}
