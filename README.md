# Elite Pulsar

A Work In Progress Application For Presenting Game Data In Realtime From Elite Dangerous Journals & Events.

A Cross Platform Fork of Elite Observatory Core.

## WIP Preview
 ![Preview](https://s3.amazonaws.com/i.snag.gy/R73z9W.jpg)

## Planned Feature

 - [x] Read & Parse the Journals
   - [x] Read Extra Files
 - [ ] Show Journal information ( Current Ship, Station, etc. )
   - [ ] Current Ship/Station/NavRoute/Jumps
   - [x] Realtime API (Events)
   - [ ] Extended Updates API (include relevant data with events)
 - [x] Realtime Journal Updates
 - [ ] Multiple Commanders
 - [ ] Cached Journal Data (in-memory SQL, enabled extended updates)
 - [ ] !System Exploration Value
   - [ ] Custom Criteria (e.g. "Has Water World", "more than 400Kcr Scan value")
 - [ ] !System Exobiology Value 
   - [ ] !Exobiology Scan info (+[Show Req. Distance](https://github.com/EDCD/EDDI/blob/e28ef64a1d41c1e39485863aa362d207e8d36834/Utilities/Functions.cs#L128C1-L152C10) for next scan)
   - [ ] Custom Criteria (e.g. "Stratum Tectonicas", "less than 500m distance per scan", "more than 1Mcr Scan value")
 - [ ] !Fuel/Jump Warning
   - [x] Fuel Scoop Estimate Time Remaining
 - [ ] Material Tracking (Flag planets with materials, set goals (Mat#, not modules), closest trader) [e.g.](https://github.com/jixxed/ed-odyssey-materials-helper)
 - [ ] Custom Sounds (on user-defined Events)
 - [ ] Mission Targets [Mission Stack Viewer](https://github.com/kaivalagi/EDMissionStackViewer)
 - [ ] Commodities Targets/Alerts (Commodity Above/Below value at current station)
 - [ ] !Outfitting Targets (does this station have wanted parts for a build)
 - [ ] Export to EDSY/Coriolis
 - [ ] CAPI Integration
 - [ ] !EDDN Submission
   - [ ] Must be easilty disableable for sneaky business
 - [ ] IGAU Submission
 - [ ] Plugin System
 - [ ] !Overlay
   - [ ] !Linux (DXVK/Vulkan) [imgoverlay](https://github.com/nowrep/imgoverlay)
   - [ ] Windows (DirectX) [Overlay.NET](https://github.com/lolp1/Overlay.NET/)
   - [ ] Docking Hint ![like this](https://i.imgur.com/VYGzxYB.jpg)
   - [ ] Biology Value & Scan Distance
   - [ ] Fuel Scoop Estimate Time Remaining
   - [ ] Custom Criteria
 - [ ] Tray Icon
 - [ ] Multiple Windows/Device Support
 - [ ] Integrate ObservatoryCore Plugins:
   - [ ] https://github.com/mcmuttons/GeoPredictor

Key:  
! = priority

## How To Use

TODO: make easy

 - Build the web app
 - install the service (web app must be served from the backend)
 - configure journal directory in appsettings.json
 - load the application
