<script lang="ts">
    import {onMount} from "svelte";

    let data: Array<Record<string, unknown>> = [];
    let isLoading = true;
    let error: string | null = null;

    const getData = async (): Promise<Array<Record<string, unknown>>> => {
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

<h1>Mission Stack</h1>

{#if isLoading}
    <div class="loading">Loading...</div>
{:else if error}
    <div class="error">An error has occurred: {error}</div>
{:else}
    <div class="stack-container">
        {#each data as row}
            {#if row.event === "Missions"}
                <div class="mission-list">
                    <h3>Active Missions</h3>
                    {#each (row.Active as Array<{ Name: string; Expires: number }>) ?? [] as mission}
                        <div class="mission-item">
                            <span class="name">{mission.Name}</span>
                            <span class="expiry">Expires: {new Date(mission.Expires * 1000).toLocaleString()}</span>
                        </div>
                    {/each}
                </div>
            {/if}
        {/each}
    </div>
{/if}

<style>
    .stack-container {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }

    .mission-list {
        display: flex;
        flex-direction: column;
        gap: 5px;
    }

    .mission-item {
        background: rgba(255, 255, 255, 0.05);
        padding: 8px 12px;
        display: flex;
        justify-content: space-between;
        border-left: 2px solid var(--accent-color);
    }

    .name {
        font-weight: bold;
    }

    .expiry {
        font-size: 0.8rem;
        color: #888;
    }

    .loading, .error {
        padding: 20px;
        text-align: center;
    }
</style>
