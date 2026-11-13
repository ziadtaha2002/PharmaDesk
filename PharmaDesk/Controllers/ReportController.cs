using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Controllers
{
    public class ReportController : Controller
    {
        private readonly PharmaDeskContext _context;

        public ReportController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Report/Expiry (تقرير الصلاحية)
        public async Task<IActionResult> Expiry()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var ninetyDaysFromNow = today.AddDays(90);

            // جلب الدفعات التي ستنتهي قريباً أو انتهت بالفعل
            var batches = await _context.Batches
                .Include(b => b.Medicine)
                .ThenInclude(m => m!.Category)
                .Where(b => b.ExpiryDate <= ninetyDaysFromNow && b.QuantityInStock > 0)
                .OrderBy(b => b.ExpiryDate)
                .ToListAsync();

            ViewBag.Today = today;
            return View(batches);
        }

        // GET: /Report/LowStock (المخزون المنخفض)
        public async Task<IActionResult> LowStock()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var stock = await _context.Medicines
                .Where(m => m.IsActive)
                .Include(m => m.Category)
                .Select(m => new StockViewModel
                {
                    MedicineId = m.Id,
                    MedicineName = m.Name,
                    Category = m.Category != null ? m.Category.Name : "",
                    Unit = m.Unit,
                    LowStockLevel = m.LowStockLevel,
                    TotalQuantity = m.Batches
                        .Where(b => b.ExpiryDate > today)
                        .Sum(b => b.QuantityInStock)
                })
                .Where(s => s.TotalQuantity <= s.LowStockLevel) // فقط المنخفض
                .OrderBy(s => s.TotalQuantity)
                .ToListAsync();

            return View(stock);
        }
    }
}