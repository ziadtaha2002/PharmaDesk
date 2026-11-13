using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Controllers
{
    public class StockController : Controller
    {
        private readonly PharmaDeskContext _context;

        public StockController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Stock
        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            // جلب كل الأدوية النشطة، مع حساب مجموع الكميات من الدفعات غير المنتهية
            var stock = await _context.Medicines
                .Where(m => m.IsActive)
                .Include(m => m.Category)
                .Select(m => new StockViewModel
                {
                    MedicineId = m.Id,
                    MedicineName = m.Name,
                    GenericName = m.GenericName ?? "",
                    Category = m.Category != null ? m.Category.Name : "",
                    Unit = m.Unit,
                    LowStockLevel = m.LowStockLevel,
                    // حساب مجموع الكميات في الدفعات التي لم تنته صلاحيتها
                    TotalQuantity = m.Batches
                        .Where(b => b.ExpiryDate > today)
                        .Sum(b => b.QuantityInStock)
                })
                .OrderBy(s => s.TotalQuantity) // الأقل أولاً
                .ToListAsync();

            return View(stock);
        }

        // GET: /Stock/Details/5  (عرض تفاصيل الدفعات لدواء معين)
        public async Task<IActionResult> Details(int id)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var medicine = await _context.Medicines
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicine == null) return NotFound();

            // جلب الدفعات لهذا الدواء (حتى المنتهية، لنعرضها بلون أحمر)
            var batches = await _context.Batches
                .Where(b => b.MedicineId == id)
                .OrderBy(b => b.ExpiryDate)
                .ToListAsync();

            ViewBag.Medicine = medicine;
            ViewBag.Today = today;

            return View(batches);
        }
    }
}