using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignTest : Fakes
    {
        private const int CampaignId = 1;
        private const int CustomerId = 2;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void GetByCampaignID_WithValidId_ReturnsCampaign()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 1;

            // Act
            var result = Campaign.GetByCampaignID(CampaignId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<ECN_Framework_Entities.Communicator.Campaign>());
        }

        [Test]
        public void GetByCustomerID_WithValidId_ReturnsList()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = Campaign.GetByCustomerID(CustomerId);

            // Assert
            resultList.Count.ShouldBe(_maxRowCount);
        }
    }
}
