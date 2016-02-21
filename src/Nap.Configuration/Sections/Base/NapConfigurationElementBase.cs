using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Configuration.Sections.Base
{
    /// <summary>
    /// The base class for configuration elements within the Nap configuration framework.
    /// </summary>
    /// <typeparam name="TI">The base interface type that the configuration element is for.</typeparam>
    /// <typeparam name="TV">The value type for the configuration element.</typeparam>
    public abstract class NapConfigurationElementBase<TI, TV> : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationElementBase{TI, TV}"/> class.
        /// </summary>
        public NapConfigurationElementBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationElementBase{TI, TV}"/> class.
        /// </summary>
        /// <param name="key">The key to initialize the configuration element with.</param>
        /// <param name="value">The value of the configuration element.</param>
        public NapConfigurationElementBase(string key, TV value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the unique key that identifies this configuration element among its siblings.
        /// </summary>
        public abstract string Key { get; set; }

        /// <summary>
        /// Gets or sets the value that is contained within the configuration element.
        /// </summary>
        public abstract TV Value { get; set; }

        /// <summary>
        /// A method to hydrate the current instance of the configuration element with a non-configuration enabled version of data.
        /// </summary>
        /// <param name="original">The original interface implementation (or other implementation) with data used to hydrate this object.</param>
        public abstract void Hydrate(TI original);
    }
}
