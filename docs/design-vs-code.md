# 🔍 Code Gaps: What Needs Fixing in the Code

This document lists the **10 specific differences and bugs** discovered between the current C# code and the `Technical Design.pdf` source of truth. Each item is plain, direct, and assigned.

---

### 🚨 GAP 1 (Critical Security): Admin Controllers Lack `[Authorize]`
- **Problem**: `CategoryController`, `SupplierController`, `PurchaseController`, and `MedicineController` have no `[Authorize(Roles = "Admin")]`. Any Staff user can bypass the menu by typing `/Purchase/Create` or `/Supplier/Create` directly into their browser.
- **Affected Files**: `Controllers/CategoryController.cs`, `SupplierController.cs`, `PurchaseController.cs`, `MedicineController.cs`
- **Fix**: Add `[Authorize(Roles = "Admin")]` to `Category`, `Supplier`, and `Purchase` controllers. In `MedicineController`, add it to `Create`, `Edit`, and `ToggleActive` (leave `Index` viewable by Staff).
- **Assigned To**: Abdulrahman

---

### 🐛 GAP 2: Hardcoded Cashier ID (`TeamMemberId = 1`)
- **Problem**: `SaleService.ProcessSaleAsync` and `PurchaseController.Create` hardcode `TeamMemberId = 1`. Invoices and receipts do not show the real logged-in cashier.
- **Affected Files**: `Services/SaleService.cs`, `Controllers/PurchaseController.cs`
- **Fix**: Read the real user ID from `HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)` instead of `1`.
- **Assigned To**: Abdulrahman

---

### 👁️ GAP 3: Staff Sees All Store Sales in History
- **Problem**: `SaleController.History` queries all sales in the pharmacy without checking role. Cashiers can see other cashiers' revenue numbers.
- **Affected Files**: `Controllers/SaleController.cs`
- **Fix**: If user is `Staff`, filter query by `s.TeamMemberId == currentUserId`. Admin sees everything.
- **Assigned To**: Abdulrahman

---

### 📄 GAP 4: Missing Daily Sales Report Screen
- **Problem**: The menu links to `/Report/Index`, but `ReportController` doesn't have a Daily Sales action, and there is no view.
- **Affected Files**: `Controllers/ReportController.cs`, `Views/Report/DailySales.cshtml`
- **Fix**: Implement `DailySales(DateTime? date)` action showing gross revenue, receipt count, average basket, and cashier breakdown. Update menu link to `/Report/DailySales`.
- **Assigned To**: Abdulrahman

---

### 🚫 GAP 5: Missing Backup & Activity Log Controllers
- **Problem**: The sidebar links to `/Backup/Index` and `/ActivityLog/Index`, but neither controller or view exists (results in 404).
- **Affected Files**: `Controllers/BackupController.cs`, `Controllers/ActivityLogController.cs`, `Views/Backup/Index.cshtml`, `Views/ActivityLog/Index.cshtml`
- **Fix**: Create `BackupController` (one-click SQL Server backup) and `ActivityLogController` (audit history viewer) with `[Authorize(Roles = "Admin")]`.
- **Assigned To**: Ziad

---

### 🏗️ GAP 6: Missing `StockService` (Logic in Controller)
- **Problem**: `PurchaseController.Create` creates batches and calculates stock inside the controller action rather than using a dedicated service.
- **Affected Files**: `Services/StockService.cs`, `Controllers/PurchaseController.cs`, `Program.cs`
- **Fix**: Create `StockService.cs` and move batch creation and incoming stock logic into `StockService.AddIncomingStockAsync`.
- **Assigned To**: Ziad

---

### 📝 GAP 7: Missing Audit Log Entries on Price Change & Purchases
- **Problem**: Currently, only sales and logins write to `ActivityLog`. Modifying a medicine's selling price or recording a supplier purchase creates no audit record.
- **Affected Files**: `Controllers/MedicineController.cs`, `Controllers/PurchaseController.cs`
- **Fix**: Add `ActivityLog` entry on `Medicine.Edit` when `SellingPrice` changes (`Action = "PriceChange"`), and on `Purchase.Create` (`Action = "Purchase"`).
- **Assigned To**: Ziad

---

### 🗄️ GAP 8: Missing Initial EF Core Migration
- **Problem**: The database context and models are ready, but the `PharmaDesk/Migrations/` folder has not been generated yet.
- **Affected Files**: `PharmaDesk/Migrations/*`
- **Fix**: Run `dotnet ef migrations add InitialCreate --project PharmaDesk` and verify `DbSeeder.cs` runs cleanly.
- **Assigned To**: Abdulrahman

---

### 🧹 GAP 9: Dead Model Files & Controller Typo
- **Problem**:
  - Four 0-byte duplicate model files exist: `Batches.cs`, `Purchases.cs`, `Suppliers.cs`, and `'PurchasItem .cs'`.
  - `'CategoryController .cs'` has an accidental space before `.cs`.
  - A backup file `PharmaDesk.csproj.Backup.tmp` is left in the root.
- **Fix**: Delete the 0-byte files, rename `'CategoryController .cs'` to `CategoryController.cs`, and remove `.tmp`.
- **Assigned To**: Abdulrahman

---

### ⏰ GAP 10: Mixed `DateTime.Now` vs `DateTime.UtcNow`
- **Problem**: Login lockout uses `DateTime.Now`, but audit logs use `DateTime.UtcNow`. This causes lockout calculation bugs across daylight savings time.
- **Affected Files**: `Controllers/AccountController.cs`
- **Fix**: Standardize on `DateTime.UtcNow` for all database timestamps and lockout calculations.
- **Assigned To**: Abdulrahman
