using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FrameworkSubGen.Entity;
using NUnit.Framework;
using Shouldly;
using EntitySubscription = global::FrameworkSubGen.Entity.Subscription;
using Subscription = FrameworkSubGen.BusinessLogic.API.Subscription;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionTest
    {
        private EntitySubscription _entitySubscription;
        private NameValueCollection _nameValueCollection;
        private Subscription _subscription;
        private int? _subscriberId = null;

        private readonly DateTime DefaultDate = DateTime.Now;

        private const string AuditClassification = "AuditClassification";
        private const string AuditRequestType = "AuditRequestType";
        private const int BillingAddressId = 10;
        private const int Copies = 20;
        private const int Issues = 30;
        private const int LastPurchaseBundleId = 40;
        private const int MailingAddressId = 50;
        private const int PaidIssuesLeft = 60;
        private const int PublicationId = 70;
        private const int SubscriptionId = 80;
        private const double UnearnedRevenue = 90;
        private const bool RenewCampaignActive = true;
        private const Enums.SubscriptionType SubscriptionType = Enums.SubscriptionType.Both;

        [SetUp]
        public void Initialize()
        {
            _subscription = new Subscription();
            _entitySubscription = new EntitySubscription();
            _nameValueCollection = new NameValueCollection();

            _entitySubscription = new EntitySubscription
            {
                audit_classification = AuditClassification,
                audit_request_type = AuditRequestType,
                billing_address_id = BillingAddressId,
                copies = Copies,
                date_created = DefaultDate,
                date_expired = DefaultDate,
                date_last_renewed = DefaultDate,
                issues = Issues,
                last_purchase_bundle_id = LastPurchaseBundleId,
                mailing_address_id = MailingAddressId,
                paid_issues_left = PaidIssuesLeft,
                publication_id = PublicationId,
                renew_campaign_active = RenewCampaignActive,
                subscription_id = SubscriptionId,
                type = SubscriptionType,
                unearned_revenue = UnearnedRevenue
            };
        }

        [Test]
        public void MapNameValueCollectionParameters_WhenSubscriptionIsNull_ThrowsException()
        {
            // Arrange
            _entitySubscription = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                _subscription.MapNameValueCollectionParameters(_entitySubscription, _nameValueCollection, _subscriberId);
            });
        }

        [Test]
        public void MapNameValueCollectionParameters_WhenNameValueCollectionIsNull_ThrowsException()
        {
            // Arrange
            _nameValueCollection = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => { 
                _subscription.MapNameValueCollectionParameters(_entitySubscription, _nameValueCollection, _subscriberId);
            });
        }

        [Test]
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(1)]
        public void MapBillingContactParameters_ShouldMapAllParameters_ReturnsCommand(int testValue)
        {
            // Arrange
            _subscriberId = testValue;
            _entitySubscription.subscriber_id = testValue;

            // Act
            _subscription.MapNameValueCollectionParameters(_entitySubscription, _nameValueCollection, _subscriberId);

            // Assert
            _nameValueCollection.ShouldNotBeNull();
            _nameValueCollection.ShouldSatisfyAllConditions(
                () => GetPropertyValue("audit_classification").ShouldBe(AuditClassification),
                () => GetPropertyValue("audit_request_type").ShouldBe(AuditRequestType),
                () => GetPropertyValue("billing_address_id").ShouldBe(BillingAddressId.ToString()),
                () => GetPropertyValue("copies").ShouldBe(Copies.ToString()),
                () => GetPropertyValue("date_created").ShouldBe(DefaultDate.ToString(CultureInfo.CurrentCulture)),
                () => GetPropertyValue("date_expired").ShouldBe(DefaultDate.ToString(CultureInfo.CurrentCulture)),
                () => GetPropertyValue("date_last_renewed").ShouldBe(DefaultDate.ToString(CultureInfo.CurrentCulture)),
                () => GetPropertyValue("issues").ShouldBe(Issues.ToString()),
                () => GetPropertyValue("last_purchase_bundle_id").ShouldBe(LastPurchaseBundleId.ToString()),
                () => GetPropertyValue("mailing_address_id").ShouldBe(MailingAddressId.ToString()),
                () => GetPropertyValue("paid_issues_left").ShouldBe(PaidIssuesLeft.ToString()),
                () => GetPropertyValue("publication_id").ShouldBe(PublicationId.ToString()),
                () => GetPropertyValue("renew_campaign_active").ShouldBe(RenewCampaignActive.ToString()),
                () => GetPropertyValue("subscription_id").ShouldBe(SubscriptionId.ToString()),
                () => GetPropertyValue("type").ShouldBe(SubscriptionType.ToString()),
                () => GetPropertyValue("unearned_revenue").ShouldBe(UnearnedRevenue.ToString(CultureInfo.InvariantCulture)),
                () => GetPropertyValue("subscriber_id").ShouldBe(_subscriberId.ToString()));
        }

        private string GetPropertyValue(string key)
        {
            return _nameValueCollection.GetValues(key) != null
                ? _nameValueCollection.GetValues(key)?.FirstOrDefault()
                : string.Empty;
        }
    }
}
