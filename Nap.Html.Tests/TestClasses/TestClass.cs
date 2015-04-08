using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Html.Attributes;

namespace Nap.Tests.TestClasses
{
    public class TestClass
    {
		[HtmlElement(".firstName")]
        public string FirstName { get; set; }
		[HtmlElement(".lastName")]
        public string LastName { get; set; }
    }
}