using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Serializers;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Nap.Tests.Serializers
{
    [TestClass]
    public class NapJsonSerializerTests : NapSerializerTestBase
    {
        private NapJsonSerializer _jsonSerializer;

        [TestInitialize]
        public void Setup()
        {
            _jsonSerializer = new NapJsonSerializer();
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.AreEqual("application/json", _jsonSerializer.ContentType);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _jsonSerializer.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            _jsonSerializer.Deserialize<TestClass>(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ConstructorNotFoundException))]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
        {
            // Act
            _jsonSerializer.Deserialize<RequiresParameters_TestClass>("");
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialize_TestClass_MatchesSchema()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };
            var schema = JsonSchema.Parse(GetFileContents("TestClass.schema.json"));

            // Act
            var json = _jsonSerializer.Serialize(test);
            var jsonObject = JObject.Parse(json);

            // Assert
            Assert.IsTrue(jsonObject.IsValid(schema), "JSON serialization for [TestClass] does not match schema.");
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialize_ParentTestClass_MatchesSchema()
        {
            // Arrange
            var test = new ParentTestClass
            {
                Children = new List<TestClass>
                {
                    new TestClass { FirstName = "John", LastName = "Doe"},
                    new TestClass { FirstName = "Jane", LastName = "Doe"}
                },
                Spouse = new TestClass { FirstName = "Jeff", LastName = "Doe" }
            };
            var schema = JsonSchema.Parse(GetFileContents("ParentTestClass.schema.json"));

            // Act
            var json = _jsonSerializer.Serialize(test);
            var jsonObject = JObject.Parse(json);

            // Assert
            Assert.IsTrue(jsonObject.IsValid(schema), "JSON serialization for [ParentTestClass] does not match schema.");
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Deserialize_BasicJson_DoesNotThrowException()
        {
            // Arrange
            string json = GetFileContents("TestClass.json");

            // Act
            var result = _jsonSerializer.Deserialize<TestClass>(json);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialization_ThenDeserialization_OfTestClass_MatchesInput()
        {
            // Arrange
            var input = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var json = _jsonSerializer.Serialize(input);
            var output = _jsonSerializer.Deserialize<TestClass>(json);

            // Assert
            Assert.AreEqual(input.FirstName, output.FirstName);
            Assert.AreEqual(input.LastName, output.LastName);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialization_ThenDeserialization_OfParentTestClass_MatchesInput()
        {
            // Arrange
            var input = new ParentTestClass
            {
                Children = new List<TestClass>
                {
                    new TestClass { FirstName = "John", LastName = "Doe"},
                    new TestClass { FirstName = "Jane", LastName = "Doe"}
                },
                Spouse = new TestClass { FirstName = "Jeff", LastName = "Doe" }
            };

            // Act
            var json = _jsonSerializer.Serialize(input);
            var output = _jsonSerializer.Deserialize<ParentTestClass>(json);

            // Assert
            for (int i = 0; i < input.Children.Count(); i++)
            {
                Assert.AreEqual(input.Children.ToList()[i].FirstName, output.Children.ToList()[i].FirstName);
                Assert.AreEqual(input.Children.ToList()[i].LastName, output.Children.ToList()[i].LastName);
            }
            Assert.AreEqual(input.Spouse.FirstName, output.Spouse.FirstName);
            Assert.AreEqual(input.Spouse.LastName, output.Spouse.LastName);
        }
    }
}
