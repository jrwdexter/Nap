using Nap.Html.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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
