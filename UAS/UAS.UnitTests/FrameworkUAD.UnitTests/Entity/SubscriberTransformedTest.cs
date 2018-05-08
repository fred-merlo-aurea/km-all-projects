using System;
using FrameworkUAD.Entity;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class SubscriberTransformedTest : EntityFakes
    {
        [SetUp]
        public void Setup()
        {
            SetupFakes();
        }

        [TearDown]
        public void Teardown()
        {
            ReleaseFakes();
        }

        [Test]
        public void Ctor_WoParams_AppliedToProperties()
        {
            // Arrange, Act
            var subscriberTransformed = new SubscriberTransformed();

            // Assert
            subscriberTransformed.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberTransformed),
                () => subscriberTransformed.SORecordIdentifier.ShouldBe(Guid.Empty),
                () => subscriberTransformed.SourceFileID.ShouldBe(SubscriberBase.NoId),
                () => subscriberTransformed.ProcessCode.ShouldBe(string.Empty));
        }

        [Test]
        public void Ctor_WithParams_AppliedToProperties()
        {
            // Arrange, Act
            var subscriberTransformed = 
                new SubscriberTransformed(SourceFileIdSample, RecordIdentifierSample, ProcessCodeSample);

            // Assert
            subscriberTransformed.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberTransformed),
                () => subscriberTransformed.SORecordIdentifier.ShouldBe(RecordIdentifierSample),
                () => subscriberTransformed.SourceFileID.ShouldBe(SourceFileIdSample),
                () => subscriberTransformed.ProcessCode.ShouldBe(ProcessCodeSample));
        }

        private void AssertCommon(SubscriberTransformed subscriber)
        {
            subscriber.ShouldSatisfyAllConditions(
                () => subscriber.SubscriberTransformedID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.PubCode.ShouldBe(string.Empty),
                () => subscriber.Sequence.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.FName.ShouldBe(string.Empty),
                () => subscriber.LName.ShouldBe(string.Empty),
                () => subscriber.Title.ShouldBe(string.Empty),
                () => subscriber.Company.ShouldBe(string.Empty),
                () => subscriber.MailStop.ShouldBe(string.Empty),
                () => subscriber.City.ShouldBe(string.Empty),
                () => subscriber.State.ShouldBe(string.Empty),
                () => subscriber.Zip.ShouldBe(string.Empty),
                () => subscriber.Plus4.ShouldBe(string.Empty),
                () => subscriber.ForZip.ShouldBe(string.Empty),
                () => subscriber.County.ShouldBe(string.Empty),
                () => subscriber.Country.ShouldBe(string.Empty),
                () => subscriber.CountryID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.Phone.ShouldBe(string.Empty),
                () => subscriber.Fax.ShouldBe(string.Empty),
                () => subscriber.Email.ShouldBe(string.Empty),
                () => subscriber.CategoryID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.TransactionID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.TransactionDate.ShouldBe(MinDate),
                () => subscriber.QDate.ShouldBe(Now),
                () => subscriber.QSourceID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.RegCode.ShouldBe(string.Empty),
                () => subscriber.Verified.ShouldBe(string.Empty),
                () => subscriber.SubSrc.ShouldBe(string.Empty),
                () => subscriber.OrigsSrc.ShouldBe(string.Empty),
                () => subscriber.Par3C.ShouldBe(string.Empty),
                () => subscriber.Source.ShouldBe(string.Empty),
                () => subscriber.Priority.ShouldBe(string.Empty),
                () => subscriber.Sic.ShouldBe(string.Empty),
                () => subscriber.SicCode.ShouldBe(string.Empty),
                () => subscriber.Gender.ShouldBe(string.Empty),
                () => subscriber.Address3.ShouldBe(string.Empty),
                () => subscriber.Home_Work_Address.ShouldBe(string.Empty),
                () => subscriber.Demo7.ShouldBe(string.Empty),
                () => subscriber.Mobile.ShouldBe(string.Empty),
                () => subscriber.Latitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.Longitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.IsLatLonValid.ShouldBe(false),
                () => subscriber.LatLonMsg.ShouldBe(string.Empty),
                () => subscriber.EmailStatusID.ShouldBe(SubscriberBase.StatusOne),
                () => subscriber.STRecordIdentifier.ToString().ShouldNotBeNullOrWhiteSpace(),
                () => subscriber.DateCreated.ShouldBe(Now),
                () => subscriber.DateUpdated.ShouldBe(MinDate),
                () => subscriber.CreatedByUserID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.UpdatedByUserID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.ImportRowNumber.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.IsActive.ShouldBe(true),
                () => subscriber.ExternalKeyId.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.AccountNumber.ShouldBe(string.Empty),
                () => subscriber.EmailID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.Copies.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.GraceIssues.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.IsComp.ShouldBe(false),
                () => subscriber.IsPaid.ShouldBe(false),
                () => subscriber.IsSubscribed.ShouldBe(true),
                () => subscriber.Occupation.ShouldBe(string.Empty),
                () => subscriber.SubscriptionStatusID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SubsrcID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.Website.ShouldBe(string.Empty),
                () => subscriber.MailPermission.ShouldBe(null),
                () => subscriber.FaxPermission.ShouldBe(null),
                () => subscriber.PhonePermission.ShouldBe(null),
                () => subscriber.OtherProductsPermission.ShouldBe(null),
                () => subscriber.ThirdPartyPermission.ShouldBe(null),
                () => subscriber.EmailRenewPermission.ShouldBe(null),
                () => subscriber.TextPermission.ShouldBe(null),
                () => subscriber.OriginalImportRow.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.OriginalImportRow.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.SubGenSubscriberID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SubGenSubscriptionID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SubGenPublicationID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SubGenMailingAddressId.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SubGenBillingAddressId.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.IssuesLeft.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.UnearnedReveue.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.SubGenIsLead.ShouldBe(false),
                () => subscriber.SubGenRenewalCode.ShouldBe(string.Empty),
                () => subscriber.SubGenSubscriptionRenewDate.ShouldBe(null),
                () => subscriber.SubGenSubscriptionExpireDate.ShouldBe(null),
                () => subscriber.SubGenSubscriptionLastQualifiedDate.ShouldBe(null));
        }
    }
}
