using RestaurantPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantPMS.Service
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


             public DbSet<Product>Products { get; set; }
    
        public DbSet<RawProduct> RawProducts { get; set; }

		public DbSet<ProductIngredient> ProductIngredients { get; set; }

		//    public DbSet<IngredientInProductsDTO> IngredientInProductsDTO { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Order> Orders{ get; set; }

        public DbSet<OrderProduct> OrderProduct { get; set; }
        //      public DbSet<Billing> Billing { get; set; }
        //      public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Employee> Employee { get; set; }
        //      public DbSet<Typing> Typing { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // 🔑 clave compuesta

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);
        }



    }

}