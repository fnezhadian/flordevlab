# Patched EP 3: Moving to Entity Framework

Part of the **Patched** series from [Flor's Lab](https://www.youtube.com/@flordevlab).

> [!NOTE]
> Demo app with fake accounts. Run it locally only. Never deploy it and never use real data or real passwords.

## What this app is

The same tiny ASP.NET Core Razor Pages app from Episodes 1 and 2, called **Lab Shop**:

- a sign-in page (`/Login`)
- a product search page (`/Products`)
- a SQLite database created and seeded on first run

Nothing is broken today, but it could be. In Episode 1 we *mitigated* SQL injection with parameterized queries. That only works as long as every new query remembers its parameter, and raw ADO.NET always leaves that door open. This episode changes **how the app talks to the database**: from hand-written ADO.NET (`Microsoft.Data.Sqlite`) to Entity Framework Core, so the risk is *avoided by design* (built-in security) instead of managed by habit.

OWASP's Top 10:2025, [A05 Injection](https://owasp.org/Top10/2025/A05_2025-Injection/), names this as the preferred option: use a safe API that provides a parameterized interface, or migrate to an Object Relational Mapping tool (ORM).

## What changes (tag `patched-episode3-before` to `patched-episode3-after`)

| Where | Before | After |
|-------|--------|-------|
| `Data/Database.cs` | hand-written SQL, `Open()`, create tables, seed | deleted |
| `Data/PasswordMigration.cs` | one-time ADO.NET migration from Ep 2 | deleted (it already ran in Ep 2) |
| `Data/User.cs`, `Data/Product.cs` | (none) | entity classes, property names = column names |
| `Data/LabDbContext.cs` | (none) | the one door to the database |
| `Data/DbSeeder.cs` | seeding inside `Database.cs` | same demo rows, only when a table is empty |
| `Pages/Login.cshtml.cs` | SQL string with `$username` | LINQ lookup by username only, same `Verify`, same error message |
| `Pages/Products.cshtml.cs` | SQL string with `LIKE $term` | LINQ `Where` with `EF.Functions.Like` |
| `Program.cs` | registers `Database`, `migrate-passwords` command | registers `LabDbContext`, runs `Migrate()` and the seeder at startup |
| `LabShop.csproj` | `Microsoft.Data.Sqlite` | `Microsoft.EntityFrameworkCore.Sqlite` and `.Design` (9.0.x) |
| `Migrations/` | (none) | `InitialCreate`, matches the tables you already have |

**What does not change:** the database file, the tables, the password hashes, the pages and the URLs. Raw SQL in the code goes from 11 `CommandText` assignments in 4 files to none.

## Good to know

- EF removes the door for everyday queries, but it is not magic. `FromSqlRaw` with a string you glued together is injectable again (the OWASP SQL Injection Prevention Cheat Sheet shows the same problem in ORM query languages such as HQL). Use LINQ, or `FromSql` / `FromSqlInterpolated`, which turn values into parameters. Pass the interpolated string straight into `FromSql`. If you build it into a variable first, or use `FromSqlRaw` with `$"..."`, the values are already glued in and it is injectable again. Only values become parameters: a table or column name (for example a user-chosen sort column) needs an allow-list check, which is an Ep 4 topic.
- `Contains()` on SQLite is case-sensitive, so a plain port would have changed the search. `EF.Functions.Like` keeps the old behavior (and, like before, `%` and `_` typed by a visitor still act as wildcards).
- `Price` stays a `double` because the existing column is `REAL`.

## Requirements

- .NET SDK 9 or newer (the project targets net9.0)
- Packages: `Microsoft.EntityFrameworkCore.Sqlite` and `Microsoft.EntityFrameworkCore.Design` (9.0.x)
- The EF command line tool, once per computer: `dotnet tool install --global dotnet-ef --version "9.*"`

## Run it

```bash
cd LabShop
dotnet run
```

A new database `labshop.db` is created from the migration and seeded on first run. Delete the file to reset the data. Every request prints the SQL that EF sends (look for `Executed DbCommand`).

Demo accounts (fake): `admin` / `flor` / `guest`.

## Use it with an existing database (from Episode 2)

Your `labshop.db` already has the tables, so EF must be told that the first migration is already applied:

1. Stop the app and **back up** `labshop.db`.
2. `dotnet ef migrations add InitialCreate` (skip if the `Migrations` folder is already there).
3. Add one row to `__EFMigrationsHistory` with the migration id and the EF version. The SQL is in the production guide (A0, Part 5) and in `sql/05-baseline-ef-history.sql`.
4. `dotnet ef database update`. Nothing runs, because nothing is pending.

Do the baseline once. Running it twice fails on purpose.

## Test cases

1. Right password still logs in (the hashes from Episode 2 still work)
2. Wrong password is rejected with the same message
3. `admin' --` is still blocked (the Episode 1 fix)
4. Search for `desk` finds Desk Mat and Standing Desk, `' OR 1=1 --` finds nothing, and the SQL in the console shows a parameter
5. Searching the code for `CommandText` finds nothing

## Folder contents

```
episode3/
├── README.md
├── LabShop/            # the finished app (EF Core)
└── patched-after/      # the files that change in the patch step
    ├── LabShop.csproj
    ├── Program.cs
    ├── appsettings.Development.json
    ├── Data/           # User, Product, LabDbContext, DbSeeder
    ├── Pages/          # Login.cshtml.cs, Products.cshtml.cs
    └── Migrations/
```

Also deleted in this step: `Data/Database.cs` and `Data/PasswordMigration.cs`.
