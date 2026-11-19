# ADR 0008: Immutable Price Snapshot in SaleItems

## Context
Medicine selling prices change over time due to inflation or supplier adjustments. If past sales dynamically read `Medicine.SellingPrice`, past revenue and accounting totals change retroactively.

## Decision
Copy the exact selling price into `SaleItems.UnitPrice` at the time of checkout.

## Why
- Past sales, accounting summaries, and reprint receipts remain 100% immutable and accurate forever.
