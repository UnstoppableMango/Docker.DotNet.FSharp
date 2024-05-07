module Docker.DotNet.Container

open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Threading
open System.Threading.Tasks
open Docker.DotNet.Internal
open Docker.DotNet.Models
open UnMango.Docker.Containers

module private Convert =
    let create: Create -> CreateContainerParameters =
        function // TODO: The rest of this
        | { Cmd = c; Entrypoint = ent; Env = env } ->
            CreateContainerParameters(Cmd = ResizeArray(c), Entrypoint = ResizeArray(ent), Env = ResizeArray(env))

type private Ext =
    [<Extension>]
    static member inline Await(docker, f: IContainerOperations -> CancellationToken -> Task) =
        WC.wrap docker |> _.Await(f)

    [<Extension>]
    static member inline Await(docker, f: IContainerOperations -> CancellationToken -> Task<_>) =
        WC.wrap docker |> _.Await(f)

    [<Extension>]
    static member inline AwaitList(docker, f: IContainerOperations -> CancellationToken -> Task<IList<_>>) =
        WC.wrap docker |> _.AwaitList(f)

let archiveFrom id p statOnly (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.GetArchiveFromContainerAsync(id, p, statOnly, ct))

let attach id tty p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.AttachContainerAsync(id, tty, p, ct))

let create p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.CreateContainerAsync(p, ct))

let export id (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.ExportContainerAsync(id, ct))

let extractArchiveTo id p stream (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.ExtractArchiveToContainerAsync(id, p, stream, ct))

let inspect id (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.InspectContainerAsync(id, ct))

let inspectChanges id (docker: IContainerOperations) =
    docker.AwaitList(fun x ct -> x.InspectChangesAsync(id, ct))

let list p (docker: IContainerOperations) =
    docker.AwaitList(fun x ct -> x.ListContainersAsync(p, ct))

let listProcesses id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.ListProcessesAsync(id, p, ct))

let kill id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.KillContainerAsync(id, p, ct))

let logs id tty p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.GetContainerLogsAsync(id, tty, p, ct))

let logTo id p progress (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.GetContainerLogsAsync(id, p, ct, progress))

let pause id (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.PauseContainerAsync(id, ct))

let prune p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.PruneContainersAsync(p, ct))

let rename id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.RenameContainerAsync(id, p, ct))

let resizeTty id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.ResizeContainerTtyAsync(id, p, ct))

let remove id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.RemoveContainerAsync(id, p, ct))

let restart id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.RestartContainerAsync(id, p, ct))

let start id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.StartContainerAsync(id, p, ct))

let stats id p progress (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.GetContainerStatsAsync(id, p, progress, ct))

let unpause id (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.UnpauseContainerAsync(id, ct))

let update id p (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.UpdateContainerAsync(id, p, ct))

let wait id (docker: IContainerOperations) =
    docker.Await(fun x ct -> x.WaitContainerAsync(id, ct))

let run (client: IContainerOperations) =
    function
    | Create action -> create (Convert.create action) client
    | _ -> failwith "TODO"
