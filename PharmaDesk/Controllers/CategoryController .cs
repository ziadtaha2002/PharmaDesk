using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Models;

namespace PharmaDesk.Controllers
{
    public class CategoryController : Controller
    {
        private readonly PharmaDeskContext _context;

        public CategoryController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Category  (قائمة الفئات)
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View(categories);
        }

        // GET: /Category/Create  (صفحة إضافة فئة)
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create  (حفظ الفئة الجديدة)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // التحقق من أن الاسم غير مكرر (رغم أن الفهرس الفريد يمنعه، لكن نعطي رسالة واضحة)
                bool exists = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());

                if (exists)
                {
                    ModelState.AddModelError("Name", "This category already exists.");
                    return View(category);
                }

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: /Category/Edit/5  (صفحة تعديل فئة)
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: /Category/Edit/5  (حفظ التعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                bool exists = await _context.Categories
                    .AnyAsync(c => c.Id != id && c.Name.ToLower() == category.Name.ToLower());

                if (exists)
                {
                    ModelState.AddModelError("Name", "This category already exists.");
                    return View(category);
                }

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: /Category/Delete/5  (صفحة تأكيد الحذف)
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // هل هذه الفئة تحتوي على أدوية؟ (لمنع الحذف كما في التصميم)
            bool hasMedicines = await _context.Medicines
                .AnyAsync(m => m.CategoryId == id);
            ViewBag.HasMedicines = hasMedicines;

            return View(category);
        }

        // POST: /Category/Delete/5  (تنفيذ الحذف)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // التحقق من عدم وجود أدوية مرتبطة
            bool hasMedicines = await _context.Medicines.AnyAsync(m => m.CategoryId == id);
            if (hasMedicines)
            {
                TempData["Error"] = "Cannot delete this category because it has medicines. Move the medicines first.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}