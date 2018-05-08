using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using NUnit.Framework;

namespace AUT.ConfigureTestProjects
{
    [ExcludeFromCodeCoverage]
    public abstract class AbstractGenericTest
    {
        protected static IFixture Fixture;

        [SetUp]
        public void Init()
        {
            if (Fixture == null)
            {
                Fixture = new Fixture().Customize(customization: new AutoMoqPropertiesCustomization());
                Fixture.Behaviors.Add(item: new OmitOnRecursionBehavior());
            }
        }
    }
}