# Patched EP 2: Plain Text Passwords

Part of the **Patched** series from [Flor's Lab](https://www.youtube.com/@flordevlab).

> [!WARNING]
> The "before" version of `LabShop` stores passwords in **plain text on purpose**. Run it locally only. Never deploy it and never use real data or real passwords.

## What this app is

The same tiny ASP.NET Core Razor Pages app from Episode 1, called **Lab Shop**:

- a sign-in page (`/Login`)
- a product search page (`/Products`)
- a SQLite database created and seeded on first run

The SQL injection flaws from Episode 1 are already fixed. This episode stays on ADO.NET (`Microsoft.Data.Sqlite`).

## What is vulnerable (tag `patched-episode2-before`)

| Where | Flaw | Why |
|-------|------|-----|
| `Data/Database.cs` | plain text passwords | the `Users` table has a `Password` column that stores the password as typed |
| `Pages/Login.cshtml.cs` | plain text comparison | the login compares the typed password directly with the stored one |

Anyone who gets a copy of the database (a backup, a leaked file, an injection bug) gets every password at once, and people reuse passwords on other sites.

## The fix (tag `patched-episode2-after`)

- `Services/PasswordService.cs` (new): wraps ASP.NET Core's `PasswordHasher` (PBKDF2, random salt per user, one string out)
- `Data/Database.cs`: `PasswordHash` column instead of `Password`, seed users are hashed
- `Data/PasswordMigration.cs` (new): one-time migration of existing users, in one transaction with a safety check, then `VACUUM`
- `Pages/Login.cshtml.cs`: verifies the hash, and gives the same error for a wrong username and a wrong password
- `Program.cs`: registers `PasswordService` and adds the `migrate-passwords` command

Compare the two tags on GitHub to see the whole change.

## Requirements

- .NET SDK 8 or newer
- Package: `Microsoft.Data.Sqlite`
- The hasher comes with ASP.NET Core, no extra package. Only the small `hash-speed-demo` console project needs `Microsoft.Extensions.Identity.Core`.

## Run it

```bash
cd LabShop
dotnet run
```

Open the URL printed in the terminal. The database file `labshop.db` is created in the project folder on first run. Delete it to reset the data.

Demo accounts (fake): `admin` / `flor` / `guest`.

## Migrate an existing database

If you already have a `labshop.db` from Episode 1 (plain text passwords):

1. Stop the app and **back up** `labshop.db`.
2. Run:

```bash
dotnet run -- migrate-passwords
```

3. Check that no plain text is left in the file.
4. **Delete the backup.** It still contains the plain text passwords.

The migration is safe to run twice. If any user is not hashed, nothing is changed.

## Test cases

1. Right password logs in
2. Wrong password is rejected
3. Unknown username gives the same message
4. `admin' --` is still blocked (the Episode 1 fix)
5. No plain text password is left in the database file

## Folder contents

```
episode2/
├── README.md
├── LabShop/            # the "before" app, as recorded
├── patched-after/      # the files that change in the patch step
│   ├── Services/PasswordService.cs
│   ├── Data/Database.cs
│   ├── Data/PasswordMigration.cs
│   ├── Pages/Login.cshtml.cs
│   └── Program.cs
└── hash-speed-demo/    # small console demo: SHA-256 vs PasswordHasher
```