<script lang="ts">
    import {onMount} from "svelte";
    import connection from "$lib/stores/Connection.store";
    import type {FSSDiscoveryScan} from "../../types/api/FSSDiscoveryScan";
    import type {Scan} from "../../types/api/Scan";
    import type JournalBase from "../../types/api/JournalBase";
    import {
        getBodyValueLabel,
        getExplorationValue,
        isTerraformable,
        isValuableExplorerBody
    } from "$lib/explorationValue";

    const targetEvents = ["Scan", "FSSScanBaryCenter", "FSSDiscoveryScan", "FSSAllBodiesFound"];

    const planetClassOptions = [
        "Earthlike body",
        "Water world",
        "Ammonia world",
        "High metal content body",
        "Metal rich body",
        "Rocky body",
        "Icy body",
        "Rocky ice world",
        "Sudarsky class I gas giant",
        "Sudarsky class II gas giant",
        "Sudarsky class III gas giant",
        "Sudarsky class IV gas giant",
        "Sudarsky class V gas giant",
        "Helium gas giant",
        "Water giant",
        "Water giant with life",
        "Gas giant with water based life",
        "Gas giant with ammonia based life",
        "Class I gas giant",
        "Class II gas giant",
        "Class III gas giant",
        "Class IV gas giant",
        "Class V gas giant"
    ];

    const defaultCriteriaClasses = [
        "Earthlike body",
        "Water world",
        "Ammonia world",
        "High metal content body",
        "Metal rich body"
    ];
    // total bodies in the current system (FSSDiscovery event)
    let totalBodies = $state(0);
    let currentSystem = $state("");
    // accumulated list of bodies in the current system (Scan events)
    let scans = $state([] as Scan[]);
    let minimumSystemValue = $state(400000);
    let selectedPlanetClasses = $state([...defaultCriteriaClasses]);
    let includeTerraformable = $state(true);

    const criteriaClasses = $derived(selectedPlanetClasses);

    const isHighValue = (body: Scan) => {
        if (body.starType) return false;
        if (includeTerraformable && isTerraformable(body)) return true;
        if (!body.planetClass || !criteriaClasses.includes(body.planetClass)) return false;
        return isValuableExplorerBody(body, minimumSystemValue);
    };

    const totalSystemValue = $derived(scans.reduce((sum, body) => sum + getExplorationValue(body).scan, 0));
    const maxSystemValue = $derived(scans.reduce((sum, body) => sum + getExplorationValue(body).mapped, 0));

    const isHighSystemValue = $derived(maxSystemValue >= minimumSystemValue);

    const highValueBodies = $derived(scans
        .filter(isHighValue)
        .sort((left, right) => getExplorationValue(right).mapped - getExplorationValue(left).mapped));

    function saveCriteria() {
        if (typeof localStorage === "undefined") return;

        localStorage.setItem("pulsar.explorer.criteria", JSON.stringify({
            minimumSystemValue,
            selectedPlanetClasses,
            includeTerraformable
        }));
    }

    function togglePlanetClass(planetClass: string, selected: boolean) {
        selectedPlanetClasses = selected
            ? [...new Set([...selectedPlanetClasses, planetClass])]
            : selectedPlanetClasses.filter((value) => value !== planetClass);
        saveCriteria();
    }

    function selectDefaultPlanetClasses() {
        selectedPlanetClasses = [...defaultCriteriaClasses];
        saveCriteria();
    }

    function normalizePlanetClasses(classes?: string[]) {
        const normalized = classes
            ? classes
            .map((value) => value === "Earthlike world" ? "Earthlike body" : value)
            .filter((value) => planetClassOptions.includes(value))
            : [];
        return normalized.length ? normalized : undefined;
    }

    onMount(() => {
        const storedCriteria = localStorage.getItem("pulsar.explorer.criteria");
        if (storedCriteria) {
            const parsed = JSON.parse(storedCriteria) as Partial<{
                minimumSystemValue: number;
                criteriaClassesText: string;
                selectedPlanetClasses: string[];
                includeTerraformable: boolean;
            }>;
            minimumSystemValue = parsed.minimumSystemValue ?? minimumSystemValue;
            selectedPlanetClasses = normalizePlanetClasses(parsed.selectedPlanetClasses)
                ?? normalizePlanetClasses(parsed.criteriaClassesText?.split(",").map((value) => value.trim()))
                ?? selectedPlanetClasses;
            includeTerraformable = parsed.includeTerraformable ?? includeTerraformable;
        }

        const handler = (messages: unknown) => {
            const journals = messages as JournalBase[];
            const filtered = journals.filter((message) =>
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
        };

        $connection.on("JournalUpdated", handler);
        return () => {
            $connection.off("JournalUpdated", handler);
        };
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
            <span class="value" class:high={isHighSystemValue}>Scan: {totalSystemValue.toLocaleString()} Cr</span>
            <span class="value" class:high={isHighSystemValue}>Mapped: {maxSystemValue.toLocaleString()} Cr</span>
        </div>
    </div>

    <details class="criteria">
        <summary>Custom Criteria</summary>
        <label>
            <span>Mapped body value target</span>
            <input
                    min="0"
                    type="number"
                    value={minimumSystemValue}
                    onchange={(event) => {
                        minimumSystemValue = Number(event.currentTarget.value) || 0;
                        saveCriteria();
                    }}
            />
        </label>
        <div class="class-picker">
            <div class="label-row">
                <span>High-value planet classes</span>
                <button type="button" onclick={selectDefaultPlanetClasses}>Defaults</button>
            </div>
            <div class="class-grid">
                {#each planetClassOptions as planetClass}
                    <label class="checkbox">
                        <input
                                checked={selectedPlanetClasses.includes(planetClass)}
                                type="checkbox"
                                onchange={(event) => togglePlanetClass(planetClass, event.currentTarget.checked)}
                        />
                        <span>{planetClass}</span>
                    </label>
                {/each}
            </div>
        </div>
        <label class="checkbox">
            <input
                    checked={includeTerraformable}
                    type="checkbox"
                    onchange={(event) => {
                        includeTerraformable = event.currentTarget.checked;
                        saveCriteria();
                    }}
            />
            <span>Always flag terraformable bodies matching the selected classes</span>
        </label>
    </details>

    {#if highValueBodies.length > 0}
        <div class="high-value">
            <h2>High Value Targets</h2>
            <div class="body-list">
                {#each highValueBodies as body}
                    <div class="body-item high">
                        <span class="type">{toShortPlanetClass(body.planetClass)}</span>
                        <span class="name">{body.bodyName}</span>
                        <span class="dist">{(body.distanceFromArrivalLS ?? 0).toFixed(0)} Ls</span>
                        <span class="cr">{getBodyValueLabel(getExplorationValue(body))}</span>
                        <span class="tags">
                            {#if isTerraformable(body)}<span>Terraformable</span>{/if}
                            {#if body.wasDiscovered}<span>Discovered</span>{/if}
                            {#if body.wasMapped}<span>Mapped</span>{/if}
                            {#if body.scanType === "NavBeaconDetail"}<span>Nav beacon</span>{/if}
                        </span>
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
                    <span class="cr">{getExplorationValue(body).scan.toLocaleString()} Cr</span>
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

    h1 {
      border: none;
      margin: 0;
    }
  }

  .stats {
    display: flex;
    flex-direction: column;
    align-items: flex-end;

    .system {
      font-weight: bold;
      color: #fff;
    }

    .count {
      font-size: 0.8rem;
      color: #888;
    }

    .value.high {
      color: #00ff00;
      font-weight: bold;
    }
  }

  h2 {
    font-size: 1rem;
    border-bottom: 1px solid #333;
    margin-bottom: 8px;
    color: #888;
  }

  .criteria {
    border: 1px solid var(--border-color);
    background: rgba(255, 125, 0, 0.08);
    padding: 8px 10px;

    summary {
      cursor: pointer;
      color: #fff;
      font-weight: bold;
      text-transform: uppercase;
    }

    label {
      display: flex;
      flex-direction: column;
      gap: 4px;
      margin-top: 10px;
      color: #bbb;
      font-size: 0.8rem;
    }

    input[type="number"] {
      border: 1px solid var(--border-color);
      background: rgba(0, 0, 0, 0.35);
      color: var(--font-color-1);
      padding: 6px;
      font: inherit;
    }

    .class-picker {
      margin-top: 10px;
    }

    .label-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 10px;
      color: #bbb;
      font-size: 0.8rem;

      button {
        border: 1px solid var(--border-color);
        background: rgba(255, 125, 0, 0.12);
        color: var(--font-color-1);
        cursor: pointer;
        padding: 3px 8px;
        text-transform: uppercase;
      }
    }

    .class-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(210px, 1fr));
      gap: 4px 10px;
      margin-top: 6px;
      max-height: 160px;
      overflow-y: auto;
      padding-right: 4px;
    }

    .checkbox {
      flex-direction: row;
      align-items: center;
      margin-top: 4px;
    }
  }

  .body-list {
    display: flex;
    flex-direction: column;
    gap: 4px;
  }

  .body-item {
    display: grid;
    grid-template-columns: 70px 1fr 100px 150px 170px;
    gap: 10px;
    padding: 4px 8px;
    background: rgba(255, 255, 255, 0.05);
    font-size: 0.9rem;
    align-items: center;

    &.high {
      background: rgba(255, 125, 0, 0.15);
      border-left: 3px solid var(--accent-color);
    }

    .type {
      color: #888;
      font-size: 0.8rem;
    }

    .name {
      font-weight: bold;
    }

    .dist {
      color: #666;
      font-size: 0.8rem;
      text-align: right;
    }

    .tags {
      display: flex;
      flex-wrap: wrap;
      justify-content: flex-end;
      gap: 4px;
    }

    .tags span {
      border: 1px solid rgba(0, 255, 0, 0.35);
      border-radius: 999px;
      padding: 1px 6px;
      color: #00ff00;
      font-size: 0.7rem;
      text-transform: uppercase;
    }
  }
</style>
