using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nap.Configuration
{
    /// <summary>
    /// The setup class for nap, which provides methods for obtaining a proper configuration class (depending on deployment target - portable or non-portable).
    /// </summary>
    public static class NapSetup
    {
        private static readonly IDictionary<Type, Func<INapConfig>> _enabledConfigs = new Dictionary<Type, Func<INapConfig>>();

        /// <summary>
        /// Gets the default (first) configuration loaded into the system.  If none is defined it returns a new <see cref="EmptyNapConfig"/> instance.
        /// </summary>
        public static INapConfig Default => _enabledConfigs.Any() ? _enabledConfigs.First().Value() : new EmptyNapConfig();

        /// <summary>
        /// Adds a configuration type into the system, allowing the configuration to be used instead of <see cref="EmptyNapConfig"/>.
        /// </summary>
        public static void AddConfig<T>() where T : INapConfig
        {
            _enabledConfigs.Add(typeof(T), () => Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Adds a configuration type into the system, allowing the configuration to be used instead of <see cref="EmptyNapConfig"/>.
        /// </summary>
        public static void AddConfig<T>(Expression<Func<T>> instanceCreator) where T : INapConfig
        {
            var castingExpression = Expression.Lambda<Func<INapConfig>>(Expression.Convert(instanceCreator, typeof(INapConfig)));
            _enabledConfigs.Add(typeof(T), castingExpression.Compile());
        }
    }
}