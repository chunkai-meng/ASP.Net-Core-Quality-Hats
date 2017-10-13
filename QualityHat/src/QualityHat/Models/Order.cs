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
        [Display(Name = "Status")]
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
        [Display(Name = "Date Shipped")]
        public System.DateTime ShippedDate { get; set; }
        [Display(Name = "Date Delieved")]
        public System.DateTime DelievedDate { get; set; }

        [Display(Name = "Date Ordered")]
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

        public static void DeleteOrder(int? id, ApplicationDbContext db)
        {
            var order = db.Orders.AsNoTracking().SingleOrDefault(o => o.OrderId == id);
            var orderDetails = db.OrderDetail.Where(d => d.Order.OrderId == id);
            foreach (var orderDetail in orderDetails)
            {
                db.OrderDetail.Remove(orderDetail);
            }
            db.Orders.Remove(order);
            db.SaveChanges();
        }
	}
}
