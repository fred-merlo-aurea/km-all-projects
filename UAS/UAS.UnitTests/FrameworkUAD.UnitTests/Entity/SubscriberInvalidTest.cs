using System;
using FrameworkUAD.Entity;
using NUnit.Framework;
using Shouldly;

namespace FrameworkUAD.UnitTests.Entity
{
    [TestFixture]
    public class SubscriberInvalidTest: EntityFakes
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
        public void CtorWoParams_AppliedToProperties()
        {
            // Arrange, Act
            var subscriberInvalid = new SubscriberInvalid();

            // Assert
            subscriberInvalid.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberInvalid),

                () => subscriberInvalid.SORecordIdentifier.ShouldBe(Guid.Empty),
                () => subscriberInvalid.SourceFileID.ShouldBe(SubscriberBase.NoId),
                () => subscriberInvalid.ProcessCode.ShouldBe(string.Empty)
            );
        }

        [Test]
        public void Ctor2Params_AppliedToProperties()
        {
            // Arrange, Act
            var subscriberInvalid = new SubscriberInvalid(SourceFileIdSample, RecordIdentifierSample, ProcessCodeSample);

            // Assert
            subscriberInvalid.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberInvalid),

                () => subscriberInvalid.SORecordIdentifier.ShouldBe(RecordIdentifierSample),
                () => subscriberInvalid.SourceFileID.ShouldBe(SourceFileIdSample),
                () => subscriberInvalid.ProcessCode.ShouldBe(ProcessCodeSample)
            );
        }

        [Test]
        public void Ctor3Params_AppliedToProperties()
        {
            // Arrange
            var subscriberTransformed = new SubscriberTransformed
            {
                SORecordIdentifier = RecordIdentifierSample
            };

            // Act
            var subscriberInvalid = new SubscriberInvalid(subscriberTransformed);

            // Assert
            subscriberInvalid.ShouldSatisfyAllConditions(
                () => AssertCommon(subscriberInvalid),

                () => subscriberInvalid.SORecordIdentifier.ShouldBe(RecordIdentifierSample)
            );
        }

        private void AssertCommon(SubscriberInvalid subscriber)
        {
            subscriber.ShouldSatisfyAllConditions(
                () => subscriber.SubscriberInvalidID.ShouldBe(SubscriberBase.NoId),
                () => subscriber.PubCode.ShouldBe(string.Empty),
                () => subscriber.Sequence.ShouldBe(SubscriberBase.NoId),
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
                () => subscriber.StatList.ShouldBe(false),
                () => subscriber.Sic.ShouldBe(string.Empty),
                () => subscriber.SicCode.ShouldBe(string.Empty),
                () => subscriber.Gender.ShouldBe(string.Empty),
                () => subscriber.Address3.ShouldBe(string.Empty),
                () => subscriber.Home_Work_Address.ShouldBe(string.Empty),
                () => subscriber.Demo7.ShouldBe(string.Empty),
                () => subscriber.Mobile.ShouldBe(string.Empty),
                () => subscriber.Latitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.Longitude.ShouldBe(SubscriberBase.NoInt),
                () => subscriber.EmailStatusID.ShouldBe(SubscriberBase.StatusOne),
                () => subscriber.SIRecordIdentifier.ToString().ShouldNotBeNullOrWhiteSpace(string.Empty),
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
                () => subscriber.SubscriptionStatusID.ShouldBe(0),
                () => subscriber.SubsrcID.ShouldBe(0),
                () => subscriber.Website.ShouldBe(string.Empty),
                () => subscriber.MailPermission.ShouldBe(null),
                () => subscriber.FaxPermission.ShouldBe(null),
                () => subscriber.PhonePermission.ShouldBe(null),
                () => subscriber.OtherProductsPermission.ShouldBe(null),
                () => subscriber.ThirdPartyPermission.ShouldBe(null),
                () => subscriber.EmailRenewPermission.ShouldBe(null),
                () => subscriber.TextPermission.ShouldBe(null)
            );
        }
    }
}
