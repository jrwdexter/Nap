using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Html;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;

#if IMMUTABLE
using Microsoft.FSharp.Core;
#endif

namespace Nap.Tests.Serializers
{
	[TestClass]
	public class NapHtmlSerializerTests : NapSerializerTestBase
	{
		private NapHtmlSerializer _htmlSerializer;

		[TestInitialize]
		public void Setup()
		{
			_htmlSerializer = new NapHtmlSerializer();
		}

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
		public void GetContentType_EqualsTextHtml()
		{
			// Assert
			Assert.AreEqual("text/html", _htmlSerializer.ContentType);
		}

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
		[ExpectedException(typeof(NotSupportedException))]
		public void Serialize_NotSupportedExceptionIsThrown()
		{
			// Act
			_htmlSerializer.Serialize("<!DOCTYPE html><html lang=\"en\"><head></head><body></body></html>");
		}

#if IMMUTABLE
        [TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
		public void Deserialize_Null_IsNone()
		{
			// Act
			var result = _htmlSerializer.Deserialize<TestClass>(null);

            // Assert
            Assert.AreEqual(FSharpOption<TestClass>.None, result);
		}
#else
        [TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Deserialize_Null_ThrowsException()
		{
			// Act
			_htmlSerializer.Deserialize<TestClass>(null);
		}
#endif

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
        [ExpectedException(typeof(ConstructorNotFoundException))]
		public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
		{
			// Act
			_htmlSerializer.Deserialize<RequiresParameters_TestClass>("");
		}

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
		public void Deserialize_BasicHtml_DoesNotThrowException()
		{
			// Arrange
			string html = GetFileContents("TestClass.html");

			// Act
			var result = _htmlSerializer.Deserialize<TestClass>(html);

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
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
			Assert.AreEqual("John", person.FirstName);
			Assert.AreEqual("Doe", person.LastName);
		}

		[TestMethod]
		[TestCategory("Serializers")]
		[TestCategory("Nap.Html")]
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
			Assert.AreEqual("Jeff", person.Spouse.FirstName);
			Assert.AreEqual("Doe", person.Spouse.LastName);
			Assert.AreEqual("John", person.Children.First().FirstName);
			Assert.AreEqual("Doe", person.Children.First().LastName);
			Assert.AreEqual("Jane", person.Children.Last().FirstName);
			Assert.AreEqual("Doe", person.Children.Last().LastName);
		}
	}
}
