namespace Nap

open System
open System.Net
open System.Net.Http

type INapSetup =
    abstract member Plugins : IPlugin list
    abstract member InstallPlugin<'T when 'T :> IPlugin and 'T : (new : unit -> 'T)> : unit -> INapSetup
    abstract member InstallPlugin : IPlugin -> INapSetup

[<Sealed>]
type NapSetup private(plugins : IPlugin list) =
    new() = NapSetup([])
    static member val GlobalPlugins = new System.Collections.Concurrent.ConcurrentBag<IPlugin>()
    static member InstallGlobalPlugin<'T when 'T :> IPlugin and 'T : (new : unit -> 'T)> () =
        new 'T() :> IPlugin |> NapSetup.InstallGlobalPlugin
    static member InstallGlobalPlugin (plugin:IPlugin) =
        NapSetup.GlobalPlugins.Add(plugin)
    static member val internal Empty = new NapSetup()
    interface INapSetup with
        member val Plugins = plugins
        member x.InstallPlugin<'T when 'T :> IPlugin and 'T : (new : unit -> 'T)> () =
            new 'T() :> IPlugin |> (x :> INapSetup).InstallPlugin
        member x.InstallPlugin plugin =
            upcast new NapSetup ((x :> INapSetup).Plugins |> List.append [plugin])

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
    member x.CreateRequest (httpMethod:HttpMethod) =
        let applyPlugins (plugins:IPlugin seq) (request:NapRequest) =
            plugins |> Seq.fold (fun c p -> p.PrepareRequest c) request
        NapRequest.Default
        |> applyPlugins NapSetup.GlobalPlugins
        |> applyPlugins x.Setup.Plugins