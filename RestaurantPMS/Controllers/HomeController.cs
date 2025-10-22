using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantPMS.Models;
using RestaurantPMS.Service;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RestaurantPMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 5;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int pageIndex,  string? search, int? Id_Table)
        {
            // pageIndex = ViewData["PageIndex"] != null ? (int)ViewData["PageIndex"] : 1;


            IQueryable<Product> query = _context.Products;
         ///   IQueryable<Order> queryOrder = _context.Orders;
            // search funtionality
            if (search != null  )
            {
                query = query.Where(s => s.Category.Contains(search));// agregamos una consulta de resultados
            }


            // Pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count(); // total del numero de paginas
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var products = query.ToList();
  //          var orders = _context.Orders
  //      .Include(o => o.OrderProducts) // <-- Esto es importante
  ////  .Where(o => o.Product.Any()) // Solo órdenes con al menos un producto

  //      .ToList();

                 var orders = _context.Orders
     .Include(o => o.OrderProducts)
        .ThenInclude(op => op.Product)
       .ToList();
 




            ViewBag.Orders = orders;
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;

            ViewData["Search"] = search ?? "";

           //return RedirectToAction("ggg", "Products");

            return View(products);
        }


        public IActionResult Bill()
        {


            return View();

        }


        public IActionResult Menues()
        {


            return View();

        }

        public IActionResult GetTables()
        {


            return View();

        }
        public IActionResult Cocina()
        {


            return View();

        }


        public IActionResult Orders()
         {


            return View();

        }

            [HttpPost]
        public  async Task<IActionResult> Orders(int Id, int idMesa)
        {
            var product = await _context.Products.FindAsync(Id);

            if (product != null)
            {
                var order = new Order
                {
                    Table_ID = idMesa,
                    CreatAt = DateTime.Now,
                    ClientId = "1",
                    State = "pendiente"
                };

                // Agregar la relación intermedia
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    Product = product,
                    Quantity = 1
                };

                order.OrderProducts.Add(orderProduct);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                TempData["error"] = "Producto agregado a la orden correctamente";
            }
            else
            {
                TempData["error"] = "Producto no encontrado";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
