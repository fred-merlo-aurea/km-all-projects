using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommLinkOwnerIndex = ECN_Framework_Entities.Communicator.LinkOwnerIndex;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LinkOwnerIndexTest : Fakes
    {
        private const int LinkOwnerIndexId = 10;
        private const int CustomerId = 20;
        private const int UserId = 30;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void LinkOwnerIndex_GetByOwnerId_ShouldFillLinkOwnerIndexId_ReturnsLinkOwnerIndex()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLinkOwnerIndex.GetSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new CommLinkOwnerIndex();
            };

            // Act
            var result = LinkOwnerIndex.GetByOwnerID(LinkOwnerIndexId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkOwnerIndexSelectSingle),
                () => GetParameterValue(Consts.ParamLinkOwnerIndexId).ShouldBe(LinkOwnerIndexId.ToString()),
                () => result.ShouldBeOfType<CommLinkOwnerIndex>());
        }

        [Test]
        public void LinkOwnerIndex_GetByCustomerId_ShouldFillAllParameters_ReturnsListLinkOwnerIndex()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLinkOwnerIndex.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommLinkOwnerIndex>();
            };

            // Act
            var result = LinkOwnerIndex.GetByCustomerID(CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkOwnerIndexSelectAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBeOfType<List<CommLinkOwnerIndex>>());
        }

        [Test]
        public void LinkOwnerIndex_Delete_ShouldFillAllParameters_UpdatesTable()
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return true;
            };

            // Act
            LinkOwnerIndex.Delete(CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkOwnerIndexDeleteAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }
    }
}
