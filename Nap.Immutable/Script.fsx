// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "../Nap/bin/Debug/Nap.dll"
#r "../Nap.Html/bin/Debug/Nap.Html.dll"
open Nap.Html
open Nap.Html.Attributes
type Person = 
    {
        [<HtmlElement(".firstName")>] FirstName : string
        [<HtmlElement(".lastName")>] LastName : string
    }

type Parent =
    {
        [<HtmlElement(".firstName")>] FirstName : string
        [<HtmlElement(".firstName")>] LastName : string
        [<HtmlElement(".children")>] Child : Person list
    }

let html = """
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <title>Document</title>
    </head>
    <body>
        <div id="spouse">
            <div class="firstName">Jeff</div>
            <div class="lastName">Doe</div>
        </div>
        <ol id="children">
            <li class="child">
                <div class="firstName">John</div>
                <div class="lastName">Doe</div>
            </li>
            <li class="child">
                <div class="firstName">Jane</div>
                <div class="lastName">Doe</div>
            </li>
        </ol>
    </body>
    </html>
"""

let htmlSerializer = new Nap.Html.NapHtmlSerializer()
htmlSerializer.Deserialize<Parent>(html)

