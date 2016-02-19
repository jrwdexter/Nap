namespace Nap

open System
open System.Net
open System.Net.Http
open Nap

module FNap =
    let get = NapClient.Lets.Get
    let post = NapClient.Lets.Post
    let put = NapClient.Lets.Put
    let delete = NapClient.Lets.Delete

    let setBody body (request:INapRequest) = 
        request.IncludeBody body

    let applyConfiguration config (request:INapRequest) =
        (request :?> NapRequest).ApplyConfig config

    let withCookie url cookieName value (request:INapRequest) = 
        request.IncludeCookie url cookieName value

    let withHeader name value (request:INapRequest) =
        request.IncludeHeader name value

    let withQueryParameter name value (request:INapRequest) =
        request.IncludeQueryParameter name value

    let withoutCookie name (request:INapRequest) =
        request.DoNot().IncludeCookie name

    let withoutHeader name (request:INapRequest) =
        request.DoNot().IncludeHeader name

    let withoutQueryParameter name (request:INapRequest) =
        request.DoNot().IncludeQueryParameter name

    let withMetadata (request:INapRequest) =
        request.FillMetadata()

    let withoutMetadata (request:INapRequest) =
        request.DoNot().FillMetadata()

    let useProxy proxy (request:INapRequest) =
        request.Advanced().SetProxy proxy

    let execute<'T> (request:INapRequest) =
        request.Execute<'T>()

    let executeRaw (request:INapRequest) =
        request.Execute()

    let executeRawAsync (request:INapRequest) =
        request.ExecuteAsync()

    let executeAsync<'T> (request:INapRequest) =
        request.ExecuteAsync<'T>()

    let logUsing logger (request:INapRequest) =
        (request :?> NapRequest).ApplyConfig (
            (request :?> NapRequest).Config.SetLogging logger
        )