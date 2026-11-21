# ADR 0004: Strict FEFO and Core Inventory Rules

## Context
Dispensing expired or newer stock before older stock leads to expired medicine waste and patient safety violations.

## Decision
Enforce strict First-Expiry-First-Out (FEFO) dispensing:
1. Batches are sorted ascending by `ExpiryDate`.
2. Deduct from the earliest expiring batch first; spill over to subsequent batches if needed.
3. Automatically exclude any batch where `ExpiryDate <= DateTime.Today`.
4. Run the entire sale in a single database transaction (`BeginTransactionAsync`).

## Why
- Guaranteed zero expired medicine sales.
- Protects pharmacy from inventory waste and legal non-compliance.
