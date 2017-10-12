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

namespace QualityHat.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Member")]
    public class MemberHatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberHatsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: MemberHats
        public async Task<IActionResult> Index(int? id, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var hats = from h in _context.Hats
                   select h;
            if (!String.IsNullOrEmpty(searchString))
            {
                hats = hats.Where(h => h.Name.Contains(searchString) || h.Disc.Contains(searchString));
            } else {
                if (id == null){
                    hats = _context.Hats.Include(h => h.Category).Include(h => h.Supplier);
                } else {
                    hats = _context.Hats.Where(h => h.CategoryID == id).Include(h => h.Category).Include(h => h.Supplier);
                }
            }

            return View(await hats.ToListAsync());
        }

        // GET: MemberHats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // GET: MemberHats/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email");
            return View();
        }

        // POST: MemberHats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HatID,CategoryID,Disc,Image,Name,Price,SupplierID")] Hat hat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", hat.SupplierID);
            return View(hat);
        }

        // GET: MemberHats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", hat.SupplierID);
            return View(hat);
        }

        // POST: MemberHats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HatID,CategoryID,Disc,Image,Name,Price,SupplierID")] Hat hat)
        {
            if (id != hat.HatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HatExists(hat.HatID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", hat.SupplierID);
            return View(hat);
        }

        // GET: MemberHats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // POST: MemberHats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            _context.Hats.Remove(hat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool HatExists(int id)
        {
            return _context.Hats.Any(e => e.HatID == id);
        }

		// GET: Categories/Details/5
		public async Task<IActionResult> Category(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			// var category = await _context.Categorys.SingleOrDefaultAsync(m => m.CategoryID == id);

			var category = await _context.Categorys.Include(h => h.Hats).AsNoTracking().SingleOrDefaultAsync(m => m.CategoryID == id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}
	}
}
