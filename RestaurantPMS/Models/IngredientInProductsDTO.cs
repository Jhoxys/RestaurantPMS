namespace RestaurantPMS.Models
{
    public class IngredientInProductsDTO
    {
        public RawProduct? productos { get; set; }
        public List<Ingredient>? ingredients;
    }
}
