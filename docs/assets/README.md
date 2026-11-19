# Original Design Assets

This directory contains the original specification documents and design artifacts provided at project inception.

## File Manifest

| File | Type | Description | Status & Authority |
|---|---|---|---|
| `Technical_Design.pdf` | Document (PDF) | Detailed technical specification: 2-tier role matrix, full data dictionary (10 tables), foreign key delete behaviors, database indexes, 4 core inventory business rules, screen mockups (Dashboard & New Sale), and style guide. | **Source of Truth** (Newer specification). Note: Typographical mention of "OnlyAdmin" on page 1 is resolved as "Admin". Dashboard mockup sidebar omitted secondary pages which are canonically specified in Section 1. |
| `PharmaDesk_-_Full_Design.pdf` | Document (PDF) | Early high-level project plan, feature overview, and 12-step implementation roadmap. | **Original / Partly Outdated**. Superseded by `Technical_Design.pdf` where conflicts exist. Specifically, the legacy "Sections" entity, "Team and Sections" screen, and "Management" role were formally removed in favor of `TeamMembers.Role` ("Admin" and "Staff"). |
| *Original ER Diagram Reference* | Schema Reference | A preliminary high-level conceptual table diagram. | **Simplified / Superseded**. The original conceptual image omitted critical fields (`Unit`, `GenericName`, `IsActive` on `Suppliers`, `Address`, `InvoiceNumber`, `PaymentMethod`, `ReceiptNumber`, `FailedPinCount`, `LockedUntil`, `CreatedAt`, `Batches.PurchaseItemId`), lacked the `ActivityLog` table, and missed the `PurchaseItems` → `Batches` lineage. The canonical entity-relationship model is documented in [`docs/04-data-model.md`](../04-data-model.md). |

## Authority Rules

1. When technical discrepancies arise between the Full Design and the Technical Design, **`Technical_Design.pdf` takes precedence**.
2. When discrepancies exist between conceptual diagrams and the **Data Dictionary** (Section 2 of `Technical_Design.pdf`), the **Data Dictionary is authoritative**.
3. All authoritative architectural and structural decisions are formally captured in [`docs/adr/`](../adr/).
