using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Common.Fakes;
using KMPlatform;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Action = System.Action;
using CustomerEntity = ECN_Framework_Entities.Accounts.Customer;
using LayoutEntity = ECN_Framework_Entities.Communicator.Layout;
using ShimCustomer = ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer;
using ShimDataFunctions = ecn.common.classes.Fakes.ShimDataFunctions;
using SubscriptionManagementEntity = ECN_Framework_Entities.Accounts.SubscriptionManagement;

namespace ECN.Tests.Common
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TemplateFunctionsTests
    {
        private const string HtmlReplaceWithLinkIdMethodName = "HtmlReplaceWithLinkId";
        private const string ExecuteScalarStringResult = "%%email_friend%%";

        private IDisposable _context;
        private string _customerId;
        private string _virtualPath;
        private string _hostName;
        private string _forwardFriend;
        private Blast _blast;
        private PrivateType _privateType;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;
        private readonly NameValueCollection _appSettings = new NameValueCollection
        {
            ["Activity_DomainPath"] = string.Empty,
            ["OpenClick_UseOldSite"] = Boolean.TrueString,
            ["MVCActivity_DomainPath"] = string.Empty,
            ["Image_DomainPath"] = string.Empty
        };

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            _privateType = new PrivateType(typeof(TemplateFunctions));
            _blast = GetBlastEntity();
            _hostName = "host";
            _customerId = "100";
            _virtualPath = string.Empty;
            _forwardFriend = string.Empty;
        }

        [TearDown]
        public void Teardown()
        {
            _context?.Dispose();
            _fakeHttpContext?.Dispose();
        }

        [Test]
        public void LinkReWriter_LogStatisticsTrue_StatisticsAreLogged()
        {
            // Arrange
            var text = "text";
            _customerId = string.Empty;
            _hostName = string.Empty;
            
            var actualNumberOfLogStatisticsCalls = 0;
            var expectedNumberOfLogStatisticsCalls = 2;
            SetupGlobalShims(
                true,
                () => actualNumberOfLogStatisticsCalls++);

            // Act
            var actualResult = TemplateFunctions.LinkReWriter(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualNumberOfLogStatisticsCalls.ShouldBe(expectedNumberOfLogStatisticsCalls));            
        }

        [Test]
        public void LinkReWriter_ValidInput_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims();
            var text = GetInputText();
            var expectedResult = GetOutputText(_blast);

            // Act
            var actualResult = TemplateFunctions.LinkReWriter(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriter_HasProductFeature_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims(false, null, true, true, true, true);
            var text = GetInputText();
            text = $"{text} href=\"mailto: href='mailto: href=mailto:";

            var mailTo = $"href=\"/engines/linkfrom.aspx?b={_blast.BlastID}&e=%%EmailID%%&l=mailto:";
            var profilePreferencesLink =
                "<a href='/engines/managesubscriptions.aspx?e=%%EmailAddress%%," +
                "%%EmailID%%&prefrence=list'>Manage my Subscriptions</a>&nbsp;";
            var userProfilePreferencesLink = 
                "<a href='/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%," +
                $"{_blast.GroupID}&prefrence=email'><img border=\"0\" " +
                "src=\"/channels/123/images//email_pref.gif\"></a>&nbsp;";
            var listProfilePreferencesLink =
                "<a href='/engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%" +
                "&prefrence=both'><img border=\"0\" src=\"/channels/123/images//list_email_pref.gif\"></a>&nbsp;";
            var expectedResult = GetOutputText(
                _blast,
                profilePreferencesLink,
                userProfilePreferencesLink,
                listProfilePreferencesLink);
            expectedResult = $"{expectedResult} {mailTo} {mailTo} {mailTo}";

            // Act
            var actualResult = TemplateFunctions.LinkReWriter(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriter_ForwardFriendNotEmpty_ReturnsParsedText()
        {
            // Arrange
            _forwardFriend = "forward_friend";

            SetupGlobalShims();
            var text = GetInputText();
            var expectedResult = $"{_forwardFriend}{GetOutputText(_blast)}";

            // Act
            var actualResult = TemplateFunctions.LinkReWriter(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriter_SubscriptionManagementInBody_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims();
            var text = GetInputText();
            text = $"{text} ECN.SUBSCRIPTIONMGMT.test.ECN.SUBSCRIPTIONMGMT";

            var subManagementLink = "<a href='/engines/subscriptionmanagement.aspx?smid=18&" +
                                    "e=%%EmailAddress%%'>Unsubscribe</a>";
            var expectedResult = $"{GetOutputText(_blast)} {subManagementLink}";

            // Act
            var actualResult = TemplateFunctions.LinkReWriter(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriterText_LogStatisticsTrue_StatisticsAreLogged()
        {
            // Arrange
            var text = "text";
            _customerId = string.Empty;
            _hostName = string.Empty;
            
            var actualNumberOfLogStatisticsCalls = 0;
            var expectedNumberOfLogStatisticsCalls = 2;
            SetupGlobalShims(
                true,
                () => actualNumberOfLogStatisticsCalls++);

            // Act
            var actualResult = TemplateFunctions.LinkReWriterText(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualNumberOfLogStatisticsCalls.ShouldBe(expectedNumberOfLogStatisticsCalls));
        }

        [Test]
        public void LinkReWriterText_ValidInput_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims();
            var text = GetInputText();
            var expectedResult = GetOutputText(_blast);

            // Act
            var actualResult = TemplateFunctions.LinkReWriterText(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriterText_HasProductFeature_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims(false, null, true, true, true, true);
            var text = GetInputText();
            text = $"{text} href=\"mailto: href='mailto: href=mailto: <http://test1 <https://test2";

            var mailTo = $"href=\"/engines/linkfrom.aspx?b={_blast.BlastID}&e=%%EmailID%%&l=mailto:";
            var profilePreferencesLink =
                "List Preferences: /engines/managesubscriptions.aspx?e=%%EmailAddress%%,%%EmailID%%&prefrence=list";
            var userProfilePreferencesLink =
                "Email Profile Preferences:/engines/managesubscriptions.aspx?e=%%EmailAddress%%," +
                $"%%EmailID%%,{_blast.GroupID}&prefrence=email";
            var listProfilePreferencesLink =
                "Email List and Profile Preferences: /engines/managesubscriptions.aspx?" +
                "e=%%EmailAddress%%,%%EmailID%%&prefrence=both";
            var redirectPage = $"</engines/linkfrom.aspx?b={_blast.BlastID}&e=ECN_EmailID&l=";
            var expectedResult = GetOutputText(
                _blast,
                profilePreferencesLink,
                userProfilePreferencesLink,
                listProfilePreferencesLink);
            expectedResult = $"{expectedResult} {mailTo} {mailTo} {mailTo} " +
                             $"{redirectPage}http://test1 {redirectPage}https://test2";

            // Act
            var actualResult = TemplateFunctions.LinkReWriterText(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriterText_ForwardFriendNotEmpty_ReturnsParsedText()
        {
            // Arrange
            _forwardFriend = "forward_friend";

            SetupGlobalShims();
            var text = GetInputText();
            var expectedResult = $"{_forwardFriend}{GetOutputText(_blast)}";

            // Act
            var actualResult = TemplateFunctions.LinkReWriterText(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void LinkReWriterText_SubscriptionManagementInBody_ReturnsParsedText()
        {
            // Arrange
            SetupGlobalShims();
            var text = GetInputText();
            text = $"{text} ECN.SUBSCRIPTIONMGMT.test.ECN.SUBSCRIPTIONMGMT";

            var subManagementLink = "Unsubscribe: /engines/subscriptionmanagement.aspx?smid=18&e=%%EmailAddress%%";
            var expectedResult = $"{GetOutputText(_blast)} {subManagementLink}";

            // Act
            var actualResult = TemplateFunctions.LinkReWriterText(
                text,
                _blast,
                _customerId,
                _virtualPath,
                _hostName,
                _forwardFriend);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.ShouldBe(expectedResult));
        }

        [Test]
        public void HtmlReplaceWithLinkId_OnEmptyString_ReturnEmptyString()
        {
            // Arrange
            var text = string.Empty;
            const int blastId = 1;

            // Act
            var actualResult = _privateType.InvokeStatic(HtmlReplaceWithLinkIdMethodName, text, blastId) as string;

            // Assert
            actualResult.ShouldBeNullOrWhiteSpace();
        }

        [Test]
        [TestCase(
            "<a href=\"http://www.google.com&l=test\" ecn_id=\"F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4\">Test</a>",
            "<a  href=\"http://www.google.com&lid=11&ulid=22&l=test\"  ecn_id=\"F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4\">Test</a>",
            "F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4")]
        [TestCase(
            "<a href=\"http://www.google.com&l=test\" ecn_id=\"\">Test</a>",
            "<a  href=\"http://www.google.com&lid=11&l=test\"  ecn_id=\"\">Test</a>",
            "")]
        [TestCase(
            "<a href=\"http://www.google.com&l=test&lid=8\" ecn_id=\"\">Test</a>",
            "<a  href=\"http://www.google.com&l=test&lid=8\"  ecn_id=\"\">Test</a>",
            "")]
        public void HtmlReplaceWithLinkId_DifferentInputs_ReturnsValidResult(
            string inputText, 
            string expectedResult, 
            string ecnId)
        {
            // Arrange
            const int blastId = 1;
            const int blastLinkId = 11;
            const int uniqueLinkId = 22;
            var linkArgument = "test";

            SetupShimsForHtmlReplaceWithLinkId(linkArgument, ecnId, blastLinkId, uniqueLinkId);

            // Act
            var actualResult = _privateType.InvokeStatic(HtmlReplaceWithLinkIdMethodName, inputText, blastId) as string;

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        [Test]
        public void HtmlReplaceWithLinkId_ValidInputButNoUniqueLinksFound_ReturnsValidResult()
        {
            // Arrange
            const int blastId = 1;
            const int blastLinkId = 11;
            const int uniqueLinkId = 22;
            var linkArgument = "test2";
            var ecnId = Guid.NewGuid().ToString();
            var inputText = $"<a href=\"http://www.google.com&l=test\" ecn_id=\"{ecnId}\">Test</a>";
            var expectedResult = $"<a  href=\"http://www.google.com&lid=1&ulid=55&l=test\"  ecn_id=\"{ecnId}\">Test</a>";

            SetupShimsForHtmlReplaceWithLinkId(linkArgument, ecnId, blastLinkId, uniqueLinkId);
            ShimUniqueLink.GetDictionaryByBlastIDInt32 = _ => new Dictionary<string, int>();
            ShimUniqueLink.SaveUniqueLink = (link) => 55;

            // Act
            var actualResult = _privateType.InvokeStatic(HtmlReplaceWithLinkIdMethodName, inputText, blastId) as string;

            // Assert
            actualResult.ShouldBe(expectedResult);
        }

        private static string GetInputText()
        {
            return @"%%groupname%% %%customer_name%% %%customer_address%% %%customer_webaddress%% " +
                   "%%customer_udf1%% %%customer_udf2%% %%customer_udf3%% %%customer_udf4%% %%customer_udf5%% " +
                   "%%hostname%% http://%%unsubscribelink%%/ http://%%emailtofriend%%/ http://%%publicview%%/ " +
                   "http://%%reportabuselink%%/ %%publicview%%/ http://%%unsubscribelink%% http://%%emailtofriend%% " +
                   "http://%%publicview%% http://%%reportabuselink%% %%profilepreferences%% " +
                   "%%userprofilepreferences%% %%listprofilepreferences%% %%company_address%% %%EmailFromAddress%% " +
                   "%%blast_start_time%% %%blast_end_time%% %%blast_id%%";
        }

        private static string GetOutputText(
            Blast blast, 
            string profilePreferencesLink = "",
            string userProfilePreferencesLink = "",
            string listProfilePreferencesLink = "")
        {
            var unsubscribeLink =
                $"/engines/Unsubscribe.aspx?e=%%EmailAddress%%&g=%%GroupID%" +
                $"%&b={blast.BlastID}&c={blast.CustomerID}&s=U&f=html";
            var publicViewLink = $"/engines/publicPreview.aspx?blastID={blast.BlastID}&emailID=%%EmailID%%";
            var reportAbuseLink =
                $"/engines/reportspam.aspx?p=%%EmailAddress%%,%%EmailID%%," +
                $"{blast.GroupID.Value},{blast.CustomerID},{blast.BlastID}";
            var emailToFriendLink = $"/engines/emailtofriend.aspx?e=%%EmailID%%&b={blast.BlastID}";
            var expectedResult =
                @"group_name customer_name customer_address, customer_city, " +
                "customer_state - customer_zip customer_webaddress " +
                "customer_udf1 customer_udf2 customer_udf3 customer_udf4 customer_udf5 " +
                $"host {unsubscribeLink} {emailToFriendLink} {publicViewLink} " +
                $"{reportAbuseLink} {publicViewLink} {unsubscribeLink} {emailToFriendLink} " +
                $"{publicViewLink} {reportAbuseLink} {profilePreferencesLink} {userProfilePreferencesLink} " +
                $"{listProfilePreferencesLink} layout_displayAddress email_from@email.com " +
                $"{blast.SendTime} {blast.FinishTime} {blast.BlastID}";
            return expectedResult;
        }

        private static Blast GetBlastEntity()
        {
            return new Blast
            {
                BlastID = 11,
                CustomerID = 22,
                GroupID = 33,
                LayoutID = 44,
                EmailFrom = "email_from@email.com",
                SendTime = DateTime.MinValue,
                FinishTime = DateTime.MinValue
            };
        }

        private void SetupGlobalShims(
            bool logStatistics = false,
            Action logStatisticsAction = null,
            bool hasProductFeatureForTrackmailtoClickThru = false,
            bool hasProductFeatureForEmailListPreferences = false,
            bool hasProductFeatureForEmailProfilePreferences = false,
            bool hasProductFeatureForEmailListandProfilePreferences = false)
        {
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimSubscriptionManagement.GetByBaseChannelIDInt32 = _ => new List<SubscriptionManagementEntity>
            {
                new SubscriptionManagementEntity
                {
                    Name = "test",
                    SubscriptionManagementID = 18
                }
            };
            ShimDataFunctions.ExecuteScalarString = _ => ExecuteScalarStringResult;
            ShimLoggingFunctions.LogStatistics = () => logStatistics;
            ShimFileFunctions.LogActivityBooleanStringString = (_, __, ___) => logStatisticsAction?.Invoke();
            ShimTemplateFunctions.HtmlReplaceWithLinkIdStringInt32 = (_, __) => _;
            ShimTemplateFunctions.TextReplaceWithLinkIDStringInt32 = (_, __) => _;

            var group = new Group
            {
                GroupName = "group_name"
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = _ => group;

            var customer = new CustomerEntity
            {
                BaseChannelID = 123,
                CustomerName = "customer_name",
                Address = "customer_address",
                City = "customer_city",
                Zip = "customer_zip",
                State = "customer_state",
                WebAddress = "customer_webaddress",
                customer_udf1 = "customer_udf1",
                customer_udf2 = "customer_udf2",
                customer_udf3 = "customer_udf3",
                customer_udf4 = "customer_udf4",
                customer_udf5 = "customer_udf5"
            };
            ShimCustomer.GetByCustomerIDInt32Boolean = (_, __) => customer;

            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (_, __, features) =>
            {
                if (features == Enums.ServiceFeatures.TrackmailtoClickThru)
                {
                    return hasProductFeatureForTrackmailtoClickThru;
                }

                if (features == Enums.ServiceFeatures.EmailListPreferences)
                {
                    return hasProductFeatureForEmailListPreferences;
                }

                if (features == Enums.ServiceFeatures.EmailProfilePreferences)
                {
                    return hasProductFeatureForEmailProfilePreferences;
                }

                return hasProductFeatureForEmailListandProfilePreferences;
            };

            var layout = new LayoutEntity
            {
                DisplayAddress = "layout_displayAddress"
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (_, __) => layout;
        }

        private static void SetupShimsForHtmlReplaceWithLinkId(
            string linkArgument, 
            string ecnId, 
            int blastLinkId, 
            int uniqueLinkId)
        {
            ShimBlastLink.GetDictionaryByBlastIDInt32 = _ => new Dictionary<string, int>
            {
                [linkArgument] = blastLinkId
            };
            ShimUniqueLink.GetDictionaryByBlastIDInt32 = _ => new Dictionary<string, int>
            {
                [ecnId] = uniqueLinkId
            };
            ShimBlastLink.InsertBlastLink = (link) => 1;
        }
    }
}
