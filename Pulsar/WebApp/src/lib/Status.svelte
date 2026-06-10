<script lang="ts">
    import {onMount} from "svelte";
    import {statusStore} from "./stores/Status.store";
    import connection from "./stores/Connection.store";
    import {FocusStatus, StatusFlags, StatusFlags2} from "../types/api/enums";
    import type JournalBase from "../types/api/JournalBase";
    import type Status from "../types/api/Status";
    import {getEnumNameFromValue, getEnumNamesFromFlag} from "../types/flags";
    import {HubConnectionState} from "@microsoft/signalr";
    import type {LoadGame} from "../types/api/LoadGame";
    import {IsLoadGameEvent} from "../types/api/LoadGame";

    const last: number[] = $state([]);
    let maxFuel: number = $state(32);
    let timeToMax = $state(0);

    let loading = $state(true);

    let alert: JournalBase[] = $state([]);
    let fuelDown = $state(false);

    let loadGame: Partial<LoadGame> = $state({});
    let location: Record<string, unknown> = $state({});

    function getJournalString(message: Record<string, unknown>, camelCase: string, pascalCase: string): string | undefined {
        const value = message[camelCase] ?? message[pascalCase];
        return typeof value === "string" && value.length > 0 ? value : undefined;
    }

    function applyLocation(message: Record<string, unknown>) {
        location = {
            ...location,
            ...message,
            starSystem: getJournalString(message, "starSystem", "StarSystem") ?? getJournalString(location, "starSystem", "StarSystem"),
            body: getJournalString(message, "body", "Body") ?? getJournalString(location, "body", "Body"),
            stationName: getJournalString(message, "stationName", "StationName") ?? getJournalString(location, "stationName", "StationName"),
            stationType: getJournalString(message, "stationType", "StationType") ?? getJournalString(location, "stationType", "StationType")
        };
    }

    function activePips(value: number | undefined): number {
        return Math.max(0, Math.min(8, Math.floor(value ?? 0)));
    }

    onMount(() => {
        loading = false;

        const statusHandler = (message: Partial<Status>) => {
            $statusStore = {...$statusStore, ...message};

            // only 3 in array
            if (last.length >= 3) {
                last.shift();
            }
            last.push(message.fuel?.fuelMain ?? 0);

            const change = [];
            for (let i = last.length - 1; i > 0; i--) {
                change.push(last[i] - last[i - 1]);
            }

            const avg = change.length
                ? change.reduce((a, b) => a + b, 0) / change.length
                : 0;
            const fuelMain = message.fuel?.fuelMain ?? 0;
            const currentEmpty = maxFuel - fuelMain;
            if (fuelMain && !Number.isNaN(avg) && avg) {
                fuelDown = avg < 0;
                timeToMax = fuelDown ? fuelMain / -avg : currentEmpty / avg;
            }

            console.log(message);
        };

        const journalHandler = (message: unknown) => {
            const journals = message as JournalBase[];
            const targetEvents = ["HullDamage", "UnderAttack"];
            const events = journals.filter((j) =>
                targetEvents.find((t) => t.toLowerCase() === j.event.toLowerCase()),
            );
            if (events.length) {
                alert = events;
            }

            journals.forEach((j) => {
                if (IsLoadGameEvent(j)) {
                    maxFuel = j.fuelCapacity;
                    $statusStore = {...$statusStore, fuel: {fuelMain: j.fuelLevel, fuelReservoir: 0}};
                    loadGame = j;
                }
                if (j.event === "Location" || j.event === "FSDJump") {
                    applyLocation(j as unknown as Record<string, unknown>);
                }
                if (j.event === "Docked" || j.event === "Undocked") {
                    applyLocation(j as unknown as Record<string, unknown>);
                }
            });
        };

        connection.hub.on("StatusUpdated", statusHandler);
        connection.hub.on("JournalUpdated", journalHandler);

        void (async () => {
            await connection.connect();

            if (!$statusStore.pips) {
                await connection.hub.invoke("Status");
            }
        })();

        return () => {
            connection.hub.off("StatusUpdated", statusHandler);
            connection.hub.off("JournalUpdated", journalHandler);
        };
    });
</script>

