# ADR 0003: Inventory Lives Strictly in Batches

## Context
Pharmacies sell medicines with varying expiration dates and lot numbers. Storing a flat `Stock` integer on `Medicines` creates data drift and makes expiry tracking impossible.

## Decision
Physical stock is tracked exclusively in the `Batches` table (`CurrentQuantity`). `Medicines` has no stock column; available stock is calculated on-demand:
$$\text{Available Stock} = \sum (\text{CurrentQuantity of non-expired batches for the medicine})$$

## Why
- Eliminates dual-source inventory bugs.
- Enables accurate First-Expiry-First-Out (FEFO) dispensing.
