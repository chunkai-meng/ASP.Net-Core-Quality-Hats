using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QualityHat.Controllers
{
	[Authorize(Roles = "Admin")]
    public class AdminApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;    
        }

		// GET: AdminApplicationUsers
		public IActionResult Index()
		{
			// return View(await _context.ApplicationUser.ToListAsync());
			IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
			return View(members);
		}

		// Week7
		private async Task<IEnumerable<ApplicationUser>> ReturnAllMembers()
        {
            IdentityRole role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "Member");
            IEnumerable<ApplicationUser> users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            return users;
        }

		// GET: AdminApplicationUsers/Details/5
		public async Task<IActionResult> Details(string id)
		{
		    if (id == null)
		    {
		        return NotFound();
		    }

		    var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
		    if (applicationUser == null)
		    {
		        return NotFound();
		    }

		    return View(applicationUser);
		}

		// GET: AdminApplicationUsers/Create
		public IActionResult Create()
		{
		    return View();
		}

		// POST: AdminApplicationUsers/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,Address,ConcurrencyStamp,CustomerName,Email,EmailConfirmed,Enabled,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] ApplicationUser applicationUser)
		{
		    if (ModelState.IsValid)
		    {
		        _context.Add(applicationUser);
		        await _context.SaveChangesAsync();
		        return RedirectToAction("Index");
		    }
		    return View(applicationUser);
		}

		// GET: AdminApplicationUsers/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
		    if (id == null)
		    {
		        return NotFound();
		    }

		    var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
		    if (applicationUser == null)
		    {
		        return NotFound();
		    }
		    return View(applicationUser);
		}

		// POST: AdminApplicationUsers/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,Address,ConcurrencyStamp,CustomerName,Email,EmailConfirmed,Enabled,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] ApplicationUser applicationUser)
		{
		    if (id != applicationUser.Id)
		    {
		        return NotFound();
		    }

		    if (ModelState.IsValid)
		    {
		        try
		        {
		            _context.Update(applicationUser);
		            await _context.SaveChangesAsync();
		        }
		        catch (DbUpdateConcurrencyException)
		        {
		            if (!ApplicationUserExists(applicationUser.Id))
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
		    return View(applicationUser);
		}

		// GET: AdminApplicationUsers/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
		    if (id == null)
		    {
		        return NotFound();
		    }

		    var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
		    if (applicationUser == null)
		    {
		        return NotFound();
		    }

		    return View(applicationUser);
		}

		// POST: AdminApplicationUsers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
			_context.ApplicationUser.Remove(applicationUser);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		private bool ApplicationUserExists(string id)
		{
			return _context.ApplicationUser.Any(e => e.Id == id);
		}
	}
}