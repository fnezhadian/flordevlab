# Patched EP 1: My ASP.NET App (SQL injection)

Part of the **Patched** series from [Flor's Lab](https://www.youtube.com/@flordevlab).

> [!WARNING]
> `LabShop` is **intentionally vulnerable**. Run it locally only. Never deploy it and never use real data or real passwords.

## What this app is

A tiny ASP.NET Core Razor Pages app called **Lab Shop**:

- a sign-in page (`/Login`)
- a product search page (`/Products`)
- a SQLite database created and seeded on first run

## What is vulnerable (tag `patched-episode1-before`)

| Where | Flaw | Why |
|-------|------|-----|
| `Pages/Login.cshtml.cs` | SQL injection | the username and password are concatenated into the SQL string |
| `Pages/Products.cshtml.cs` | SQL injection | the search term is concatenated into the SQL string |
| `Pages/Products.cshtml.cs` | verbose errors | raw database error messages are shown to the visitor |

Passwords are also stored in plain text on purpose. That flaw is left in place and gets its own episode.

## The fix (tag `patched-episode1-after`)

- Parameterized queries in `Login.cshtml.cs` and `Products.cshtml.cs`
- A generic error message instead of the raw database error

Compare the two tags on GitHub to see the whole change.

## Requirements

- .NET SDK 8 or newer
- Package: `Microsoft.Data.Sqlite`

## Run it

```bash
cd LabShop
dotnet run
```

Open the URL printed in the terminal. The database file `labshop.db` is created in the project folder on first run. Delete it to reset the data.

Demo accounts (fake): `admin` / `flor` / `guest`.

## Folder contents

```
episode1/
├── README.md
├── LabShop/            # the vulnerable app, as recorded
└── patched-after/      # the two files that change in the patch step
    ├── Login.cshtml.cs
    └── Products.cshtml.cs
```
