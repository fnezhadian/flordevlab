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
├── ep01-patched-aspnet-app/     # Episode 1: test an ASP.NET app, then patch it
│   └── README.md                # what is vulnerable, what was fixed
└── ...                          # one folder per episode or project
```

## Before and after

Each episode that breaks something and then fixes it has two Git tags:

| Tag | Meaning |
|-----|---------|
| `epXX-before` | the app as it starts in the video (vulnerable) |
| `epXX-after`  | the app after the fixes (patched) |

Compare them on GitHub to see exactly what changed:
`https://github.com/fnezhadian/flordevlab/compare/ep01-before...ep01-after`

## Episodes

| # | Title | Video | Code |
|---|-------|-------|------|
| 1 | Patched: my ASP.NET app | _link after publishing_ | [`ep01-patched-aspnet-app`](./ep01-patched-aspnet-app) |

## Running a project locally

1. Install the [.NET SDK](https://dotnet.microsoft.com/download) (version noted in each project's README).
2. Clone the repo and open the project folder.
3. Run `dotnet run`.

## License

Code is released under the MIT License. See [LICENSE](./LICENSE).
