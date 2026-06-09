<script>
    import Fuel from "$lib/Fuel.svelte";
    import { QueryClient, QueryClientProvider } from "@sveltestack/svelte-query";

    const queryClient = new QueryClient();
</script>

<QueryClientProvider client={queryClient}>
<header>
    <nav>
        <ul>
            <li><a href="/">Dashboard</a></li>
            <li><a href="/ship">Ship</a></li>
            <li><a href="/explorer">Explorer</a></li>
            <li><a href="/botany">Botany</a></li>
        </ul>
    </nav>
    <Fuel />
</header>

<main>
    <slot />
</main>
</QueryClientProvider>

<style>
    :root {
        --background-color: #0d0302;
        --header-color: #202225;
        --headings-color: #ffffff;
        --font-color-1: #ff7d00; /* Elite Orange */
        --font-color-2: #000000;
        --border-color: #5f3100;
        --line-color: #ffffff;
        --link-color: #ff7d00;
        --link-color-hover: #ffffff;
        --accent-color: #ff7d00;
        --panel-bg: rgba(25, 10, 0, 0.8);
    }

    :global(body) {
        background-color: var(--background-color);
        background-image: radial-gradient(circle at center, #1a0a00 0%, #0d0302 100%);
        color: var(--font-color-1);
        font-family: "Eurostile", Helvetica, sans-serif;
        letter-spacing: 0.05em;
        font-size: 1rem;
        box-sizing: border-box;
        margin: 0px;
        line-height: 1.6;
        min-height: 100vh;
    }

    :global(h1, h2, h3) {
        margin: 0;
        color: var(--accent-color);
        text-transform: uppercase;
        border-bottom: 1px solid var(--border-color);
        margin-bottom: 10px;
    }

    header {
        margin-top: 10px;
        margin-bottom: 20px;
        padding: 0 20px;
    }

    nav {
        border-bottom: 3px solid var(--accent-color);
        border-top: 3px solid var(--accent-color);
        max-width: 1600px;
        margin: 0 auto;
        background-color: var(--panel-bg);
    }

    ul {
        display: flex;
        list-style-type: none;
        margin: 0px;
        padding: 0px;
        gap: 2px;
    }

    li {
        margin-top: 5px;
        margin-bottom: 5px;
        width: 150px;
        background-color: #3c1e05;
        font-weight: bold;
        font-size: 1rem;
        text-align: center;
        transition: background-color 0.2s;
    }

    li:hover {
        background-color: var(--accent-color);
    }

    li:hover a {
        color: var(--font-color-2);
    }

    a {
        display: block;
        padding: 8px 15px;
        width: 100%;
        color: var(--link-color);
        text-decoration: none;
    }

    main {
        max-width: 1600px;
        margin: 0 auto;
        padding: 0 20px;
    }

    :global(section) {
        background-color: var(--panel-bg);
        border: 1px solid var(--border-color);
        padding: 15px;
        margin-bottom: 20px;
        box-shadow: 0 0 15px rgba(255, 125, 0, 0.1);
    }
</style>
