namespace Nap.Exceptions

open System
open Nap.Exceptions.Base

type AuthorizationException =
    inherit NapException
    new() = { inherit NapException() }
    new(message) = { inherit NapException(message) }
    new(message, innerException) = { inherit NapException(message, innerException) }

type ConstructorNotFoundException =
    inherit NapException
    new() = { inherit NapException() }
    new(message) = { inherit NapException(message) }
    new(message, innerException) = { inherit NapException(message, innerException) }

type NapConfigurationException =
    inherit NapException
    new() = { inherit NapException() }
    new(message) = { inherit NapException(message) }
    new(message, innerException) = { inherit NapException(message, innerException) }

type NapPluginException =
    inherit NapException
    new() = { inherit NapException() }
    new(message) = { inherit NapException(message) }
    new(message, innerException) = { inherit NapException(message, innerException) }

type NapSerializingException =
    inherit NapException
    new() = { inherit NapException() }
    new(message) = { inherit NapException(message) }
    new(message, innerException) = { inherit NapException(message, innerException) }
