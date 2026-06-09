<script lang="ts">
import { onMount } from "svelte";
import { statusStore } from "./stores/Status.store";
import connection from "./stores/Connection.store";
import { FocusStatus, StatusFlags, StatusFlags2 } from "../types/api/enums";
import type JournalBase from "../types/api/JournalBase";
import { scale } from "svelte/transition";
import { getEnumNameFromValue, getEnumNamesFromFlag } from "../types/flags";
import { HubConnectionState } from "@microsoft/signalr";
import { IsLoadGameEvent } from "../types/api/LoadGame";

import type { LoadGame } from "../types/api/LoadGame";

const last: number[] = $state([]);
let maxFuel: number = $state(32);
let timeToMax = $state(0);

let loading = $state(true);

let alert: JournalBase[] = $state([]);
let fuelDown = $state(false);

let loadGame: Partial<LoadGame> = $state({});
let location: any = $state({});

onMount(async () => {
	loading = false;

	$connection.on("StatusUpdated", (message) => {
		$statusStore = { ...$statusStore, ...message };

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
		const currentEmpty = maxFuel - message.fuel?.fuelMain;
		if (message.fuel?.fuelMain && !Number.isNaN(avg) && avg) {
			fuelDown = avg < 0;
			timeToMax = fuelDown ? message.fuel?.fuelMain / -avg : currentEmpty / avg;
		}

		console.log(message);
	});

	$connection.on("JournalUpdated", (message) => {
		const journals = message as JournalBase[];
		const targetEvents = ["HullDamage", "UnderAttack"];
		const events = journals.filter((j) =>
			targetEvents.find((t) => t.toLowerCase() === j.event.toLowerCase()),
		);
		if (events.length) {
			alert = events;
		}
		
		journals.forEach(j => {
			if (IsLoadGameEvent(j)) {
				maxFuel = j.fuelCapacity;
				$statusStore.fuel = { fuelMain: j.fuelLevel, fuelReservoir: 0 };
				loadGame = j;
			}
			if (j.event === "Location" || j.event === "FSDJump") {
				location = j;
			}
			if (j.event === "Docked" || j.event === "Undocked") {
				location = { ...location, ...j };
			}
		});
	});

	if ($connection.state === HubConnectionState.Disconnected) {
		await $connection.start();
	}

	if (!$statusStore.pips) {
		await $connection.invoke("Status");
	}
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
      <span class="value">{location.StarSystem ?? "---"}</span>
    </div>
    <div class="info-item">
      <span class="label">Body:</span>
      <span class="value">{location.Body ?? "---"}</span>
    </div>
    {#if location.StationName}
    <div class="info-item">
      <span class="label">Station:</span>
      <span class="value">{location.StationName}</span>
    </div>
    <div class="info-item">
      <span class="label">Type:</span>
      <span class="value">{location.StationType ?? "---"}</span>
    </div>
    {/if}
  </div>

  <div class="status-details">
    <div class="fuel-section">
      <div class="label">Fuel</div>
      <div class="fuel-bar-container">
        <div class="fuel-bar" style="width: {(($statusStore.fuel?.fuelMain ?? 0) / maxFuel) * 100}%"></div>
      </div>
      <div class="fuel-info">
        <span>{((($statusStore.fuel?.fuelMain ?? 0) / maxFuel) * 100).toFixed(1)}%</span>
        {#if $statusStore.flags! & StatusFlags.FuelScooping}
          <span class="scooping">Scooping: {timeToMax.toFixed(0)}s {fuelDown ? 'rem' : 'to fill'}</span>
        {/if}
      </div>
    </div>

    <div class="power">
      <div class="pip-group sys">
        <div class="pip-label">SYS</div>
        <div class="pip-value">{$statusStore?.pips?.sys ?? 0}</div>
        <div class="pips">
          {#each [...Array(8)].map((_,i) => i) as i}
            <div class="pip" class:active={i < ($statusStore?.pips?.sys ?? 0) * 2}></div>
          {/each}
        </div>
      </div>
      <div class="pip-group eng">
        <div class="pip-label">ENG</div>
        <div class="pip-value">{$statusStore?.pips?.eng ?? 0}</div>
        <div class="pips">
          {#each [...Array(8)].map((_,i) => i) as i}
            <div class="pip" class:active={i < ($statusStore?.pips?.eng ?? 0) * 2}></div>
          {/each}
        </div>
      </div>
      <div class="pip-group wep">
        <div class="pip-label">WEP</div>
        <div class="pip-value">{$statusStore?.pips?.wep ?? 0}</div>
        <div class="pips">
          {#each [...Array(8)].map((_,i) => i) as i}
            <div class="pip" class:active={i < ($statusStore?.pips?.wep ?? 0) * 2}></div>
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
      h1 { border: none; margin: 0; }
  }

  .connection-status {
      font-size: 0.8rem;
      padding: 2px 8px;
      border: 1px solid #444;
      &.connected { color: #00ff00; border-color: #00ff00; }
  }

  .info-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 10px;
      font-size: 0.9rem;
      .info-item {
          display: flex;
          gap: 10px;
          .label { color: #888; }
          .value { color: #fff; font-weight: bold; }
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
          .scooping { color: #00ff00; }
      }
  }

  .power {
    display: flex;
    justify-content: space-between;
    background: rgba(0,0,0,0.3);
    padding: 10px;
    border: 1px solid #333;

    .pip-group {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 5px;
        .pip-label { font-size: 0.7rem; color: #888; }
        .pip-value { font-size: 1.2rem; font-weight: bold; }
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
      .label { color: #888; }
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
      .alert-item { font-size: 0.8rem; margin-bottom: 5px; }
  }
</style>
