using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;
using UADShimDownloadTemplate = FrameworkUAD.DataAccess.Fakes.ShimDownloadTemplate;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    public class DownloadTemplateTest
    {
        private const int DownloadTemplateId = 42;
        private const int UserId = 24;
        private const int ExecuteResult = 12345;
        private const int ExecuteScalarResult = 12;
        private const string DownloadTemplateDeleteCommandText = "e_DownloadTemplate_Delete";
        private const string DownloadTemplateSaveCommandText = "e_DownloadTemplate_Save";
        private const string DownloadTemplateIdKey = "@DownloadTemplateID";
        private const string UserIdKey = "@UserID";
        private const string DownloadTemplateNameKey = "@DownloadTemplateName";
        private const string BrandIdKey = "@BrandID";
        private const string ProductIdKey = "@PubID";
        private const string IsDeletedKey = "@IsDeleted";
        private const string CreatedUserIdKey = "@CreatedUserID";
        private const string CreatedDateKey = "@CreatedDate";
        private const string UpdatedUserIdKey = "@UpdatedUserID";
        private const string UpdatedDateKey = "@UpdatedDate";
        private const string TestDbString = "TestDb";
        private const string LiveDbString = "LiveDb";
        private const int DownloadTemplateBrandId = 12335;
        private const int DownloadTemplateDownloadTemplateId = 54321;
        private const string DownloadTemplateDownloadTemplateName = "DummyTemplateName";
        private const int DownloadTemplateCreatedUserId = 12;
        private const int DownloadTemplateUpdatedUserId = 21;
        private const int DownloadTemplatePubId = 567;
        private const bool DownloadTemplateIsDeleted = true;

        private static readonly DateTime DownloadTemplateCreatedDate = DateTime.MinValue;
        private static readonly DateTime DownloadTemplateUpdatedDate = DateTime.MaxValue;
        private static ClientConnections clientConnections;

        private IDisposable _context;

        private static ClientConnections ClientConnections => clientConnections ?? (clientConnections = new ClientConnections(TestDbString, LiveDbString));

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            AddCommonDbShims();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Delete_ValidArgument_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            ClientConnections actualClientConnections = null;
            var numberOfTimesExecuteCalled = 0;
            var numberOfTimeClearCachedCalled = 0;

            UADShimDownloadTemplate.DeleteCacheClientConnections = clientConnectionsParameter =>
            {
                numberOfTimeClearCachedCalled++;
                actualClientConnections = clientConnectionsParameter;
            };

            ShimDataFunctions.ExecuteSqlCommandSqlConnection = (command, connection) =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return ExecuteResult;
            };

            // Act
            DownloadTemplate.Delete(ClientConnections, DownloadTemplateId, UserId);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(DownloadTemplateDeleteCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(2),
                () => actualSqlCommand.Parameters[DownloadTemplateIdKey].Value.ShouldBe(DownloadTemplateId),
                () => actualSqlCommand.Parameters[UserIdKey].Value.ShouldBe(UserId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => numberOfTimeClearCachedCalled.ShouldBe(1),
                () => actualClientConnections.ShouldBe(ClientConnections));
        }
        
        [Test]
        public void Save_ValidArgument_CommandCreatedAndExecuted()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            ClientConnections actualClientConnections = null;
            var numberOfTimesExecuteCalled = 0;
            var numberOfTimeClearCachedCalled = 0;
            var downloadTemplate = CreateDownloadTemplate();

            UADShimDownloadTemplate.DeleteCacheClientConnections = clientConnectionsParameter =>
            {
                numberOfTimeClearCachedCalled++;
                actualClientConnections = clientConnectionsParameter;
            };

            ShimDataFunctions.ExecuteScalarSqlCommandSqlConnection = (command, connection) => 
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return ExecuteScalarResult;
            };

            // Act
            var result = DownloadTemplate.Save(ClientConnections, downloadTemplate);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldBe(ExecuteScalarResult),
                () => actualSqlCommand.CommandText.ShouldBe(DownloadTemplateSaveCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(9),
                () => actualSqlCommand.Parameters[DownloadTemplateIdKey].Value.ShouldBe(downloadTemplate.DownloadTemplateID),
                () => actualSqlCommand.Parameters[DownloadTemplateNameKey].Value.ShouldBe(downloadTemplate.DownloadTemplateName),
                () => actualSqlCommand.Parameters[CreatedDateKey].Value.ShouldBe(downloadTemplate.CreatedDate),
                () => actualSqlCommand.Parameters[CreatedUserIdKey].Value.ShouldBe(downloadTemplate.CreatedUserID),
                () => actualSqlCommand.Parameters[UpdatedDateKey].Value.ShouldBe(downloadTemplate.UpdatedDate),
                () => actualSqlCommand.Parameters[UpdatedUserIdKey].Value.ShouldBe(downloadTemplate.UpdatedUserID),
                () => actualSqlCommand.Parameters[ProductIdKey].Value.ShouldBe(downloadTemplate.PubID),
                () => actualSqlCommand.Parameters[IsDeletedKey].Value.ShouldBe(downloadTemplate.IsDeleted),
                () => actualSqlCommand.Parameters[BrandIdKey].Value.ShouldBe(downloadTemplate.BrandID),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => numberOfTimeClearCachedCalled.ShouldBe(1),
                () => actualClientConnections.ShouldBe(ClientConnections));
        }

        private static DownloadTemplate CreateDownloadTemplate()
        {
            var downloadTemplate = new DownloadTemplate
            {
                BrandID = DownloadTemplateBrandId,
                DownloadTemplateName = DownloadTemplateDownloadTemplateName,
                DownloadTemplateID = DownloadTemplateDownloadTemplateId,
                CreatedDate = DownloadTemplateCreatedDate,
                CreatedUserID = DownloadTemplateCreatedUserId,
                UpdatedDate = DownloadTemplateUpdatedDate,
                UpdatedUserID = DownloadTemplateUpdatedUserId,
                PubID = DownloadTemplatePubId,
                IsDeleted = DownloadTemplateIsDeleted
            };

            return downloadTemplate;
        }

        private static void AddCommonDbShims()
        {
            ShimSqlConnection.AllInstances.Open = _ => { };
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
        }
    }
}