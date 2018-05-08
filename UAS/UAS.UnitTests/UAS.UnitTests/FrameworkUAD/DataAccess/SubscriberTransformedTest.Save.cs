using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit tests for <see cref="SubscriberTransformed.Save"/>
    /// </summary>
    public partial class SubscriberTransformedTest
    {
        private const string CommandText = "e_SubscriberTransformed_Save";

        private SqlCommand _saveCommand;
        private Entity.SubscriberTransformed _transformed;

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            SetupFakesForSaveMethod();
            _transformed = typeof(Entity.SubscriberTransformed).CreateInstance();

            // Act
            SubscriberTransformed.Save(_transformed, new ClientConnections());

            // Assert
            _transformed.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            SetupFakesForSaveMethod();
            _transformed = typeof(Entity.SubscriberTransformed).CreateInstance(true);

            // Act
            SubscriberTransformed.Save(_transformed, new ClientConnections());

            // Assert
            _transformed.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriberTransformedID"].Value.ShouldBe(_transformed.SubscriberTransformedID),
                () => _saveCommand.Parameters["@SORecordIdentifier"].Value.ShouldBe(_transformed.SORecordIdentifier),
                () => _saveCommand.Parameters["@SourceFileID"].Value.ShouldBe(_transformed.SourceFileID),
                () => _saveCommand.Parameters["@PubCode"].Value.ShouldBe((object)_transformed.PubCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sequence"].Value.ShouldBe(_transformed.Sequence),
                () => _saveCommand.Parameters["@FName"].Value.ShouldBe((object)_transformed.FName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LName"].Value.ShouldBe((object)_transformed.LName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_transformed.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_transformed.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address"].Value.ShouldBe((object)_transformed.Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailStop"].Value.ShouldBe((object)_transformed.MailStop ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_transformed.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@State"].Value.ShouldBe((object)_transformed.State ?? DBNull.Value),
                () => _saveCommand.Parameters["@Zip"].Value.ShouldBe((object)_transformed.Zip ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_transformed.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@ForZip"].Value.ShouldBe((object)_transformed.ForZip ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_transformed.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_transformed.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_transformed.CountryID),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_transformed.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe(_transformed.Fax),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_transformed.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@CategoryID"].Value.ShouldBe(_transformed.CategoryID),
                () => _saveCommand.Parameters["@TransactionID"].Value.ShouldBe(_transformed.TransactionID),
                () => _saveCommand.Parameters["@TransactionDate"].Value.ShouldBe((object)_transformed.TransactionDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QDate"].Value.ShouldBe((object)_transformed.QDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QSourceID"].Value.ShouldBe(_transformed.QSourceID),
                () => _saveCommand.Parameters["@RegCode"].Value.ShouldBe((object)_transformed.RegCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Verified"].Value.ShouldBe((object)_transformed.Verified ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubSrc"].Value.ShouldBe((object)_transformed.SubSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@OrigsSrc"].Value.ShouldBe((object)_transformed.OrigsSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@Par3C"].Value.ShouldBe((object)_transformed.Par3C ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailPermission"].Value.ShouldBe((object)_transformed.MailPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@FaxPermission"].Value.ShouldBe((object)_transformed.FaxPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhonePermission"].Value.ShouldBe((object)_transformed.PhonePermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@OtherProductsPermission"].Value.ShouldBe((object)_transformed.OtherProductsPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@ThirdPartyPermission"].Value.ShouldBe((object)_transformed.ThirdPartyPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailRenewPermission"].Value.ShouldBe((object)_transformed.EmailRenewPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@TextPermission"].Value.ShouldBe((object)_transformed.TextPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@Source"].Value.ShouldBe((object)_transformed.Source ?? DBNull.Value),
                () => _saveCommand.Parameters["@Priority"].Value.ShouldBe((object)_transformed.Priority ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sic"].Value.ShouldBe((object)_transformed.Sic ?? DBNull.Value),
                () => _saveCommand.Parameters["@SicCode"].Value.ShouldBe((object)_transformed.SicCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Gender"].Value.ShouldBe((object)_transformed.Gender ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_transformed.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Home_Work_Address"].Value.ShouldBe((object)_transformed.Home_Work_Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@Demo7"].Value.ShouldBe((object)_transformed.Demo7 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_transformed.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_transformed.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_transformed.Longitude),
                () => _saveCommand.Parameters["@IsLatLonValid"].Value.ShouldBe((object)_transformed.IsLatLonValid ?? DBNull.Value),
                () => _saveCommand.Parameters["@LatLonMsg"].Value.ShouldBe((object)_transformed.LatLonMsg ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailStatusID"].Value.ShouldBe((object)_transformed.EmailStatusID ?? DBNull.Value),
                () => _saveCommand.Parameters["@STRecordIdentifier"].Value.ShouldBe(_transformed.STRecordIdentifier),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_transformed.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_transformed.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_transformed.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_transformed.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ImportRowNumber"].Value.ShouldBe(_transformed.ImportRowNumber),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe((object)_transformed.ProcessCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_transformed.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@ExternalKeyId"].Value.ShouldBe(_transformed.ExternalKeyId),
                () => _saveCommand.Parameters["@AccountNumber"].Value.ShouldBe((object)_transformed.AccountNumber ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailID"].Value.ShouldBe(_transformed.EmailID),
                () => _saveCommand.Parameters["@Copies"].Value.ShouldBe(_transformed.Copies),
                () => _saveCommand.Parameters["@GraceIssues"].Value.ShouldBe(_transformed.GraceIssues),
                () => _saveCommand.Parameters["@IsComp"].Value.ShouldBe((object)_transformed.IsComp ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsPaid"].Value.ShouldBe((object)_transformed.IsPaid ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsSubscribed"].Value.ShouldBe((object)_transformed.IsSubscribed ?? DBNull.Value),
                () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe((object)_transformed.Occupation ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_transformed.SubscriptionStatusID),
                () => _saveCommand.Parameters["@SubsrcID"].Value.ShouldBe(_transformed.SubsrcID),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object)_transformed.Website ?? DBNull.Value),
                () => _saveCommand.Parameters["SubGenSubscriberID"].Value.ShouldBe(_transformed.SubGenSubscriberID),
                () => _saveCommand.Parameters["SubGenSubscriptionID"].Value.ShouldBe(_transformed.SubGenSubscriptionID),
                () => _saveCommand.Parameters["SubGenPublicationID"].Value.ShouldBe(_transformed.SubGenPublicationID),
                () => _saveCommand.Parameters["SubGenMailingAddressId"].Value.ShouldBe(_transformed.SubGenMailingAddressId),
                () => _saveCommand.Parameters["SubGenBillingAddressId"].Value.ShouldBe(_transformed.SubGenBillingAddressId),
                () => _saveCommand.Parameters["IssuesLeft"].Value.ShouldBe(_transformed.IssuesLeft),
                () => _saveCommand.Parameters["UnearnedReveue"].Value.ShouldBe(_transformed.UnearnedReveue),
                () => _saveCommand.Parameters["@SubGenIsLead"].Value.ShouldBe((object)_transformed.SubGenIsLead ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubGenRenewalCode"].Value.ShouldBe((object)_transformed.SubGenRenewalCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubGenSubscriptionRenewDate"].Value.ShouldBe((object)_transformed.SubGenSubscriptionRenewDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubGenSubscriptionExpireDate"].Value.ShouldBe((object)_transformed.SubGenSubscriptionExpireDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubGenSubscriptionLastQualifiedDate"].Value.ShouldBe((object)_transformed.SubGenSubscriptionLastQualifiedDate ?? DBNull.Value));
        }

        private void SetupFakesForSaveMethod()
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
