using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManager.DbLibrary;
using InventoryManager.Models;

namespace InventoryManager.Web.Controllers
{
    public class CategoryDetailsController : Controller
    {
        private readonly InventoryDbContext _context;

        public CategoryDetailsController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: CategoryDetails
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.CategoryDetails.Include(c => c.Category);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: CategoryDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryDetails == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            return View(categoryDetail);
        }

        // GET: CategoryDetails/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: CategoryDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ColorValue,ColorName")] CategoryDetail categoryDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name", categoryDetail.Id);
            return View(categoryDetail);
        }

        // GET: CategoryDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryDetails == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails.FindAsync(id);
            if (categoryDetail == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name", categoryDetail.Id);
            return View(categoryDetail);
        }

        // POST: CategoryDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ColorValue,ColorName")] CategoryDetail categoryDetail)
        {
            if (id != categoryDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryDetailExists(categoryDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name", categoryDetail.Id);
            return View(categoryDetail);
        }

        // GET: CategoryDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryDetails == null)
            {
                return NotFound();
            }

            var categoryDetail = await _context.CategoryDetails
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            return View(categoryDetail);
        }

        // POST: CategoryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryDetails == null)
            {
                return Problem("Entity set 'InventoryDbContext.CategoryDetails'  is null.");
            }
            var categoryDetail = await _context.CategoryDetails.FindAsync(id);
            if (categoryDetail != null)
            {
                _context.CategoryDetails.Remove(categoryDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryDetailExists(int id)
        {
          return (_context.CategoryDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
