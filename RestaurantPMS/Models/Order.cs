namespace RestaurantPMS.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int Table_ID { get; set; } 

        public DateTime CreatAt { get; set; }

        public string State { get; set; } = "pendiente";

        // Relación con productos

    public List<OrderProduct> OrderProducts { get; set; } = new();


        //// Relación con cliente
        public string ClientId { get; set; } = "";

        //// Relación con empleado que atiende la comanda
        public string EmployeeId { get; set; } = "";


    }
}
