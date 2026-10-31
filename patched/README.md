# Patched

A series from [Flor's Lab](https://www.youtube.com/@flordevlab): test an app, find what is wrong, then patch it.

> [!WARNING]
> Episode apps may be **intentionally vulnerable**. Run them locally only. Never deploy them or use real data.

## Episodes

| # | Title | Folder | Tags |
|---|-------|--------|------|
| 1 | Patched EP 1: My ASP.NET App (SQL injection) | [`episode1`](./episode1) | `patched-episode1-before`, `patched-episode1-after` |
| 2 | Patched EP 2: Plain Text Passwords | [`episode2`](./episode2) | `patched-episode2-before`, `patched-episode2-after` |
| 3 | Patched EP 3: Moving to Entity Framework | [`episode3`](./episode3) | `patched-episode3-before`, `patched-episode3-after` |
| 4 | Patched EP 4: Input Validation | [`episode4`](./episode4) | `patched-episode4-before`, `patched-episode4-after` |
| 5 | Patched EP 5: Rate Limiting | [`episode5`](./episode5) | `patched-episode5-before`, `patched-episode5-after` |

More episodes are coming. Stay tuned.

## Folder convention

- One folder per episode: `episode1`, `episode2`, and so on
- Each folder has its own `README.md`: what the app does, what is vulnerable, what the fix is, and the .NET version
- Tag the start of the video `patched-episodeN-before` and the finished fixes `patched-episodeN-after`
