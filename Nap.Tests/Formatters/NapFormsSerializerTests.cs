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
        private NapFormsSerializer _formsSerializer;

        [TestInitialize]
        public void Setup()
        {
            _formsSerializer = new NapFormsSerializer();
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.AreEqual("application/x-www-form-urlencoded", _formsSerializer.ContentType);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _formsSerializer.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(NotSupportedException))]
        public void Deserialize_Null_ThrowsNotSupprtedException()
        {
            // Act
            _formsSerializer.Deserialize<TestClass>("anything");
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_TestClass_IsCorrect()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var serialized = _formsSerializer.Serialize(test);

            // Assert
            Assert.AreEqual("FirstName=John&LastName=Doe", serialized);
        }
    }
}
