using Nap.Plugins.Base;

namespace Nap.Configuration
{
    /// <summary>
    /// A plugin which allows for Nap to use *.config files instead of empty initial configurations.
    /// </summary>
    public class NapConfigurationPlugin : NapPlugin
    {
        /// <summary>
        /// Gets Nap configuration from *.config files instead of from an empty implementation.
        /// </summary>
        /// <returns>The new <see cref="NapConfig"/> object on success.</returns>
        public override INapConfig GetConfiguration()
        {
            return NapConfig.GetCurrent();
        }
    }
}
