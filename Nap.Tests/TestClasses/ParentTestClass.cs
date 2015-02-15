using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Tests.TestClasses
{
    public class ParentTestClass
    {
        public IEnumerable<TestClass> Children { get; set; }

        public TestClass Spouse { get; set; }
    }
}
