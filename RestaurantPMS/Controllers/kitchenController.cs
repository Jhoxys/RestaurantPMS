using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Models;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
    public class kitchenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public kitchenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vista principal de cocina
        public IActionResult Index()
        {
            var vm = new CocinaViewModel
            {
                Pendientes = _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                    .Where(o => o.State == "pendiente").ToList(),

                Preparando = _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                    .Where(o => o.State == "preparando").ToList(),

                Listos = _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                    .Where(o => o.State == "listo").ToList()
            };

            return View(vm); // Views/Kitchen/Index.cshtml
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var orden = _context.Orders.Find(id);
            if (orden != null)
            {
                orden.State = nuevoEstado;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Pendientes()
        {
            var pendientes = _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .Where(o => o.State == "pendiente")
                .OrderBy(o => o.CreatAt) // ordenados por hora
                .ToList();

            return View(pendientes);
        }
    }
}
