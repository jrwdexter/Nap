namespace Nap

open System
open System.Net.Http
open Nap.Configuration
open Nap.Serializers
open Nap.Serializers.Base

type NapConfig = 
    {
        Advanced        : AdvancedNapConfig
        BaseUrl         : string
        Headers         : Map<string, string>
        QueryParameters : Map<string, string>
        FillMetadata    : bool
        Serialization   : string
        Serializers     : Map<string, INapSerializer>
        Log             : LogLevel -> string -> string -> unit
    }
    with

    (*** Advanced ***)
    member x.ConfigureAdvanced (f:Func<_,_>) =
        { x with Advanced = f.Invoke(x.Advanced) }

    (*** Serializers ***)
    member x.AddSerializer (serializer:INapSerializer) =
        { x with Serializers = x.Serializers |> Map.add serializer.ContentType serializer}
    member x.AddSerializerFor contentType serializer =
        { x with Serializers = x.Serializers |> Map.add contentType serializer }
    member x.RemoveSerializer serializer =
        { x with Serializers = x.Serializers |> Map.toSeq |> Seq.filter (fun (_,serializer) -> serializer <> serializer ) |> Map.ofSeq}
    member x.RemoveSerializerFor contentType =
        { x with Serializers = x.Serializers |> Map.remove contentType}
    member x.ResetSerializers () =
        { x with Serializers = NapConfig.Default.Serializers }
    member x.ClearSerializers () =
        { x with Serializers = Map.empty }

    (*** Base URL ***)
    member x.SetBaseUrl url =
        { x with BaseUrl = url}

    (*** Headers ***)
    member x.AddHeader headerName headerValue =
        { x with Headers = x.Headers |> Map.add headerName headerValue }
    member x.RemoveHeader headerName =
        { x with Headers = x.Headers |> Map.remove headerName }
    member x.ClearHeaders =
        { x with Headers = Map.empty }

    (*** Query Parameters ***)
    member x.AddQueryParameter headerName headerValue =
        { x with QueryParameters = x.QueryParameters |> Map.add headerName headerValue }
    member x.RemoveQueryParameter headerName =
        { x with QueryParameters = x.QueryParameters |> Map.remove headerName }
    member x.ClearQueryParameters =
        { x with QueryParameters = Map.empty }

    (*** Serialization ***)
    member x.SetDefaultSerialization serialization =
        { x with Serialization = serialization }

    (*** Fill metadata ***)
    member x.SetMetadataBehavior fillMetadataFlag =
        { x with FillMetadata = fillMetadataFlag }

    (*** Logging ***)
    member x.SetLogging log =
        { x with Log = log }
    member x.ResetLogging () =
        { x with Log = NapConfig.Default.Log }
    member x.ClearLogging () =
        { x with Log = fun _ _ _ -> () }

    (*** Default ***)
    static member Default =
        {
            Serializers =
                [ new NapJsonSerializer() :> INapSerializer; new NapXmlSerializer() :> INapSerializer; new NapFormsSerializer() :> INapSerializer ]
                |> Seq.map (fun s -> (s.ContentType, s))
                |> Map.ofSeq
            BaseUrl = ""
            Headers = Map.empty
            QueryParameters = Map.empty
            Serialization = RequestFormat.Json
            Advanced = AdvancedNapConfig.Default
            FillMetadata = true
            Log = fun _ _ _ -> ()
        }
    override x.ToString() =
        sprintf "%A" x
