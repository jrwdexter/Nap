using Nap.Configuration.Sections.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Represents a header on a request.  Can be configured within a *.config file
    /// Example: Content-Type: application/json.
    /// </summary>
    /// <example>
    /// Configuration can be implemented as:
    /// <c>
    /// &lt;Nap&gt;
    ///   &lt;Headers&gt;
    ///     &lt;add key="Content-Type" value="application/json" /&gt;
    ///   &lt;/Headers&gt;
    /// &lt;Nap&gt;
    /// </c>
    /// or by configuration on <see cref="NapClient.Config"/>.
    /// </example>
    public class Header : NapConfigurationElementBase<IHeader, string>, IHeader
    {
        /// <summary>
        /// Gets or sets the key of the header.
        /// Example: "Content-Type" in "Content-Type: application/json"
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public override string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the header.
        /// Example: "application/json" in "Content-Type: application/json"
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public override string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

        /// <summary>
        /// Creates a copy of the <see cref="Header"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public Header Clone()
        {
            return new Header { Key = Key, Value = Value };
        }

        /// <summary>
        /// A method to hydrate the current instance of the <see cref="Header"/> with a non-configuration enabled version of data.
        /// </summary>
        /// <param name="original">The original <see cref="IHeader"/> implementation (or other implementation) with data used to hydrate this object.</param>
        public override void Hydrate(IHeader original)
        {
            Key = original.Key;
            Value = original.Value;
        }
    }
}