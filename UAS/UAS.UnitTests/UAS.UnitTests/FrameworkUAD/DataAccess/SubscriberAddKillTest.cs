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
    /// Unit tests for <see cref="SubscriberAddKill"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriberAddKillTest
    {
        private const string CommandText = "e_SubscriberAddKill_Save";
        private const string DataBase = "data-base";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberAddKill _subscriberAddKill;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();
            _bulkCopyColumns = new Dictionary<string, string>();
            _bulkCopyClosed = false;

            _objWithRandomValues = typeof(Entity.SubscriberAddKill).CreateInstance();
            _objWithDefaultValues = typeof(Entity.SubscriberAddKill).CreateInstance(true);

            _list = new List<Entity.SubscriberAddKill>
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
            _subscriberAddKill = typeof(Entity.SubscriberAddKill).CreateInstance();

            // Act
            SubscriberAddKill.Save(_subscriberAddKill, new ClientConnections());

            // Assert
            _subscriberAddKill.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriberAddKill = typeof(Entity.SubscriberAddKill).CreateInstance(true);

            // Act
            SubscriberAddKill.Save(_subscriberAddKill, new ClientConnections());

            // Assert
            _subscriberAddKill.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@AddKillID"].Value.ShouldBe(_subscriberAddKill.AddKillID),
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_subscriberAddKill.PublicationID),
                () => _saveCommand.Parameters["@Count"].Value.ShouldBe(_subscriberAddKill.Count),
                () => _saveCommand.Parameters["@AddKillCount"].Value.ShouldBe(_subscriberAddKill.AddKillCount),
                () => _saveCommand.Parameters["@Type"].Value.ShouldBe(_subscriberAddKill.Type),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_subscriberAddKill.IsActive),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriberAddKill.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriberAddKill.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriberAddKill.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe(_subscriberAddKill.UpdatedByUserID));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                _sqlCommand = cmd;
                return Rows;
            };

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection
            {
                DatabaseGet = () => DataBase
            }.Instance;
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

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}