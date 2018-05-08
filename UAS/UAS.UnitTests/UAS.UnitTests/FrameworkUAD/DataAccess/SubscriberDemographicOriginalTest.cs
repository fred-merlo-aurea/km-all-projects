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
    /// Unit tests for <see cref="SubscriberDemographicOriginal"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicOriginalTest
    {
        private const string CommandText = "e_SubscriberDemographicOriginal_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberDemographicOriginal _subscriberDemographicOriginal;

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
            _subscriberDemographicOriginal = typeof(Entity.SubscriberDemographicOriginal).CreateInstance();

            // Act
            SubscriberDemographicOriginal.Save(_subscriberDemographicOriginal, new ClientConnections());

            // Assert
            _subscriberDemographicOriginal.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriberDemographicOriginal = typeof(Entity.SubscriberDemographicOriginal).CreateInstance(true);

            // Act
            SubscriberDemographicOriginal.Save(_subscriberDemographicOriginal, new ClientConnections());

            // Assert
            _subscriberDemographicOriginal.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SDOriginalID"].Value.ShouldBe(_subscriberDemographicOriginal.SDOriginalID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_subscriberDemographicOriginal.PubID),
                () => _saveCommand.Parameters["@SORecordIdentifier"].Value.ShouldBe(_subscriberDemographicOriginal.SORecordIdentifier),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe(_subscriberDemographicOriginal.MAFField),
                () => _saveCommand.Parameters["@Value"].Value.ShouldBe(_subscriberDemographicOriginal.Value),
                () => _saveCommand.Parameters["@NotExists"].Value.ShouldBe(_subscriberDemographicOriginal.NotExists),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriberDemographicOriginal.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriberDemographicOriginal.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriberDemographicOriginal.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_subscriberDemographicOriginal.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DemographicUpdateCodeId"].Value.ShouldBe(_subscriberDemographicOriginal.DemographicUpdateCodeId),
                () => _saveCommand.Parameters["@IsAdhoc"].Value.ShouldBe(_subscriberDemographicOriginal.IsAdhoc),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_subscriberDemographicOriginal.ResponseOther));
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