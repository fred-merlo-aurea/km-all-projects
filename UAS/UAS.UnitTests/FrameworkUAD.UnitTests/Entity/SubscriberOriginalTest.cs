using FrameworkUAD.Entity;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class SubscriberOriginalTest: EntityFakes
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
            var subscriberOriginal = new SubscriberOriginal();

            // Assert
            subscriberOriginal.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberOriginal),
                () => subscriberOriginal.SourceFileID.ShouldBe(SubscriberBase.NoId),
                () => subscriberOriginal.ImportRowNumber.ShouldBe(SubscriberBase.NoInt),
                () => subscriberOriginal.ProcessCode.ShouldBe(string.Empty));
        }

        [Test]
        public void Ctor_WithParams_AppliedToProperties()
        {
            // Arrange, Act
            var subscriberOriginal = new SubscriberOriginal(SourceFileIdSample, RowNumberSample, ProcessCodeSample);

            // Assert
            subscriberOriginal.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberOriginal),
                () => subscriberOriginal.SourceFileID.ShouldBe(SourceFileIdSample),
                () => subscriberOriginal.ImportRowNumber.ShouldBe(RowNumberSample),
                () => subscriberOriginal.ProcessCode.ShouldBe(ProcessCodeSample));
        }

        private void AssertCommon(SubscriberOriginal subscriber)
        {
            subscriber.ShouldSatisfyAllConditions(
               () => subscriber.SubscriberOriginalID.ShouldBe(SubscriberBase.NoId),
               () => subscriber.PubCode.ShouldBe(string.Empty),
               () => subscriber.Sequence.ShouldBe(SubscriberBase.NoInt),
               () => subscriber.FName.ShouldBe(string.Empty),
               () => subscriber.LName.ShouldBe(string.Empty),
               () => subscriber.Title.ShouldBe(string.Empty),
               () => subscriber.Company.ShouldBe(string.Empty),
               () => subscriber.Address.ShouldBe(string.Empty),
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
               () => subscriber.Score.ShouldBe(SubscriberBase.NoInt),
               () => subscriber.EmailStatusID.ShouldBe(SubscriberBase.StatusOne),
               () => subscriber.SORecordIdentifier.ToString().ShouldNotBeNullOrWhiteSpace(),
               () => subscriber.DateCreated.ShouldBe(Now),
               () => subscriber.DateUpdated.ShouldBe(MinDate),
               () => subscriber.CreatedByUserID.ShouldBe(SubscriberBase.NoId),
               () => subscriber.UpdatedByUserID.ShouldBe(SubscriberBase.NoId),
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
               () => subscriber.TextPermission.ShouldBe(null));
        }
    }
}
