using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;

namespace Nap.Tests.Configuration
{
    [TestClass]
    public class NapSetupTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        private sealed class TestNapConfig : INapConfig
        {
	        public ISerializersConfig Serializers { get; set; }

	        public string BaseUrl { get; set; }

            public IHeaders Headers { get; set; }

            public IQueryParameters QueryParameters { get; set; }

	        string INapConfig.Serialization { get; set; }

            public IAdvancedNapConfig Advanced { get; set; }

            public bool FillMetadata { get; set; }

            public INapConfig Clone()
            {
                throw new NotImplementedException();
            }
        }
    }
}
