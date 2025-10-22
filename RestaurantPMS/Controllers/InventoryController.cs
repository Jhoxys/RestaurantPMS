using Microsoft.AspNetCore.Mvc;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public InventoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
