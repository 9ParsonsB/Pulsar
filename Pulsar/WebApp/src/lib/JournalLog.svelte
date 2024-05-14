<script lang="ts">
  import type JournalBase from "../types/api/JournalBase";
  import connection from "./stores/Connection.store";

  const values: JournalBase[] = $state([]);

  $connection.on("JournalUpdated", (journals) => {
    console.log(journals);
    values.push(...(journals as JournalBase[]));
    values.sort((a, b) => {
      // sort based on timestamp
      if (a.timestamp < b.timestamp) return 1;
      if (a.timestamp > b.timestamp) return -1;
      return 0;
    });
  });
</script>

<section>
  <div class="title">
    <h1>Live Journals</h1>
  </div>
  <button
    onclick={() => {
      fetch("http://localhost:5000/api/journal");
    }}
  >
    Fetch All (debug)
  </button>
  <ul>
    {#each values as value (value.timestamp + value.event)}
      <li>
        <span class="time">{value.timestamp}</span>
        <span class="event">{value.event}</span>
        <input readonly value={JSON.stringify(value)} />
      </li>
    {/each}
  </ul>
</section>

<style lang="scss">
  section {
    margin-top: 5px;
    height: 500px;
    overflow-y: scroll;
  }

  .title {
    padding-left: 5px;
    padding-right: 5px;
    width: 50%;
    display: inline-block;
  }

  button {
    position: relative;
    display: inline-block;
    margin-left: 40%;
  }

  input {
    width: 100%;
    background-color: transparent;
    color: white;
  }
</style>
