namespace Nap.Serializers.Base

type INapSerializer = 
    abstract member ContentType : string with get
    abstract member Deserialize<'T> : string -> 'T option
    abstract member Serialize : obj -> string

[<AbstractClass>]
type NapSerializerBase() as this =
    abstract member ContentType : string with get
    abstract member Deserialize<'T> : string -> 'T option
    abstract member Serialize : obj -> string
    interface INapSerializer with
        member x.ContentType with get () = this.ContentType
        member x.Deserialize<'T> serialized = this.Deserialize<'T> serialized
        member x.Serialize graph = this.Serialize graph