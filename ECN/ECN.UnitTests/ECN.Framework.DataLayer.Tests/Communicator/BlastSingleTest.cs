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
    public class BlastSingleTest : Fakes
    {
        private const int EmailId = 10;
        private const int LayoutPlanId = 20;
        private const int BlastId = 30;
        private const int CustomerId = 40;
        private const int UserId = 50;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void BlastSingle_ExistsByBlastEmailLayoutPlan_ShouldFillAllParameters_ReturnsCorrectValue(int execResult)
        {
            // Arrange
            var commandText = string.Empty;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, conn) =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return execResult;
            };

            // Act
            var result = BlastSingle.ExistsByBlastEmailLayoutPlan(BlastId, EmailId, LayoutPlanId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureBlastSingleExistsByBlastEmailLayoutPlan),
                () => GetParameterValue(Consts.ParamBlastId).ShouldBe(BlastId.ToString()),
                () => GetParameterValue(Consts.ParamEmailId).ShouldBe(EmailId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutPlanId).ShouldBe(LayoutPlanId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        public void BlastSingle_Delete_ShouldFillAllParameters_UpdatesTable()
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
            BlastSingle.DeleteByEmailID(EmailId, LayoutPlanId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureBlastSingleDeleteEmailId),
                () => GetParameterValue(Consts.ParamEmailId).ShouldBe(EmailId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutPlanId).ShouldBe(LayoutPlanId.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void BlastSingle_DeleteNoOpenFromAbandonEmailId_ShouldFillAllParameters_UpdatesTable()
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
            BlastSingle.DeleteNoOpenFromAbandon_EmailID(EmailId, LayoutPlanId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureBlastSingleDeleteNoOpenFromAbandonEmailId),
                () => GetParameterValue(Consts.ParamEmailId).ShouldBe(EmailId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutPlanId).ShouldBe(LayoutPlanId.ToString()),
                () => GetParameterValue(Consts.ParamUpdatedUserId).ShouldBe(UserId.ToString()));
        }
    }
}
