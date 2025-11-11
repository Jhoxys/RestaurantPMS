using Microsoft.AspNetCore.Mvc;
using RestaurantPMS.Models;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdenesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pendientes = _context.Orders.Where(o => o.State == "pendiente").ToList();
            var preparando = _context.Orders.Where(o => o.State == "preparando").ToList();
            var listos = _context.Orders.Where(o => o.State == "listo").ToList();

            var vm = new OrdenesViewModel
            {
                Pendientes = pendientes,
                Preparando = preparando,
                Listos = listos
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var orden = _context.Orders.Find(id);
            if (orden != null)
            {
                orden.State = nuevoEstado; // solo cambiamos el string
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}