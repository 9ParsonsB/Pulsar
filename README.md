# Elite Pulsar

A Work In Progress Application For Presenting Game Data In Realtime From Elite Dangerous Journals & Events.

A Cross Platform Fork of Elite Observatory Core.

## Planned Feature

 - [ ] Read the Journal
   - [x] Read Status File
 - [ ] Show Journal information ( Current Ship, Station, etc. )
   - [ ] Realtime API (Events, with extended updates)
 - [ ] Realtime Journal Updates
 - [ ] Multiple Commanders
 - [ ] Cached Journal Data (in-memory SQL, enabled extended updates)
 - [ ] !System Exploration Value
 - [ ] !System Exobiology Value 
   - [ ] !Exobiology Scan info (+[Show Req. Disatance](https://github.com/EDCD/EDDI/blob/e28ef64a1d41c1e39485863aa362d207e8d36834/Utilities/Functions.cs#L128C1-L152C10) for next scan)
 - [ ] !Fuel/Jump Warning
 - [ ] Material Tracking (Flag planets with materials, set goals, closest trader) [e.g.](https://github.com/jixxed/ed-odyssey-materials-helper)
 - [ ] Custom Sounds (on user-defined Events)
 - [ ] Mission Targets [Mission Stack Viewer](https://github.com/kaivalagi/EDMissionStackViewer)
 - [ ] Commodities Targets/Alerts (Commodity Above/Below value at current station)
 - [ ] !Outfitting Targets (does this station have wanted parts for build)
 - [ ] CAPI Integration
 - [ ] !EDDN Submission
   - [ ] Must be easilty disableable for sneaky business
 - [ ] IGAU Submission
 - [ ] Plugin System
 - [ ] !Overlay
   - [ ] Docking Hint ![like this](https://i.imgur.com/VYGzxYB.jpg)
 - [ ] Tray Icon
 - [ ] Multiple Windows/Device Support
 
## How To Use

TODO: make easy

 - Build the web app
 - install the service (web app must be served from the backend)
 - configure journal directory in appsettings.json
 - load the application
