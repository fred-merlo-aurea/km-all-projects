using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using ecn.publisher.main.Edition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Ecn.Publisher.Tests.main.Edition
{
    /// <summary>
    /// Unit Test class for <see cref="AddLinks"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class AddLinksTest
    {
        private const string Pages = "Pages";
        private const string CustomerId = "CustomerID";

        [TestCase(Pages, 1)]
        [TestCase(CustomerId, 1)]
        public void IntegerProperty_DefaultValueSetValue_ReturnsDefaultValueOrSetValue(string propertyName, int defaultValueExp)
        {
            // Arrange
            using (var testObject = new AddLinks())
            {
                var privateObject = new PrivateObject(testObject);

                // Act
                var defaultValue = (int)privateObject.GetFieldOrProperty(propertyName);
                privateObject.SetFieldOrProperty(propertyName, 10);

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => defaultValue.ShouldBe(defaultValueExp),
                    () => ((int)privateObject.GetFieldOrProperty(propertyName)).ShouldBe(10));
            }
        }
    }
}
