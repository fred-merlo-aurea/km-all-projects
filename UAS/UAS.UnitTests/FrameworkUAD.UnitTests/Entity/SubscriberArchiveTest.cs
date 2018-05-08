using System;
using FrameworkUAD.Entity;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class SubscriberArchiveTest: EntityFakes
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
            var subscriber = new SubscriberArchive();

            // Assert
            subscriber.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriber),
                () => subscriber.SubscriberArchiveID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.SFRecordIdentifier.ShouldBe(Guid.Empty),
                () => subscriber.SourceFileID.ShouldBe(SubscriberBase.NoId),
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
                () => subscriber.CategoryID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.TransactionID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.QSourceID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.RegCode.ShouldBe(string.Empty),
                () => subscriber.Verified.ShouldBe(string.Empty),
                () => subscriber.SubSrc.ShouldBe(string.Empty),
                () => subscriber.OrigsSrc.ShouldBe(string.Empty),
                () => subscriber.Par3C.ShouldBe(string.Empty),
                () => subscriber.Source.ShouldBe(string.Empty),
                () => subscriber.Priority.ShouldBe(string.Empty),
                () => subscriber.IGrp_Cnt.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.CGrp_Cnt.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.Sic.ShouldBe(string.Empty),
                () => subscriber.SicCode.ShouldBe(string.Empty),
                () => subscriber.Gender.ShouldBe(string.Empty),
                () => subscriber.IGrp_Rank.ShouldBe(string.Empty),
                () => subscriber.CGrp_Rank.ShouldBe(string.Empty),
                () => subscriber.Address3.ShouldBe(string.Empty),
                () => subscriber.Home_Work_Address.ShouldBe(string.Empty),
                () => subscriber.Demo7.ShouldBe(string.Empty),
                () => subscriber.Mobile.ShouldBe(string.Empty),
                () => subscriber.Latitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.Longitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.CreatedByUserID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.UpdatedByUserID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.ImportRowNumber.ShouldBe(SubscriberBase.NoId),
                () => subscriber.ProcessCode.ShouldBe(string.Empty));
        }

        [Test]
        public void Ctor_WithParams_AppliedToProperties()
        {
            // Arrange, Act
            var subscriber = new SubscriberArchive(SourceFileIdSample, RecordIdentifierSample, ProcessCodeSample);

            // Assert
            subscriber.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriber),
                () => subscriber.SubscriberArchiveID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.SFRecordIdentifier.ShouldBe(RecordIdentifierSample),
                () => subscriber.SourceFileID.ShouldBe(SourceFileIdSample),
                () => subscriber.PubCode.ShouldBe(null),
                () => subscriber.Sequence.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.FName.ShouldBe(null),
                () => subscriber.LName.ShouldBe(null),
                () => subscriber.Title.ShouldBe(null),
                () => subscriber.Company.ShouldBe(null),
                () => subscriber.Address.ShouldBe(null),
                () => subscriber.MailStop.ShouldBe(null),
                () => subscriber.City.ShouldBe(null),
                () => subscriber.State.ShouldBe(null),
                () => subscriber.Zip.ShouldBe(null),
                () => subscriber.Plus4.ShouldBe(null),
                () => subscriber.ForZip.ShouldBe(null),
                () => subscriber.County.ShouldBe(null),
                () => subscriber.Country.ShouldBe(null),
                () => subscriber.CountryID.ShouldBe(0),
                () => subscriber.Phone.ShouldBe(null),
                () => subscriber.Fax.ShouldBe(null),
                () => subscriber.CategoryID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.TransactionID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.QSourceID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.RegCode.ShouldBe(null),
                () => subscriber.Verified.ShouldBe(null),
                () => subscriber.SubSrc.ShouldBe(null),
                () => subscriber.OrigsSrc.ShouldBe(null),
                () => subscriber.Par3C.ShouldBe(null),
                () => subscriber.Source.ShouldBe(null),
                () => subscriber.Priority.ShouldBe(null),
                () => subscriber.IGrp_Cnt.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.CGrp_Cnt.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.Sic.ShouldBe(null),
                () => subscriber.SicCode.ShouldBe(null),
                () => subscriber.Gender.ShouldBe(null),
                () => subscriber.IGrp_Rank.ShouldBe(null),
                () => subscriber.CGrp_Rank.ShouldBe(null),
                () => subscriber.Address3.ShouldBe(null),
                () => subscriber.Home_Work_Address.ShouldBe(null),
                () => subscriber.Demo7.ShouldBe(null),
                () => subscriber.Mobile.ShouldBe(null),
                () => subscriber.Latitude.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.Longitude.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.CreatedByUserID.ShouldBe(SubscriberBase.ZeroId),
                () => subscriber.UpdatedByUserID.ShouldBe(null),
                () => subscriber.ImportRowNumber.ShouldBe(SubscriberBase.ZeroInt),
                () => subscriber.ProcessCode.ShouldBe(ProcessCodeSample));
        }

        private void AssertCommon(SubscriberArchive subscriber)
        {
            subscriber.ShouldSatisfyAllConditions(
                () => subscriber.PhoneExists.ShouldBe(false),
                () => subscriber.FaxExists.ShouldBe(false),
                () => subscriber.Email.ShouldBe(string.Empty),
                () => subscriber.EmailExists.ShouldBe(false),
                () => subscriber.TransactionDate.ShouldBe(MinDate),
                () => subscriber.QDate.ShouldBe(Now),
                () => subscriber.IGrp_No.ToString().ShouldNotBeNullOrWhiteSpace(),
                () => subscriber.CGrp_No.ToString().ShouldNotBeNullOrWhiteSpace(),
                () => subscriber.StatList.ShouldBe(false),
                () => subscriber.EmailStatusID.ShouldBe(SubscriberBase.StatusOne),
                () => subscriber.IsMailable.ShouldBe(true),
                () => subscriber.SARecordIdentifier.ToString().ShouldNotBeNullOrWhiteSpace(string.Empty),
                () => subscriber.DateCreated.ShouldBe(Now),
                () => subscriber.DateUpdated.ShouldBe(MinDate),
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
                () => subscriber.SubsrcID.ShouldBe(0),
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
