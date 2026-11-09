using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Models;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Controllers
{
    public class MedicineController : Controller
    {
        private readonly PharmaDeskContext _context;

        public MedicineController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Medicine  (قائمة الأدوية)
        public async Task<IActionResult> Index(string search = "")
        {
            var query = _context.Medicines
                .Include(m => m.Category)
                .AsQueryable();

            // بحث بالاسم أو الباركود أو الاسم العلمي
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m =>
                    m.Name.Contains(search) ||
                    m.Barcode.Contains(search) ||
                    (m.GenericName != null && m.GenericName.Contains(search)));
            }

            var medicines = await query
                .OrderBy(m => m.Name)
                .ToListAsync();

            ViewBag.Search = search;
            return View(medicines);
        }

        // GET: /Medicine/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MedicineViewModel
            {
                Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync()
            };
            return View(vm);
        }

        // POST: /Medicine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicineViewModel vm)
        {
            // التحقق من أن الباركود غير مكرر
            bool barcodeExists = await _context.Medicines
                .AnyAsync(m => m.Barcode == vm.Barcode);
            if (barcodeExists)
                ModelState.AddModelError("Barcode", "This barcode is already used by another medicine.");

            if (ModelState.IsValid)
            {
                var medicine = new Medicine
                {
                    Name = vm.Name,
                    GenericName = vm.GenericName,
                    Barcode = vm.Barcode,
                    CategoryId = vm.CategoryId,
                    Unit = vm.Unit,
                    SellingPrice = vm.SellingPrice,
                    LowStockLevel = vm.LowStockLevel,
                    IsActive = true
                };

                _context.Medicines.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // إعادة تحميل القائمة المنسدلة عند وجود خطأ
            vm.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(vm);
        }

        // GET: /Medicine/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();

            var vm = new MedicineViewModel
            {
                Id = medicine.Id,
                Name = medicine.Name,
                GenericName = medicine.GenericName,
                Barcode = medicine.Barcode,
                CategoryId = medicine.CategoryId,
                Unit = medicine.Unit,
                SellingPrice = medicine.SellingPrice,
                LowStockLevel = medicine.LowStockLevel,
                IsActive = medicine.IsActive,
                Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync()
            };
            return View(vm);
        }

        // POST: /Medicine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicineViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            // التحقق من أن الباركود غير مكرر (مع استثناء الدواء نفسه)
            bool barcodeExists = await _context.Medicines
                .AnyAsync(m => m.Id != id && m.Barcode == vm.Barcode);
            if (barcodeExists)
                ModelState.AddModelError("Barcode", "This barcode is already used by another medicine.");

            if (ModelState.IsValid)
            {
                var medicine = await _context.Medicines.FindAsync(id);
                if (medicine == null) return NotFound();

                medicine.Name = vm.Name;
                medicine.GenericName = vm.GenericName;
                medicine.Barcode = vm.Barcode;
                medicine.CategoryId = vm.CategoryId;
                medicine.Unit = vm.Unit;
                medicine.SellingPrice = vm.SellingPrice;
                medicine.LowStockLevel = vm.LowStockLevel;
                // لا نغير IsActive هنا (لها زر منفصل)

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(vm);
        }

        // POST: /Medicine/ToggleActive/5  (إخفاء / إظهار الدواء)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();

            medicine.IsActive = !medicine.IsActive;
            await _context.SaveChangesAsync();

            TempData["Success"] = medicine.IsActive
                ? $"\"{medicine.Name}\" is now active."
                : $"\"{medicine.Name}\" is now hidden.";

            return RedirectToAction(nameof(Index));
        }
    }
}