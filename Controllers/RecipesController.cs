using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Repository;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RestauranteRepository _repo;

        public RecipesController(ILogger<HomeController> logger, ApplicationDbContext context, RestauranteRepository repo)
        {
            _repo = repo;
        }
        public IActionResult ResumenRecetas()
        {
            var data = _repo.GetVistaResumenRecetas();

            var jerarquia = data
           .GroupBy(i => i.GananciaBrutaMonto)
           .Select(g => new
           {
               Receta = g.Key,
               Ingredientes = g.ToList(),
               CostoTotal = g.Sum(x => x.RecetaId)
           })
           .ToList();

            return View(data);
        }
        public IActionResult DetalleReseta(int id = 0)
        {
            if (id == 0) return RedirectToAction("ResumenRecetas");

            var data = _repo.GetAllAsync().Where(x => x.RecetaId == id).ToList();
            return View(data);
        }
    }
}