<section>
    <div class="header-row">
        <h1>Status</h1>
        <div class="connection-status" class:connected={$connection.state === HubConnectionState.Connected}>
            {$connection.state}
        </div>
    </div>

    {#if alert.length}
        <div class="alerts">
            <h2>Alerts!</h2>
            {#each alert as a}
                <div class="alert-item">{a.event}: {JSON.stringify(a)}</div>
            {/each}
            <button onclick={() => (alert = [])}>Clear</button>
        </div>
    {/if}

    <div class="info-grid">
        <div class="info-item">
            <span class="label">Commander:</span>
            <span class="value">{loadGame.commander ?? "---"}</span>
        </div>
        <div class="info-item">
            <span class="label">Ship:</span>
            <span class="value">{loadGame.ship_Localised ?? loadGame.ship ?? "---"}</span>
        </div>
        <div class="info-item">
            <span class="label">System:</span>
            <span class="value">{getJournalString(location, "starSystem", "StarSystem") ?? "---"}</span>
        </div>
        <div class="info-item">
            <span class="label">Body:</span>
            <span class="value">{getJournalString(location, "body", "Body") ?? "---"}</span>
        </div>
        {#if getJournalString(location, "stationName", "StationName")}
            <div class="info-item">
                <span class="label">Station:</span>
                <span class="value">{getJournalString(location, "stationName", "StationName")}</span>
            </div>
            <div class="info-item">
                <span class="label">Type:</span>
                <span class="value">{getJournalString(location, "stationType", "StationType") ?? "---"}</span>
            </div>
        {/if}
    </div>

    <div class="status-details">
        <div class="power">
            <div class="pip-group sys">
                <div class="pip-label">SYS</div>
                <div class="pip-value">{$statusStore?.pips?.sys ?? 0}</div>
                <div class="pips">
                    {#each [...Array(8)].map((_, i) => i) as i}
                        <div class="pip" class:active={i < activePips($statusStore?.pips?.sys)}></div>
                    {/each}
                </div>
            </div>
            <div class="pip-group eng">
                <div class="pip-label">ENG</div>
                <div class="pip-value">{$statusStore?.pips?.eng ?? 0}</div>
                <div class="pips">
                    {#each [...Array(8)].map((_, i) => i) as i}
                        <div class="pip" class:active={i < activePips($statusStore?.pips?.eng)}></div>
                    {/each}
                </div>
            </div>
            <div class="pip-group wep">
                <div class="pip-label">WEP</div>
                <div class="pip-value">{$statusStore?.pips?.wep ?? 0}</div>
                <div class="pips">
                    {#each [...Array(8)].map((_, i) => i) as i}
                        <div class="pip" class:active={i < activePips($statusStore?.pips?.wep)}></div>
                    {/each}
                </div>
            </div>
        </div>

        <div class="other-info">
            <div><span class="label">Focus:</span> {getEnumNameFromValue(FocusStatus, $statusStore.guiFocus ?? 0)}</div>
            <div><span class="label">Cargo:</span> {$statusStore.cargo ?? 0}</div>
            {#if $statusStore.destination?.name}
                <div><span class="label">Dest:</span> {$statusStore.destination.name}</div>
            {/if}
        </div>

        <div class="flags">
            {#each getEnumNamesFromFlag(StatusFlags, $statusStore.flags ?? 0) as flag}
                <span class="flag">{flag}</span>
            {/each}
            {#each getEnumNamesFromFlag(StatusFlags2, $statusStore.flags2 ?? 0) as flag}
                <span class="flag secondary">{flag}</span>
            {/each}
        </div>
    </div>
</section>

<style lang="scss">
  section {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .header-row {
    display: flex;
    justify-content: space-between;
    align-items: center;

    h1 {
      border: none;
      margin: 0;
    }
  }

  .connection-status {
    font-size: 0.8rem;
    padding: 2px 8px;
    border: 1px solid #444;

    &.connected {
      color: #00ff00;
      border-color: #00ff00;
    }
  }

  .info-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 10px;
    font-size: 0.9rem;

    .info-item {
      display: flex;
      gap: 10px;

      .label {
        color: #888;
      }

      .value {
        color: #fff;
        font-weight: bold;
      }
    }
  }

  .fuel-section {
    margin-bottom: 10px;

    .fuel-bar-container {
      height: 10px;
      background: #222;
      border: 1px solid var(--border-color);
      margin: 5px 0;

      .fuel-bar {
        height: 100%;
        background: #ff7d00;
        transition: width 0.3s;
      }
    }

    .fuel-info {
      display: flex;
      justify-content: space-between;
      font-size: 0.8rem;

      .scooping {
        color: #00ff00;
      }
    }
  }

  .power {
    display: flex;
    justify-content: space-between;
    background: rgba(0, 0, 0, 0.3);
    padding: 10px;
    border: 1px solid #333;

    .pip-group {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 5px;

      .pip-label {
        font-size: 0.7rem;
        color: #888;
      }

      .pip-value {
        font-size: 1.2rem;
        font-weight: bold;
      }

      .pips {
        display: flex;
        flex-direction: column-reverse;
        gap: 2px;

        .pip {
          width: 20px;
          height: 4px;
          background: #222;
          border: 1px solid #333;

          &.active {
            background: var(--accent-color);
            box-shadow: 0 0 5px var(--accent-color);
          }
        }
      }
    }
  }

  .other-info {
    font-size: 0.9rem;

    .label {
      color: #888;
    }
  }

  .flags {
    display: flex;
    flex-wrap: wrap;
    gap: 5px;

    .flag {
      font-size: 0.7rem;
      padding: 2px 6px;
      background: #3c1e05;
      color: #ff7d00;
      border: 1px solid #5f3100;

      &.secondary {
        color: #00ccff;
        border-color: #004466;
      }
    }
  }

  .alerts {
    background: rgba(255, 0, 0, 0.2);
    border: 1px solid red;
    padding: 10px;

    .alert-item {
      font-size: 0.8rem;
      margin-bottom: 5px;
    }
  }
</style>
