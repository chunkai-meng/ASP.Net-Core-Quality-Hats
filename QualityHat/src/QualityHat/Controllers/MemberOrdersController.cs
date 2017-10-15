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
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			return View(await _context.Orders.Where(o => o.User.Id == user.Id && o.OrderStatus != 0).Include(o => o.User).AsNoTracking().ToListAsync());
		}

		// GET: Orders/Edit/5
		[HttpGet]
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var order = await _context.Orders.SingleOrDefaultAsync(o => o.User.Id == user.Id && o.OrderId == id);
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

		

		// GET: MemberOrders/Bag/CK
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> AddToBag()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
			Order order = await _context.Orders
				.Include(o => o.OrderDetails)
				.AsNoTracking().
				SingleOrDefaultAsync(m => m.User.Id == user.Id && m.OrderStatus == 0);

			if (order == null)
			{
				Order orderToUpdate = new Order { OrderStatus = 0, User = user };

				if (ModelState.IsValid)
				{
					List<CartItem> items = cart.GetCartItems(_context);
					List<OrderDetail> details = new List<OrderDetail>();
					foreach (CartItem item in items)
					{

						OrderDetail detail = CreateOrderDetailForThisItem(item);
						detail.Order = orderToUpdate;
						details.Add(detail);
						_context.Add(detail);

					}
					try
					{
						await _context.SaveChangesAsync();
					}
					catch (DbUpdateException /* ex */)
					{
						//Log the error (uncomment ex variable name and write a log.)
						ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
					}

					orderToUpdate.OrderDate = DateTime.Today;
					orderToUpdate.OrderDetails = details;
					try
					{
						await _context.SaveChangesAsync();
					}
					catch (DbUpdateException /* ex */)
					{
						//Log the error (uncomment ex variable name and write a log.)
						ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
					}

					cart.EmptyCart(_context);

					
					// orderToUpdate.Total = Order.GetUserTotalPrice(user, _context);
					decimal t = Order.GetUserTotalPrice(user, _context);
					decimal gst = 0.15m * t;
					decimal total = t + gst;
					orderToUpdate.GST = gst;
					orderToUpdate.Total = total;
					await _context.SaveChangesAsync();

					return RedirectToAction("ShoppingBag", new RouteValueDictionary(
					new { action = "ShoppingBag" }));
				}
			}
			else
			{
				List<CartItem> items = cart.GetCartItems(_context);
				Order orderToUpdate = new Order
				{
					OrderId = order.OrderId,
					City = "",
					Country = "",
					Phone = "",
					PostalCode = "",
					State = "",
					User = user,
					FirstName = "",
					LastName = "",
					Total = 0,
					OrderStatus = 0
				};
				//var hatsInCart = new HashSet<int>(items.Select(i => i.Hat.HatID));
				List<OrderDetail> detailsToUpdate = new List<OrderDetail>();
				var details = _context.OrderDetail.Include(d => d.Hat).AsNoTracking().Where(d => d.Order.OrderId == orderToUpdate.OrderId);
				foreach (CartItem item in items)
				{
					var existDetail = _context.OrderDetail.AsNoTracking().SingleOrDefault(d => d.Hat.HatID == item.Hat.HatID && d.Order.OrderId == orderToUpdate.OrderId);
					bool ifNewCart = true;
					foreach (OrderDetail detail in details)
					{
						if (detail.Hat.HatID == item.Hat.HatID)
						{
							OrderDetail detailToUpdate = new OrderDetail { OrderDetailId = existDetail.OrderDetailId, Quantity = existDetail.Quantity + item.Count, UnitPrice = existDetail.UnitPrice };
							_context.Update(detailToUpdate);
							ifNewCart = false;
						}
						detailsToUpdate.Add(detail);
					}

					if (ifNewCart)
					{
						OrderDetail detailToUpdate = CreateOrderDetailForThisItem(item);
						detailToUpdate.Order = orderToUpdate;
						detailsToUpdate.Add(detailToUpdate);

						_context.Add(detailToUpdate);

					}
				}

				// orderToUpdate.Total = cart.GetTotal(_context) + order.Total;
				decimal t = cart.GetTotal(_context) + order.Total;
				decimal gst = 0.15m * t;
				decimal total = t + gst;
				orderToUpdate.GST = gst;
				orderToUpdate.Total = total;
				_context.Orders.Update(orderToUpdate);
				cart.EmptyCart(_context);

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException /* ex */)
				{
					//Log the error (uncomment ex variable name and write a log.)
					ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
				}

				return RedirectToAction("ShoppingBag", new RouteValueDictionary(
				new { action = "ShoppingBag" }));
			}
			return View(order);
		}

		// GET: MemberOrders/ShoppingBag/
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> ShoppingBag()
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
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var order = await _context.Orders
				.Include(o => o.User)
				.AsNoTracking().
				SingleOrDefaultAsync(m => m.User.Id == user.Id && m.OrderStatus == 0);
			//var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderId == id);

			if (order == null)
			{
				return View("ShoppingBagIsEmpty");
			}

			// This is a good example for how to retrive a one-many-many relationship data.
			var details = _context.OrderDetail.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;
			//var recipients = _context.Recipient.Where(i => i.User == user);
			order.User.Recipients = _context.Recipient.Where(i => i.User == user).AsNoTracking().ToList();
			return View(order);
		}

		// GET: MemberOrders/Details/5
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> Details(int? id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderId == id);

			if (order == null)
			{
				return NotFound();
			}

			var details = _context.OrderDetail.AsNoTracking().Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;
			order.User.Recipients = _context.Recipient.Where(i => i.User == user).ToList();
			return View(order);
		}


		[Authorize(Roles = "Member")]
		public async Task<IActionResult> EmptyBag(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			Order.DeleteOrder(id, _context);
			return View("ShoppingBagIsEmpty");
		}


		private OrderDetail CreateOrderDetailForThisItem(CartItem item)
		{

			OrderDetail detail = new OrderDetail();


			detail.Quantity = item.Count;
			detail.Hat = item.Hat;
			detail.UnitPrice = item.Hat.Price;

			return detail;

		}

		// POST: MemberOrders/Submit Order
		[Authorize(Roles = "Member")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Submited(int? id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var order = await _context.Orders
				.Include(o => o.OrderDetails)
				.AsNoTracking().
				SingleOrDefaultAsync(m => m.User.Id == user.Id && m.OrderStatus == 0);
			order.OrderStatus = OrderStatus.Placed;
			order.OrderDate = DateTime.Now;

			_context.Orders.Update(order);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
			}
			// return View();
			return RedirectToAction("OrderSubmited");
		}

		// Get: MemberOrders/Submit Order
		[HttpGet]
		[Authorize(Roles = "Admin, Member")]
		public IActionResult OrderSubmited()
		{
			return View();
		}

		[Authorize(Roles = "Admin, Member")]
		public async Task<IActionResult> OrderDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var order = new Order();
			if (user.UserName == "admin@email.com"){
				order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.OrderId == id);
			} else {
				order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.User.Id == user.Id && m.OrderId == id);
			}
			
			if (order == null)
            {
                return View("NotFound");
            }

			var details = _context.OrderDetail.Where(detail => detail.Order.OrderId == order.OrderId).Include(detail => detail.Hat).ToList();
			order.OrderDetails = details;

			return View(order);
        }

		private bool OrderExists(int id)
        {
           return _context.Orders.Any(e => e.OrderId == id);
        }
	}
}
