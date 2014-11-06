using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Napper.Configuration
{
    public class AdvancedNapConfig : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        [ConfigurationProperty("Proxy")]
        public Uri Proxy
        {
            get { return (Uri)this["Proxy"]; }
            set { this["Proxy"] = value; }
        }

        internal AdvancedNapConfig Clone()
        {
            var advancedNapConfig = new AdvancedNapConfig
            {
                Proxy = Proxy
            };

            return advancedNapConfig;
        }
    }
}
