using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace QualityHat.Models
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;


		public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			// So we can get useid here
			_userManager = userManager;
		}

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
			ViewBag.OrderStatus = new List<SelectListItem>
			{
				new SelectListItem {Text = "InCart", Value = "0"},
				new SelectListItem {Text = "Placed", Value = "1"},
				new SelectListItem {Text = "InProgress", Value = "2"},
				new SelectListItem {Text = "PreparingToShip", Value = "3"},
				new SelectListItem {Text = "Shipped", Value = "4"},
				new SelectListItem {Text = "Delivered", Value = "5"}
			};
			//return View(await _context.Orders.Include(i => i.User).AsNoTracking().Where(m => m.OrderStatus != 0).ToListAsync());
			return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
		}


		// GET: Orders/Create
		[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([Bind("City,Country,FirstName,LastName,Phone,PostalCode,State")]Order order)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);

			if (ModelState.IsValid)
			{

				ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
				List<CartItem> items = cart.GetCartItems(_context);
				List<OrderDetail> details = new List<OrderDetail>();
				foreach (CartItem item in items)
				{

					OrderDetail detail = CreateOrderDetailForThisItem(item);
					detail.Order = order;
					details.Add(detail);
					_context.Add(detail);

				}

				order.User = user;
				order.OrderDate = DateTime.Today;
				order.Total = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context);
				order.OrderDetails = details;
				_context.SaveChanges();


				return RedirectToAction("Purchased", new RouteValueDictionary(
				new { action = "Purchased", id = order.OrderId }));
			}

			return View(order);
		}

		private OrderDetail CreateOrderDetailForThisItem(CartItem item)
		{

			OrderDetail detail = new OrderDetail();


			detail.Quantity = item.Count;
			detail.Hat = item.Hat;
			detail.UnitPrice = item.Hat.Price;

			return detail;

		}
		public async Task<IActionResult> Purchased(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderId == id);
			if (order == null)
			{
				return NotFound();
			}

			var details = _context.OrderDetail.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();

			order.OrderDetails = details;
			ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);
			return View(order);
		}


		// GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.OrderStatus = new List<SelectListItem>
			{
				new SelectListItem {Text = "InCart", Value = "0"},
				new SelectListItem {Text = "Placed", Value = "1"},
				new SelectListItem {Text = "InProgress", Value = "2"},
				new SelectListItem {Text = "PreparingToShip", Value = "3"},
				new SelectListItem {Text = "Shipped", Value = "4"},
				new SelectListItem {Text = "Delivered", Value = "5"}
			};
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderStatus,Address1,Address2,City,Country,FirstName,LastName,Phone,PostalCode,State,Total")] Order order)
        {
           if (id != order.OrderId)
           {
               return NotFound();
           }

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var orderToUpdate = await _context.Orders.SingleOrDefaultAsync(o => o.OrderId == id);
            if (orderToUpdate == null)
            {
                return NotFound();
            }

           if (ModelState.IsValid)
           {
                switch (order.OrderStatus)
                {
                    case OrderStatus.Placed:
                        orderToUpdate.OrderDate = DateTime.Now;
                        break;
                    case OrderStatus.Shipped:
                        orderToUpdate.ShippedDate = DateTime.Now;
                        break;
                    case OrderStatus.Delieved:
                        order.DelievedDate = DateTime.Now;
                        break;
                    default:
                        break;
                }
				orderToUpdate.OrderStatus = order.OrderStatus;
				orderToUpdate.City = order.City;
				orderToUpdate.Country = order.Country;
				orderToUpdate.FirstName = order.FirstName;
				orderToUpdate.LastName = order.LastName;
                orderToUpdate.Address1 = order.Address1;
				orderToUpdate.Address2 = order.Address2;					
				orderToUpdate.Phone = order.Phone;
				orderToUpdate.PostalCode = order.PostalCode;
				orderToUpdate.State = order.State;

               try
               {
                   _context.Update(orderToUpdate);
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!OrderExists(order.OrderId))
                   {
                       return NotFound();
                   }
                   else
                   {
                       throw;
                   }
               }
               return RedirectToAction("Index");
           }
           return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			//var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
			var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderId == id);

			if (order == null)
            {
                return NotFound();
            }

			var details = _context.OrderDetail.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;

			return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
           return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
