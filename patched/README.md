# Patched

A series from [Flor's Lab](https://www.youtube.com/@flordevlab): test an app, find what is wrong, then patch it.

> [!WARNING]
> Episode apps may be **intentionally vulnerable**. Run them locally only. Never deploy them or use real data.

## Episodes

| # | Title | Folder | Tags |
|---|-------|--------|------|
| 1 | Patched EP1: Can someone login to your app without the right password? | [`episode1`](./episode1) | `patched-episode1-before`, `patched-episode1-after` |
| 2 | Patched EP2: Are your passwords visible? | [`episode2`](./episode2) | `patched-episode2-before`, `patched-episode2-after` |
| 3 | Patched EP3: Is your database code one typo away from a breach? | [`episode3`](./episode3) | `patched-episode3-before`, `patched-episode3-after` |

## Folder convention

- One folder per episode: `episode1`, `episode2`, and so on
- Each folder has its own `README.md`: what the app does, what is vulnerable, what the fix is, and the .NET version
- Tag the start of the video `patched-episodeN-before` and the finished fixes `patched-episodeN-after`
