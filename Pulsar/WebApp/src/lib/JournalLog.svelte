<script lang="ts">
    import type {FSSDiscoveryScan} from "../types/api/FSSDiscoveryScan";
    import type JournalBase from "../types/api/JournalBase";
    import type {Scan} from "../types/api/Scan";
    import connection from "./stores/Connection.store";
    import {onMount} from "svelte";

    let values: JournalBase[] = $state([]);

    function isFSSDiscoveryScan(value: JournalBase): value is FSSDiscoveryScan {
        return value.event === "FSSDiscoveryScan";
    }

    function isScan(value: JournalBase): value is Scan {
        return value.event === "Scan";
    }

    function isFSDJump(value: JournalBase): value is JournalBase & { StarSystem: string } {
        return value.event === "FSDJump" && "StarSystem" in value;
    }

    function getKeyField(value: JournalBase, field: string): string | number | undefined {
        const journal = value as unknown as Record<string, string | number | undefined>;
        return journal[field] ?? journal[field.charAt(0).toLowerCase() + field.slice(1)];
    }

    function isDuplicateSupercruiseEntry(candidate: JournalBase): boolean {
        if (candidate.event !== "SupercruiseEntry") {
            return false;
        }

        return values.some((value) =>
            value.event === "SupercruiseEntry" &&
            value.timestamp === candidate.timestamp &&
            getKeyField(value, "SystemAddress") === getKeyField(candidate, "SystemAddress") &&
            getKeyField(value, "StarSystem") === getKeyField(candidate, "StarSystem"),
        );
    }

    function journalKey(value: JournalBase, index: number): string {
        const identity =
            getKeyField(value, "SystemAddress") ??
            getKeyField(value, "BodyID") ??
            getKeyField(value, "SystemBody") ??
            getKeyField(value, "BodyName") ??
            getKeyField(value, "StarSystem") ??
            index;

        return `${value.timestamp}|${value.event}|${identity}`;
    }

    onMount(() => {
        const handler = (journals: unknown) => {
            console.log(journals);
            const nextValues = (journals as JournalBase[]).filter((journal) => !isDuplicateSupercruiseEntry(journal));
            values.push(...nextValues);
            values.sort((a, b) => {
                // sort based on timestamp
                if (a.timestamp < b.timestamp) return 1;
                if (a.timestamp > b.timestamp) return -1;
                return 0;
            });
            if (values.length > 100) {
                values = values.slice(0, 100);
            }
        };

        $connection.on("JournalUpdated", handler);

        return () => {
            $connection.off("JournalUpdated", handler);
        };
    });
</script>

<section>
    <div class="header">
        <h1>Live Journals</h1>
        <button
                onclick={() => {
        fetch("/api/journal");
      }}
        >
            Clear & Refresh
        </button>
    </div>

    <div class="log-container">
        {#each values as value, index (journalKey(value, index))}
            <div class="log-entry">
                <div class="meta">
                    <span class="time">{new Date(value.timestamp).toLocaleTimeString()}</span>
                    <span class="event">{value.event}</span>
                </div>
                <div class="details">
                    {#if value.event === "FSSDiscoveryScan"}
                        {#if isFSSDiscoveryScan(value)}Bodies: {value.bodyCount}{/if}
                    {:else if value.event === "Scan"}
                        {#if isScan(value)}{value.bodyName} ({value.planetClass ?? value.starType}){/if}
                    {:else if value.event === "FSDJump"}
                        {#if isFSDJump(value)}Jumped to {value.StarSystem}{/if}
                    {:else}
                        {JSON.stringify(value)}
                    {/if}
                </div>
            </div>
        {/each}
    </div>
</section>

<style lang="scss">
  section {
    display: flex;
    flex-direction: column;
    gap: 10px;
    height: 500px;
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;

    h1 {
      border: none;
      margin: 0;
    }
  }

  .log-container {
    flex: 1;
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    gap: 2px;
    background: rgba(0, 0, 0, 0.2);
    border: 1px solid #333;
    padding: 5px;
  }

  .log-entry {
    padding: 4px 8px;
    border-bottom: 1px solid #222;
    font-size: 0.85rem;

    &:hover {
      background: rgba(255, 125, 0, 0.05);
    }

    .meta {
      display: flex;
      gap: 10px;
      margin-bottom: 2px;

      .time {
        color: #666;
        font-size: 0.75rem;
      }

      .event {
        color: var(--accent-color);
        font-weight: bold;
        font-size: 0.8rem;
        text-transform: uppercase;
      }
    }

    .details {
      color: #ccc;
      word-break: break-all;
      font-family: monospace;
    }
  }

  button {
    background: #3c1e05;
    color: var(--accent-color);
    border: 1px solid var(--border-color);
    padding: 4px 10px;
    cursor: pointer;
    font-size: 0.8rem;

    &:hover {
      background: var(--accent-color);
      color: #000;
    }
  }
</style>
