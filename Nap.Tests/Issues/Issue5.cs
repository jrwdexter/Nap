using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nap.Html;
using Nap.Html.Attributes;
using Nap.Tests.Serializers;
using Nap.Tests.Serializers.Base;

namespace Nap.Tests.Issues
{
    public class Issue5 : NapSerializerTestBase
    {
		private NapHtmlSerializer _htmlSerializer;

		[TestInitialize]
		public void Setup()
		{
			_htmlSerializer = new NapHtmlSerializer();
		}

		[TestMethod]
		[TestCategory("Issues")]
		[TestCategory("Nap.Html")]
	    public void Deserialization_ChildLists_Works()
	    {
	        var html = GetFileContents("Issue5.html");

#if IMMUTABLE
		    var tests = _htmlSerializer.Deserialize<TestIssue5>(html).Value;
#else
            var tests = _htmlSerializer.Deserialize<TestIssue5>(html);
#endif

            Assert.AreEqual(5, tests.Items.Count);


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
