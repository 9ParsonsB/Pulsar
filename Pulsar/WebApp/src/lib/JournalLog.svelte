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
  <div class="header">
    <h1>Live Journals</h1>
    <button
      onclick={() => {
        fetch("http://localhost:5000/api/journal");
      }}
    >
      Clear & Refresh
    </button>
  </div>
  
  <div class="log-container">
    {#each values as value (value.timestamp + value.event)}
      <div class="log-entry">
        <div class="meta">
          <span class="time">{new Date(value.timestamp).toLocaleTimeString()}</span>
          <span class="event">{value.event}</span>
        </div>
        <div class="details">
            {#if value.event === "FSSDiscoveryScan"}
                Bodies: {value.bodyCount}
            {:else if value.event === "Scan"}
                {value.bodyName} ({value.planetClass ?? value.starType})
            {:else if value.event === "FSDJump"}
                Jumped to {value.StarSystem}
            {:else}
                {JSON.stringify(value)}
            {/if}
        </div>
      </div>
    {/each}
  </div>
</section>

<style lang="scss">
  section {
    display: flex;
    flex-direction: column;
    gap: 10px;
    height: 500px;
  }

  .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      h1 { border: none; margin: 0; }
  }

  .log-container {
    flex: 1;
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    gap: 2px;
    background: rgba(0,0,0,0.2);
    border: 1px solid #333;
    padding: 5px;
  }

  .log-entry {
      padding: 4px 8px;
      border-bottom: 1px solid #222;
      font-size: 0.85rem;
      
      &:hover { background: rgba(255, 125, 0, 0.05); }

      .meta {
          display: flex;
          gap: 10px;
          margin-bottom: 2px;
          .time { color: #666; font-size: 0.75rem; }
          .event { color: var(--accent-color); font-weight: bold; font-size: 0.8rem; text-transform: uppercase; }
      }
      
      .details {
          color: #ccc;
          word-break: break-all;
          font-family: monospace;
      }
  }

  button {
    background: #3c1e05;
    color: var(--accent-color);
    border: 1px solid var(--border-color);
    padding: 4px 10px;
    cursor: pointer;
    font-size: 0.8rem;
    &:hover { background: var(--accent-color); color: #000; }
  }
</style>
