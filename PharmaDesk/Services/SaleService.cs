using Microsoft.EntityFrameworkCore;
using PharmaDesk.Data;
using PharmaDesk.Models;
using PharmaDesk.ViewModels;

namespace PharmaDesk.Services
{
    public class SaleService
    {
        private readonly PharmaDeskContext _context;

        public SaleService(PharmaDeskContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, int SaleId)> ProcessSaleAsync(SaleViewModel vm)
        {
            // استخدام Transaction لضمان حفظ كل شيء أو لا شيء
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. إنشاء فاتورة البيع الأساسية
                var sale = new Sale
                {
                    ReceiptNumber = await GenerateReceiptNumberAsync(),
                    SaleDate = DateTime.Now,
                    PaymentMethod = vm.PaymentMethod,
                    TeamMemberId = 1, // مؤقتاً (لاحقاً: المستخدم الحالي)
                    TotalAmount = 0 // سيتم حسابه لاحقاً
                };
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                decimal totalAmount = 0;
                var today = DateOnly.FromDateTime(DateTime.Today);

                // 2. المرور على كل دواء في السلة
                foreach (var item in vm.Items)
                {
                    var medicine = await _context.Medicines.FindAsync(item.MedicineId);
                    if (medicine == null) throw new Exception($"Medicine ID {item.MedicineId} not found.");

                    // جلب كل الدفعات الصالحة (غير منتهية) لهذا الدواء، مرتبة حسب الأقرب انتهاءً (FEFO)
                    var validBatches = await _context.Batches
                        .Where(b => b.MedicineId == item.MedicineId && b.ExpiryDate > today && b.QuantityInStock > 0)
                        .OrderBy(b => b.ExpiryDate)
                        .ToListAsync();

                    int quantityNeeded = item.Quantity;

                    // التحقق من توفر الكمية الإجمالية في كل الدفعات
                    int totalAvailable = validBatches.Sum(b => b.QuantityInStock);
                    if (totalAvailable < quantityNeeded)
                    {
                        throw new Exception($"Not enough stock for {medicine.Name}. Available: {totalAvailable}, Requested: {quantityNeeded}.");
                    }

                    // 3. الخصم من الدفعات (FEFO)
                    foreach (var batch in validBatches)
                    {
                        if (quantityNeeded == 0) break;

                        int takenFromBatch = Math.Min(batch.QuantityInStock, quantityNeeded);

                        // إنقاص الكمية من الدفعة
                        batch.QuantityInStock -= takenFromBatch;
                        quantityNeeded -= takenFromBatch;

                        // إنشاء بند في الفاتورة مربوط بهذه الدفعة
                        var saleItem = new SaleItem
                        {
                            SaleId = sale.Id,
                            MedicineId = medicine.Id,
                            BatchId = batch.Id,
                            Quantity = takenFromBatch,
                            UnitPrice = medicine.SellingPrice // نأخذ السعر الحالي
                        };
                        _context.SaleItems.Add(saleItem);

                        // إضافة إلى إجمالي الفاتورة
                        totalAmount += (takenFromBatch * medicine.SellingPrice);
                    }
                }

                // 4. تحديث إجمالي الفاتورة
                sale.TotalAmount = totalAmount;

                // 5. تسجيل النشاط في ActivityLog
                var log = new ActivityLog
                {
                    TeamMemberId = sale.TeamMemberId,
                    Action = "Sale",
                    Details = $"Receipt {sale.ReceiptNumber}, Total: ${sale.TotalAmount:0.00}",
                    CreatedAt = DateTime.Now
                };
                _context.ActivityLogs.Add(log);

                // حفظ كل التغييرات (الدفعات + الفاتورة + السجل) معاً
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Sale completed successfully.", sale.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex.Message, 0);
            }
        }

        // دالة مساعدة لتوليد رقم إيصال فريد (مثل S-000001)
        private async Task<string> GenerateReceiptNumberAsync()
        {
            var lastSale = await _context.Sales
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            int nextNumber = (lastSale?.Id ?? 0) + 1;
            return $"S-{nextNumber:D6}"; // مثال: S-000001
        }
    }
}