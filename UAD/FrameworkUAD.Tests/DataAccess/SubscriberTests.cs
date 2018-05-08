using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Text;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.Tests.DataAccess
{
    [TestFixture]
    public class SubscriberTests
    {
        private const string SubscriptionFields = "@SubscriptionFields";
        private const string StandardColumns = "@StandardColumns";
        private const string MasterGroupValues = "@MasterGroupValues";
        private const string MasterGroupValuesDesc = "@MasterGroupValues_Desc";
        private const string SubscriptionsExtMapperValues = "@SubscriptionsExtMapperValues";
        private const string CustomColumns = "@CustomColumns";
        private const string PubSubscriptionsExtMapperValues = "@PubSubscriptionsExtMapperValues";
        private const string ResponseGroupID = "@ResponseGroupID";
        private const string ResponseGroupIDDesc = "@ResponseGroupID_Desc";

        private const string ExpectedParametersForGetProductDimensionSubscriberData = 
            "@Queries,@SubscriptionFields,@PubID,@ResponseGroupID,@ResponseGroupID_Desc,@PubSubscriptionsExtMapperValues,@CustomColumns,@BrandID,@DownloadCount";

        private const string ExpectedParametersForGetProductDimensionSubscriberDataSubscriberId = 
            "@Queries,@SubscriptionFields,@PubID,@ResponseGroupID,@ResponseGroupID_Desc,@PubSubscriptionsExtMapperValues,@CustomColumns,@BrandID,@DownloadCount,@FilterBased,@SubscriberIds";

        private const string ExpectedParametersForGetProductDimensionSubscriberDataIssueId = 
            "@Queries,@SubscriptionFields,@PubID,@IssueID,@ResponseGroupID,@ResponseGroupID_Desc,@PubSubscriptionsExtMapperValues,@CustomColumns,@BrandID,@DownloadCount";
        
        private const string ExpectedParametersForGetProductDimensionSubscriberDataUniqueDownload = 
            "@Queries,@SubscriptionFields,@PubID,@ResponseGroupID,@ResponseGroupID_Desc,@PubSubscriptionsExtMapperValues,@CustomColumns,@BrandID,@uniquedownload";

        private const string ExpectedParametersForGetSubscriberData = 
            "@Queries,@StandardColumns,@MasterGroupValues,@MasterGroupValues_Desc,@SubscriptionsExtMapperValues,@CustomColumns,@BrandID,@PubIDs,@DownloadCount";

        private const string ExpectedParametersForGetSubscriberDataSubscriptionId = 
            "@Queries,@StandardColumns,@MasterGroupValues,@MasterGroupValues_Desc,@SubscriptionsExtMapperValues,@CustomColumns,@BrandID,@PubIDs,@DownloadCount,@SubscriptionID";

        private const string ExpectedParametersForGetSubscriberDataUniqueDownload = 
            "@Queries,@StandardColumns,@MasterGroupValues,@MasterGroupValues_Desc,@SubscriptionsExtMapperValues,@CustomColumns,@BrandID,@PubIDs,@uniquedownload";

        public readonly List<string> ColumnsWithDisplayName = new List<string> {"Table.Id|0", "Table.Name as Full Name|1"};
        public readonly List<string> ColumnsWithCase = new List<string> {"Id|0", "Name|1"};
        public readonly List<string> ColumnsOnly = new List<string> {"Id", "Name"};
        public readonly List<string> ColumnsEmpty = new List<string>();
        public readonly List<int> PubIds = new List<int> {1, 2};

        public readonly string ExpectedStandardFields = "<StandardFields>" + Environment.NewLine +
                                                        "  <StandardField>" + Environment.NewLine +
                                                        "    <Column>Table.Id</Column>" + Environment.NewLine +
                                                        "    <DisplayName>Id</DisplayName>" + Environment.NewLine +
                                                        "    <Case>0</Case>" + Environment.NewLine +
                                                        "  </StandardField>" + Environment.NewLine +
                                                        "  <StandardField>" + Environment.NewLine +
                                                        "    <Column>Table.Name</Column>" + Environment.NewLine +
                                                        "    <DisplayName>Full Name</DisplayName>" + Environment.NewLine +
                                                        "    <Case>1</Case>" + Environment.NewLine +
                                                        "  </StandardField>" + Environment.NewLine +
                                                        "</StandardFields>";
            

        public readonly string ExpectedMasterGroups = "<MasterGroups>" + Environment.NewLine +
                                                      "  <MasterGroup>" + Environment.NewLine +
                                                      "    <Column>Id</Column>" + Environment.NewLine +
                                                      "  </MasterGroup>" + Environment.NewLine +
                                                      "  <MasterGroup>" + Environment.NewLine +
                                                      "    <Column>Name</Column>" + Environment.NewLine +
                                                      "  </MasterGroup>" + Environment.NewLine +
                                                      "</MasterGroups>";

        public readonly string ExpectedMasterGroupsDesc = "<MasterGroups>" + Environment.NewLine +
                                                          "  <MasterGroup>" + Environment.NewLine +
                                                          "    <Column>Id</Column>" + Environment.NewLine +
                                                          "    <Case>0</Case>" + Environment.NewLine +
                                                          "  </MasterGroup>" + Environment.NewLine +
                                                          "  <MasterGroup>" + Environment.NewLine +
                                                          "    <Column>Name</Column>" + Environment.NewLine +
                                                          "    <Case>1</Case>" + Environment.NewLine +
                                                          "  </MasterGroup>" + Environment.NewLine +
                                                          "</MasterGroups>";

        public readonly string ExpectedResponseGroups = "<ResponseGroups>" + Environment.NewLine +
                                                      "  <ResponseGroup>" + Environment.NewLine +
                                                      "    <Column>Id</Column>" + Environment.NewLine +
                                                      "  </ResponseGroup>" + Environment.NewLine +
                                                      "  <ResponseGroup>" + Environment.NewLine +
                                                      "    <Column>Name</Column>" + Environment.NewLine +
                                                      "  </ResponseGroup>" + Environment.NewLine +
                                                      "</ResponseGroups>";

        public readonly string ExpectedResponseGroupsDesc = "<ResponseGroups>" + Environment.NewLine +
                                                          "  <ResponseGroup>" + Environment.NewLine +
                                                          "    <Column>Id</Column>" + Environment.NewLine +
                                                          "    <Case>0</Case>" + Environment.NewLine +
                                                          "  </ResponseGroup>" + Environment.NewLine +
                                                          "  <ResponseGroup>" + Environment.NewLine +
                                                          "    <Column>Name</Column>" + Environment.NewLine +
                                                          "    <Case>1</Case>" + Environment.NewLine +
                                                          "  </ResponseGroup>" + Environment.NewLine +
                                                          "</ResponseGroups>";

        public readonly string ExpectedExtMapperValues = "<SubscriptionsExtMapperValues>" + Environment.NewLine +
                                                         "  <SubscriptionsExtMapperValue>" + Environment.NewLine +
                                                         "    <Column>Id</Column>" + Environment.NewLine +
                                                         "    <Case>0</Case>" + Environment.NewLine +
                                                         "  </SubscriptionsExtMapperValue>" + Environment.NewLine +
                                                         "  <SubscriptionsExtMapperValue>" + Environment.NewLine +
                                                         "    <Column>Name</Column>" + Environment.NewLine +
                                                         "    <Case>1</Case>" + Environment.NewLine +
                                                         "  </SubscriptionsExtMapperValue>" + Environment.NewLine +
                                                         "</SubscriptionsExtMapperValues>";

        public readonly string ExpectedPubSubscriptionsExtMapperValues = "<PubSubscriptionsExtMapperValues>" + Environment.NewLine +
                                                                         "  <PubSubscriptionsExtMapperValue>" + Environment.NewLine +
                                                                         "    <Column>Id</Column>" + Environment.NewLine +
                                                                         "    <Case>0</Case>" + Environment.NewLine +
                                                                         "  </PubSubscriptionsExtMapperValue>" + Environment.NewLine +
                                                                         "  <PubSubscriptionsExtMapperValue>" + Environment.NewLine +
                                                                         "    <Column>Name</Column>" + Environment.NewLine +
                                                                         "    <Case>1</Case>" + Environment.NewLine +
                                                                         "  </PubSubscriptionsExtMapperValue>" + Environment.NewLine +
                                                                         "</PubSubscriptionsExtMapperValues>";

        

        public readonly string ExpectedCustomFields = "<CustomFields>" + Environment.NewLine +
                                                      "  <CustomField>" + Environment.NewLine +
                                                      "    <Column>Id</Column>" + Environment.NewLine +
                                                      "    <DisplayName>Id</DisplayName>" + Environment.NewLine +
                                                      "    <Case>0</Case>" + Environment.NewLine +
                                                      "  </CustomField>" + Environment.NewLine +
                                                      "  <CustomField>" + Environment.NewLine +
                                                      "    <Column>Name</Column>" + Environment.NewLine +
                                                      "    <DisplayName>Name</DisplayName>" + Environment.NewLine +
                                                      "    <Case>1</Case>" + Environment.NewLine +
                                                      "  </CustomField>" + Environment.NewLine +
                                                      "</CustomFields>";

        private const string ProcGetSubscriberData = "sp_GetSubscriberData";
        private const string ProcGetSubscriberDataCLV = "sp_GetSubscriberData_CLV";
        private const string ProcGetSubscriberDataEV = "sp_GetSubscriberData_EV ";
        private const string ProcGetArchivedProductDimensionSubscriberData = "sp_GetArchivedProductDimensionSubscriberData";
        private const string ProcGetSubscriberDataRecentConsensus = "sp_GetSubscriberData_RecentConsensus";
        private const string ProcGetSubscriberDataRecentConsensusCLV = "sp_GetSubscriberData_RecentConsensus_CLV";
        private const string ProcGetSubscriberDataRecentConsensusEV = "sp_GetSubscriberData_RecentConsensus_EV";
        private ClientConnections _connection = new ClientConnections();
        private IDisposable _shims;
        private ShimSqlDataReader _reader;
        
        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();

            _reader = new ShimSqlDataReader();

            ShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();
        }

        [TearDown]
        public void TearDown()
        {
            _reader?.Instance?.Dispose();
            _shims?.Dispose();
        }

        [Test]
        public void GetProductDimensionSubscriberData_SendingColumnsList_CorrectXmlInCommandParameter()
        {
            // Arrange
            var builder = new StringBuilder();
            var sdoc = string.Empty;
            var responseGroup = string.Empty;
            var responseGroupDesc = string.Empty;
            var subscriptionsExtMapper = string.Empty;
            var customDoc = string.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                sdoc = command.Parameters[SubscriptionFields].Value.ToString();
                responseGroup = command.Parameters[ResponseGroupID].Value.ToString();
                responseGroupDesc = command.Parameters[ResponseGroupIDDesc].Value.ToString();
                subscriptionsExtMapper = command.Parameters[PubSubscriptionsExtMapperValues].Value.ToString();
                customDoc = command.Parameters[CustomColumns].Value.ToString();
                return _reader;
            };

            // Act
            Subscriber.GetProductDimensionSubscriberData(_connection, builder, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, 1);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            responseGroup.ShouldBe(ExpectedResponseGroups);
            responseGroupDesc.ShouldBe(ExpectedResponseGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedPubSubscriptionsExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }

        [Test]
        public void GetProductDimensionSubscriberData_Default_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetProductDimensionSubscriberData(_connection, query, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, 1);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataSubscriberId);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetProductDimensionSubscriberData_CLV_Default_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;
            var standardColumns = "Table.Id|0";
            var responseGroupColumns = new List<Entity.ResponseGroup>();
            var responseGroupDescColumns = new List<Entity.ResponseGroup>();
            var pubSubscriptionExtensionsColumns = new List<Entity.ProductSubscriptionsExtensionMapper>();
            var customColumns = string.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetProductDimensionSubscriberData_CLV(_connection, query, standardColumns, PubIds, responseGroupColumns, responseGroupDescColumns, pubSubscriptionExtensionsColumns, customColumns, 1, false);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData_CLV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetProductDimensionSubscriberData_EV_Default_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;
            var standardColumns = "Table.Id|0";
            var responseGroupColumns = new List<Entity.ResponseGroup>();
            var responseGroupDescColumns = new List<Entity.ResponseGroup>();
            var pubSubscriptionExtensionsColumns = new List<Entity.ProductSubscriptionsExtensionMapper>();
            var customColumns = string.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetProductDimensionSubscriberData_EV(_connection, query, standardColumns, PubIds, responseGroupColumns, responseGroupDescColumns, pubSubscriptionExtensionsColumns, customColumns, 1, false);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData_EV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetProductDimensionSubscriberData_MasterGroupObjects_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;
            var standardColumns = "Table.Id|0";
            var responseGroupColumns = new List<Entity.ResponseGroup>();
            var responseGroupDescColumns = new List<Entity.ResponseGroup>();
            var pubSubscriptionExtensionsColumns = new List<Entity.ProductSubscriptionsExtensionMapper>();
            var customColumns = string.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetProductDimensionSubscriberData(_connection, query, standardColumns, PubIds, responseGroupColumns, responseGroupDescColumns, pubSubscriptionExtensionsColumns, customColumns, 1, 1);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberData);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetArchivedProductDimensionSubscriberData_SendingColumnsList_CorrectXmlInCommandParameter()
        {
            // Arrange
            var sdoc = string.Empty;
            var responseGroup = string.Empty;
            var responseGroupDesc = string.Empty;
            var subscriptionsExtMapper = string.Empty;
            var customDoc = string.Empty;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                sdoc = command.Parameters[SubscriptionFields].Value.ToString();
                responseGroup = command.Parameters[ResponseGroupID].Value.ToString();
                responseGroupDesc = command.Parameters[ResponseGroupIDDesc].Value.ToString();
                subscriptionsExtMapper = command.Parameters[PubSubscriptionsExtMapperValues].Value.ToString();
                customDoc = command.Parameters[CustomColumns].Value.ToString();
                return _reader;
            };

            // Act
            Subscriber.GetArchivedProductDimensionSubscriberData(_connection, string.Empty, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, 1, 1);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            responseGroup.ShouldBe(ExpectedResponseGroups);
            responseGroupDesc.ShouldBe(ExpectedResponseGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedPubSubscriptionsExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }
        
        [Test]
        public void GetArchivedProductDimensionSubscriberData_Default_CommandHasCorrectParams()
        {
            // Arrange
            var query = string.Empty;
            var columnsList = new List<string> {"Table.Id|0", "Table.Name as Full Name|1"};
            var pubIds = new List<int> {1, 2};
            var responseGroupIds = new List<string>();
            var responseGroupDescIds = new List<string>();
            var pubExtensionsColumns = new List<string>();
            var customColumns = new List<string>();
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetArchivedProductDimensionSubscriberData(_connection, query, columnsList, pubIds, responseGroupIds, responseGroupDescIds, pubExtensionsColumns, customColumns, 1, 1, 1);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetArchivedProductDimensionSubscriberData),
                () => result.ShouldNotBeNull()
            );

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataIssueId);
        }

        [Test]
        public void GetSubscriberData_IsMostRecentDataFalse_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var columnsList = new List<string> {"Table.Id|0", "Table.Name as Full Name|1"};
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<string>();
            var masterGroupDescColumns = new List<string>();
            var subExtensionsColumns = new List<string>();
            var customColumns = new List<string>();
            var isMostRecentData = false;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, columnsList, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, 1);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberData),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataSubscriptionId);
        }

        [Test]
        public void GetSubscriberData_IsMostRecentDataTrue_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var columnsList = new List<string> {"Table.Id|0", "Table.Name as Full Name|1"};
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<string>();
            var masterGroupDescColumns = new List<string>();
            var subExtensionsColumns = new List<string>();
            var customColumns = new List<string>();
            var isMostRecentData = true;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, columnsList, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, 1);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataRecentConsensus),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberData);
        }

        [Test]
        public void GetSubscriberData_MasterGroupObjectsIsMostRecentDataFalse_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = false;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, 1);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberData),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataSubscriptionId);
        }

        [Test]
        public void GetSubscriberData_MasterGroupObjectsIsMostRecentDataTrue_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = true;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, 1);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataRecentConsensus),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberData);
        }

        [Test]
        public void GetSubscriberData_CLV_MasterGroupObjectsIsMostRecentDataFalse_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = false;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData_CLV(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, false);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataCLV),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
        }

        [Test]
        public void GetSubscriberData_CLV_MasterGroupObjectsIsMostRecentDataTrue_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = true;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData_CLV(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, true);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataRecentConsensusCLV),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
        }

        [Test]
        public void GetSubscriberData_EV_MasterGroupObjectsIsMostRecentDataFalse_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = false;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData_EV(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, false);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataEV),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
        }

        [Test]
        public void GetSubscriberData_EV_MasterGroupObjectsIsMostRecentDataTrue_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            var standardColumns = "Table.Id|0";
            var pubIds = new List<int> {1, 2};
            var masterGroupColumns = new List<Entity.MasterGroup>();
            var masterGroupDescColumns = new List<Entity.MasterGroup>();
            var subExtensionsColumns = new List<Entity.SubscriptionsExtensionMapper>();
            var customColumns = string.Empty;
            var isMostRecentData = true;
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            // Act
            var result = Subscriber.GetSubscriberData_EV(_connection, query, standardColumns, masterGroupColumns, masterGroupDescColumns, subExtensionsColumns, customColumns, 1, pubIds, isMostRecentData, true);

            // Assert
            expectedCommand.ShouldSatisfyAllConditions(
                () => expectedCommand.ShouldNotBeNull(),
                () => expectedCommand.CommandText.ShouldBe(ProcGetSubscriberDataRecentConsensusEV),
                () => result.ShouldNotBeNull());

            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
        }

        [Test]
        public void GetSubscriberData_SendingColumnsLists_CorrectXmlInCommandParameters()
        {
            // Arrange
            var query = new StringBuilder();
            var sdoc = string.Empty;
            var masterGroup = string.Empty;
            var masterGroupDesc = string.Empty;
            var subscriptionsExtMapper = string.Empty;
            var customDoc = string.Empty;
            
            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                sdoc = command.Parameters[StandardColumns].Value.ToString();
                masterGroup = command.Parameters[MasterGroupValues].Value.ToString();
                masterGroupDesc = command.Parameters[MasterGroupValuesDesc].Value.ToString();
                subscriptionsExtMapper = command.Parameters[SubscriptionsExtMapperValues].Value.ToString();
                customDoc = command.Parameters[CustomColumns].Value.ToString();
                
                return _reader;
            };

            // Act
            Subscriber.GetSubscriberData(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, 1);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            masterGroup.ShouldBe(ExpectedMasterGroups);
            masterGroupDesc.ShouldBe(ExpectedMasterGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }

        private void ValidateCommandParameters(SqlCommand command, string expectedParameters)
        {
            var parameters = expectedParameters.Split(',');
            command.Parameters.Count.ShouldBe(parameters.Length);

            for (var paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
            {
                command.Parameters[paramIndex].ParameterName.ShouldBe(parameters[paramIndex]);
            }
        }
    }
}
