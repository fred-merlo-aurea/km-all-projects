using System;
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

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="SubscriberDemographicArchive"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicArchiveTest
    {
        private const string CommandText = "e_SubscriberDemographicArchive_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberDemographicArchive _archive;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
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
            _archive = typeof(Entity.SubscriberDemographicArchive).CreateInstance();

            // Act
            SubscriberDemographicArchive.Save(_archive, new ClientConnections());

            // Assert
            _archive.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _archive = typeof(Entity.SubscriberDemographicArchive).CreateInstance(true);

            // Act
            SubscriberDemographicArchive.Save(_archive, new ClientConnections());

            // Assert
            _archive.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SDArchiveID"].Value.ShouldBe(_archive.SDArchiveID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_archive.PubID),
                () => _saveCommand.Parameters["@SARecordIdentifier"].Value.ShouldBe(_archive.SARecordIdentifier),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe(_archive.MAFField),
                () => _saveCommand.Parameters["@Value"].Value.ShouldBe(_archive.Value),
                () => _saveCommand.Parameters["@NotExists"].Value.ShouldBe(_archive.NotExists),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_archive.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_archive.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_archive.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_archive.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DemographicUpdateCodeId"].Value.ShouldBe(_archive.DemographicUpdateCodeId),
                () => _saveCommand.Parameters["@IsAdhoc"].Value.ShouldBe(_archive.IsAdhoc),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_archive.ResponseOther));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return -1;
            };
        }
    }
}
