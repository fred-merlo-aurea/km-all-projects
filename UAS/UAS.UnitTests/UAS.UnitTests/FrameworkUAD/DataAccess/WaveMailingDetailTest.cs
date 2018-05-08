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
    /// Unit tests for <see cref="WaveMailingDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WaveMailingDetailTest
    {
        private const string CommandText = "e_WaveMailingDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.WaveMailingDetail _waveMailingDetail;

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
            _waveMailingDetail = typeof(Entity.WaveMailingDetail).CreateInstance();

            // Act
            WaveMailingDetail.Save(_waveMailingDetail, new ClientConnections());

            // Assert
            _waveMailingDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _waveMailingDetail = typeof(Entity.WaveMailingDetail).CreateInstance(true);

            // Act
            WaveMailingDetail.Save(_waveMailingDetail, new ClientConnections());

            // Assert
            _waveMailingDetail.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@WaveMailingDetailID"].Value.ShouldBe(_waveMailingDetail.WaveMailingDetailID),
                () => _saveCommand.Parameters["@WaveMailingID"].Value.ShouldBe(_waveMailingDetail.WaveMailingID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_waveMailingDetail.PubSubscriptionID),
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_waveMailingDetail.SubscriptionID),
                () => _saveCommand.Parameters["@Demo7"].Value.ShouldBe(_waveMailingDetail.Demo7),
                () => _saveCommand.Parameters["@PubCategoryID"].Value.ShouldBe(_waveMailingDetail.PubCategoryID),
                () => _saveCommand.Parameters["@PubTransactionID"].Value.ShouldBe(_waveMailingDetail.PubTransactionID),
                () => _saveCommand.Parameters["@IsSubscribed"].Value.ShouldBe((object)_waveMailingDetail.IsSubscribed ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsPaid"].Value.ShouldBe((object)_waveMailingDetail.IsPaid ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_waveMailingDetail.SubscriptionStatusID),
                () => _saveCommand.Parameters["@Copies"].Value.ShouldBe((object)_waveMailingDetail.Copies ?? DBNull.Value),
                () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe((object)_waveMailingDetail.FirstName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LastName"].Value.ShouldBe((object)_waveMailingDetail.LastName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_waveMailingDetail.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_waveMailingDetail.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressTypeID"].Value.ShouldBe((object)_waveMailingDetail.AddressTypeID ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address1"].Value.ShouldBe((object)_waveMailingDetail.Address1 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address2"].Value.ShouldBe((object)_waveMailingDetail.Address2 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_waveMailingDetail.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_waveMailingDetail.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionCode"].Value.ShouldBe((object)_waveMailingDetail.RegionCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionID"].Value.ShouldBe((object)_waveMailingDetail.RegionID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ZipCode"].Value.ShouldBe((object)_waveMailingDetail.ZipCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_waveMailingDetail.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_waveMailingDetail.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_waveMailingDetail.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe((object)_waveMailingDetail.CountryID ?? DBNull.Value),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_waveMailingDetail.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_waveMailingDetail.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhoneExt"].Value.ShouldBe((object)_waveMailingDetail.PhoneExt ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe((object)_waveMailingDetail.Fax ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_waveMailingDetail.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_waveMailingDetail.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_waveMailingDetail.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_waveMailingDetail.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_waveMailingDetail.UpdatedByUserID ?? DBNull.Value));
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