Nap
========

## Disclaimer

Nap is in Beta, and not ready to be used on production projects yet.

## Summary

**Nap** is a short and sweet REST client.  It was made with the purpose of streamlining REST requests *as much as possible*.

## Short

**Nap** is short.

```c#
var nap = new Nap("http://example.com/");
var dogs = await nap.Get<Dogs>("/dogs").ExecuteAsync();
var cats = await nap.Get<Cats>("http://otherexample.com/cats").ExecuteAsync();
```

Or, even shorter!

```c#
var dogs = Nap.Lets.Get<Dogs>("http://example.com/dogs").ExecuteAsync()
```

## Sweet

**Nap** is sweet.

```c#
var ingredients = new { Flour = 10, Eggs = 2, CakeMix = 1 };
var cake = Nap.Lets.Post<Cake>("http://example.com/bake-cake")
                   .IncludeQueryParameter("temp", "425F")
                   .IncludeHeader("sugar", "100g")
                   .IncludeBody(ingredients)
                   .Execute();
```

## Configurable

**Nap** removes the need to configure stuff over and over again if you don't want to, or the need to create a client if you don't want one.  Your `App.config` or `Web.config` file can include:

```xml
<configuration>
  <configSections>
    <section 
      name="nap" 
      type="Nap.Configuration.NapConfig" 
      allowLocation="true" 
      allowDefinition="Everywhere"
    />
  </configSections>
</configuration>
...
<Nap baseUrl="http://example.com" fillMetada="true">
  <Headers>
    <add key="sugar" value="100g" />
  </Headers>
  <QueryParameters>
    <add key="temp" value="425F" />
  </QueryParameters>
  <Advanced useSsl="true">
    <Proxy address="http://localhost:8080" />
    <Authentication username="jdoe" password="password123" />
  </Advanced>
</Nap>
```

Along with a simple Configuration declaration:
```c#
NapSetup.AddConfig(NapConfig.GetCurrent());
```

And make your **Naps** even easier:

```c#
var ingredients = new { Flour = 10, Eggs = 2, CakeMix = 1 };
Nap.Lets.Post<Cake>("/bake-cake")
        .IncludeBody(ingredients)
        .Execute();
```

And still fully malleable:

```c#
Nap.Lets.Get<Potato>("/potato")
        .DoNot.IncludeHeader("sugar")
        .Execute();
```

Nap supports 3 levels of cascading configuration: *.config < Nap() < Fluent.  In this example:

```xml
...
<nap baseUrl="http://example.com">
  <headers>
    <add key="sugar" value="100g" />
  </headers>
  <queryParameters>
    <add key="temp" value="425F" />
  </queryParameters>
</nap>
...
```

```c#
var nap = new Nap();
nap.Config.QueryParameters["temp"] = "300F";
var cake = nap.Get<Cake>("/cake")
              .IncludeHeader("sugar", "10g")
              .Execute();
```

The end result would perform an HTTP GET Request to `http://example.com/cake` (from *.config), using a query parameter of "temp=300F" (Nap() configuration level) and a Header of "sugar: 10g" (Fluent level).  This allows for a high degree of customization on a per-request or per-scope basis.

## Powerful

Most of all, **Nap** is aimed at bringing the full power of the RESTful requests to your projects.  Although few methods are exposed at the `INapRequest` level, the `INapRequest.Advanced` property quickly allows access to many more features:

```c#
var cake = Nap.Lets.Get<Cake>("http://example.com/cake")
                   .Advanced
                   .Proxy("http://localhost:8888")
                   .Authentication.Basic("jdoe@example.com", "Password")
                   .UseSSL()
                   .Execute()
```

Metadata properties are supported!

```c#
class Cake
{
  public bool Tasty { get; set; }
  public string Type { get; set; }
  public int StatusCode { get; set; }
}

var cake = Nap.Lets.Get<Cake>("http://example.com/cake")
                   .FillMetadata()
                   .Execute()
```

In the above example, if the URL cake has no property called "Status Code", it would be populated by the `FillMetadata()` flag, and result in the status code of the request.

Deserialization is performed by default through Json.Net and XmlSerializer.  Additional formatters for deserialization can be implemented by inheriting from the `INapFormatter` interface.
