using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Tests
{
    [TestClass]
    public class NapFluentTests
    {
        private const string _exampleUrl = "http://www.example.com";
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        [TestCategory("NapFluent")]
        public void NapFluent_BasicAuth_AddsAuthenticationHeader()
        {
            // Arrange
            var nap = new Nap();

            // Act
            var request = nap.Get(_exampleUrl).Advanced.Authentication.Basic("johndoe", "password");

            // Assert
            Assert.AreEqual(1, nap.Config.Headers.Count, "At least one header must be present");
            Assert.IsTrue(!string.IsNullOrEmpty(nap.Config.Headers.AsDictionary()["Authentication"]), "Authentication header must be present");
        }
    }
}
