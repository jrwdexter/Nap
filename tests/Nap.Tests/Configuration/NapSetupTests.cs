using System;
using System.Collections.Generic;
using System.Diagnostics;
using FakeItEasy;
using Nap.Configuration;
using Nap.Plugins.Base;
using Nap.Tests.AutoFixtureConfiguration;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Nap.Tests.Configuration
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Class", "NapSetup")]
    public class NapSetupTests
    {
        private readonly NapSetup _napSetup;

        public NapSetupTests()
        {
            _napSetup = new NapSetup();
        }

        [Fact]
        public void InitialState_ContainsNoPlugins()
        {
            Assert.Empty(_napSetup.Plugins);
        }

        [Theory, AutoFakeItEasy]
        public void InstallPlugin_AddsPluginToList([Frozen]IPlugin plugin)
        {
            _napSetup.InstallPlugin(plugin);

            Assert.Contains(plugin, _napSetup.Plugins);
        }

        [Theory, AutoFakeItEasy]
        public void InstallPlugin_ThenUninstall_LeavesNoPlugins([Frozen] IPlugin plugin)
        {
            _napSetup.InstallPlugin(plugin);
            _napSetup.UninstallPlugin(plugin);

            Assert.DoesNotContain(plugin, _napSetup.Plugins);
        }

        [Theory, AutoData]
        public void InstallPlugin_ThenUninstallByType_LeavesNoPlugins([Frozen] TestPlugin plugin)
        {
            _napSetup.InstallPlugin(plugin);
            _napSetup.UninstallPlugin<TestPlugin>();

            Assert.DoesNotContain(plugin, _napSetup.Plugins);
        }

        public class TestPlugin : NapPluginBase
        {
        }
    }
}
