using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;
using Nap.Html.Attributes;

namespace Nap.Tests.TestClasses
{
    public class ParentTestClass
    {
		[HtmlElement(".child")]
        public List<TestClass> Children { get; set; }

		[HtmlElement("#spouse")]
        public TestClass Spouse { get; set; }
    }
}
