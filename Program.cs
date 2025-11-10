using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Repository;
using RestaurantPMS.Service;
using Rotativa.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{

    var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionStrings);
});

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<RestauranteRepository>();
/*
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
*/
var app = builder.Build();
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
