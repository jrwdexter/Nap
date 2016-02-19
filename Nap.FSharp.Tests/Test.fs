namespace Nap.FSharp.Tests

open System
open System.Web
open System.Net
open System.Net.Http
open System.Reflection
open Nap
open FsUnit
open FsUnit.MsTest
open Microsoft.VisualStudio.TestTools.UnitTesting


module FunctionalNapTests = 
    let url = "http://www.example.com/"
    let key = "foo"
    let value = "bar"

    let baseRequest = FNap.get url

    [<TestClass>]
    type FNapTests () =
        [<TestMethod>]
        member x.``FNap.get requests url with correct verb`` () =
            (downcast FNap.get url).Url |> should equal url
            (downcast FNap.get url).Method |> should equal HttpMethod.Get

        [<TestMethod>]
        member x.``FNap.post requests url with correct verb`` () =
            (downcast FNap.post url).Url |> should equal url
            (downcast FNap.post url).Method |> should equal HttpMethod.Post

        [<TestMethod>]
        member x.``FNap.put requests url with correct verb`` () =
            (downcast FNap.put url).Url |> should equal url
            (downcast FNap.put url).Method |> should equal HttpMethod.Put

        [<TestMethod>]
        member x.``FNap.delete requests url with correct verb`` () =
            (downcast FNap.delete url).Url |> should equal url
            (downcast FNap.delete url).Method |> should equal HttpMethod.Delete

        [<TestMethod>]
        member x.``FNap.withCookies adds a cookie`` () =
            let uri = Uri(url)
            (downcast (baseRequest |> FNap.withCookie url key value)).Cookies |> should haveLength 1
            (downcast (baseRequest |> FNap.withCookie url key value)).Cookies.Head |> should equal (url |> Uri,new Cookie(key, value, uri.AbsolutePath, uri.Host))
