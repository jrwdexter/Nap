namespace Nap

type IPlugin =
    // Modify configuration as necessary. This is called after a configuration is initially created.
    abstract member ModifyConfiguration : NapConfig -> NapConfig

    // Prepare a request by further modifying or adjusting it's parameters. Called immediately after creating a request.
    abstract member PrepareRequest : NapRequest -> NapRequest