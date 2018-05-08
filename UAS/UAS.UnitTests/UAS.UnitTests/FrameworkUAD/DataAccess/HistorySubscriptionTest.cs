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
    /// Unit tests for <see cref="HistorySubscription"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistorySubscriptionTest
    {
        private const string CommandText = "e_HistorySubscription_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.HistorySubscription _history;

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
            _history = typeof(Entity.HistorySubscription).CreateInstance();

            // Act
            HistorySubscription.Save(_history, new ClientConnections());

            // Assert
            _history.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _history = typeof(Entity.HistorySubscription).CreateInstance(true);

            // Act
            HistorySubscription.Save(_history, new ClientConnections());

            // Assert
            _history.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
          _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@HistorySubscriptionID"].Value.ShouldBe(_history.HistorySubscriptionID),
                () => _saveCommand.Parameters["@PubSubscriptionID"].Value.ShouldBe(_history.PubSubscriptionID),
                () => _saveCommand.Parameters["@SubscriptionID"].Value.ShouldBe(_history.SubscriptionID),
                () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_history.PubID),
                () => _saveCommand.Parameters["@demo7"].Value.ShouldBe((object)_history.Demo7 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Qualificationdate"].Value.ShouldBe((object)_history.QualificationDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@PubQSourceID"].Value.ShouldBe(_history.PubQSourceID),
                () => _saveCommand.Parameters["@PubCategoryID"].Value.ShouldBe(_history.PubCategoryID),
                () => _saveCommand.Parameters["@PubTransactionID"].Value.ShouldBe(_history.PubTransactionID),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_history.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailStatusID"].Value.ShouldBe(_history.EmailStatusID),
                () => _saveCommand.Parameters["@StatusUpdatedDate"].Value.ShouldBe(_history.StatusUpdatedDate),
                () => _saveCommand.Parameters["@StatusUpdatedReason"].Value.ShouldBe((object)_history.StatusUpdatedReason ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsComp"].Value.ShouldBe((object)_history.IsComp ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_history.SubscriptionStatusID),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_history.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_history.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_history.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_history.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ExternalKeyID"].Value.ShouldBe(_history.ExternalKeyID),
                () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe((object)_history.FirstName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LastName"].Value.ShouldBe((object)_history.LastName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_history.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_history.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe((object)_history.Occupation ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressTypeID"].Value.ShouldBe(_history.AddressTypeID),
                () => _saveCommand.Parameters["@Address1"].Value.ShouldBe((object)_history.Address1 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address2"].Value.ShouldBe((object)_history.Address2 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_history.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_history.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionCode"].Value.ShouldBe((object)_history.RegionCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@RegionID"].Value.ShouldBe(_history.RegionID),
                () => _saveCommand.Parameters["@ZipCode"].Value.ShouldBe((object)_history.ZipCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_history.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@CarrierRoute"].Value.ShouldBe((object)_history.CarrierRoute ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_history.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_history.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_history.CountryID),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_history.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_history.Longitude),
                () => _saveCommand.Parameters["@AddressValidationDate"].Value.ShouldBe((object)_history.AddressValidationDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressValidationSource"].Value.ShouldBe((object)_history.AddressValidationSource ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressValidationMessage"].Value.ShouldBe((object)_history.AddressValidationMessage ?? DBNull.Value),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_history.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe((object)_history.Fax ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_history.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object)_history.Website ?? DBNull.Value),
                () => _saveCommand.Parameters["@Birthdate"].Value.ShouldBe(_history.Birthdate),
                () => _saveCommand.Parameters["@Age"].Value.ShouldBe(_history.Age),
                () => _saveCommand.Parameters["@Income"].Value.ShouldBe((object)_history.Income ?? DBNull.Value),
                () => _saveCommand.Parameters["@Gender"].Value.ShouldBe((object)_history.Gender ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhoneExt"].Value.ShouldBe((object)_history.PhoneExt ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsInActiveWaveMailing"].Value.ShouldBe(_history.IsInActiveWaveMailing),
                () => _saveCommand.Parameters["@WaveMailingID"].Value.ShouldBe(_history.WaveMailingID),
                () => _saveCommand.Parameters["@AddressTypeCodeId"].Value.ShouldBe(_history.AddressTypeCodeId),
                () => _saveCommand.Parameters["@AddressLastUpdatedDate"].Value.ShouldBe((object)_history.AddressLastUpdatedDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@AddressUpdatedSourceTypeCodeId"].Value.ShouldBe(_history.AddressUpdatedSourceTypeCodeId),
                () => _saveCommand.Parameters["@IGrp_No"].Value.ShouldBe((object)_history.IGrp_No ?? DBNull.Value),
                () => _saveCommand.Parameters["@SFRecordIdentifier"].Value.ShouldBe((object)_history.SFRecordIdentifier ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubSrcID"].Value.ShouldBe(_history.SubSrcID),
                () => _saveCommand.Parameters["@Par3CID"].Value.ShouldBe(_history.Par3CID),
                () => _saveCommand.Parameters["@SequenceID"].Value.ShouldBe(_history.SequenceID),
                () => _saveCommand.Parameters["@AddRemoveID"].Value.ShouldBe(_history.AddRemoveID),
                () => _saveCommand.Parameters["@Copies"].Value.ShouldBe(_history.Copies),
                () => _saveCommand.Parameters["@GraceIssues"].Value.ShouldBe(_history.GraceIssues),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe(_history.IsActive),
                () => _saveCommand.Parameters["@IsAddressValidated"].Value.ShouldBe(_history.IsAddressValidated),
                () => _saveCommand.Parameters["@IsPaid"].Value.ShouldBe(_history.IsPaid),
                () => _saveCommand.Parameters["@IsSubscribed"].Value.ShouldBe(_history.IsSubscribed),
                () => _saveCommand.Parameters["@MemberGroup"].Value.ShouldBe((object)_history.MemberGroup ?? DBNull.Value),
                () => _saveCommand.Parameters["@OnBehalfOf"].Value.ShouldBe((object)_history.OnBehalfOf ?? DBNull.Value),
                () => _saveCommand.Parameters["@OrigsSrc"].Value.ShouldBe((object)_history.OrigsSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@Status"].Value.ShouldBe((object)_history.Status ?? DBNull.Value),
                () => _saveCommand.Parameters["@Verified"].Value.ShouldBe((object)_history.Verified ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriberSourceCode"].Value.ShouldBe((object)_history.SubscriberSourceCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubGenSubscriberID"].Value.ShouldBe(_history.SubGenSubscriberID),
                () => _saveCommand.Parameters["@MailPermission"].Value.ShouldBe((object)_history.MailPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@FaxPermission"].Value.ShouldBe((object)_history.FaxPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhonePermission"].Value.ShouldBe((object)_history.PhonePermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@OtherProductsPermission"].Value.ShouldBe((object)_history.OtherProductsPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailRenewPermission"].Value.ShouldBe((object)_history.EmailRenewPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@ThirdPartyPermission"].Value.ShouldBe((object)_history.ThirdPartyPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@TextPermission"].Value.ShouldBe((object)_history.TextPermission ?? DBNull.Value));
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
