# ADR 0009: 100% Offline Local Assets (No CDN)

## Context
Community pharmacies often experience intermittent or absent internet connectivity. CDN-hosted CSS or JavaScript assets fail to load offline, breaking the UI.

## Decision
Bundle all static dependencies locally inside `wwwroot/lib/`:
- Bootstrap 5 CSS & JS
- Bootstrap Icons font files
- jQuery

## Why
- Zero internet dependency: the UI renders with complete fidelity 100% offline.
