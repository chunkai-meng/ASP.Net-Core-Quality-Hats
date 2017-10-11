using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

namespace QualityHat.Models {
        public enum OrderStatus
        {
            InCart, Placed, InProgress, PreparingToShip, Shipped, Delieved
        }

    public class Order {

        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
		    public decimal GST { get; set; }
        public decimal Total { get; set; }
        public System.DateTime ShippedDate { get; set; }
        public System.DateTime DelievedDate { get; set; }

        [Display(Name = "Order Date")]
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ApplicationUser User { get; set; }



		public static decimal GetUserTotalPrice(ApplicationUser user, ApplicationDbContext _context)
		{
			decimal? total = (from detail in _context.OrderDetail
							  where detail.Order.User == user && detail.Order.OrderStatus == 0
							  select (int?)detail.Quantity * detail.UnitPrice).Sum();

			return total ?? decimal.Zero;
		}

		public static async void SetOrderTotalPrice(ApplicationUser user, ApplicationDbContext _context)
		{
			var order = await _context.Orders.SingleOrDefaultAsync(m => m.User == user && m.OrderStatus == 0);
			order.Total = GetUserTotalPrice(user, _context);
			await _context.SaveChangesAsync();
		}

		public static async void DeleteCart(ApplicationUser user, ApplicationDbContext _context)
		{
			var order = await _context.Orders.SingleOrDefaultAsync(m => m.User == user && m.OrderStatus == 0);
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}
	}

}
