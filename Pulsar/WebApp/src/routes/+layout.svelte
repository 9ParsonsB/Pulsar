<script>
    import Fuel from "$lib/Fuel.svelte";
</script>

<header>
    <nav>
        <ul>
            <li><a href="/">Dashboard</a></li>
            <!--<li><a href="/settings">Settings</a></li>-->
        </ul>
    </nav>
    <Fuel />
</header>

<main>
    <slot />
</main>

<style>
    :root {
        --background-color: #0d0302;
        --header-color: #202225;
        --headings-color: #ffffff;
        --font-color-1: #cccccc;
        --font-color-2: #000000;
        --border-color: #5f3100;
        --line-color: #ffffff;
        --link-color: #d06527;
        --link-color-hover: #000000;
    }

    :global(body) {
        background-color: var(--background-color);
        color: var(--font-color-1);
        font-family: "Eurostile", Helvetica, sans-serif;
        letter-spacing: 0.05em;
        font-size: 1rem;
        box-sizing: border-box;
        margin: 0px;
        line-height: 1.6;
    }

    :global(h1) {
        margin: 0;
    }

    header {
        margin-top: 10px;
        margin-bottom: 20px;
    }

    nav {
        border-bottom: 3px solid #d66325;
        border-top: 3px solid #d66325;
        max-width: 1600px;
        margin: 0 auto;
    }

    ul {
        display: flex;
        list-style-type: none;
        margin: 0px;
        padding: 0px;
        gap: 10px;
    }

    li {
        margin-top: 5px;
        margin-bottom: 5px;
        width: 200px;
        padding-left: 10px;
        padding-right: 10px;
        padding-top: 4px;
        padding-bottom: 4px;
        background-color: #3c1e05;
        font-weight: bold;
        font-size: 1.125rem;
        text-align: center;
    }

    li:hover {
        background-color: #ef7d15;
    }

    li:hover a {
        color: var(--link-color-hover);
    }

    a {
        display: block;
        width: 100%;
        color: var(--link-color);
        text-decoration: none;
    }

    main {
        max-width: 1600px;
        margin: 0 auto;
    }
</style>
