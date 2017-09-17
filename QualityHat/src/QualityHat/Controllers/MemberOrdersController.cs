using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace QualityHat.Controllers
{
    public class MemberOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public MemberOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_userManager = userManager;
		}

        // GET: MemberOrders
        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Orders.ToListAsync());
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View(await _context.Orders.Where(o => o.User.Id == user.Id).Include(o => o.User).AsNoTracking().ToListAsync());
        }

        // GET: MemberOrders/Bag/CK
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Bag()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            // IList<Order> orders = await _context.Orders.Where(o => o.User.Id == user.Id && o.OrderStatus == 0).Include(o => o.User).AsNoTracking().ToListAsync();
            Order order = await _context.Orders
				.Include(i => i.OrderDetails)
				.AsNoTracking().
				SingleOrDefaultAsync(m => m.User.Id == user.Id && m.OrderStatus == 0);

			if (order == null) {
                order = new Order{OrderStatus = 0};

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


                    return RedirectToAction("Details", new RouteValueDictionary(
                    new { action = "Details", id = order.OrderId }));
                }
            } else {
				List<OrderDetail> details = await _context.OrderDetails.Where(o => o.Order.OrderId == order.OrderId).Include(i => i.Hat).AsNoTracking().ToListAsync();
				ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
                List<CartItem> items = cart.GetCartItems(_context);

				foreach (CartItem item in items) 
                {
                    bool ifFound = false;
                    foreach (OrderDetail detail in details)
                    {
						if (item.Hat.HatID == detail.Hat.HatID)
						{
							detail.Hat = item.Hat;
							detail.Quantity += item.Count;
							_context.Update(detail);
							ifFound = true;
                            break;
						}
					}
                    if(!ifFound)
                    {
                        OrderDetail detail = CreateOrderDetailForThisItem(item);
                        detail.Order = order;
                        details.Add(detail);
						//_context.Update(order);
                        _context.Add(detail);
                    }

                }

                //order.User = user;
                // order.OrderDate = DateTime.Today;
                // order.Total = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context);
                order.OrderDetails = details;
                _context.Update(order);
				_context.SaveChanges();

				return RedirectToAction("Details", new RouteValueDictionary(
                new { action = "Details", id = order.OrderId }));
            }

			return View(order);
        }

        // GET: MemberOrders/Details/5
        public async Task<IActionResult> Details(int? id)
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

			var details = _context.OrderDetails.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;

			return View(order);
        }

        // GET: MemberOrders/Create
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public IActionResult Create()
        {
			//ViewData["OrderStatus"] = new SelectList(_context.Orders, "OrderStatus", "OrderStatus");
			ViewBag.OrderStatus = new List<SelectListItem>
			{
				new SelectListItem {Text = "InCart", Value = "0"},
				new SelectListItem {Text = "Placed", Value = "1"},
				new SelectListItem {Text = "InProgress", Value = "2"},
				new SelectListItem {Text = "PreparingToShip", Value = "3"},
				new SelectListItem {Text = "Shipped", Value = "4"},
				new SelectListItem {Text = "Delieved", Value = "5"}
			};
			return View();
        }

        // POST: CheckoutCart
        // [HttpPost]
		//[ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
		public async Task<IActionResult> Checkout()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
            Order order = new Order{OrderStatus = 0};
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


				return RedirectToAction("Details", new RouteValueDictionary(
				new { action = "Details", id = order.OrderId }));
			}

			return View(order);
		}

        // POST: MemberOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
		public async Task<IActionResult> Create([Bind("OrderStatus,City,Country,FirstName,LastName,Phone,PostalCode,State")]Order order)
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


				return RedirectToAction("Details", new RouteValueDictionary(
				new { action = "Details", id = order.OrderId }));
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

		// GET: MemberOrders/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
			ViewBag.OrderStatus = new List<SelectListItem>
			{
				new SelectListItem {Text = "InCart", Value = "0"},
				new SelectListItem {Text = "Placed", Value = "1"},
				new SelectListItem {Text = "InProgress", Value = "2"},
				new SelectListItem {Text = "PreparingToShip", Value = "3"},
				new SelectListItem {Text = "Shipped", Value = "4"},
				new SelectListItem {Text = "Delieved", Value = "5"}
			};

			if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: MemberOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,City,Country,DelievedDate,FirstName,LastName,OrderDate,OrderStatus,Phone,PostalCode,ShippedDate,State,Total")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
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


        // GET: MemberOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

			var details = _context.OrderDetails.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;

            return View(order);
        }

        // POST: MemberOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
