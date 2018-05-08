using System;
using FrameworkUAD.Entity;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;
using BusinessLogicEnums = FrameworkUAD.BusinessLogic.Enums;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class IssueCompDetailsTest
    {
        private const string StatusUpdatedReasonDefaultValue = "Subscribed";
        private const string DummyStringValue = "DummyString";
        private const int DummyIntValue = 10;

        [Test]
        public void IssueCompDetailsTest_CtorParams_AppliedToPRoperties()
        {
            // Arrange, Act
            var issueCompDetails = new IssueCompDetail();

            // Assert
            issueCompDetails.ExternalKeyID.ShouldBe(0);
            issueCompDetails.FirstName.ShouldBe(string.Empty);
            issueCompDetails.LastName.ShouldBe(string.Empty);
            issueCompDetails.Company.ShouldBe(string.Empty);
            issueCompDetails.Title.ShouldBe(string.Empty);
            issueCompDetails.Occupation.ShouldBe(string.Empty);
            issueCompDetails.AddressTypeID.ShouldBe(0);
            issueCompDetails.Address1.ShouldBe(string.Empty);
            issueCompDetails.Address2.ShouldBe(string.Empty);
            issueCompDetails.Address3.ShouldBe(string.Empty);
            issueCompDetails.City.ShouldBe(string.Empty);
            issueCompDetails.RegionCode.ShouldBe(string.Empty);
            issueCompDetails.RegionID.ShouldBe(0);
            issueCompDetails.ZipCode.ShouldBe(string.Empty);
            issueCompDetails.Plus4.ShouldBe(string.Empty);
            issueCompDetails.CarrierRoute.ShouldBe(string.Empty);
            issueCompDetails.County.ShouldBe(string.Empty);
            issueCompDetails.Country.ShouldBe(string.Empty);
            issueCompDetails.CountryID.ShouldBe(0);
            issueCompDetails.Latitude.ShouldBe(0);
            issueCompDetails.Longitude.ShouldBe(0);
            issueCompDetails.IsAddressValidated.ShouldBe(false);
            issueCompDetails.AddressValidationSource.ShouldBe(string.Empty);
            issueCompDetails.AddressValidationMessage.ShouldBe(string.Empty);
            issueCompDetails.Email.ShouldBe(string.Empty);
            issueCompDetails.Phone.ShouldBe(string.Empty);
            issueCompDetails.Fax.ShouldBe(string.Empty);
            issueCompDetails.Mobile.ShouldBe(string.Empty);
            issueCompDetails.Website.ShouldBe(string.Empty);
            issueCompDetails.Birthdate.ShouldBe(DateTimeFunctions.GetMinDate());
            issueCompDetails.Age.ShouldBe(0);
            issueCompDetails.Income.ShouldBe(string.Empty);
            issueCompDetails.Gender.ShouldBe(string.Empty);
            issueCompDetails.IsLocked.ShouldBe(false);
            issueCompDetails.LockedByUserID.ShouldBe(0);
            issueCompDetails.PhoneExt.ShouldBe(string.Empty);
            issueCompDetails.IsInActiveWaveMailing.ShouldBe(false);
            issueCompDetails.WaveMailingID.ShouldBe(0);
            issueCompDetails.AddressTypeCodeId.ShouldBe(0);
            issueCompDetails.AddressUpdatedSourceTypeCodeId.ShouldBe(0);
        }

        [Test]
        public void IssueCompDetails_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var issueCompDetails = new IssueCompDetail();

            // Assert
            issueCompDetails.ShouldSatisfyAllConditions(
                () => issueCompDetails.QualificationDate?.Date.ShouldBe(DateTime.Now.Date),
                () => issueCompDetails.PubQSourceID.ShouldBe(0),
                () => issueCompDetails.PubCategoryID.ShouldBe(0),
                () => issueCompDetails.PubTransactionID.ShouldBe(0),
                () => issueCompDetails.StatusUpdatedDate.Date.ShouldBe(DateTime.Now.Date),
                () => issueCompDetails.StatusUpdatedReason.ShouldBe(StatusUpdatedReasonDefaultValue),
                () => issueCompDetails.DateCreated.Date.ShouldBe(DateTime.Now.Date),
                () => issueCompDetails.DateUpdated.ShouldBeNull(),
                () => issueCompDetails.CreatedByUserID.ShouldBe(1),
                () => issueCompDetails.UpdatedByUserID.ShouldBe(0),
                () => issueCompDetails.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Active.ToString()),
                () => issueCompDetails.PubName.ShouldBeNull(),
                () => issueCompDetails.PubCode.ShouldBeNull(),
                () => issueCompDetails.PubTypeDisplayName.ShouldBeNull(),
                () => issueCompDetails.PubID.ShouldBe(0),
                () => issueCompDetails.PubSubscriptionID.ShouldBe(0),
                () => issueCompDetails.SubscriptionStatusID.ShouldBe(1),
                () => issueCompDetails.Demo7.ShouldBe(string.Empty));
        }

        [Test]
        public void IssueCompDetails_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange, Act
            var dummyDate = DateTime.Now.Date;
            var issueCompDetails = new IssueCompDetail
            {
                QualificationDate = dummyDate,
                PubQSourceID = DummyIntValue,
                PubCategoryID = DummyIntValue,
                PubTransactionID = DummyIntValue,
                StatusUpdatedDate = dummyDate,
                StatusUpdatedReason = DummyStringValue,
                DateCreated = dummyDate,
                DateUpdated = dummyDate,
                CreatedByUserID = DummyIntValue,
                UpdatedByUserID = DummyIntValue,
                Status = BusinessLogicEnums.EmailStatus.Inactive.ToString(),
                PubName = DummyStringValue,
                PubCode = DummyStringValue,
                PubTypeDisplayName = DummyStringValue,
                PubID = DummyIntValue,
                PubSubscriptionID = DummyIntValue,
                SubscriptionStatusID = DummyIntValue,
                Demo7 = DummyStringValue
            };

            // Assert
            issueCompDetails.ShouldSatisfyAllConditions(
                () => issueCompDetails.QualificationDate?.Date.ShouldBe(dummyDate),
                () => issueCompDetails.PubQSourceID.ShouldBe(DummyIntValue),
                () => issueCompDetails.PubCategoryID.ShouldBe(DummyIntValue),
                () => issueCompDetails.PubTransactionID.ShouldBe(DummyIntValue),
                () => issueCompDetails.StatusUpdatedDate.Date.ShouldBe(dummyDate),
                () => issueCompDetails.StatusUpdatedReason.ShouldBe(DummyStringValue),
                () => issueCompDetails.DateCreated.Date.ShouldBe(dummyDate),
                () => issueCompDetails.DateUpdated.ShouldBe(dummyDate),
                () => issueCompDetails.CreatedByUserID.ShouldBe(DummyIntValue),
                () => issueCompDetails.UpdatedByUserID.ShouldBe(DummyIntValue),
                () => issueCompDetails.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Inactive.ToString()),
                () => issueCompDetails.PubName.ShouldBe(DummyStringValue),
                () => issueCompDetails.PubCode.ShouldBe(DummyStringValue),
                () => issueCompDetails.PubTypeDisplayName.ShouldBe(DummyStringValue),
                () => issueCompDetails.PubID.ShouldBe(DummyIntValue),
                () => issueCompDetails.PubSubscriptionID.ShouldBe(DummyIntValue),
                () => issueCompDetails.SubscriptionStatusID.ShouldBe(DummyIntValue),
                () => issueCompDetails.Demo7.ShouldBe(DummyStringValue));
        }
    }
}