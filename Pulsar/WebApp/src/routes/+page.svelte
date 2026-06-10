<script lang="ts">
    import Status from "$lib/Status.svelte";
    import Ship from "$lib/Ship.svelte";
    import MissionStack from "$lib/MissionStack.svelte";
    import JournalLog from "$lib/JournalLog.svelte";
    import Explorer from "./explorer/Explorer.svelte";
</script>

<section>
    <Status/>
    <Explorer/>
    <JournalLog/>
    <Ship/>
    <MissionStack/>
</section>

<style>
    section {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(450px, 1fr));
        gap: 20px;
        padding: 20px;
    }

    section > :global(*) {
        min-height: 400px;
    }
</style>
