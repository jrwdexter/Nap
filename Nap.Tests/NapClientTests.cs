using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Napper.Tests
{
    [TestClass]
    public class NapClientTests
    {
        [TestMethod]
        public async void Something()
        {
            var response = await Nap.Lets.Get("/")
                                         .IncludeQueryParameter("q", "v")
                                         .IncludeHeader("aHeader", "aValue")
                                         .IncludeBody(new { FirstName = "John", LastName = "Doe" })
                                         .ExecuteAsync();
            Assert.IsNotNull(response);
        }
    }
}
