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
    public class EmailDataValuesTest : Fakes
    {
        private const int GroupId = 10;
        private const int GroupDataFieldsId = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void EmailDataValues_Delete_ShouldFillAllParameters_UpdatesTable()
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
            EmailDataValues.Delete(GroupId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailDataValuesDeleteSingle),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void EmailDataValues_DeleteWithGroupDataFieldsId_ShouldFillAllParameters_UpdatesTable()
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
            EmailDataValues.Delete(GroupId, GroupDataFieldsId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureEmailDataValuesDeleteSingle),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamGroupId).ShouldBe(GroupId.ToString()),
                () => GetParameterValue(Consts.ParamGroupDataFieldsId).ShouldBe(GroupDataFieldsId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }
    }
}
