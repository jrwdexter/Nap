using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Formatters.Base;

namespace Nap.Tests.Configuration
{
    [TestClass]
    public class NapSetupTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void ResetConfiguration_ResetsToEmptyNapConfig()
        {
            // Arrange
            NapSetup.AddConfig(() => new EmptyNapConfig());
            NapSetup.ResetConfig();

            // Act
            var config = NapSetup.Default;

            // Assert
            Assert.IsInstanceOfType(config, typeof(EmptyNapConfig));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetDefaultConfiguration_ReturnsEmptyConfiguration()
        {
            // Arrange
            NapSetup.ResetConfig();

            // Act
            var configuration = NapSetup.Default;

            // Assert
            Assert.IsNotNull(configuration);
            Assert.IsInstanceOfType(configuration, typeof(EmptyNapConfig));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void AddConfiguration_DoesNotThrowException()
        {
            // Act
            NapSetup.AddConfig<TestNapConfig>();
            NapSetup.AddConfig(() => new TestNapConfig());
            NapSetup.AddConfig(() => new TestNapConfig());
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void AddConfiguration_ViaGeneric_ReturnsProperConfiguration()
        {
            // Arrange
            NapSetup.AddConfig<TestNapConfig>();

            // Act
            var napConfig = NapSetup.Default;

            // Assert
            Assert.IsInstanceOfType(napConfig, typeof(TestNapConfig));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void AddConfiguration_ViaTypeParam_ReturnsProperConfiguration()
        {
            // Arrange
            NapSetup.AddConfig(typeof(TestNapConfig));

            // Act
            var napConfig = NapSetup.Default;

            // Assert
            Assert.IsInstanceOfType(napConfig, typeof(TestNapConfig));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void AddConfiguration_ViaLambda_ReturnsProperConfiguration()
        {
            // Arrange
            NapSetup.AddConfig(() => new TestNapConfig());

            // Act
            var napConfig = NapSetup.Default;

            // Assert
            Assert.IsInstanceOfType(napConfig, typeof(TestNapConfig));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        [TestCategory("Performance")]
        public void Instantiation_TimedTests()
        {
            var iterations = 1000000;

            var sw = Stopwatch.StartNew();
            INapConfig test;
            NapSetup.AddConfig<TestNapConfig>();
            for (int i = 0; i < iterations; i++)
            {
                test = NapSetup.Default;
            }
            Console.WriteLine("Generic instantiation: \{sw.ElapsedMilliseconds}");

            sw.Restart();
            NapSetup.AddConfig(typeof(TestNapConfig));
            for (int i = 0; i < iterations; i++)
            {
                test = NapSetup.Default;
            }
            Console.WriteLine("Typed instantiation: \{sw.ElapsedMilliseconds}");

            sw.Restart();
            NapSetup.AddConfig(() => new TestNapConfig());
            for (int i = 0; i < iterations; i++)
            {
                test = NapSetup.Default;
            }
            Console.WriteLine("Lambda instantiation: \{sw.ElapsedMilliseconds}");
        }

        private class TestNapConfig : INapConfig
        {
            public Dictionary<RequestFormat, INapSerializer> Serializers { get; set; }

            public string BaseUrl { get; set; }

            public RequestFormat Serialization { get; set; }

            public IAdvancedNapConfig Advanced { get; set; }

            public bool FillMetadata { get; set; }

            public INapConfig Clone()
            {
                throw new NotImplementedException();
            }
        }
    }
}
