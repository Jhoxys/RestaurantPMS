using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD:Program.cs
using RestaurantPMS.Repository;
using RestaurantPMS.Service;
using Rotativa.AspNetCore;
=======
using RestaurantPMS.Models;
>>>>>>> 7e56b5d391543f7992cbb5927eb650b73dfa1f12:RestaurantPMS/Program.cs
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionStrings);
});

<<<<<<< HEAD:Program.cs
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<RestauranteRepository>();
/*
=======
>>>>>>> 7e56b5d391543f7992cbb5927eb650b73dfa1f12:RestaurantPMS/Program.cs
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(

    options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

    }).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // token usado en la action  forgot password

var app = builder.Build();
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userMnager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
        as UserManager<ApplicationUser>;

    var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
       as RoleManager<IdentityRole>;

    await DBInitializer.SeedDataAsync(userMnager, roleManager);
}

app.Run();