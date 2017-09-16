using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public decimal Total { get; set; }

        public System.DateTime ShippedDate { get; set; }
        public System.DateTime DelievedDate { get; set; }

        [Display(Name = "Order Date")]
        public System.DateTime OrderDate { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public ApplicationUser User { get; set; }

    }

}