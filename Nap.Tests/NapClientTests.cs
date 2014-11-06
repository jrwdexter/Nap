using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Napper.Tests
{
    [TestClass]
    public class NapClientTests
    {
        [TestMethod]
        public async Task Something()
        {
            var response = await Nap.Lets.Get("http://example.com/")
                                         .IncludeQueryParameter("q", "v")
                                         .IncludeHeader("aHeader", "aValue")
                                         .IncludeBody(new { FirstName = "John", LastName = "Doe" })
                                         .ExecuteAsync();
            Assert.IsNotNull(response);
        }
    }
}
