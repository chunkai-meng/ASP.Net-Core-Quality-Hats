using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualityHat.Models;

namespace QualityHat.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
		public DbSet<Hat> Hats { get; set; }
        public DbSet<Category> Categorys { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<ApplicationUser> ApplicationUser { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
			builder.Entity<Hat>().ToTable("Hat");
			builder.Entity<Category>().ToTable("Category");
			builder.Entity<Supplier>().ToTable("Supplier");
		}
    }
}
