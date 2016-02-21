using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using Nap.Plugins.Base;

namespace Nap.Configuration
{
    /// <summary>
    /// The setup class for nap, which provides methods for obtaining a proper configuration class (depending on deployment target - portable or non-portable).
    /// </summary>
    public class NapSetup
    {
        private readonly List<IPlugin> _plugins = new List<IPlugin>();

        /// <summary>
        /// Gets the read-only collection of plugins that have been registered.
        /// </summary>
        internal IReadOnlyCollection<IPlugin> Plugins => _plugins;

        /// <summary>
        /// Register a plugin for use with each and every <see cref="NapRequest"/>.
        /// </summary>
        /// <param name="plugin">The plugin to register for nap requests.</param>
        public void InstallPlugin(NapPlugin plugin) => _plugins.Add(plugin);

        /// <summary>
        /// Register a plugin for use with each and every <see cref="NapRequest"/>.
        /// </summary>
        /// <typeparam name="T">The plugin type to register for nap requests.  Must have a parameterless constructor.</typeparam>
        public void InstallPlugin<T>() where T : NapPlugin, new() => InstallPlugin(Activator.CreateInstance<T>());

        /// <summary>
        /// Remove a previously registered plugin from the list of plugins being used.
        /// </summary>
        /// <param name="plugin">The instance of the plugin to remove.</param>
        /// <returns>True if the plugin has been successfully unregistered; otherwise false.</returns>
        public bool UninstallPlugin(NapPlugin plugin) => _plugins.Remove(plugin);

        /// <summary>
        /// Remove a previously registered plugin from the list of plugins being used.
        /// </summary>
        /// <typeparam name="T">The type of the plugin to remove.</typeparam>
        /// <returns>True if the plugin has been successfully unregistered; otherwise false.</returns>
        public bool UninstallPlugin<T>()
        {
            var plugin = _plugins.FirstOrDefault(p => p.GetType() == typeof(T));
            return plugin != null && _plugins.Remove(plugin);
        }
    }
}