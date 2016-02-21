namespace Nap.Exceptions.Base

open System

[<AbstractClass>]
type NapException =
    inherit Exception
    new() = { inherit Exception() }
    new(message) = { inherit Exception(message) }
    new(message, innerException) = { inherit Exception(message, innerException) }


