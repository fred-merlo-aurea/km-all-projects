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
    public class FilterGroupTest : Fakes
    {
        private const int FilterId = 1;
        private const int CustomerId = 2;
        private const int UserId = 3;
        private const int FilterGroupId = 4;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void Delete_WithValidId_ShouldFillAllParameters()
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
            FilterGroup.Delete(FilterId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldSatisfyAllConditions(
                () => ParameterCollection.ShouldNotBeNull(),
                () => commandText.ShouldBe(Consts.ProcedureFilterGroupDeleteFilterId),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamFilterId).ShouldBe(FilterId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void Delete_FilterGroupWithValidId_ShouldFillAllParameters()
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
            FilterGroup.Delete(FilterId, FilterGroupId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldSatisfyAllConditions(
                () => ParameterCollection.ShouldNotBeNull(),
                () => commandText.ShouldBe(Consts.ProcedureFilterGroupDeleteFilterGroupId),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamFilterId).ShouldBe(FilterId.ToString()),
                () => GetParameterValue(Consts.ParamFilterGroupId).ShouldBe(FilterGroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void GetByFilterGroupID_WithValidId_ReturnsFilterGroup()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 1;

            // Act
            var result = FilterGroup.GetByFilterGroupID(FilterGroupId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<ECN_Framework_Entities.Communicator.FilterGroup>());
        }

        [Test]
        public void GetByFilterID_WithValidId_ReturnsFilterGroupList()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = FilterGroup.GetByFilterID(FilterId);

            // Assert
            resultList.Count.ShouldBe(_maxRowCount);
        }
    }
}
