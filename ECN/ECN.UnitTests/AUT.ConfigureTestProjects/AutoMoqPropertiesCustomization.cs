using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

namespace AUT.ConfigureTestProjects
{
    [ExcludeFromCodeCoverage]
    public class AutoMoqPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new PropertiesPostProcessor(
                    new MockPostprocessor(
                        new MethodInvoker(
                            new MockConstructorQuery()))));
            fixture.ResidueCollectors.Add(new MockRelay());
        }
    }
}