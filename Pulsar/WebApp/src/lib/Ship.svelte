<script lang="ts">
    import {onMount} from "svelte";

    type ModuleInfo = {
        item?: string;
        item_Localised?: string;
        power?: number;
        priority?: number;
        slot: string;
    };

    type ModulesInfoResponse = {
        modules: ModuleInfo[];
    };

    let data: ModulesInfoResponse | null = null;
    let isLoading = true;
    let error: string | null = null;

    const getData = async (): Promise<ModulesInfoResponse> => {
        const response = await fetch("/api/modulesinfo");
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

<h1>Ship Modules</h1>

{#if isLoading}
    <div class="loading">Loading...</div>
{:else if error}
    <div class="error">An error has occurred: {error}</div>
{:else}
    <div class="module-list">
        {#each data?.modules ?? [] as row}
            <div class="module-item">
                <div class="slot">{row.slot}</div>
                <div class="item-name">{row.item_Localised ?? row.item}</div>
                <div class="stats">
                    <span class="prio">P{row.priority}</span>
                    <span class="power">{(row.power ?? 0).toFixed(2)} MW</span>
                </div>
            </div>
        {/each}
    </div>
{/if}

<style>
    .module-list {
        display: flex;
        flex-direction: column;
        gap: 5px;
        max-height: 500px;
        overflow-y: auto;
    }

    .module-item {
        background: rgba(255, 255, 255, 0.05);
        padding: 6px 12px;
        display: grid;
        grid-template-columns: 120px 1fr 120px;
        gap: 10px;
        align-items: center;
        border-bottom: 1px solid #222;
    }

    .slot {
        font-size: 0.75rem;
        color: #888;
        text-transform: uppercase;
    }

    .item-name {
        font-weight: bold;
        color: #eee;
    }

    .stats {
        display: flex;
        justify-content: flex-end;
        gap: 15px;
        font-size: 0.85rem;
    }

    .prio {
        color: var(--accent-color);
    }

    .power {
        color: #aaa;
    }

    .loading, .error {
        padding: 20px;
        text-align: center;
    }
</style>
