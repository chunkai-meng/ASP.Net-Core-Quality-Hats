using Microsoft.EntityFrameworkCore;
using QualityHat.Models;

namespace QualityHat.Data
{
    public class ShopContext : DbContext
    {
		public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
		}

		//
		public DbSet<Hat> Hats { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Category> Categorys { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		//public DbSet<Customer> Customers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Hat>().ToTable("Hat");
			modelBuilder.Entity<Order>().ToTable("Order");
			modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
			modelBuilder.Entity<Category>().ToTable("Category");
			modelBuilder.Entity<Supplier>().ToTable("Supplier");
			//modelBuilder.Entity<Customer>().ToTable("Customer");
		}
	}
}
