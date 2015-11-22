namespace Nap

open System.Net.Http

type NapClient (?config:NapConfig, ?setup:INapSetup) =
    // Consturcotrs
    member val Setup =
        match setup with
        | Some(someSetup) -> someSetup
        | None -> upcast new NapSetup()
    member val Config =
        let applyPlugins (plugins:IPlugin seq) (configuration:NapConfig) =
            plugins |> Seq.fold (fun c p -> p.ModifyConfiguration c) configuration
        let someConfig = match config with | Some(c) -> c | None -> NapConfig.Default
        let someSetup = match setup with | Some(s) -> s | None -> upcast NapSetup.Empty
        someConfig |> applyPlugins NapSetup.GlobalPlugins |> applyPlugins someSetup.Plugins
    static member Lets
        with get () = new NapClient()
    member x.Get () = x.CreateRequest(HttpMethod.Get)
    member x.Post () = x.CreateRequest(HttpMethod.Post)
    member x.Put () = x.CreateRequest(HttpMethod.Put)
    member x.Delete () = x.CreateRequest(HttpMethod.Delete)
    member x.CreateRequest httpMethod =
        let applyPlugins (plugins:IPlugin seq) (request:NapRequest) =
            plugins |> Seq.fold (fun c p -> p.PrepareRequest c) request
        { NapRequest.Default with Method = httpMethod }
        |> applyPlugins NapSetup.GlobalPlugins
        |> applyPlugins x.Setup.Plugins