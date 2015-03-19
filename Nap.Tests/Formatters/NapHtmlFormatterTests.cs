using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Formatters;
using Nap.Html;
using Nap.Tests.Formatters.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Formatters
{
	[TestClass]
	public class NapHtmlFormatterTests : NapFormatterTestBase
	{
		private NapHtmlFormatter _htmlFormatter;

		[TestInitialize]
		public void Setup()
		{
			_htmlFormatter = new NapHtmlFormatter();
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		public void GetContentType_EqualsTextHtml()
		{
			// Assert
			Assert.AreEqual("text/html", _htmlFormatter.ContentType);
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		[ExpectedException(typeof(NotSupportedException))]
		public void Serialize_NotSupportedExceptionIsThrown()
		{
			// Act
			_htmlFormatter.Serialize("<!DOCTYPE html><html lang=\"en\"><head></head><body></body></html>");
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Deserialize_Null_ThrowsException()
		{
			// Act
			_htmlFormatter.Deserialize<TestClass>(null);
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		[ExpectedException(typeof(ConstructorNotFoundException))]
		public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
		{
			// Act
			_htmlFormatter.Deserialize<RequiresParameters_TestClass>("");
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		public void Deserialize_BasicHtml_DoesNotThrowException()
		{
			// Arrange
			string html = GetFileContents("TestClass.html");

			// Act
			var result = _htmlFormatter.Deserialize<TestClass>(html);

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		public void Deserialization_TestClass_MatchesAppropriateValues()
		{
			// Arrange
			var html = GetFileContents("TestClass.html");

			// Act
			var person = _htmlFormatter.Deserialize<TestClass>(html);

			// Assert
			Assert.AreEqual("John", person.FirstName);
			Assert.AreEqual("Doe", person.LastName);
		}

		[TestMethod]
		[TestCategory("Formatters")]
		[TestCategory("Nap.Html")]
		public void Deserialization_ParentTestClass_MatchesAppropriateValues()
		{
			// Arrange
			var html = GetFileContents("ParentTestClass.html");

			// Act
			var person = _htmlFormatter.Deserialize<ParentTestClass>(html);

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
