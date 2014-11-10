using System;
using System.Configuration;

namespace Nap.Configuration
{
    public class AdvancedNapConfig : ConfigurationElement, IAdvancedNapConfig
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
        
        /// <summary>
        /// Creates a copy of the <see cref="NapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        internal AdvancedNapConfig Clone()
        {
            var advancedNapConfig = new AdvancedNapConfig
            {
                Proxy = Proxy
            };

            return advancedNapConfig;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
