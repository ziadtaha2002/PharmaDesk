# ADR 0010: Forensic Audit Trail in ActivityLog

## Context
Pharmacy owners need clear accountability for high-impact operations: price modifications, logins, purchases, and database backups.

## Decision
Log critical business events into the append-only `ActivityLogs` table with `TeamMemberId`, `Action`, `Details`, and `CreatedAt` (UTC).

## Why
- Full accountability for revenue and price adjustments.
- Read-only audit trail accessible only to the Admin.
