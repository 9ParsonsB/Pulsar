<script lang="ts">
    import axios from "axios";
    import { useQuery, useQueryClient } from "@sveltestack/svelte-query";

    const queryClient = useQueryClient();

    const getData = async () => {
        const response = await axios.get("http://localhost:5000/api/journal/");
        return response.data;
    };

    const query = useQuery("journal", getData, { staleTime: Infinity });
</script>

<h1>Mission Stack</h1>

{#if $query.isLoading}
    <span>Loading...</span>
{:else if $query.error}
    <span>An error has occurred: {$query.error}</span>
{:else}
    {#each $query.data as row}
        {#if row.event == "FSSDiscoveryScan"}
            <div>
                <textarea value={JSON.stringify(row, null, 2)} />
            </div>
        {/if}
    {/each}
{/if}

<style>
    table {
        table-layout: fixed;
        width: 100%;
        word-wrap: break-word;
    }

    thead {
        background-color: #505050;
    }

    tbody tr:nth-child(odd) {
        background-color: #23404c;
    }

    tbody tr:nth-child(even) {
        background-color: #282828;
    }

    th,
    td {
        padding-top: 5px;
        padding-bottom: 5px;
        padding-left: 5px;
        padding-right: 5px;
        width: 200px;
        text-wrap: wrap;
    }

    th {
        text-align: left;
    }

    textarea {
        /* width: 100%;
        height: 100px; */
    }
</style>
