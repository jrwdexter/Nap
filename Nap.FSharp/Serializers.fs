namespace Nap

open System
open System.IO
open System.Text
open System.Reflection
open Newtonsoft.Json

type ISerializer = 
    abstract member ContentType : string
    abstract member Deserialize<'T> : string -> 'T option
    abstract member Serialize : obj -> string

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

type FormsSerializer =
    interface ISerializer with
        member this.ContentType = "application/x-www-form-urlencoded";
        member this.Deserialize<'T> serialized =
            raise (new NotSupportedException("Forms deserialization is not supported."))
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

type XmlSerializer =
    interface ISerializer with
        member this.ContentType = "application/xml"
        member this.Deserialize<'T> serialized =
            match serialized with
            | null -> None
            | _ ->
                let xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof<'T>)
                use sr = new StringReader(serialized)
                xmlSerializer.Deserialize(sr) |> unbox<'T> |> Some
        member this.Serialize graph = 
            match graph with
            | null -> ""
            | _ ->
                let xmlSerializer = new System.Xml.Serialization.XmlSerializer(graph.GetType())
                use textWriter = new StringWriter()
                xmlSerializer.Serialize(textWriter, graph)
                textWriter.ToString()