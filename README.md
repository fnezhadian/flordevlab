# Flor's Lab

**Build it. Test it. Secure it.**

Code that goes with the [Flor's Lab](https://www.youtube.com/@flordevlab) YouTube channel: real ASP.NET / .NET apps, built, tested on screen, and then patched.

> [!WARNING]
> Some projects in this repository are **intentionally vulnerable** so they can be tested and fixed on screen. They are for learning only.
> **Never deploy them to a public server or use them with real data.** Run them locally only.

## How the repo is organised

```
flordevlab/
├── README.md
├── LICENSE
└── patched/                    # the "Patched" series
    ├── README.md               # what the series is
    ├── episode1/               # Patched EP 1
    │   └── README.md           # what is vulnerable, what was fixed
    └── episode2/               # one folder per episode
    │   └── README.md           # what is vulnerable, what was fixed
```

More series can get their own top-level folder later.

## Series

### Patched

Test an app, find what is wrong, then patch it. Each episode is a numbered folder under [`patched/`](./patched).

| # | Title | Video | Code |
|---|-------|-------|------|
| 1 | EP1: Can someone login to your app without the right password? | [_Youtube_](https://www.youtube.com/watch?v=Ev9vu9wRfD4) | [`episode1`](./patched/episode1) |
| 1 | EP2: Are your passwords visible? | [_Youtube_](https://www.youtube.com/watch?v=uDXjeFNA0Fs) | [`episode2`](./patched/episode2) |

## Before and after

Each Patched episode has two Git tags:

| Tag | Meaning |
|-----|---------|
| `patched-episodeN-before` | the app as it starts in the video (vulnerable) |
| `patched-episodeN-after`  | the app after the fixes (patched) |

Compare them on GitHub to see exactly what changed:
`https://github.com/fnezhadian/flordevlab/compare/patched-episode1-before...patched-episode1-after`

## Running a project locally

1. Install the [.NET SDK](https://dotnet.microsoft.com/download) (version noted in each project's README).
2. Clone the repo and open the episode folder.
3. Run `dotnet run`.

## License

Code is released under the MIT License. See [LICENSE](./LICENSE).
