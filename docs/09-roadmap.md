# 09. Project Roadmap (12 Steps)

This roadmap outlines the complete build order for PharmaDesk from initial database setup to final offline deployment.

---

## 🗺️ Build Steps & Current Status

| Step | Milestone Name | Status | Summary |
|:---:|---|:---:|---|
| **01** | **Foundation & Database** | 🟡 **In Progress** | 10 models & DbContext created. Migrations & seeder test remaining. |
| **02** | **Authentication & Lockout** | 🟡 **In Progress** | PIN login & lockout implemented. Server-side `[Authorize]` pending. |
| **03** | **Layout & Navigation** | 🟢 **Done** | Local Bootstrap shell and role-based sidebar complete. |
| **04** | **Categories & Medicines** | 🟢 **Done** | Master catalog, barcode check, and soft-delete complete. |
| **05** | **Suppliers Directory** | 🟢 **Done** | Supplier management & contact records complete. |
| **06** | **Purchases & Inward Stock** | 🟡 **In Progress** | Invoice entry exists; needs `StockService` refactoring & cashier claims. |
| **07** | **POS Sales & Receipts** | 🟡 **In Progress** | FEFO sale logic done; needs cashier claims & Staff history scoping. |
| **08** | **Inventory & Expiry Badges** | 🟢 **Done** | Current quantity calculation & color-coded expiry badges active. |
| **09** | **Daily Sales Report** | 🔴 **Not Started** | Missing `DailySales` action, view model, and Razor view. |
| **10** | **Backup & Activity Log** | 🔴 **Not Started** | Missing `BackupController`, `ActivityLogController`, and views. |
| **11** | **UI Polish & Ergonomics** | 🟡 **In Progress** | Green styling applied; keyboard shortcuts and counter testing pending. |
| **12** | **Offline Standalone Publish** | 🔴 **Not Started** | Self-contained .NET 9 publish profile for Windows. |
