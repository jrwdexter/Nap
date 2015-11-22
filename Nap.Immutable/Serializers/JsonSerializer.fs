namespace Nap

open System.Reflection
open Newtonsoft.Json

type JsonSerializer = 
    interface ISerializer with
        member this.ContentType = "application/json"
        member this.Deserialize<'T> serialized = 
            match serialized with
            | null -> None
            | _ ->
                let hasEmptyConstructor = typeof<'T>.GetTypeInfo().DeclaredConstructors |> Seq.exists (fun c ->
                    (not c.ContainsGenericParameters) && (c.GetParameters() |> Seq.isEmpty)
                )
                if hasEmptyConstructor
                then JsonConvert.DeserializeObject<'T>(serialized) |> Some
                else None
        member this.Serialize graph =
            match graph with
            | null -> ""
            | _ -> JsonConvert.SerializeObject(graph)


