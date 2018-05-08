using System;
using System.Diagnostics.CodeAnalysis;
using ecn.communicator.main.lists;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="groupsubscribePrePopScript"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GroupSubscribePrePopScriptTest : BaseListsTest<groupsubscribePrePopScript>
    {
        private const int SFID = 20;
        private const string SFIDPropertyName = "SFID";
        private const string SFIDQueryStringKey = "SFID";
        private const int CustomerId = 40;
        private const string CustomerIdPropertyName = "CustomerId";
        private const string CustomerIdQueryStringKey = "cuID";
        
        [Test]
        public void SFIDGetter_IfQueryStringContainsSFID_ReturnsSFID()
        {
            // Arrange
            QueryString.Add(SFIDQueryStringKey, SFID.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(SFID));
        }

        [Test]
        public void SFIDGetter_IfQueryStringDoesNotContainSFID_ReturnsDefaultValue()
        {
            // Arrange
            // set nothing to query string

            // Act
            var returnedValue = privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void CustomerIdGetter_IfQueryStringContainsCustomerId_ReturnsCustomerId()
        {
            // Arrange
            QueryString.Add(CustomerIdQueryStringKey, CustomerId.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(CustomerIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(CustomerId));
        }

        [Test]
        public void CustomerIdGetter_IfQueryStringDoesNotContainCustomerId_ReturnsDefaultValue()
        {
            // Arrange
            // set nothing to query string

            // Act
            var returnedValue = privateObject.GetProperty(CustomerIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }
	}
}