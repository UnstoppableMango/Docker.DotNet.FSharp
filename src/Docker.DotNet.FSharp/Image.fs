module Docker.DotNet.Image

open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading
open System.Threading.Tasks
open Docker.DotNet.Internal
open Docker.DotNet.Models
open UnMango.Docker.Images

module private Convert =
    let create: Create -> ImagesCreateParameters =
        function
        | { FromImage = Some i } -> ImagesCreateParameters(FromImage = i)
        | _ -> failwith "unsupported configuration"

type private Ext =
    [<Extension>]
    static member inline Await(docker, f: IImageOperations -> CancellationToken -> Task) = WC.wrap docker |> _.Await(f)

    [<Extension>]
    static member inline Await(docker, f: IImageOperations -> CancellationToken -> Task<_>) =
        WC.wrap docker |> _.Await(f)

    [<Extension>]
    static member inline AwaitList(docker, f: IImageOperations -> CancellationToken -> Task<IList<_>>) =
        WC.wrap docker |> _.AwaitList(f)

    [<Extension>]
    static member Create(docker: IImageOperations, create) =
        let p = Convert.create create
        docker.Await(fun d ct -> d.CreateImageAsync(p, null, null, ct))

let buildFromDockerfile p contents auth headers progress (docker: IImageOperations) =
    docker.Await(fun x ct -> x.BuildImageFromDockerfileAsync(p, contents, auth, headers, progress, ct))

let commitContainerChanges p (docker: IImageOperations) =
    docker.Await(fun x ct -> x.CommitContainerChangesAsync(p, ct))

// TODO: There are a number of `CreateImageAsync` overloads

let create p auth progress (docker: IImageOperations) =
    docker.Await(fun x ct -> x.CreateImageAsync(p, auth, progress, ct))

let delete name p (docker: IImageOperations) =
    docker.Await(fun x ct -> task {
        let! result = x.DeleteImageAsync(name, p, ct)
        return result |> Seq.map Map.ofDict
    })

let history name (docker: IImageOperations) =
    docker.AwaitList(fun x ct -> x.GetImageHistoryAsync(name, ct))

let inspect name (docker: IImageOperations) =
    docker.Await(fun x ct -> x.InspectImageAsync(name, ct))

let list p (docker: IImageOperations) =
    docker.Await(fun x ct -> x.ListImagesAsync(p, ct))

let load p stream progress (docker: IImageOperations) =
    docker.Await(fun x ct -> x.LoadImageAsync(p, stream, progress, ct))

let prune p (docker: IImageOperations) =
    docker.Await(fun x ct -> x.PruneImagesAsync(p, ct))

let push name p auth progress (docker: IImageOperations) =
    docker.Await(fun x ct -> x.PushImageAsync(name, p, auth, progress, ct))

let save name (docker: IImageOperations) =
    docker.Await(fun x ct -> x.SaveImageAsync(name, ct))

let saveAll names (docker: IImageOperations) =
    docker.Await(fun x ct -> x.SaveImagesAsync(names, ct))

let search p (docker: IImageOperations) =
    docker.AwaitList(fun x ct -> x.SearchImagesAsync(p, ct))

let tag name p (docker: IImageOperations) =
    docker.Await(fun x ct -> x.TagImageAsync(name, p, ct))

let run (client: IImageOperations) =
    function
    | Create x -> client.Create(x)
    | _ -> failwith "unsupported operation"

let runAll (docker: IImageOperations) =
    Seq.map (run docker) >> Async.Sequential
