<script lang="ts">
  import connection from "$lib/stores/Connection.store";
  import type { FSSDiscoveryScan } from "../../types/api/FSSDiscoveryScan";
  import type { Scan } from "../../types/api/Scan";
  import type JournalBase from "../../types/api/JournalBase";
  import type { FSSBodySignals } from "../../types/api/Signals";

  const data: Partial<Scan>[] = [{}, {}, {}, {}];
  // total bodies in the current system (FSSDiscovery event)
  let totalBodies = $state(0);
  let currentSystem = $state("");
  // accumulated list of bodies in the current system (Scan events)
  let scans = $state([] as Scan[]);
  let signals = $state([] as FSSBodySignals[]);

  const isHighValue = (body: Scan) => {
    if (body.starType) return false; // stars are usually low value unless rare, but sticking to planets
    const highValueClasses = [
      "Earthlike world",
      "Water world",
      "Ammonia world",
      "High metal content body",
      "Metal rich body"
    ];
    if (body.planetClass && highValueClasses.includes(body.planetClass)) {
        if (body.terraformState && body.terraformState !== "") return true;
        if (body.planetClass === "Earthlike world" || body.planetClass === "Ammonia world" || body.planetClass === "Water world") return true;
    }
    return false;
  };

  const getScanValue = (body: Scan) => {
    // Very simplified Elite Dangerous scan value formula
    // Base values (approximate)
    let baseValue = 0;
    if (body.starType) {
        baseValue = 1200;
    } else {
        switch (body.planetClass) {
            case "Earthlike world": baseValue = 64831; break;
            case "Ammonia world": baseValue = 33268; break;
            case "Water world": baseValue = 15557; break;
            case "High metal content body": baseValue = 14000; break;
            case "Metal rich body": baseValue = 30000; break;
            default: baseValue = 300; break;
        }
    }

    let modifier = 1;
    if (body.terraformState && body.terraformState !== "") modifier = 5;
    
    return Math.round(baseValue * modifier);
  };

  const totalSystemValue = $derived(scans.reduce((sum, body) => sum + getScanValue(body), 0));

  const highValueBodies = $derived(scans.filter(isHighValue));

  const targetEvents = ["Scan", "FSSScanBaryCenter", "FSSDiscoveryScan", "FSSAllBodiesFound"];

  $connection.on("JournalUpdated", (messages: JournalBase[]) => {
    const filtered = messages.filter((message) =>
      targetEvents.includes(message.event)
    );
    if (!filtered.length) return;

    for (let i = 0; i < filtered.length; i++) {
      const message = filtered[i];

      switch (message.event) {
        case "FSSDiscoveryScan": {
          // initial scan when jumping into a system
          const scan = message as FSSDiscoveryScan;
          totalBodies = scan.bodyCount;
          if (currentSystem !== scan.systemName) {
            scans = [];
            currentSystem = scan.systemName;
          }
          break;
        }
        case "Scan": {
          // contains all information about a scanned body (resources, biology, mapping/discovery status, body type, etc.)
          const scan = message as Scan;
          if (currentSystem !== scan.starSystem) {
            currentSystem = scan.starSystem;
            scans = [];
          }
          scans.push(scan);
          break;
        }
        case "FSSAllBodiesFound": {
          // when all bodies in a system have been scanned
          break;
        }
        default:
          console.log(message);
          break;
      }
    }
  });

  const toShortPlanetClass = (planetClass?: string) => {
    switch (planetClass) {
      case "High metal content":
      case "High metal content body":
        return "HMC";
      case "Metal rich body":
        return "MRB";
      case "Sudarsky class I gas giant":
      case "Sudarsky class II gas giant":
      case "Sudarsky class III gas giant":
      case "Sudarsky class IV gas giant":
        return "GAS";
      case "Icy body":
        return "ICE";
      case "Rocky body":
        return "ROC";
      default:
        return planetClass;
    }
  };
</script>

<section>
  <div class="header">
    <h1>Explorer</h1>
    <div class="stats">
      <span class="system">{currentSystem || "No System Data"}</span>
      <span class="count">Scanned: {scans.length} / {totalBodies}</span>
      <span class="value">Est. Value: {totalSystemValue.toLocaleString()} Cr</span>
    </div>
  </div>

  {#if highValueBodies.length > 0}
    <div class="high-value">
      <h2>High Value Targets</h2>
      <div class="body-list">
        {#each highValueBodies as body}
          <div class="body-item high">
            <span class="type">{toShortPlanetClass(body.planetClass)}</span>
            <span class="name">{body.bodyName}</span>
            <span class="dist">{(body.distanceFromArrivalLS ?? 0).toFixed(0)} Ls</span>
            <span class="cr">{getScanValue(body).toLocaleString()} Cr</span>
            {#if body.terraformState}
              <span class="terraform">Terraformable</span>
            {/if}
          </div>
        {/each}
      </div>
    </div>
  {/if}

  <div class="all-bodies">
    <h2>All Scanned Bodies</h2>
    <div class="body-list">
      {#each scans as body}
        <div class="body-item">
          <span class="type">{body.starType ?? toShortPlanetClass(body.planetClass)}</span>
          <span class="name">{body.bodyName}</span>
          <span class="dist">{(body.distanceFromArrivalLS ?? 0).toFixed(0)} Ls</span>
        </div>
      {/each}
    </div>
  </div>
</section>

<style lang="scss">
  section {
    display: flex;
    flex-direction: column;
    gap: 15px;
    max-height: 500px;
    overflow-y: auto;
  }

  .header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      h1 { border: none; margin: 0; }
  }

  .stats {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      .system { font-weight: bold; color: #fff; }
      .count { font-size: 0.8rem; color: #888; }
  }

  h2 {
      font-size: 1rem;
      border-bottom: 1px solid #333;
      margin-bottom: 8px;
      color: #888;
  }

  .body-list {
      display: flex;
      flex-direction: column;
      gap: 4px;
  }

  .body-item {
      display: grid;
      grid-template-columns: 60px 1fr 100px 100px;
      gap: 10px;
      padding: 4px 8px;
      background: rgba(255, 255, 255, 0.05);
      font-size: 0.9rem;
      align-items: center;

      &.high {
          background: rgba(255, 125, 0, 0.15);
          border-left: 3px solid var(--accent-color);
      }

      .type { color: #888; font-size: 0.8rem; }
      .name { font-weight: bold; }
      .dist { color: #666; font-size: 0.8rem; text-align: right; }
      .terraform { color: #00ff00; font-size: 0.7rem; text-transform: uppercase; }
  }
</style>
