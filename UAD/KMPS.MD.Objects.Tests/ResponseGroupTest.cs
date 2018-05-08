using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KM.Common.Fakes;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KPMSShimDataFunctions = KMPS.MD.Objects.Fakes.ShimDataFunctions;
using ShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
{
    using TestCommonHelpers;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ResponseGroupTest
    {
        private const int ResponseGroupId = 42;
        private const int ProductId = 24;
        private const int ExecuteResult = 12345;
        private const string ResponseGroupDeleteCommandText = "sp_ResponseGroups_Delete";
        private const string ResponseGroupValidateDeleteOrInactiveCommandText = "e_ResponseGroup_Validate_DeleteorInActive";
        private const string ResponseGroupIdKey = "@ResponseGroupId";
        private const string ProductIdKey = "@PubID";
        private const string TestDbString = "TestDb";
        private const string LiveDbString = "LiveDb";
        private const string FilterExportSchedule = "FILTER EXPORT SCHEDULE";
        private const string ReferenceKey = "Reference";
        private const string ReferenceId2Key = "ReferenceID2";
        private const string ReferenceId1Key = "ReferenceID1";
        private const string ReferenceNameKey = "ReferenceName";
        private const string ReferenceFilter = "<a href='../main/FilterExport.aspx?FilterScheduleId="
                                               + ReferenceId2Value + "&FilterID=" + ReferenceId1Value + "'>"
                                               + ReferenceNameValue + "</a>";
        private const string ReferenceId2Value = "ReferenceID2";
        private const string ReferenceId1Value = "ReferenceID1";
        private const string ReferenceNameValue = "ReferenceName";
        private const string DummyReference = "Dummy";
        private const string DummyReferenceName = "DummyName";
        private const string ParameterKeySuffix = " : ";
        private const string DummyDatabaseName = "MyTestDatabase";
        private const string CacheKeyResponseGroup = "RESPONSEGROUP_";
        private const string CacheKeyAllResponseGroup = "ALLRESPONSEGROUP_";
        private const string GetDataMethodName = "GetData";
        private const string GetAllDataMethodName = "GetAllData";
        private const string ResponseGroupName1 = "ResponseGroupName1";
        private const string ResponseGroupName2 = "ResponseGroupName2";
        private const string ProductIdParameterName = "@PubID";
        private const string GetDataSqlCommandText = "select ResponseGroupID, PubID, ResponseGroupName, DisplayName, DateCreated, DateUpdated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, isnull(IsActive, 1) as IsActive, WQT_ResponseGroupID, ResponseGroupTypeID from ResponseGroups where  pubID = @PubID and ISNULL(ResponseGroupTypeId,0) not in (select c.codeID from UAD_Lookup..CodeType ct join UAD_Lookup..Code c on ct.CodeTypeId = c.CodeTypeId where ct.CodeTypeName = 'Response Group'  and c.CodeName = 'Circ Only' ) order by ResponseGroupName";
        private const string GetAllDataSqlCommandText = "select ResponseGroupID, PubID, ResponseGroupName, DisplayName, DateCreated, DateUpdated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, isnull(IsActive, 1) as IsActive, WQT_ResponseGroupID, ResponseGroupTypeID from ResponseGroups where  pubID = @PubID order by ResponseGroupName";

        private static ClientConnections clientConnections;

        private IDisposable _context;

        private static IList<ResponseGroup> ListResponseGroupTestData => new List<ResponseGroup>()
        {
            new ResponseGroup() { ResponseGroupName = ResponseGroupName1 },
            new ResponseGroup() { ResponseGroupName = ResponseGroupName2 }
        };

        private static ClientConnections ClientConnections => clientConnections ?? (clientConnections = new ClientConnections(TestDbString, LiveDbString));

        private static IEnumerable<IDictionary<string, string>> TestData
        {
            get
            {
                yield return new Dictionary<string, string>()
                             {
                                 [ReferenceKey] = FilterExportSchedule,
                                 [ReferenceId2Key] = ReferenceId2Value,
                                 [ReferenceId1Key] = ReferenceId1Value,
                                 [ReferenceNameKey] = ReferenceNameValue,
                             };

                yield return new Dictionary<string, string>()
                             {
                                 [ReferenceKey] = DummyReference,
                                 [ReferenceId2Key] = ReferenceId2Value,
                                 [ReferenceId1Key] = ReferenceId1Value,
                                 [ReferenceNameKey] = DummyReferenceName,
                             };
            }
        }

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
            var actualProductId = 0;

            ShimResponseGroup.DeleteCacheClientConnectionsInt32 = (clientConnectionsParameter, productId) => 
            {
                numberOfTimeClearCachedCalled++;
                actualClientConnections = clientConnectionsParameter;
                actualProductId = productId;
            };

            ShimDataFunctions.ExecuteSqlCommandSqlConnection = (command, connection) =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return ExecuteResult;
            };

            // Act
            ResponseGroup.Delete(ClientConnections, ResponseGroupId, ProductId);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(ResponseGroupDeleteCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[ResponseGroupIdKey].Value.ShouldBe(ResponseGroupId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => numberOfTimeClearCachedCalled.ShouldBe(1),
                () => actualClientConnections.ShouldBe(ClientConnections),
                () => actualProductId.ShouldBe(ProductId));
        }

        [Test]
        public void ValidationForDeleteorInActive_ValidArgument_CommandExecutedCollectionReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return CreateShimDataReader(TestData.ToList());
            }; 

            // Act
            var result = ResponseGroup.ValidationForDeleteorInActive(ClientConnections, ResponseGroupId, ProductId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(ResponseGroupValidateDeleteOrInactiveCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(2),
                () => actualSqlCommand.Parameters[ResponseGroupIdKey].Value.ShouldBe(ResponseGroupId),
                () => actualSqlCommand.Parameters[ProductIdKey].Value.ShouldBe(ProductId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => result.Count.ShouldBe(2),
                () => result[GetParameterName(FilterExportSchedule)].ShouldBe(ReferenceFilter),
                () => result[GetParameterName(DummyReference)].ShouldBe(DummyReferenceName));
        }

        [Test]
        public void DeleteCache_ProductIdFound_RemovedFromCache()
        {
            // Arrange
            var actualDatabaseNames = new HashSet<string>();
            var actualRemoveFromCache = new List<string>();

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) => new object();

            ShimCacheUtil.RemoveFromCacheStringString = (cacheKey, dataBaseName) =>
            {
                actualRemoveFromCache.Add(cacheKey);
                actualDatabaseNames.Add(dataBaseName);
            };

            // Act
            ResponseGroup.DeleteCache(ClientConnections, ProductId);
            
            // Assert
            actualRemoveFromCache.ShouldSatisfyAllConditions(
                () => actualRemoveFromCache.Count.ShouldBe(2),
                () => actualRemoveFromCache.ShouldContain($"{CacheKeyResponseGroup}{ProductId}"),
                () => actualRemoveFromCache.ShouldContain($"{CacheKeyAllResponseGroup}{ProductId}"),
                () => actualDatabaseNames.Count.ShouldBe(1),
                () => actualDatabaseNames.ShouldContain(DummyDatabaseName));
        } 
        
        [Test]
        public void DeleteCache_ProductIdNotFound_NothingDeleted()
        {
            // Arrange
            var actualRemoveFromCache = new List<string>();

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) => null;

            ShimCacheUtil.RemoveFromCacheStringString = (cacheKey, dataBaseName) =>
            {
                actualRemoveFromCache.Add(cacheKey);
            };

            // Act
            ResponseGroup.DeleteCache(ClientConnections, ProductId);
            
            // Assert
            actualRemoveFromCache.ShouldSatisfyAllConditions(
                () => actualRemoveFromCache.Count.ShouldBe(0));
        }
        
        [Test]
        public void DeleteCache_CacheNotEnabled_NothingDeleted()
        {
            // Arrange
            var actualRemoveFromCache = new List<string>();

            ShimCacheUtil.IsCacheEnabled = () => false;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) => new object();

            ShimCacheUtil.RemoveFromCacheStringString = (cacheKey, dataBaseName) =>
            {
                actualRemoveFromCache.Add(cacheKey);
            };

            // Act
            ResponseGroup.DeleteCache(ClientConnections, ProductId);
            
            // Assert
            actualRemoveFromCache.ShouldSatisfyAllConditions(
                () => actualRemoveFromCache.Count.ShouldBe(0));
        }

        [Test]
        public void GetAllByPubId_CacheNotEnabled_DataFetched()
        {
            // Arrange
            var actualNumberFetchedFromCache = 0;
            var actualProductId = 0;
            var listOfResponseGroups = new List<ResponseGroup>() { new ResponseGroup() { ResponseGroupName = DummyReferenceName } };

            ShimCacheUtil.IsCacheEnabled = () => false;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) =>
            {
                actualNumberFetchedFromCache++;
                return new object();
            };

            ShimResponseGroup.GetAllDataClientConnectionsInt32 = (connections, productId) =>
            {
                actualProductId = productId;
                return listOfResponseGroups;
            };

            // Act
            var responseGroups = ResponseGroup.GetAllByPubID(ClientConnections, ProductId);

            // Assert
            responseGroups.ShouldSatisfyAllConditions(
                () => actualNumberFetchedFromCache.ShouldBe(0),
                () => actualProductId.ShouldBe(ProductId),
                () => responseGroups.Count.ShouldBe(1),
                () => responseGroups.ShouldContain(x => x.ResponseGroupName == DummyReferenceName));
        }
        
        [Test]
        public void GetAllByPubId_CacheEnabledAndCacheHit_DataFetchedFromCache()
        {
            // Arrange
            var actualNumberFetchedFromCache = 0;
            var actualCacheKey = string.Empty;
            var actualRegionKey = string.Empty;
            var listOfResponseGroups = new List<ResponseGroup>() { new ResponseGroup() { ResponseGroupName = DummyReferenceName } };

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) =>
            {
                actualNumberFetchedFromCache++;
                actualCacheKey = cacheKey;
                actualRegionKey = regionKey;
                return listOfResponseGroups;
            };

            ShimResponseGroup.GetAllDataClientConnectionsInt32 = (connections, productId) => throw new InvalidOperationException("should not be called");

            // Act
            var responseGroups = ResponseGroup.GetAllByPubID(ClientConnections, ProductId);

            // Assert
            responseGroups.ShouldSatisfyAllConditions(
                () => actualNumberFetchedFromCache.ShouldBe(1),
                () => actualCacheKey.ShouldBe($"{CacheKeyAllResponseGroup}{ProductId}"),
                () => actualRegionKey.ShouldBe(DummyDatabaseName),
                () => responseGroups.Count.ShouldBe(1),
                () => responseGroups.ShouldContain(x => x.ResponseGroupName == DummyReferenceName));
        }  
        
        [Test]
        public void GetAllByPubId_CacheEnabledAndCacheMiss_DataFetchedAndStored()
        {
            // Arrange
            var actualNumberFetchedFromCache = 0;
            var actualNumberStoredInCache = 0;
            var actualCacheKey = string.Empty;
            var actualRegionKey = string.Empty;
            var listOfResponseGroups = new List<ResponseGroup>() { new ResponseGroup() { ResponseGroupName = DummyReferenceName } };
            List<ResponseGroup> actualValueToStore = null;

            ShimCacheUtil.IsCacheEnabled = () => true;
            ShimCacheUtil.GetFromCacheStringStringBoolean = (cacheKey, regionKey, _) =>
            {
                actualNumberFetchedFromCache++;
                return null;
            };

            ShimCacheUtil.AddToCacheStringObjectStringBoolean = (cacheKey, valueToStore, regionKey, _) =>
            {
                actualCacheKey = cacheKey;
                actualRegionKey = regionKey;
                actualValueToStore = valueToStore as List<ResponseGroup>;
                actualNumberStoredInCache++;
            };

            ShimResponseGroup.GetAllDataClientConnectionsInt32 = (connections, productId) => listOfResponseGroups;

            // Act
            var responseGroups = ResponseGroup.GetAllByPubID(ClientConnections, ProductId);

            // Assert
            responseGroups.ShouldSatisfyAllConditions(
                () => actualNumberFetchedFromCache.ShouldBe(1),
                () => actualNumberStoredInCache.ShouldBe(1),
                () => actualCacheKey.ShouldBe($"{CacheKeyAllResponseGroup}{ProductId}"),
                () => actualRegionKey.ShouldBe(DummyDatabaseName),
                () => actualValueToStore.Count.ShouldBe(1),
                () => actualValueToStore.ShouldContain(x => x.ResponseGroupName == DummyReferenceName),
                () => responseGroups.Count.ShouldBe(1),
                () => responseGroups.ShouldContain(x => x.ResponseGroupName == DummyReferenceName));
        }

        [Test]
        public void GetData_AllParameterSet_DataReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;
            ShimResponseGroup.CreateResponseListFromBuilderIDataReader = reader => ListResponseGroupTestData; 
            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return CreateShimDataReader(TestData.ToList());
            }; 

            // Act
            var responseGroups = CallGetDataMethod(ClientConnections, ProductId);

            // Assert
            responseGroups.ShouldSatisfyAllConditions(
                () => responseGroups.Count().ShouldBe(2),
                () => responseGroups.ShouldContain(responeGroup => responeGroup.ResponseGroupName == ResponseGroupName1),
                () => responseGroups.ShouldContain(responeGroup => responeGroup.ResponseGroupName == ResponseGroupName2),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualSqlCommand.CommandText.ShouldBe(GetDataSqlCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[ProductIdParameterName].Value.ShouldBe(ProductId));
        }

        [Test]
        public void GetAllData_AllParameterSet_DataReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;
            ShimResponseGroup.CreateResponseListFromBuilderIDataReader = reader => ListResponseGroupTestData; 
            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return CreateShimDataReader(TestData.ToList());
            }; 

            // Act
            var responseGroups = CallGetAllDataMethod(ClientConnections, ProductId);

            // Assert
            responseGroups.ShouldSatisfyAllConditions(
                () => responseGroups.Count().ShouldBe(2),
                () => responseGroups.ShouldContain(responeGroup => responeGroup.ResponseGroupName == ResponseGroupName1),
                () => responseGroups.ShouldContain(responeGroup => responeGroup.ResponseGroupName == ResponseGroupName2),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualSqlCommand.CommandText.ShouldBe(GetAllDataSqlCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.Text),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[ProductIdParameterName].Value.ShouldBe(ProductId));
        }

        private static SqlDataReader CreateShimDataReader(IList<IDictionary<string, string>> data)
        {
            var shimDataReader = new ShimSqlDataReader();
            var counter = -1;
            shimDataReader.Read = () => ++counter < data.Count;
            shimDataReader.ItemGetString = key => data[counter][key];

            return shimDataReader.Instance;
        }

        private static void AddCommonDbShims()
        {
            KPMSShimDataFunctions.GetDBNameClientConnections = connections => DummyDatabaseName;
            KPMSShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
            ShimSqlConnection.AllInstances.Open = _ => { }; 
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
        }

        private static string GetParameterName(string parameter)
        {
            return $"{parameter}{ParameterKeySuffix}";
        }

        private static IEnumerable<ResponseGroup> CallGetDataMethod(ClientConnections connections, int productId)
        {
            return (IList<ResponseGroup>)typeof(ResponseGroup).CallMethod(GetDataMethodName, new object[] { connections, productId });
        } 
        
        private static IEnumerable<ResponseGroup> CallGetAllDataMethod(ClientConnections connections, int productId)
        {
            return (IList<ResponseGroup>)typeof(ResponseGroup).CallMethod(GetAllDataMethodName, new object[] { connections, productId });
        }
    }
}