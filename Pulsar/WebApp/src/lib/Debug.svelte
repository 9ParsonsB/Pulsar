<script lang="ts">
    import {onMount} from "svelte";
    import type JournalBase from "../types/api/JournalBase";

    let data: JournalBase[] = [];
    let isLoading = true;
    let error: string | null = null;

    const getData = async (): Promise<JournalBase[]> => {
        const response = await fetch("/api/journal");
        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }
        return response.json();
    };

    onMount(() => {
        void (async () => {
            try {
                data = await getData();
            } catch (cause) {
                error = cause instanceof Error ? cause.message : "Unknown error";
            } finally {
                isLoading = false;
            }
        })();
    });
</script>

<h1>Debug</h1>

{#if isLoading}
    <span>Loading...</span>
{:else if error}
    <span>An error has occurred: {error}</span>
{:else}

    {#each data as row}
        {#if row.event == 'FSSDiscoveryScan'}
            <textarea value={JSON.stringify(row, null, 2)}/>
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

    th, td {
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
        /* width: 100%; */
        /* height: 100px; */
    }
</style>
