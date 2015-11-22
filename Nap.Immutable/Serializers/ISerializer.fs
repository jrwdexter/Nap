namespace Nap.Serializers.Base

type INapSerializer = 
    abstract member ContentType : string
    abstract member Deserialize<'T> : string -> 'T option
    abstract member Serialize : obj -> string
