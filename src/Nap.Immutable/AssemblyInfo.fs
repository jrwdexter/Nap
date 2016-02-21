namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Nap.Immutable")>]
[<assembly: AssemblyProductAttribute("Nap")>]
[<assembly: AssemblyDescriptionAttribute("A short and sweet REST client.")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
