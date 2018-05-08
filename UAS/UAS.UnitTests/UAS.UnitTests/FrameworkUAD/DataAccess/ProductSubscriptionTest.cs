using System;
using System.Collections.Generic;
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
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    ///     Unit tests for <see cref="ProductSubscription"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ProductSubscriptionTest
    {
        private const string ProcProductSubscriptionSave = "e_ProductSubscription_Save";

        private ClientConnections _client;
        private IDisposable _context;
        private Dictionary<string, object> _parameters;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _client = typeof(ClientConnections).CreateInstance();
            _parameters = new Dictionary<string, object>();

            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.ProductSubscription).CreateInstance();
            _objWithDefaultValues = typeof(Entity.ProductSubscription).CreateInstance(true);

            _list = new List<Entity.ProductSubscription>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCaseSource(nameof(_productSubscriptions))]
        public void Save_WhenCalledWithDifferentValues_VerifyStoredProcedureParameters(Entity.ProductSubscription x)
        {
            // Arrange, Act
            int result = ProductSubscription.Save(x, _client);

            // Assert
            Verify(x);
            result.ShouldBe(Rows);
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifyStoredProcedureParameters()
        {
            // Arrange
            Entity.ProductSubscription x = typeof(Entity.ProductSubscription).CreateInstance(true);

            // Act
            int result = ProductSubscription.Save(x, _client);

            // Assert
            Verify(x);
            result.ShouldBe(Rows);
        }

        private void Verify(Entity.ProductSubscription x)
        {
            _parameters.ShouldSatisfyAllConditions(
                () => _parameters["@PubSubscriptionID"].ShouldBe(x.PubSubscriptionID),
                () => _parameters["@SubscriptionID"].ShouldBe(x.SubscriptionID),
                () => _parameters["@PubID"].ShouldBe(x.PubID),
                () => _parameters["@Demo7"].ShouldBe((object)x.Demo7 ?? DBNull.Value),
                () => _parameters["@QualificationDate"].ShouldBe((object)x.QualificationDate ?? DBNull.Value),
                () => _parameters["@PubQSourceID"].ShouldBe(x.PubQSourceID),
                () => _parameters["@PubCategoryID"].ShouldBe(x.PubCategoryID),
                () => _parameters["@PubTransactionID"].ShouldBe(x.PubTransactionID),
                () => _parameters["@Email"].ShouldBe((object)x.Email ?? DBNull.Value),
                () => _parameters["@EmailStatusID"].ShouldBe(x.EmailStatusID),
                () => _parameters["@StatusUpdatedDate"].ShouldBe(x.StatusUpdatedDate),
                () => _parameters["@StatusUpdatedReason"].ShouldBe((object)x.StatusUpdatedReason ?? DBNull.Value),
                () => _parameters["@IsComp"].ShouldBe((object)x.IsComp ?? DBNull.Value),
                () => _parameters["@SubscriptionStatusID"].ShouldBe(x.SubscriptionStatusID),
                () => _parameters["@DateCreated"].ShouldBe(x.DateCreated),
                () => _parameters["@DateUpdated"].ShouldBe((object)x.DateUpdated ?? DBNull.Value),
                () => _parameters["@CreatedByUserID"].ShouldBe(x.CreatedByUserID),
                () => _parameters["@UpdatedByUserID"].ShouldBe((object)x.UpdatedByUserID ?? DBNull.Value),
                () => _parameters["@ExternalKeyID"].ShouldBe(x.ExternalKeyID),
                () => _parameters["@FirstName"].ShouldBe((object)x.FirstName ?? DBNull.Value),
                () => _parameters["@LastName"].ShouldBe((object)x.LastName ?? DBNull.Value),
                () => _parameters["@Company"].ShouldBe((object)x.Company ?? DBNull.Value),
                () => _parameters["@Title"].ShouldBe((object)x.Title ?? DBNull.Value),
                () => _parameters["@Occupation"].ShouldBe((object)x.Occupation ?? DBNull.Value),
                () => _parameters["@AddressTypeID"].ShouldBe(x.AddressTypeID),
                () => _parameters["@Address1"].ShouldBe((object)x.Address1 ?? DBNull.Value),
                () => _parameters["@Address2"].ShouldBe((object)x.Address2 ?? DBNull.Value),
                () => _parameters["@Address3"].ShouldBe((object)x.Address3 ?? DBNull.Value),
                () => _parameters["@City"].ShouldBe((object)x.City ?? DBNull.Value),
                () => _parameters["@RegionCode"].ShouldBe((object)x.RegionCode ?? DBNull.Value),
                () => _parameters["@RegionID"].ShouldBe(x.RegionID),
                () => _parameters["@ZipCode"].ShouldBe((object)x.ZipCode ?? DBNull.Value),
                () => _parameters["@Plus4"].ShouldBe((object)x.Plus4 ?? DBNull.Value),
                () => _parameters["@CarrierRoute"].ShouldBe((object)x.CarrierRoute ?? DBNull.Value),
                () => _parameters["@County"].ShouldBe((object)x.County ?? DBNull.Value),
                () => _parameters["@Country"].ShouldBe((object)x.Country ?? DBNull.Value),
                () => _parameters["@CountryID"].ShouldBe(x.CountryID),
                () => _parameters["@Latitude"].ShouldBe(x.Latitude),
                () => _parameters["@Longitude"].ShouldBe(x.Longitude),
                () => _parameters["@AddressValidationDate"].ShouldBe((object)x.AddressValidationDate ?? DBNull.Value),
                () => _parameters["@AddressValidationSource"].ShouldBe((object)x.AddressValidationSource ?? DBNull.Value),
                () => _parameters["@AddressValidationMessage"].ShouldBe((object)x.AddressValidationMessage ?? DBNull.Value),
                () => _parameters["@Phone"].ShouldBe((object)x.Phone ?? DBNull.Value),
                () => _parameters["@Fax"].ShouldBe((object)x.Fax ?? DBNull.Value),
                () => _parameters["@Mobile"].ShouldBe((object)x.Mobile ?? DBNull.Value),
                () => _parameters["@Website"].ShouldBe((object)x.Website ?? DBNull.Value),
                () => _parameters["@Birthdate"].ShouldBe(x.Birthdate),
                () => _parameters["@Age"].ShouldBe(x.Age),
                () => _parameters["@Income"].ShouldBe((object)x.Income ?? DBNull.Value),
                () => _parameters["@Gender"].ShouldBe((object)x.Gender ?? DBNull.Value),
                () => _parameters["@PhoneExt"].ShouldBe((object)x.PhoneExt ?? DBNull.Value),
                () => _parameters["@IsInActiveWaveMailing"].ShouldBe(x.IsInActiveWaveMailing),
                () => _parameters["@WaveMailingID"].ShouldBe(x.WaveMailingID),
                () => _parameters["@AddressTypeCodeId"].ShouldBe(x.AddressTypeCodeId),
                () => _parameters["@AddressLastUpdatedDate"].ShouldBe((object)x.AddressLastUpdatedDate ?? DBNull.Value),
                () => _parameters["@AddressUpdatedSourceTypeCodeId"].ShouldBe(x.AddressUpdatedSourceTypeCodeId),
                () => _parameters["@IGrp_No"].ShouldBe((object)x.IGrp_No ?? DBNull.Value),
                () => _parameters["@SFRecordIdentifier"].ShouldBe((object)x.SFRecordIdentifier ?? DBNull.Value),
                () => _parameters["@SubSrcID"].ShouldBe(x.SubSrcID),
                () => _parameters["@Par3CID"].ShouldBe(x.Par3CID),
                () => _parameters["@SequenceID"].ShouldBe(x.SequenceID),
                () => _parameters["@AddRemoveID"].ShouldBe(x.AddRemoveID),
                () => _parameters["@Copies"].ShouldBe(x.Copies),
                () => _parameters["@GraceIssues"].ShouldBe(x.GraceIssues),
                () => _parameters["@IsActive"].ShouldBe(x.IsActive),
                () => _parameters["@IsAddressValidated"].ShouldBe(x.IsAddressValidated),
                () => _parameters["@IsPaid"].ShouldBe(x.IsPaid),
                () => _parameters["@IsSubscribed"].ShouldBe(x.IsSubscribed),
                () => _parameters["@MemberGroup"].ShouldBe((object)x.MemberGroup ?? DBNull.Value),
                () => _parameters["@OnBehalfOf"].ShouldBe((object)x.OnBehalfOf ?? DBNull.Value),
                () => _parameters["@OrigsSrc"].ShouldBe((object)x.OrigsSrc ?? DBNull.Value),
                () => _parameters["@Status"].ShouldBe((object)x.Status ?? DBNull.Value),
                () => _parameters["@Verified"].ShouldBe((object)x.Verify ?? DBNull.Value),
                () => _parameters["@EmailID"].ShouldBe(x.EmailID),
                () => _parameters["@SubscriberSourceCode"].ShouldBe((object)x.SubscriberSourceCode ?? DBNull.Value),
                () => _parameters["@IMBSEQ"].ShouldBe((object)x.IMBSeq ?? DBNull.Value),
                () => _parameters["@SubGenSubscriberID"].ShouldBe(x.SubGenSubscriberID),
                () => _parameters["@SubGenSubscriptionID"].ShouldBe(x.SubGenSubscriptionID),
                () => _parameters["@SubGenPublicationID"].ShouldBe(x.SubGenPublicationID),
                () => _parameters["@SubGenMailingAddressId"].ShouldBe(x.SubGenMailingAddressId),
                () => _parameters["@SubGenBillingAddressId"].ShouldBe(x.SubGenBillingAddressId),
                () => _parameters["@IssuesLeft"].ShouldBe(x.IssuesLeft),
                () => _parameters["@UnearnedReveue"].ShouldBe(x.UnearnedReveue),
                () => _parameters["@MailPermission"].ShouldBe((object)x.MailPermission ?? DBNull.Value),
                () => _parameters["@FaxPermission"].ShouldBe((object)x.FaxPermission ?? DBNull.Value),
                () => _parameters["@PhonePermission"].ShouldBe((object)x.PhonePermission ?? DBNull.Value),
                () => _parameters["@OtherProductsPermission"].ShouldBe((object)x.OtherProductsPermission ?? DBNull.Value),
                () => _parameters["@EmailRenewPermission"].ShouldBe((object)x.EmailRenewPermission ?? DBNull.Value),
                () => _parameters["@ThirdPartyPermission"].ShouldBe((object)x.ThirdPartyPermission ?? DBNull.Value),
                () => _parameters["@TextPermission"].ShouldBe((object)x.TextPermission ?? DBNull.Value),
                () => _parameters["@ReqFlag"].ShouldBe(x.ReqFlag),
                () => _parameters["@SubGenIsLead"].ShouldBe((object)x.SubGenIsLead ?? DBNull.Value),
                () => _parameters["@SubGenRenewalCode"].ShouldBe((object)x.SubGenRenewalCode ?? DBNull.Value),
                () => _parameters["@SubGenSubscriptionRenewDate"].ShouldBe((object)x.SubGenSubscriptionRenewDate ?? DBNull.Value),
                () => _parameters["@SubGenSubscriptionExpireDate"].ShouldBe((object)x.SubGenSubscriptionExpireDate ?? DBNull.Value),
                () => _parameters["@SubGenSubscriptionLastQualifiedDate"].ShouldBe((object)x.SubGenSubscriptionLastQualifiedDate ?? DBNull.Value),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => _sqlCommand.CommandText.ShouldBe(ProcProductSubscriptionSave));
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
                foreach (SqlParameter parameter in command.Parameters)
                {
                    _parameters.Add(parameter.ParameterName, parameter.Value);
                }

                _sqlCommand = command;
                return Rows;
            };

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            KMFakes.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }

        private static List<Entity.ProductSubscription> _productSubscriptions =
            new List<Entity.ProductSubscription>
            {
                new Entity.ProductSubscription(new Entity.IssueCompDetail()),
                new Entity.ProductSubscription(typeof(Entity.ProductSubscription).CreateInstance()),
                new Entity.ProductSubscription(typeof(Entity.ProductSubscription).CreateInstance(true)),
                new Entity.ProductSubscription(new Entity.Subscription(typeof(Entity.Subscription).CreateInstance())),
                new Entity.ProductSubscription(new Entity.Subscription(typeof(Entity.IssueCompDetail).CreateInstance())),
                new Entity.ProductSubscription(new Entity.Subscription(true))
            };
    }
}