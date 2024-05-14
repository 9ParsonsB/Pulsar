<script lang="ts">
  const data: unknown[] = [{}, {}, {}, {}];
  // number of scans completed
  const scanned = 2;
  // total bodies in the current system (FSSDiscovery event)
  const totalBodies = 12;
  // accumulated list of bodies in the current system (Scan events)
  const bodies = $state([
    { value: 50 },
    { value: 1000 },
    { value: 800000 },
    { value: 800000 },
  ]);
</script>

<section>
  <div class="title">
    <h1>Explorer</h1>
  </div>
  <!-- summary & high value targets -->
  <h1>Bodies</h1>
  Scan:&nbsp;<span>{scanned}</span>/<span>{totalBodies}</span>
  <div class="title">High Value (>500kcr)</div>
  <ol>
    {#each bodies.filter((b) => b.value > 500000) as body}
      <li>[HMC/WW/ELT/ELN] $body.name - {body.value}cr</li>
    {/each}
  </ol>
  <br />
  <br />
  <!-- Full system data -->
  <div class="box">
    {#each data as row}
      <div class="group">
        <div class="summary">
          <span>Body Name: Farseer Inc</span>
          <div>
            <span>Signals: 1</span>
            <span>Base Value: 11111</span>
          </div>
        </div>

        <table class="details">
          <thead>
            <tr>
              <th>Flags</th>
              <th>Genus</th>
              <th>Species</th>
              <th>Seen</th>
              <th>Samples</th>
              <th>Type</th>
              <th>Possible Variants</th>
              <th>Base Value</th>
              <th>Distance</th>
            </tr>
          </thead>
          <tbody>
            {#each data as row}
              <tr>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>Test</td>
                <td>500m</td>
              </tr>
            {/each}
          </tbody>
        </table>
      </div>
    {/each}
  </div>
</section>

<style lang="scss">
  section {
    margin-top: 5px;
    max-height: 500px;
    overflow-y: scroll;
  }

  .title {
    padding-left: 5px;
    padding-right: 5px;
  }

  .box {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }

  .summary {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 50px;
    padding-top: 5px;
    padding-bottom: 5px;
    background-color: #c06400;
    color: var(--font-color-2);
    font-weight: bold;
    span {
      padding-left: 5px;
    }
    div {
      display: flex;
      gap: 20px;
    }
  }

  .details {
    border-collapse: collapse;
    border: none;
    table-layout: auto;
    width: 100%;
  }

  thead {
    background-color: #c06400;
  }

  tbody {
    tr:nth-child(even) {
      background-color: #301900;
      color: #cccccc;
    }
    tr:nth-child(odd) {
      background-color: #170c00;
      color: #cccccc;
    }
    tr:hover td {
      background-color: rgba(79, 42, 0, 0.85);
    }
    th {
      padding-top: 5px;
      padding-bottom: 5px;
      padding-left: 5px;
      padding-right: 5px;
      text-wrap: wrap;
      text-align: left;
      color: var(--font-color-2);
    }
  }

  th:not(:first-child) {
    text-align: center;
  }

  td:not(:first-child) {
    text-align: center;
  }
</style>
