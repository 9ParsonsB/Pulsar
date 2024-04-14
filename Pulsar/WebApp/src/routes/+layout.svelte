<nav>
    <ul>
        <li><a href="/">Dashboard</a></li>
        <li><a href="/ship">Ship</a></li>
        <li><a href="/botany">Botanist</a></li>
        <li><a href="/explorer">Explorer</a></li>
        <li><a href="/options">Options</a></li>
    </ul>
</nav>

<slot />

<style>
    ul {
        display: flex;
        list-style-type: none;
    }
    li {
        padding: 1%;
    }

    :global(body) {
        background-color: black;
        color: white;
        [href] {
            color: red;
            &:visited {
                color: aqua;
            }
        }
    }
</style>