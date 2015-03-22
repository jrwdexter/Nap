Nap.Html
========

## Setup

If `Nap.Configuration` is installed, adding the following to the *.config file of your application will register Nap.Html for use:
```xml
...
  <nap>
    <formatters>
      <add contentType="text/html" formatterType="Nap.Html.NapHtmlFormatter, Nap.Html" />
    </formatters>
  </nap>
...
```

Otherwise, fluent configuration may be performed:

```c#
var nap = new NapClient();
nap.Config.Formatters.Add(new NapHtmlFormatter());
```

## Usage

**Nap.Html** is a library that performs object binding, but instead of having a well-formed dataset (such as JSON or XML), on HTML.

For example, if the page `http://example.com/person` returns

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Person</title>
</head>
<body>
  <div id="person">
    <span id="firstName">John</span>
    <span class="lastName">Doe</span>
  </div>
</body>
</html>
```

with content-type "text/html".  We can create a simple poco object

```c#
public class Person
{
  [HtmlElement("#firstName")]
  public string FirstName { get; set; }

  [HtmlElement("#person .lastName")]
  public string LastName { get; set; }
}
```

and run the query `await nap.Get("http://example.com/person").ExecuteAsync<Person>()`.  **Nap** detects the returning *ContentType*, and selects the proper formatter's deserialization method to use.  In this case, the `NapHtmlFormatter.Deserialize()` method is used.  Child properties and enumerable types are also supported, so that

```html
  <div id="person">
    <span id="firstname">John</span>
    <span class="lastName">Doe</span>
    <div id="children">
      <div class="child">...</div>
      <div class="child">...</div>
    </div>
  </div>
```

could be bound to

```c#
public class Person
{
  [HtmlElement(".firstName")]
  public string FirstName { get; set; }

  [HtmlElement("#person .lastName")]
  public string LastName { get; set; }

  [HtmlElement(".child")]
  public List<Person> Children { get; set; }
}
```
## Advanced

### Binding Formats

Beyond simple binding, **Nap.Html** allows for selection of the binding behavior.  Binding types include:

 - `BindingFormat.InnerHtml`
 - `BindingFormat.InnerText`
 - `BindingFormat.Attribute`
 - `BindingFormat.Class`
 - `BindingFormat.Id`
 - `BindingFormat.Value`
 - `BindingFormat.Smart`

Class, Id and Value are just special cases of the `BindingFormat.Attribute` type that shortcut the need to enter an attribute.  `BindingFormat.Smart` performs a set of operations, in order:
 1. Attempt to find an `option[selected]` element, and bind to its InnerText
 2. Bind to the `value` attribute of the current element, or an input within the current element's inner html.
 3. Bind to the inner textg of the current element.

As `BindingFormat.Smart` is the default behavior, this allows for relative ease in ensuring that data is getting bound correctly.

### NumericalHtmlElementAttribute

Beyond the basic `HtmlElementAttribute`, there is also a `NumericalHtmlElementAttribute` that offers additional configuration for numerical properties.