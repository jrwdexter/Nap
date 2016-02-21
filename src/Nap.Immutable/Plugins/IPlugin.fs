namespace Nap

type IPlugin =
    // Modify configuration as necessary
    abstract member ModifyConfiguration : NapConfig -> NapConfig
    // Prepare a request by further modifying or adjusting it's parameters
    abstract member PrepareRequest : NapRequest -> NapRequest