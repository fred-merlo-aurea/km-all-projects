using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace ECN_Framework_EntitiesTests.ConfigureProject
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
                Fixture = new Fixture().Customize(new AutoMoqCustomization());
                Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }
        }
    }
}