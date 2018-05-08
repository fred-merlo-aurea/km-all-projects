using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using KMFakes = KM.Common.Fakes;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    ///     Unit tests for <see cref="IssueArchiveProductSubscription"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class IssueArchiveProductSubscriptionTest
    {
        private const string ProcIssueArchiveProductSubscriptionSaveAll = "e_IssueArchiveProductSubscription_SaveAll";
        private const string DataBase = "data-base";

        private IDisposable _context;
        private Dictionary<string, object> _parameters;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _parameters = new Dictionary<string, object>();

            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.IssueArchiveProductSubscription).CreateInstance();
            _objWithDefaultValues = typeof(Entity.IssueArchiveProductSubscription).CreateInstance(true);

            _list = new List<Entity.IssueArchiveProductSubscription>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };

            _bulkCopyClosed = false;
            _bulkCopyColumns = new Dictionary<string, string>();

            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SaveAll_WhenCalledWithDefaultValues_VerifyStoredProcedureParameters()
        {
            // Arrange
            var x = new Entity.IssueArchiveProductSubscription(new Entity.ProductSubscription());

            // Act
            int result = IssueArchiveProductSubscription.SaveAll(x, Client);

            // Assert
            Verify(x);
            result.ShouldBe(Rows);
        }

        [Test]
        public void SaveAll_WhenCalledWithNullValues_VerifyStoredProcedureParameters()
        {
            // Arrange
            var x = typeof(Entity.IssueArchiveProductSubscription).CreateInstance(true);

            // Act
            int result = IssueArchiveProductSubscription.SaveAll(x, Client);

            // Assert
            Verify(x);
            result.ShouldSatisfyAllConditions(
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure),
                () => _sqlCommand.CommandText.ShouldBe(ProcIssueArchiveProductSubscriptionSaveAll),
                () => result.ShouldBe(Rows));
        }

        private void Verify(Entity.IssueArchiveProductSubscription x)
        {
            _parameters.ShouldSatisfyAllConditions(
                () => _parameters["@IssueArchiveSubscriptionId"].ShouldBe(x.IssueArchiveSubscriptionId),
                () => _parameters["@IsComp"].ShouldBe(x.IsComp),
                () => _parameters["@CompId"].ShouldBe(x.CompId),
                () => _parameters["@IssueID"].ShouldBe(x.IssueID),
                () => _parameters["@SplitID"].ShouldBe(x.SplitID),
                () => _parameters["@PubSubscriptionID"].ShouldBe(x.PubSubscriptionID),
                () => _parameters["@SubscriptionID"].ShouldBe(x.SubscriptionID),
                () => _parameters["@PubID"].ShouldBe(x.PubID),
                () => _parameters["@Demo7"].ShouldBe(x.Demo7),
                () => _parameters["@QualificationDate"].ShouldBe(x.QualificationDate),
                () => _parameters["@PubQSourceID"].ShouldBe(x.PubQSourceID),
                () => _parameters["@PubCategoryID"].ShouldBe(x.PubCategoryID),
                () => _parameters["@PubTransactionID"].ShouldBe(x.PubTransactionID),
                () => _parameters["@EmailStatusID"].ShouldBe(x.EmailStatusID),
                () => _parameters["@StatusUpdatedDate"].ShouldBe(x.StatusUpdatedDate),
                () => _parameters["@StatusUpdatedReason"].ShouldBe(x.StatusUpdatedReason),
                () => _parameters["@Email"].ShouldBe(x.Email),
                () => _parameters["@DateCreated"].ShouldBe(x.DateCreated),
                () => _parameters["@DateUpdated"].ShouldBe(x.DateUpdated),
                () => _parameters["@CreatedByUserID"].ShouldBe(x.CreatedByUserID),
                () => _parameters["@UpdatedByUserID"].ShouldBe(x.UpdatedByUserID),
                () => _parameters["@SubscriptionStatusID"].ShouldBe(x.SubscriptionStatusID),
                () => _parameters["@AccountNumber"].ShouldBe(x.AccountNumber),
                () => _parameters["@AddRemoveID"].ShouldBe(x.AddRemoveID),
                () => _parameters["@Copies"].ShouldBe(x.Copies),
                () => _parameters["@GraceIssues"].ShouldBe(x.GraceIssues),
                () => _parameters["@IMBSEQ"].ShouldBe(x.IMBSeq),
                () => _parameters["@IsActive"].ShouldBe(x.IsActive),
                () => _parameters["@IsPaid"].ShouldBe(x.IsPaid),
                () => _parameters["@IsSubscribed"].ShouldBe(x.IsSubscribed),
                () => _parameters["@MemberGroup"].ShouldBe(x.MemberGroup),
                () => _parameters["@OnBehalfOf"].ShouldBe(x.OnBehalfOf),
                () => _parameters["@OrigsSrc"].ShouldBe(x.OrigsSrc),
                () => _parameters["@Par3CID"].ShouldBe(x.Par3CID),
                () => _parameters["@SequenceID"].ShouldBe(x.SequenceID),
                () => _parameters["@Status"].ShouldBe(x.Status),
                () => _parameters["@SubscriberSourceCode"].ShouldBe(x.SubscriberSourceCode),
                () => _parameters["@SubSrcID"].ShouldBe(x.SubSrcID),
                () => _parameters["@Verify"].ShouldBe(x.Verified),
                () => _parameters["@ExternalKeyID"].ShouldBe(x.ExternalKeyID),
                () => _parameters["@FirstName"].ShouldBe(x.FirstName),
                () => _parameters["@LastName"].ShouldBe(x.LastName),
                () => _parameters["@Company"].ShouldBe(x.Company),
                () => _parameters["@Title"].ShouldBe(x.Title),
                () => _parameters["@Occupation"].ShouldBe(x.Occupation),
                () => _parameters["@AddressTypeID"].ShouldBe(x.AddressTypeID),
                () => _parameters["@Address1"].ShouldBe(x.Address1),
                () => _parameters["@Address2"].ShouldBe(x.Address2),
                () => _parameters["@Address3"].ShouldBe(x.Address3),
                () => _parameters["@City"].ShouldBe(x.City),
                () => _parameters["@RegionCode"].ShouldBe(x.RegionCode),
                () => _parameters["@RegionID"].ShouldBe(x.RegionID),
                () => _parameters["@ZipCode"].ShouldBe(x.ZipCode),
                () => _parameters["@Plus4"].ShouldBe(x.Plus4),
                () => _parameters["@CarrierRoute"].ShouldBe(x.CarrierRoute),
                () => _parameters["@County"].ShouldBe(x.County),
                () => _parameters["@Country"].ShouldBe(x.Country),
                () => _parameters["@CountryID"].ShouldBe(x.CountryID),
                () => _parameters["@Latitude"].ShouldBe(x.Latitude),
                () => _parameters["@Longitude"].ShouldBe(x.Longitude),
                () => _parameters["@IsAddressValidated"].ShouldBe(x.IsAddressValidated),
                () => _parameters["@AddressValidationDate"].ShouldBe(x.AddressValidationDate),
                () => _parameters["@AddressValidationSource"].ShouldBe(x.AddressValidationSource),
                () => _parameters["@AddressValidationMessage"].ShouldBe(x.AddressValidationMessage),
                () => _parameters["@Phone"].ShouldBe(x.Phone),
                () => _parameters["@Fax"].ShouldBe(x.Fax),
                () => _parameters["@Mobile"].ShouldBe(x.Mobile),
                () => _parameters["@Website"].ShouldBe(x.Website),
                () => _parameters["@Birthdate"].ShouldBe(x.Birthdate),
                () => _parameters["@Age"].ShouldBe(x.Age),
                () => _parameters["@Income"].ShouldBe(x.Income),
                () => _parameters["@Gender"].ShouldBe(x.Gender),
                () => _parameters["@IsLocked"].ShouldBe(x.IsLocked),
                () => _parameters["@LockedByUserID"].ShouldBe(x.LockedByUserID),
                () => _parameters["@LockDate"].ShouldBe(x.LockDate),
                () => _parameters["@LockDateRelease"].ShouldBe(x.LockDateRelease),
                () => _parameters["@PhoneExt"].ShouldBe(x.PhoneExt),
                () => _parameters["@IsInActiveWaveMailing"].ShouldBe(x.IsInActiveWaveMailing),
                () => _parameters["@AddressTypeCodeId"].ShouldBe(x.AddressTypeCodeId),
                () => _parameters["@AddressLastUpdatedDate"].ShouldBe(x.AddressLastUpdatedDate),
                () => _parameters["@AddressUpdatedSourceTypeCodeId"].ShouldBe(x.AddressUpdatedSourceTypeCodeId),
                () => _parameters["@WaveMailingID"].ShouldBe(x.WaveMailingID),
                () => _parameters["@IGrp_No"].ShouldBe(x.IGrp_No),
                () => _parameters["@SFRecordIdentifier"].ShouldBe(x.SFRecordIdentifier),
                () => _parameters["@ReqFlag"].ShouldBe(x.ReqFlag),
                () => _parameters["@SubGenSubscriberID"].ShouldBe(x.SubGenSubscriberID),
                () => _parameters["@MailPermission"].ShouldBe(x.MailPermission),
                () => _parameters["@FaxPermission"].ShouldBe(x.FaxPermission),
                () => _parameters["@PhonePermission"].ShouldBe(x.PhonePermission),
                () => _parameters["@OtherProductsPermission"].ShouldBe(x.OtherProductsPermission),
                () => _parameters["@ThirdPartyPermission"].ShouldBe(x.ThirdPartyPermission),
                () => _parameters["@EmailRenewPermission"].ShouldBe(x.EmailRenewPermission),
                () => _parameters["@TextPermission"].ShouldBe(x.TextPermission));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = client =>
            {
                client.ShouldBe(Client);
                return new SqlConnection();
            };

            ShimDataFunctions.ExecuteScalarSqlCommand = command =>
            {
                _sqlCommand = command;
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (!_parameters.ContainsKey(parameter.ParameterName))
                    {
                        _parameters.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                return Rows;
            };

            var connection = new ShimSqlConnection
            {
                DatabaseGet = () => DataBase
            }.Instance;

            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            KMFakes.ShimDataFunctions.GetSqlConnectionString = _ => connection;
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

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}