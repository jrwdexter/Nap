using Nap.Html.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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