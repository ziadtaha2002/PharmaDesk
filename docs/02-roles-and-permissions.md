# 02. Roles & Permissions

PharmaDesk has **only two roles**: `Admin` and `Staff`. 
*(Note: Legacy references to "Sections" or "Management" were permanently removed).*

---

## 📋 Permission Matrix

| Feature / Screen | Route | Admin | Staff | Notes |
|---|---|:---:|:---:|---|
| **Login / Logout** | `/Account/Login` | ✅ | ✅ | 4-digit PIN authentication |
| **POS Sales Counter** | `/Sale/New` | ✅ | ✅ | Barcode search, cart, checkout |
| **Sales History** | `/Sale/History` | ✅ *(All)* | ✅ *(Own)* | Staff sees only their own sales |
| **Receipt Details** | `/Sale/Details/{id}` | ✅ | ✅ | View receipt |
| **Medicine Catalog** | `/Medicine/Index` | ✅ | ✅ *(Read)* | Staff cannot Add or Edit |
| **Add / Edit Medicine** | `/Medicine/Create`, `/Edit` | ✅ | ❌ | Admin only |
| **Categories** | `/Category/*` | ✅ | ❌ | Admin only |
| **Suppliers** | `/Supplier/*` | ✅ | ❌ | Admin only |
| **Purchasing (Inward)** | `/Purchase/*` | ✅ | ❌ | Admin only |
| **Daily Sales Report** | `/Report/DailySales` | ✅ | ❌ | Admin only |
| **Team Management** | `/TeamMember/*` | ✅ | ❌ | Admin only |
| **Database Backup** | `/Backup/*` | ✅ | ❌ | Admin only |
| **Activity Audit Log** | `/ActivityLog/*` | ✅ | ❌ | Admin only |

---

## 🔒 How to Enforce in Code

Always enforce permissions on the server using ASP.NET Core authorization attributes:

```csharp
// 1. Controller-level lock (Admin only)
[Authorize(Roles = "Admin")]
public class PurchaseController : Controller { ... }

// 2. Action-level lock (Allow Staff to view Index, but Admin to edit)
public class MedicineController : Controller 
{
    [Authorize] // Any logged-in user can view
    public async Task<IActionResult> Index() { ... }

    [Authorize(Roles = "Admin")] // Admin only
    public IActionResult Create() { ... }
}
```

In Razor views (`_Layout.cshtml`), hide links from Staff:
```razor
@if (User.IsInRole("Admin"))
{
    <li class="nav-item">
        <a class="nav-link" asp-controller="Purchase" asp-action="Index">Purchases</a>
    </li>
}
```
