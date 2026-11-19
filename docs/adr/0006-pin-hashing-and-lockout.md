# ADR 0006: PBKDF2 PIN Hashing and 5-Try Lockout

## Context
Operators need quick counter logins using a 4-digit PIN. However, 4-digit PINs are vulnerable to brute-force guessing without rate limiting.

## Decision
- Hash all PINs using ASP.NET Core `PasswordHasher<TeamMember>` (PBKDF2 with SHA-256 and salted key derivation).
- Track `FailedPinCount` per user.
- On the 5th failed attempt, lock the account for 5 minutes (`LockedUntil = DateTime.UtcNow.AddMinutes(5)`).

## Why
- Fast for cashiers, secure against brute-force tampering.
- Resets automatically after successful login.
