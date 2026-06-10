<script lang="ts">
    import {onMount} from "svelte";
    import connection from "$lib/stores/Connection.store";
    import type JournalBase from "../../types/api/JournalBase";

    type Signal = {
        type?: string;
        type_Localised?: string;
        count?: number;
        Type?: string;
        Type_Localised?: string;
        Count?: number;
    };

    type Genus = {
        genus?: string;
        genus_Localised?: string;
        Genus?: string;
        Genus_Localised?: string;
    };

    type BioSignalEvent = JournalBase & {
        bodyName?: string;
        bodyID?: number;
        signals?: Signal[];
        genuses?: Genus[];
        BodyName?: string;
        BodyID?: number;
        Signals?: Signal[];
        Genuses?: Genus[];
    };

    type ScanOrganicEvent = JournalBase & {
        scanType?: "Log" | "Sample" | "Analyse";
        genus?: string;
        genus_Localised?: string;
        species?: string;
        species_Localised?: string;
        variant?: string;
        variant_Localised?: string;
        body?: number;
        ScanType?: "Log" | "Sample" | "Analyse";
        Genus?: string;
        Genus_Localised?: string;
        Species?: string;
        Species_Localised?: string;
        Variant?: string;
        Variant_Localised?: string;
        Body?: number;
    };

    type StatusPosition = {
        bodyName?: string;
        BodyName?: string;
        latitude?: number;
        Latitude?: number;
        longitude?: number;
        Longitude?: number;
        planetRadius?: number;
        PlanetRadius?: number;
        heading?: number;
        Heading?: number;
        altitude?: number;
        Altitude?: number;
    };

    type BodySignals = {
        bodyId: number;
        bodyName: string;
        biologicalSignalCount: number;
        genuses: string[];
        updatedAt: Date | string;
    };

    type SamplePoint = {
        key: string;
        genus: string;
        species: string;
        bodyId: number | null;
        latitude: number;
        longitude: number;
        timestamp: Date | string;
        scanType: "Log" | "Sample" | "Analyse";
    };

    const colonyDistancesByGenus: Record<string, number> = {
        Aleoida: 150,
        Bacterium: 500,
        Cactoida: 300,
        Clypeus: 150,
        Concha: 150,
        Electricae: 1000,
        Fonticulua: 500,
        Frutexa: 150,
        Fumerola: 100,
        Fungoida: 300,
        Osseus: 800,
        Recepta: 150,
        Stratum: 500,
        Tubus: 800,
        Tussock: 200,
    };

    const englishGenusByIdentifier: Record<string, string> = {
        "$Codex_Ent_Aleoids_Genus_Name;": "Aleoida",
        "$Codex_Ent_Bacterial_Genus_Name;": "Bacterium",
        "$Codex_Ent_Cactoid_Genus_Name;": "Cactoida",
        "$Codex_Ent_Clepeus_Genus_Name;;": "Clypeus",
        "$Codex_Ent_Clypeus_Genus_Name;": "Clypeus",
        "$Codex_Ent_Conchas_Genus_Name;": "Concha",
        "$Codex_Ent_Electricae_Genus_Name;": "Electricae",
        "$Codex_Ent_Fonticulus_Genus_Name;": "Fonticulua",
        "$Codex_Ent_Shrubs_Genus_Name;": "Frutexa",
        "$Codex_Ent_Fumerolas_Genus_Name;": "Fumerola",
        "$Codex_Ent_Fungoids_Genus_Name;": "Fungoida",
        "$Codex_Ent_Osseus_Genus_Name;": "Osseus",
        "$Codex_Ent_Recepta_Genus_Name;": "Recepta",
        "$Codex_Ent_Stratum_Genus_Name;": "Stratum",
        "$Codex_Ent_Tubus_Genus_Name;": "Tubus",
        "$Codex_Ent_Tussocks_Genus_Name;": "Tussock",
    };

    let currentBodyId = $state<number | null>(null);
    let currentSample = $state<ScanOrganicEvent | null>(null);
    let currentPosition = $state<StatusPosition>({});
    let sampleProgress = $state(0);
    let completedScans = $state<ScanOrganicEvent[]>([]);
    let bodySignals = $state<BodySignals[]>([]);
    let samplePoints = $state<SamplePoint[]>([]);

    const activeGenus = $derived(normalizeGenus(currentSample?.Genus ?? currentSample?.genus, currentSample?.Genus_Localised ?? currentSample?.genus_Localised));
    const activeSpecies = $derived(displaySpecies(currentSample));
    const activeSampleKey = $derived(currentSample ? sampleKey(activeGenus, activeSpecies, currentBodyId) : "");
    const requiredDistance = $derived(activeGenus ? colonyDistancesByGenus[activeGenus] ?? 100 : null);
    const activeBodySignals = $derived(currentBodyId === null ? null : bodySignals.find((body) => body.bodyId === currentBodyId));
    const nearestSampleDistance = $derived(activeSampleKey ? getNearestSampleDistance(activeSampleKey) : null);
    const spacingRemaining = $derived(requiredDistance === null || nearestSampleDistance === null ? null : Math.max(0, Math.ceil(requiredDistance - nearestSampleDistance)));
    const spacingIsReady = $derived(requiredDistance !== null && nearestSampleDistance !== null && nearestSampleDistance >= requiredDistance);

    function normalizeGenus(identifier?: string, localised?: string): string {
        if (!identifier && !localised) return "";
        return englishGenusByIdentifier[identifier ?? ""] ?? localised ?? identifier ?? "";
    }

    function displaySpecies(scan: ScanOrganicEvent | null): string {
        return scan?.Species_Localised ?? scan?.species_Localised ?? scan?.Species ?? scan?.species ?? "Unknown species";
    }

    function displayVariant(scan: ScanOrganicEvent | null): string {
        return scan?.Variant_Localised ?? scan?.variant_Localised ?? scan?.Variant ?? scan?.variant ?? "";
    }

    function sampleKey(genus: string, species: string, bodyId: number | null): string {
        return `${bodyId ?? "unknown"}:${genus}:${species}`;
    }

    function scanStage(scanType: string | undefined): number {
        switch (scanType) {
            case "Log":
                return 1;
            case "Sample":
                return 2;
            case "Analyse":
                return 3;
            default:
                return sampleProgress;
        }
    }

    function formatDistance(value: number | null | undefined): string {
        if (value === null || value === undefined || Number.isNaN(value)) return "--";
        if (value >= 1000) return `${(value / 1000).toFixed(value >= 10000 ? 0 : 1)}km`;
        return `${Math.round(value)}m`;
    }

    function statusLatitude(status: StatusPosition): number | undefined {
        return status.Latitude ?? status.latitude;
    }

    function statusLongitude(status: StatusPosition): number | undefined {
        return status.Longitude ?? status.longitude;
    }

    function statusRadius(status: StatusPosition): number | undefined {
        return status.PlanetRadius ?? status.planetRadius;
    }

    function getBioSignalCount(signals: Signal[]): number {
        const biological = signals.find((signal) => {
            const type = signal.Type_Localised ?? signal.type_Localised ?? signal.Type ?? signal.type ?? "";
            return type.toLowerCase().includes("biological");
        });

        return biological?.Count ?? biological?.count ?? 0;
    }

    function distanceBetween(first: SamplePoint, position: StatusPosition): number | null {
        const latitude = statusLatitude(position);
        const longitude = statusLongitude(position);
        const radius = statusRadius(position);
        if (latitude === undefined || longitude === undefined || radius === undefined) return null;

        const phi1 = toRadians(latitude);
        const phi2 = toRadians(first.latitude);
        const deltaPhi = toRadians(first.latitude - latitude);
        const deltaLambda = toRadians(first.longitude - longitude);
        const a = Math.sin(deltaPhi / 2) ** 2 + Math.cos(phi1) * Math.cos(phi2) * Math.sin(deltaLambda / 2) ** 2;
        return radius * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    }

    function toRadians(degrees: number): number {
        return degrees * (Math.PI / 180);
    }

    function getNearestSampleDistance(key: string): number | null {
        const distances = samplePoints
            .filter((point) => point.key === key)
            .map((point) => distanceBetween(point, currentPosition))
            .filter((distance): distance is number => distance !== null);

        return distances.length ? Math.min(...distances) : null;
    }

    function recordSamplePoint(message: ScanOrganicEvent, scanType: "Log" | "Sample" | "Analyse") {
        const latitude = statusLatitude(currentPosition);
        const longitude = statusLongitude(currentPosition);
        const genus = normalizeGenus(message.Genus ?? message.genus, message.Genus_Localised ?? message.genus_Localised);
        const species = displaySpecies(message);
        const bodyId = message.Body ?? message.body ?? currentBodyId;

        if (latitude === undefined || longitude === undefined || !genus || !species) return;

        const nextPoint = {
            key: sampleKey(genus, species, bodyId ?? null),
            genus,
            species,
            bodyId: bodyId ?? null,
            latitude,
            longitude,
            timestamp: message.timestamp,
            scanType,
        };

        samplePoints = [nextPoint, ...samplePoints].slice(0, 60);
    }

    function applySignals(message: BioSignalEvent) {
        const bodyId = message.BodyID ?? message.bodyID;
        if (bodyId === undefined) return;

        const signals = message.Signals ?? message.signals ?? [];
        const genuses = (message.Genuses ?? message.genuses ?? [])
            .map((genus) => normalizeGenus(genus.Genus ?? genus.genus, genus.Genus_Localised ?? genus.genus_Localised))
            .filter(Boolean);

        const next = {
            bodyId,
            bodyName: message.BodyName ?? message.bodyName ?? `Body ${bodyId}`,
            biologicalSignalCount: getBioSignalCount(signals),
            genuses,
            updatedAt: message.timestamp,
        };

        bodySignals = [next, ...bodySignals.filter((body) => body.bodyId !== bodyId)].slice(0, 24);
        currentBodyId ??= bodyId;
    }

    function applyOrganicScan(message: ScanOrganicEvent) {
        const scanType = message.ScanType ?? message.scanType;
        currentBodyId = message.Body ?? message.body ?? currentBodyId;
        sampleProgress = scanStage(scanType);

        if (scanType === "Log" || scanType === "Sample" || scanType === "Analyse") {
            recordSamplePoint(message, scanType);
        }

        if (scanType === "Analyse") {
            completedScans = [message, ...completedScans].slice(0, 12);
            currentSample = null;
            sampleProgress = 0;
            return;
        }

        currentSample = message;
    }

    onMount(() => {
        const journalHandler = (messages: unknown) => {
            const journals = messages as JournalBase[];

            for (const journal of journals) {
                switch (journal.event) {
                    case "FSSBodySignals":
                    case "SAASignalsFound":
                        applySignals(journal as BioSignalEvent);
                        break;
                    case "ScanOrganic":
                        applyOrganicScan(journal as ScanOrganicEvent);
                        break;
                }
            }
        };

        const statusHandler = (message: StatusPosition) => {
            currentPosition = {...currentPosition, ...message};
        };

        connection.hub.on("JournalUpdated", journalHandler);
        connection.hub.on("StatusUpdated", statusHandler);

        return () => {
            connection.hub.off("JournalUpdated", journalHandler);
            connection.hub.off("StatusUpdated", statusHandler);
        };
    });
