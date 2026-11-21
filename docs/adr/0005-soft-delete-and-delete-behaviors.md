# ADR 0005: Soft Delete for Master Data & Restrict Foreign Keys

## Context
Hard-deleting a medicine, category, or supplier corrupts historic receipts, purchase audits, and accounting balances.

## Decision
- Use soft delete (`IsActive = false`) for master tables (`Medicines`, `Categories`, `Suppliers`, `TeamMembers`).
- Apply `DeleteBehavior.Restrict` on all parent relationships in EF Core.
- Apply `DeleteBehavior.Cascade` only to line item tables (`PurchaseItems`, `SaleItems`).

## Why
- Historic receipts and purchase invoices remain 100% intact forever.
- Accidental deletions are blocked at the database engine level.
