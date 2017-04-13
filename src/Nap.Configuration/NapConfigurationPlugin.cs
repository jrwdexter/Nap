using Nap.Plugins.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Configuration
{
    /// <summary>
    /// A plugin to make generation of <see cref="NapClient"/>s with *.config values simple.
    /// </summary>
    public class NapConfigurationPlugin : NapPluginBase
    {
        /// <summary>
        /// Setup a <see cref="NapClient"/> for initial use using a *.config file.
        /// Overwrites the <paramref name="config"/>, so this should be first in plugin order.
        /// </summary>
        /// <param name="config">Disposed, ignored configuration.</param>
        /// <returns>A new configuration that has been loaded using the *.config file.</returns>
        public override INapConfig Configure(INapConfig config)
        {
            return NapConfig.GetCurrent();
        }
    }
}
