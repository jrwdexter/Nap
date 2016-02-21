using System;

using Nap.Serializers;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;
using Xunit;

namespace Nap.Tests.Serializers
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Serialization")]
    [Trait("Class", "NapFormsSerializer")]
    public class NapFormsSerializerTests : NapSerializerTestBase
    {
        private readonly NapFormsSerializer _formsSerializer;

        public NapFormsSerializerTests()
        {
            _formsSerializer = new NapFormsSerializer();
        }

        [Fact]
        public void GetContentType_EqualsApplicationJson()
        {
            // Assert
            Assert.Equal("application/x-www-form-urlencoded", _formsSerializer.ContentType);
        }

#if IMMUTABLE
        [Fact]
        public void Serialize_WhenNull_ThenNoExceptionIsThrown()
        {
            // Act
            var result = _formsSerializer.Serialize(null);
            
            // Assert
            Assert.Equal(string.Empty, result);
        }
#else
        [Fact]
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _formsSerializer.Serialize(null));
        }
#endif

        [Fact]
        public void Deserialize_Null_ThrowsNotSupprtedException()
        {
            // Act
            Assert.Throws<NotSupportedException>(() => _formsSerializer.Deserialize<TestClass>("anything"));
        }

        [Fact]
        public void Serialize_TestClass_IsCorrect()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var serialized = _formsSerializer.Serialize(test);

            // Assert
            Assert.Equal("FirstName=John&LastName=Doe", serialized);
        }
    }
}
