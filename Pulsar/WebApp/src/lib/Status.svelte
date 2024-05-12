<script lang="ts">
  import { onMount } from "svelte";
  import { statusStore } from "./stores/Status.store";
  import connection from "./stores/Connection.store";

  const x: string | null = $state(null);

  onMount(async () => {
    await $connection.start();

    $connection.on("StatusUpdated", (message) => {
      statusStore.update((s) => {
        return { ...s, ...message };
      });
      console.log($statusStore);
    });
  });
</script>

<h1>Status</h1>

<br />

<div>
  {#if $statusStore}
    <span>Fuel%: {(($statusStore.fuel?.fuelMain ?? 0) / 32) * 100}%</span>
    <span
      >Sys: {$statusStore?.pips?.sys ?? "?"} Eng: {$statusStore?.pips?.eng ??
        "?"} Wep:
      {$statusStore?.pips?.wep ?? "?"}</span
    >
    <span>dest?: {$statusStore?.destination?.name}</span>
    <span>gui focus: {$statusStore.guiFocus}</span>
    <span>cargo: {$statusStore.cargo}</span>
    <span>flag1: {$statusStore.flags}</span>
    <span>flag2: {$statusStore.flags2}</span>
  {:else}
    <span>No data :(</span>
  {/if}
</div>

<style>
  div {
    display: flex;
    flex-direction: column;
  }
</style>
