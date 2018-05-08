using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Fakes;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.DomainTracker.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class LinkFormTest : PageHelper
    {
        private const string AnyString = "AnyString";
        private const string UsePatchForDoubleKey = "UsePatchForDouble";
        private const string UrlWithConverstionTrack = "http://KM.com/%%ConversionTrkCDE%%";
        private const string UrlWithConverstionTrackWithQuery = "http://KM.com/%%ConversionTrkCDE%%?somQuery";
        private linkfrom _page;
        private PrivateObject _pagePrivate;
        private NameValueCollection _appSettings;
        private CampaignItem _campaignItem;
        private List<LinkTracking> _getByCampaignItemIdResult;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _campaignItem = new CampaignItem
            {
                CustomerID = GetNumber()
            };
            _appSettings = new NameValueCollection();
            _getByCampaignItemIdResult = new List<LinkTracking>();
            _page = new linkfrom();
            _pagePrivate = new PrivateObject(_page);
            InitializeAllControls(_page);
            CommonShims();
        }

        [Test]
        [TestCase(UrlWithConverstionTrack, AnyString, UrlWithConverstionTrack + "?" + AnyString)]
        [TestCase(UrlWithConverstionTrackWithQuery, AnyString, UrlWithConverstionTrackWithQuery + "&" + AnyString)]
        public void CreateRedirectLink_UsePatchForDoubleFalseAndHasLinkTracking_ShouldCreateUrl(
            string linkToStore,
            string trackingLink,
            string expectedResult)
        {
            // Arrange
            _appSettings.Add(UsePatchForDoubleKey, bool.FalseString);
            _page.LinkToStore = linkToStore;
            _page.EmailID = GetNumber();
            _page.BlastID = GetNumber();
            var emailId = $"eid={_page.EmailID}";
            var blastId = $"bid={_page.BlastID}";
            _getByCampaignItemIdResult.Add(new LinkTracking
            {
                LTID = GetNumber()
            });
            Shimlinkfrom.AllInstances.CreateTrackingLinkDataTableBoolean = (instance, table, convertionsTracking) =>
                trackingLink;
            // Act
            var result = CallCreateRedirectLink();
            // Assert
            result.ShouldContain(expectedResult);
        }

        [Test]
        [TestCase(UrlWithConverstionTrack, "?")]
        [TestCase(UrlWithConverstionTrackWithQuery, "&")]
        public void CreateRedirectLink_UsePatchForDoubleFalse_ShouldAddEmailIdToUrl(
            string linkToStore,
            string queryStringSeparator)
        {
            // Arrange
            _appSettings.Add(UsePatchForDoubleKey, bool.FalseString);
            _page.LinkToStore = linkToStore;
            _page.EmailID = GetNumber();
            _page.BlastID = GetNumber();
            var emailId = $"{queryStringSeparator}eid={_page.EmailID}";
            var blastId = $"bid={_page.BlastID}";
            // Act
            var result = CallCreateRedirectLink();
            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldContain(emailId),
                () => result.ShouldContain(blastId));
        }

        [Test]
        [TestCase(UrlWithConverstionTrack, AnyString, UrlWithConverstionTrack + "?" + AnyString)]
        [TestCase(UrlWithConverstionTrackWithQuery, AnyString, UrlWithConverstionTrackWithQuery + "&" + AnyString)]
        public void CreateRedirectLink_UsePatchForDoubleTrueAndHasLinkTracking_ShouldCreateUrl(
            string linkToStore,
            string trackingLink,
            string expectedResult)
        {
            // Arrange
            _appSettings.Add(UsePatchForDoubleKey, bool.TrueString);
            _page.LinkToStore = linkToStore;
            _page.EmailID = GetNumber();
            _page.BlastID = GetNumber();
            var emailId = $"eid={_page.EmailID}";
            var blastId = $"bid={_page.BlastID}";
            _getByCampaignItemIdResult.Add(new LinkTracking
            {
                LTID = GetNumber()
            });
            Shimlinkfrom.AllInstances.CreateTrackingLinkDataTableBoolean = (instance, table, convertionsTracking) =>
                trackingLink;
            // Act
            var result = CallCreateRedirectLink();
            // Assert
            result.ShouldContain(expectedResult);
        }

        [Test]
        [TestCase(UrlWithConverstionTrack, "?")]
        [TestCase(UrlWithConverstionTrackWithQuery, "&")]
        public void CreateRedirectLink_UsePatchForDoubleTrue_ShouldAddEmailIdToUrl(
            string linkToStore,
            string queryStringSeparator)
        {
            // Arrange
            _appSettings.Add(UsePatchForDoubleKey, bool.TrueString);
            _page.LinkToStore = linkToStore;
            _page.EmailID = GetNumber();
            _page.BlastID = GetNumber();
            var emailId = $"{queryStringSeparator}eid={_page.EmailID}";
            var blastId = $"bid={_page.BlastID}";
            // Act
            var result = CallCreateRedirectLink();
            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldContain(emailId),
                () => result.ShouldContain(blastId));
        }

        private string CallCreateRedirectLink()
        {
            return _pagePrivate.Invoke("CreateRedirectLink", new object[0]) as string;
        }
        private void CommonShims()
        {
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (id, includeChildren) => _campaignItem;
            ShimLinkTracking.GetByCampaignItemIDInt32 = campaignItemId => _getByCampaignItemIdResult;
            ShimLinkTracking.CreateLinkTrackingParamsInt32StringInt32 = (customerId, domain, ltid) => true;
            ShimCampaignItemLinkTracking.GetParamInfoInt32Int32 = (blastId, ltid) =>
            {
                var result = new DataTable();
                result.Rows.Add(result.NewRow());
                return result;
            };
            ShimCustomer.GetByCustomerIDInt32Boolean = (customerId, includeChildren) => new Customer
            {
                BaseChannelID = GetNumber()
            };
            ShimBaseChannel.GetByBaseChannelIDInt32 = id => new BaseChannel
            {
                PlatformClientGroupID = GetNumber()
            };
            ShimClientGroup.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures =
                (clientGroupId, serviceCode, featureCode) => true;
            ShimDomainTracker.ExistsStringInt32 = (domain, baseChannelId) => true;
            Shimlinkfrom.AllInstances.replaceUDFWithValueString = (instance, url) => url;
        }

        private int GetNumber()
        {
            return _random.Next(10, 100);
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
