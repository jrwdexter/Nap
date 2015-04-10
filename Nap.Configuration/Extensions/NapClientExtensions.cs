using Nap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Configuration.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="NapClient"/> object to allow for configuration usage.
    /// </summary>
    public static class NapClientExtensions
    {
        /// <summary>
        /// Applies the profile with key <paramref name="configurationKey"/> to the nap client for any requests made with it.
        /// </summary>
        /// <param name="napClient">The nap client to apply a profile to.</param>
        /// <param name="configurationKey">The configuration key.</param>
        /// <returns></returns>
        public static NapClient ApplyProfile(this NapClient napClient, string configurationKey)
        {
            var config = napClient.Config as NapConfig;
            if (config == null)
                throw new NapConfigurationException("Cannot apply profiles without using Nap.Configuration - no profiles exist.  Did you call NapSetup.");

            var configurationProfile = config.Profiles.FirstOrDefault(p => p.Key.Equals(configurationKey, StringComparison.InvariantCultureIgnoreCase))?.Value;
            if (configurationProfile == null)
                throw new NapConfigurationException($"Could not find profile with name {configurationKey}.  It is possible a profile has already been applied.");

            napClient.Config = configurationProfile;
            return napClient;
        }
    }
}
