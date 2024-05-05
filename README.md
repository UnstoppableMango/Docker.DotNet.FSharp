# Docker.DotNet.FSharp

[![Build](https://img.shields.io/github/actions/workflow/status/UnstoppableMango/Docker.DotNet.FSharp/main.yml?branch=main)](https://github.com/UnstoppableMango/Docker.DotNet.FSharp/actions)
[![Codecov](https://img.shields.io/codecov/c/github/UnstoppableMango/Docker.DotNet.FSharp)](https://app.codecov.io/gh/UnstoppableMango/Docker.DotNet.FSharp)
[![GitHub Release](https://img.shields.io/github/v/release/UnstoppableMango/Docker.DotNet.FSharp)](https://github.com/UnstoppableMango/Docker.DotNet.FSharp/releases)
[![NuGet Version](https://img.shields.io/nuget/v/UnMango.Docker.DotNet.FSharp)](https://nuget.org/packages/UnMango.Docker.DotNet.FSharp)
[![NuGet Downloads](https://img.shields.io/nuget/dt/UnMango.Docker.DotNet.FSharp)](https://nuget.org/packages/UnMango.Docker.DotNet.FSharp)

Idiomatic F# support for [Docker.DotNet](https://github.com/dotnet/Docker.DotNet).

## Install

- [NuGet](https://nuget.org/packages/UnMango.Docker.DotNet.FSharp): `dotnet add package UnMango.Docker.DotNet.FSharp`
- [GitHub Packages](https://github.com/UnstoppableMango/Docker.DotNet.FSharp/pkgs/nuget/UnMango.Docker.DotNet.FSharp): `dotnet add package UnMango.Docker.DotNet.FSharp -s github`
  - [Authenticating to GitHub Packages](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#authenticating-to-github-packages)
  - [Installing from GitHub Packages](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#installing-a-package)

## Usage

At the moment, the library supports thin bindings for `IImageOperations` and `IContainerOperations`.

```fsharp
let docker: IDockerClient // Create a client
let auth: AuthConfig // Auth stuff if required
let progress: IProgress<JSONMessage> // Callbacks
let config = ImageCreateParameters(Image = "ubuntu")

do! docker.Images |> Image.create config auth progress
```

```fsharp
let docker: IDockerClient // Create a client
let config = ContainerCreateParameters(Image = "ubuntu:latest")

let! container = docker.Containers |> Container.create config
```

## Q/A

### Idiomatic? This looks nothing like the F# I write!

If something looks off please open an issue! I've only recently been diving further into the F# ecosystem.

### Deprecated Operations

When I started this project there were a few deprecated operations and I chose not to implement them.
Future deprecations will be tagged and removed along with upstream.
