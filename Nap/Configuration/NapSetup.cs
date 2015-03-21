using System;
using System.Linq;
using System.Collections.Generic;

using Nap.Plugins.Base;

namespace Nap.Configuration
{
    /// <summary>
    /// The setup class for nap, which provides methods for obtaining a proper configuration class (depending on deployment target - portable or non-portable).
    /// </summary>
    public static class NapSetup
    {
        private static Func<INapConfig> _napConfigCreator;
		private static List<IPlugin> _plugins = new List<IPlugin>();

		/// <summary>
		/// Gets the read-only collection of plugins that have been registered.
		/// </summary>
		static internal IReadOnlyCollection<IPlugin> Plugins => _plugins;

        /// <summary>
        /// Gets the default (first) configuration loaded into the system.  If none is defined it returns a new <see cref="EmptyNapConfig"/> instance.
        /// </summary>
        public static INapConfig Default => _napConfigCreator != null ? _napConfigCreator() : new EmptyNapConfig();

        /// <summary>
        /// Adds a configuration type into the system, allowing the configuration to be used instead of <see cref="EmptyNapConfig" />.
        /// For performance, prefer <see cref="AddConfig(Func{INapConfig})"/>.
        /// </summary>
        /// <typeparam name="T">The type of configuration to use for nap configuration.</typeparam>
        public static void AddConfig<T>() where T : INapConfig
        {
            _napConfigCreator = () => Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Adds a configuration type into the system, allowing the configuration to be used instead of <see cref="EmptyNapConfig"/>.
        /// For performance, prefer <see cref="AddConfig(Func{INapConfig})"/>.
        /// </summary>
        /// <param name="t">The type of configuration to use for nap configuration.  Needs to inherit from <see cref="INapConfig"/>.</param>
        public static void AddConfig(Type t)
        {
            _napConfigCreator = () => Activator.CreateInstance(t) as INapConfig;
        }

        /// <summary>
        /// Adds a configuration type into the system, allowing the configuration to be used instead of <see cref="EmptyNapConfig" />.
        /// </summary>
        /// <param name="instanceCreator">The lambda function (or expression) used to create an instance of an <see cref="INapConfig"/> for configuration.</param>
        public static void AddConfig(Func<INapConfig> instanceCreator)
        {
            _napConfigCreator = instanceCreator;
        }

		/// <summary>
		/// Register a plugin for use with each and every <see cref="NapRequest"/>.
		/// </summary>
		/// <param name="plugin">The plugin to register for nap requests.</param>
		public static void RegisterPlugin(IPlugin plugin) => _plugins.Add(plugin);

		/// <summary>
		/// Register a plugin for use with each and every <see cref="NapRequest"/>.
		/// </summary>
		/// <typeparam name="T">The plugin type to register for nap requests.  Must have a parameterless constructor.</typeparam>
		public static void RegisterPlugin<T>() where T : IPlugin, new() => RegisterPlugin(Activator.CreateInstance<T>());

		/// <summary>
		/// Remove a previously registered plugin from the list of plugins being used.
		/// </summary>
		/// <param name="plugin">The instance of the plugin to remove.</param>
		/// <returns>True if the plugin has been successfully unregistered; otherwise false.</returns>
		public static bool UnregisterPlugin(IPlugin plugin) => _plugins.Remove(plugin);

		/// <summary>
		/// Remove a previously registered plugin from the list of plugins being used.
		/// </summary>
		/// <typeparam name="T">The type of the plugin to remove.</typeparam>
		/// <returns>True if the plugin has been successfully unregistered; otherwise false.</returns>
		public static bool UnregisterPlugin<T>()
		{
			var plugin = _plugins.FirstOrDefault(p => p.GetType() == typeof(T));
			return plugin != null && _plugins.Remove(plugin);
		}

        /// <summary>
        /// Resets the configuration to use the default internal configuration.
        /// </summary>
        public static void ResetConfig()
        {
            _napConfigCreator = null;
        }
    }
}