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
        <span>{$statusStore.event}</span>
        <span>{(($statusStore.fuel?.fuelMain ?? 0) / 32) * 100}%</span>
        <span>{$statusStore?.pips?.join(',')}</span>        
        <span>{$statusStore?.destination?.name}</span>
        <span>{$statusStore.guiFocus}</span>
        <span>{$statusStore.cargo}</span>   
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
