using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nap.Html;
using Nap.Html.Attributes;
using Nap.Tests.Serializers;
using Nap.Tests.Serializers.Base;
using Xunit;

namespace Nap.Tests.Issues
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Serialization")]
    [Trait("Class", "HtmlSerializer")]
    public class Issue5 : NapSerializerTestBase
    {
        private readonly NapHtmlSerializer _htmlSerializer;

        public Issue5()
        {
            _htmlSerializer = new NapHtmlSerializer();
        }

        [Fact]
        public void Deserialization_ChildLists_Works()
        {
            var html = GetFileContents("Issue5.html");

#if IMMUTABLE
		    var tests = _htmlSerializer.Deserialize<TestIssue5>(html).Value;
#else
            var tests = _htmlSerializer.Deserialize<TestIssue5>(html);
#endif

            Assert.Equal(5, tests.Items.Count);


        }

        public class TestIssue5
        {
            [HtmlElement(".list>li:not(:has(ul))")]
            public List<string> Items { get; set; }

            [HtmlElement("li>ul")]
            public List<TestIssue5> ChildLists { get; set; }
        }
    }
}
