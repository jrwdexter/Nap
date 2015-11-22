namespace Nap

open System
open System.Net.Http

type NapConfig = 
    {
        Serializers     : Map<string, ISerializer>
        BaseUri         : string
        Headers         : Map<string, string>
        QueryParameters : Map<string, string>
        Serialization   : string
        Advanced        : AdvancedNapConfig
        FillMetadata    : bool
        Log             : LogLevel -> string -> string -> unit
    }
    with
    static member Default =
        {
            Serializers = Map.empty
            BaseUri = ""
            Headers = Map.empty
            QueryParameters = Map.empty
            Serialization = ""
            Advanced = AdvancedNapConfig.Default
            FillMetadata = true
            Log = fun _ _ _ -> ()
        }
    override x.ToString() =
        sprintf "%A" x
