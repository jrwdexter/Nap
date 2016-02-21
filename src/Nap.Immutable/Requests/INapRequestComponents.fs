namespace Nap

open System
open System.Net.Http
open System.Threading.Tasks

type INapRequest =
    abstract member DoNot : unit -> IRemovableNapRequestComponent
    abstract member Advanced : unit -> IAdvancedNapRequestComponent
    abstract member IncludeQueryParameter : string -> string -> INapRequest
    abstract member IncludeBody : obj -> INapRequest
    abstract member IncludeHeader : string -> string -> INapRequest
    abstract member IncludeCookie : string -> string -> string -> INapRequest
    abstract member FillMetadata : unit -> INapRequest
    abstract member ExecuteAsync : unit -> Task<string>
    abstract member ExecuteAsync<'T> : unit -> Task<'T>
    abstract member Execute : unit -> string
    abstract member Execute<'T> : unit -> 'T

and IRemovableNapRequestComponent =
    abstract member FillMetadata : unit -> INapRequest
    abstract member IncludeBody : unit -> INapRequest
    abstract member IncludeHeader : string -> INapRequest
    abstract member IncludeCookie : string -> INapRequest
    abstract member IncludeQueryParameter : string -> INapRequest

and IAdvancedNapRequestComponent =
    abstract ClientCreator : (INapRequest -> HttpClient) with get
    abstract Proxy : ProxyNapConfig option with get
    abstract Authentication : AuthenticationNapConfig with get
    abstract Configuration : NapConfig with get
    abstract member SetProxy : ProxyNapConfig -> INapRequest
    abstract member SetClientCreator : Func<INapRequest, HttpClient> -> INapRequest
    abstract member SetAuthentication : AuthenticationNapConfig -> INapRequest


