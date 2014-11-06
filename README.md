Nap
========

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

## Fast

**Nap** removes the need to configure stuff over and over again if you don't want to, or the need to create a client if you don't want one.  Your `App.config` or `Web.config` file can include:

```xml
<configuration>
  <configSections>
    <section 
      name="Nap" 
      type="Napper.Configuration.NapConfig" 
      allowLocation="true" 
      allowDefinition="Everywhere"
    />
  </configSections>
</configuration>
...
<Nap>
  <BaseUrl>http://example.com</BaseUrl>
  <Headers>
    <Header key="sugar" value="100g" />
  </Headers>
  <QueryParameters>
    <QueryParameter key="temp" value="425F" />
  </QueryParameters>
  <Advanced>
    <Proxy>...</Proxy>
    <Credentials>...</Credentials>
    <UseSSL>true</UseSSL>
  </Advanced>
</Nap>
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

## Powerful

