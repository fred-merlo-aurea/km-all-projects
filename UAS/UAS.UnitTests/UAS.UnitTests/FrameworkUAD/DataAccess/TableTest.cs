using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="Table"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TableTest
    {
        private const int Rows = 5;
        private const string DbName = "db-name";
        private const string TableParameter = "table";
        private const string PubCode = "pub-code";

        private const string ProcForSelectWithDbName = "SELECT TABLE_NAME as 'TableName' FROM INFORMATION_SCHEMA.TABLES With(NoLock) WHERE TABLE_TYPE = 'BASE TABLE'";
        private const string ProcSelectWithClient = "e_Table_Select";
        private const string ProcSelect = "e_UADTable_ExportData";
        private const string GetMethod = "Get";

        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.Table> _list;
        private Entity.Table _objWithRandomValues;
        private Entity.Table _objWithDefaultValues;
        private SqlCommand _sqlCommand;
        private string _script;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.Table).CreateInstance();
            _objWithDefaultValues = typeof(Entity.Table).CreateInstance(true);

            _list = new List<Entity.Table>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Select_WhenCalledWithDBName_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Table.Select(DbName);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcForSelectWithDbName),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.Text));
        }

        [Test]
        public void Select_WhenCalledWithClient_VerifyReturnItem()
        {
            // Arrange, Act
            var result = Table.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectWithClient),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalled_VerifyReturnItem()
        {
            // Arrange
            var script = "DECLARE @Table varchar(100) = '" + TableParameter + "', @PubCode varchar(50) = '" + PubCode + "' " +
                            @"IF EXISTS(SELECT * FROM sys.columns 
                                    WHERE (UPPER([name]) = 'PUBCODE' OR UPPER([name]) = 'PUBID') AND [object_id] = OBJECT_ID(@Table))
                            BEGIN    
                            IF (@PubCode != '')
	                            BEGIN
		                            DECLARE @PubID int = (SELECT PubID FROM Pubs With(NoLock) WHERE PubCode = @PubCode)
		                            IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubID%')
		                            BEGIN
			                            EXEC('SELECT * FROM ' + @Table + ' WHERE PubID = ' + @PubID);
		                            END
		                            ELSE
		                            BEGIN
			                            IF EXISTS (SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS With(NoLock) WHERE TABLE_NAME = @Table and COLUMN_NAME like '%PubCode%')
			                            BEGIN
				                            EXEC('SELECT * FROM ' + @Table + ' WHERE PubCode = ' + @PubCode);
			                            END
			                            ELSE
			                            BEGIN
				                            EXEC('SELECT * FROM ' + @Table);
			                            END
		                            END
	                            END
	                            ELSE
	                            BEGIN
		                            EXEC('SELECT * FROM ' + @Table);
	                            END
                            END
                            ELSE
                            BEGIN
	                            BEGIN
		                            EXEC('SELECT * FROM ' + @Table);
	                            END
                            END";

            // Act
            var result = Table.Select(DbName, TableParameter, PubCode);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(_dataTable),
                () => _script.ShouldBe(script, StringCompareShould.IgnoreLineEndings));
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = Table.Select(Client, DbName, TableParameter, PubCode);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@Table"].Value.ShouldBe(TableParameter),
                () => _sqlCommand.Parameters["@PubCode"].Value.ShouldBe(PubCode),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(Table).CallMethod(GetMethod, new object[] { new SqlCommand(), DbName });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            ShimSqlConnection.ConstructorString = (_, __) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;

            KMFakes.ShimDataFunctions.GetDataTableStringSqlConnection = (script, conn) =>
            {
                _script = script;
                return _dataTable;
            };

            SetupDataReaderFakes();
            SetupKmFakes();
        }

        private void SetupDataReaderFakes()
        {
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };

            KMFakes.ShimDataFunctions.ExecuteReaderSqlCommandStringString = (cmd, conn, dbName) =>
            {
                _sqlCommand = cmd;

                conn.ShouldSatisfyAllConditions(
                    () => conn.ShouldBe(DataFunctions.ConnectionString.UAD_Master.ToString()),
                    () => dbName.ShouldBe(DbName));

                return _list.GetSqlDataReader();
            };
        }

        private void SetupKmFakes()
        {
            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            KMFakes.ShimDataFunctions.GetSqlConnectionString = conn =>
            {
                var connectionString = DataFunctions.ConnectionString.UAD_Master.ToString();
                conn.ShouldBe(connectionString);
                return new ShimSqlConnection
                {
                    ConnectionStringGet = () => connectionString
                }.Instance;
            };
        }
    }
}