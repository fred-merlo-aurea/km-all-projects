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
    public class ConversionLinksTest : Fakes
    {
        private const int LayoutId = 10;
        private const int LinkId = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        private const string LinkName = "LinkName";
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void ConversionLinks_Exists_ShouldFillAllParameters_ReturnsCorrectValue(int execResult)
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
            var result = ConversionLinks.Exists(LinkId, LinkName, LayoutId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureConversionLinksExistsByName),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamLinkName).ShouldBe(LinkName),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        public void ConversionLinks_Delete_ShouldFillAllParameters_UpdatesTable()
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
            ConversionLinks.Delete(LayoutId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureConversionLinksDeleteAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void ConversionLinks_DeleteWithLinkId_ShouldFillAllParameters_UpdatesTable()
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
            ConversionLinks.Delete(LayoutId, LinkId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureConversionLinksDeleteSingle),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamLinkId).ShouldBe(LinkId.ToString()),
                () => GetParameterValue(Consts.ParamLayoutId).ShouldBe(LayoutId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }
    }
}
