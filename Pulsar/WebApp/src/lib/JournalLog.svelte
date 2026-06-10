<script lang="ts">
    import type {FSSDiscoveryScan} from "../types/api/FSSDiscoveryScan";
    import type JournalBase from "../types/api/JournalBase";
    import type {Scan} from "../types/api/Scan";
    import type {FSSBodySignals, SAASignalsFound, Signal} from "../types/api/Signals";
    import connection from "./stores/Connection.store";
    import {onMount} from "svelte";

    type JournalEntry = {
        key: string;
        fingerprint: string;
        value: JournalBase;
    };

    const MAX_ENTRIES = 100;

    let values: JournalEntry[] = $state([]);
    let nextKey = 0;

    function isFSSDiscoveryScan(value: JournalBase): value is FSSDiscoveryScan {
        return value.event === "FSSDiscoveryScan";
    }

    function isScan(value: JournalBase): value is Scan {
        return value.event === "Scan";
    }

    function isFSSAllBodiesFound(value: JournalBase): value is JournalBase & {
        SystemName?: string;
        systemName?: string;
        Count?: number;
        count?: number;
    } {
        return value.event === "FSSAllBodiesFound";
    }

    function isFSSSignalDiscovered(value: JournalBase): value is JournalBase & Record<string, unknown> {
        return value.event === "FSSSignalDiscovered";
    }

    function isBodySignals(value: JournalBase): value is FSSBodySignals | SAASignalsFound {
        return value.event === "FSSBodySignals" || value.event === "SAASignalsFound";
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
            value.value.event === "SupercruiseEntry" &&
            value.value.timestamp === candidate.timestamp &&
            getKeyField(value.value, "SystemAddress") === getKeyField(candidate, "SystemAddress") &&
            getKeyField(value.value, "StarSystem") === getKeyField(candidate, "StarSystem"),
        );
    }

    function stableSerialize(value: unknown): string {
        if (value === null || typeof value !== "object") {
            return JSON.stringify(value);
        }

        if (Array.isArray(value)) {
            return `[${value.map(stableSerialize).join(",")}]`;
        }

        const entries = Object.entries(value as Record<string, unknown>)
            .sort(([left], [right]) => left.localeCompare(right))
            .map(([key, nested]) => `${JSON.stringify(key)}:${stableSerialize(nested)}`);

        return `{${entries.join(",")}}`;
    }

    function journalFingerprint(value: JournalBase): string {
        return stableSerialize(value);
    }

    function appendJournals(journals: JournalBase[]): void {
        const seen = new Set(values.map((entry) => entry.fingerprint));
        const additions: JournalEntry[] = [];

        for (const journal of journals) {
            if (isDuplicateSupercruiseEntry(journal)) {
                continue;
            }

            const fingerprint = journalFingerprint(journal);
            if (seen.has(fingerprint)) {
                continue;
            }

            seen.add(fingerprint);
            additions.push({
                key: `journal-${nextKey++}`,
                fingerprint,
                value: journal,
            });
        }

        if (additions.length === 0) {
            return;
        }

        values.push(...additions);
        values.sort((a, b) => {
            if (a.value.timestamp < b.value.timestamp) return 1;
            if (a.value.timestamp > b.value.timestamp) return -1;
            return 0;
        });

        if (values.length > MAX_ENTRIES) {
            values = values.slice(0, MAX_ENTRIES);
        }
    }

    function formatSignal(signal: Signal): string {
        const label = signal.type_Localised || signal.type.replaceAll("$", "").replaceAll(";", "");
        return `${signal.count} ${label}`;
    }

    function formatBodySignals(value: FSSBodySignals | SAASignalsFound): string {
        const totalSignals = value.Signals.reduce((total, signal) => total + signal.count, 0);
        const signals = value.Signals.length ? value.Signals.map(formatSignal).join(", ") : "No signals";
        const genuses = value.Genuses.length
            ? ` | Biology: ${value.Genuses.map((genus) => genus.Genus_Localised || genus.Genus).join(", ")}`
            : "";
        return `${value.BodyName}: ${totalSignals} signal${totalSignals === 1 ? "" : "s"} | ${signals}${genuses}`;
    }

    function formatScan(value: Scan): string {
        const bodyType = value.planetClass ?? value.starType ?? "Unknown body";
        const scanMode = value.scanType ? ` via ${value.scanType}` : "";
        const distance = value.distanceFromArrivalLS != null ? ` | ${value.distanceFromArrivalLS.toFixed(0)} Ls` : "";
        const terraformable = value.terraformState ? ` | ${value.terraformState}` : "";
        return `${value.bodyName}: ${bodyType}${scanMode}${distance}${terraformable}`;
    }

    function formatFSSSignalDiscovered(value: Record<string, unknown>): string {
        const signalName = (value.SignalName_Localised as string) || (value.signalName_Localised as string) ||
            (value.SignalName as string) || (value.signalName as string) || "Signal";
        const signalType = (value.USSType_Localised as string) || (value.uSSType_Localised as string) ||
            (value.SignalType as string) || (value.signalType as string);
        const threat = (value.ThreatLevel as number | undefined) ?? (value.threatLevel as number | undefined);
        const faction = (value.SpawningFaction_Localised as string) || (value.spawningFaction_Localised as string);
        const remaining = (value.TimeRemaining as number | undefined) ?? (value.timeRemaining as number | undefined);
        const isStation = Boolean(value.IsStation ?? value.isStation);

        const parts = [signalName];
        if (signalType) parts.push(signalType);
        if (threat != null && threat > 0) parts.push(`Threat ${threat}`);
        if (faction) parts.push(faction);
        if (remaining != null && remaining > 0) parts.push(`${Math.round(remaining / 60)}m remaining`);
        if (isStation) parts.push("Station");

        return parts.join(" | ");
    }

    onMount(() => {
        const handler = (journals: unknown) => {
            appendJournals(journals as JournalBase[]);
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
        {#each values as entry (entry.key)}
            <div class="log-entry">
                <div class="meta">
                    <span class="time">{new Date(entry.value.timestamp).toLocaleTimeString()}</span>
                    <span class="event">{entry.value.event}</span>
                </div>
                <div class="details">
                    {#if entry.value.event === "FSSDiscoveryScan"}
                        {#if isFSSDiscoveryScan(entry.value)}{entry.value.systemName}: {entry.value.bodyCount} bodies, {entry.value.nonBodyCount} signals, {(entry.value.progress * 100).toFixed(0)}%{/if}
                    {:else if entry.value.event === "FSSAllBodiesFound"}
                        {#if isFSSAllBodiesFound(entry.value)}{getKeyField(entry.value, "SystemName")}: all {getKeyField(entry.value, "Count")} bodies identified{/if}
                    {:else if entry.value.event === "FSSBodySignals" || entry.value.event === "SAASignalsFound"}
                        {#if isBodySignals(entry.value)}{formatBodySignals(entry.value)}{/if}
                    {:else if entry.value.event === "FSSSignalDiscovered"}
                        {#if isFSSSignalDiscovered(entry.value)}{formatFSSSignalDiscovered(entry.value)}{/if}
                    {:else if entry.value.event === "Scan"}
                        {#if isScan(entry.value)}{formatScan(entry.value)}{/if}
                    {:else if entry.value.event === "FSDJump"}
                        {#if isFSDJump(entry.value)}Jumped to {entry.value.StarSystem}{/if}
                    {:else}
                        {JSON.stringify(entry.value)}
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
