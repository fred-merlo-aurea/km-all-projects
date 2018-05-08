using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Drawing.Fakes;
using System.Net.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using EmailPreview;
using EmailPreview.Fakes;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Fakes;
using Shouldly;
using CommEntities = ECN_Framework_Entities.Communicator;
using DrawingFakes = System.Drawing.Fakes;
using Encryption = KM.Common.Entity.Encryption;
using FakeCommData = ECN_Framework_DataLayer.Communicator.Fakes;
using KmCommFake = KM.Common.Fakes;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class BlastEmailPreviewTest
    {
        private const int BindPreviewApiIdCount = 3;
        private const string BindPreviewBlastId = "1";
        private const string BindPreviewAppLongName = "appLongName";
        private const string BindPreviewBlastIdQueryStringKey = "blastID";
        private const string BindPreviewDummyEncryptedString = "dummyEncrption";
        private const string BindPreviewNoEmailResultErrorMsg = "No results are available yet. Please wait 15 minutes and try again.  If the issue persists please contact ECN Customer Support.";
        private const string BindPreviewNoResultAvailableErrorMsg = "No results are available yet.";
        private const string BindPreviewPastEmailPreviewErrorMsg = "There is currently an issue with retrieving your results.  Please wait 5 minutes and try again.  If the issue persists please contact ECN Customer Support.";
        private const string BindPreviewCreatePublicLinkErrorMsg = "Unable to generate public URL at this time";

        [Test]
        public void BindPreview_WithOnlySpamEmailTestResults_ErrorMsg()
        {
            // Arrange
            InitTestingApplicationForBindPreviewWithOnlySpamEmail(out List<TestingApplication> testingApplications);
            InitTestForBindPreview(emailPreviews: CreateEmailPreviewsForBindPreview(), testingApplicationList: testingApplications);

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            _labelMessage.Text.ShouldBe(BindPreviewNoEmailResultErrorMsg);
        }

        [Test]
        public void BindPreview_WithEmailEmailTestResults_DataBound()
        {
            // Arrange
            InitTestingApplicationForBindPreviewWithEmail(out List<TestingApplication> testingApplications);
            InitTestForBindPreview(emailPreviews: CreateEmailPreviewsForBindPreview(), testingApplicationList: testingApplications);

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            var emails = _rptSideBar.DataSource as List<EmailResult>;
            var spams = _rptSpam.DataSource as List<EmailResult>;
            _labelMessage.ShouldSatisfyAllConditions(
               () => _labelMessage.Text.ShouldBeNullOrEmpty(),
               () => emails.ShouldNotBeNull(),
               () => spams.ShouldNotBeNull(),
               () => emails.Count.ShouldBe(1),
               () => spams.Count.ShouldBe(1),
               () => _litHtml.Text.ShouldNotBeEmpty());
        }

        [Test]
        public void BindPreview_NoTestResultNoEmailPreview_Error()
        {
            // Arrange
            var fakeEmailPreviewList = new List<CommEntities.EmailPreview>() { null };
            InitTestForBindPreview(emailPreviews: fakeEmailPreviewList, testingApplicationList: new List<TestingApplication>());

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            _labelMessage.Text.ShouldBe(BindPreviewNoResultAvailableErrorMsg);
        }

        [Test]
        public void BindPreview_NoTestResultWithFutureEmailPreview_Error()
        {
            // Arrange
            var emailPreviewList = CreateEmailPreviewsForBindPreview(dateCreated: DateTime.Now.AddDays(2), timeCreated: DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 1, 1)));
            InitTestForBindPreview(emailPreviews: emailPreviewList, testingApplicationList: new List<TestingApplication>());

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            _labelMessage.Text.ShouldContain(BindPreviewNoResultAvailableErrorMsg);
        }

        [Test]
        public void BindPreview_NoTestResultWithPastEmailPreview_Error()
        {
            // Arrange
            var emailPreviewList = CreateEmailPreviewsForBindPreview(dateCreated: DateTime.MinValue, timeCreated: TimeSpan.MinValue);
            InitTestForBindPreview(emailPreviews: emailPreviewList, testingApplicationList: new List<TestingApplication>());

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            _labelMessage.Text.ShouldBe(BindPreviewPastEmailPreviewErrorMsg);
        }

        [Test]
        public void BindPreview_CodeAnalysisTestExist_DataBound()
        {
            // Arrange
            InitTestingApplicationForBindPreviewWithEmail(out List<TestingApplication> testingApplications);
            var codeAnalysisResults = CreateCodeAnalysisResultForBindPreview();
            var codeAnalysisTests = CreateCodeAnalysisTestForBindPreview();
            var linkTest = CreateLinkTestForBindPreview();
            InitTestForBindPreview(
                emailPreviews: CreateEmailPreviewsForBindPreview(),
                codeAnalysisResults: codeAnalysisResults,
                codeAnalysisTest: codeAnalysisTests,
                testingApplicationList: testingApplications,
                linkTest: linkTest);

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            // Assert
            var codeAnalysisResultsDataSource = _rptPotentialProblems.DataSource as List<CodeAnalysisResult>;
            var htmlValidationDataSource = _rptCodeHtmlValidation.DataSource as List<CodeAnalysisHtmlValidation>;
            var linkCheckDataSource = _rptLinkCheck.DataSource as List<Link>;
            _plHtmlValidation.ShouldSatisfyAllConditions(
                () => codeAnalysisResultsDataSource.ShouldNotBeNull(),
                () => htmlValidationDataSource.ShouldNotBeNull(),
                () => linkCheckDataSource.ShouldNotBeNull(),
                () => codeAnalysisResultsDataSource.Count.ShouldBe(BindPreviewApiIdCount),
                () => htmlValidationDataSource.Count.ShouldBe(1),
                () => linkCheckDataSource.Count.ShouldBe(linkTest.Links.Count));
        }

        [Test]
        public void BindPreview_CreatePublicLinkException_Error()
        {
            // Arrange
            InitTestingApplicationForBindPreviewWithEmail(out List<TestingApplication> testingApplications);
            var codeAnalysisResults = CreateCodeAnalysisResultForBindPreview();
            var codeAnalysisTests = CreateCodeAnalysisTestForBindPreview();
            var linkTest = CreateLinkTestForBindPreview();
            InitTestForBindPreview(
                emailPreviews: CreateEmailPreviewsForBindPreview(),
                codeAnalysisResults: codeAnalysisResults,
                codeAnalysisTest: codeAnalysisTests,
                testingApplicationList: testingApplications,
                linkTest: linkTest);
            ShimEncryption.GetCurrentByApplicationIDInt32 = (id) => throw new Exception();

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            //Assert
            _labelPublicURL.Text.ShouldBe(BindPreviewCreatePublicLinkErrorMsg);
        }

        [Test]
        public void BindPreview_CreatePublicLink_NoError()
        {
            // Arrange
            InitTestingApplicationForBindPreviewWithEmail(out List<TestingApplication> testingApplications);
            var codeAnalysisResults = CreateCodeAnalysisResultForBindPreview();
            var codeAnalysisTests = CreateCodeAnalysisTestForBindPreview();
            var linkTest = CreateLinkTestForBindPreview();
            InitTestForBindPreview(
                emailPreviews: CreateEmailPreviewsForBindPreview(),
                codeAnalysisResults: codeAnalysisResults,
                codeAnalysisTest: codeAnalysisTests,
                testingApplicationList: testingApplications,
                linkTest: linkTest,
                shimEncryption: false);

            // Act
            _blastEmailPreviewPrivateObject.Invoke("BindPreview", new object[] { });

            //Assert
            _labelPublicURL.Text.ShouldContain(BindPreviewDummyEncryptedString);
        }

        private void InitTestForBindPreview(
            List<CommEntities.EmailPreview> emailPreviews = null,
            string previewHtml = "",
            CodeAnalysisTest codeAnalysisTest = null,
            List<CodeAnalysisResult> codeAnalysisResults = null,
            LinkTest linkTest = null,
            List<TestingApplication> testingApplicationList = null,
            bool shimEncryption = true)
        {
            InitCommonShimForBindPreview();
            SetPageControlsForBindPreview();
            SetPagePropertiesForBindPreview();
            Set(CodeAnalysisResultFieldName, codeAnalysisResults);
            FakeCommData.ShimEmailPreview.GetListSqlCommand = (cmd) => emailPreviews;
            FakeCommData.ShimBlast.GetByBlastIDInt32 = (id) => new CommEntities.BlastSMS() { LayoutID = 1 };
            ShimLayout.GetPreviewInt32EnumsContentTypeCodeBooleanUserNullableOfInt32NullableOfInt32NullableOfInt32 = (id, type, isMobile, user, emailID, groupID, blastid) => previewHtml;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, feature, view) => true;
            ShimAccessCheck.CanAccessByCustomerOf1M0User<CommEntities.BlastAbstract>((blastAbstract, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0EnumsServicesEnumsServiceFeaturesEnumsAccessUser<CommEntities.BlastFields>((fields, code, feature, access, user) => true);
            FakeCommData.ShimBlastFields.GetSqlCommand = (command) => CreateBlastFieldsForBindPreview();
            ShimPreview.AllInstances.GetCodeAnalysisTestString = (p, html) => codeAnalysisTest;
            ShimRestClient.AllInstances.ExecuteOf1IRestRequest((client, req) =>
            {
                var serializedLinkTest = JsonConvert.SerializeObject(linkTest);
                var response = new RestResponse<LinkTest>();
                response.Content = serializedLinkTest;
                return response;
            });
            ShimRestClient.AllInstances.ExecuteOf1IRestRequest((client, req) =>
            {
                var emailTest = new EmailTest();
                emailTest.TestingApplications = testingApplicationList;
                var serializedEmailTest = JsonConvert.SerializeObject(emailTest);
                var response = new RestResponse<EmailTest>();
                response.Content = serializedEmailTest;
                return response;
            });
            ShimRestClient.AllInstances.ExecuteOf1IRestRequest((client, req) =>
            {
                var testingAppList = new List<TestingApplication>();
                testingAppList.Add(new TestingApplication() { ApplicationName = "2" });
                var serializedTestingAppList = JsonConvert.SerializeObject(testingAppList);
                var response = new RestResponse<List<TestingApplication>>();
                response.Content = serializedTestingAppList;
                return response;
            });
            if (shimEncryption)
            {
                ShimEncryption.GetCurrentByApplicationIDInt32 = (id) => new Encryption();
            }
        }

        private void InitCommonShimForBindPreview()
        {
            ShimWebClient.AllInstances.DownloadDataString = (client, url) => new byte[2];
            DrawingFakes.ShimImage.FromStreamStream = (st) => new ShimBitmap();
            ShimGraphics.FromImageImage = (img) => new ShimGraphics();
            ShimBitmap.ConstructorStream = (bitmap, st) => { };
            DrawingFakes.ShimImage.AllInstances.SaveStreamImageFormat = (img, st, format) => { };
            KmCommFake.ShimEncryption.EncryptStringEncryption = (st, enc) => BindPreviewDummyEncryptedString;
            KmCommFake.ShimDataFunctions.ExecuteReaderSqlCommand = (cmd) => new ShimSqlDataReader();
            ShimSqlConnection.AllInstances.Open = (conn) => { };
            ShimSqlConnection.AllInstances.Close = (conn) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = (cmd) => new ShimSqlConnection();
        }

        private void SetPageControlsForBindPreview()
        {
            _labelMessage = InitField<Label>(LbMessageId);
            _mpeMessage = InitField<ModalPopupExtender>(MpeMessageId);
            _rptSideBar = InitField<Repeater>(RPTSideBarId);
            _rptSpam = InitField<Repeater>(RPTSpamId);
            _litHtml = InitField<Literal>(LitHtmlId);
            _rptPotentialProblems = InitField<Repeater>(RPTPotentialProblemsId);
            _rptCodeHtmlValidation = InitField<Repeater>(RPTCodeHtmlValidationId);
            _labelCodeAnalysisResult = InitField<Label>(LblCodeAnalysisResultId);
            _plHtmlValidation = InitField<PlaceHolder>(PLHtmlValidationId);
            _plHtmlValidation.Visible = false;
            _plPotentialProblems = InitField<PlaceHolder>(PLPotentialProblemsId);
            _plPotentialProblems.Visible = false;
            _labelLinkCheckResult = InitField<Label>(LblLinkCheckResultId);
            _rptLinkCheck = InitField<Repeater>(RPTLinkCheckId);
            _imageMap1 = InitField<ImageMap>(ImageMap1Id);
            _labelPublicURL = InitField<Label>(LblPublicURLId);
        }

        private void SetPagePropertiesForBindPreview()
        {
            var shimSession = new ShimECNSession();
            ShimECNSession.CurrentSession = () => shimSession;
            shimSession.Instance.CurrentUser = new User();
            var appSettings = new NameValueCollection();
            var queryString = new NameValueCollection();
            queryString[BindPreviewBlastIdQueryStringKey] = BindPreviewBlastId;
            appSettings.Add(AppSettingsLitmusAPIKey, AppSettingsLitmusAPIKeyValue);
            appSettings.Add(AppSettingsLitmusAPIPasswordKey, AppSettingsLitmusAPIPasswordValue);
            appSettings.Add(AppSettingsKMCommonApplicationKey, AppSettingsKMCommonApplicationValue);
            ShimHttpRequest.AllInstances.QueryStringGet = (request) => queryString;
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
        }

        private void InitTestingApplicationForBindPreviewWithOnlySpamEmail(out List<TestingApplication> testingApplicationList)
        {
            testingApplicationList = new List<TestingApplication>();
            var testingApp = CreateTestingApplicationForBindPreview(
                    applicationName: EmailResultEnum.EmailSpam.htmlvalidation.ToString(),
                    resultType: EmailResultEnum.ResultType.spam.ToString(),
                    businessOrPopular: false,
                    completed: false,
                    desktopClient: false,
                    foundInSpam: false,
                    supportsContentBlocking: false);
            testingApplicationList.Add(testingApp);
        }

        private void InitTestingApplicationForBindPreviewWithEmail(out List<TestingApplication> testingApplicationList)
        {
            testingApplicationList = new List<TestingApplication>();
            var testingApp1 = CreateTestingApplicationForBindPreview(
                    applicationName: EmailResultEnum.EmailSpam.htmlvalidation.ToString(),
                    resultType: EmailResultEnum.ResultType.email.ToString(),
                    businessOrPopular: false,
                    completed: false,
                    desktopClient: false,
                    foundInSpam: false,
                    supportsContentBlocking: false);
            var testingApp2 = CreateTestingApplicationForBindPreview(
                    applicationName: "dummyAppName",
                    resultType: EmailResultEnum.ResultType.spam.ToString(),
                    businessOrPopular: true,
                    completed: true,
                    desktopClient: true,
                    foundInSpam: true,
                    supportsContentBlocking: true);
            var testingApp3 = CreateTestingApplicationForBindPreview(
                   applicationName: EmailResultEnum.EmailSpam.htmlvalidation.ToString(),
                   resultType: EmailResultEnum.ResultType.spam.ToString());
            testingApplicationList.Add(testingApp1);
            testingApplicationList.Add(testingApp2);
            testingApplicationList.Add(testingApp3);
        }

        private List<CommEntities.EmailPreview> CreateEmailPreviewsForBindPreview(DateTime? dateCreated = null, TimeSpan? timeCreated = null)
        {
            var emailPreviews = new List<CommEntities.EmailPreview>();
            var emailPreview = new CommEntities.EmailPreview();
            emailPreview.DateCreated = dateCreated.HasValue 
                ? dateCreated.Value 
                : DateTime.MinValue;
            emailPreview.TimeCreated = timeCreated.HasValue
                ? timeCreated.Value 
                : TimeSpan.MinValue;
            emailPreviews.Add(emailPreview);
            return emailPreviews;
        }

        private TestingApplication CreateTestingApplicationForBindPreview(
            string applicationName,
            string resultType,
            bool? completed = null,
            bool? desktopClient = null,
            bool? foundInSpam = null,
            bool? supportsContentBlocking = null,
            bool? businessOrPopular = null)
        {
            var spamHeader = new SpamHeader()
            {
                Key = "spamHeaderKey",
                Description = "spamHeaderDesc"
            };
            var spamHeaderList = new List<SpamHeader>()
            {
                spamHeader
            };
            var testingApp = new TestingApplication();
            testingApp.ResultType = resultType;
            testingApp.ApplicationLongName = BindPreviewAppLongName;
            testingApp.ApplicationName = applicationName;
            testingApp.Completed = completed;
            testingApp.DesktopClient = desktopClient;
            testingApp.FoundInSpam = foundInSpam;
            testingApp.SupportsContentBlocking = supportsContentBlocking;
            testingApp.BusinessOrPopular = businessOrPopular;
            testingApp.SpamHeaders = spamHeaderList;
            return testingApp;
        }

        private CommEntities.BlastFields CreateBlastFieldsForBindPreview()
        {
            return new CommEntities.BlastFields()
            {
                Field1 = "field1",
                Field2 = "field2",
                Field3 = "field3",
                Field4 = "field4",
                Field5 = "field5",
            };
        }

        private CodeAnalysisTest CreateCodeAnalysisTestForBindPreview(int apiIdCount = BindPreviewApiIdCount)
        {
            var compatibilityProblems = new List<CodeAnalysisPotentialProblems>();
            var codeAnalysisPotentialProblems = new CodeAnalysisPotentialProblems();
            var htmlProplemsList = new List<CodeAnalysisHtmlValidation>()
            {
                new CodeAnalysisHtmlValidation()
            };
            var apiIds = new List<string>();
            for (var i = 1; i < BindPreviewApiIdCount + 1; i++)
            {
                apiIds.Add(i.ToString());
            }
            codeAnalysisPotentialProblems.ApiIds = apiIds;
            compatibilityProblems.Add(codeAnalysisPotentialProblems);
            var codeAnalysisTest = new CodeAnalysisTest()
            {
                CompatibilityRulesCount = 5,
                CompatibilityProblems = compatibilityProblems,
                HtmlProblems = htmlProplemsList
            };
            return codeAnalysisTest;
        }

        private List<CodeAnalysisResult> CreateCodeAnalysisResultForBindPreview()
        {
            var codeAnalysisResultList = new List<CodeAnalysisResult>();
            var codeAnalysisResult = new CodeAnalysisResult()
            {
                ApplicationID = "1",
                CodeAnalysisResultDetails = new List<CodeAnalysisResultDetail>()
            };
            codeAnalysisResultList.Add(codeAnalysisResult);
            return codeAnalysisResultList;
        }

        private LinkTest CreateLinkTestForBindPreview()
        {
            var linkTest = new LinkTest();
            var links = new List<Link>();
            links.Add(new Link() { IsValid = true });
            links.Add(new Link() { IsValid = false });
            linkTest.Links = links;
            return linkTest;
        }
    }
}
