namespace Nap

open System

type ProxyNapConfig =
    {
        Address : Uri
        Credentials : Credentials option
    }
    with

    (*** Configuration ***)
    member x.SetProxy(address) =
        { x with Address = address ; Credentials = None }
    member x.SetProxy(address, credentials) =
        { x with Address = address; Credentials = credentials |> Some }

    (*** Default / ToString() ***)
    override x.ToString() =
        sprintf "%A" x