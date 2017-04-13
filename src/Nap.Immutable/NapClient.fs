namespace Nap

open System.Net.Http
open Nap.Plugins.Base

type NapClient (config:NapConfig option, setup:INapSetup option) =
    (*** Constructors ***)
    new() = new NapClient(Option<NapConfig>.None, Option<INapSetup>.None)
    new(config:NapConfig) = new NapClient(config |> Some, Option<INapSetup>.None)
    new(setup:INapSetup) = new NapClient(Option<NapConfig>.None, setup |> Some)
    new(config, setup) = new NapClient(config |> Some, setup |> Some)
    new(uri) = new NapClient({NapConfig.Default with BaseUrl = uri} |> Some, None)
    member val Setup =
        match setup with
        | Some(someSetup) -> someSetup
        | None -> upcast new NapSetup()
    member val Config =
        let applyPlugins (plugins:IPlugin seq) (configuration:NapConfig) =
            plugins |> Seq.fold (fun c p -> p.Configure c) configuration
        let someConfig = match config with | Some(c) -> c | None -> NapConfig.Default
        let someSetup = match setup with | Some(s) -> s | None -> upcast NapSetup.Empty
        someConfig |> applyPlugins someSetup.Plugins
    static member Lets
        with get () = new NapClient()
    member x.Get () = x.CreateRequest(HttpMethod.Get) :> INapRequest
    member x.Post () = x.CreateRequest(HttpMethod.Post) :> INapRequest
    member x.Put () = x.CreateRequest(HttpMethod.Put) :> INapRequest
    member x.Delete () = x.CreateRequest(HttpMethod.Delete)
    member x.Get url = x.CreateRequest(HttpMethod.Get) |> fun req -> { req with Url = url} :> INapRequest
    member x.Post url = x.CreateRequest(HttpMethod.Post) |> fun req -> { req with Url = url} :> INapRequest
    member x.Put url = x.CreateRequest(HttpMethod.Put) |> fun req -> { req with Url = url} :> INapRequest
    member x.Delete url = x.CreateRequest(HttpMethod.Delete) |> fun req -> { req with Url = url} :> INapRequest
    member x.CreateRequest httpMethod =
        { NapRequest.Default with Method = httpMethod }.ApplyConfig(x.Config)