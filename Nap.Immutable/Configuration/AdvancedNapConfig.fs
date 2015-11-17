namespace Nap

open System.Net.Http

type AdvancedNapConfig =
    {
        ClientCreator : obj -> HttpClient
        Proxy : ProxyNapConfig
        Authentication : AuthenticationNapConfig
    }
    with
    static member Default =
        {
            ClientCreator = fun _ -> new HttpClient()
            Proxy = ProxyNapConfig.Default
            Authentication = Unauthenticated
        }
    override x.ToString() =
        sprintf "%A" x