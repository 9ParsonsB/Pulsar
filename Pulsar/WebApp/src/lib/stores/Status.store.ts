
import { writable } from "svelte/store";
import type Status from "../../types/api/Status";

export const statusStore = writable<Partial<Status>>({});