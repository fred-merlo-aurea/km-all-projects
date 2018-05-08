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
    /// Unit tests for <see cref="SubscriberDemographicFinal"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberDemographicFinalTest
    {
        private const string CommandText = "e_SubscriberDemographicFinal_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.SubscriberDemographicFinal _subscriberDemographicFinal;

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
            _subscriberDemographicFinal = typeof(Entity.SubscriberDemographicFinal).CreateInstance();

            // Act
            SubscriberDemographicFinal.Save(_subscriberDemographicFinal, new ClientConnections());

            // Assert
            _subscriberDemographicFinal.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _subscriberDemographicFinal = typeof(Entity.SubscriberDemographicFinal).CreateInstance(true);

            // Act
            SubscriberDemographicFinal.Save(_subscriberDemographicFinal, new ClientConnections());

            // Assert
            _subscriberDemographicFinal.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SDFinalID"].Value.ShouldBe(_subscriberDemographicFinal.SDFinalID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_subscriberDemographicFinal.PubID),
                () => _saveCommand.Parameters["@SFRecordIdentifier"].Value.ShouldBe(_subscriberDemographicFinal.SFRecordIdentifier),
                () => _saveCommand.Parameters["@MAFField"].Value.ShouldBe(_subscriberDemographicFinal.MAFField),
                () => _saveCommand.Parameters["@Value"].Value.ShouldBe(_subscriberDemographicFinal.Value),
                () => _saveCommand.Parameters["@NotExists"].Value.ShouldBe(_subscriberDemographicFinal.NotExists),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_subscriberDemographicFinal.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_subscriberDemographicFinal.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_subscriberDemographicFinal.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_subscriberDemographicFinal.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@DemographicUpdateCodeId"].Value.ShouldBe(_subscriberDemographicFinal.DemographicUpdateCodeId),
                () => _saveCommand.Parameters["@IsAdhoc"].Value.ShouldBe(_subscriberDemographicFinal.IsAdhoc),
                () => _saveCommand.Parameters["@ResponseOther"].Value.ShouldBe(_subscriberDemographicFinal.ResponseOther),
                () => _saveCommand.Parameters["@IsDemoDate"].Value.ShouldBe(_subscriberDemographicFinal.IsDemoDate));
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