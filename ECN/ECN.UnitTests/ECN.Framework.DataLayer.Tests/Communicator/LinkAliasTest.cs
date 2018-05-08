using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN.Framework.DataLayer.Tests.Communicator.Common;
using ECN_Framework_DataLayer.Communicator;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using NUnit.Framework;
using Shouldly;
using CommLinkAlias = ECN_Framework_Entities.Communicator.LinkAlias;

namespace ECN.Framework.DataLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LinkAliasTest : Fakes
    {
        private const int LinkOwnerId = 10;
        private const int ContentId = 20;
        private const int CustomerId = 30;
        private const int UserId = 40;
        private const int AliasId = 50;
        private const int CodeId = 60;
        private const string Alias = "Sample Alias";
        private const string Link = "Link";
        
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [Test]
        public void LinkAlias_GetByOwnerId_ShouldFillAllParameters_ReturnsListLinkAlias()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLinkAlias.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommLinkAlias>();
            };

            // Act
            var result = LinkAlias.GetByOwnerID(LinkOwnerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkAliasSelectOwnerId),
                () => GetParameterValue(Consts.ParamOwnerId).ShouldBe(LinkOwnerId.ToString()),
                () => result.ShouldBeOfType<List<CommLinkAlias>>());
        }

        [Test]
        public void LinkAlias_GetByLink_ShouldFillAllParameters_ReturnsListLinkAlias()
        {
            // Arrange
            var commandText = string.Empty;
            ShimLinkAlias.GetListSqlCommand = command =>
            {
                commandText = command.CommandText;
                ParameterCollection = command.Parameters;
                return new List<CommLinkAlias>();
            };

            // Act
            var result = LinkAlias.GetByLink(ContentId, Link);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkAliasSelectLink),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => GetParameterValue(Consts.ParamLink).ShouldBe(Link),
                () => result.ShouldBeOfType<List<CommLinkAlias>>());
        }

        [Test]
        public void LinkAlias_Delete_ShouldFillAllParameters_UpdatesTable()
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
            LinkAlias.Delete(ContentId, CustomerId, UserId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkAliasDeleteAll),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamUserId).ShouldBe(UserId.ToString()));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void LinkAlias_IsValidated_ShouldFillAllParameters_ReturnsBoolean(int execResult)
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
            var result = LinkAlias.Exists(AliasId, Alias, ContentId, CustomerId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureLinkAliasExistsByAlias),
                () => GetParameterValue(Consts.ParamCustomerId).ShouldBe(CustomerId.ToString()),
                () => GetParameterValue(Consts.ParamAlias).ShouldBe(Alias),
                () => GetParameterValue(Consts.ParamAliasId).ShouldBe(AliasId.ToString()),
                () => GetParameterValue(Consts.ParamContentId).ShouldBe(ContentId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void LinkAlias_CodeUsedInLinkAlias_ShouldFillParameter_ReturnsBoolean(int execResult)
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
            var result = LinkAlias.CodeUsedInLinkAlias(CodeId);

            // Assert
            ParameterCollection.ShouldNotBeNull();
            ParameterCollection.ShouldSatisfyAllConditions(
                () => commandText.ShouldBe(Consts.ProcedureCodeExistsLinkTypeId),
                () => GetParameterValue(Consts.ParamLinkTypeId).ShouldBe(CodeId.ToString()),
                () => result.ShouldBe(execResult > 0));
        }
    }
}
