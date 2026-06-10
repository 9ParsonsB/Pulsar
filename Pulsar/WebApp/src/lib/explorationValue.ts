import type {Scan} from "../types/api/Scan";

export type ExplorationValue = {
    scan: number;
    mapped: number;
    honk: number;
    minScan: number;
    minMapped: number;
    minHonk: number;
};

const minimumBodyValue = 500;

export function getStarClassK(starClass?: string) {
    if (!starClass) return 1200;
    if (starClass === "N" || starClass === "H") return 22628;
    if (starClass.startsWith("D")) return 14057;
    return 1200;
}

function getPlanetClassK(planetClass?: string, terraformable = false) {
    switch (planetClass) {
        case "Metal rich body":
            return {base: 21790, terraform: 0, terraformMultiplier: 1};
        case "Ammonia world":
            return {base: 96932, terraform: 0, terraformMultiplier: 1};
        case "Sudarsky class I gas giant":
        case "Class I gas giant":
            return {base: 1656, terraform: 0, terraformMultiplier: 1};
        case "Sudarsky class II gas giant":
        case "Class II gas giant":
        case "High metal content body":
            return {base: 9654, terraform: terraformable ? 100677 : 0, terraformMultiplier: terraformable ? 0.9 : 1};
        case "Water world":
            return {base: 64831, terraform: terraformable ? 116295 : 0, terraformMultiplier: terraformable ? 0.75 : 1};
        case "Earthlike body":
        case "Earthlike world":
            return {base: 64831, terraform: 116295, terraformMultiplier: terraformable ? 0 : 1};
        default:
            return {base: 300, terraform: terraformable ? 93328 : 0, terraformMultiplier: terraformable ? 0.9 : 1};
    }
}

function clampExplorationValue(value: number) {
    return value > minimumBodyValue ? value : minimumBodyValue;
}

function roundValue(value: number) {
    return Math.round(value);
}

export function isTerraformable(body: Pick<Scan, "terraformState">) {
    return Boolean(body.terraformState?.trim());
}

export function getExplorationValue(body: Scan, odysseyBonus = true): ExplorationValue {
    const firstDiscoverer = !body.wasDiscovered;

    if (body.starType) {
        const k = getStarClassK(body.starType);
        let scan = k + ((body.stellarMass ?? 0) * k) / 66.25;
        let honk = scan / 3;

        if (firstDiscoverer) {
            scan *= 2.6;
            honk *= 2.6;
        }

        return {
            scan: roundValue(scan),
            mapped: 0,
            honk: roundValue(honk),
            minScan: roundValue(scan),
            minMapped: 0,
            minHonk: roundValue(honk)
        };
    }

    const {base, terraform, terraformMultiplier} = getPlanetClassK(body.planetClass, isTerraformable(body));
    const mass = Math.max(body.massEM || 0, 0.000001);
    const q = 0.56591828;
    const finalK = base + terraform;
    const finalMinK = base + terraform * terraformMultiplier;
    const firstMapper = !body.wasMapped;
    const mappingMultiplier = firstDiscoverer && firstMapper ? 3.699622554 : firstMapper ? 8.0956 : 10 / 3;

    let scan = finalK + finalK * q * Math.pow(mass, 0.2);
    let minScan = finalMinK + finalMinK * q * Math.pow(mass, 0.2);
    let mapped = scan * mappingMultiplier;
    let minMapped = minScan * mappingMultiplier;
    let honk = scan / 3;
    let minHonk = minScan / 3;

    if (odysseyBonus) {
        mapped += Math.max(mapped * 0.3, 555);
        minMapped += Math.max(minMapped * 0.3, 555);
    }

    scan = clampExplorationValue(scan);
    minScan = clampExplorationValue(minScan);
    mapped = clampExplorationValue(mapped);
    minMapped = clampExplorationValue(minMapped);
    honk = clampExplorationValue(honk);
    minHonk = clampExplorationValue(minHonk);

    if (firstDiscoverer) {
        scan *= 2.6;
        minScan *= 2.6;
        mapped *= 2.6;
        minMapped *= 2.6;
        honk *= 2.6;
        minHonk *= 2.6;
    }

    return {
        scan: roundValue(scan),
        mapped: roundValue(mapped),
        honk: roundValue(honk),
        minScan: roundValue(minScan),
        minMapped: roundValue(minMapped),
        minHonk: roundValue(minHonk)
    };
}

export function getBodyValueLabel(value: ExplorationValue) {
    return value.minMapped !== value.mapped
        ? `${value.minMapped.toLocaleString()}-${value.mapped.toLocaleString()} Cr`
        : `${value.mapped.toLocaleString()} Cr`;
}

export function isValuableExplorerBody(body: Scan, minimumMappedValue: number) {
    if (body.starType) return false;
    const value = getExplorationValue(body);
    return value.mapped >= minimumMappedValue || isTerraformable(body);
}
