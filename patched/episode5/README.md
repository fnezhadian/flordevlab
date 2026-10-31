# Patched Ep 5: Rate Limiting

Part of the **Patched** series from [Flor's Lab](https://www.youtube.com/@flordevlab).

> [!NOTE]
> Demo app with fake accounts. Run it locally only. Never deploy it and never use real data or real passwords.

## What this app is

The same tiny ASP.NET Core Razor Pages app from Episodes 1 to 4, called **Lab Shop**: a sign-in page (`/Login`), a product search page (`/Products`) and a SQLite database accessed with Entity Framework Core.

Nothing is broken today, but the login lets anyone try a password as many times as they want. A person guesses slowly. A program guesses fast. Hashing protects the stored passwords and validation checks what comes in, but neither one counts the tries. This episode adds a **rate limiter** on the login.

## What changes (tag `patched-episode5-before` to `patched-episode5-after`)

| Where | Before | After |
|-------|--------|-------|
| `Program.cs` | no rate limiter | a rate limiter with one policy called `login`: a fixed window, 3 tries per minute for each client address. It counts only `POST` requests (the sign-in tries). When the limit is reached, the app answers `429, Too Many Requests` and sets the `Retry-After` header. `app.UseRateLimiter()` turns it on |
| `Pages/Login.cshtml.cs` | no limit | one line above the class: `[EnableRateLimiting("login")]` |

**What does not change:** the database, the migrations, the packages, hashing, EF and validation. No new packages and no migration. The rate limiter is part of ASP.NET Core.

## How it works

- A rate limiter counts the requests and says stop when there are too many.
- A **fixed window** counts the tries inside a time window (here 1 minute). When the window ends, the count starts again.
- The count is for each client address. Opening the login page (a `GET`) is not counted, only the sign-in tries (a `POST`).
- A blocked request never reaches the page, so it never reaches the database.

## Good to know

- Rate limiting is a layer, not a complete fix. It reduces how fast someone can guess. It does not replace hashing, EF or validation.
- One minute is for the demo. In real life, you probably need to block for longer than a minute.
- The count is per client address. Someone with many addresses can spread their tries, and many users can share one address (for example behind a proxy).
- Account lockout is another layer, which counts the tries for each user. It is not part of this episode.
- OWASP Top 10:2025, [A07 Authentication Failures](https://owasp.org/Top10/2025/A07_2025-Authentication_Failures/), is the category this layer belongs to.

## Requirements

The same as Episode 4: .NET SDK 9, and the packages already in the project. Nothing new to install.

## Run it

```bash
cd LabShop
dotnet run
```

Demo accounts (fake): `admin` / `flor` / `guest`.

## Test cases

1. Three wrong tries on `/Login` are allowed (each shows `Invalid username or password.`)
2. The next try is blocked: `429, Too Many Requests. Try again in N seconds.`
3. Even the correct password is blocked during the wait
4. After the wait, the login works again, and other pages (for example `/Products?Search=desk`) work all the time
5. In the terminal, only the first three tries show a `Users` query. The blocked tries never reached the database

## Folder contents

```
episode5/
├── README.md
├── LabShop/            # the finished app
└── patched-after/      # the files that change in the patch step
    ├── Program.cs
    └── Pages/
        └── Login.cshtml.cs
```
