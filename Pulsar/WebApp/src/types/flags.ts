/***
 * returns all possible flags for an enum
 * @param obj the Enum type
 */
export function getEnumFlags<
    O extends object,
>(obj: O): [number, string][] {
    if (!obj) return [];

    return Object.entries(obj)
        .filter((k) => Number.parseInt(k[0]))
        .map((k) => [Number(k[0]), k[1]]);
}

export function getEnumPairsFromValue<E extends object>(e: E, flag: number) {
    const flags = getEnumFlags(e);
    const active = [];
    for (const f of flags) {
        if (f[0] & flag) active.push(f[1]);
    }
    return active;
}

export function getEnumNamesFromFlag<E extends Record<number, string>>(
    e: E,
    flag: number,
) {
    const flags = getEnumFlags(e);
    const active = [];
    for (const f of flags) {
        if (f[0] & flag) active.push(e[f[0]]);
    }
    return active;
}

export function getEnumNameFromValue<E extends Record<number, string>>(
    e: E,
    value: number,
) {
    return e[value];
}
