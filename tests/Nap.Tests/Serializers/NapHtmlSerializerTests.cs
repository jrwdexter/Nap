using System;
using System.Linq;
using Nap.Html;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;
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
    [Trait("Class", "NapHtmlSerializer")]
    public class NapHtmlSerializerTests : NapSerializerTestBase
    {
        private readonly NapHtmlSerializer _htmlSerializer;

        public NapHtmlSerializerTests()
        {
            _htmlSerializer = new NapHtmlSerializer();
        }

        [Fact]
        public void GetContentType_EqualsTextHtml()
        {
            // Assert
            Assert.Equal("text/html", _htmlSerializer.ContentType);
        }

        [Fact]
        public void Serialize_NotSupportedExceptionIsThrown()
        {
            // Act
            Assert.Throws<NotSupportedException>(() => _htmlSerializer.Serialize("<!DOCTYPE html><html lang=\"en\"><head></head><body></body></html>"));
        }

#if IMMUTABLE
        [Fact]
		public void Deserialize_Null_IsNone()
		{
			// Act
			var result = _htmlSerializer.Deserialize<TestClass>(null);

            // Assert
            Assert.Equal(FSharpOption<TestClass>.None, result);
		}

		[Fact]
		public void Deserialize_IntoClassWithoutParameterlessConstructor_DeserializesCorrectly()
		{
            // Arrange
		    string html = GetFileContents("TestClass.html");

			// Act
			var result = _htmlSerializer.Deserialize<RequiresParameters_TestClass>(html);

            // Assert
			Assert.True(result.Value != null);
            Assert.Equal("John", result.Value.FirstName);
            Assert.Equal("Doe", result.Value.LastName);
		}
#else
        [Fact]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _htmlSerializer.Deserialize<TestClass>(null));
        }

        [Fact]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_DeserializesCorrectly()
        {
            // Arrange
            string html = GetFileContents("TestClass.html");

            // Act
            var result = _htmlSerializer.Deserialize<RequiresParameters_TestClass>(html);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }
#endif

        [Fact]
        public void Deserialize_BasicHtml_DoesNotThrowException()
        {
            // Arrange
            string html = GetFileContents("TestClass.html");

            // Act
            var result = _htmlSerializer.Deserialize<TestClass>(html);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Deserialization_TestClass_MatchesAppropriateValues()
        {
            // Arrange
            var html = GetFileContents("TestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<TestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<TestClass>(html);
#endif

            // Assert
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Doe", person.LastName);
        }

        [Fact]
        public void Deserialization_ParentTestClass_MatchesAppropriateValues()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<ParentTestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<ParentTestClass>(html);
#endif

            // Assert
            Assert.Equal("Jeff", person.Spouse.FirstName);
            Assert.Equal("Doe", person.Spouse.LastName);
            Assert.Equal("John", person.Children.First().FirstName);
            Assert.Equal("Doe", person.Children.First().LastName);
            Assert.Equal("Jane", person.Children.Last().FirstName);
            Assert.Equal("Doe", person.Children.Last().LastName);
        }
    }
}
