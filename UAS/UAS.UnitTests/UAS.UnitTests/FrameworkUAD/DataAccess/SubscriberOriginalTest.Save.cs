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
    /// Unit tests for <see cref="SubscriberOriginal.Save"/>
    /// </summary>
    public partial class SubscriberOriginalTest
    {
        private const string CommandText = "e_SubscriberOriginal_Save";

        private SqlCommand _saveCommand;
        private Entity.SubscriberOriginal _org;

        [Test]
        public void Save_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            SetupFakesForSaveMethod();
            _org = typeof(Entity.SubscriberOriginal).CreateInstance();

            // Act
            SubscriberOriginal.Save(_org, new ClientConnections());

            // Assert
            _org.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            SetupFakesForSaveMethod();
            _org = typeof(Entity.SubscriberOriginal).CreateInstance(true);

            // Act
            SubscriberOriginal.Save(_org, new ClientConnections());

            // Assert
            _org.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
                () => _saveCommand.Parameters["@SubscriberOriginalID"].Value.ShouldBe(_org.SubscriberOriginalID),
                () => _saveCommand.Parameters["@SourceFileID"].Value.ShouldBe(_org.SourceFileID),
                () => _saveCommand.Parameters["@PubCode"].Value.ShouldBe((object)_org.PubCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sequence"].Value.ShouldBe(_org.Sequence),
                () => _saveCommand.Parameters["@FName"].Value.ShouldBe((object)_org.FName ?? DBNull.Value),
                () => _saveCommand.Parameters["@LName"].Value.ShouldBe((object)_org.LName ?? DBNull.Value),
                () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_org.Title ?? DBNull.Value),
                () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_org.Company ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address"].Value.ShouldBe((object)_org.Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailStop"].Value.ShouldBe((object)_org.MailStop ?? DBNull.Value),
                () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_org.City ?? DBNull.Value),
                () => _saveCommand.Parameters["@State"].Value.ShouldBe((object)_org.State ?? DBNull.Value),
                () => _saveCommand.Parameters["@Zip"].Value.ShouldBe((object)_org.Zip ?? DBNull.Value),
                () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_org.Plus4 ?? DBNull.Value),
                () => _saveCommand.Parameters["@ForZip"].Value.ShouldBe((object)_org.ForZip ?? DBNull.Value),
                () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_org.County ?? DBNull.Value),
                () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_org.Country ?? DBNull.Value),
                () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_org.CountryID),
                () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_org.Phone ?? DBNull.Value),
                () => _saveCommand.Parameters["@Fax"].Value.ShouldBe(_org.Fax),
                () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_org.Email ?? DBNull.Value),
                () => _saveCommand.Parameters["@CategoryID"].Value.ShouldBe(_org.CategoryID),
                () => _saveCommand.Parameters["@TransactionID"].Value.ShouldBe(_org.TransactionID),
                () => _saveCommand.Parameters["@TransactionDate"].Value.ShouldBe((object)_org.TransactionDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QDate"].Value.ShouldBe((object)_org.QDate ?? DBNull.Value),
                () => _saveCommand.Parameters["@QSourceID"].Value.ShouldBe(_org.QSourceID),
                () => _saveCommand.Parameters["@RegCode"].Value.ShouldBe((object)_org.RegCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Verified"].Value.ShouldBe((object)_org.Verified ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubSrc"].Value.ShouldBe((object)_org.SubSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@OrigsSrc"].Value.ShouldBe((object)_org.OrigsSrc ?? DBNull.Value),
                () => _saveCommand.Parameters["@Par3C"].Value.ShouldBe((object)_org.Par3C ?? DBNull.Value),
                () => _saveCommand.Parameters["@MailPermission"].Value.ShouldBe((object)_org.MailPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@FaxPermission"].Value.ShouldBe((object)_org.FaxPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@PhonePermission"].Value.ShouldBe((object)_org.PhonePermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@OtherProductsPermission"].Value.ShouldBe((object)_org.OtherProductsPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@ThirdPartyPermission"].Value.ShouldBe((object)_org.ThirdPartyPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailRenewPermission"].Value.ShouldBe((object)_org.EmailRenewPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@TextPermission"].Value.ShouldBe((object)_org.TextPermission ?? DBNull.Value),
                () => _saveCommand.Parameters["@Source"].Value.ShouldBe((object)_org.Source ?? DBNull.Value),
                () => _saveCommand.Parameters["@Priority"].Value.ShouldBe((object)_org.Priority ?? DBNull.Value),
                () => _saveCommand.Parameters["@Sic"].Value.ShouldBe((object)_org.Sic ?? DBNull.Value),
                () => _saveCommand.Parameters["@SicCode"].Value.ShouldBe((object)_org.SicCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@Gender"].Value.ShouldBe((object)_org.Gender ?? DBNull.Value),
                () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_org.Address3 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Home_Work_Address"].Value.ShouldBe((object)_org.Home_Work_Address ?? DBNull.Value),
                () => _saveCommand.Parameters["@Demo7"].Value.ShouldBe((object)_org.Demo7 ?? DBNull.Value),
                () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_org.Mobile ?? DBNull.Value),
                () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_org.Latitude),
                () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_org.Longitude),
                () => _saveCommand.Parameters["@Score"].Value.ShouldBe(_org.Score),
                () => _saveCommand.Parameters["@EmailStatusID"].Value.ShouldBe(_org.EmailStatusID),
                () => _saveCommand.Parameters["@SORecordIdentifier"].Value.ShouldBe(_org.SORecordIdentifier),
                () => _saveCommand.Parameters["@ImportRowNumber"].Value.ShouldBe(_org.ImportRowNumber),
                () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_org.DateCreated),
                () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_org.DateUpdated ?? DBNull.Value),
                () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_org.CreatedByUserID),
                () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_org.UpdatedByUserID ?? DBNull.Value),
                () => _saveCommand.Parameters["@ProcessCode"].Value.ShouldBe((object)_org.ProcessCode ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsActive"].Value.ShouldBe((object)_org.IsActive ?? DBNull.Value),
                () => _saveCommand.Parameters["@ExternalKeyId"].Value.ShouldBe(_org.ExternalKeyId),
                () => _saveCommand.Parameters["@AccountNumber"].Value.ShouldBe((object)_org.AccountNumber ?? DBNull.Value),
                () => _saveCommand.Parameters["@EmailID"].Value.ShouldBe(_org.EmailID),
                () => _saveCommand.Parameters["@Copies"].Value.ShouldBe(_org.Copies),
                () => _saveCommand.Parameters["@GraceIssues"].Value.ShouldBe(_org.GraceIssues),
                () => _saveCommand.Parameters["@IsComp"].Value.ShouldBe((object)_org.IsComp ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsPaid"].Value.ShouldBe((object)_org.IsPaid ?? DBNull.Value),
                () => _saveCommand.Parameters["@IsSubscribed"].Value.ShouldBe((object)_org.IsSubscribed ?? DBNull.Value),
                () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe((object)_org.Occupation ?? DBNull.Value),
                () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_org.SubscriptionStatusID),
                () => _saveCommand.Parameters["@SubsrcID"].Value.ShouldBe(_org.SubsrcID),
                () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object)_org.Website ?? DBNull.Value));
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
