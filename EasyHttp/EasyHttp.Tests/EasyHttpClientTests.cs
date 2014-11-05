using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyHttp.Tests
{
    [TestClass]
    public class EasyHttpClientTests
    {
        [TestMethod]
        public async void Something()
        {
            var ec = new EasyHttpClient("http://api.github.com/");
            ec.Config.Proxy = new Uri("http://localhost:8888");
            var response = await ec.Get("/")
                                   .IncludeQueryParameter("q", "v")
                                   .IncludeHeader("aHeader", "aValue")
                                   .IncludeBody(new { FirstName = "John", LastName = "Doe" })
                                   .ExecuteAsync();
            Assert.IsNotNull(response);
        }
    }
}
