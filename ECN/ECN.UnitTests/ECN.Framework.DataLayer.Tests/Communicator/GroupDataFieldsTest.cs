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
    public class GroupDataFieldsTest : Fakes
    {
        private const int CustomerId = 6;
        private const int UserId = 7;
        private const int GroupId = 8;
        private const int GroupDataFieldsId = 9;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void Delete_WithValidId_All_ShouldFillAllParameters()
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
            GroupDataFields.Delete(GroupId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldSatisfyAllConditions(
                () => ParameterCollection.ShouldNotBeNull(),
                () => commandText.ShouldBe(Consts.ProcedureGroupDataFieldsDeleteAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
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
            GroupDataFields.Delete(GroupId, GroupDataFieldsId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureGroupDataFieldsDeleteSingle),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamGroupDataFieldsId).ShouldBe(GroupDataFieldsId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void GetByID_WithValidId_ReturnsGroupDataFields()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 1;

            // Act
            var result = GroupDataFields.GetByID(GroupDataFieldsId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBeOfType<ECN_Framework_Entities.Communicator.GroupDataFields>());
        }

        [Test]
        public void GetByDataFieldSetID_WithValidId_ReturnsGroupDataFieldsIdList()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = GroupDataFields.GetByDataFieldSetID(GroupId, GroupDataFieldsId);

            // Assert
            resultList.Count.ShouldBe(_maxRowCount);
        }

        [Test]
        public void GetByGroupID_WithValidId_ReturnsGroupDataFieldsIdList()
        {
            // Arrange
            _counter = 0;
            _maxRowCount = 5;

            // Act
            var resultList = GroupDataFields.GetByGroupID(GroupId);

            // Assert
            resultList.Count.ShouldBe(_maxRowCount);
        }
    }
}
