using Nap.Configuration.Sections.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Represents a header on a request.  Can be configured within a *.config file
    /// Example: /index?name=John
    /// </summary>
    /// <example>
    /// Configuration can be implemented as:
    /// <c>
    /// &lt;Nap&gt;
    ///   &lt;QueryParameters&gt;
    ///     &lt;add key="name" value="John" /&gt;
    ///   &lt;/QueryParameters&gt;
    /// &lt;Nap&gt;
    /// </c>
    /// or by configuration on <see cref="NapClient.Config"/>.
    /// </example>
    public class QueryParameter : NapConfigurationElementBase<IQueryParameter, string>, IQueryParameter
    {
        /// <summary>
        /// Gets or sets the key of the header.
        /// Example: "name" in "/index?name=John"
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public override string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the header.
        /// Example: "John" in "/index?name=John"
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public override string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

        /// <summary>
        /// Creates a copy of the <see cref="QueryParameter"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public QueryParameter Clone()
        {
            return new QueryParameter { Key = Key, Value = Value };
        }

        /// <summary>
        /// A method to hydrate the current instance of the <see cref="QueryParameter"/> with a non-configuration enabled version of data.
        /// </summary>
        /// <param name="original">The original <see cref="IQueryParameter"/> implementation (or other implementation) with data used to hydrate this object.</param>
        public override void Hydrate(IQueryParameter original)
        {
            Key = original.Key;
            Value = original.Value;
        }
    }
}