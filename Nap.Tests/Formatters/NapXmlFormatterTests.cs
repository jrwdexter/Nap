using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Formatters;
using Nap.Tests.Formatters.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Formatters
{
    [TestClass]
    public class NapXmlFormatterTests : NapFormatterTestBase
    {
        private NapXmlFormatter _xmlFormatter;

        [TestInitialize]
        public void Setup()
        {
            _xmlFormatter = new NapXmlFormatter();
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void GetContentType_EqualsApplicationXml()
        {
            // Assert
            Assert.AreEqual("application/xml", _xmlFormatter.ContentType);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_Null_ThrowsException()
        {
            // Act
            _xmlFormatter.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            _xmlFormatter.Deserialize<TestClass>(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        [ExpectedException(typeof(ConstructorNotFoundException))]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
        {
            // Act
            _xmlFormatter.Deserialize<RequiresParameters_TestClass>("");
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_TestClass_ReturnsValidXml()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };
            var validatorMessages = new XmlValidatorChecks();
            var schema = GetFileContents("TestClass.xsd");
            var xdoc = new XmlDocument();
            xdoc.Schemas.Add(string.Empty, XmlReader.Create(new StringReader(schema)));

            // Act
            var xml = _xmlFormatter.Serialize(test);
            xdoc.LoadXml(xml);
            xdoc.Validate(validatorMessages.HandleValidationError);

            // Assert
            Assert.IsTrue(validatorMessages.IsValid, string.Join("\r\n", validatorMessages.Errors));
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_ParentTestClass_ReturnsValidXml()
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
            var validatorMessages = new XmlValidatorChecks();
            var schema = GetFileContents("ParentTestClass.xsd");
            var xdoc = new XmlDocument();
            xdoc.Schemas.Add(string.Empty, XmlReader.Create(new StringReader(schema)));

            // Act
            var xml = _xmlFormatter.Serialize(test);
            xdoc.LoadXml(xml);
            xdoc.Validate(validatorMessages.HandleValidationError);

            // Assert
            Assert.IsTrue(validatorMessages.IsValid, string.Join("\r\n", validatorMessages.Errors));
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Deserialize_BasicJson_DoesNotThrowException()
        {
            // Arrange
            string json = GetFileContents("TestClass.xml");

            // Act
            var result = _xmlFormatter.Deserialize<TestClass>(json);

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
            var json = _xmlFormatter.Serialize(input);
            var output = _xmlFormatter.Deserialize<TestClass>(json);

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
            var json = _xmlFormatter.Serialize(input);
            var output = _xmlFormatter.Deserialize<ParentTestClass>(json);

            // Assert
            for (int i = 0; i < input.Children.Count(); i++)
            {
                Assert.AreEqual(input.Children.ToList()[i].FirstName, output.Children.ToList()[i].FirstName);
                Assert.AreEqual(input.Children.ToList()[i].LastName, output.Children.ToList()[i].LastName);
            }
            Assert.AreEqual(input.Spouse.FirstName, output.Spouse.FirstName);
            Assert.AreEqual(input.Spouse.LastName, output.Spouse.LastName);
        }

        private sealed class XmlValidatorChecks
        {
            internal bool IsValid { get; private set; } = true;

            internal IList<string> Errors { get; } = new List<string>();

            internal void HandleValidationError(object sender, ValidationEventArgs e)
            {
                switch (e.Severity)
                {
                    case XmlSeverityType.Error:
                    case XmlSeverityType.Warning:
                        IsValid = false;
                        Errors.Add(e.Message);
                        break;
                }
            }
        }
    }
}
