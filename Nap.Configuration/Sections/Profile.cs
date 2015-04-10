using System;
using Nap.Configuration.Sections.Base;
using System.Configuration;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Describes a profile (a <see cref="INapConfig"/> associated with a string key) that can be applied using 
    /// </summary>
    public class Profile : NapConfigurationElementBase<IProfile, ChildNapConfig>, IProfile
    {
        /// <summary>
        /// Gets or sets the unique key that identifies this configuration element among its siblings.
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public override string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the value that is contained within the configuration element.
        /// </summary>
        [ConfigurationProperty("profile", IsDefaultCollection = false, IsRequired = true)]
        public override ChildNapConfig Value
        {
            get { return (ChildNapConfig)this["profile"]; }
            set { this["profile"] = value; }
        }

        /// <summary>
        /// A method to hydrate the current instance of the configuration element with a non-configuration enabled version of data.
        /// </summary>
        /// <param name="original">The original <see cref="IProfile"/> implementation (or other implementation) with data used to hydrate this object.</param>
        public override void Hydrate(IProfile original)
        {
            Key = original.Key;
            Value = original.Value;
        }
    }
}