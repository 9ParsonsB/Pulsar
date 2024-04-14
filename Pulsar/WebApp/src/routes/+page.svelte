<script lang="ts">
    let x = $state(1)

    const increment = () => {
        x = x + 1;
    }
</script>

<h1>Welcome to Pulsar</h1>

<button on:click={increment} > Increment </button>

<span> {x} </span>
