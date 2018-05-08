using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using KMPlatform.Object;
using KMPS.MD.Objects.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KPMSShimDataFunctions = KMPS.MD.Objects.Fakes.ShimDataFunctions;

namespace KMPS.MD.Objects.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CrossTabTest
    {
        private const string Queries = "MyQueriesParameterTestValue";
        private const string Column = "MyColumnValue";
        private const string Row = "MyRowValue";
        private const int BrandId = 42;
        private const int ProductId = 24;
        private const bool IsRecencyView = false;
        private const string DummyDatabaseName = "MyTestDatabase";
        private const string ColDesc1 = "ColDesc1";
        private const string ColDesc2 = "ColDesc2";
        private const string GetMasterCrossTabDataCommandText = "sp_GetCrossTabConsensusData";
        private const string QueriesParameterName = "@Queries";
        private const string RowParameterName = "@Row";
        private const string ColumnParameterName = "@Column";
        private const string BrandIdParameterName = "@BrandID";
        private const string IsRecencyViewParameterName = "@IsRecencyView";
        private const string GetProductCrossTabDataCommandText = "sp_GetCrossTabProductData";
        private const string ProductIdParameterName = "@PubID";

        private IDisposable _context;

        private static IList<CrossTab> ListCrossTabsTestData => new List<CrossTab>()
        {
            new CrossTab() { ColDesc = ColDesc1},
            new CrossTab() { ColDesc = ColDesc2}
        };

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
        public void GetMasterCrossTabData_ParameterSet_ListReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return new ShimSqlDataReader().Instance;
            };

            ShimUtilities.CreateListFromBuilderOf1SqlDataReader(reader => ListCrossTabsTestData);

            // Act
            var crossTabs = CrossTab.GetMasterCrossTabData(
                new ClientConnections(),
                new StringBuilder(Queries),
                Column,
                Row,
                BrandId,
                IsRecencyView);

            // Assert
            crossTabs.ShouldSatisfyAllConditions(
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandText.ShouldBe(GetMasterCrossTabDataCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualSqlCommand.Parameters.Count.ShouldBe(5),
                () => actualSqlCommand.Parameters[QueriesParameterName].Value.ShouldBe(Queries),
                () => actualSqlCommand.Parameters[QueriesParameterName].SqlDbType.ShouldBe(SqlDbType.Text),
                () => actualSqlCommand.Parameters[RowParameterName].Value.ShouldBe(Row),
                () => actualSqlCommand.Parameters[ColumnParameterName].Value.ShouldBe(Column),
                () => actualSqlCommand.Parameters[BrandIdParameterName].Value.ShouldBe(BrandId),
                () => actualSqlCommand.Parameters[IsRecencyViewParameterName].Value.ShouldBe(IsRecencyView));
        }
        
        [Test]
        public void GetProductCrossTabData_ParameterSet_ListReturned()
        {
            // Arrange
            SqlCommand actualSqlCommand = null;
            var numberOfTimesExecuteCalled = 0;

            ShimSqlCommand.AllInstances.ExecuteReader = command =>
            {
                actualSqlCommand = command;
                numberOfTimesExecuteCalled++;
                return new ShimSqlDataReader().Instance;
            };

            ShimUtilities.CreateListFromBuilderOf1SqlDataReader(reader => ListCrossTabsTestData);

            // Act
            var crossTabs = CrossTab.GetProductCrossTabData(
                new ClientConnections(),
                new StringBuilder(Queries),
                Column,
                Row,
                ProductId);

            // Assert
            crossTabs.ShouldSatisfyAllConditions(
                () => numberOfTimesExecuteCalled.ShouldBe(1),
                () => actualSqlCommand.CommandText.ShouldBe(GetProductCrossTabDataCommandText),
                () => actualSqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => actualSqlCommand.CommandTimeout.ShouldBe(0),
                () => actualSqlCommand.Parameters.Count.ShouldBe(4),
                () => actualSqlCommand.Parameters[QueriesParameterName].Value.ShouldBe(Queries),
                () => actualSqlCommand.Parameters[QueriesParameterName].SqlDbType.ShouldBe(SqlDbType.Text),
                () => actualSqlCommand.Parameters[RowParameterName].Value.ShouldBe(Row),
                () => actualSqlCommand.Parameters[ColumnParameterName].Value.ShouldBe(Column),
                () => actualSqlCommand.Parameters[ProductIdParameterName].Value.ShouldBe(ProductId));
        }

        private static void AddCommonDbShims()
        {
            KPMSShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
            KPMSShimDataFunctions.GetDBNameClientConnections = connections => DummyDatabaseName;
            ShimSqlConnection.AllInstances.Open = _ => { }; 
        }
    }
}
