<script lang="ts">
    import { useQuery, useQueryClient } from "@sveltestack/svelte-query";

    const queryClient = useQueryClient();

    const getData = async () => {
        const response = await fetch(
            "http://localhost:5000/api/modulesinfo/",
        );
        return response.json();
    };

    const query = useQuery("modulesinfo", getData, { staleTime: Infinity });
</script>

<h1>Ship</h1>

{#if $query.isLoading}
    <span>Loading...</span>
{:else if $query.error}
    <span>An error has occurred: {$query.error}</span>
{:else}
    {#each $query.data.modules as row}
        <div class="module">
            <div>{row.slot}</div>
            <div>{row.power}</div>
            <div>{row.priority}</div>
        </div>
    {/each}
{/if}

<style>
    .module {
        display: flex;
        gap: 10px;
    }
</style>
