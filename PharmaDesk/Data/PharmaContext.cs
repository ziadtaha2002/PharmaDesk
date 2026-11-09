using Microsoft.EntityFrameworkCore;
using PharmaDesk.Models;

namespace PharmaDesk.Data
{
    public class PharmaDeskContext : DbContext
    {
        public PharmaDeskContext(DbContextOptions<PharmaDeskContext> options) : base(options)
        {
        }

        // الجداول (كل جدول يمثل Class في مجلد Models)
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. الفهارس (Indexes) — لتسريع البحث
            // ==========================================

            // باركود الدواء يجب أن يكون فريداً (لا يوجد دواءان بنفس الباركود)
            modelBuilder.Entity<Medicine>()
                .HasIndex(m => m.Barcode)
                .IsUnique();

            // فهرس على اسم الدواء لتسريع البحث بالاسم في شاشة البيع
            modelBuilder.Entity<Medicine>()
                .HasIndex(m => m.Name);

            // فهرس على تاريخ الانتهاء لتسريع تقرير الصلاحية
            modelBuilder.Entity<Batch>()
                .HasIndex(b => b.ExpiryDate);

            // رقم الفاتورة يجب أن يكون فريداً (مثل S-000125)
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.ReceiptNumber)
                .IsUnique();

            // فهرس على تاريخ البيع لتسريع تقرير المبيعات اليومية
            modelBuilder.Entity<Sale>()
                .HasIndex(s => s.SaleDate);

            // اسم الفئة يجب أن يكون فريداً (لا يوجد فئتان بنفس الاسم)
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();


            // ==========================================
            // 2. منع الحذف (Restrict / Block Deletion)
            // ==========================================

            // لا يمكن حذف فئة إذا كانت تحتوي على أدوية
            modelBuilder.Entity<Medicine>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Medicines)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا يمكن حذف دواء إذا كان لديه دفعات في المخزون
            modelBuilder.Entity<Batch>()
                .HasOne(b => b.Medicine)
                .WithMany(m => m.Batches)
                .HasForeignKey(b => b.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا يمكن حذف دواء إذا تم بيعه سابقاً (حماية الفواتير القديمة)
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Medicine)
                .WithMany(m => m.SaleItems)
                .HasForeignKey(si => si.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا يمكن حذف دواء إذا تم شراؤه سابقاً (حماية فواتير الشراء)
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Medicine)
                .WithMany(m => m.PurchaseItems)
                .HasForeignKey(pi => pi.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا يمكن حذف مورد إذا كانت لديه فواتير شراء
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا يمكن حذف موظف إذا قام بعمليات بيع
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.TeamMember)
                .WithMany(t => t.Sales)
                .HasForeignKey(s => s.TeamMemberId)
                .OnDelete(DeleteBehavior.Restrict);


            // ==========================================
            // 3. الحذف المتسلسل (Cascade Deletion)
            // ==========================================

            // إذا حذفت فاتورة شراء، تُحذف بنودها تلقائياً
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Purchase)
                .WithMany(p => p.PurchaseItems)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // إذا حذفت فاتورة بيع، تُحذف بنودها تلقائياً
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==========================================
            // 4. علاقة Batch مع PurchaseItem (اختيارية)
            // ==========================================
            // الدفعة قد تأتي من فاتورة شراء، وإذا حُذفت الفاتورة لا نحذف الدفعة
            // (لأن الدفعة قد تكون استُهلكت في مبيعات)
            modelBuilder.Entity<Batch>()
                .HasOne(b => b.PurchaseItem)
                .WithMany()
                .HasForeignKey(b => b.PurchaseItemId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}