namespace Nap

type EventCode =
    | BeforeConfigurationApplied = 0x01
    | AfterConfigurationApplied = 0x02
    | BeforeRequestExecution = 0x04
    | AfterRequestExecution = 0x08
    | BeforeResponseDeserialization = 0x10
    | AfterResponseDeserialization = 0x20
    | BeforeRequestSerialization = 0x40
    | AfterRequestSerialization = 0x80
    | CreatingNapRequest = 0x100
    | SetAuthentication = 0x200
    | All = 0x3FF