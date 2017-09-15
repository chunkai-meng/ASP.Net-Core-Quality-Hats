using Microsoft.EntityFrameworkCore;
using QualityHat.Models;

namespace QualityHat.Data
{
	public class ShopContext : DbContext
	{
		public ShopContext(DbContextOptions<ShopContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
