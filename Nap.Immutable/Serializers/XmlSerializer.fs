namespace Nap.Serializers

open System.IO
open System.Reflection
open Nap.Serializers.Base
open System.Xml

type NapXmlSerializer() =
    inherit NapSerializerBase()
        override x.ContentType with get () = "application/xml"
        override this.Deserialize<'T> serialized =
            match serialized with
            | null -> None
            | _ ->
                let hasDefaultConstructor =
                    typeof<'T>.GetTypeInfo().DeclaredConstructors
                    |> Seq.exists (fun c -> c.GetParameters() |> Array.isEmpty)
                if hasDefaultConstructor
                then 
                    let xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof<'T>)
                    use sr = new StringReader(serialized)
                    xmlSerializer.Deserialize(sr) |> unbox<'T> |> Some
                else None
        override this.Serialize graph = 
            match graph with
            | null -> ""
            | _ ->
                let xmlSerializer = new System.Xml.Serialization.XmlSerializer(graph.GetType())
                use textWriter = new StringWriter()
                xmlSerializer.Serialize(textWriter, graph)
                textWriter.ToString()