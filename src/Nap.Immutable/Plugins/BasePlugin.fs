namespace Nap.Plugins.Base
open Nap

[<AbstractClass>]
type BasePlugin() =
    abstract member Setup : NapConfig -> NapConfig
    default x.Setup config = config
    abstract member Prepare : NapConfig -> NapConfig
    default x.Prepare request = request
    interface IPlugin with
        //abstract member x.Setup : NapConfig -> NapConfig
        member x.ModifyConfiguration config = x.Setup config
        member x.PrepareRequest request = request