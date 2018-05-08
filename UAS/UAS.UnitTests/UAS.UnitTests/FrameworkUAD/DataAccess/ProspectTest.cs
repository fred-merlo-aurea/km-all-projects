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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="Prospect"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProspectTest
    {
        private const string CommandText = "e_Prospect_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Prospect _prospect;

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
            _prospect = typeof(Entity.Prospect).CreateInstance();

            // Act
            Prospect.Save(_prospect, new ClientConnections());

            // Assert
            _prospect.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _prospect = typeof(Entity.Prospect).CreateInstance(true);

            // Act
            Prospect.Save(_prospect, new ClientConnections());

            // Assert
            _prospect.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@PublicationID"].Value.ShouldBe(_prospect.PublicationID),
                () => _saveCommand.Parameters["@SubscriberID"].Value.ShouldBe(_prospect.SubscriberID),
                () => _saveCommand.Parameters["@IsProspect"].Value.ShouldBe(_prospect.IsProspect),
                () => _saveCommand.Parameters["@IsVerifiedProspect"].Value.ShouldBe(_prospect.IsVerifiedProspect),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_prospect.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_prospect.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_prospect.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_prospect.UpdatedByUserID ?? DBNull.Value));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _saveCommand = cmd;
                return true;
            };
        }
    }
}