namespace Nap

open System
open System.Text
open System.Reflection
open Exceptions

type FormsSerializer =
    interface ISerializer with
        member this.ContentType = "application/x-www-form-urlencoded";
        member this.Deserialize<'T> serialized =
            notSupported "Forms deserialization is not supported."
            Option<'T>.None

        member this.Serialize graph =
            match graph with
            | isNull -> ""
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



