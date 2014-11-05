using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyHttp.Tests
{
    [TestClass]
    public class EasyHttpClientTests
    {
        [TestMethod]
        public void Something()
        {
            var ec = new EasyHttpClient();
            ec.Config.Proxy = new Uri("http://localhost:8888");
            var task = ec.Post("http://api.github.com/")
                         .IncludeQueryParameter("q", "v")
                         .IncludeHeader("aHeader", "aValue")
                         .IncludeBody(new { FirstName = "John", LastName = "Doe" })
                         .ExecuteAsync()
                         .Result;
            Assert.IsNotNull(task);
        }
    }
}
