<script lang="ts">
  import connection from "$lib/stores/Connection.store";
  import type { FSSDiscoveryScan } from "../../types/api/FSSDiscoveryScan";
  import type { Scan } from "../../types/api/Scan";
  import type JournalBase from "../../types/api/JournalBase";

  const data: Partial<Scan>[] = [{}, {}, {}, {}];
  // total bodies in the current system (FSSDiscovery event)
  let totalBodies = $state(0);
  let currentSystem = $state("");
  // accumulated list of bodies in the current system (Scan events)
  let scans = $state([] as Scan[]);

  const targetEvents = ["Scan", "FSSScanBaryCenter", "FSSDiscoveryScan"];

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
      case "Sudarsky class I gas giant":
      case "Sudarsky class II gas giant":
      case "Sudarsky class III gas giant":
      case "Sudarsky class IV gas giant":
        return "GAS";
      default:
        return planetClass;
    }
  };
</script>

<section>
  <div class="title">
    <h1>Explorer</h1>
  </div>
  Current System: {currentSystem}
  <!-- summary & high value targets -->
  <h1>Bodies</h1>
  Scan:&nbsp;<span>{scans.length}</span>/<span>{totalBodies}</span>
  <div class="title">High Value (>500kcr)</div>
  <ol>
    <li>example</li>
    {#each scans as body}
      <li>
        [HMC/WW/ELT/ELN] {toShortPlanetClass(body.planetClass)}
        {body.bodyName} - 0cr
      </li>
    {/each}
  </ol>
  <br />
  <br />
  <!-- Full system data -->
  <div class="box">
    {#each data as row}
      <div class="group">
        <div class="summary">
          <span>Body Name: Farseer Inc</span>
          <div>
            <span>Signals: 1</span>
            <span>Base Value: 11111</span>
          </div>
        </div>

        <table class="details">
          <thead>
            <tr>
              <th>Flags</th>
              <th>Genus</th>
              <th>Species</th>
              <th>Seen</th>
              <th>Samples</th>
              <th>Type</th>
              <th>Possible Variants</th>
              <th>Base Value</th>
              <th>Distance</th>
            </tr>
          </thead>
          <tbody>
            {#each data as row}
              <tr>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>500m</td>
              </tr>
            {/each}
          </tbody>
        </table>
      </div>
    {/each}
  </div>
</section>

<style lang="scss">
  section {
    margin-top: 5px;
    max-height: 300px;
    overflow-y: scroll;
  }

  .title {
    padding-left: 5px;
    padding-right: 5px;
  }

  .box {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }

  .summary {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 50px;
    padding-top: 5px;
    padding-bottom: 5px;
    background-color: #c06400;
    color: var(--font-color-2);
    font-weight: bold;
    span {
      padding-left: 5px;
    }
    div {
      display: flex;
      gap: 20px;
    }
  }

  .details {
    border-collapse: collapse;
    border: none;
    table-layout: auto;
    width: 100%;
  }

  thead {
    background-color: #c06400;
  }

  tbody {
    tr:nth-child(even) {
      background-color: #301900;
      color: #cccccc;
    }
    tr:nth-child(odd) {
      background-color: #170c00;
      color: #cccccc;
    }
    tr:hover td {
      background-color: rgba(79, 42, 0, 0.85);
    }
    th {
      padding-top: 5px;
      padding-bottom: 5px;
      padding-left: 5px;
      padding-right: 5px;
      text-wrap: wrap;
      text-align: left;
      color: var(--font-color-2);
    }
  }

  th:not(:first-child) {
    text-align: center;
  }

  td:not(:first-child) {
    text-align: center;
  }
</style>
