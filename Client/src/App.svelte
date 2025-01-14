<script lang="ts">
    import { onMount } from 'svelte';
    import svelteLogo from './assets/svelte.svg'
    import fsharpLogo from '/fsharp.svg'
    import Counter from './lib/Counter.svelte'

    let message = $state("")

    onMount(async () => {
        let response = await fetch('/api/hello');
        let data = await response.json() as Message
        message = data.text
    })

</script>

<main>
    <div>
        <a href="https://dotnet.microsoft.com/en-us/languages/fsharp" target="_blank" rel="noreferrer">
            <img src={fsharpLogo} class="logo" alt="Vite Logo"/>
        </a>
        <a href="https://svelte.dev" target="_blank" rel="noreferrer">
            <img src={svelteLogo} class="logo svelte" alt="Svelte Logo"/>
        </a>
    </div>
    <h1>F# + Svelte</h1>

    <div>{message}</div>

    <div class="card">
        <Counter/>
    </div>

    <p class="read-the-docs">
        Click on the F# and Svelte logos to learn more
    </p>
</main>

<style>
    .logo {
        height: 6em;
        padding: 1.5em;
        will-change: filter;
        transition: filter 300ms;
    }

    .logo:hover {
        filter: drop-shadow(0 0 2em #646cffaa);
    }

    .logo.svelte:hover {
        filter: drop-shadow(0 0 2em #ff3e00aa);
    }

    .read-the-docs {
        color: #888;
    }
</style>