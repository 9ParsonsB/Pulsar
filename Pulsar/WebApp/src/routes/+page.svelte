<script lang="ts">
    import * as signalR from "@microsoft/signalr"
    import {onMount} from "svelte";
    let x = $state(null);
    let textarea = $state("");

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/api/events")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    onMount(async () => {

        connection.onclose(async () => {
            console.log("Lost connection to Event Hub. Attempting to reconnect...");
            await connection.start();
        });

        connection.on("StatusUpdated", (message) => {
            console.log('we did it!');
            console.log(message);
            textarea += JSON.stringify(message) + "\n"
        });

        await connection.start();
    })

    const getStatus = async () => {
        var response = await fetch("http://localhost:5000/api/status", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        });

        if (response.ok)
        {
            const data = await response.text();
            console.log(data);
            x = data;
        }

        console.log(response);
    }
</script>

<h1>Welcome to Pulsar</h1>

<button on:click={getStatus}> GetStatus </button>

<span> {x} </span>

<br/>

<textarea bind:value={textarea}></textarea>
