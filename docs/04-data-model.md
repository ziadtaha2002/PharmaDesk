# 04. Data Model & Database Dictionary

The database consists of **10 tables** in SQL Server. Stock balance is tracked strictly at the **Batch** level.

---

## 📊 Entity-Relationship Diagram

```mermaid
erDiagram
    Categories ||--o{ Medicines : "classifies"
    Suppliers ||--o{ Purchases : "supplies"
    Purchases ||--o{ PurchaseItems : "contains"
    Medicines ||--o{ PurchaseItems : "ordered_in"
    PurchaseItems ||--o{ Batches : "creates"
    Medicines ||--o{ Batches : "stocks"
    Batches ||--o{ SaleItems : "dispensed_from"
    Sales ||--o{ SaleItems : "contains"
    TeamMembers ||--o{ Sales : "processes"
    TeamMembers ||--o{ Purchases : "enters"
    TeamMembers ||--o{ ActivityLogs : "performs"

    Categories {
        int Id PK
        string Name
        bool IsActive
    }
    Medicines {
        int Id PK
        string Name
        string GenericName
        string Barcode
        int CategoryId FK
        string Unit
        decimal SellingPrice
        int MinimumStockAlert
        bool IsActive
    }
    Suppliers {
        int Id PK
        string Name
        string Phone
        string Address
        bool IsActive
    }
    Purchases {
        int Id PK
        int SupplierId FK
        int TeamMemberId FK
        string InvoiceNumber
        datetime PurchaseDate
        decimal TotalAmount
        datetime CreatedAt
    }
    PurchaseItems {
        int Id PK
        int PurchaseId FK
        int MedicineId FK
        int Quantity
        decimal CostPrice
    }
    Batches {
        int Id PK
        int MedicineId FK
        int PurchaseItemId FK
        string BatchNumber
        datetime ExpiryDate
        int InitialQuantity
        int CurrentQuantity
        datetime CreatedAt
    }
    Sales {
        int Id PK
        int TeamMemberId FK
        string ReceiptNumber
        datetime SaleDate
        decimal TotalAmount
        string PaymentMethod
        datetime CreatedAt
    }
    SaleItems {
        int Id PK
        int SaleId FK
        int BatchId FK
        int Quantity
        decimal UnitPrice
        decimal Subtotal
    }
    TeamMembers {
        int Id PK
        string FullName
        string PinHash
        string Role
        int FailedPinCount
        datetime LockedUntil
        bool IsActive
        datetime CreatedAt
    }
    ActivityLogs {
        int Id PK
        int TeamMemberId FK
        string Action
        string Details
        datetime CreatedAt
    }
```

---

## 🗃️ Table Breakdown

### 1. `Medicines` & `Categories`
- **Medicines**: Tracks catalog items. `Barcode` is unique. `MinimumStockAlert` triggers warning badges. Soft-deleted via `IsActive = false`.
- **Categories**: Classifies medicines. Unique name index.

### 2. `Suppliers`, `Purchases`, & `PurchaseItems`
- **Suppliers**: Vendors and contact information. Soft-deleted via `IsActive = false`.
- **Purchases**: Header for supplier invoices. Stores total amount and invoice number.
- **PurchaseItems**: Line items in an invoice. `CostPrice` is the cost per single unit.

### 3. `Batches` (The Core Inventory Ledger)
- Every physical box/bottle belongs to a `Batch`.
- `ExpiryDate`: Used for FEFO sorting.
- `CurrentQuantity`: Decrements on sale. Cannot drop below 0.
- **Total Stock Calculation**:
  $$\text{Total Available Stock} = \sum (\text{CurrentQuantity of non-expired batches for this medicine})$$

### 4. `Sales` & `SaleItems`
- **Sales**: Checkout transaction. `ReceiptNumber` formatted as `REC-YYYYMMDD-####`.
- **SaleItems**: Line items in a sale. Always references the specific `BatchId` it was deducted from. `UnitPrice` snapshots the selling price at checkout time.

### 5. `TeamMembers` & `ActivityLogs`
- **TeamMembers**: Users with 4-digit PINs. `Role` is either `Admin` or `Staff`. Tracks failed PIN attempts and lockout time.
- **ActivityLogs**: Append-only log recording logins, sales, purchases, price changes, and backups.

---

## 🔒 Foreign Key Delete Rules

- **Restrict**: Applied to master tables (`Medicines`, `Categories`, `Suppliers`, `TeamMembers`, `Batches`). Cannot delete a category if medicines exist; cannot delete a medicine if batches or sales exist.
- **Cascade**: Applied only to line items (`PurchaseItems` when a `Purchase` is removed, `SaleItems` when a `Sale` is removed).
