using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Models;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly PharmaDeskContext _context;

        public PurchaseController(PharmaDeskContext context)
        {
            _context = context;
        }

        // GET: /Purchase  (قائمة فواتير الشراء)
        public async Task<IActionResult> Index()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            return View(purchases);
        }

        // GET: /Purchase/Create  (فورم الشراء)
        public async Task<IActionResult> Create()
        {
            var vm = new PurchaseViewModel
            {
                Suppliers = await _context.Suppliers
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync(),
                Medicines = await _context.Medicines
                    .Where(m => m.IsActive)
                    .OrderBy(m => m.Name)
                    .ToListAsync(),
                // نبدأ بصف واحد فارغ
                Items = new List<PurchaseItemViewModel> { new PurchaseItemViewModel() }
            };
            return View(vm);
        }

        // POST: /Purchase/Create  (حفظ الفاتورة)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseViewModel vm)
        {
            // التحقق: يجب أن يكون هناك بند واحد على الأقل
            if (vm.Items == null || vm.Items.Count == 0)
            {
                ModelState.AddModelError("", "Please add at least one item to the purchase.");
            }
            else
            {
                // التحقق من كل بند
                for (int i = 0; i < vm.Items.Count; i++)
                {
                    var item = vm.Items[i];

                    // هل الدواء موجود؟
                    bool medicineExists = await _context.Medicines.AnyAsync(m => m.Id == item.MedicineId);
                    if (!medicineExists)
                    {
                        ModelState.AddModelError($"Items[{i}].MedicineId", "Invalid medicine.");
                    }

                    // هل تاريخ الانتهاء في المستقبل؟
                    if (item.ExpiryDate <= DateOnly.FromDateTime(DateTime.Today))
                    {
                        ModelState.AddModelError($"Items[{i}].ExpiryDate", "Expiry date must be in the future.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                // إعادة تحميل القوائم
                vm.Suppliers = await _context.Suppliers.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                vm.Medicines = await _context.Medicines.Where(m => m.IsActive).OrderBy(m => m.Name).ToListAsync();
                return View(vm);
            }

            // ==========================================
            // حفظ الفاتورة (في Transaction واحد)
            // ==========================================
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. حساب الإجمالي
                decimal totalCost = vm.Items.Sum(i => i.Quantity * i.CostPrice);

                // 2. إنشاء فاتورة الشراء
                var purchase = new Purchase
                {
                    SupplierId = vm.SupplierId,
                    InvoiceNumber = vm.InvoiceNumber,
                    PurchaseDate = DateTime.Now,
                    TotalCost = totalCost,
                    TeamMemberId = 1 // مؤقتاً (لاحقاً: المستخدم الحالي)
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync(); // لتحصل على purchase.Id

                // 3. لكل بند: إنشاء PurchaseItem + Batch
                foreach (var item in vm.Items)
                {
                    var purchaseItem = new PurchaseItem
                    {
                        PurchaseId = purchase.Id,
                        MedicineId = item.MedicineId,
                        BatchNumber = item.BatchNumber,
                        ExpiryDate = item.ExpiryDate,
                        Quantity = item.Quantity,
                        CostPrice = item.CostPrice
                    };
                    _context.PurchaseItems.Add(purchaseItem);
                    await _context.SaveChangesAsync(); // لتحصل على purchaseItem.Id

                    // 4. إنشاء Batch (هنا يدخل المخزون فعلياً)
                    var batch = new Batch
                    {
                        MedicineId = item.MedicineId,
                        BatchNumber = item.BatchNumber,
                        ExpiryDate = item.ExpiryDate,
                        QuantityInStock = item.Quantity,
                        CostPrice = item.CostPrice,
                        PurchaseItemId = purchaseItem.Id
                    };
                    _context.Batches.Add(batch);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"Purchase saved successfully. Total: ${totalCost:0.00}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);

                vm.Suppliers = await _context.Suppliers.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                vm.Medicines = await _context.Medicines.Where(m => m.IsActive).OrderBy(m => m.Name).ToListAsync();
                return View(vm);
            }
        }
    }
}