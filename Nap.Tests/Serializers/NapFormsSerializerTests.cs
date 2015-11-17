using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nap.Serializers;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Serializers
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
        [TestCategory("Serializers")]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.AreEqual("application/x-www-form-urlencoded", _formsSerializer.ContentType);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _formsSerializer.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(NotSupportedException))]
        public void Deserialize_Null_ThrowsNotSupprtedException()
        {
            // Act
            _formsSerializer.Deserialize<TestClass>("anything");
        }

        [TestMethod]
        [TestCategory("Serializers")]
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
