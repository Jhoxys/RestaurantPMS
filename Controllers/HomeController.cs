using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Models;
using RestaurantPMS.Service;
using System.Diagnostics;

namespace RestaurantPMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 5;

        public HomeController(
            UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
        }

        public IActionResult Index(int pageIndex = 1,  string? search = null, int? Id_Table = null)
        {
			var today = DateTime.Today;

            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(search))
                query = query.Where(s => s.Category.Contains(search));

            if (pageIndex < 1) pageIndex = 1;

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (decimal)_pageSize);

            var products = query
                .Skip((pageIndex - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var orders = _context.Orders
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .Include(o => o.Table)
                .ToList();

            // Mesas reales desde la BD
            var tables = _context.Tables
                .Select(t => new TableStatusDto
                {
                    TableId = t.Id,
                    Number = t.Number,
                    IsOccupied = t.IsOccupied
                })
                .ToList();

            // Métricas
            var model = new DashboardDto
            {
                Products = products,
                Orders = orders,
                OrdersToday = orders.Count(o => o.CreatAt.Date == today),
                PendingOrders = orders.Count(o => o.State == "Pendiente"),
                OccupiedTables = orders.Where(o => o.State != "Completada")
                                       .Select(o => o.TableId).Distinct().Count(),
                IncomeToday = orders.Where(o => o.CreatAt.Date == today && o.State == "Completada")
                                    .SelectMany(o => o.OrderProducts)
                                    .Sum(op => op.Product.UnidPrice * op.Quantity),
                LowStock = _context.Products.Count(p => p.Stock < 5),
                Tables = tables
            };


            ViewBag.Orders = orders;
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            ViewData["Search"] = search ?? "";

            return View(model);
        }

        public IActionResult Bill() => View();
        public IActionResult Menues() => View();
        public IActionResult GetTables() => View();
        public IActionResult Cocina() => View();
        public IActionResult Orders() => View();

        [HttpPost]
        public  async Task<IActionResult> Orders(int Id, int idMesa)
        {
            var product = await _context.Products.FindAsync(Id);

            if (product == null)
            {
                TempData["error"] = "Producto no encontrado";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                TableId = idMesa,
                CreatAt = DateTime.Now,
                ClientId = $"Mesa-{idMesa}",
                State = "Pendiente",
                OrderProducts = new List<OrderProduct>()
            };

            order.OrderProducts.Add(new OrderProduct
            {
                Order = order,
                Product = product,
                Quantity = 1
            });

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["message"] = "Producto agregado a la orden correctamente";
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}