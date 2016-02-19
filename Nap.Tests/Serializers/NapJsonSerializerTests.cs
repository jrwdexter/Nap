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

#if IMMUTABLE
using Microsoft.FSharp.Core;
#endif

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

#if IMMUTABLE
        [TestMethod]
        [TestCategory("Serializers")]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ReturnsNone()
        {
            // Act
            var result = _jsonSerializer.Deserialize<RequiresParameters_TestClass>("");

            // Assert
            Assert.AreEqual(FSharpOption<RequiresParameters_TestClass>.None, result);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Deserialize_Null_ReturnsNone()
        {
            // Act
            var result = _jsonSerializer.Deserialize<TestClass>(null);
            
            // Assert
            Assert.AreEqual(FSharpOption<RequiresParameters_TestClass>.None, result);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialize_WhenNull_ThenEmptyStringIsReturned()
        {
            // Act
            var result = _jsonSerializer.Serialize(null);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }
#else
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            _jsonSerializer.Deserialize<TestClass>(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _jsonSerializer.Serialize(null);
        }
#endif

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
#if IMMUTABLE
            var output = _jsonSerializer.Deserialize<TestClass>(json).Value;
#else
            var output = _jsonSerializer.Deserialize<TestClass>(json);
#endif

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
#if IMMUTABLE
            var output = _jsonSerializer.Deserialize<ParentTestClass>(json).Value;
#else
            var output = _jsonSerializer.Deserialize<ParentTestClass>(json);
#endif

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
