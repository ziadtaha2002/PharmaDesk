using Microsoft.AspNetCore.Identity;
using PharmaDesk.Models;

namespace PharmaDesk.Data
{
    public static class DbSeeder
    {
        public static void Seed(PharmaDeskContext context)
        {
            // ==========================================
            // 1. إضافة أول Admin إذا لم يكن موجوداً
            // ==========================================
            if (!context.TeamMembers.Any())
            {
                var admin = new TeamMember
                {
                    FullName = "Admin",
                    Role = "Admin",
                    IsActive = true,
                    FailedPinCount = 0,
                    CreatedAt = DateTime.Now
                };

                // تشفير الـ PIN باستخدام PasswordHasher (لن نخزنه كنص عادي أبداً)
                var hasher = new PasswordHasher<TeamMember>();
                admin.PinHash = hasher.HashPassword(admin, "1234"); // PIN الافتراضي = 1234

                context.TeamMembers.Add(admin);
                context.SaveChanges();
            }

            // ==========================================
            // 2. إضافة الفئات الأساسية إذا لم تكن موجودة
            // ==========================================
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Tablets" },      // أقراص
                    new Category { Name = "Syrups" },       // شراب
                    new Category { Name = "Creams" },       // كريمات
                    new Category { Name = "Injections" },   // حقن
                    new Category { Name = "Drops" },        // قطرة
                    new Category { Name = "Capsules" }      // كبسولات
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}