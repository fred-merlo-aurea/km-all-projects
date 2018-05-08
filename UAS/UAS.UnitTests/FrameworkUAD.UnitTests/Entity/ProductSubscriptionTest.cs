using System;
using FrameworkUAD.Entity;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;
using BusinessLogicEnums = FrameworkUAD.BusinessLogic.Enums;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class ProductSubscriptionTest
    {
        private const string StatusUpdatedReasonDefaultValue = "Subscribed";
        private const string DummyStringValue = "DummyString";
        private const int DummyIntValue = 10;

        [Test]
        public void ProductSubscription_CtorParams_AppliedToPRoperties()
        {
            // Arrange, Act
            var productSubscription = new ProductSubscription();

            // Assert
            productSubscription.ExternalKeyID.ShouldBe(0);
            productSubscription.FirstName.ShouldBe(string.Empty);
            productSubscription.LastName.ShouldBe(string.Empty);
            productSubscription.Company.ShouldBe(string.Empty);
            productSubscription.Title.ShouldBe(string.Empty);
            productSubscription.Occupation.ShouldBe(string.Empty);
            productSubscription.AddressTypeID.ShouldBe(0);
            productSubscription.Address1.ShouldBe(string.Empty);
            productSubscription.Address2.ShouldBe(string.Empty);
            productSubscription.Address3.ShouldBe(string.Empty);
            productSubscription.City.ShouldBe(string.Empty);
            productSubscription.RegionCode.ShouldBe(string.Empty);
            productSubscription.RegionID.ShouldBe(0);
            productSubscription.ZipCode.ShouldBe(string.Empty);
            productSubscription.Plus4.ShouldBe(string.Empty);
            productSubscription.CarrierRoute.ShouldBe(string.Empty);
            productSubscription.County.ShouldBe(string.Empty);
            productSubscription.Country.ShouldBe(string.Empty);
            productSubscription.CountryID.ShouldBe(0);
            productSubscription.Latitude.ShouldBe(0);
            productSubscription.Longitude.ShouldBe(0);
            productSubscription.IsAddressValidated.ShouldBe(false);
            productSubscription.AddressValidationSource.ShouldBe(string.Empty);
            productSubscription.AddressValidationMessage.ShouldBe(string.Empty);
            productSubscription.Email.ShouldBe(string.Empty);
            productSubscription.Phone.ShouldBe(string.Empty);
            productSubscription.Fax.ShouldBe(string.Empty);
            productSubscription.Mobile.ShouldBe(string.Empty);
            productSubscription.Website.ShouldBe(string.Empty);
            productSubscription.Birthdate.ShouldBe(DateTimeFunctions.GetMinDate());
            productSubscription.Age.ShouldBe(0);
            productSubscription.Income.ShouldBe(string.Empty);
            productSubscription.Gender.ShouldBe(string.Empty);
            productSubscription.IsLocked.ShouldBe(false);
            productSubscription.LockedByUserID.ShouldBe(0);
            productSubscription.PhoneExt.ShouldBe(string.Empty);
            productSubscription.IsInActiveWaveMailing.ShouldBe(false);
            productSubscription.WaveMailingID.ShouldBe(0);
            productSubscription.AddressTypeCodeId.ShouldBe(0);
            productSubscription.AddressUpdatedSourceTypeCodeId.ShouldBe(0);
        }

        [Test]
        public void ProductSubscription_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var productSubscription = new ProductSubscription();

            // Assert
            productSubscription.ShouldSatisfyAllConditions(
                () => productSubscription.QualificationDate?.Date.ShouldBe(DateTime.Now.Date),
                () => productSubscription.PubQSourceID.ShouldBe(0),
                () => productSubscription.PubCategoryID.ShouldBe(0),
                () => productSubscription.PubTransactionID.ShouldBe(0),
                () => productSubscription.StatusUpdatedDate.Date.ShouldBe(DateTime.Now.Date),
                () => productSubscription.StatusUpdatedReason.ShouldBe(StatusUpdatedReasonDefaultValue),
                () => productSubscription.DateCreated.Date.ShouldBe(DateTime.Now.Date),
                () => productSubscription.DateUpdated.ShouldBeNull(),
                () => productSubscription.CreatedByUserID.ShouldBe(1),
                () => productSubscription.UpdatedByUserID.ShouldBe(0),
                () => productSubscription.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Active.ToString()),
                () => productSubscription.PubName.ShouldBe(string.Empty),
                () => productSubscription.PubCode.ShouldBe(string.Empty),
                () => productSubscription.PubTypeDisplayName.ShouldBe(string.Empty),
                () => productSubscription.PubID.ShouldBe(0),
                () => productSubscription.PubSubscriptionID.ShouldBe(0),
                () => productSubscription.SubscriptionStatusID.ShouldBe(1),
                () => productSubscription.Demo7.ShouldBe(string.Empty));
        }

        [Test]
        public void ProductSubscription_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange, Act
            var dummyDate = DateTime.Now.Date;
            var productSubscription = new ProductSubscription
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
            productSubscription.ShouldSatisfyAllConditions(
                () => productSubscription.QualificationDate?.Date.ShouldBe(dummyDate),
                () => productSubscription.PubQSourceID.ShouldBe(DummyIntValue),
                () => productSubscription.PubCategoryID.ShouldBe(DummyIntValue),
                () => productSubscription.PubTransactionID.ShouldBe(DummyIntValue),
                () => productSubscription.StatusUpdatedDate.Date.ShouldBe(dummyDate),
                () => productSubscription.StatusUpdatedReason.ShouldBe(DummyStringValue),
                () => productSubscription.DateCreated.Date.ShouldBe(dummyDate),
                () => productSubscription.DateUpdated.ShouldBe(dummyDate),
                () => productSubscription.CreatedByUserID.ShouldBe(DummyIntValue),
                () => productSubscription.UpdatedByUserID.ShouldBe(DummyIntValue),
                () => productSubscription.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Inactive.ToString()),
                () => productSubscription.PubName.ShouldBe(DummyStringValue),
                () => productSubscription.PubCode.ShouldBe(DummyStringValue),
                () => productSubscription.PubTypeDisplayName.ShouldBe(DummyStringValue),
                () => productSubscription.PubID.ShouldBe(DummyIntValue),
                () => productSubscription.PubSubscriptionID.ShouldBe(DummyIntValue),
                () => productSubscription.SubscriptionStatusID.ShouldBe(DummyIntValue),
                () => productSubscription.Demo7.ShouldBe(DummyStringValue));
        }
    }
}