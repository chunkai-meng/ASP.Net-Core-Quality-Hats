using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QualityHat.Controllers
{
	[Authorize(Roles = "Member")]
	public class RecipientsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public RecipientsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
			_userManager = userManager;
		}

        // GET: Recipients
        public async Task<IActionResult> Index()
        {
			ApplicationUser user = await _userManager.GetUserAsync(User);
			return View(await _context.Recipient.Where(i => i.User == user).ToListAsync());
        }

		// GET: Recipients/Details/5
		public async Task<IActionResult> SetDefault(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var defaultRecipient = _context.Recipient.AsNoTracking().SingleOrDefault(r => r.RecipientId == id);
			var allRecipient = await _context.Recipient.Where(i => i.User == user).ToListAsync();
			foreach ( Recipient recipient in allRecipient)
			{
				if (recipient.RecipientId == id)
				{
					recipient.Default = true;
				}
				else
				{
					recipient.Default = false;
				}
				_context.Update(recipient);
			}


			var order = await _context.Orders.SingleOrDefaultAsync(o => o.User == user && o.OrderStatus == 0);
			order.FirstName = defaultRecipient.FirstName;
			order.City = defaultRecipient.City;
			order.Country = defaultRecipient.Country;
			order.Phone = defaultRecipient.Phone;
			order.PostalCode = defaultRecipient.PostalCode;
			order.State = defaultRecipient.State;
			order.FirstName = defaultRecipient.FirstName;
			order.LastName = defaultRecipient.LastName;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!RecipientExists(defaultRecipient.RecipientId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			//SetShoppingCartRecipient(user, id);

			return Redirect(Request.Headers["Referer"].ToString());
		}

		//public async void SetShoppingCartRecipient(ApplicationUser user, int id)
		//{
		//	var defaultRecipient = _context.Recipient.AsNoTracking().SingleOrDefault(r => r.RecipientId == id);
		//	var order = await _context.Orders.SingleOrDefaultAsync(o => o.User == user && o.OrderStatus == 0);
		//	Order orderToUpdate = new Order
		//	{
		//		OrderId = order.OrderId,
		//		City = defaultRecipient.City,
		//		Country = defaultRecipient.Country,
		//		Phone = defaultRecipient.Phone,
		//		PostalCode = defaultRecipient.PostalCode,
		//		State = defaultRecipient.State,
		//		User = user,
		//		FirstName = defaultRecipient.FirstName,
		//		LastName = defaultRecipient.LastName,
		//		Total = order.Total,
		//		OrderStatus = 0
		//	};

		//	_context.Orders.Update(orderToUpdate);

		//	try
		//	{
		//		await _context.SaveChangesAsync();
		//	}
		//	catch (DbUpdateException /* ex */)
		//	{
		//		//Log the error (uncomment ex variable name and write a log.)
		//		ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
		//	}
		//}

		// GET: Recipients/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient.SingleOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // GET: Recipients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipientId,City,Country,FirstName,LastName,Phone,PostalCode,State")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
				ApplicationUser user = await _userManager.GetUserAsync(User);
				recipient.User = user;
                _context.Add(recipient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(recipient);
        }

        // GET: Recipients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient.SingleOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

		// POST: Recipients/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipientId,City,Country,FirstName,LastName,Phone,PostalCode,State")] Recipient recipient)
        {
            if (id != recipient.RecipientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientExists(recipient.RecipientId))
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
            return View(recipient);
        }

        // GET: Recipients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipient.SingleOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipient = await _context.Recipient.SingleOrDefaultAsync(m => m.RecipientId == id);
            _context.Recipient.Remove(recipient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RecipientExists(int id)
        {
            return _context.Recipient.Any(e => e.RecipientId == id);
        }
    }
}
