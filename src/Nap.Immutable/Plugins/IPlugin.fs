namespace Nap.Plugins.Base

open Nap
open System.Net.Http

type IPlugin =
    // TODO: Define plugin architecture.
    // Option: Pull Nap.Plugins into parent assembly of Nap and Nap.Immutable

    // Modify configuration as necessary. This is called after a configuration is initially created.
    abstract member Configure : NapConfig -> NapConfig
    abstract member Prepare : NapRequest -> NapRequest

    abstract member Execute<'T when 'T : not struct> : NapRequest -> 'T

    // Prepare a request by further modifying or adjusting it's parameters. Called immediately after creating a request.
    abstract member Process : NapResponse -> NapResponse

//    // Execute a NapRequest.
//    // If the request returns Null
//    abstract member ExecuteRequest : NapRequest -> obj option
//
//    abstract member CreateRequest : (NapConfig * string * HttpMethod) -> NapRequest