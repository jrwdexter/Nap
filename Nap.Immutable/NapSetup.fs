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

