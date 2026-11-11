using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Models;

namespace PharmaDesk.Controllers
{
    public class SupplierController : Controller
    {
        private readonly PharmaDeskContext _context;

        public SupplierController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Supplier
        public async Task<IActionResult> Index(string search = "")
        {
            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    (s.Phone != null && s.Phone.Contains(search)));
            }

            var suppliers = await query
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.Search = search;
            return View(suppliers);
        }

        // GET: /Supplier/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: /Supplier/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        // POST: /Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: /Supplier/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            // هل لديه فواتير شراء؟ (لمنع الحذف)
            bool hasPurchases = await _context.Purchases
                .AnyAsync(p => p.SupplierId == id);
            ViewBag.HasPurchases = hasPurchases;

            return View(supplier);
        }

        // POST: /Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            bool hasPurchases = await _context.Purchases.AnyAsync(p => p.SupplierId == id);
            if (hasPurchases)
            {
                // لا نحذفه، بل نخفيه (Soft Delete)
                supplier.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"\"{supplier.Name}\" has purchases, so it was hidden instead of deleted.";
                return RedirectToAction(nameof(Index));
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"\"{supplier.Name}\" deleted.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Supplier/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return NotFound();

            supplier.IsActive = !supplier.IsActive;
            await _context.SaveChangesAsync();

            TempData["Success"] = supplier.IsActive
                ? $"\"{supplier.Name}\" is now active."
                : $"\"{supplier.Name}\" is now hidden.";

            return RedirectToAction(nameof(Index));
        }
    }
}