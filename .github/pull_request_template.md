## Summary
<!-- Provide a brief 1-2 sentence description of what this PR introduces or fixes. -->

## Changes Made
<!-- Bulleted list of specific changes, new files, modified controllers/views/services. -->
- 

## Linked Issue
<!-- Link the issue closed by this PR, e.g. Closes #12 -->
Closes #

## Hard Rule & Quality Verifications
- [ ] **No Direct Commit to Main**: Branch was created from `main`.
- [ ] **Zero Code Deletions / Renames (if documentation PR)**: Existing source code unchanged.
- [ ] **Clean Build**: `dotnet build` succeeds with 0 errors and 0 new warnings.
- [ ] **No Secrets**: No passwords, API keys, or plaintext connection strings included.
- [ ] **Role Authorization**: Any new/edited Admin action enforces `[Authorize(Roles = "Admin")]`.
- [ ] **Offline Execution**: No external CDN links added; all assets load from local `wwwroot/lib/`.
- [ ] **Transaction Invariants**: Any inventory/sales mutations occur within an atomic database transaction.

## How to Test
<!-- Step-by-step instructions to verify the changes locally. -->
1. Run `dotnet run --project PharmaDesk`.
2. Navigate to `...`.
3. Verify that `...`.
