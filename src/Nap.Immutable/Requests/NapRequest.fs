namespace rec Nap

open System
open System.Collections.Generic
open System.IO
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Reflection
open System.Threading.Tasks

open Nap.Serializers.Base

open Finalizable
open AsyncFinalizable
open Operators
open Text
    
/// <summary>
/// A basic parsed response from a server.
/// Contains the most basic information necessary from a server - StatusCode, Headers (Including Cookies
/// </summary>
type NapResponse =
    {
        /// <summary>
        /// Gets the request that generated this response.
        /// </summary>
        Request: NapRequest

        /// <summary>
        /// Gets the URI that the request was made against.
        /// </summary>
        Url: Uri

        /// <summary>
        /// The status code for the response.
        /// </summary>
        StatusCode: HttpStatusCode

        /// <summary>
        /// The key/value pair collection of headers present in the response.
        /// Also contains all <see cref="ContentHeaders"/> and <see cref="NonContentHeaders"/> in a more raw form.
        /// </summary>
        Headers: Map<string, string>

        /// <summary>
        /// Get content headers for more in-depth use.
        /// All of these headers are also contained within <see cref="Headers"/>.
        /// </summary>
        ContentHeaders: HttpContentHeaders

        /// <summary>
        /// Get non-content headers for more in-depth use.
        /// All of these headers are also contained within <see cref="Headers"/>.
        /// </summary>
        NonContentHeaders: HttpResponseHeaders

        /// <summary>
        /// Gets the set of headers that are of key 'Set-Cookie' and casts them to cookies.
        /// </summary>
        Cookies: NapCookie list

        /// <summary>
        /// Gets the body of the response.
        /// </summary>
        Body: string

        /// <summary>
        /// Gets the deserialized object.
        /// </summary>
        Deserialized: obj option
    }
    with
    static member Create (request : NapRequest) (response : HttpResponseMessage) (body : string) = 
        let headers = response.Headers |> Seq.append (response.Content.Headers)
                                       |> Seq.collect (fun h -> h.Value |> Seq.map (fun v -> (h.Key, v)))
                                       |> Map.ofSeq
        {
            Request = request
            Url = new Uri(request.Url)
            StatusCode = response.StatusCode
            Headers = headers
            Cookies = headers |> Seq.filter (fun h -> h.Key.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                              |> Seq.map (fun h -> NapCookie.create (new Uri(request.Url)) (h.Value))
                              |> Seq.toList
            Body = body
            ContentHeaders = response.Content.Headers
            NonContentHeaders = response.Headers
            Deserialized = None
        }

    /// <summary>
    /// Get the value of a header for this response.
    /// </summary>
    /// <param name="headerName">The name of the header value to return.</param>
    /// <returns>Returns the value of the given header.</returns>
    member x.GetHeader (headerName : string) = x.Headers |> Map.tryFind headerName

    /// <summary>
    /// Get the value of all headers matching a header name for this response.
    /// </summary>
    /// <param name="headerName">The name of the header value to return.</param>
    /// <returns>Returns the value of the given header.</returns>
    member x.GetHeaders (headerName : string) = x.Headers |> Map.filter (fun k _ -> k.Equals(headerName, StringComparison.OrdinalIgnoreCase))

    /// <summary>
    /// Get the cookie matching a specific name.
    /// </summary>
    /// <param name="cookieName">The name of the cookie to find.</param>
    /// <returns>The cookie that matches <paramref name="cookieName"/>.</returns>
    member x.GetCookie (cookieName : string) = x.Cookies |> Seq.tryFind(fun c -> c.Name.Equals(cookieName, StringComparison.OrdinalIgnoreCase))

and NapRequest =
    {
        Config          : NapConfig
        Response        : NapResponse option
        RequestEvents   : Map<EventCode, Event<NapRequest> list>
        Url             : string
        Method          : HttpMethod
        QueryParameters : Map<string, string>
        Headers         : Map<string, string>
        Cookies         : (Uri*Cookie) list
        Content         : string
        ClientCreator   : INapRequest -> HttpClient
    }
    with
    (*** Properties ***)
    static member private CreateClient (irequest:INapRequest) = 
        match irequest with
        | :? NapRequest as request ->
            let handler = new HttpClientHandler()
            let request = downcast irequest
            match request.Config.Advanced.Proxy with
            | Some(proxy) -> () // TODO: Add proxy
            | None -> ()
            for (uri,cookie) in request.Cookies do
                handler.CookieContainer.Add(uri, cookie)
            new HttpClient(handler)
        | _ ->
            new HttpClient()

    static member internal Default =
        {
            Config = NapConfig.Default
            Response = None
            RequestEvents = Map.empty
            Url = ""
            Method = HttpMethod.Get
            QueryParameters = Map.empty
            Headers = Map.empty
            Cookies = List.empty
            Content = ""
            ClientCreator = NapRequest.CreateClient
        }

    (*** Config helpers ***)
    member x.ApplyConfig config =
        x.Verbose "NapRequest.ApplyConfig()" <| sprintf "Applying configuration %A" config
        Continuing(x)
        |> NapRequest.RunEvents EventCode.BeforeConfigurationApplied
        <!> fun request ->
            { request with
                Config = config
                QueryParameters = request.Config.QueryParameters |> Map.join config.QueryParameters
                Headers = request.Config.Headers |> Map.join config.Headers
                ClientCreator = 
                    match config.Advanced.ClientCreator with
                    | Some(creator) -> fun nr -> creator (box nr)
                    | None -> NapRequest.CreateClient
            }
        |> NapRequest.RunEvents EventCode.AfterConfigurationApplied
        |> Finalizable.get

    (*** Constructors ***)
    static member Create () =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents EventCode.CreatingNapRequest
        |> Finalizable.get
        |> fun x -> x :> INapRequest
    static member Create config =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents EventCode.CreatingNapRequest
        |> Finalizable.get
        |> fun x -> x.ApplyConfig(config)
        |> fun x -> x :> INapRequest
    static member Create (config,uri) =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents EventCode.CreatingNapRequest
        <!> fun x -> x.ApplyConfig(config)
        |> Finalizable.get
        |> fun x -> { x with Url = uri }
        |> fun x -> x :> INapRequest
    static member Create (config,uri,httpMethod) =
        NapRequest.Default
        |> Continuing
        |> NapRequest.RunEvents EventCode.CreatingNapRequest
        <!> fun x -> x.ApplyConfig(config)
        |> Finalizable.get
        |> fun x -> { x with Url = uri; Method = httpMethod}
        |> fun x -> x :> INapRequest

    (*** Event helpers ***)
    static member internal RunEventsAsync eventName asyncFinalizableRequest =
        async {
            let! finalizableRequest = asyncFinalizableRequest
            return NapRequest.RunEvents eventName finalizableRequest
        }

    static member internal RunEvents eventName (finalizableRequest:FinalizableType<NapRequest, _>) =
        match finalizableRequest with
        | Continuing(request) ->
            let result =
                match request.RequestEvents |> Map.tryFind eventName with
                | Some(event) ->
                    request.Debug "Nap.NapRequest.RunEvents()" (sprintf "Running events for %A on %A" eventName request)
                    event |> List.fold (fun r event -> r >>= event.RunEvent) (Continuing(request))
                | None -> finalizableRequest
            if eventName = EventCode.All then result else result |> NapRequest.RunEvents EventCode.All
        | Final(_) -> finalizableRequest
        
    (*** Logging helpers ***)
    member private x.Log logLevel path message =
        x.Config.Log logLevel path message
    member private x.Verbose = x.Log LogLevel.Verbose
    member private x.Debug path message = x.Log LogLevel.Debug path message
    member private x.Info = x.Log LogLevel.Info
    member private x.Warn = x.Log LogLevel.Warn
    member private x.Error = x.Log LogLevel.Error
    member private x.Fatal path message = x.Log LogLevel.Fatal path message; failwith (sprintf "[%s] %s" path message)

    (*** Request running implementation ***)
    member private x.CreateUrl () =
        let pairToQueryStringValue (k,v) =
            sprintf "%s=%s" k <| WebUtility.UrlEncode(v)
        x.Url |?? String.Empty
        |> fun url ->
            match url with
            | Prefix "http" _ -> url
            | _ -> new Uri(new Uri(x.Config.BaseUrl), url) |> string
        |> fun url ->
            match x.QueryParameters |> Map.toArray with 
            | [||] -> url
            | parameters -> sprintf "%s?%s" (url.TrimEnd('?')) <|
                                (parameters |> Array.map pairToQueryStringValue |> fun l -> String.Join("&", l))

    member private x.GetSerializer contentType =
        let serializer = x.Config.Serializers |> Map.tryFind contentType
        let seperator = "/"
        match serializer with
        | Some(s) -> s |> Some
        | None -> 
            match contentType with
            | Split seperator (_::tail)  -> x.GetSerializer (tail |> String.concat seperator)
            | _ -> None

    static member private RunRequestAsync request =
        async {
            let excludedHeaders = ["content-type"] |> Set.ofList
            use client = request.ClientCreator (upcast request)
            let requestMessage = new HttpRequestMessage(request.Method, request.CreateUrl())
            let allowedDefaultHeaders = request.Headers |> Map.filter (fun headerName _ -> excludedHeaders |> Set.contains(headerName.ToLower()) |> not)
            for (h,v) in allowedDefaultHeaders |> Map.toSeq do
                client.DefaultRequestHeaders.Add(h, v)
            if request.Method <> HttpMethod.Get && request.Method <> HttpMethod.Trace then
                let content = new StringContent(request.Content |?? "")
                requestMessage.Content <- content
                match request.Headers |> Map.toSeq |> Seq.tryFind (fun (k,_) -> k.Equals("content-type", StringComparison.OrdinalIgnoreCase)) with
                | Some((_,v)) -> content.Headers.ContentType.MediaType <- v
                | None -> content.Headers.ContentType.MediaType <- (request.Config.Serializers.[request.Config.Serialization]).ContentType
            let! response = client.SendAsync(requestMessage) |> Async.AwaitTask
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return { request with Response = Some(NapResponse.Create request response content) }
        }

    static member private Deserialize<'T> request =
        let path = sprintf "NapRequest.RunReqest()/NapRequest.Deserialize<%s>() [%s]" typeof<'T>.FullName request.Url
        match request.Response with
            | Some(response) ->
                let serializerOption = request.GetSerializer <| response.ContentHeaders.ContentType.MediaType
                match serializerOption with
                | Some(serializer) -> 
                    request.Verbose path <| sprintf "Deserializing content with %s deserializer" (serializer.GetType().FullName)
                    { request with
                        Response =
                        {
                            response with
                                Deserialized =
                                    serializer.Deserialize<'T> response.Body
                                    |> fun o -> 
                                            match o with
                                            | None -> 
                                                let parameterlessConstructor =
                                                    typeof<'T>.GetTypeInfo().DeclaredConstructors
                                                    |> Seq.tryFind (fun c -> c.GetParameters().Length = 0 && not <| c.ContainsGenericParameters)
                                                parameterlessConstructor |> Option.bind (fun c -> c.Invoke(Array.empty) :?> 'T |> Some)
                                            | x -> x
                                    |> Option.bind (box>>Some)
                        } |> Some
                    }
                | None ->
                    request.Warn path <| sprintf "No deserializer found for content type '%s'" response.ContentHeaders.ContentType.MediaType
                    let emptyDeserializedResponse =
                        typeof<'T>.GetTypeInfo().DeclaredConstructors
                        |> Seq.tryFind (fun c -> c.GetParameters().Length = 0 && not <| c.ContainsGenericParameters)
                        |> Option.map (fun c -> c.Invoke(Array.empty) :?> 'T)
                        |> Option.map (box)
                    { request with Response = { response with Deserialized = emptyDeserializedResponse } |> Some }
            | None ->
                request.Warn path "No response recieved."
                request

    static member private FillMetadata<'T> request =
        match request.Config.FillMetadata, request.Response, request.Response |> Option.bind(fun r -> r.Deserialized) with
        | true,Some(response),Some(deserializedObj) ->
            let deserialized = unbox<'T> deserializedObj
            let statusCodeProperty = typeof<'T>.GetRuntimeProperty("StatusCode")
            if isNull statusCodeProperty |> not then
                statusCodeProperty.SetValue(deserialized, Convert.ChangeType(response.StatusCode, statusCodeProperty.PropertyType))
            let cookieProperty = typeof<'T>.GetRuntimeProperty("Cookies")
            if isNull cookieProperty |> not then
                cookieProperty.SetValue(deserialized, response.Cookies)
            { request with Response = { response with Deserialized = Some(box deserialized) } |> Some } 
        | _ -> request

    member x.ExecuteFSTypedAsync<'T> () =
        async {
            let path = sprintf "NapRequest.Execute() [%s]" x.Url
            x.Info path <| sprintf "Executing %O request with body %s" x.Method x.Content
            x.Verbose path <| sprintf "Executing %O request with parameters full structure %A" x.Method x
            let! processedRequest =
                async.Return <| Continuing(x)
                |> NapRequest.RunEventsAsync EventCode.BeforeRequestExecution
                |> AsyncFinalizable.bindAsync (NapRequest.RunRequestAsync)
                |> NapRequest.RunEventsAsync EventCode.AfterRequestExecution
                |> NapRequest.RunEventsAsync EventCode.BeforeResponseDeserialization
                <!!> fun response -> NapRequest.Deserialize<'T> response
                |> NapRequest.RunEventsAsync EventCode.AfterResponseDeserialization
                |> AsyncFinalizable.get
                |> Async.map (fun request -> NapRequest.FillMetadata<'T> request)
            let deserializedObj = processedRequest.Response |> Option.bind (fun response -> response.Deserialized)
            return
                match deserializedObj with
                | Some(deserialized) -> unbox<'T> deserialized |> Some
                | None -> None
        }
        
            
    member x.ExecuteFSAsync () = 
        async {
            let path = sprintf "NapRequest.Execute() [%s]" x.Url
            x.Info path <| sprintf "Executing %O request with body %s" x.Method x.Content
            x.Verbose path <| sprintf "Executing %O request with parameters full structure %A" x.Method x
            let! processedRequest =
                async.Return <| Continuing(x)
                |> NapRequest.RunEventsAsync EventCode.BeforeRequestExecution
                |> AsyncFinalizable.bindAsync (NapRequest.RunRequestAsync)
                |> NapRequest.RunEventsAsync EventCode.AfterRequestExecution
                |> AsyncFinalizable.get
            return processedRequest.Response |> Option.bind (fun response -> response.Body |> Some)
        }

    (*** INapRequest interface implementation ***)
    interface INapRequest with
        member x.Advanced(): IAdvancedNapRequestComponent = 
            x :> IAdvancedNapRequestComponent

        member x.DoNot(): IRemovableNapRequestComponent = 
            x :> IRemovableNapRequestComponent

        member x.Execute(): 'T = 
            let result = x.ExecuteFSTypedAsync () |> Async.RunSynchronously
            defaultArg result Unchecked.defaultof<'T>

        member x.Execute(): string = 
            let result = x.ExecuteFSAsync () |> Async.RunSynchronously
            defaultArg result null

        member x.ExecuteAsync<'T> () : Task<'T> =
            async {
                let! result = x.ExecuteFSTypedAsync<'T> ()
                return defaultArg result Unchecked.defaultof<'T>
            } |> Async.StartAsTask

        member x.ExecuteAsync(): Task<string> = 
            async {
                let! result = x.ExecuteFSTypedAsync<string> ()
                return defaultArg result null
            } |> Async.StartAsTask

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
                |> NapRequest.RunEvents EventCode.BeforeRequestSerialization
                <!> fun request ->
                    match request.Config.Serializers |> Map.tryFind request.Config.Serialization with
                    | Some(serializer) -> { request with Content = serializer.Serialize(body) }
                    | None -> 
                        x.Error "Nap.NapRequest.IncludeBody()" (sprintf "Could not serialize content of type %s with serializer %s" (body.GetType().ToString()) x.Config.Serialization)
                        request
                |> NapRequest.RunEvents EventCode.AfterRequestSerialization
                |> fun r -> upcast Finalizable.get r
        member x.IncludeCookie(url: string) (cookieName: string) (value: string): INapRequest = 
            x.Verbose "Nap.NapRequest.IncludeCookie()" (sprintf "Adding cookie for URL \"%s\". %s: %s" url cookieName value)
            let uri = new Uri(url)
            upcast { x with Cookies = x.Cookies |> List.append ([uri, new Cookie(cookieName, value, uri.AbsolutePath, uri.Host)]) }
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
        member x.Proxy =
            x.Config.Advanced.Proxy
        member x.ClientCreator = 
            x.ClientCreator
        member x.SetClientCreator clientCreator =
            upcast { x with ClientCreator = (fun nr -> clientCreator.Invoke nr) }
        member x.SetAuthentication(authenticationScheme: AuthenticationNapConfig): INapRequest = 
            x.Debug "Nap.NapRequest.SetAuthentication()" (sprintf "Setting authentication as %A" authenticationScheme)
            Continuing(x)
            |> NapRequest.RunEvents EventCode.SetAuthentication
            |> Finalizable.get
            |> fun r ->
                upcast { r with Config = { r.Config with Advanced = { r.Config.Advanced with Authentication = authenticationScheme }}}
        member x.SetProxy(proxy: ProxyNapConfig): INapRequest = 
            x.Debug "Nap.NapRequest.SetProxy()" (sprintf "Setting proxy as %A" proxy)
            upcast { x with Config = { x.Config with Advanced = { x.Config.Advanced with Proxy = proxy |> Some }}}
        
        
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
