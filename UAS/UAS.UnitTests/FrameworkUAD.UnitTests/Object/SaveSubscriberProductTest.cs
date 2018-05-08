using System;
using FrameworkUAD.Object;
using NUnit.Framework;
using Shouldly;
using BusinessLogicEnums = FrameworkUAD.BusinessLogic.Enums;

namespace FrameworkUAD.UnitTests.Object
{
    [TestFixture]
    public class SaveSubscriberProductTest
    {
        private const string DummyStringValue = "DummyString";
        private const int DummyIntValue = 10;

        [Test]
        public void SaveSubscriberProduct_SetAndGetValue_ReturnsDefaultValue()
        {
            // Arrange, Act
            var saveSubscriberProduct = new SaveSubscriberProduct();

            // Assert
            saveSubscriberProduct.ShouldSatisfyAllConditions(
                () => saveSubscriberProduct.QualificationDate.ShouldBeNull(),
                () => saveSubscriberProduct.PubQSourceID.ShouldBe(0),
                () => saveSubscriberProduct.PubCategoryID.ShouldBe(0),
                () => saveSubscriberProduct.PubTransactionID.ShouldBe(0),
                () => saveSubscriberProduct.StatusUpdatedDate.ShouldBe(DateTime.MinValue),
                () => saveSubscriberProduct.StatusUpdatedReason.ShouldBeNull(),
                () => saveSubscriberProduct.DateCreated.Date.ShouldBe(DateTime.MinValue),
                () => saveSubscriberProduct.DateUpdated.ShouldBeNull(),
                () => saveSubscriberProduct.Status.ShouldBeNull(),
                () => saveSubscriberProduct.PubName.ShouldBeNull(),
                () => saveSubscriberProduct.PubCode.ShouldBeNull(),
                () => saveSubscriberProduct.PubTypeDisplayName.ShouldBeNull(),
                () => saveSubscriberProduct.Demo7.ShouldBeNull(),
                () => saveSubscriberProduct.Email.ShouldBeNull(),
                () => saveSubscriberProduct.ExternalKeyID.ShouldBe(0),
                () => saveSubscriberProduct.FirstName.ShouldBeNull(),
                () => saveSubscriberProduct.LastName.ShouldBeNull(),
                () => saveSubscriberProduct.Company.ShouldBeNull(),
                () => saveSubscriberProduct.Title.ShouldBeNull(),
                () => saveSubscriberProduct.Occupation.ShouldBeNull(),
                () => saveSubscriberProduct.Address1.ShouldBeNull(),
                () => saveSubscriberProduct.Address2.ShouldBeNull(),
                () => saveSubscriberProduct.Address3.ShouldBeNull(),
                () => saveSubscriberProduct.City.ShouldBeNull(),
                () => saveSubscriberProduct.ZipCode.ShouldBeNull(),
                () => saveSubscriberProduct.Plus4.ShouldBeNull(),
                () => saveSubscriberProduct.CarrierRoute.ShouldBeNull(),
                () => saveSubscriberProduct.County.ShouldBeNull(),
                () => saveSubscriberProduct.Country.ShouldBeNull(),
                () => saveSubscriberProduct.Latitude.ShouldBe(0),
                () => saveSubscriberProduct.Longitude.ShouldBe(0),
                () => saveSubscriberProduct.Phone.ShouldBeNull(),
                () => saveSubscriberProduct.Fax.ShouldBeNull(),
                () => saveSubscriberProduct.Mobile.ShouldBeNull(),
                () => saveSubscriberProduct.Website.ShouldBeNull(),
                () => saveSubscriberProduct.Birthdate.ShouldBe(DateTime.MinValue),
                () => saveSubscriberProduct.Age.ShouldBe(0),
                () => saveSubscriberProduct.Income.ShouldBeNull(),
                () => saveSubscriberProduct.Gender.ShouldBeNull(),
                () => saveSubscriberProduct.PhoneExt.ShouldBeNull());
        }

        [Test]
        public void SaveSubscriberProduct_SetAndGetValue_ReturnsSetValue()
        {
            // Arrange
            var dummyDate = DateTime.Now.Date;

            // Act
            var saveSubscriberProduct = new SaveSubscriberProduct
            {
                QualificationDate = dummyDate,
                PubQSourceID = DummyIntValue,
                PubCategoryID = DummyIntValue,
                PubTransactionID = DummyIntValue,
                StatusUpdatedDate = dummyDate,
                StatusUpdatedReason = DummyStringValue,
                DateCreated = dummyDate,
                DateUpdated = dummyDate,
                Status = BusinessLogicEnums.EmailStatus.Inactive.ToString(),
                PubName = DummyStringValue,
                PubCode = DummyStringValue,
                PubTypeDisplayName = DummyStringValue,
                Demo7 = DummyStringValue,
                Email = DummyStringValue,
                ExternalKeyID = DummyIntValue,
                FirstName = DummyStringValue,
                LastName = DummyStringValue,
                Company = DummyStringValue,
                Title = DummyStringValue,
                Occupation = DummyStringValue,
                Address1 = DummyStringValue,
                Address2 = DummyStringValue,
                Address3 = DummyStringValue,
                City = DummyStringValue,
                ZipCode = DummyStringValue,
                Plus4 = DummyStringValue,
                CarrierRoute = DummyStringValue,
                County = DummyStringValue,
                Country = DummyStringValue,
                Latitude = DummyIntValue,
                Longitude = DummyIntValue,
                Phone = DummyStringValue,
                Fax = DummyStringValue,
                Mobile = DummyStringValue,
                Website = DummyStringValue,
                Birthdate = dummyDate,
                Age = DummyIntValue,
                Income = DummyStringValue,
                Gender = DummyStringValue,
                PhoneExt = DummyStringValue
            };

            // Assert
            saveSubscriberProduct.ShouldSatisfyAllConditions(
                () => saveSubscriberProduct.QualificationDate?.Date.ShouldBe(dummyDate),
                () => saveSubscriberProduct.PubQSourceID.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.PubCategoryID.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.PubTransactionID.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.StatusUpdatedDate.Date.ShouldBe(dummyDate),
                () => saveSubscriberProduct.StatusUpdatedReason.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.DateCreated.Date.ShouldBe(dummyDate),
                () => saveSubscriberProduct.DateUpdated.ShouldBe(dummyDate),
                () => saveSubscriberProduct.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Inactive.ToString()),
                () => saveSubscriberProduct.PubName.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.PubCode.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.PubTypeDisplayName.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Demo7.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Email.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.ExternalKeyID.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.FirstName.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.LastName.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Company.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Title.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Occupation.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Address1.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Address2.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Address3.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.City.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.ZipCode.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Plus4.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.CarrierRoute.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.County.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Country.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Latitude.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.Longitude.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.Phone.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Fax.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Mobile.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Website.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Birthdate.ShouldBe(dummyDate),
                () => saveSubscriberProduct.Age.ShouldBe(DummyIntValue),
                () => saveSubscriberProduct.Income.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.Gender.ShouldBe(DummyStringValue),
                () => saveSubscriberProduct.PhoneExt.ShouldBe(DummyStringValue));
        }
    }
}