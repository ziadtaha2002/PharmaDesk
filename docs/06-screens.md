# 06. Screen Directory & UI Guidelines

All screens in PharmaDesk are designed for high-speed counter operation with a clear, calm green theme.

---

## 🖥️ Screen Directory

| Screen | Route | Who Can Access | Description |
|---|---|:---:|---|
| **Login** | `/Account/Login` | Anonymous | 4-digit PIN pad with failed attempt lockout |
| **Access Denied** | `/Account/AccessDenied` | Authenticated | 403 screen for unauthorized Staff attempts |
| **POS Counter** | `/Sale/New` | Admin + Staff | Live barcode search, cart table, cash total |
| **Sales History** | `/Sale/History` | Admin + Staff | Admin sees all; Staff sees own transactions |
| **Receipt View** | `/Sale/Details/{id}` | Admin + Staff | Clean printable customer receipt |
| **Medicines List** | `/Medicine/Index` | Admin + Staff | Searchable inventory with stock & expiry badges |
| **Add / Edit Medicine** | `/Medicine/Create`, `/Edit` | Admin only | Barcode, name, generic name, category, price |
| **Categories** | `/Category/Index`, `/Create` | Admin only | Category CRUD with unique name checks |
| **Suppliers** | `/Supplier/Index`, `/Create` | Admin only | Vendor contact details and purchase history |
| **New Purchase** | `/Purchase/Create` | Admin only | Inward invoice with batch numbers & expiry dates |
| **Purchase History** | `/Purchase/Index` | Admin only | Past supplier invoices and line totals |
| **Daily Sales Report** | `/Report/DailySales` | Admin only | Today's revenue, invoice count, cashier breakdown |
| **Team Members** | `/TeamMember/Index`, `/Create` | Admin only | Add staff, set 4-digit PINs, toggle active |
| **Database Backup** | `/Backup/Index` | Admin only | 1-click SQL Server `.bak` generator to disk |
| **Activity Log** | `/ActivityLog/Index` | Admin only | Chronological audit trail with action filters |

---

## 🎨 UI Style Rules

- **Primary Color**: Calm Pharmacy Green (`#2e7d32`).
- **POS Totals**: Displayed in 28–32px bold font so cashiers can read totals clearly from an angle.
- **Keyboard Shortcuts**: POS supports full keyboard flow (Enter to add to cart, F9 or Enter to pay).
- **100% Local**: Bootstrap 5, Bootstrap Icons, and jQuery are bundled inside `wwwroot/lib/`. No internet access required.
