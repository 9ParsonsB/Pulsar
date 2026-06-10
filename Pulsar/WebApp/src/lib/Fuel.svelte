<script lang="ts">
    import {onMount} from "svelte";
    import {statusStore} from "./stores/Status.store";
    import connection from "./stores/Connection.store";
    import {StatusFlags} from "../types/api/enums";
    import type JournalBase from "../types/api/JournalBase";
    import type Status from "../types/api/Status";
    import {IsLoadGameEvent} from "../types/api/LoadGame";

    let maxFuel: number = $state(32);
    const last: number[] = $state([]);
    let timeToMax = $state(0);
    let fuelDown = $state(false);

    onMount(() => {
        const statusHandler = (message: Partial<Status>) => {
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

            const currentEmpty = maxFuel - (message.fuel?.fuelMain ?? 0);
            if (message.fuel?.fuelMain && !Number.isNaN(avg) && avg) {
                fuelDown = avg < 0;
                timeToMax = fuelDown ? message.fuel?.fuelMain / -avg : currentEmpty / avg;
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
            connection.hub.off("StatusUpdated", statusHandler);
            connection.hub.off("JournalUpdated", journalHandler);
        };
    });

    const fuelPercent = $derived((($statusStore.fuel?.fuelMain ?? 0) / maxFuel) * 100);
    const isLowFuel = $derived($statusStore.flags !== undefined && ($statusStore.flags & StatusFlags.LowFuel) !== 0);
</script>

<div class="fuel-container" class:low={isLowFuel}>
    <div class="label">Fuel</div>
    <div class="fuel-bar-container">
        <div
                class="fuel-bar"
                class:warning={isLowFuel}
                style="width: {fuelPercent}%"
        ></div>
    </div>
    <div class="fuel-info">
        <span class:warning={isLowFuel}>{fuelPercent.toFixed(1)}%</span>
        {#if $statusStore.flags! & StatusFlags.FuelScooping}
            <span class="scooping">Scooping: {timeToMax.toFixed(0)}s {fuelDown ? 'rem' : 'to fill'}</span>
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
    font-size: 0.8rem;
    font-weight: bold;

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
