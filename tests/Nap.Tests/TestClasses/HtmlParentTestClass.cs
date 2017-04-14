using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;
using Nap.Html.Attributes;

namespace Nap.Tests.TestClasses
{
    public class HtmlParentTestClass
    {
        [HtmlElement(".child")]
        public TestClass[] Children_Array { get; set; }

        [HtmlElement(".child")]
        public IEnumerable<TestClass> Children_Enumerable { get; set; }

#if IMMUTABLE
        [HtmlElement(".child")]
        public FSharpList<TestClass> Children_FSharpList { get; set; }
#endif

        [HtmlElement(".child")]
        public List<TestClass> Children_List { get; set; }

        [HtmlElement("#spouse")]
        public TestClass Spouse { get; set; }
    }
}
