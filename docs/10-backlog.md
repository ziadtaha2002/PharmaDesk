# 10. Engineering Backlog & Task Distribution

This backlog breaks down the remaining work into concrete, independent tasks for **Abdulrahman** and **Ziad**.

---

## 👤 Abdulrahman's Tasks (Auth, Security, Reporting, Data)

### Task 1.1: Generate Initial EF Core Migration
- **Goal**: Run `dotnet ef migrations add InitialCreate --project PharmaDesk`.
- **Files**: `PharmaDesk/Migrations/*`, `Data/PharmaDeskContext.cs`
- **Acceptance**: Clean migration generated without warnings; `dotnet ef database update` succeeds on empty DB.

### Task 1.2: Clean Up Dead Model Files & Typos
- **Goal**: Delete 0-byte files (`Batches.cs`, `Purchases.cs`, `Suppliers.cs`, `'PurchasItem .cs'`). Rename `'CategoryController .cs'` to remove whitespace.
- **Files**: `Models/*`, `Controllers/CategoryController.cs`
- **Acceptance**: 0-byte files deleted; clean compile.

### Task 2.1: Enforce Server-Side `[Authorize]` on Admin Controllers (CRITICAL)
- **Goal**: Add `[Authorize(Roles = "Admin")]` to `CategoryController`, `SupplierController`, and `PurchaseController`. In `MedicineController`, protect `Create`, `Edit`, and `ToggleActive`.
- **Files**: `Controllers/CategoryController.cs`, `SupplierController.cs`, `PurchaseController.cs`, `MedicineController.cs`
- **Acceptance**: Staff visiting `/Purchase/Create` gets redirected to `/Account/AccessDenied` (HTTP 403).

### Task 2.2: Standardize Lockout on UTC
- **Goal**: Switch lockout comparison and timestamps in `AccountController` to `DateTime.UtcNow`.
- **Files**: `Controllers/AccountController.cs`
- **Acceptance**: Remaining lockout minutes calculate correctly.

### Task 6.1: Resolve Active Cashier in Sales & Purchases
- **Goal**: Replace hardcoded `TeamMemberId = 1` by reading `int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))`.
- **Files**: `Services/SaleService.cs`, `Controllers/PurchaseController.cs`
- **Acceptance**: Real cashier ID and name recorded on receipts and invoices.

### Task 7.1: Scope Staff Sales History to Own Sales
- **Goal**: In `SaleController.History`, filter by `s.TeamMemberId == currentUserId` if user is `Staff`. Admin sees all.
- **Files**: `Controllers/SaleController.cs`
- **Acceptance**: Staff sees only their receipts; Admin sees pharmacy totals.

### Task 7.2: Fix Medicine Name Length
- **Goal**: Update `[StringLength(100)]` on `Medicine.Name` to `150` to match ViewModel and Technical Design.
- **Files**: `Models/Medicine.cs`
- **Acceptance**: Medicine names up to 150 characters accepted.

### Task 9.1: Build Daily Sales Report Screen
- **Goal**: Implement `DailySales(DateTime? date)` in `ReportController` with total revenue, receipt count, average basket, and cashier breakdown. Update sidebar link to `/Report/DailySales`.
- **Files**: `Controllers/ReportController.cs`, `ViewModels/DailySalesReportViewModel.cs`, `Views/Report/DailySales.cshtml`
- **Acceptance**: Admin can view daily financial totals; Staff blocked.

### Task 12.1: Self-Contained Offline Publish Profile
- **Goal**: Configure self-contained .NET 9 publishing for `win-x64` to run without .NET runtime installed.
- **Files**: `PharmaDesk.csproj`
- **Acceptance**: `dotnet publish -c Release -r win-x64 --self-contained` runs offline.

---

## 👤 Ziad's Tasks (StockService, Purchases, Backup, Auditing)

### Task 6.2: Refactor Batch Creation into `StockService`
- **Goal**: Create `StockService.cs` and move batch creation and stock incrementing logic out of `PurchaseController.Create` into `StockService.AddIncomingStockAsync`.
- **Files**: `Services/IStockService.cs`, `Services/StockService.cs`, `Controllers/PurchaseController.cs`, `Program.cs`
- **Acceptance**: `StockService` registered in DI; purchases delegate batch creation to it.

### Task 10.1: Build Database Backup Service & Screen
- **Goal**: Create `BackupService.cs` to execute SQL Server `BACKUP DATABASE` to `App_Data/Backups/`. Build `BackupController` and view with a "Backup Now" button and backup history table.
- **Files**: `Services/BackupService.cs`, `Controllers/BackupController.cs`, `Views/Backup/Index.cshtml`, `Program.cs`
- **Acceptance**: Clicking "Backup Now" creates a `.bak` file on disk; logs to `ActivityLog`.

### Task 10.2: Build Activity Audit Log Screen
- **Goal**: Create `ActivityLogController` with `[Authorize(Roles = "Admin")]` and Razor view to browse chronological events with action filter.
- **Files**: `Controllers/ActivityLogController.cs`, `Views/ActivityLog/Index.cshtml`
- **Acceptance**: Admin can review system audit trail; Staff blocked.

### Task 10.3: Add Audit Log Triggers for Purchases & Price Edits
- **Goal**: Log `PriceChange` in `MedicineController.Edit`, and log `Purchase` in `PurchaseController.Create`.
- **Files**: `Controllers/MedicineController.cs`, `Controllers/PurchaseController.cs`
- **Acceptance**: Price changes and purchases show up in `ActivityLog`.

### Task 11.1: UI Polish & POS Keyboard Flow
- **Goal**: Audit buttons, badges, and ensure the POS counter can be operated completely via keyboard (Enter, F-keys).
- **Files**: `wwwroot/css/site.css`, `Views/Sale/New.cshtml`
- **Acceptance**: Fast checkout with keyboard-only flow.
