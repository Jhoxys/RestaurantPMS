using Microsoft.AspNetCore.Identity;
using RestaurantPMS.Models;

namespace RestaurantPMS.Service
{
    public class DBInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
                                            RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager or roleManager is null => exit");
                return;
            }

            // check if we have the admin role or not
            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                Console.WriteLine("Admin role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            // check if we have the admin role or not
             exists = await roleManager.RoleExistsAsync("cocina");
            if (!exists)
            {
                Console.WriteLine("cocina role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("cocina"));
            }



            // check if we have the admin role or not
            exists = await roleManager.RoleExistsAsync("sala");
            if (!exists)
            {
                Console.WriteLine("sala role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("sala"));
            }


            // check if we have the admin role or not
            var adminUsers = await userManager.GetUsersInRoleAsync("admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }


            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com",
                Address = "n/a",
                Email = "admin@admin.com",
                CreatedAt = DateTime.Now

            };

            string initialPassoword = "admin123";// default 1 time

            var result = await userManager.CreateAsync(user, initialPassoword);

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(user, "admin");
                Console.WriteLine("Admin user created successfuly please update the initial password");
                Console.WriteLine("Email: " + user.Email);
                Console.WriteLine("Initial Password: " + initialPassoword);
            }



        }



    }
}
