# 05. Core Business Rules & FEFO

PharmaDesk enforces 4 strict rules at all times to prevent inventory discrepancies, legal violations, or overselling.

---

## 🛡️ The 4 Core Invariants

1. **Rule 1: No Overselling (No Negative Stock)**
   - A sale can never proceed if the requested quantity exceeds available stock.
   - `CurrentQuantity` on a batch can never drop below zero.

2. **Rule 2: Never Dispense Expired Stock**
   - Batches where `ExpiryDate <= DateTime.Today` are strictly excluded from sale calculations.
   - Even if physical stock exists on the shelf, the system will reject the sale.

3. **Rule 3: Strict FEFO (First-Expiry-First-Out)**
   - Medicines are dispensed from the batch with the **earliest expiry date first**.
   - If one batch doesn't have enough, the remaining quantity is automatically deducted from the next earliest batch.

4. **Rule 4: Atomic Database Transaction**
   - Every sale or purchase happens inside a single SQL transaction (`BeginTransactionAsync`).
   - If any step fails, everything is rolled back—no partial sales or ghost inventory deductions.

---

## 🎨 Expiry Badge Colors

On screen, batches are color-coded:

| Condition | Badge Color | Meaning |
|---|:---:|---|
| `ExpiryDate <= Today` | 🔴 **Red** | **Expired** (Blocked from sale) |
| `ExpiryDate <= Today + 90 Days` | 🟠 **Orange** | **Near Expiry** (Sell soon alert) |
| `ExpiryDate > Today + 90 Days` | 🟢 **Green** | **Good Stock** |

---

## 🧾 Receipt Numbering

Receipts follow this exact format:
$$\text{REC-YYYYMMDD-####}$$
Example: `REC-20261120-0001` (resets sequence daily).
