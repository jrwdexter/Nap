namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Nap.Immutable")>]
[<assembly: AssemblyProductAttribute("Nap")>]
[<assembly: AssemblyDescriptionAttribute("A short and sweet REST client.")>]
[<assembly: AssemblyVersionAttribute("0.3")>]
[<assembly: AssemblyFileVersionAttribute("0.3")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.3"
