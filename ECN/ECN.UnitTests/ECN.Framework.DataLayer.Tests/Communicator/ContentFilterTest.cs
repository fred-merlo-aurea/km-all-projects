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
    public class ContentFilterTest : Fakes
    {
        private const int ContentId = 10;
        private const int FilterId = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void ContentFilter_Delete_ShouldFillAllParameters_UpdatesTable()
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
            ContentFilter.Delete(ContentId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentFilterDeleteAll),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        public void ContentFilter_DeleteWithFilterId_ShouldFillAllParameters_UpdatesTable()
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
            ContentFilter.Delete(ContentId, FilterId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureContentFilterDeleteSingle),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamFilterId).ShouldBe(FilterId.ToString()),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }
    }
}
