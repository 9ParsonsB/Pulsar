<script lang="ts">
    import Status from "$lib/Status.svelte";
    import Ship from "$lib/Ship.svelte";
    import Debug from "$lib/Debug.svelte";
    import MissionStack from "$lib/MissionStack.svelte"; 
    import JournalLog from "$lib/JournalLog.svelte";
</script>

<section>
    <div>
        <Status />
    </div>
    <div>
        <JournalLog />
    </div>
    <div>
        <!-- <Ship /> -->
    </div>
    <div>
        <!-- <MissionStack /> -->
    </div>
    <!--<div>
        <Debug />
    </div>-->
</section>

<style>
    div {
        border: 2px solid #d66325;
    }

    section {
        display: flex;
        flex-wrap: wrap;
        /* display: grid; */
        /* grid-template-columns: repeat(auto-fit, minmax(600px, 1fr)); */
        gap: 20px;

        /* display: flex; */
        /* flex-direction: column; */
        /* gap: 10px; */
    }

    div {
        flex: 50%;
        padding: 5px;
    }
</style>
