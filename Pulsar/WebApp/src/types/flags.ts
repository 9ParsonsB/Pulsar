const isPowerOfTwo = (x: number): boolean => {
	return x !== 0 && (x & (x - 1)) === 0;
};

/***
 * returns all possible flags for an enum
 * @param obj the Enum type
 */
export function getEnumFlags<
	O extends object,
	F extends O[keyof O] = O[keyof O],
>(obj: O): [number, string][] {
	const isFlag = (arg: string | number | F): arg is F => {
		const nArg = Number(arg);
		const isNumber = !Number.isNaN(nArg);
		return isNumber && isPowerOfTwo(nArg);
	};

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
