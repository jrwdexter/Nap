namespace Nap

open System
open System.Collections.Generic
open System.Net
open System.Linq

/// <summary>
/// A single cookie returned from the server.
/// </summary>
type NapCookie =
    {
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
    with
    /// <summary>
    /// Initializes a new instance of a <see cref="NapCookie"/> class.
    /// </summary>
    /// <param name="requestUri">The request URI that this cookie belongs to.</param>
    /// <param name="cookieString">The string that will generate a cookie (value of a Set-Cookie header).</param>
    static member Create (requestUri:Uri) (cookieString: string) =
        let lowercaseSegmentKeys =
            Map.toSeq >> Seq.map (fun ((k:string),v) -> k.ToLower(),v) >> Map.ofSeq
        let segments =
            seq { for c in cookieString.Split(';') do
                     let split = c.Split('=') |> Seq.map(fun s -> s |> Option.ofObj |> Option.map (fun o -> o.Trim()) |> Option.toObj) |> Seq.toList
                     match split with
                     | [name] -> yield (name, None)
                     | name::rest -> yield (name, Some(rest.Head))
                     | [] -> ()
            }
        if (segments |> Seq.length > 0) then
            {
                Name = (segments |> Seq.head) |> fst
                Value = 
                    match (segments |> Seq.head) |> snd with
                    | Some(v) -> v
                    | None -> String.Empty
                Metadata = NapCookieMetadata.Create requestUri (lowercaseSegmentKeys <| Map.ofSeq segments)
            }
            else
            {
                Name = String.Empty
                Value = String.Empty
                Metadata = NapCookieMetadata.Create requestUri (lowercaseSegmentKeys <| Map.ofSeq segments)
            }

    static member FromCookie (cookie:Cookie) =
        {
            Name = cookie.Name
            Value = cookie.Value
            Metadata = { CreationDate = cookie.TimeStamp
                         Expires = cookie.Expires |> Some
                         MaxAge = None
                         Domain = cookie.Domain
                         Path = cookie.Path
                         IsSecure = cookie.Secure
                         HttpOnly = cookie.HttpOnly
                         SameSite = SameSitePolicy.Unset
                       }
        }