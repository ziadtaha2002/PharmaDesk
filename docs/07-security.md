# 07. Security & Authentication

PharmaDesk balances fast counter logins with solid local security controls.

---

## 🔑 PIN Authentication

- Each team member is assigned a **4-digit PIN**.
- PINs are never stored in plaintext. They are hashed using ASP.NET Core's `PasswordHasher<TeamMember>` (PBKDF2 with SHA-256 and salted key derivation).
- On login, the session authentication cookie stores the user's ID, Name, and Role (`Admin` or `Staff`).

---

## 🛑 5-Try Account Lockout

To prevent brute-force guessing of 4-digit PINs:
- Every failed attempt increments `FailedPinCount`.
- On the **5th consecutive failure**, `LockedUntil` is set to `DateTime.UtcNow.AddMinutes(5)`.
- The user is locked out for **5 minutes**, and the login screen displays the remaining wait time.
- A successful login resets `FailedPinCount` back to `0` and clears `LockedUntil`.

---

## 🛡️ Role-Based Access Control (RBAC)

- The frontend menu in `_Layout.cshtml` hides administrative links from Staff.
- **Critical Server Protection**: Every administrative controller action must be decorated with `[Authorize(Roles = "Admin")]`. Staff users trying to tamper with URLs directly are redirected to `/Account/AccessDenied` (HTTP 403).

---

## 📜 Audit Trail

All critical operations are logged to the `ActivityLogs` table:
- Logins (Success / Failure / Lockout)
- Completed Sales
- Completed Purchases
- Medicine price changes (logs old and new price)
- Database backups
- Team member deactivations
