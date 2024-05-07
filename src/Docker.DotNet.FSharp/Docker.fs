module Docker.DotNet.Docker

open System
open Docker.DotNet
open UnMango.Docker

module Client =
    type Config = { Endpoint: Uri option }

    module Config =
        let build =
            function
            | { Endpoint = Some e } -> new DockerClientConfiguration(endpoint = e)
            | _ -> new DockerClientConfiguration()

        let empty = { Endpoint = None }

        let fromUri uri = { Endpoint = Some uri }

    let create: Config -> IDockerClient = Config.build >> _.CreateClient()

    let fromUri: Uri -> IDockerClient = Config.fromUri >> create

let defaultClient = Client.Config.empty |> Client.create

let run (client: IDockerClient) =
    Seq.map (function
        | Container action -> Container.run client.Containers action |> Async.Ignore
        | Image action -> Image.run client.Images action |> Async.Ignore)
