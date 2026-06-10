# Pulsar

Pulsar is an Elite Dangerous companion app that reads local journal and status files and serves a web UI with live updates over websockets.

The repository currently contains:

- `Pulsar/`: ASP.NET Core backend, journal processing pipeline, SQLite persistence, APIs, SignalR hub, and native overlay service
- `Pulsar/WebApp/`: Svelte frontend bundled into static assets and served by the backend
- `ObservatoryFramework/`: shared framework code
- `TestProject1/`: automated tests for journal processing, status handling, hub behavior, and file watching
- `Botanist/`: related or experimental components kept in the repo

## Current Capabilities

- Watches an Elite Dangerous journal directory and processes live journal updates
- Optionally processes historical journals on startup
- Persists cached state with SQLite via Entity Framework Core
- Exposes HTTP controllers and a SignalR event stream at `api/events`
- Serves a dashboard-style web UI from the ASP.NET app
- Shows ship, fuel, journal log, mission stack, and explorer views
- Includes explorer value calculation and filtering for high-value bodies
- Includes a botany page for exobiology tracking and scan-distance assistance
- Includes a native overlay service with configurable placement and refresh settings

## Roadmap

- [x] Read and parse journals
  - [x] Read related files and live updates
  - [x] Realtime journal processing
  - [x] Historical backfill on startup
- [x] Commander state and status views
  - [x] Current ship and status data
  - [x] Station and location updates
  - [x] Nav route and jump-related events
  - [x] Realtime API and SignalR events
  - [x] Cached journal-derived state with SQLite
- [x] Explorer workflow
  - [x] System exploration value calculation
  - [x] High-value body filtering and custom criteria
  - [ ] More advanced explorer criteria and routing hints
- [x] Exobiology workflow
  - [x] Botany page and scan tracking
  - [x] Required scan-distance assistance
  - [ ] Expanded species/value filters and richer route guidance
- [x] Fuel and travel support
  - [x] Fuel status display
  - [ ] Jump-range and scoop-time prediction improvements
  - [ ] Additional alerts and audio cues
- [x] Mission tracking
  - [x] Mission stack view
  - [ ] Better mission target correlation and filtering
- [x] Overlay foundation
  - [x] Native overlay service in the backend
  - [x] Production-ready Linux overlay path
  - [x] Windows overlay path
  - [ ] Overlay modules for:
    - [ ] Docking Hint ![like this](https://i.imgur.com/VYGzxYB.jpg)
    - [ ] exobiology 
    - [ ] fuel
    - [ ] explorer
- [ ] Multi-commander support
- [ ] Material tracking and material goals
- [ ] Commodity alerts and station trading helpers
- [ ] Outfitting targets and ship-build assistance
- [ ] Export to EDSY/Coriolis
- [ ] CAPI integration
- [ ] EDDN submission
- [ ] IGAU submission
- [ ] Plugin system
- [ ] Tray icon and background controls
- [ ] Multi-window or multi-device support

## Tech Stack

- `.NET 10` ASP.NET Core
- `Entity Framework Core` with SQLite
- `SignalR`
- `SvelteKit` + `Vite`
- `Bun` for frontend package management
- `Avalonia` for native overlay work

## Requirements

- .NET 10 SDK
- Bun 1.2.x
- Elite Dangerous journal files available on the local machine

## Configuration

Application settings are loaded from the repository root `appsettings.json` plus the environment-specific variant when present.

Relevant settings in `appsettings.json`:

```json
{
  "Pulsar": {
    "JournalDirectory": "C:\\Users\\User Name\\Saved Games\\Frontier Developments\\Elite Dangerous\\",
    "ProcessHistoricalJournals": false,
    "Overlay": {
      "Enabled": false,
      "NativeOverlayEnabled": true
    }
  }
}
```

Minimum setup:

1. Set `Pulsar:JournalDirectory` to your Elite Dangerous journal directory.
2. Enable `Pulsar:ProcessHistoricalJournals` if you want startup backfill.
3. Adjust `Pulsar:Overlay` settings if you want the native overlay enabled.

Default local endpoints:

- `http://localhost:5000`
- `https://localhost:5001`

## Development

### 1. Install frontend dependencies

```bash
cd Pulsar/WebApp
bun install
```

### 2. Build the frontend

The backend serves static files from `Pulsar/WebApp`, so build the web app before running the server.

```bash
cd Pulsar/WebApp
bun run build
```

### 3. Run the backend

From the repository root:

```bash
dotnet run --project Pulsar/Pulsar.csproj
```

Then open `https://localhost:5001` or `http://localhost:5000`.

## Frontend Commands

From `Pulsar/WebApp`:

```bash
bun run dev
bun run build
bun run check
bun run lint
bun run test
```

Notes:

- `bun run dev` starts the standalone Vite dev server for frontend work.
- The production app path is the built static frontend served by the ASP.NET backend.

## Backend Notes

- Controllers currently live under `Journal`, `ModulesInfo`, and `Overlay` features.
- Live events are pushed through SignalR at `api/events`.
- The backend registers hosted services for file watching, journal processing, and the native overlay service.
- SQLite database creation is triggered automatically on startup.

## Tests

Run the .NET test suite from the repository root:

```bash
dotnet test
```

Current test coverage includes:

- journal processing
- status service behavior
- scan key handling
- events hub behavior
- file watcher behavior

## Status

This project is still under active development. Some older documentation and related subprojects in the repository are experimental or incomplete, but the main `Pulsar/` application is wired for local development and live journal-driven updates.
