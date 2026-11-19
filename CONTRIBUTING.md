# 🤝 How We Work Together (Abdulrahman & Ziad)

This is a private repository for **Abdulrahman** and **Ziad**. Keep development simple, friendly, and conflict-free.

---

## 🎯 Ground Rules

1. **Never break `main`**: Always verify `dotnet build` passes with 0 errors before merging.
2. **Branch before coding**: Never push directly to `main`. Create a short branch:
   - `feature/name` for new features (e.g. `feature/daily-sales`)
   - `fix/name` for bug fixes (e.g. `fix/hardcoded-cashier-id`)
3. **Keep PRs small**: Tackle one task or bug at a time so your friend can review it in 2 minutes.
4. **Clean commit messages**: Use simple prefixes: `feat:`, `fix:`, `docs:`, `chore:`.

---

## 🗂️ Task & File Ownership Split

To make sure we never edit the same files at the same time:

| Domain | Owner | Key Controllers & Files |
|---|---|---|
| **Security & Auth** | **Abdulrahman** | `AccountController`, PIN lockout, role checks |
| **Sales History & Reporting** | **Abdulrahman** | `SaleController.History`, `ReportController`, `DailySales.cshtml` |
| **Master Catalog & DB** | **Abdulrahman** | `CategoryController`, `SupplierController`, EF Core migrations |
| **Inventory & FEFO** | **Ziad** | `StockService.cs`, `Batches`, Expiry alerts |
| **Purchasing & Invoices** | **Ziad** | `PurchaseController.cs`, `Views/Purchase/*` |
| **Backups & Auditing** | **Ziad** | `BackupService.cs`, `BackupController.cs`, `ActivityLogController.cs` |

---

## 🛠️ Step-by-Step Feature Workflow

1. **Pick an issue** from the [Roadmap / Backlog](file:///home/abdulrahman/projects/PharmaDesk/docs/10-backlog.md).
2. **Create a branch**:
   ```bash
   git checkout main
   git pull origin main
   git checkout -b feature/your-task-name
   ```
3. **Implement the code**:
   - Keep business logic in services (`Services/`), not inside controllers.
   - Guard all Admin actions with `[Authorize(Roles = "Admin")]`.
4. **Test the build**:
   ```bash
   dotnet build PharmaDesk/PharmaDesk.csproj
   ```
5. **Commit and push**:
   ```bash
   git add .
   git commit -m "feat: implement daily sales summary screen"
   git push -u origin feature/your-task-name
   ```
6. **Open a PR**: Send the link to your friend for a quick thumbs-up and merge!
