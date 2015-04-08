using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Configuration.Sections;
using Nap.Exceptions;
using Nap.Html;

namespace Nap.Html.Tests.Configuration
{
    [TestClass]
    public class NapConfigurationTests
    {
        // Tests to ensure that *.config files get parsed correctly.

        [TestInitialize]
        public void Setup()
        {
            NapSetup.AddConfig(NapConfig.GetCurrent);
        }

        [TestMethod]
        [TestCategory("Configuration")]
		[TestCategory("Nap.Html")]
		public void GetConfiguration_FromConfigFile_Formatters_Match()
		{
			var formatters = NapClient.Lets.Config.Formatters.AsDictionary();

			// Assert
			Assert.IsNotNull(formatters);
			Assert.AreEqual(4, formatters.Count, "App.Config should populate one formatter, and 3 should be added by default.");
			Assert.AreEqual("text/html", formatters.Last().Key);
			Assert.IsInstanceOfType(formatters.Last().Value, typeof(NapHtmlFormatter));
		}
    }
}
