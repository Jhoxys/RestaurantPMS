using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantPMS.Models;
using RestaurantPMS.Service;

namespace RestaurantPMS.Controllers
{
    public class InventoryController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly ApplicationDbContext context;

        public InventoryController(UserManager<ApplicationUser> userManager,
			   SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.context = context;
        }
        public IActionResult Index()
        {

			if (signInManager.IsSignedIn(User))
			{


			}
			else { return RedirectToAction("Login", "Account"); }



			return View();
        }
    }
}
