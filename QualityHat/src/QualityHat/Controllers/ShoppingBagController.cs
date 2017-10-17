using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityHat.Models;
using QualityHat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace QualityHat.Controllers
{
	[Authorize(Roles = "Member")]
    public class ShoppingBagController : Controller
    {

		private readonly ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public ShoppingBagController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles = "Member")]
		public async Task<IActionResult> DeleteOneFromBag(int id)
		{
			var detail = await _context.OrderDetail.SingleOrDefaultAsync(m => m.OrderDetailId == id);

			if (detail != null)
			{
				if(detail.Quantity > 1)
				{
					detail.Quantity --;
				}
				else
				{
					_context.Remove(detail);
				}
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

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var orderToUpdate = await _context.Orders.AsNoTracking().SingleOrDefaultAsync(m => m.User == user && m.OrderStatus == 0);
			// orderToUpdate.Total = Order.GetUserTotalPrice(user, _context);
			decimal t = Order.GetUserTotalPrice(user, _context);
			if (t == 0)
			{
				Order.DeleteOrder(orderToUpdate.OrderId, _context);
			} else
			{
				decimal gst = 0.15m * t;
				decimal total = t + gst;
				orderToUpdate.GST = gst;
				orderToUpdate.Total = total;
				await _context.SaveChangesAsync();
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		[Authorize(Roles = "Member")]
		public async Task<IActionResult> AddOneToBag(int id)
		{
			var detail = await _context.OrderDetail.SingleOrDefaultAsync(m => m.OrderDetailId == id);

			if (detail != null)
			{
				detail.Quantity++;
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

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var orderToUpdate = await _context.Orders.SingleOrDefaultAsync(m => m.User == user && m.OrderStatus == 0);
			// orderToUpdate.Total = Order.GetUserTotalPrice(user, _context);
			decimal t = Order.GetUserTotalPrice(user, _context);
			decimal gst = 0.15m * t;
			decimal total = t + gst;
			orderToUpdate.GST = gst;
			orderToUpdate.Total = total;
			await _context.SaveChangesAsync();


			return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}