# 💊 PharmaDesk — Pharmacy Management System

A lightweight, offline-first pharmacy management system built with **ASP.NET Core MVC** and **Entity Framework Core**. Designed for small pharmacies to manage medicines, stock batches, sales, purchases, and expiry alerts — all running locally on a single PC with no internet required.

---

## 📋 Table of Contents
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Database Design](#-database-design)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [Security](#-security)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## ✨ Features

### Core
- **Barcode-driven sales** — scan a barcode, the medicine drops into the cart, press Enter to checkout. Cashier never needs the mouse.
- **FEFO stock deduction** — First Expiry, First Out. Stock is always taken from the batch that expires soonest.
- **Batch-level inventory** — every delivery is tracked as a separate batch with its own expiry date and cost price.
- **Expiry alerts** — medicines expiring within 90 days are highlighted in orange; expired ones in red.
- **Low-stock alerts** — each medicine has a configurable alert level.
- **Purchase management** — record supplier invoices; saving a purchase automatically adds new batches to stock.
- **Daily sales reports** — money totals, counts, and per-staff breakdowns.

### Administration
- **Role-based access** — `Admin` and `Staff` roles with different menus and permissions.
- **Team management** — add staff, reset PINs, deactivate accounts.
- **Activity log** — every login, sale, purchase, price change, and backup is recorded.
- **One-click backup** — saves a copy of the database file.
- **Soft delete** — medicines and users are never truly deleted (`IsActive = false`), so old sales stay correct.

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core MVC (.NET 9) |
| ORM | Entity Framework Core |
| Database | SQL Server (LocalDB for dev) / SQLite (for deployment) |
| Frontend | Razor Views + Bootstrap 5 + Bootstrap Icons |
| Auth | Custom PIN-based authentication with `PasswordHasher` |
| Language | C# |

---

## 🏗 Architecture

The project follows the **MVC (Model-View-Controller)** pattern:

- **Models** — database entities (one class per table).
- **ViewModels** — shapes the data for the UI (separate folder, never mixed with Models).
- **Data** — `PharmaDeskContext` (EF Core) and `DbSeeder` (initial data).
- **Controllers** — handle HTTP requests and business logic.
- **Views** — Razor pages styled with Bootstrap.

---

## 🗄 Database Design

The schema is normalized and designed around **batch-level stock tracking**:

| Table | Purpose |
|-------|---------|
| `TeamMembers` | Staff accounts with hashed PINs and lockout tracking |
| `Categories` | Medicine categories (Tablets, Syrups, …) |
| `Medicines` | The product catalog (one row per product) |
| `Batches` | Actual stock (one row per delivery, with expiry & cost) |
| `Suppliers` | Supplier information |
| `Purchases` / `PurchaseItems` | Incoming stock from suppliers |
| `Sales` / `SaleItems` | Outgoing stock to customers |
| `ActivityLog` | Audit trail of every action |

### Key relationships
- `Category` → `Medicines` (block delete)
- `Medicine` → `Batches` (block delete)
- `Medicine` → `SaleItems` / `PurchaseItems` (block delete — protects historical records)
- `Supplier` → `Purchases` (block delete)
- `Sale` → `SaleItems` (cascade delete)
- `Purchase` → `PurchaseItems` (cascade delete)

### Business rules
1. A sale cannot take more than the stock in the batch.
2. A sale never uses an expired batch.
3. Batches are picked by earliest `ExpiryDate` first (FEFO).
4. Saving a sale or a purchase happens in a single database transaction.

---

🗺 Roadmap
☑ Database schema & migrations
☑ Category CRUD
☑ Medicine CRUD
□ Supplier CRUD
□ Batch & stock view
□ New Sale screen (FEFO logic)
□ Purchases
□ PIN authentication + roles
□ Dashboard & reports
□ Backup & activity log UI
□ SQLite support for offline deployment

