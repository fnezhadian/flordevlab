# Patched

A series from [Flor's Lab](https://www.youtube.com/@flordevlab): test an app, find what is wrong, then patch it. The same app, **LabShop** (ASP.NET Core Razor Pages, SQLite), grows from one episode to the next.

> [!WARNING]
> Episode apps may be **intentionally vulnerable**. Run them locally only. Never deploy them or use real data.

## Episodes

| # | Question | What we patch | Folder | Tags | Video |
|---|----------|---------------|--------|------|-------|
| 1 | Can someone log in to your app without the right password? | SQL injection and broken access control | [`episode1`](./episode1) | `patched-episode1-before`, `patched-episode1-after` | [https://youtu.be/Ev9vu9wRfD4] |
| 2 | Are your passwords visible? | Plain text passwords: hashing (PBKDF2) | [`episode2`](./episode2) | `patched-episode2-before`, `patched-episode2-after` | [https://youtu.be/uDXjeFNA0Fs] |
| 3 | Is your database code one typo away from a breach? | Raw ADO.NET to Entity Framework | [`episode3`](./episode3) | `patched-episode3-before`, `patched-episode3-after` | [https://youtu.be/4Sna3TwqEgM] |
| 4 | Does your app trust everything users type? | Input validation (allow list) | [`episode4`](./episode4) | `patched-episode4-before`, `patched-episode4-after` | [https://youtu.be/QlR5UudIdug] |
| 5 | Can someone try a thousand passwords on your login? | Rate limiting on the login | [`episode5`](./episode5) | `patched-episode5-before`, `patched-episode5-after` | [https://youtu.be/JCPyT3ib76U] |

More episodes are coming. Stay tuned.

## Folder convention

- One folder per episode: `episode1`, `episode2`, and so on
- Each folder has its own `README.md`: what the app does, what is vulnerable, what the fix is, and the .NET version
- Each episode folder has `LabShop` (the finished app) and `patched-after` (only the files that change in the patch step)
- Tag the start of the video `patched-episodeN-before` and the finished fix `patched-episodeN-after`

## See what changed in an episode

Compare the two tags on GitHub. For example, for Episode 5:

`https://github.com/fnezhadian/flordevlab/compare/patched-episode5-before...patched-episode5-after`
