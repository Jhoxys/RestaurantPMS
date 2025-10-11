using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        //[Required, MaxLength(100)]
        //public string Brand { get; set; } = "";

        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";

        [Required]
        public decimal UnidPrice { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public IFormFile? ImagenFileName { get; set; }


    }
}
