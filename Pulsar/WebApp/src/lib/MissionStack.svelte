<script lang="ts">
    import { useQuery, useQueryClient } from "@sveltestack/svelte-query";

    const queryClient = useQueryClient();

    const getData = async () => {
        const response = await fetch("http://localhost:5000/api/journal/");
        return response.json();
    };

    const query = useQuery("journal", getData, { staleTime: Number.POSITIVE_INFINITY });
</script>

<h1>Mission Stack</h1>

{#if $query.isLoading}
    <div class="loading">Loading...</div>
{:else if $query.error}
    <div class="error">An error has occurred: {$query.error}</div>
{:else}
    <div class="stack-container">
        {#each $query.data as row}
            {#if row.event == "Missions"}
                <div class="mission-list">
                    <h3>Active Missions</h3>
                    {#each row.Active as mission}
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

    .name { font-weight: bold; }
    .expiry { font-size: 0.8rem; color: #888; }

    .loading, .error { padding: 20px; text-align: center; }
</style>
