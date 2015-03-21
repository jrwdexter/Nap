# Nap.Configuration

## Setup

Make a simple sonfiguration declaration:
```c#
NapSetup.AddConfig(NapConfig.GetCurrent);
```

## Usage

**Nap.Configuration** removes the need to configure stuff over and over again if you don't want to, or the need to create a client if you don't want one.  Your `App.config` or `Web.config` file can include:

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
<nap baseUrl="http://example.com" fillMetada="true">
  <headers>
    <add key="sugar" value="100g" />
  </headers>
  <queryParameters>
    <add key="temp" value="425F" />
  </queryParameters>
  <advanced useSsl="true">
    <proxy address="http://localhost:8080" />
    <authentication username="jdoe" password="password123" />
  </advanced>
</nap>
```

And make your **Naps** even easier:

```c#
var ingredients = new { Flour = 10, Eggs = 2, CakeMix = 1 };
NapClient.Lets.Post("/bake-cake")
              .IncludeBody(ingredients)
              .Execute<Cake>();
```

And still fully malleable:

```c#
NapClient.Lets.Get("/potato")
              .DoNot.IncludeHeader("sugar")
              .Execute<Potato>();
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
var nap = new NapClient();
nap.Config.QueryParameters["temp"] = "300F";
var cake = nap.Get("/cake")
              .IncludeHeader("sugar", "10g")
              .Execute<Cake>();
```

The end result would perform an HTTP GET Request to `http://example.com/cake` (from *.config), using a query parameter of "temp=300F" (Nap() configuration level) and a Header of "sugar: 10g" (Fluent level).  This allows for a high degree of customization on a per-request or per-scope basis.
