# ADR 0001: Monolithic ASP.NET Core MVC with SQL Server Express

## Context
PharmaDesk runs offline in a local community pharmacy on a single counter PC. There is no reliable internet connection or IT operations team.

## Decision
Build PharmaDesk as a single, self-contained ASP.NET Core MVC application with Entity Framework Core 9 and a local SQL Server Express instance.

## Why
- Zero network dependencies: boots and operates 100% offline.
- Simple deployment: run as a local service or executable on the cashier's Windows machine.
- LAN-ready: if the pharmacy adds a second PC in the future, it can connect via local Wi-Fi / Ethernet to the main PC.
