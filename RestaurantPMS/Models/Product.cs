using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    public class Product
    {

        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";


        [Precision(18, 2)]
        public decimal UnidPrice { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new();



    }
}
