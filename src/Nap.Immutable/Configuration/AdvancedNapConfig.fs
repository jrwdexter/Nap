namespace Nap

open System
open System.Net
open System.Net.Http

type AdvancedNapConfig =
    {
        Proxy : ProxyNapConfig option
        Authentication : AuthenticationNapConfig
        ClientCreator : (obj -> HttpClient) option
    }
    with
    (*** Client creator ***)
    member x.SetClientCreator (clientCreator:Func<obj, HttpClient>) =
        { x with ClientCreator = (fun nr -> clientCreator.Invoke(nr)) |> Some }

    member x.ResetClientCreator () =
        { x with ClientCreator = None }

    (*** Proxy ***)
    member x.ConfigureProxy (f:Func<_,_>) =
        { x with Proxy = f.Invoke(x.Proxy) }

    (*** Authentication ***)
    member x.ConfigureAuthentication (f:Func<_,_>) =
        { x with Authentication = f.Invoke(x.Authentication)}

    (*** Default/ToString ***)
    static member Default =
        {
            Proxy = None
            Authentication = Unauthenticated
            ClientCreator = None
        }
    override x.ToString() =
        sprintf "%A" x