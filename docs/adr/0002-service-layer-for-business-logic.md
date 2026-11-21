# ADR 0002: Service Layer for Domain Logic

## Context
Controllers should handle HTTP requests and view rendering, not complex inventory calculations, FEFO allocations, or raw database backups.

## Decision
Introduce dedicated scoped domain services:
- `SaleService`: Processes checkout, calculates totals, snapshots prices, and allocates batches.
- `StockService`: Handles batch creation, inventory receipts, and stock balance calculations.
- `BackupService`: Executes SQL Server database backups to local storage.

## Why
- Keeps controllers thin and easy to read.
- Centralizes critical pharmacy business rules so they cannot be bypassed.
