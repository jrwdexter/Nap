namespace Nap.FSharp

open System
open System.Net
open System.Net.Http
open Nap

        
        
        

//module FNap =
//    let get = NapClient.Lets.Get
//    let post = NapClient.Lets.Post
//    let put = NapClient.Lets.Put
//    let delete = NapClient.Lets.Delete
//
//    let withCookie url cookieName value (request:INapRequest) = 
//        request.IncludeCookie(url, cookieName, value)
//
//    let setBody body (request:INapRequest) = 
//        request.IncludeBody(body)
//
//    let withHeader name value (request:INapRequest) =
//        request.IncludeHeader(name, value)
//
//    let withQueryParameter name value (request:INapRequest) =
//        request.IncludeQueryParameter(name, value)
//
//    let withoutHeader 
//        