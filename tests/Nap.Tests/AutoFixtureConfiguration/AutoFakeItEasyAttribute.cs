using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Xunit2;

namespace Nap.Tests.AutoFixtureConfiguration
{
    public class AutoFakeItEasyAttribute : AutoDataAttribute
    {
        public AutoFakeItEasyAttribute() : base(new Fixture().Customize(new AutoFakeItEasyCustomization())) { }
    }
}
