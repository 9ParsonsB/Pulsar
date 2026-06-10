<script lang="ts">
    import {onMount} from "svelte";

    type OverlayBodyTarget = {
        name?: string;
        class?: string;
        distanceFromArrivalLs?: number;
        estimatedValue?: number;
        terraformable?: boolean;
    };

    type OverlayState = {
        commander?: string;
        ship?: string;
        shipName?: string;
        shipIdent?: string;
        system?: string;
        body?: string;
        station?: string;
        stationType?: string;
        destination?: string;
        cargo?: number;
        fuelMain?: number;
        fuelCapacity?: number;
        fuelPercent?: number;
        lowFuel?: boolean;
        fuelScooping?: boolean;
        fuelSecondsRemaining?: number | null;
        alerts?: string[];
        dockingState?: string;
        dockingStation?: string;
        landingPad?: number | null;
        currentSystemScan?: string;
        bodiesScanned?: number;
        totalBodies?: number;
        estimatedSystemValue?: number;
        highValueBodies?: OverlayBodyTarget[];
        biologySpecies?: string;
        biologyGenus?: string;
        biologySample?: number | null;
        biologyRequiredDistance?: number | null;
    };

    let data = $state<OverlayState>({});

    const shipLabel = $derived([data.ship, data.shipName, data.shipIdent].filter(Boolean).join(" | "));
    const fuelLabel = $derived(data.fuelMain !== undefined ? `${data.fuelMain.toFixed(1)} / ${(data.fuelCapacity ?? 0).toFixed(1)}t` : "--");
    const scoopLabel = $derived(
        data.fuelScooping && data.fuelSecondsRemaining !== null && data.fuelSecondsRemaining !== undefined
            ? `${data.fuelSecondsRemaining}s`
            : null
    );

    const load = async () => {
        const response = await fetch("/api/overlay/state", {cache: "no-store"});
        if (!response.ok) {
            return;
        }

        data = await response.json();
    };

    onMount(() => {
        void load();
        const interval = window.setInterval(() => void load(), 1000);

        return () => {
            window.clearInterval(interval);
        };
    });
</script>

<svelte:head>
    <title>Pulsar Overlay</title>
</svelte:head>

<div class="overlay">
    <div class="stack">
        <div class="panel hero">
            <div class="eyebrow">Commander</div>
            <div class="title">{data.commander ?? "Waiting for data"}</div>
            <div class="muted">{shipLabel || "No active ship"}</div>
            <div class="route">{data.system ?? "---"}{#if data.body} <span>• {data.body}</span>{/if}</div>
        </div>

        {#if data.dockingState || data.station}
            <div class="panel">
                <div class="eyebrow">Docking</div>
                <div class="row strong">
                    <span>{data.dockingState ?? "At Station"}</span>
                    {#if data.landingPad}
                        <span>Pad {data.landingPad}</span>
                    {/if}
                </div>
                <div class="muted">{data.dockingStation ?? data.station}{#if data.stationType} • {data.stationType}{/if}</div>
            </div>
        {/if}

        <div class="panel" class:warning={data.lowFuel}>
            <div class="eyebrow">Fuel</div>
            <div class="row strong">
                <span>{fuelLabel}</span>
                <span>{(data.fuelPercent ?? 0).toFixed(1)}%</span>
            </div>
            <div class="meter">
                <div class="fill" style={`width:${Math.max(0, Math.min(100, data.fuelPercent ?? 0))}%`}></div>
            </div>
            <div class="row muted">
                <span>{data.destination ?? "No destination"}</span>
                {#if scoopLabel}
                    <span>Scoop {scoopLabel}</span>
                {/if}
            </div>
        </div>

        {#if data.biologySpecies}
            <div class="panel">
                <div class="eyebrow">Biology</div>
                <div class="strong">{data.biologySpecies}</div>
                <div class="row muted">
                    <span>{data.biologyGenus}</span>
                    <span>Sample {data.biologySample ?? "?"}/3</span>
                </div>
                <div class="hint">Next sample distance: {data.biologyRequiredDistance ?? 0}m</div>
            </div>
        {/if}

        {#if (data.highValueBodies?.length ?? 0) > 0}
            <div class="panel">
                <div class="eyebrow">Exploration</div>
                <div class="row strong">
                    <span>{data.currentSystemScan ?? "No system"}</span>
                    <span>{data.bodiesScanned ?? 0}/{data.totalBodies ?? 0}</span>
                </div>
                <div class="hint">Estimated value: {(data.estimatedSystemValue ?? 0).toLocaleString()} cr</div>
                <div class="targets">
                    {#each data.highValueBodies ?? [] as body}
                        <div class="target">
                            <div class="row">
                                <span class="strong">{body.name}</span>
                                <span>{(body.estimatedValue ?? 0).toLocaleString()} cr</span>
                            </div>
                            <div class="muted">{body.class} • {(body.distanceFromArrivalLs ?? 0).toFixed(0)} ls{#if body.terraformable} • terraformable{/if}</div>
                        </div>
                    {/each}
                </div>
            </div>
        {/if}

        {#if (data.alerts?.length ?? 0) > 0}
            <div class="panel alerts">
                <div class="eyebrow">Alerts</div>
                {#each data.alerts ?? [] as alert}
                    <div class="alert">{alert}</div>
                {/each}
            </div>
        {/if}
    </div>
</div>

<style>
    .overlay {
        min-height: 100vh;
        padding: 18px;
        display: flex;
        align-items: flex-start;
        justify-content: flex-start;
        pointer-events: none;
    }

    .stack {
        width: min(420px, 100%);
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .panel {
        background: rgba(7, 13, 18, 0.62);
        border: 1px solid rgba(115, 220, 255, 0.28);
        border-radius: 14px;
        padding: 12px 14px;
        backdrop-filter: blur(10px);
        box-shadow: 0 12px 28px rgba(0, 0, 0, 0.28);
    }

    .panel.warning {
        border-color: rgba(255, 124, 92, 0.55);
    }

    .hero {
        padding-top: 14px;
    }

    .eyebrow {
        font-size: 0.7rem;
        text-transform: uppercase;
        letter-spacing: 0.16em;
        color: #80b7c6;
        margin-bottom: 6px;
    }

    .title {
        font-size: 1.55rem;
        font-weight: 700;
        color: #e5f8ff;
    }

    .route,
    .muted,
    .hint {
        color: #9ec6d2;
    }

    .strong {
        color: #f0fbff;
        font-weight: 600;
    }

    .row {
        display: flex;
        justify-content: space-between;
        gap: 12px;
    }

    .meter {
        margin: 8px 0 6px;
        height: 10px;
        background: rgba(255, 255, 255, 0.08);
        border-radius: 999px;
        overflow: hidden;
    }

    .fill {
        height: 100%;
        background: linear-gradient(90deg, #58d7ff, #92f3ff);
    }

    .warning .fill {
        background: linear-gradient(90deg, #ff7658, #ffcb71);
    }

    .targets {
        margin-top: 8px;
        display: flex;
        flex-direction: column;
        gap: 8px;
    }

    .target {
        padding-top: 8px;
        border-top: 1px solid rgba(255, 255, 255, 0.08);
    }

    .target:first-child {
        border-top: 0;
        padding-top: 0;
    }

    .alerts .alert {
        color: #ff9f88;
        font-weight: 700;
    }
</style>
