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
    /// or by configuration on <see cref="Nap.Config"/>.
    /// </example>
    public class QueryParameter : ConfigurationElement, IQueryParameter
    {
        /// <summary>
        /// Gets or sets the key of the header.
        /// Example: "name" in "/index?name=John"
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the header.
        /// Example: "John" in "/index?name=John"
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
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
    }
}