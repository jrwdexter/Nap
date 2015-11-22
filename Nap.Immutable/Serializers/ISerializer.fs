namespace Nap

type ISerializer = 
    abstract member ContentType : string
    abstract member Deserialize<'T> : string -> 'T option
    abstract member Serialize : obj -> string
