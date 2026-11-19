# 🌿 PharmaDesk

**PharmaDesk** is a fast, offline pharmacy desktop web application built for local pharmacies. It handles point-of-sale (POS) counter dispensing, batch-level expiry tracking with FEFO, supplier invoice recording, and daily sales reporting on a single PC with SQL Server Express.

Built with **ASP.NET Core MVC (.NET 9)**, **Entity Framework Core 9**, and **Bootstrap 5**.

---

## 👥 The Team & Roles

Developed privately by **Abdulrahman** and **Ziad**:

| Developer | Primary Ownership |
|---|---|
| **Abdulrahman** | Authentication & Security, Sales History Scoping, Daily Sales Reports, Categories & Suppliers, EF Core Migrations |
| **Ziad** | Inventory & StockService (FEFO), Purchase Invoices & Batches, Database Backup Service, Activity Audit Log |

---

## 🚀 Quick Start (Running Locally)

### 1. Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server Express / LocalDB or SQL Server Developer Edition

### 2. Configure Database Connection
Check `PharmaDesk/appsettings.json` and ensure the connection string points to your local SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PharmaDeskDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Apply Migrations & Seed Database
Open terminal in the project root:
```bash
# Apply EF Core migrations to create the database schema
dotnet ef database update --project PharmaDesk
```
*(On first launch, `DbSeeder.cs` automatically creates the default Admin account and basic medicine categories).*

### 4. Run the Application
```bash
dotnet run --project PharmaDesk
```
Open your browser and navigate to `https://localhost:5001` (or `http://localhost:5000`).

---

## 🔑 Default Login Credentials

| Role | Default PIN | Permissions |
|---|---|---|
| **Admin** | `1234` | Full access: Purchases, Suppliers, Reports, Team, Backup, Audit Log |
| **Staff** | *(Created by Admin)* | Restricted: POS Sales, View Medicines, View Own Sales History |

---

## ⚡ Key Features (v1)

- **Fast POS Counter**: Cashier barcode search, live cart calculation, keyboard-friendly checkout.
- **FEFO Inventory**: Batches sold by earliest expiry date first. No overselling and no selling of expired stock.
- **Supplier Purchasing**: Inward invoices automatically generate new medicine batches with cost tracking.
- **Daily Financial Report**: Today's revenue, invoice count, average basket size, and cashier breakdown.
- **1-Click Local Backup**: Back up SQL Server database to disk with one click from the UI.
- **Forensic Activity Log**: Tracks logins, sales, purchases, and price edits.
- **100% Offline**: All Bootstrap and icon assets are stored locally in `wwwroot/lib/`—no internet required.

---

## 📚 Project Documentation

All system documentation is kept simple and practical in `docs/`:

- [01. Product Overview](file:///home/abdulrahman/projects/PharmaDesk/docs/01-product-overview.md) — Scope, user roles, and non-goals
- [02. Roles & Permissions](file:///home/abdulrahman/projects/PharmaDesk/docs/02-roles-and-permissions.md) — Permission matrix for Admin vs Staff
- [03. Architecture](file:///home/abdulrahman/projects/PharmaDesk/docs/03-architecture.md) — Clean layers and request flow
- [04. Data Model](file:///home/abdulrahman/projects/PharmaDesk/docs/04-data-model.md) — Complete 10-table ERD and column dictionary
- [05. Business Rules](file:///home/abdulrahman/projects/PharmaDesk/docs/05-business-rules.md) — FEFO rules, expiry colors, and receipt format
- [06. Screen Directory](file:///home/abdulrahman/projects/PharmaDesk/docs/06-screens.md) — List of all 22 screens and access rules
- [07. Security](file:///home/abdulrahman/projects/PharmaDesk/docs/07-security.md) — PIN hashing, 5-try lockout, and server-side RBAC
- [08. Development Workflow](file:///home/abdulrahman/projects/PharmaDesk/docs/08-development-workflow.md) — Git workflow and how we build together
- [09. Roadmap](file:///home/abdulrahman/projects/PharmaDesk/docs/09-roadmap.md) — 12-step progress tracker
- [10. Backlog & Tasks](file:///home/abdulrahman/projects/PharmaDesk/docs/10-backlog.md) — Remaining task assignments for Abdulrahman & Ziad
- [Design vs. Code Gaps](file:///home/abdulrahman/projects/PharmaDesk/docs/design-vs-code.md) — 10 bugs and missing pieces in the current code
- [Architecture Decisions (ADRs)](file:///home/abdulrahman/projects/PharmaDesk/docs/adr/) — Short, crisp records of why we built it this way
