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
    /// Unit tests for <see cref="SubscriberDemographicInvalid"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicInvalidTest
    {
        private const string CommandText = "e_SubscriberDemographicInvalid_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberDemographicInvalid _subscriberDemographicInvalid;

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
            _subscriberDemographicInvalid = typeof(Entity.SubscriberDemographicInvalid).CreateInstance();

            // Act
            SubscriberDemographicInvalid.Save(_subscriberDemographicInvalid, new ClientConnections());

            // Assert
            _subscriberDemographicInvalid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriberDemographicInvalid = typeof(Entity.SubscriberDemographicInvalid).CreateInstance(true);

            // Act
            SubscriberDemographicInvalid.Save(_subscriberDemographicInvalid, new ClientConnections());

            // Assert
            _subscriberDemographicInvalid.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SDInvalidID"].Value.ShouldBe(_subscriberDemographicInvalid.SDInvalidID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_subscriberDemographicInvalid.PubID),
                () => _saveCommand.Parameters["@SIRecordIdentifier"].Value.ShouldBe(_subscriberDemographicInvalid.SIRecordIdentifier),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe(_subscriberDemographicInvalid.MAFField),
                () => _saveCommand.Parameters["@Value"].Value.ShouldBe(_subscriberDemographicInvalid.Value),
                () => _saveCommand.Parameters["@NotExists"].Value.ShouldBe(_subscriberDemographicInvalid.NotExists),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriberDemographicInvalid.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriberDemographicInvalid.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriberDemographicInvalid.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_subscriberDemographicInvalid.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DemographicUpdateCodeId"].Value.ShouldBe(_subscriberDemographicInvalid.DemographicUpdateCodeId),
                () => _saveCommand.Parameters["@IsAdhoc"].Value.ShouldBe(_subscriberDemographicInvalid.IsAdhoc),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_subscriberDemographicInvalid.ResponseOther));
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