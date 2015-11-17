namespace Nap

open System

type ProxyNapConfig =
    {
        Address : Uri option
        Credentials : Credentials option
    }
    with
    static member Default = 
        { Address = None; Credentials = None }
    override x.ToString() =
        sprintf "%A" x