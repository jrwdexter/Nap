namespace Nap

open System
open System.Net
open System.Net.Http

open Finalizable

type NapRequest =
    {
        Config          : NapConfig
        RequestEvents   : Map<string, Event<NapRequest> list>
        Url             : string
        Method          : HttpMethod
        QueryParameters : Map<string, string>
        Headers         : Map<string, string>
        Cookies         : (Uri*Cookie) list
        Content         : string
    }
    with
    (*** Properties ***)
    static member internal Default =
        {
            Config = NapConfig.Default
            RequestEvents = Map.empty
            Url = ""
            Method = HttpMethod.Get
            QueryParameters = Map.empty
            Headers = Map.empty
            Cookies = List.empty
            Content = ""
        }

    (*** Config helpers ***)
    member x.ApplyConfig config =
        x.Verbose "NapRequest.ApplyConfig()" <| sprintf "Applying configuration %A" config
        Continuing(x)
        |> NapRequest.RunEvents "BeforeConfigurationApplied"
        <!> fun request ->
            { request with
                Config = config
                Url = config.BaseUri
                QueryParameters = x.Config.QueryParameters |> Map.join config.QueryParameters
                Headers = x.Config.Headers |> Map.join config.Headers
            }
        |> NapRequest.RunEvents "AfterConfigurationApplied"
        |> Finalizable.get

    (*** Constructors ***)
    static member Create () =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents "CreatingNapRequest"
        |> Finalizable.get
        |> fun x -> x :> INapRequest
    static member Create config =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents "CreatingNapRequest"
        |> Finalizable.get
        |> fun x -> x.ApplyConfig(config)
        |> fun x -> x :> INapRequest
    static member Create (config,uri) =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents "CreatingNapRequest"
        <!> fun x -> x.ApplyConfig(config)
        |> Finalizable.get
        |> fun x -> { x with Url = uri }
        |> fun x -> x :> INapRequest
    static member Create (config,uri,httpMethod) =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents "CreatingNapRequest"
        <!> fun x -> x.ApplyConfig(config)
        |> Finalizable.get
        |> fun x -> { x with Url = uri; Method = httpMethod}
        |> fun x -> x :> INapRequest

    (*** Event helpers ***)
    static member internal RunEvents eventName (finalizableRequest:FinalizableType<NapRequest>) =
        let request = finalizableRequest |> Finalizable.get
        request.Debug "Nap.NapRequest.RunEvents()" (sprintf "Running events for %s on %A" eventName request)
        let result = request.RequestEvents.[eventName] |> List.fold (fun r event -> r >>= event.RunEvent) finalizableRequest
        if eventName = "*" then result else result |> NapRequest.RunEvents "*"
        
    (*** Logging helpers ***)
    member private x.Log logLevel path message =
        x.Config.Log logLevel path message
    member private x.Verbose = x.Log LogLevel.Verbose
    member private x.Debug path message = x.Log LogLevel.Debug path message
    member private x.Info = x.Log LogLevel.Info
    member private x.Warn = x.Log LogLevel.Warn
    member private x.Error = x.Log LogLevel.Error
    member private x.Fatal path message = x.Log LogLevel.Fatal path message; failwith (sprintf "[%s] %s" path message)

    (*** INapRequest interface implementation ***)
    interface INapRequest with
        member x.Advanced(): IAdvancedNapRequestComponent = 
            x :> IAdvancedNapRequestComponent

        member x.DoNot(): IRemovableNapRequestComponent = 
            x :> IRemovableNapRequestComponent

        member x.Execute(): 'T = 
            failwith "Not implemented yet"

        member x.Execute(): string = 
            failwith "Not implemented yet"

        member x.ExecuteAsync(): Async<'T> = 
            failwith "Not implemented yet"

        member x.ExecuteAsync(): Async<string> = 
            failwith "Not implemented yet"

        member x.FillMetadata(): INapRequest = 
            x.Verbose "Nap.NapRequest.FillMetadata()" "Flagging metadata to be filled."
            upcast { x with Config = { x.Config with FillMetadata = true } }

        member x.IncludeBody(body: obj): INapRequest = 
            if body |> isNull then
                x.Verbose "Nap.NapRequest.IncludeBody()" (sprintf "Null object found; serializing nothing.")
                x :> INapRequest
            else
                x.Verbose "Nap.NapRequest.Includebody()" (sprintf "Serializing %O" body)
                x.Debug "Nap.NapRequest.Includebody()" (sprintf "Serializing object of type %s" (body.GetType().FullName))
                Continuing(x)
                |> NapRequest.RunEvents "BeforeRequestSerialization"
                <!> fun request ->
                    match request.Config.Serializers |> List.tryFind (fun s -> s.ContentType = request.Config.Serialization) with
                    | Some(serializer) -> { request with Content = serializer.Serialize(body) }
                    | None -> 
                        x.Error "Nap.NapRequest.IncludeBody()" (sprintf "Could not serialize content of type %s with serializer %s" (body.GetType().ToString()) x.Config.Serialization)
                        request
                |> NapRequest.RunEvents "AfterRequestSerialization"
                |> fun r -> upcast Finalizable.get r
        member x.IncludeCookie(url: string) (cookieName: string) (value: string): INapRequest = 
            x.Verbose "Nap.NapRequest.IncludeCookie()" (sprintf "Adding cookie for URL \"%s\". %s: %s" url cookieName value)
            upcast { x with Cookies = x.Cookies |> List.append ([Uri(url), new Cookie(cookieName, value, url)]) }
        member x.IncludeHeader(name: string) (value: string): INapRequest = 
            x.Verbose "Nap.NapRequest.IncludeCookie()" (sprintf "Adding header: %s: %s" name value)
            upcast { x with Headers = x.Headers |> Map.add name value }
        member x.IncludeQueryParameter(name: string) (value: string): INapRequest = 
            x.Verbose "Nap.NapRequest.IncludeCookie()" (sprintf "Adding query parameter: %s=%s" name value)
            upcast { x with QueryParameters = x.QueryParameters |> Map.add name value }

    interface IAdvancedNapRequestComponent with
        member x.Configuration : NapConfig = 
            x.Config
        member x.Authentication: AuthenticationNapConfig = 
            x.Config.Advanced.Authentication
        member x.ClientCreator: obj -> HttpClient = 
            x.Config.Advanced.ClientCreator
        member x.Proxy: ProxyNapConfig = 
            x.Config.Advanced.Proxy
        member x.SetAuthentication(authenticationScheme: AuthenticationNapConfig): INapRequest = 
            x.Debug "Nap.NapRequest.SetAuthentication()" (sprintf "Setting authentication as %A" authenticationScheme)
            Continuing(x)
            |> NapRequest.RunEvents "SetAuthentication"
            |> Finalizable.get
            |> fun r ->
                upcast { r with Config = { r.Config with Advanced = { r.Config.Advanced with Authentication = authenticationScheme }}}
        member x.SetClientCreator(clientCreator : INapRequest -> HttpClient): INapRequest = 
            let boxedCreator = unbox>>clientCreator
            upcast { x with Config = { x.Config with Advanced = { x.Config.Advanced with ClientCreator = boxedCreator }}}
        member x.SetProxy(proxy: ProxyNapConfig): INapRequest = 
            x.Debug "Nap.NapRequest.SetProxy()" (sprintf "Setting proxy as %A" proxy)
            upcast { x with Config = { x.Config with Advanced = { x.Config.Advanced with Proxy = proxy }}}
        
        
    interface IRemovableNapRequestComponent with
        member x.FillMetadata(): INapRequest = 
            { x with Config = {x.Config with FillMetadata = false } } :> INapRequest
        member x.IncludeBody(): INapRequest = 
            upcast { x with Content = "" }
        member x.IncludeCookie(cookieName: string): INapRequest = 
            upcast { x with Cookies = x.Cookies |> List.filter (fun (_,c) -> c.Name <> cookieName) }
        member x.IncludeHeader(headerName: string): INapRequest = 
            upcast { x with Headers = x.Headers |> Map.remove headerName }
        member x.IncludeQueryParameter(queryParameterName: string): INapRequest = 
            upcast { x with QueryParameters = x.QueryParameters |> Map.remove queryParameterName }

    override x.ToString() =
        sprintf "%A" x
