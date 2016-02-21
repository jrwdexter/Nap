using System;
using System.Collections.Generic;
using System.Diagnostics;

using Nap.Configuration;
using Xunit;

namespace Nap.Tests.Configuration
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Class", "NapSetup")]
    public class NapSetupTests
    {
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
