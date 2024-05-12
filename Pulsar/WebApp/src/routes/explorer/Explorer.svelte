<script lang="ts">
  const data: unknown[] = [{}, {}, {}, {}];
</script>

<section>
  <div class="title">
    <h1>Explorer</h1>
  </div>
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
