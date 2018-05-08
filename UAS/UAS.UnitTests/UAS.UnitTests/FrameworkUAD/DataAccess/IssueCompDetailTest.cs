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
    /// Unit tests for <see cref="IssueCompDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class IssueCompDetailTest
    {
        private const string CommandText = "e_IssueCompDetail_Save";

        private IDisposable _context;
        private SqlCommand _saveCommand;
        private Entity.IssueCompDetail _issue;

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
            _issue = typeof(Entity.IssueCompDetail).CreateInstance();

            // Act
            IssueCompDetail.Save(_issue, new ClientConnections());

            // Assert
            _issue.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        [Test]
        public void Save_WhenCalledWithNullValues_VerifySqlParameters()
        {
            // Arrange
            _issue = typeof(Entity.IssueCompDetail).CreateInstance(true);

            // Act
            IssueCompDetail.Save(_issue, new ClientConnections());

            // Assert
            _issue.ShouldSatisfyAllConditions(
                () => _saveCommand.CommandText.ShouldBe(CommandText),
                () => _saveCommand.CommandType.ShouldBe(CommandType.StoredProcedure));

            VerifySqlParameters();
        }

        private void VerifySqlParameters()
        {
            _saveCommand.ShouldSatisfyAllConditions(
               () => _saveCommand.Parameters["@IssueCompDetailId"].Value.ShouldBe(_issue.IssueCompDetailId),
               () => _saveCommand.Parameters["@IssueCompId"].Value.ShouldBe(_issue.IssueCompID),
               () => _saveCommand.Parameters["@PubID"].Value.ShouldBe(_issue.PubID),
               () => _saveCommand.Parameters["@demo7"].Value.ShouldBe((object)_issue.Demo7 ?? DBNull.Value),
               () => _saveCommand.Parameters["@Qualificationdate"].Value.ShouldBe((object)_issue.QualificationDate ?? DBNull.Value),
               () => _saveCommand.Parameters["@PubQSourceID"].Value.ShouldBe(_issue.PubQSourceID),
               () => _saveCommand.Parameters["@PubCategoryID"].Value.ShouldBe(_issue.PubCategoryID),
               () => _saveCommand.Parameters["@PubTransactionID"].Value.ShouldBe(_issue.PubTransactionID),
               () => _saveCommand.Parameters["@Email"].Value.ShouldBe((object)_issue.Email ?? DBNull.Value),
               () => _saveCommand.Parameters["@EmailStatusID"].Value.ShouldBe(_issue.EmailStatusID),
               () => _saveCommand.Parameters["@StatusUpdatedDate"].Value.ShouldBe(_issue.StatusUpdatedDate),
               () => _saveCommand.Parameters["@StatusUpdatedReason"].Value.ShouldBe((object)_issue.StatusUpdatedReason ?? DBNull.Value),
               () => _saveCommand.Parameters["@IsComp"].Value.ShouldBe((object)_issue.IsComp ?? DBNull.Value),
               () => _saveCommand.Parameters["@SubscriptionStatusID"].Value.ShouldBe(_issue.SubscriptionStatusID),
               () => _saveCommand.Parameters["@DateCreated"].Value.ShouldBe(_issue.DateCreated),
               () => _saveCommand.Parameters["@DateUpdated"].Value.ShouldBe((object)_issue.DateUpdated ?? DBNull.Value),
               () => _saveCommand.Parameters["@CreatedByUserID"].Value.ShouldBe(_issue.CreatedByUserID),
               () => _saveCommand.Parameters["@UpdatedByUserID"].Value.ShouldBe((object)_issue.UpdatedByUserID ?? DBNull.Value),
               () => _saveCommand.Parameters["@ExternalKeyID"].Value.ShouldBe(_issue.ExternalKeyID),
               () => _saveCommand.Parameters["@FirstName"].Value.ShouldBe((object)_issue.FirstName ?? DBNull.Value),
               () => _saveCommand.Parameters["@LastName"].Value.ShouldBe((object)_issue.LastName ?? DBNull.Value),
               () => _saveCommand.Parameters["@Company"].Value.ShouldBe((object)_issue.Company ?? DBNull.Value),
               () => _saveCommand.Parameters["@Title"].Value.ShouldBe((object)_issue.Title ?? DBNull.Value),
               () => _saveCommand.Parameters["@Occupation"].Value.ShouldBe((object)_issue.Occupation ?? DBNull.Value),
               () => _saveCommand.Parameters["@AddressTypeID"].Value.ShouldBe(_issue.AddressTypeID),
               () => _saveCommand.Parameters["@Address1"].Value.ShouldBe((object)_issue.Address1 ?? DBNull.Value),
               () => _saveCommand.Parameters["@Address2"].Value.ShouldBe((object)_issue.Address2 ?? DBNull.Value),
               () => _saveCommand.Parameters["@Address3"].Value.ShouldBe((object)_issue.Address3 ?? DBNull.Value),
               () => _saveCommand.Parameters["@City"].Value.ShouldBe((object)_issue.City ?? DBNull.Value),
               () => _saveCommand.Parameters["@RegionCode"].Value.ShouldBe((object)_issue.RegionCode ?? DBNull.Value),
               () => _saveCommand.Parameters["@RegionID"].Value.ShouldBe(_issue.RegionID),
               () => _saveCommand.Parameters["@ZipCode"].Value.ShouldBe((object)_issue.ZipCode ?? DBNull.Value),
               () => _saveCommand.Parameters["@Plus4"].Value.ShouldBe((object)_issue.Plus4 ?? DBNull.Value),
               () => _saveCommand.Parameters["@CarrierRoute"].Value.ShouldBe((object)_issue.CarrierRoute ?? DBNull.Value),
               () => _saveCommand.Parameters["@County"].Value.ShouldBe((object)_issue.County ?? DBNull.Value),
               () => _saveCommand.Parameters["@Country"].Value.ShouldBe((object)_issue.Country ?? DBNull.Value),
               () => _saveCommand.Parameters["@CountryID"].Value.ShouldBe(_issue.CountryID),
               () => _saveCommand.Parameters["@Latitude"].Value.ShouldBe(_issue.Latitude),
               () => _saveCommand.Parameters["@Longitude"].Value.ShouldBe(_issue.Longitude),
               () => _saveCommand.Parameters["@IsAddressValidated"].Value.ShouldBe(_issue.IsAddressValidated),
               () => _saveCommand.Parameters["@AddressValidationDate"].Value.ShouldBe((object)_issue.AddressValidationDate ?? DBNull.Value),
               () => _saveCommand.Parameters["@AddressValidationSource"].Value.ShouldBe((object)_issue.AddressValidationSource ?? DBNull.Value),
               () => _saveCommand.Parameters["@AddressValidationMessage"].Value.ShouldBe((object)_issue.AddressValidationMessage ?? DBNull.Value),
               () => _saveCommand.Parameters["@Phone"].Value.ShouldBe((object)_issue.Phone ?? DBNull.Value),
               () => _saveCommand.Parameters["@Fax"].Value.ShouldBe((object)_issue.Fax ?? DBNull.Value),
               () => _saveCommand.Parameters["@Mobile"].Value.ShouldBe((object)_issue.Mobile ?? DBNull.Value),
               () => _saveCommand.Parameters["@Website"].Value.ShouldBe((object)_issue.Website ?? DBNull.Value),
               () => _saveCommand.Parameters["@Birthdate"].Value.ShouldBe(_issue.Birthdate),
               () => _saveCommand.Parameters["@Age"].Value.ShouldBe(_issue.Age),
               () => _saveCommand.Parameters["@Income"].Value.ShouldBe((object)_issue.Income ?? DBNull.Value),
               () => _saveCommand.Parameters["@Gender"].Value.ShouldBe((object)_issue.Gender ?? DBNull.Value),
               () => _saveCommand.Parameters["@PhoneExt"].Value.ShouldBe((object)_issue.PhoneExt ?? DBNull.Value),
               () => _saveCommand.Parameters["@IsInActiveWaveMailing"].Value.ShouldBe(_issue.IsInActiveWaveMailing),
               () => _saveCommand.Parameters["@WaveMailingID"].Value.ShouldBe(_issue.WaveMailingID),
               () => _saveCommand.Parameters["@AddressTypeCodeId"].Value.ShouldBe(_issue.AddressTypeCodeId),
               () => _saveCommand.Parameters["@AddressLastUpdatedDate"].Value.ShouldBe((object)_issue.AddressLastUpdatedDate ?? DBNull.Value),
               () => _saveCommand.Parameters["@AddressUpdatedSourceTypeCodeId"].Value.ShouldBe(_issue.AddressUpdatedSourceTypeCodeId),
               () => _saveCommand.Parameters["@IGrp_No"].Value.ShouldBe((object)_issue.IGrp_No ?? DBNull.Value),
               () => _saveCommand.Parameters["@SFRecordIdentifier"].Value.ShouldBe((object)_issue.SFRecordIdentifier ?? DBNull.Value),
               () => _saveCommand.Parameters["@SubSrcID"].Value.ShouldBe(_issue.SubSrcID),
               () => _saveCommand.Parameters["@Par3CID"].Value.ShouldBe(_issue.Par3CID));
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
