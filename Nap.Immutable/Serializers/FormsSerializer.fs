namespace Nap.Serializers

open System
open System.Text
open System.Reflection
open Nap.Serializers.Base
open Nap.ExceptionHelpers

type NapFormsSerializer() =
    inherit NapSerializerBase()
        override x.ContentType with get () = "application/x-www-form-urlencoded";
        override this.Deserialize<'T> serialized =
            notSupported "Forms deserialization is not supported."
            Option<'T>.None

        override this.Serialize graph =
            match graph with
            | null -> ""
            | _ -> 
                let sb = new StringBuilder()
                seq {
                    for prop in graph.GetType().GetRuntimeProperties() do
                    yield sprintf "%s=%s" prop.Name (
                        match prop.GetValue(graph) with
                        | null -> ""
                        | value -> value.ToString()
                    )
                }
                |> String.concat "&"



