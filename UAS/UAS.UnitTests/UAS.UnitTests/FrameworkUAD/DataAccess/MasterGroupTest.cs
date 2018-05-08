using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="MasterGroup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class MasterGroupTest
    {
        private const string CommandText = "e_MasterGroup_Save";
        private const string ConnectionString = "connection-string";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.MasterGroup _masterGroup;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _dataTable = new DataTable();
            _dataSet = new DataSet();
            _objWithRandomValues = typeof(Entity.MasterGroup).CreateInstance();
            _objWithDefaultValues = typeof(Entity.MasterGroup).CreateInstance(true);

            _list = new List<Entity.MasterGroup>
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
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            _masterGroup = typeof(Entity.MasterGroup).CreateInstance();

            // Act
            MasterGroup.Save(_masterGroup, new ClientConnections());

            // Assert
            _masterGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _masterGroup = typeof(Entity.MasterGroup).CreateInstance(true);

            // Act
            MasterGroup.Save(_masterGroup, new ClientConnections());

            // Assert
            _masterGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@MasterGroupID"].Value.ShouldBe(_masterGroup.MasterGroupID),
                () => _saveCommand.Parameters["@DisplayName"].Value.ShouldBe(_masterGroup.DisplayName),
                () => _saveCommand.Parameters["@Name"].Value.ShouldBe(_masterGroup.Name),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_masterGroup.IsActive),
                () => _saveCommand.Parameters["@EnableSubReporting"].Value.ShouldBe(_masterGroup.EnableSubReporting),
                () => _saveCommand.Parameters["@EnableSearching"].Value.ShouldBe(_masterGroup.EnableSearching),
                () => _saveCommand.Parameters["@EnableAdhocSearch"].Value.ShouldBe(_masterGroup.EnableAdhocSearch),
                () => _saveCommand.Parameters["@SortOrder"].Value.ShouldBe(_masterGroup.SortOrder),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_masterGroup.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_masterGroup.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_masterGroup.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_masterGroup.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection
            {
                ConnectionStringGet = () => ConnectionString
            }.Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.GetDataSetSqlCommandString = (cmd, _) =>
            {
                _sqlCommand = cmd;
                return _dataSet;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}