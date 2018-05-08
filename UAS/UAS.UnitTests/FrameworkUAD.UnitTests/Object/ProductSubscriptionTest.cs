using System;
using FrameworkUAD.Object;
using KM.Common.Functions;
using NUnit.Framework;
using Shouldly;
using BusinessLogicEnums = FrameworkUAD.BusinessLogic.Enums;

namespace FrameworkUAD.UnitTests.Object
{
	[TestFixture]
	public class ProductSubscriptionTest
	{
		private const string StatusUpdatedReasonDefaultValue = "Subscribed";
		private const string DummyStringValue = "DummyString";
		private const int DummyIntValue = 10;

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
				() => productSubscription.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Active.ToString()),
				() => productSubscription.PubName.ShouldBe(string.Empty),
				() => productSubscription.PubCode.ShouldBe(string.Empty),
				() => productSubscription.PubTypeDisplayName.ShouldBe(string.Empty),
				() => productSubscription.PubID.ShouldBe(0),
				() => productSubscription.PubSubscriptionID.ShouldBe(0),
				() => productSubscription.SubscriptionStatusID.ShouldBe(1),
				() => productSubscription.Demo7.ShouldBe(string.Empty),
				() => productSubscription.Email.ShouldBe(string.Empty),
				() => productSubscription.ExternalKeyID.ShouldBe(0),
				() => productSubscription.FirstName.ShouldBe(string.Empty),
				() => productSubscription.LastName.ShouldBe(string.Empty),
				() => productSubscription.Company.ShouldBe(string.Empty),
				() => productSubscription.Title.ShouldBe(string.Empty),
				() => productSubscription.Occupation.ShouldBe(string.Empty),
				() => productSubscription.Address1.ShouldBe(string.Empty),
				() => productSubscription.Address2.ShouldBe(string.Empty),
				() => productSubscription.Address3.ShouldBe(string.Empty),
				() => productSubscription.City.ShouldBe(string.Empty),
				() => productSubscription.ZipCode.ShouldBe(string.Empty),
				() => productSubscription.Plus4.ShouldBe(string.Empty),
				() => productSubscription.CarrierRoute.ShouldBe(string.Empty),
				() => productSubscription.County.ShouldBe(string.Empty),
				() => productSubscription.Country.ShouldBe(string.Empty),
				() => productSubscription.Latitude.ShouldBe(0),
				() => productSubscription.Longitude.ShouldBe(0),
				() => productSubscription.Phone.ShouldBe(string.Empty),
				() => productSubscription.Fax.ShouldBe(string.Empty),
				() => productSubscription.Mobile.ShouldBe(string.Empty),
				() => productSubscription.Website.ShouldBe(string.Empty),
				() => productSubscription.Birthdate.ShouldBe(DateTimeFunctions.GetMinDate()),
				() => productSubscription.Age.ShouldBe(0),
				() => productSubscription.Income.ShouldBe(string.Empty),
				() => productSubscription.Gender.ShouldBe(string.Empty),
				() => productSubscription.PhoneExt.ShouldBe(string.Empty));
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
				Status = BusinessLogicEnums.EmailStatus.Inactive.ToString(),
				PubName = DummyStringValue,
				PubCode = DummyStringValue,
				PubTypeDisplayName = DummyStringValue,
				PubID = DummyIntValue,
				PubSubscriptionID = DummyIntValue,
				SubscriptionStatusID = DummyIntValue,
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
			productSubscription.ShouldSatisfyAllConditions(
				() => productSubscription.QualificationDate?.Date.ShouldBe(dummyDate),
				() => productSubscription.PubQSourceID.ShouldBe(DummyIntValue),
				() => productSubscription.PubCategoryID.ShouldBe(DummyIntValue),
				() => productSubscription.PubTransactionID.ShouldBe(DummyIntValue),
				() => productSubscription.StatusUpdatedDate.Date.ShouldBe(dummyDate),
				() => productSubscription.StatusUpdatedReason.ShouldBe(DummyStringValue),
				() => productSubscription.DateCreated.Date.ShouldBe(dummyDate),
				() => productSubscription.DateUpdated.ShouldBe(dummyDate),
				() => productSubscription.Status.ShouldBe(BusinessLogicEnums.EmailStatus.Inactive.ToString()),
				() => productSubscription.PubName.ShouldBe(DummyStringValue),
				() => productSubscription.PubCode.ShouldBe(DummyStringValue),
				() => productSubscription.PubTypeDisplayName.ShouldBe(DummyStringValue),
				() => productSubscription.PubID.ShouldBe(DummyIntValue),
				() => productSubscription.PubSubscriptionID.ShouldBe(DummyIntValue),
				() => productSubscription.SubscriptionStatusID.ShouldBe(DummyIntValue),
				() => productSubscription.Demo7.ShouldBe(DummyStringValue),
				() => productSubscription.Email.ShouldBe(DummyStringValue),
				() => productSubscription.ExternalKeyID.ShouldBe(DummyIntValue),
				() => productSubscription.FirstName.ShouldBe(DummyStringValue),
				() => productSubscription.LastName.ShouldBe(DummyStringValue),
				() => productSubscription.Company.ShouldBe(DummyStringValue),
				() => productSubscription.Title.ShouldBe(DummyStringValue),
				() => productSubscription.Occupation.ShouldBe(DummyStringValue),
				() => productSubscription.Address1.ShouldBe(DummyStringValue),
				() => productSubscription.Address2.ShouldBe(DummyStringValue),
				() => productSubscription.Address3.ShouldBe(DummyStringValue),
				() => productSubscription.City.ShouldBe(DummyStringValue),
				() => productSubscription.ZipCode.ShouldBe(DummyStringValue),
				() => productSubscription.Plus4.ShouldBe(DummyStringValue),
				() => productSubscription.CarrierRoute.ShouldBe(DummyStringValue),
				() => productSubscription.County.ShouldBe(DummyStringValue),
				() => productSubscription.Country.ShouldBe(DummyStringValue),
				() => productSubscription.Latitude.ShouldBe(DummyIntValue),
				() => productSubscription.Longitude.ShouldBe(DummyIntValue),
				() => productSubscription.Phone.ShouldBe(DummyStringValue),
				() => productSubscription.Fax.ShouldBe(DummyStringValue),
				() => productSubscription.Mobile.ShouldBe(DummyStringValue),
				() => productSubscription.Website.ShouldBe(DummyStringValue),
				() => productSubscription.Birthdate.ShouldBe(dummyDate),
				() => productSubscription.Age.ShouldBe(DummyIntValue),
				() => productSubscription.Income.ShouldBe(DummyStringValue),
				() => productSubscription.Gender.ShouldBe(DummyStringValue),
				() => productSubscription.PhoneExt.ShouldBe(DummyStringValue));
		}
	}
}