module DockerTests

open System
open Docker.DotNet.Docker
open FsCheck
open FsCheck.FSharp
open FsCheck.Xunit

type ValidUri =
    | ValidUri of string

    member this.Uri() =
        match this with
        | ValidUri uri -> Uri(uri, UriKind.RelativeOrAbsolute)

module ValidUri =
    let fromHostName (HostName x) = ValidUri x
    let toHostName (ValidUri x) = HostName x

let isValidUri x =
    match Uri.TryCreate(x, UriKind.RelativeOrAbsolute) with
    | valid, _ -> valid

type Generators =
    static member ValidUris() =
        ArbMap.defaults
        |> ArbMap.arbitrary<HostName>
        |> Arb.filter (fun (HostName x) -> isValidUri x)
        |> Arb.convert ValidUri.fromHostName ValidUri.toHostName

module ConfigTests =
    [<Property(Arbitrary = [| typeof<Generators> |])>]
    let ``Built config sets endpoint`` (uri: ValidUri) =
        let config = uri.Uri() |> Client.Config.fromUri

        let actual = Client.Config.build config

        uri.Uri() = actual.EndpointBaseUri
