using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    ///     Unit tests for <see cref="SubscriberFinal"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriberFinalTest
    {
        private ClientConnections _client;
        private IDisposable _context;
        private Dictionary<string, object> _parameters;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _client = typeof(ClientConnections).CreateInstance();
            _parameters = new Dictionary<string, object>();

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Save_WhenCalledWithDefaultValues_VerifyStoredProcedureParameters()
        {
            // Arrange
            Entity.SubscriberFinal sf = new Entity.SubscriberFinal("process-code");

            // Act
            SubscriberFinal.Save(sf, _client);

            // Assert
            Verify(sf);
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifyStoredProcedureParameters()
        {
            // Arrange
            Entity.SubscriberFinal sf = typeof(Entity.SubscriberFinal).CreateInstance(true);
            sf.Email = string.Empty;

            // Act
            SubscriberFinal.Save(sf, _client);

            // Assert
            Verify(sf);
        }

        private void Verify(Entity.SubscriberFinal sf)
        {
            _parameters["@SubscriberFinalID"].ShouldBe(sf.SubscriberFinalID);
            _parameters["@STRecordIdentifier"].ShouldBe(sf.STRecordIdentifier);
            _parameters["@SourceFileID"].ShouldBe(sf.SourceFileID);
            _parameters["@PubCode"].ShouldBe((object) sf.PubCode ?? DBNull.Value);
            _parameters["@Sequence"].ShouldBe(sf.Sequence);
            _parameters["@FName"].ShouldBe((object) sf.FName ?? DBNull.Value);
            _parameters["@LName"].ShouldBe((object) sf.LName ?? DBNull.Value);
            _parameters["@Title"].ShouldBe((object) sf.Title ?? DBNull.Value);
            _parameters["@Company"].ShouldBe((object) sf.Company ?? DBNull.Value);
            _parameters["@Address"].ShouldBe((object) sf.Address ?? DBNull.Value);
            _parameters["@MailStop"].ShouldBe((object) sf.MailStop ?? DBNull.Value);
            _parameters["@City"].ShouldBe((object) sf.City ?? DBNull.Value);
            _parameters["@State"].ShouldBe((object) sf.State ?? DBNull.Value);
            _parameters["@Zip"].ShouldBe((object) sf.Zip ?? DBNull.Value);
            _parameters["@Plus4"].ShouldBe((object) sf.Plus4 ?? DBNull.Value);
            _parameters["@ForZip"].ShouldBe((object) sf.ForZip ?? DBNull.Value);
            _parameters["@County"].ShouldBe((object) sf.County ?? DBNull.Value);
            _parameters["@Country"].ShouldBe((object) sf.Country ?? DBNull.Value);
            _parameters["@CountryID"].ShouldBe(sf.CountryID);
            _parameters["@Phone"].ShouldBe((object) sf.Phone ?? DBNull.Value);
            _parameters["@Fax"].ShouldBe(sf.Fax);
            _parameters["@Email"].ShouldBe((object) sf.Email ?? DBNull.Value);
            _parameters["@CategoryID"].ShouldBe(sf.CategoryID);
            _parameters["@TransactionID"].ShouldBe(sf.TransactionID);
            _parameters["@TransactionDate"].ShouldBe(sf.TransactionDate);
            _parameters["@QDate"].ShouldBe(sf.QDate);
            _parameters["@QSourceID"].ShouldBe(sf.QSourceID);
            _parameters["@RegCode"].ShouldBe((object) sf.RegCode ?? DBNull.Value);
            _parameters["@Verified"].ShouldBe((object) sf.Verified ?? DBNull.Value);
            _parameters["@SubSrc"].ShouldBe((object) sf.SubSrc ?? DBNull.Value);
            _parameters["@OrigsSrc"].ShouldBe((object) sf.OrigsSrc ?? DBNull.Value);
            _parameters["@Par3C"].ShouldBe((object) sf.Par3C ?? DBNull.Value);
            _parameters["@MailPermission"].ShouldBe((object) sf.MailPermission ?? DBNull.Value);
            _parameters["@FaxPermission"].ShouldBe((object) sf.FaxPermission ?? DBNull.Value);
            _parameters["@PhonePermission"].ShouldBe((object) sf.PhonePermission ?? DBNull.Value);
            _parameters["@OtherProductsPermission"].ShouldBe((object) sf.OtherProductsPermission ?? DBNull.Value);
            _parameters["@ThirdPartyPermission"].ShouldBe((object) sf.ThirdPartyPermission ?? DBNull.Value);
            _parameters["@EmailRenewPermission"].ShouldBe((object) sf.EmailRenewPermission ?? DBNull.Value);
            _parameters["@TextPermission"].ShouldBe((object) sf.TextPermission ?? DBNull.Value);
            _parameters["@Source"].ShouldBe((object) sf.Source ?? DBNull.Value);
            _parameters["@Priority"].ShouldBe((object) sf.Priority ?? DBNull.Value);
            _parameters["@IGrp_No"].ShouldBe(sf.IGrp_No);
            _parameters["@IGrp_Cnt"].ShouldBe(sf.IGrp_Cnt);
            _parameters["@CGrp_No"].ShouldBe(sf.CGrp_No);
            _parameters["@CGrp_Cnt"].ShouldBe(sf.CGrp_Cnt);
            _parameters["@StatList"].ShouldBe((object) sf.StatList ?? DBNull.Value);
            _parameters["@Sic"].ShouldBe((object) sf.Sic ?? DBNull.Value);
            _parameters["@SicCode"].ShouldBe((object) sf.SicCode ?? DBNull.Value);
            _parameters["@Gender"].ShouldBe((object) sf.Gender ?? DBNull.Value);
            _parameters["@IGrp_Rank"].ShouldBe((object) sf.IGrp_Rank ?? DBNull.Value);
            _parameters["@CGrp_Rank"].ShouldBe((object) sf.CGrp_Rank ?? DBNull.Value);
            _parameters["@Address3"].ShouldBe((object) sf.Address3 ?? DBNull.Value);
            _parameters["@Home_Work_Address"].ShouldBe((object) sf.Home_Work_Address ?? DBNull.Value);
            _parameters["@Demo7"].ShouldBe((object) sf.Demo7 ?? DBNull.Value);
            _parameters["@Mobile"].ShouldBe((object) sf.Mobile ?? DBNull.Value);
            _parameters["@Latitude"].ShouldBe(sf.Latitude);
            _parameters["@Longitude"].ShouldBe(sf.Longitude);
            _parameters["@IsLatLonValid"].ShouldBe((object) sf.IsLatLonValid ?? DBNull.Value);
            _parameters["@LatLonMsg"].ShouldBe((object) sf.LatLonMsg ?? DBNull.Value);
            _parameters["@EmailStatusID"].ShouldBe((object) sf.EmailStatusID ?? DBNull.Value);
            _parameters["@IsMailable"].ShouldBe((object) sf.IsMailable ?? DBNull.Value);
            _parameters["@Ignore"].ShouldBe((object) sf.Ignore ?? DBNull.Value);
            _parameters["@IsDQMProcessFinished"].ShouldBe((object) sf.IsDQMProcessFinished ?? DBNull.Value);
            _parameters["@DQMProcessDate"].ShouldBe(sf.DQMProcessDate);
            _parameters["@IsUpdatedInLive"].ShouldBe((object) sf.IsUpdatedInLive ?? DBNull.Value);
            _parameters["@UpdateInLiveDate"].ShouldBe(sf.UpdateInLiveDate);
            _parameters["@SFRecordIdentifier"].ShouldBe(sf.SFRecordIdentifier);
            _parameters["@DateCreated"].ShouldBe(sf.DateCreated);
            _parameters["@DateUpdated"].ShouldBe((object) sf.DateUpdated ?? DBNull.Value);
            _parameters["@CreatedByUserID"].ShouldBe(sf.CreatedByUserID);
            _parameters["@UpdatedByUserID"].ShouldBe((object) sf.UpdatedByUserID ?? DBNull.Value);
            _parameters["@ImportRowNumber"].ShouldBe(sf.ImportRowNumber);
            _parameters["@ProcessCode"].ShouldBe((object) sf.ProcessCode ?? DBNull.Value);
            _parameters["@IsActive"].ShouldBe((object) sf.IsActive ?? DBNull.Value);
            _parameters["@ExternalKeyId"].ShouldBe(sf.ExternalKeyId);
            _parameters["@AccountNumber"].ShouldBe((object) sf.AccountNumber ?? DBNull.Value);
            _parameters["@EmailID"].ShouldBe(sf.EmailID);
            _parameters["@Copies"].ShouldBe(sf.Copies);
            _parameters["@GraceIssues"].ShouldBe(sf.GraceIssues);
            _parameters["@IsComp"].ShouldBe((object) sf.IsComp ?? DBNull.Value);
            _parameters["@IsPaid"].ShouldBe((object) sf.IsPaid ?? DBNull.Value);
            _parameters["@IsSubscribed"].ShouldBe((object) sf.IsSubscribed ?? DBNull.Value);
            _parameters["@Occupation"].ShouldBe((object) sf.Occupation ?? DBNull.Value);
            _parameters["@SubscriptionStatusID"].ShouldBe(sf.SubscriptionStatusID);
            _parameters["@SubsrcID"].ShouldBe(sf.SubsrcID);
            _parameters["@Website"].ShouldBe((object) sf.Website ?? DBNull.Value);
            _parameters["SubGenSubscriberID"].ShouldBe(sf.SubGenSubscriberID);
            _parameters["SubGenSubscriptionID"].ShouldBe(sf.SubGenSubscriptionID);
            _parameters["SubGenPublicationID"].ShouldBe(sf.SubGenPublicationID);
            _parameters["SubGenMailingAddressId"].ShouldBe(sf.SubGenMailingAddressId);
            _parameters["SubGenBillingAddressId"].ShouldBe(sf.SubGenBillingAddressId);
            _parameters["IssuesLeft"].ShouldBe(sf.IssuesLeft);
            _parameters["UnearnedReveue"].ShouldBe(sf.UnearnedReveue);
            _parameters["@SubGenIsLead"].ShouldBe((object) sf.SubGenIsLead ?? DBNull.Value);
            _parameters["@SubGenRenewalCode"].ShouldBe((object) sf.SubGenRenewalCode ?? DBNull.Value);
            _parameters["@SubGenSubscriptionRenewDate"]
                .ShouldBe((object) sf.SubGenSubscriptionRenewDate ?? DBNull.Value);
            _parameters["@SubGenSubscriptionExpireDate"]
                .ShouldBe((object) sf.SubGenSubscriptionExpireDate ?? DBNull.Value);
            _parameters["@SubGenSubscriptionLastQualifiedDate"]
                .ShouldBe((object) sf.SubGenSubscriptionLastQualifiedDate ?? DBNull.Value);
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = client =>
            {
                client.ShouldBe(_client);
                return new SqlConnection();
            };

            ShimDataFunctions.ExecuteScalarSqlCommand = command =>
            {
                command.CommandType.ShouldBe(CommandType.StoredProcedure);
                command.CommandText.ShouldBe("e_SubscriberFinal_Save");

                foreach (SqlParameter parameter in command.Parameters)
                {
                    _parameters.Add(parameter.ParameterName, parameter.Value);
                }

                return 1;
            };
        }
    }
}