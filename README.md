# F# Svelte Starter Project

<img src="./Client/public/fsharp-svelte.svg" height="200">


This project is a starter template to build Single Page Applications using F# (Giraffe) and Svelte. It is bundled together with a Fake script

## Tools
- [FSharp](https://dotnet.microsoft.com/en-us/languages/fsharp)
- [Svelte](https://svelte.dev)
- [Giraffe](https://giraffe.wiki/)
- [Vite](https://vite.dev/)
- [Fake](https://fake.build/index.html)

# Running

1. Clone the Repository
2. Run `dotnet fsi build.fsx -t run`
3. Navigate to http://localhost:3000


# Building

## Bundle
You can build and bundle the application with the following command

```bash
dotnet fsi build.fsx -t bundle
```
The you can copy the output bundle in `./.deploy`

## Docker

You can build a docker image using

```bash
docker build -t fsharp-svelte .
```

and then run it using

```bash
docker run -it -p 8080:8080 fsharp-svelte
```

Then visit http://localhost:8080
