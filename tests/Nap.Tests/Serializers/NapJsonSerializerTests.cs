using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Exceptions;
using Nap.Serializers;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;

#if IMMUTABLE
using Microsoft.FSharp.Core;
#endif

namespace Nap.Tests.Serializers
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Serialization")]
    [Trait("Class", "NapJsonSerializer")]
    public class NapJsonSerializerTests : NapSerializerTestBase
    {
        private readonly NapJsonSerializer _jsonSerializer;

        public NapJsonSerializerTests()
        {
            _jsonSerializer = new NapJsonSerializer();
        }

        [Fact]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.Equal("application/json", _jsonSerializer.ContentType);
        }

#if IMMUTABLE
        [Fact]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ReturnsNone()
        {
            // Act
            var result = _jsonSerializer.Deserialize<RequiresParameters_TestClass>("");

            // Assert
            Assert.Equal(FSharpOption<RequiresParameters_TestClass>.None, result);
        }

        [Fact]
        public void Deserialize_Null_ReturnsNone()
        {
            // Act
            var result = _jsonSerializer.Deserialize<TestClass>(null);
            
            // Assert
            Assert.Equal(FSharpOption<TestClass>.None, result);
        }

        [Fact]
        public void Serialize_WhenNull_ThenEmptyStringIsReturned()
        {
            // Act
            var result = _jsonSerializer.Serialize(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }
#else
        [Fact]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
        {
            // Act
            Assert.Throws<ConstructorNotFoundException>(() => _jsonSerializer.Deserialize<RequiresParameters_TestClass>(""));
        }

        [Fact]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _jsonSerializer.Deserialize<TestClass>(null));
        }

        [Fact]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _jsonSerializer.Serialize(null));
        }
#endif

        [Fact]
        public void Serialize_TestClass_MatchesSchema()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };
            var schema = JsonSchema.Parse(GetFileContents("TestClass.schema.json"));

            // Act
            var json = _jsonSerializer.Serialize(test);
            var jsonObject = JObject.Parse(json);

            // Assert
            Assert.True(jsonObject.IsValid(schema), "JSON serialization for [TestClass] does not match schema.");
        }

        [Fact]
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
            Assert.True(jsonObject.IsValid(schema), "JSON serialization for [ParentTestClass] does not match schema.");
        }

        [Fact]
        public void Deserialize_BasicJson_DoesNotThrowException()
        {
            // Arrange
            string json = GetFileContents("TestClass.json");

            // Act
            var result = _jsonSerializer.Deserialize<TestClass>(json);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
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
            Assert.Equal(input.FirstName, output.FirstName);
            Assert.Equal(input.LastName, output.LastName);
        }

        [Fact]
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
            for (int i = 0; i < input.Children.Count; i++)
            {
                Assert.Equal(input.Children.ToList()[i].FirstName, output.Children.ToList()[i].FirstName);
                Assert.Equal(input.Children.ToList()[i].LastName, output.Children.ToList()[i].LastName);
            }
            Assert.Equal(input.Spouse.FirstName, output.Spouse.FirstName);
            Assert.Equal(input.Spouse.LastName, output.Spouse.LastName);
        }
    }
}
