using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityHat.Models
{
    public static class DbInitializer
    {
		public static void Initialize(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();

			// Look for any students.
			if (context.Categorys.Any())
			{
				return;   // DB has been seeded
			}

			var applicationUsers = new ApplicationUser[]
			{
				new ApplicationUser{UserName="test@me.com", Email="test@me.com", Address="Unitec", EmailConfirmed=true, CustomerName="Test", 
					PhoneNumber="021-123123", PasswordHash="AQAAAAEAACcQAAAAEGt6T1qkdeJrm1vOCon/mjRZUvMxZVSWL4mrenW0kmrAQDdUY2iZUDW9v7ldY6qAiw==",
					SecurityStamp="74459115-ad54-45d4-bbfa-2fa5e2ea20e6",Enabled=true}
			};
			foreach (ApplicationUser c in applicationUsers)
			{
				context.ApplicationUser.Add(c);
			}
			context.SaveChanges();


			var categorys = new Category[]
			{
				new Category{Name="Mens"},
				new Category {Name="WOMENS" },
				new Category {Name="KIDS" }
			};
			foreach (Category c in categorys)
			{
				context.Categorys.Add(c);
			}
			context.SaveChanges();

			var suppliers = new Supplier[]
			{
				new Supplier{Name="Adidas",WorkPhone="021-2344222",Email="test@gmail.com", Address="42 Park Road, Auckland" },
				new Supplier{Name="361",WorkPhone="021-6433888",Email="test2@hotmail.com", Address="2A Princess Road, Auckland"},
				new Supplier{Name="Nike",WorkPhone="021-8788883",Email="CustomerService@me.com", Address="771 Wanghhari Road, Auckland" },
			};
			foreach (Supplier s in suppliers)
			{
				context.Suppliers.Add(s);
			}
			context.SaveChanges();

			var hats = new Hat[]
			{
				// new Hat{Name="Carson",CategoryID=1,Price=11.9, Disc="The first hat", Image="/images/hat1.png"}
				new Hat { Name="Good Dog",Price=11,Disc="This is a hat's discription",Image="/images/hats/636408330595139931.png",CategoryID=1,SupplierID=1 },
				new Hat { Name="Cool Hat",Price=11,Disc="Here is the hat's discription",Image="/images/hats/636408476761332375.png",CategoryID=2,SupplierID=2 },
				new Hat { Name="Fans Hat",Price=11,Disc="Want to know more about this cool hat? ",Image="/images/hats/636410890049279767.jpg",CategoryID=3,SupplierID=3 },
			};
			foreach (Hat h in hats)
			{
				context.Hats.Add(h);
			}
			context.SaveChanges();

			//var customers = new Customer[]
			//{
			//	new Customer { Name="cust1", MobilePhone="021-11111", HomePhone="2344234", WorkPhone="234286", Email="asdfa@jkjc.com", Address="asdf asd dfs-22", CustomerStatus=CustomerStatus.Active },
			//	new Customer { Name="cust2", MobilePhone="021-222222", HomePhone="2344234", WorkPhone="234286", Email="asdfa@jkjc.com", Address="asdf asd dfs-22", CustomerStatus=CustomerStatus.Active },
			//	new Customer { Name="cust3", MobilePhone="021-33333", HomePhone="2344234", WorkPhone="234286", Email="asdfa@jkjc.com", Address="asdf asd dfs-22", CustomerStatus=CustomerStatus.Active }
			//};
			//foreach (Customer c in customers)
			//{
			//	context.Customers.Add(c);
			//}
			//context.SaveChanges();

			//var orders = new Order[]
			//{
			//	new Order { CustomerID=1, OrderStatus=OrderStatus.InCart, Subtotal=23.5, GST=3.2, GrandTotal=23 }
			//};
			//foreach (Order o in orders)
			//{
			//	context.Orders.Add(o);
			//}
			//context.SaveChanges();

			//var orderItems = new OrderItem[]
			//{
			//	new OrderItem { HatID=1, OrderID=1, Quantity=1 },
			//	new OrderItem { HatID=2, OrderID=1, Quantity=2 },
			//};
			//foreach(OrderItem oi in orderItems)
			//{
			//	context.OrderItems.Add(oi);
			//}
			//context.SaveChanges();


			//var enrollments = new Enrollment[]
			//{
			//new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
			//new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
			//new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
			//new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
			//new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
			//new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
			//new Enrollment{StudentID=3,CourseID=1050},
			//new Enrollment{StudentID=4,CourseID=1050},
			//new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
			//new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
			//new Enrollment{StudentID=6,CourseID=1045},
			//new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
			//};
			//foreach (Enrollment e in enrollments)
			//{
			//	context.Enrollments.Add(e);
			//}
			//context.SaveChanges();
		}
	}
}
