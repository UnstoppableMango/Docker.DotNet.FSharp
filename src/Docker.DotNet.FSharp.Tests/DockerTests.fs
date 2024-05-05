module DockerTests

open System
open Docker.DotNet.Docker
open FsCheck.Xunit
open Xunit

module ConfigTests =
    [<Property>]
    let ``Built config sets endpoint`` (endpoint: Uri) =
        let config = Client.Config.fromUri endpoint

        let actual = Client.Config.build config

        endpoint = actual.EndpointBaseUri

[<Property>]
let ``My test`` () = Assert.True(true)
