using System;

namespace Nap.Configuration
{
    /// <summary>
    /// The setup class for nap, which provides methods for obtaining a proper configuration class (depending on deployment target - portable or non-portable).
    /// </summary>
    public static class NapSetup
    {
        private static Func<INapConfig> _napConfigCreator = null;

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
        /// Resets the configuration to use the default internal configuration.
        /// </summary>
        public static void ResetConfig()
        {
            _napConfigCreator = null;
        }
    }
}