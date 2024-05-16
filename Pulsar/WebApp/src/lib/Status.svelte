<script lang="ts">
  import { onMount } from "svelte";
  import { statusStore } from "./stores/Status.store";
  import connection from "./stores/Connection.store";
  import { FocusStatus, StatusFlags, StatusFlags2 } from "../types/api/enums";
  import type JournalBase from "../types/api/JournalBase";
  import { fly, scale, slide } from "svelte/transition";
  import { getEnumFlags, getEnumNameFromValue, getEnumNamesFromFlag, getEnumPairsFromValue } from "../types/flags";
  import type Status from "../types/api/Status";
  import { HubConnectionState } from "@microsoft/signalr";

  const x: string | null = $state(null);

  const last: number[] = $state([]);
  let timeToMax = $state(0);

  let loading = $state(true);

  let alert: JournalBase[] = $state([]);

  onMount(async () => {

    if ($connection.state === HubConnectionState.Disconnected)
    {
      await $connection.start();
    }

    loading = false;

    $connection.on("StatusUpdated", (message) => {
      statusStore.update((s) => {
        return { ...s, ...message };
      });

      // only 3 in array
      if (last.length >= 3) {
        last.shift();
      }
      last.push(message.fuel?.fuelMain ?? 0);

      const change = [];
      for (let i = last.length - 1; i === 0; i--) {
        change.push(last[i] - last[i - 1]);
      }

      const avg = change.length ? change.reduce((a, b) => a + b, 0) / change.length : 0;
      const max = 32;
      if ($statusStore.fuel?.fuelMain && avg) {
        timeToMax = (max - $statusStore.fuel?.fuelMain) / avg;
      }

      console.log($statusStore);
    });

    $connection.on("JournalUpdated", (message) => {
      const journals = message as JournalBase[];
      const targetEvents = ["HullDamage", "UnderAttack"];
      const events = journals.filter((j) =>
        targetEvents.find((t) => t.toLowerCase() === j.event.toLowerCase())
      );
      if (events.length) {
        alert = events;
      }
    });

    if (!$statusStore.pips) {
      const value = (await (
        await fetch("http://localhost:5000/api/status")
      ).json()) as Status;

      console.log(value);

      statusStore.set({
        ...value,
      });
    }
  });
</script>

<h1>Status</h1>

<br />

{#if loading}
  <h1>LOADING ....</h1>
{/if}

{#if alert.length}
  <h1>Alert!</h1>

  {#each alert as a}
    <input readonly value={JSON.stringify(a)} />
  {/each}
  <button onclick={() => (alert = [])}>Clear</button>
{/if}

<div>
  {#if $statusStore}
    <span
      >Fuel%: {(($statusStore.fuel?.fuelMain ?? 0) / 32) * 100}% est: {timeToMax}s</span
    >
    <div class="power">
      <div class="sys">
        <div>{$statusStore?.pips?.sys ?? "?"}</div>
        <div>Sys</div>
        {#each [...Array($statusStore?.pips?.sys ?? 0)].map((_,i) => i) as sys (sys)}
          <div class="pip" transition:scale|global  ></div>
        {/each}
      </div>
      <div class="eng"> 
        <div>{$statusStore?.pips?.eng ?? "?"}</div> 
        <div>Eng</div>
        {#each [...Array($statusStore?.pips?.eng ?? 0)].map((_,i) => i) as eng (eng)}
          <div class="pip" transition:scale|global></div>
        {/each}
      </div>    
      <div class="wep"> 
        <div>{$statusStore?.pips?.wep ?? "?"}</div>
        <div>Wep</div>  
        {#each [...Array($statusStore?.pips?.wep ?? 0)].map((_,i) => i) as wep (wep)}
          <div class="pip" transition:scale|global></div>
        {/each}
      </div>
    </div>

    <span>dest?: {$statusStore?.destination?.name}</span>
    <span>gui focus: {getEnumNameFromValue(FocusStatus, $statusStore.guiFocus!)}</span>
    <span>cargo: {$statusStore.cargo}</span>
    <span>flag1: {getEnumNamesFromFlag(StatusFlags, $statusStore.flags!)}</span>
    <span>flag2: {getEnumNamesFromFlag(StatusFlags2,  $statusStore.flags2!)}</span>     
  {:else}
    <span>No data :(</span>
  {/if}
</div>

<style lang="scss">
  div {
    display: flex;
    flex-direction: column;
    height: 100%;
    .power {
      display: flex;
      flex-direction: row;
      height: 100%;
      align-items: flex-end;
      div {
        display: flex;
        height: 100%;
        flex-direction: column-reverse;
        align-items: center;
        div.pip {
          min-width: 2vw;
          min-height: 1vh;
          background-color: #d06527;
          border: 1px solid #96491c;
        }
      }
    }
  }

  input {
    width: 100%;
    background-color: transparent;
    color: white;
  }
</style>
