namespace Nap.Serializers

open System
open System.Text
open System.Reflection
open Nap.Serializers.Base
open Nap.ExceptionHelpers

type NapEmptySerializer() =
    inherit NapSerializerBase()
        override x.ContentType with get () = "";
        override this.Deserialize<'T> serialized =
            typeof<'T>.GetTypeInfo().DeclaredConstructors
            |> Seq.tryFind (fun c -> c.GetParameters() |> Seq.isEmpty)
            |> Option.map (fun c -> c.Invoke([||]) :?> 'T) 

        override this.Serialize _ =
            ""