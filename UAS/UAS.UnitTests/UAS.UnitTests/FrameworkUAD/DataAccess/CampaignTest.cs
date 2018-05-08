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
    /// Unit tests for <see cref="Campaign"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignTest
    {
        private const string CommandText = "e_Campaign_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.Campaign _campaign;

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
            _campaign = typeof(Entity.Campaign).CreateInstance();

            // Act
            Campaign.Save(_campaign, new ClientConnections());

            // Assert
            _campaign.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _campaign = typeof(Entity.Campaign).CreateInstance(true);

            // Act
            Campaign.Save(_campaign, new ClientConnections());

            // Assert
            _campaign.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@CampaignID"].Value.ShouldBe(_campaign.CampaignID),
                () => _saveCommand.Parameters["@CampaignName"].Value.ShouldBe(_campaign.CampaignName),
                () => _saveCommand.Parameters["@AddedBy"].Value.ShouldBe(_campaign.AddedBy),
                () => _saveCommand.Parameters["@DateAdded"].Value.ShouldBe(_campaign.DateAdded),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_campaign.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@UpdatedBy"].Value.ShouldBe(_campaign.UpdatedBy),
                () => _saveCommand.Parameters["@BrandID"].Value.ShouldBe(_campaign.BrandID));
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