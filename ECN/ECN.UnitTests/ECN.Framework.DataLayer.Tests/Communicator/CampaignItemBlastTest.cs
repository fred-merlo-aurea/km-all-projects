using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommCampaignItemBlast = ECN_Framework_Entities.Communicator.CampaignItemBlast;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignItemBlastTest : Fakes
    {
        private const int CampaignItemId = 10;
        private const int CampaignItemBlastId = 20;
        private const int BlastId = 30;
        private const int UserId = 40;
        private const int CustomerId = 50;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void CampaignItemBlast_GetByCampaignItemBlastId_ShouldFillCampaignItemBlastId_ReturnsCampaignItemBlast()
        {
            // Arrange
            var commandText = string.Empty;
            ShimCampaignItemBlast.GetSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new CommCampaignItemBlast();
            };

            // Act
            var result = CampaignItemBlast.GetByCampaignItemBlastID(CampaignItemBlastId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastSelectCampaignItemBlastId),
                () => GetParameterValue(Consts.ParamCampaignItemBlastId).ShouldBe(CampaignItemBlastId.ToString()),
                () => result.ShouldBeOfType<CommCampaignItemBlast>());
        }

        [Test]
        public void CampaignItemBlast_GetByCampaignItemId_ShouldFillCampaignItemId_ReturnsListCampaignItemBlast()
        {
            // Arrange
            var commandText = string.Empty;
            ShimCampaignItemBlast.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommCampaignItemBlast>();
            };

            // Act
            var result = CampaignItemBlast.GetByCampaignItemID(CampaignItemId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastSelectCampaignItemId),
                () => GetParameterValue(Consts.ParamCampaignItemId).ShouldBe(CampaignItemId.ToString()),
                () => result.ShouldBeOfType<List<CommCampaignItemBlast>>());
        }

        [Test]
        public void CampaignItemBlast_GetByBlastId_ShouldFillBlastId_ReturnsListCampaignItemBlast()
        {
            // Arrange
            var commandText = string.Empty;
            ShimCampaignItemBlast.GetSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new CommCampaignItemBlast();
            };

            // Act
            var result = CampaignItemBlast.GetByBlastID(BlastId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastSelectBlastId),
                () => GetParameterValue(Consts.ParamBlastId).ShouldBe(BlastId.ToString()),
                () => result.ShouldBeOfType<CommCampaignItemBlast>());
        }

        [Test]
        public void CampaignItemBlast_DeleteAll_ShouldFillAllParameters_UpdatesTable()
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
            CampaignItemBlast.Delete(CampaignItemId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastDeleteAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamCampaignItemId).ShouldBe(CampaignItemId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void CampaignItemBlast_DeleteSingle_ShouldFillAllParameters_UpdatesTable()
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
            CampaignItemBlast.Delete(CampaignItemId, CampaignItemBlastId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastDeleteSingle),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamCampaignItemId).ShouldBe(CampaignItemId.ToString()),
                () => GetParameterValue(Consts.ParamCampaignItemBlastId).ShouldBe(CampaignItemBlastId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void CampaignItemBlast_UpdateBlastId_ShouldFillAllParameters_UpdatesTable()
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
            CampaignItemBlast.UpdateBlastID(CampaignItemBlastId, BlastId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCampaignItemBlastUpdateBlastId),
                () => GetParameterValue(Consts.ParamBlastId).ShouldBe(BlastId.ToString()),
                () => GetParameterValue(Consts.ParamCampaignItemBlastId).ShouldBe(CampaignItemBlastId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }
    }
}
