using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantPMS.Models.RestaurantInventorySystem.Models;
using System.Data;

namespace RestaurantPMS.Service
{
    public class DapperContext(IConfiguration configuration)
    {
        private readonly string _restaurantDBConnectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

        public IDbConnection CreateConnection()
                => new SqlConnection(_restaurantDBConnectionString);
        public DbSet<Recipe> Recipes { get; set; }


    }
}

