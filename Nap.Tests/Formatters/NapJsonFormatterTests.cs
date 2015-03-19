using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Formatters;
using Nap.Tests.Formatters.Base;
using Nap.Tests.TestClasses;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Nap.Tests.Formatters
{
    [TestClass]
    public class NapJsonFormatterTests : NapFormatterTestBase
    {
        private NapJsonFormatter _jsonFormatter;

        [TestInitialize]
        public void Setup()
        {
            _jsonFormatter = new NapJsonFormatter();
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.AreEqual("application/json", _jsonFormatter.ContentType);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _jsonFormatter.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            _jsonFormatter.Deserialize<TestClass>(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ConstructorNotFoundException))]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
        {
            // Act
            _jsonFormatter.Deserialize<RequiresParameters_TestClass>("");
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_TestClass_MatchesSchema()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };
            var schema = JsonSchema.Parse(GetFileContents("TestClass.schema.json"));

            // Act
            var json = _jsonFormatter.Serialize(test);
            var jsonObject = JObject.Parse(json);

            // Assert
            Assert.IsTrue(jsonObject.IsValid(schema), "JSON serialization for [TestClass] does not match schema.");
        }

        [TestMethod]
        [TestCategory("Formatters")]
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
            var json = _jsonFormatter.Serialize(test);
            var jsonObject = JObject.Parse(json);

            // Assert
            Assert.IsTrue(jsonObject.IsValid(schema), "JSON serialization for [ParentTestClass] does not match schema.");
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Deserialize_BasicJson_DoesNotThrowException()
        {
            // Arrange
            string json = GetFileContents("TestClass.json");

            // Act
            var result = _jsonFormatter.Deserialize<TestClass>(json);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialization_ThenDeserialization_OfTestClass_MatchesInput()
        {
            // Arrange
            var input = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var json = _jsonFormatter.Serialize(input);
            var output = _jsonFormatter.Deserialize<TestClass>(json);

            // Assert
            Assert.AreEqual(input.FirstName, output.FirstName);
            Assert.AreEqual(input.LastName, output.LastName);
        }

        [TestMethod]
        [TestCategory("Formatters")]
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
            var json = _jsonFormatter.Serialize(input);
            var output = _jsonFormatter.Deserialize<ParentTestClass>(json);

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
