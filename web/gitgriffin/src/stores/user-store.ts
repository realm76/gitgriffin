import { createSignal } from "solid-js";

export const [user, setUser] = createSignal<{ email: string } | null>(
    JSON.parse(localStorage.getItem("user") || "null")
);
