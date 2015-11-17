using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Serializers;
using Nap.Tests.Serializers.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Serializers
{
    [TestClass]
    public class NapXmlSerializerTests : NapSerializerTestBase
    {
        private NapXmlSerializer _xmlSerializer;

        [TestInitialize]
        public void Setup()
        {
            _xmlSerializer = new NapXmlSerializer();
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void GetContentType_EqualsApplicationXml()
        {
            // Assert
            Assert.AreEqual("application/xml", _xmlSerializer.ContentType);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serialize_Null_ThrowsException()
        {
            // Act
            _xmlSerializer.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Deserialize_Null_ThrowsException()
        {
            // Act
            _xmlSerializer.Deserialize<TestClass>(null);
        }

        [TestMethod]
        [TestCategory("Serializers")]
        [ExpectedException(typeof(ConstructorNotFoundException))]
        public void Deserialize_IntoClassWithoutParameterlessConstructor_ThrowsException()
        {
            // Act
            _xmlSerializer.Deserialize<RequiresParameters_TestClass>("");
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Serialize_TestClass_ReturnsValidXml()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };
            var validatorMessages = new XmlValidatorChecks();
            var schema = GetFileContents("TestClass.xsd");
            var xdoc = new XmlDocument();
            xdoc.Schemas.Add(string.Empty, XmlReader.Create(new StringReader(schema)));

            // Act
            var xml = _xmlSerializer.Serialize(test);
            xdoc.LoadXml(xml);
            xdoc.Validate(validatorMessages.HandleValidationError);

            // Assert
            Assert.IsTrue(validatorMessages.IsValid, string.Join("\r\n", validatorMessages.Errors));
        }

        [TestMethod]
        [TestCategory("Serializers")]
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
            var xml = _xmlSerializer.Serialize(test);
            xdoc.LoadXml(xml);
            xdoc.Validate(validatorMessages.HandleValidationError);

            // Assert
            Assert.IsTrue(validatorMessages.IsValid, string.Join("\r\n", validatorMessages.Errors));
        }

        [TestMethod]
        [TestCategory("Serializers")]
        public void Deserialize_BasicJson_DoesNotThrowException()
        {
            // Arrange
            string json = GetFileContents("TestClass.xml");

            // Act
            var result = _xmlSerializer.Deserialize<TestClass>(json);

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
            var json = _xmlSerializer.Serialize(input);
            var output = _xmlSerializer.Deserialize<TestClass>(json);

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
            var json = _xmlSerializer.Serialize(input);
            var output = _xmlSerializer.Deserialize<ParentTestClass>(json);

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
