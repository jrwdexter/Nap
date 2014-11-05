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
            using (var ec = new EasyHttpClient())
            {
                ec.Config.Proxy = new Uri("http://localhost:8888");
                var task = ec.Post("http://api.github.com/")
                             .WithQueryParameter("q", "v")
                             .WithHeader("aHeader", "aValue")
                             .WithBody(new { FirstName = "John", LastName = "Doe" })
                             .ExecuteAsync()
                             .Result;
                Assert.IsNotNull(task);
            }
        }
    }
}
