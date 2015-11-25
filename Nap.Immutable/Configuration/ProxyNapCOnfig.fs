namespace Nap

open System

type ProxyNapConfig =
    {
        Address : Uri option
        Credentials : Credentials option
    }
    with

    (*** Configuration ***)
    member x.SetProxy(address) =
        { x with Address = address |> Some; Credentials = None }
    member x.SetProxy(address, credentials) =
        { x with Address = address |> Some; Credentials = credentials |> Some }
    member x.ClearProxy () =
        ProxyNapConfig.Default

    (*** Default / ToString() ***)
    static member Default = 
        { Address = None; Credentials = None }
    override x.ToString() =
        sprintf "%A" x