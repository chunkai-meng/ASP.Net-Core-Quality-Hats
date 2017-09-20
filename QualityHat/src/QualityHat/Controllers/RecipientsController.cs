using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;

namespace QualityHat.Controllers
{
    public class RecipientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipientsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Recipients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipient.ToListAsync());
        }

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
