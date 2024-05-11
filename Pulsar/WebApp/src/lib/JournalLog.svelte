<script lang="ts">
  import connection from "./stores/Connection.store";

  let value = $state('');


$connection.on("JournalUpdated", (journals) => {
    console.log(journals);
    value += `${JSON.stringify(journals)}\n`;
});

</script>

<h1>Journals:</h1>

<textarea bind:value ></textarea>

