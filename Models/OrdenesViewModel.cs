namespace RestaurantPMS.Models
{
    public class OrdenesViewModel
    {
        public List<Order> Pendientes { get; set; } = new();
        public List<Order> Preparando { get; set; } = new();
        public List<Order> Listos { get; set; } = new();
    }
}
