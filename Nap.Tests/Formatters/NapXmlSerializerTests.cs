using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using Nap.Formatters;
using Nap.Tests.Formatters.Base;
using Nap.Tests.TestClasses;

namespace Nap.Tests.Formatters
{
    [TestClass]
    public class NapXmlSerializerTests : NapSerializerTestBase
    {
        private NapXmlSerializer _xmlFormatter;

        [TestInitialize]
        public void Setup()
        {
            _xmlFormatter = new NapXmlSerializer();
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
        public void Serialize_WhenNull_ThenExceptionIsThrown()
        {
            // Act
            _xmlFormatter.Serialize(null);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_TestClass_ReturnsValidXml()
        {
            // Arrange
            var test = new TestClass { FirstName = "John", LastName = "Doe" };

            // Act
            var xml = _xmlFormatter.Serialize(test);
            var xdoc = new XmlDocument();
            //xdoc.;

            // Assert
            //var doXmlDocument
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Serialize_ParentTestClass_ReturnsXml()
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
            const string validXml = "{\"Children\":[{\"FirstName\":\"John\",\"LastName\":\"Doe\"},{\"FirstName\":\"Jane\",\"LastName\":\"Doe\"}],\"Spouse\":{\"FirstName\":\"Jeff\",\"LastName\":\"Doe\"}}";

            // Act
            var json = _xmlFormatter.Serialize(test);
            json = Regex.Replace(json, @"\s+", ""); // Ignore white space

            // Assert
            Assert.AreEqual(validXml, json);
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
        public void Deserialize_ValidXml_CreatesValidTestClass()
        {
            // Arrange
            const string validXml = "{\"FirstName\":\"John\",\"LastName\":\"Doe\"}";

            // Act
            var deserialized = _xmlFormatter.Deserialize<TestClass>(validXml);

            // Assert
            Assert.AreEqual("John", deserialized.FirstName);
            Assert.AreEqual("Doe", deserialized.LastName);
        }

        [TestMethod]
        [TestCategory("Formatters")]
        public void Deserialize_ValidXml_CreatesValidParentTestClass()
        {
            // Arrange
            const string validXml = "{\"Children\":[{\"FirstName\":\"John\",\"LastName\":\"Doe\"},{\"FirstName\":\"Jane\",\"LastName\":\"Doe\"}],\"Spouse\":{\"FirstName\":\"Jeff\",\"LastName\":\"Doe\"}}";

            // Act
            var deserialized = _xmlFormatter.Deserialize<ParentTestClass>(validXml);

            // Assert
            var firstChild = deserialized.Children.First();
            var lastChild = deserialized.Children.Last();
            Assert.AreEqual("John", firstChild.FirstName);
            Assert.AreEqual("Doe", firstChild.LastName);
            Assert.AreEqual("Jane", lastChild.FirstName);
            Assert.AreEqual("Doe", lastChild.LastName);
            Assert.AreEqual("Jeff", deserialized.Spouse.FirstName);
            Assert.AreEqual("Doe", deserialized.Spouse.LastName);
        }
    }
}
