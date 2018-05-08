using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Linq;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using UADShimDataFunctions = FrameworkUAD.DataAccess.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
{
    public class PubsTest
    {
        private const int BrandId = 24;
        private const int PubsId = 42;
        private const int ExecuteResult = 12345;
        private const string PubsDeleteCommandText = "sp_Pubs_Delete";
        private const string PubsIdKey = "@PubID";
        private const string TestDbString = "TestDb";
        private const string LiveDbString = "LiveDb";
        private const string PubsValidateDeleteOrInactive = "e_Product_Validate_DeleteorInActive";
        private const string ReferenceKey = "Reference";
        private const string ReferenceNameKey = "ReferenceName";
        private const string DummyReference = "Dummy";
        private const string DummyReferenceName = "DummyName";
        private const string ParameterKeySuffix = " : ";

        private static ClientConnections clientConnections;

        private IDisposable _context;

        private static ClientConnections ClientConnections => clientConnections ?? (clientConnections = new ClientConnections(TestDbString, LiveDbString));

        private static IEnumerable<IDictionary<string, string>> TestData
        {
            get
            {
                yield return new Dictionary<string, string>()
                {
                    [ReferenceKey] = DummyReference,
                    [ReferenceNameKey] = DummyReferenceName,
                };
            }
        }

        private static IEnumerable<Pubs> TestPubs
        {
            get
            {
                yield return new Pubs() { IsActive = true, EnableSearching = true, SortOrder = 0 };
                yield return new Pubs() { IsActive = true, EnableSearching = false, SortOrder = 1 };
                yield return new Pubs() { IsActive = false, EnableSearching = true,  SortOrder = 2 };
                yield return new Pubs() { IsActive = false, EnableSearching = false,  SortOrder = 3 }; 
                yield return new Pubs() { IsActive = true, EnableSearching = true, SortOrder = 4 };
                yield return new Pubs() { IsActive = true, EnableSearching = false, SortOrder = 5 };
                yield return new Pubs() { IsActive = false, EnableSearching = true,  SortOrder = 6 };
                yield return new Pubs() { IsActive = false, EnableSearching = false,  SortOrder = 7 };
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

            ShimPubs.DeleteCacheClientConnections = clientConnectionsParameter =>
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
            Pubs.Delete(ClientConnections, PubsId);

            // Assert
            actualSqlCommand.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(PubsDeleteCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[PubsIdKey].Value.ShouldBe(PubsId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => numberOfTimeClearCachedCalled.ShouldBe(1),
                () => actualClientConnections.ShouldBe(ClientConnections));
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
            var result = Pubs.ValidationForDeleteorInActive(ClientConnections, PubsId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => actualSqlCommand.CommandText.ShouldBe(PubsValidateDeleteOrInactive),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.Parameters.Count.ShouldBe(1),
                () => actualSqlCommand.Parameters[PubsIdKey].Value.ShouldBe(PubsId),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => result.Count.ShouldBe(1),
                () => result[GetParameterName(DummyReference)].ShouldBe(DummyReferenceName));
        }

        [Test]
        public void GetSearchEnabled_EntriesFound_SortedListReturned()
        {
            // Arrange
            AddPubsGetAllShim();

            // Act
            var result = Pubs.GetSearchEnabled(ClientConnections);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(2),
                () => result[0].SortOrder.ShouldBe(0),
                () => result[1].SortOrder.ShouldBe(4));
        }

        [Test]
        public void GetActive_EntriesFound_SortedListReturned()
        {
            // Arrange
            AddPubsGetAllShim();

            // Act
            var result = Pubs.GetActive(ClientConnections);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(4),
                () => result[0].SortOrder.ShouldBe(0),
                () => result[1].SortOrder.ShouldBe(1),
                () => result[2].SortOrder.ShouldBe(4),
                () => result[3].SortOrder.ShouldBe(5));
        }

        [Test]
        public void GetSearchEnabledByBrandId_BrandIdFound_SortedListReturned()
        {
            // Arrange
            AddPubsGetAllShim();

            // Act
            var result = Pubs.GetSearchEnabledByBrandID(ClientConnections, BrandId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(2),
                () => result[0].SortOrder.ShouldBe(0),
                () => result[1].SortOrder.ShouldBe(4));
        }

        [Test]
        public void GetActiveByBrandId_BrandIdFound_SortedListReturned()
        {
            // Arrange
            AddPubsGetAllShim();

            // Act
            var result = Pubs.GetActiveByBrandID(ClientConnections, BrandId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.Count.ShouldBe(4),
                () => result[0].SortOrder.ShouldBe(0),
                () => result[1].SortOrder.ShouldBe(1),
                () => result[2].SortOrder.ShouldBe(4),
                () => result[3].SortOrder.ShouldBe(5));
        }

        private static SqlDataReader CreateShimDataReader(IList<IDictionary<string, string>> data)
        {
            var shimDataReader = new ShimSqlDataReader();
            var counter = -1;
            shimDataReader.Read = () => ++counter < data.Count;
            shimDataReader.ItemGetString = key => data[counter][key];

            return shimDataReader.Instance;
        }

        private static void AddPubsGetAllShim()
        {
            ShimPubs.GetAllClientConnections = connections => TestPubs.ToList();
            ShimPubs.GetByBrandIDClientConnectionsInt32 = (connections, brandId) => TestPubs.ToList();
        }

        private static void AddCommonDbShims()
        {
            ShimSqlConnection.AllInstances.Open = _ => { };
            UADShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
        }

        private static string GetParameterName(string parameter)
        {
            return $"{parameter}{ParameterKeySuffix}";
        }
    }
}