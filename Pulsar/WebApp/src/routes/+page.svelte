<script lang="ts">
  import Status from "$lib/Status.svelte";
  import Ship from "$lib/Ship.svelte";
  import Debug from "$lib/Debug.svelte";
  import MissionStack from "$lib/MissionStack.svelte";
  import JournalLog from "$lib/JournalLog.svelte";
  import Explorer from "./explorer/Explorer.svelte";
</script>

<section>
  <div>
    <Status />
  </div>
  <div>
    <Explorer />
  </div>
  <div>
    <JournalLog />
  </div>
</section>

<style>
  div {
    border: 2px solid var(--border-color);
  }

  section {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
  }

  div {
    flex: 50%;
    max-width: 100%;
    height: 100%;
  }
</style>
