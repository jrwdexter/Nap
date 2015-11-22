namespace Nap

open System.IO
open System.Xml

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