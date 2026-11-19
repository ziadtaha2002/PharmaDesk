# 01. Product Overview

**PharmaDesk** is a standalone, offline pharmacy management and Point of Sale (POS) system designed to run on a single PC at a local community pharmacy.

---

## 🎯 Target Users

1. **Pharmacy Manager / Owner (Admin)**:
   - Sets up medicine catalog and categories.
   - Manages suppliers and records inward purchase invoices.
   - Reviews daily financial revenue and sales totals.
   - Creates staff PINs and handles backups and activity logs.

2. **Cashier / Pharmacist (Staff)**:
   - Operates the POS sales counter (searches by barcode or name).
   - Dispenses medicines with automatic FEFO batch allocation.
   - Views own sales history.
   - Cannot see purchase costs, supplier info, or store-wide revenue.

---

## ✅ In Scope (Version 1)

- **Offline-First**: Runs locally on Windows with SQL Server Express. Zero CDN dependencies.
- **PIN-Based Authentication**: Quick 4-digit PIN login with automatic lockout after 5 failed attempts.
- **Master Data**: Medicines, Categories, Suppliers, and Team Members.
- **Supplier Purchasing**: Inward invoices with cost tracking that automatically generate new stock batches.
- **FEFO Inventory**: Batches sold in order of earliest expiration date first. Never oversells and never sells expired stock.
- **POS Checkout**: Barcode scanning, live totals, cash payments, and receipt generation.
- **Daily Sales Report**: Today's revenue, invoice count, and cashier breakdown.
- **Backup & Auditing**: One-click local database backup and append-only activity log.

---

## ❌ Out of Scope for v1 (Future Roadmap)

- Online ordering or customer-facing mobile apps.
- Home delivery tracking.
- Physical receipt printer ESC/POS driver integration (uses standard browser print).
- Customer credit / debt accounts.
- Arabic localized UI (deferred to v2).
