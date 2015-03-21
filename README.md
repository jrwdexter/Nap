Nap
========


## Disclaimer

Nap is in Beta, and not ready to be used on production projects yet.

## Build Status
[![Master Build status](https://ci.appveyor.com/api/projects/status/hwj660imaji1hr4l/branch/master?svg=true&pendingText=master Building...&passingText=master OK&failingText=master Failed)](https://ci.appveyor.com/project/mandest/Nap/branch/master) [![Develop Build status](https://ci.appveyor.com/api/projects/status/hwj660imaji1hr4l/branch/develop?svg=true&pendingText=develop Building...&passingText=develop OK&failingText=develop Failed)](https://ci.appveyor.com/project/mandest/Nap/branch/develop)

## Summary

**Nap** is a short and sweet REST client.  It was made with the purpose of streamlining REST requests *as much as possible*.

## Plugins and Extensions
 - [Nap.Configuration](Nap.Configuration/README.md)
 - [Nap.Html](Nap.Html/README.md)

## Short

**Nap** is short.

```c#
var nap = new NapClient("http://example.com/");
var dogs = await nap.Get("/dogs").ExecuteAsync<Dogs>();
var cats = await nap.Get("http://otherexample.com/cats").ExecuteAsync<Cats>();
```

Or, even shorter!

```c#
var dogs = NapClient.Lets.Get("http://example.com/dogs").ExecuteAsync<Dogs>()
```

## Sweet

**Nap** is sweet.

```c#
var ingredients = new { Flour = 10, Eggs = 2, CakeMix = 1 };
var cake = NapClient.Lets.Post("http://example.com/bake-cake")
                         .IncludeQueryParameter("temp", "425F")
                         .IncludeHeader("sugar", "100g")
                         .IncludeBody(ingredients)
                         .Execute<Cake>();
```

## Configuration

**Nap** removes the need to configure stuff over and over again if you don't want to, or the need to create a client if you don't want one.  Nap follows a cascading configuration pattern: NapClient() > Fluent.

In this example:

```c#
var nap = new NapClient();
nap.Config.QueryParameters["temp"] = "300F";
var cake = nap.Get("http://example.com/cake")
              .IncludeHeader("sugar", "10g")
              .Execute<Cake>();
```

The end result would perform an HTTP GET Request to `http://example.com/cake`, using a query parameter of "temp=300F" (Nap() configuration level) and a Header of "sugar: 10g" (Fluent level).  This allows for a high degree of customization on a per-request or per-scope basis.

## Powerful

Most of all, **Nap** is aimed at bringing the full power of the RESTful requests to your projects.  Although few methods are exposed at the `INapRequest` level, the `INapRequest.Advanced` property quickly allows access to many more features:

```c#
var cake = NapClient.Lets.Get("http://example.com/cake")
                   .Advanced
                   .Proxy("http://localhost:8888")
                   .Authentication.Basic("jdoe@example.com", "Password")
                   .UseSSL()
                   .Execute<Cake>()
```

Metadata properties are supported!

```c#
class Cake
{
  public bool Tasty { get; set; }
  public string Type { get; set; }
  public int StatusCode { get; set; }
}

var cake = NapClient.Lets.Get("http://example.com/cake")
                         .FillMetadata()
                         .Execute<Cake>()
```

In the above example, if the URL cake has no property called "Status Code", it would be populated by the `FillMetadata()` flag, and result in the status code of the request.

Deserialization is performed by default through Json.Net and XmlSerializer.  Additional formatters for deserialization can be implemented by inheriting from the `INapFormatter` interface.
