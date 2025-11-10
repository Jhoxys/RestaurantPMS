namespace RestaurantPMS.Models
{
    public class DashboardDto
    {
        public List<Product> Products { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<TableStatusDto> Tables { get; set; } = new();
        public int OrdersToday { get; set; }
        public int PendingOrders { get; set; }
        public int OccupiedTables { get; set; }
        public decimal IncomeToday { get; set; }
        public int LowStock { get; set; }

    }

    public class TableStatusDto
    {
        public int TableId { get; set; }
        public string Number { get; set; } = string.Empty;

        public bool IsOccupied { get; set; }

    }
}
