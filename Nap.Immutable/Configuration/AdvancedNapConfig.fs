namespace Nap

open System
open System.Net.Http

type AdvancedNapConfig =
    {
        ClientCreator : obj -> HttpClient
        Proxy : ProxyNapConfig
        Authentication : AuthenticationNapConfig
    }
    with
    (*** Client Creator ***)
    member x.SetClientCreator (clientCreator:Func<_,_>) =
        { x with ClientCreator = fun req -> clientCreator.Invoke(req) }
    member x.ResetClientCreator () =
        { x with ClientCreator = AdvancedNapConfig.Default.ClientCreator }

    (*** Proxy ***)
    member x.ConfigureProxy (f:Func<_,_>) =
        { x with Proxy = f.Invoke(x.Proxy) }

    (*** Authentication ***)
    member x.ConfigureAuthentication (f:Func<_,_>) =
        { x with Authentication = f.Invoke(x.Authentication)}

    (*** Default/ToString ***)
    static member Default =
        {
            ClientCreator = fun _ -> new HttpClient()
            Proxy = ProxyNapConfig.Default
            Authentication = Unauthenticated
        }
    override x.ToString() =
        sprintf "%A" x