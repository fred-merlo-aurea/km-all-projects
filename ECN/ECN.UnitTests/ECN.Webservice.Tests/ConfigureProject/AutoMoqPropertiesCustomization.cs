using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

namespace ECN_Framework_EntitiesTests.ConfigureProject
{
    public class AutoMoqPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new PropertiesPostprocessor(
                    new MockPostprocessor(
                        new MethodInvoker(
                            new MockConstructorQuery()))));
            fixture.ResidueCollectors.Add(new MockRelay());
        }
    }
}