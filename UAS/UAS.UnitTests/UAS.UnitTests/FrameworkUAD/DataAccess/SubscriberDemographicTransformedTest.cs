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
    /// Unit tests for <see cref="SubscriberDemographicTransformed"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicTransformedTest
    {
        private const string CommandText = "e_SubscriberDemographicTransformed_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberDemographicTransformed _subscriberDemographicTransformed;

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
            _subscriberDemographicTransformed = typeof(Entity.SubscriberDemographicTransformed).CreateInstance();

            // Act
            SubscriberDemographicTransformed.Save(_subscriberDemographicTransformed, new ClientConnections());

            // Assert
            _subscriberDemographicTransformed.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriberDemographicTransformed = typeof(Entity.SubscriberDemographicTransformed).CreateInstance(true);

            // Act
            SubscriberDemographicTransformed.Save(_subscriberDemographicTransformed, new ClientConnections());

            // Assert
            _subscriberDemographicTransformed.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriberDemographicTransformedID"].Value.ShouldBe(_subscriberDemographicTransformed.SubscriberDemographicTransformedID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_subscriberDemographicTransformed.PubID),
                () => _saveCommand.Parameters["@SORecordIdentifier"].Value.ShouldBe(_subscriberDemographicTransformed.SORecordIdentifier),
                () => _saveCommand.Parameters["@STRecordIdentifier"].Value.ShouldBe(_subscriberDemographicTransformed.STRecordIdentifier),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe(_subscriberDemographicTransformed.MAFField),
                () => _saveCommand.Parameters["@Value"].Value.ShouldBe(_subscriberDemographicTransformed.Value),
                () => _saveCommand.Parameters["@NotExists"].Value.ShouldBe(_subscriberDemographicTransformed.NotExists),
                () => _saveCommand.Parameters["@NotExistReason"].Value.ShouldBe(_subscriberDemographicTransformed.NotExistReason),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriberDemographicTransformed.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriberDemographicTransformed.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriberDemographicTransformed.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_subscriberDemographicTransformed.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DemographicUpdateCodeId"].Value.ShouldBe(_subscriberDemographicTransformed.DemographicUpdateCodeId),
                () => _saveCommand.Parameters["@IsAdhoc"].Value.ShouldBe(_subscriberDemographicTransformed.IsAdhoc),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_subscriberDemographicTransformed.ResponseOther),
                () => _saveCommand.Parameters["@IsDemoDate"].Value.ShouldBe(_subscriberDemographicTransformed.IsDemoDate));
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