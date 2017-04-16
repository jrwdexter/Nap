namespace Nap

open System
open System.Collections.Generic

/// <summary>
/// Metadata and flags associated with a cookie response.
/// </summary>
type NapCookieMetadata =
    {
        /// <summary>
        /// Gets the moment at which this cookie was generated.
        /// </summary>
        CreationDate: DateTime

        /// <summary>
        /// Gets the GMT moment at which the cookie expires.
        /// </summary>
        Expires: DateTime option

        /// <summary>
        /// Gets the max age that the cookie will be usable for, in seconds.
        /// </summary>
        MaxAge: int option

        /// <summary>
        /// Gets the domain that the cookie is valid for.
        /// </summary>
        Domain: string

        /// <summary>
        /// Gets the absolute path that the cookie is valid for.
        /// </summary>
        Path: string

        /// <summary>
        /// Gets a flag that if true indicates that the cookie should only be used in HTTPS/SSL requests.
        /// </summary>
        IsSecure: bool

        /// <summary>
        /// Gets the cookie is set to not be accessible through JavaScript.
        /// </summary>
        HttpOnly: bool

        /// <summary>
        /// Gets the level of protection the cookie offers for cross-site access.
        /// </summary>
        /// <remarks>Currently experimental API component.</remarks>
        SameSite: SameSitePolicy
    }
    with
    /// <summary>
    /// Create a new instance of a <see cref="NapCookieMetadata" /> object.
    /// </summary>
    /// <param name="hostUri">The host URI that originated the request.</param>
    /// <param name="directives">The collection of directives that the cookie contains.</param>
    static member Create (hostUri : Uri) (directives : Map<string, string option>) =
        let creationDate = DateTime.UtcNow

        // Expires
        let expires = 
            if (directives.ContainsKey("expires") && directives.["expires"] |> Option.isSome) then
                DateTime.Parse(directives.["expires"].Value) |> Some
            else
                None
        // Max-Age
        let maxAge = 
            if (directives.ContainsKey("max-age") && directives.["max-age"] |> Option.isSome) then
                let mutable maxAge = 0
                if (System.Int32.TryParse(directives.["max-age"].Value, &maxAge)) then
                    Some(maxAge)
                else
                    None
            else
                None
        // Domain
        let domain =
            if (directives.ContainsKey("domain")) then
                if directives.["domain"] |> Option.isNone then hostUri.Host else directives.["domain"].Value
            else
                hostUri.Host
        // Path
        let path = 
            if (directives.ContainsKey("path")) then
                if directives.["path"] |> Option.isNone then "/" else directives.["path"].Value
            else
                "/"
        // Path
        let isSecure = directives.ContainsKey("secure")

        // HttpOnly
        let httpOnly = directives.ContainsKey("httponly")

        // SameSite
        let sameSite =
            if (directives.ContainsKey("samesite") && directives.["samesite"] |> Option.isSome) then
                let mutable policy = SameSitePolicy.Unset
                if (Enum.TryParse(directives.["samesite"].Value, &policy)) then
                    policy
                else
                    SameSitePolicy.Unset
            else
                SameSitePolicy.Unset
        
        {
            CreationDate = creationDate
            Expires = expires
            MaxAge = maxAge
            Domain = domain
            Path = path
            IsSecure = isSecure
            HttpOnly = httpOnly
            SameSite = sameSite
        }

    member x.Url
        with get() =
            Uri(sprintf "http%s://%s/%s" (if x.IsSecure then "s" else String.Empty) (x.Domain.TrimEnd('/')) (x.Path.TrimStart('/')))
    
    member x.IsValid
        with get() =
            match x.MaxAge, x.Expires with
            | Some(age), _ -> DateTime.UtcNow < x.CreationDate.AddSeconds(float age)
            | _, Some(exp) -> DateTime.UtcNow < exp
            | None, None -> true