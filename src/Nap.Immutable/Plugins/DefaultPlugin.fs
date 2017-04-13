namespace Nap.Plugins

open System
open Nap
open Nap.Plugins.Base

type DefaultPlugin() =
    inherit BasePlugin()
    override x.Configure config = { config with Log = x.Log }
    member private x.Log(logLevel: LogLevel) (path: string) (message: string): unit = 
        // Default logging is super basic - just write to the attached debugger if available; otherwise, do nothing.
        if logLevel > LogLevel.Debug && System.Diagnostics.Debugger.IsAttached then
            sprintf "%s - [%s] %s" (Enum.GetName(typeof<LogLevel>,logLevel).ToUpper()) path message
            |> System.Diagnostics.Debug.WriteLine