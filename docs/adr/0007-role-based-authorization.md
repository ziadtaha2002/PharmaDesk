# ADR 0007: Two-Role Authorization & Server-Side Enforcement

## Context
PharmaDesk requires two operator personas: Admin (owner/manager) and Staff (cashier). Hiding UI links in the Razor view is not enough—users can type URLs directly into the browser.

## Decision
- Standardize on exactly 2 roles: `Admin` and `Staff` (legacy "Sections" removed).
- Enforce server-side authorization on every administrative controller and action using `[Authorize(Roles = "Admin")]`.
- Direct unauthorized requests redirect to `/Account/AccessDenied` (HTTP 403).

## Why
- Complete defense-in-depth: frontend hides links, backend blocks direct HTTP requests.