</script>

<section class="botany">
    <div class="hero">
        <div>
            <p class="eyebrow">Exobiology</p>
            <h1>Botany</h1>
            <p>Active organism, colony spacing, and recent biological bodies.</p>
        </div>
        <div class="sample-badge" class:live={sampleProgress > 0}>
            <span>{sampleProgress ? `Sample ${sampleProgress}/3` : "Idle"}</span>
            <small>{activeGenus || "No active genus"}</small>
        </div>
    </div>

    <div class="topology">
        <article class="panel active-sample">
            <div class="panel-title">
                <h2>Active Sample</h2>
                {#if requiredDistance}
                    <span>{formatDistance(requiredDistance)} colony rule</span>
                {/if}
            </div>

            {#if currentSample}
                <div class="species">{activeSpecies}</div>
                <div class="muted">{activeGenus || "Unknown genus"}</div>

                <div class="progress" aria-label="Organic sample progress">
                    {#each [1, 2, 3] as stage}
                        <span class:filled={stage <= sampleProgress}>{stage}</span>
                    {/each}
                </div>

                <div class="spacing-card" class:ready={spacingIsReady}>
                    <span>{spacingIsReady ? "Spacing ready" : "Next valid scan"}</span>
                    <strong>{spacingIsReady ? `>${formatDistance(requiredDistance)}` : formatDistance(spacingRemaining ?? requiredDistance)}</strong>
                    <small>
                        {#if nearestSampleDistance === null}
                            Waiting for planetary coordinates from Status.json
                        {:else}
                            Nearest previous sample: {formatDistance(nearestSampleDistance)}
                        {/if}
                    </small>
                </div>

                {#if displayVariant(currentSample)}
                    <div class="muted">Variant: {displayVariant(currentSample)}</div>
                {/if}
            {:else}
                <div class="empty-state">
                    <strong>No active organism</strong>
                    <span>Use the genetic sampler to populate live sample progress.</span>
                </div>
            {/if}
        </article>

        <article class="panel body-focus">
            <div class="panel-title">
                <h2>Body Focus</h2>
                <span>{bodySignals.length} tracked</span>
            </div>

            {#if activeBodySignals}
                <div class="species">{activeBodySignals.bodyName}</div>
                <div class="signal-count">
                    <strong>{activeBodySignals.biologicalSignalCount}</strong>
                    <span>biological signals</span>
                </div>
                <div class="chips">
                    {#each activeBodySignals.genuses as genus}
                        <span title={`${colonyDistancesByGenus[genus] ?? 100}m spacing`}>{genus}</span>
                    {/each}
                    {#if !activeBodySignals.genuses.length}
                        <span>Unknown genus mix</span>
                    {/if}
                </div>
            {:else}
                <div class="empty-state">
                    <strong>No focused body</strong>
                    <span>FSS or DSS biological signals will fill this panel.</span>
                </div>
            {/if}
        </article>
    </div>

    <div class="content-grid">
        <article class="panel">
            <div class="panel-title">
                <h2>Recent Biological Bodies</h2>
                <span>FSS / DSS</span>
            </div>

            {#if bodySignals.length}
                <div class="body-list">
                    {#each bodySignals as body}
                        <button class:active={body.bodyId === currentBodyId} onclick={() => (currentBodyId = body.bodyId)}>
                            <span>{body.bodyName}</span>
                            <small>{body.biologicalSignalCount} signals · {body.genuses.length || "?"} genera</small>
                        </button>
                    {/each}
                </div>
            {:else}
                <div class="empty-state compact">
                    <strong>No biological bodies yet</strong>
                    <span>Waiting for FSSBodySignals or SAASignalsFound.</span>
                </div>
            {/if}
        </article>

        <article class="panel">
            <div class="panel-title">
                <h2>Sample Log</h2>
                <span>{samplePoints.length} points</span>
            </div>

            {#if samplePoints.length}
                <div class="sample-list">
                    {#each samplePoints.slice(0, 8) as point}
                        <div>
                            <strong>{point.species}</strong>
                            <span>{point.scanType} · {point.genus} · {point.latitude.toFixed(3)}, {point.longitude.toFixed(3)}</span>
                        </div>
                    {/each}
                </div>
            {:else}
                <div class="empty-state compact">
                    <strong>No sample coordinates</strong>
                    <span>Coordinates are captured from Status.json when ScanOrganic arrives.</span>
                </div>
            {/if}
        </article>

        <article class="panel completed-panel">
            <div class="panel-title">
                <h2>Completed Samples</h2>
                <span>{completedScans.length} analysed</span>
            </div>

            {#if completedScans.length}
                <div class="completed">
                    {#each completedScans as scan}
                        <div>
                            <strong>{displaySpecies(scan)}</strong>
                            <span>{normalizeGenus(scan.Genus ?? scan.genus, scan.Genus_Localised ?? scan.genus_Localised)}</span>
                        </div>
                    {/each}
                </div>
            {:else}
                <div class="empty-state compact">
                    <strong>No analysed samples</strong>
                    <span>Completed ScanOrganic Analyse events will appear here.</span>
                </div>
            {/if}
        </article>
    </div>
</section>

<style>
    .botany {
        display: flex;
        flex-direction: column;
        gap: 18px;
        padding: 0;
        background: transparent;
        border: 0;
        box-shadow: none;
    }

    .hero,
    .panel {
        border: 1px solid var(--border-color);
        background:
            linear-gradient(135deg, rgba(255, 125, 0, 0.12), transparent 34%),
            var(--panel-bg);
        box-shadow: 0 0 18px rgba(255, 125, 0, 0.12);
    }

    .hero {
        display: flex;
        justify-content: space-between;
        gap: 18px;
        padding: 18px;
        overflow: hidden;
        position: relative;
    }

    .hero::after {
        content: "";
        position: absolute;
        inset: auto -8% -70% 48%;
        height: 180px;
        border: 1px solid rgba(255, 125, 0, 0.28);
        transform: rotate(-8deg);
        pointer-events: none;
    }

    .hero h1 {
        border: 0;
        margin: 0;
        font-size: clamp(2rem, 7vw, 4.5rem);
        line-height: 0.9;
    }

    .hero p:not(.eyebrow) {
        max-width: 720px;
        margin: 10px 0 0;
        color: #ffd29a;
    }

    .eyebrow,
    .panel-title span,
    .muted,
    .empty-state span,
    small,
    .sample-list span,
    .completed span {
        color: #b88b61;
    }

    .eyebrow,
    .sample-badge,
    .panel-title span {
        text-transform: uppercase;
        letter-spacing: 0.14em;
        font-weight: 700;
    }

    .sample-badge {
        align-self: flex-start;
        min-width: 180px;
        border: 1px solid rgba(255, 177, 74, 0.75);
        background: rgba(0, 0, 0, 0.28);
        padding: 12px 14px;
        color: #fff2d8;
        text-align: right;
    }

    .sample-badge span,
    .sample-badge small {
        display: block;
    }

    .sample-badge.live {
        box-shadow: 0 0 24px rgba(255, 125, 0, 0.24);
    }

    .topology,
    .content-grid {
        display: grid;
        gap: 18px;
    }

    .topology {
        grid-template-columns: minmax(0, 1.25fr) minmax(320px, 0.75fr);
    }

    .content-grid {
        grid-template-columns: repeat(3, minmax(0, 1fr));
    }

    .panel {
        padding: 16px;
        min-width: 0;
    }

    .panel-title {
        display: flex;
        justify-content: space-between;
        gap: 12px;
        align-items: baseline;
        margin-bottom: 14px;
        border-bottom: 1px solid rgba(255, 125, 0, 0.25);
    }

    .panel-title h2 {
        border: 0;
        margin: 0 0 8px;
        font-size: 1rem;
    }

    .active-sample {
        border-color: #ffb14a;
    }

    .species {
        color: #fff2d8;
        font-size: clamp(1.35rem, 3vw, 2rem);
        font-weight: 800;
        line-height: 1.1;
    }

    .progress {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 10px;
        margin: 18px 0;
    }

    .progress span {
        display: grid;
        place-items: center;
        min-height: 34px;
        border: 1px solid rgba(255, 125, 0, 0.35);
        background: rgba(0, 0, 0, 0.35);
        color: #7d5a32;
        font-weight: 800;
    }

    .progress span.filled {
        background: linear-gradient(90deg, #ff7d00, #ffe0a6);
        box-shadow: 0 0 14px rgba(255, 125, 0, 0.35);
        color: #120600;
    }

    .spacing-card,
    .signal-count {
        border: 1px solid rgba(255, 125, 0, 0.35);
        background: rgba(255, 125, 0, 0.08);
        padding: 12px;
        margin: 14px 0;
    }

    .spacing-card.ready {
        border-color: #8cff9a;
        background: rgba(65, 255, 122, 0.12);
    }

    .spacing-card span,
    .spacing-card strong,
    .spacing-card small,
    .signal-count strong,
    .signal-count span {
        display: block;
    }

    .spacing-card strong,
    .signal-count strong {
        color: #fff2d8;
        font-size: 2rem;
        line-height: 1.1;
    }

    .chips,
    .completed {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin-top: 12px;
    }

    .chips span,
    .completed div,
    .sample-list div {
        border: 1px solid rgba(255, 125, 0, 0.35);
        background: rgba(255, 125, 0, 0.08);
        padding: 8px 10px;
    }

    .body-list,
    .sample-list {
        display: grid;
        gap: 8px;
    }

    .body-list {
        grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
    }

    .body-list button {
        text-align: left;
        color: #ffd29a;
        background: rgba(255, 255, 255, 0.04);
        border: 1px solid rgba(255, 125, 0, 0.25);
        padding: 10px;
        cursor: pointer;
    }

    .body-list button.active,
    .body-list button:hover {
        border-color: #ffb14a;
        background: rgba(255, 125, 0, 0.14);
    }

    .body-list span,
    .body-list small,
    .sample-list strong,
    .sample-list span,
    .completed strong,
    .completed span {
        display: block;
    }

    .empty-state {
        display: grid;
        gap: 4px;
        min-height: 160px;
        align-content: center;
        border: 1px dashed rgba(255, 125, 0, 0.25);
        padding: 16px;
    }

    .empty-state.compact {
        min-height: 96px;
    }

    @media (max-width: 1050px) {
        .topology,
        .content-grid {
            grid-template-columns: 1fr;
        }
    }

    @media (max-width: 700px) {
        .hero {
            flex-direction: column;
        }

        .sample-badge {
            width: 100%;
            box-sizing: border-box;
            text-align: left;
        }
    }
</style>
