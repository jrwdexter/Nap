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
        /// Overwrites the <see cref="NapClient"/>, so this should be first in plugin order.
        /// </summary>
        /// <param name="client">Disposed, ignored client.</param>
        /// <returns>A new client that has been loaded using the *.config file.</returns>
        public override NapClient Setup(NapClient client)
        {
            return new NapClient(NapConfig.GetCurrent());
        }
    }
}
