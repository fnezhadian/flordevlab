# Patched Ep 4: Input Validation

Part of the **Patched** series from [Flor's Lab](https://www.youtube.com/@flordevlab).

> [!NOTE]
> Demo app with fake accounts. Run it locally only. Never deploy it and never use real data or real passwords.

## What this app is

The same tiny ASP.NET Core Razor Pages app from Episodes 1 to 3, called **Lab Shop**: a sign-in page (`/Login`), a product search page (`/Products`) and a SQLite database accessed with Entity Framework Core.

Nothing is broken today, but the app accepts anything: empty fields, very long text, any characters. Entity Framework protects the database query, but it does not decide which input is acceptable. This episode adds an **allow list** on the server, in front of everything else.

OWASP's Top 10:2025, [A05 Injection](https://owasp.org/Top10/2025/A05_2025-Injection/), says positive (allow list) server-side validation helps but is not a complete defense. That is why EF stays: these are layers, not replacements.

## What changes (tag `patched-episode4-before` to `patched-episode4-after`)

| Where | Before | After |
|-------|--------|-------|
| `Pages/Login.cshtml.cs` | no rules | Username: required, 50 characters at most, letters, numbers, `.`, `_`, `-`. Password: required, 128 at most. `if (!ModelState.IsValid) return Page();` |
| `Pages/Login.cshtml` | plain inputs | a message under each field, and the validation scripts |
| `Pages/Products.cshtml.cs` | any search text | Search: 50 characters at most, letters, numbers, spaces and hyphens. Not valid: no search |
| `Pages/Products.cshtml` | plain input | `asp-for` and a message under the box |

**What does not change:** the database, the migrations, the packages, the hashing and EF. No new packages and no migration.

## How it works

- Rules are data annotations on the page model properties (`Required`, `StringLength`, `RegularExpression`).
- The page asks `ModelState.IsValid`. If it is not valid, it returns before any database call.
- The browser can check too (the validation scripts), but the user can skip that. The server must always do its own check, and that is the check we rely on.
- Wrong characters on login show the same generic message as a wrong password, so the page does not teach an attacker the rules.

## Good to know

- Validation is a layer, not a fix on its own. It reduces what reaches the rest of the app. It does not replace parameterized queries or EF.
- Length limits stop odd input, not a flood of requests. That is the next episode (rate limiting).
- There is no minimum password length on login. That belongs to wherever a password is created.

## Requirements

The same as Episode 3: .NET SDK 9, and the packages already in the project. Nothing new to install.

## Run it

```bash
cd LabShop
dotnet run
```

Demo accounts (fake): `admin` / `flor` / `guest`.

## Test cases

1. Empty fields show `Enter your username.` and `Enter your password.`
2. The Username box stops you at 50 characters (the page adds `maxlength` by itself). If you skip that limit, the server shows `Username is too long.`
3. `admin' --` is rejected with the generic message
4. With JavaScript off, the server still rejects them, and no SQL runs
5. Normal login and a search for `desk` still work; `' OR 1=1 --` in the search shows `Use letters, numbers, spaces and hyphens.`

## Folder contents

```
episode4/
├── README.md
├── LabShop/            # the finished app
└── patched-after/      # the files that change in the patch step
    └── Pages/
        ├── Login.cshtml
        ├── Login.cshtml.cs
        ├── Products.cshtml
        └── Products.cshtml.cs
```
