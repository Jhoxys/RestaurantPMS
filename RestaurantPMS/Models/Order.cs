namespace RestaurantPMS.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Clave foránea a la mesa
        public int Table_ID { get; set; }

        // Prpiedad de navegación
        public Table Table { get; set; } = null!;

        public DateTime CreatAt { get; set; }

        public string State { get; set; } = "pendiente";

        public string ClientId { get; set; } = "";

        public string EmployeeId { get; set; } = "";

        public int TableId { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}
