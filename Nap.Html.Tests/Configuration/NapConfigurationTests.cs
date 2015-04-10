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
    }
}
