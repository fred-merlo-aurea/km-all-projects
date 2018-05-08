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
    public class FilterConditionTest : Fakes
    {
        private const int CustomerId = 6;
        private const int UserId = 7;
        private const int FilterGroupId = 8;
        private const int FilterConditionId = 9;

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
            FilterCondition.Delete(FilterGroupId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldSatisfyAllConditions(
                () => ParameterCollection.ShouldNotBeNull(),
                () => commandText.ShouldBe(Consts.ProcedureFilterConditionDeleteFilterGroupId),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamFilterGroupId).ShouldBe(FilterGroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void Delete_FilterConditionWithValidId_ShouldFillAllParameters()
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
            FilterCondition.Delete(FilterGroupId, FilterConditionId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldSatisfyAllConditions(
                () => ParameterCollection.ShouldNotBeNull(),
                () => commandText.ShouldBe(Consts.ProceudreFilterConditionDeleteFilterConditionId),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamFilterConditionId).ShouldBe(FilterConditionId.ToString()),
                () => GetParameterValue(Consts.ParamFilterGroupId).ShouldBe(FilterGroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void GetByFilterConditionID_WithValidId_ReturnsFilterCondition()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 1;

            // Act
            var result = FilterCondition.GetByFilterConditionID(FilterGroupId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<ECN_Framework_Entities.Communicator.FilterCondition>()
            );
        }

        [Test]
        public void GetByFilterGroupID_WithValidId_ReturnsFilterConditionList()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = FilterCondition.GetByFilterGroupID(FilterGroupId);

            // Assert
            resultList.Count.ShouldBe(_maxRowCount);
        }
    }
}
