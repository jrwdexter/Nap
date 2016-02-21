namespace Nap.Configuration

[<Sealed>]
type RequestFormat =
    static member Form = "application/x-www-form-urlencoded"
    static member Json = "application/json"
    static member Xml = "application/xml"
    static member Other = "OTHER"