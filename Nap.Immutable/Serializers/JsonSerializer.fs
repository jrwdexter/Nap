namespace Nap.Serializers

open System.Reflection
open Nap.Serializers.Base
open Newtonsoft.Json

type NapJsonSerializer() = 
    inherit NapSerializerBase()
        override x.ContentType with get () = "application/json"
        override this.Deserialize<'T> serialized = 
            match serialized with
            | null -> Option<'T>.None
            | _ -> 
                // If it's default value and it isn't a value type, return None instead of null
                let deserialized = JsonConvert.DeserializeObject<'T>(serialized) 
                match (box deserialized) with
                | null -> Option<'T>.None
                | _ -> Option<'T>.Some(deserialized)
        override this.Serialize graph =
            match graph with
            | null -> ""
            | _ -> JsonConvert.SerializeObject(graph)