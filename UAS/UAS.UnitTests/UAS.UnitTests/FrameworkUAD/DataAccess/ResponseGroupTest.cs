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
    /// Unit tests for <see cref="ResponseGroup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ResponseGroupTest
    {
        private const string CommandText = "e_ResponseGroup_Save";
        private const string ConnectionString = "connection-string";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.ResponseGroup _responseGroup;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _dataTable = new DataTable();
            _dataSet = new DataSet();

            _objWithRandomValues = typeof(Entity.ResponseGroup).CreateInstance();
            _objWithDefaultValues = typeof(Entity.ResponseGroup).CreateInstance(true);

            _list = new List<Entity.ResponseGroup>
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
            _responseGroup = typeof(Entity.ResponseGroup).CreateInstance();

            // Act
            ResponseGroup.Save(_responseGroup, new ClientConnections());

            // Assert
            _responseGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _responseGroup = typeof(Entity.ResponseGroup).CreateInstance(true);

            // Act
            ResponseGroup.Save(_responseGroup, new ClientConnections());

            // Assert
            _responseGroup.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@ResponseGroupID"].Value.ShouldBe(_responseGroup.ResponseGroupID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_responseGroup.PubID),
                () => _saveCommand.Parameters["@ResponseGroupName"].Value.ShouldBe((object)_responseGroup.ResponseGroupName ?? DBNull.Value),
                () => _saveCommand.Parameters["@DisplayName"].Value.ShouldBe((object)_responseGroup.DisplayName ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_responseGroup.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_responseGroup.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_responseGroup.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_responseGroup.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DisplayOrder"].Value.ShouldBe((object)_responseGroup.DisplayOrder ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsMultipleValue"].Value.ShouldBe((object)_responseGroup.IsMultipleValue ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsRequired"].Value.ShouldBe((object)_responseGroup.IsRequired ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_responseGroup.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@WQT_ResponseGroupID"].Value.ShouldBe((object)_responseGroup.WQT_ResponseGroupID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ResponseGroupTypeId"].Value.ShouldBe(_responseGroup.ResponseGroupTypeId));
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