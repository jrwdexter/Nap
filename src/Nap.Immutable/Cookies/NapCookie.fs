namespace Nap

open System
open System.Collections.Generic
open System.Linq

/// <summary>
/// A single cookie returned from the server.
/// </summary>
type NapCookie = {
    /// <summary>
    /// Gets the name of the cookie.
    /// </summary>
    Name: string

    /// <summary>
    /// Gets the value of the cookie.
    /// </summary>
    Value: string

    /// <summary>
    /// Gets all metadata associated with this cookie.
    /// </summary>
    Metadata: NapCookieMetadata
}
  
module NapCookie =
    /// <summary>
    /// Initializes a new instance of a <see cref="NapCookie"/> class.
    /// </summary>
    /// <param name="requestUri">The request URI that this cookie belongs to.</param>
    /// <param name="cookieString">The string that will generate a cookie (value of a Set-Cookie header).</param>
    let create (requestUri:Uri) (cookieString: string) =
        let segments =
            cookieString.Split(';') |> Seq.map(fun c ->
                let split = c.Split('=') |> Seq.map(fun s -> s |> Option.ofObj |> Option.map (fun o -> o.Trim()) |> Option.toObj) |> Seq.toList
                if (c.Length > 1) then
                    (split.[0], Some(split.[1]))
                else
                    (split.[0], None)
            ) |> Map.ofSeq
        if (segments |> Seq.length > 0) then
            {
                Name = (segments |> Seq.head).Key
                Value = (segments |> Seq.head).Value |> Option.get
                Metadata = NapCookieMetadata.create requestUri segments
            }
            else
            {
                Name = String.Empty
                Value = String.Empty
                Metadata = NapCookieMetadata.create requestUri segments
            }
