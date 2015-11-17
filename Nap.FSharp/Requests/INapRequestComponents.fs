namespace Nap

open System.Net.Http

type INapRequest =
    abstract member DoNot : unit -> IRemovableNapRequestComponent
    abstract member Advanced : unit -> IAdvancedNapRequestComponent
    abstract member IncludeQueryParameter : string -> string -> INapRequest
    abstract member IncludeBody : obj -> INapRequest
    abstract member IncludeHeader : string -> string -> INapRequest
    abstract member IncludeCookie : string -> string -> string -> INapRequest
    abstract member FillMetadata : unit -> INapRequest
    abstract member ExecuteAsync : unit -> Async<string>
    abstract member ExecuteAsync<'T> : unit -> Async<'T>
    abstract member Execute : unit -> string
    abstract member Execute<'T> : unit -> 'T

and IRemovableNapRequestComponent =
    abstract member FillMetadata : unit -> INapRequest
    abstract member IncludeBody : unit -> INapRequest
    abstract member IncludeHeader : string -> INapRequest
    abstract member IncludeCookie : string -> INapRequest
    abstract member IncludeQueryParameter : string -> INapRequest

and IAdvancedNapRequestComponent =
    abstract ClientCreator : (obj -> HttpClient) with get
    abstract Proxy : ProxyNapConfig with get
    abstract Authentication : AuthenticationNapConfig with get
    abstract Configuration : NapConfig with get
    abstract member SetProxy : ProxyNapConfig -> INapRequest
    abstract member SetClientCreator : (INapRequest -> HttpClient) -> INapRequest
    abstract member SetAuthentication : AuthenticationNapConfig -> INapRequest


