using System.ComponentModel.DataAnnotations;

namespace RestaurantPMS.Models
{
    public class Tables
    {
        public int Id { get; set; }

        public int Capacity { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public string Number { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public bool IsOccupied { get; set; } = false;

        // Relación con órdenes
        public ICollection<Order>? Orders { get; set; }

    }
}
