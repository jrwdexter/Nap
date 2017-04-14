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
        public void Deserialization_HtmlParentTestClass_MatchesAppropriateValues()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html);
#endif

            // Assert
            Assert.Equal("Jeff", person.Spouse.FirstName);
            Assert.Equal("Doe", person.Spouse.LastName);
        }

        [Fact]
        public void Deserialization_HtmlParentTestClass_Matches_Children_List()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html);
#endif

            // Assert
            Assert.Equal("John", person.Children_List.First().FirstName);
            Assert.Equal("Doe", person.Children_List.First().LastName);
            Assert.Equal("Jane", person.Children_List.Last().FirstName);
            Assert.Equal("Doe", person.Children_List.Last().LastName);
        }

        [Fact]
        public void Deserialization_HtmlParentTestClass_Matches_Children_Array()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html);
#endif

            // Assert
            Assert.Equal("John", person.Children_Enumerable.First().FirstName);
            Assert.Equal("Doe", person.Children_Enumerable.First().LastName);
            Assert.Equal("Jane", person.Children_Enumerable.Last().FirstName);
            Assert.Equal("Doe", person.Children_Enumerable.Last().LastName);
        }

#if IMMUTABLE
        [Fact]
        public void Deserialization_HtmlParentTestClass_Matches_Children_Enumerable()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html).Value;

            // Assert
            Assert.Equal("John", person.Children_FSharpList.First().FirstName);
            Assert.Equal("Doe", person.Children_FSharpList.First().LastName);
            Assert.Equal("Jane", person.Children_FSharpList.Last().FirstName);
            Assert.Equal("Doe", person.Children_FSharpList.Last().LastName);
        }
#endif

        [Fact]
        public void Deserialization_HtmlParentTestClass_Matches_Children_FSharpList()
        {
            // Arrange
            var html = GetFileContents("ParentTestClass.html");

            // Act
#if IMMUTABLE
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html).Value;
#else
            var person = _htmlSerializer.Deserialize<HtmlParentTestClass>(html);
#endif

            // Assert
            Assert.Equal("John", person.Children_Array.First().FirstName);
            Assert.Equal("Doe", person.Children_Array.First().LastName);
            Assert.Equal("Jane", person.Children_Array.Last().FirstName);
            Assert.Equal("Doe", person.Children_Array.Last().LastName);
        }
    }
}
