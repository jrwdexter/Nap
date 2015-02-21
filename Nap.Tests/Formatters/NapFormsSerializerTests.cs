using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Formatters;
using Nap.Tests.Formatters.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Formatters
{
    [TestClass]
    public class NapFormsSerializerTests : NapSerializerTestBase
    {
        private NapFormsFormatter _formsFormatter;

        [TestInitialize]
        public void Setup()
        {
            _formsFormatter = new NapFormsFormatter();
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.AreEqual("application/x-www-form-urlencoded", _formsFormatter.ContentType);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _formsFormatter.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(NotSupportedException))]
        public void Deserialize_Null_ThrowsNotSupprtedException()
        {
            // Act
            _formsFormatter.Deserialize<TestClass>("anything");
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_TestClass_IsCorrect()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var serialized = _formsFormatter.Serialize(test);

            // Assert
            Assert.AreEqual("FirstName=John&LastName=Doe", serialized);
        }
    }
}
