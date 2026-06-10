<script lang="ts">
    import {onMount} from "svelte";
    import {statusStore} from "./stores/Status.store";
    import connection from "./stores/Connection.store";
    import {StatusFlags, StatusFlags2} from "../types/api/enums";
    import type JournalBase from "../types/api/JournalBase";
    import type Status from "../types/api/Status";
    import {IsLoadGameEvent} from "../types/api/LoadGame";

    let maxFuel: number | undefined = $state();
    const last: number[] = $state([]);
    let timeToMax = $state(0);
    let fuelDown = $state(false);
    let fuelDeltaPercent = $state<number | null>(null);
    let fuelDeltaVisible = $state(false);
    let fuelDeltaPositive = $state(false);
    let fuelDeltaTimer: ReturnType<typeof setTimeout> | undefined;

    function formatFuelTime(seconds: number): string {
        if (!Number.isFinite(seconds) || seconds <= 0) {
            return "00:00";
        }

        const totalSeconds = Math.round(seconds);
        const minutes = Math.floor(totalSeconds / 60);
        const remainingSeconds = totalSeconds % 60;

        return `${minutes.toString().padStart(2, "0")}:${remainingSeconds.toString().padStart(2, "0")}`;
    }

    onMount(() => {
        const statusHandler = (message: Partial<Status>) => {
            const previousFuel = last.at(-1);

            if (last.length >= 3) {
                last.shift();
            }

            const fuelMain = message.fuel?.fuelMain ?? 0;
            last.push(fuelMain);

            if (maxFuel && previousFuel !== undefined) {
                const fuelDelta = fuelMain - previousFuel;
                if (fuelDelta !== 0) {
                    fuelDeltaPercent = (fuelDelta / maxFuel) * 100;
                    fuelDeltaPositive = fuelDelta > 0;
                    fuelDeltaVisible = false;
                    if (fuelDeltaTimer) {
                        clearTimeout(fuelDeltaTimer);
                    }

                    requestAnimationFrame(() => {
                        fuelDeltaVisible = true;
                    });

                    fuelDeltaTimer = setTimeout(() => {
                        fuelDeltaVisible = false;
                        fuelDeltaTimer = setTimeout(() => {
                            fuelDeltaPercent = null;
                            fuelDeltaTimer = undefined;
                        }, 320);
                    }, 1400);
                }
            }

            const change = [];
            for (let i = last.length - 1; i > 0; i--) {
                change.push(last[i] - last[i - 1]);
            }

            const avg = change.length
                ? change.reduce((a, b) => a + b, 0) / change.length
                : 0;

            const currentEmpty = (maxFuel ?? fuelMain) - fuelMain;
            if (maxFuel && fuelMain && !Number.isNaN(avg) && avg) {
                fuelDown = avg < 0;
                timeToMax = fuelDown ? fuelMain / -avg : currentEmpty / avg;
            }
        };

        const journalHandler = (message: unknown) => {
            const journals = message as JournalBase[];
            journals.forEach((j) => {
                if (IsLoadGameEvent(j)) {
                    maxFuel = j.fuelCapacity;
                }
            });
        };

        connection.hub.on("StatusUpdated", statusHandler);
        connection.hub.on("JournalUpdated", journalHandler);

        return () => {
            if (fuelDeltaTimer) {
                clearTimeout(fuelDeltaTimer);
            }

            connection.hub.off("StatusUpdated", statusHandler);
            connection.hub.off("JournalUpdated", journalHandler);
        };
    });

    const fuelPercent = $derived.by(() => {
        if (!maxFuel || maxFuel <= 0) {
            return undefined;
        }

        return (($statusStore.fuel?.fuelMain ?? 0) / maxFuel) * 100;
    });
    const isLowFuel = $derived($statusStore.flags !== undefined && ($statusStore.flags & StatusFlags.LowFuel) !== 0);
    const isScoActive = $derived($statusStore.flags2 !== undefined && ($statusStore.flags2 & StatusFlags2.SuperCruiseOverdriveActive) !== 0);
    const isFuelScooping = $derived($statusStore.flags !== undefined && ($statusStore.flags & StatusFlags.FuelScooping) !== 0);
</script>

<div class="fuel-container" class:low={isLowFuel}>
    <div class="label">Fuel</div>
    <div class="fuel-bar-container">
        <div
                class="fuel-bar"
                class:warning={isLowFuel}
                style="width: {fuelPercent ?? 0}%"
        ></div>
    </div>
    <div class="fuel-info">
        <div class="fuel-percent">
            <span class:warning={isLowFuel}>{fuelPercent !== undefined ? `${fuelPercent.toFixed(1)}%` : "--"}</span>
            {#if fuelDeltaPercent !== null}
                <span
                        class="fuel-delta"
                        class:visible={fuelDeltaVisible}
                        class:positive={fuelDeltaPositive}
                        class:negative={!fuelDeltaPositive}
                >
                    {fuelDeltaPositive ? "+" : ""}{fuelDeltaPercent.toFixed(1)}%
                </span>
            {/if}
        </div>
        {#if maxFuel && (isFuelScooping || isScoActive)}
            <span class="scooping">{isScoActive ? 'Super Cruise Overdrive' : 'Scooping'}: {formatFuelTime(timeToMax)} {fuelDown ? 'till Empty' : 'to fill'}</span>
        {/if}
        {#if isLowFuel}
            <span class="warning-text">LOW FUEL WARNING</span>
        {/if}
    </div>
</div>

<style lang="scss">
  .fuel-container {
    display: flex;
    flex-direction: column;
    width: 100%;
    max-width: 400px;
    padding: 5px;

    &.low {
      animation: pulse 2s infinite;
    }
  }

  .label {
    font-size: 0.8rem;
    color: var(--font-color-1);
    text-transform: uppercase;
    font-weight: bold;
  }

  .fuel-bar-container {
    height: 12px;
    background: #222;
    border: 1px solid var(--border-color);
    margin: 2px 0;
    overflow: hidden;

    .fuel-bar {
      height: 100%;
      background: #ff7d00;
      transition: width 0.3s ease-out;

      &.warning {
        background: #ff0000;
      }
    }
  }

  .fuel-info {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    font-size: 0.8rem;
    font-weight: bold;

    .fuel-percent {
      position: relative;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      min-width: 4.5rem;
    }

    .fuel-delta {
      margin-top: 0.15rem;
      font-size: 0.72rem;
      line-height: 1;
      opacity: 0;
      transform: translateY(-4px);
      transition: opacity 180ms ease, transform 320ms ease;

      &.visible {
        opacity: 1;
        transform: translateY(0);
      }

      &.positive {
        color: #00ff90;
      }

      &.negative {
        color: #ff7d7d;
      }
    }

    .scooping {
      color: #00ff00;
    }

    .warning-text {
      color: #ff0000;
      animation: flash 1s infinite;
    }

    .warning {
      color: #ff0000;
    }
  }

  @keyframes pulse {
    0% {
      background-color: transparent;
    }
    50% {
      background-color: rgba(255, 0, 0, 0.1);
    }
    100% {
      background-color: transparent;
    }
  }

  @keyframes flash {
    0% {
      opacity: 1;
    }
    50% {
      opacity: 0.3;
    }
    100% {
      opacity: 1;
    }
  }
</style>
