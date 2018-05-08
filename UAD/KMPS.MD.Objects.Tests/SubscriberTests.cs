using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
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
        public void GetProductDimensionSubscriberData_EV_SendingColumnsList_CorrectXmlInCommandParameter()
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
            Subscriber.GetProductDimensionSubscriberData_EV(_connection, builder, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, true);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            responseGroup.ShouldBe(ExpectedResponseGroups);
            responseGroupDesc.ShouldBe(ExpectedResponseGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedPubSubscriptionsExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }

        [Test]
        public void GetProductDimensionSubscriberData_CLV_SendingColumnsList_CorrectXmlInCommandParameter()
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
            Subscriber.GetProductDimensionSubscriberData_CLV(_connection, builder, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, true);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            responseGroup.ShouldBe(ExpectedResponseGroups);
            responseGroupDesc.ShouldBe(ExpectedResponseGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedPubSubscriptionsExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
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
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberData);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetProductDimensionSubscriberData_EV_Default_CommandHasCorrectParams()
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
            var result = Subscriber.GetProductDimensionSubscriberData_EV(_connection, query, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, true);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData_EV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetProductDimensionSubscriberData_CLV_Default_CommandHasCorrectParams()
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
            var result = Subscriber.GetProductDimensionSubscriberData_CLV(_connection, query, ColumnsWithDisplayName, PubIds, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, true);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetProductDimensionSubscriberData_CLV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetProductDimensionSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }
        
        [Test]
        public void GetSubscriberData_IsMostRecentDataFalse_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, 1);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataSubscriptionId);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetSubscriberData_IsMostRecentDataTrue_CommandHasCorrectParams()
        {
            // Arrange
            var query = new StringBuilder();
            SqlCommand expectedCommand = null;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                expectedCommand = command;
                return _reader;
            };

            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();

            // Act
            var result = Subscriber.GetSubscriberData(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, true, 1);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData_RecentConsensus");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberData);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetSubscriberData_EV_IsMostRecentDataFalse_CommandHasCorrectParams()
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
            var result = Subscriber.GetSubscriberData_EV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, false);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData_EV ");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetSubscriberData_EV_IsMostRecentDataTrue_CommandHasCorrectParams()
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
            var result = Subscriber.GetSubscriberData_EV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, true, true);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData_RecentConsensus_EV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetSubscriberData_CLV_IsMostRecentDataFalse_CommandHasCorrectParams()
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
            var result = Subscriber.GetSubscriberData_CLV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, false);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData_CLV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetSubscriberData_CLV_IsMostRecentDataTrue_CommandHasCorrectParams()
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
            var result = Subscriber.GetSubscriberData_CLV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, true, true);

            // Assert
            expectedCommand.ShouldNotBeNull();
            expectedCommand.CommandText.ShouldBe("sp_GetSubscriberData_RecentConsensus_CLV");
            ValidateCommandParameters(expectedCommand, ExpectedParametersForGetSubscriberDataUniqueDownload);
            result.ShouldNotBeNull();
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

            UADShimDataFunctions.GetClientSqlConnectionClientConnections = connections => new ShimSqlConnection();

            // Act
            Subscriber.GetSubscriberData(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, 1);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            masterGroup.ShouldBe(ExpectedMasterGroups);
            masterGroupDesc.ShouldBe(ExpectedMasterGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }

        [Test]
        public void GetSubscriberData_EV_SendingColumnsLists_CorrectXmlInCommandParameters()
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
            Subscriber.GetSubscriberData_EV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, false);

            // Assert
            sdoc.ShouldBe(ExpectedStandardFields);
            masterGroup.ShouldBe(ExpectedMasterGroups);
            masterGroupDesc.ShouldBe(ExpectedMasterGroupsDesc);
            subscriptionsExtMapper.ShouldBe(ExpectedExtMapperValues);
            customDoc.ShouldBe(ExpectedCustomFields);
        }

        [Test]
        public void GetSubscriberData_CLV_SendingColumnsLists_CorrectXmlInCommandParameters()
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
            Subscriber.GetSubscriberData_CLV(_connection, query, ColumnsWithDisplayName, ColumnsOnly, ColumnsWithCase, ColumnsWithCase, ColumnsWithCase, 1, PubIds, false, false);

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
