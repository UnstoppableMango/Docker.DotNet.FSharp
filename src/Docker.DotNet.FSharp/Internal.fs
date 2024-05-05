namespace Docker.DotNet.Internal

open System.Collections.Generic
open System.Threading
open System.Threading.Tasks

[<Struct>]
type WithCancellation<'T> =
    { CancellationToken: CancellationToken
      Value: 'T }

    member internal this.Await(f: 'T -> CancellationToken -> Task) =
        f this.Value this.CancellationToken |> Async.AwaitTask

    member internal this.Await(f: 'T -> CancellationToken -> Task<_>) =
        f this.Value this.CancellationToken |> Async.AwaitTask

    member internal this.AwaitList(f: 'T -> CancellationToken -> Task<IList<_>>) =
        this.Await(fun x ct -> task {
            let! result = f x ct
            return List.ofSeq result
        })

type internal WC<'T> = WithCancellation<'T>

module internal WC =
    let map f wc =
        { CancellationToken = wc.CancellationToken
          Value = f wc.Value }

    let token wc = wc.CancellationToken

    let wrap x =
        { CancellationToken = Async.DefaultCancellationToken
          Value = x }

module internal Map =
    let ofDict d = d |> Seq.map (|KeyValue|) |> Map.ofSeq
