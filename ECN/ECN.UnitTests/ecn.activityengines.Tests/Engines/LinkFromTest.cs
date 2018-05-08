using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using System.Xml.Linq;
using ecn.activityengines.Fakes;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KM.Common.Utilities.Email.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    /// <summary>
    /// Unit test for <see cref="linkfrom"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class LinkFromTest : PageHelper
    {
        private const string CreateTrackingLink = "CreateTrackingLink";
        private const string TestedMethod_Redirect = "Redirect";
        private const string TestedMethod_NotifyOfMissingTopicUDF = "NotifyOfMissingTopicUDF";
        private const string TestedMethod_LogTransactionalUDF = "LogTransactionalUDF";
        private const string TestedMethod_TrackData = "TrackData";
        private const string TestedMethod_replaceUDFWithValue = "replaceUDFWithValue";
        private const string OmnitureDefault = "omniture";
        private const string BlastId = "blastid";
        private const int BlastIdValue = 1;
        private const string GroupName = "groupname";
        private const string FolderName="FolderName";
        private const int GroupNameValue = 2;
        private const string SamleGroup = "SamleGroup";
        private const string LinkToStore = "http://km.all.com?a=test";
        private const string KMCommonApplication = "KMCommon_Application";
        private const string KMCommonApplicationValue = "1";
        private const string AdmimNotifyKey = "Admin_Notify";
        private const string AdmimNotifyValue = "true";
        private const string AdminToEmailConfigurationKey = "Admin_ToEmail";        
        private const string AdminFromEmailConfigurationKey = "Admin_FromEmail";
        private const string DummyString = "dummyStringValue";
        private const string LinkDummyValue = "http://km.all.com/linkDummyVal";
        private const string DummyEmailAddress= "email@email.com";
        private const string PrivateFieldNameLinkToStore = "LinkToStore";
        private const string PrivateFieldNameUserAgent = "UserAgent";
        private const string PrivateFieldNameDecodeURL = "DecodeURL";
        private const int SamleGroupValue = 3;
        private const int DummyInt = 100;
        private linkfrom _linkfrom;
        private PrivateObject _privateObject;        

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _linkfrom = new linkfrom();
            _privateObject = new PrivateObject(_linkfrom);
            base.InitializeAllControls(_linkfrom);
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => { return new CampaignItem { CustomerID = 1 }; };
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => { return new Customer { BaseChannelID = 1, CustomerID = 1 }; };

            var appSettings = new NameValueCollection();
            appSettings.Add(KMCommonApplication, KMCommonApplicationValue);
            appSettings.Add(AdmimNotifyKey, AdmimNotifyValue);
            appSettings.Add(AdminToEmailConfigurationKey, DummyEmailAddress);
            appSettings.Add(AdminFromEmailConfigurationKey, DummyEmailAddress);
            ShimConfigurationManager.AppSettingsGet = () => { return appSettings; };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32
                = (error, sourceMethod, applicationId, note, charityId, customerId) => { return 1; };
        }
        
        [TestCase("9", "-1", true)]
        [TestCase("9", "1", true)]
        [TestCase("9", "2", false)]
        [TestCase("9", "3", false)]
        [TestCase("10", "-1", true)]
        [TestCase("10", "1", true)]
        [TestCase("10", "2", false)]
        [TestCase("10", "3", false)]
        [TestCategory("CreateTrackingLink")]
        public void CreateTrackingLink_ConversionTrackingExistsIsTrue_ReturnsResultObject(string omnitureValue, string ltpoid, bool allowCustomerOverride)
        {
            // Arrange
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { XMLConfig = CreateSettingXml(allowCustomerOverride) };
            };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { XMLConfig = CreateSettingXml(allowCustomerOverride) };
            };
            DataTable dataTable = CreateDataTableObjectForOmitor(string.Concat(OmnitureDefault, omnitureValue), ltpoid);
            var conversionTrackingExists = true;
            var parameters = new object[] { dataTable, conversionTrackingExists };
            ShimBlast.GetLinkTrackingParamInt32String = (blastID, param) =>
            {
                if (param == GroupName)
                {
                    throw new Exception();
                }
                return param;
            };
            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = (ltPoId) =>
            {
                return CreateLinkTrackingParamOptionObject(ltPoId);
            };

            // Act
            var result = _privateObject.Invoke(CreateTrackingLink, parameters) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNullOrEmpty(),
                 () => result.ShouldNotBeNullOrWhiteSpace()
             );
        }

        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCategory("CreateTrackingLink")]
        public void CreateTrackingLink_OmnitureNotExistInDataTable_ReturnsResultObject(bool creatXmlConfile, bool isDefaultSettingNode)
        {
            // Arrange
            var xmlConfig = creatXmlConfile ? CreateSettingXml(false, isDefaultSettingNode) : string.Empty;
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { XMLConfig = xmlConfig };
            };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
            {
                return new LinkTrackingSettings { XMLConfig = xmlConfig };
            };
            DataTable dataTable = CreateDataTableObject();
            _linkfrom.LinkToStore = LinkToStore;
            var conversionTrackingExists = false;
            var parameters = new object[] { dataTable, conversionTrackingExists };
            ShimBlast.GetLinkTrackingParamInt32String = (blastID, param) =>
            {
                if (param == FolderName)
                {
                    throw new Exception();
                }
                return param;
            };
            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = (ltPoId) =>
            {
                return CreateLinkTrackingParamOptionObject(ltPoId);
            };

            // Act
            var result = _privateObject.Invoke(CreateTrackingLink, parameters) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldNotBeNullOrWhiteSpace()
            );
        }

        [Test]
        [TestCategory("Redirect")]
        public void LinkFrom_Redirect_WithAnchorHash_WithSuccess()
        {
            //Arrange
            var expectedValue = LinkDummyValue;
            var linkToStoreValue = DummyString + "#" + expectedValue;
            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, linkToStoreValue);
            Shimlinkfrom.AllInstances.CreateRedirectLink = (linkFrom) => DummyString;

            //Act
            _privateObject.Invoke(TestedMethod_Redirect);

            //Asert
            ResponseWriteText.ShouldSatisfyAllConditions(() => ResponseWriteText.ShouldNotBeNullOrEmpty(),
                                                         () => ResponseWriteText.ShouldContain(expectedValue));
        }

        [Test]
        [TestCategory("Redirect")]
        public void LinkFrom_Redirect_WithAnchorPercent_WithSuccess()
        {
            //Arrange
            var expectedValue = LinkDummyValue;
            var linkToStoreValue = DummyString + "%23" + expectedValue;
            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, linkToStoreValue);
            Shimlinkfrom.AllInstances.CreateRedirectLink = (linkFrom) => DummyString;

            //Act
            _privateObject.Invoke(TestedMethod_Redirect);

            //Asert
            ResponseWriteText.ShouldSatisfyAllConditions(() => ResponseWriteText.ShouldNotBeNullOrEmpty(),
                                                        () => ResponseWriteText.ShouldContain(expectedValue));
        }

        [Test]
        [TestCategory("Redirect")]
        public void LinkFrom_Redirect_WithMailToLink_WithSuccess()
        {
            //Arrange
            var expectedValue = DummyEmailAddress;
            var linkToStoreValue = "mailto:" + expectedValue;            
            Shimlinkfrom.AllInstances.CreateRedirectLink = (linkFrom) => linkToStoreValue;

            //Act
            _privateObject.Invoke(TestedMethod_Redirect);

            //Asert
            ResponseWriteText.ShouldSatisfyAllConditions(() => ResponseWriteText.ShouldNotBeNullOrEmpty(),
                                                        () => ResponseWriteText.ShouldContain(expectedValue));
        }

        [Test]
        [TestCategory("Redirect")]
        public void LinkFrom_Redirect_WithBlackBerryUserAgent_WithSuccess()
        {
            //Arrange
            var expectedValue = LinkDummyValue;
            var linkToStoreValue = DummyString + LinkDummyValue;
            _privateObject.SetFieldOrProperty(PrivateFieldNameUserAgent, "blackberry");            
            Shimlinkfrom.AllInstances.CreateRedirectLink = (linkFrom) => linkToStoreValue;

            //Act
            _privateObject.Invoke(TestedMethod_Redirect);

            //Asert
            RedirectUrl.ShouldSatisfyAllConditions(() => RedirectUrl.ShouldNotBeNullOrEmpty(),
                                                        () => RedirectUrl.ShouldContain(expectedValue));
        }
        
        [Test]
        [TestCategory("TrackData")]
        public void LinkFrom_TrackData_DecodingURL_WithSuccess()
        {
            //Arrange
            var expectedValue = 10;
            int? trackDataResult = 0;
            var linkURL = LinkDummyValue;

            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, linkURL);
            _privateObject.SetFieldOrProperty(PrivateFieldNameDecodeURL, true);

            ShimHttpRequest.AllInstances.UserHostAddressGet = (req) => DummyString;
            ShimHttpRequest.AllInstances.UserAgentGet = (req) => DummyString;
            ShimHttpServerUtility.AllInstances.UrlDecodeString = (req, decode) => linkURL;
            ShimEmailActivityLog.InsertClickInt32Int32StringStringInt32User = (EmailID, BlastID, urlToInsert, spyinfo, UniqueLinkID, User) => expectedValue;

            //Act
            trackDataResult =_privateObject.Invoke(TestedMethod_TrackData) as int?;

            //Assert
            trackDataResult.ShouldBe(expectedValue);
        }

        [Test]
        [TestCategory("TrackData")]
        public void LinkFrom_TrackData_WithouDecodingURL_WithSuccess()
        {
            //Arrange
            var expectedValue = 10;
            int? trackDataResult = 0;
            var linkURL = LinkDummyValue;

            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, linkURL);
            _privateObject.SetFieldOrProperty(PrivateFieldNameDecodeURL, false);

            ShimHttpRequest.AllInstances.UserHostAddressGet = (req) => DummyString;
            ShimHttpRequest.AllInstances.UserAgentGet = (req) => DummyString;
            ShimHttpServerUtility.AllInstances.UrlDecodeString = (req, decode) => linkURL;
            ShimEmailActivityLog.InsertClickInt32Int32StringStringInt32User = (EmailID, BlastID, urlToInsert, spyinfo, UniqueLinkID, User) => expectedValue;

            //Act
            trackDataResult = _privateObject.Invoke(TestedMethod_TrackData) as int?;

            //Asert
            trackDataResult.ShouldBe(expectedValue);
        }

        [Test]
        [TestCategory("TrackData")]
        public void LinkFrom_TrackData_WithExceptionCaught()
        {
            //Arrange
            var expectedValue = 0;
            int? trackDataResult = 0;            
            
            //Act
            Should.NotThrow(() => trackDataResult = _privateObject.Invoke(TestedMethod_TrackData) as int?);

            //Asert
            trackDataResult.ShouldBe(expectedValue);
        }
                
        [Test]
        [TestCategory("NotifyOfMissingTopicUDF")]
        public void LinkFrom_NotifyOfMissingTopicUDF_WithSuccess()
        {
            //Arrange
            var emailMessabeBodyResult = String.Empty;
            var blastID = 1;
            var emailID = 10;
            var blastLinkID = 2;
            var linkFromBlastLinkID = LinkDummyValue;
            var linkFromURL = LinkDummyValue;            

            _linkfrom.BlastID = blastID;
            _linkfrom.EmailID = emailID;
            _linkfrom.BlastLinkID = blastLinkID;
            _linkfrom.LinkFromBlastLinkID = linkFromBlastLinkID;
            _linkfrom.LinkFromURL = linkFromURL;

            ShimHttpRequest.AllInstances.RawUrlGet = (req) => RedirectUrl;
            ShimEmailService.ConstructorIEmailClientIConfigurationProvider = 
                (emailService, emailClient, configurationProvider) => 
                {
                    var shimProvider = new ShimEmailService(emailService)
                    {
                        SendEmailEmailMessageString = (emailMessage, mailServer) => 
                        {
                            emailMessabeBodyResult = emailMessage.Body;
                        }
                    };                    
                };

            //Act
            Should.NotThrow(() => _privateObject.Invoke(TestedMethod_NotifyOfMissingTopicUDF));

            //Asert
            emailMessabeBodyResult.ShouldSatisfyAllConditions(
                () => emailMessabeBodyResult.ShouldContain(blastID.ToString()),
                () => emailMessabeBodyResult.ShouldContain(emailID.ToString()),
                () => emailMessabeBodyResult.ShouldContain(blastLinkID.ToString()),
                () => emailMessabeBodyResult.ShouldContain(linkFromBlastLinkID),
                () => emailMessabeBodyResult.ShouldContain(linkFromURL));
        }
                
        [Test]
        [TestCategory("LogTransactionalUDF")]
        public void LinkFrom_LogTransactionalUDF_WithErrorLog_WithSuccess()
        {
            //Arrange            
            var expectedValue = DummyString;
            var linkToStoreValue = LinkDummyValue + "topic=" + expectedValue;
            var resultValue = String.Empty;
            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, linkToStoreValue);

            ShimEmailDataValues.RecordTopicsValueInt32Int32String = (RefBlastID, EmailID, udfValue) =>
            {
                resultValue = udfValue;
                return 0;
            };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (error, sourceMethod, applicationId, note, charityId, customerId) => DummyInt;
            Shimlinkfrom.AllInstances.NotifyOfMissingTopicUDF = (linkFrom) => { };

            //Act
            _privateObject.Invoke(TestedMethod_LogTransactionalUDF);

            //Asert
            resultValue.ShouldContain(expectedValue);
        }

        [Test]
        [TestCategory("LogTransactionalUDF")]
        public void LinkFrom_LogTransactionalUDF_WithExceptionCaughtAndLogged()
        {
            //Arrange           
            var exceptionLogged = false;            
            var resultValue = String.Empty;
            _privateObject.SetFieldOrProperty(PrivateFieldNameLinkToStore, null);

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, sourceMethod, applicationId, note, charityId, customerId) =>
            {
                exceptionLogged = true;
                return DummyInt;
            };
            Shimlinkfrom.AllInstances.CreateNote = (linkFrom) => DummyString;

            //Act
            _privateObject.Invoke(TestedMethod_LogTransactionalUDF);

            //Asert
            resultValue.ShouldBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }
        
        [Test]
        [TestCategory("replaceUDFWithValue")]
        public void LinkFrom_replaceUDFWithValue_WithSuccess()
        {
            //Arrange      
            var expectedResult = DummyString;
            var udfName = "valueToReplace";
            var udfRegexMatch= "%%valueToReplace%%";
            var linkURL = LinkDummyValue + udfRegexMatch;
            var result = String.Empty;

            var dataTableNames = new DataTable();
            dataTableNames.Columns.Add(udfName);
            dataTableNames.Rows.Add(expectedResult);

            ShimBlastActivity.FilterEmailsAllWithSmartSegmentInt32Int32 = (emailID, blastID) => dataTableNames;

            //Act
            result = (string)_privateObject.Invoke(TestedMethod_replaceUDFWithValue, linkURL);

            //Asert            
            result.ShouldContain(expectedResult);
        }

        [Test]
        [TestCategory("replaceUDFWithValue")]
        public void LinkFrom_replaceUDFWithValue_WithExceptionCaught()
        {
            //Arrange    
            var udfRegexMatch = "%%valueToReplace%%";
            var linkURL = LinkDummyValue + udfRegexMatch;
            var result = String.Empty;

            ShimBlastActivity.FilterEmailsAllWithSmartSegmentInt32Int32 = (emailID, blastID) => throw new Exception();

            //Act
            Should.NotThrow(()=> result = (string)_privateObject.Invoke(TestedMethod_replaceUDFWithValue, linkURL));

            //Asert            
            result.ShouldContain(linkURL);
        }
        
        private LinkTrackingParamOption CreateLinkTrackingParamOptionObject(int ltPoId)
        {
            var linkTrackingParamOption = new LinkTrackingParamOption();
            if (ltPoId == BlastIdValue)
            {
                linkTrackingParamOption.Value = BlastId;
                linkTrackingParamOption.IsDynamic = true;
            }
            else if (ltPoId == GroupNameValue)
            {
                linkTrackingParamOption.Value = GroupName;
                linkTrackingParamOption.IsDynamic = true;
            }
            else
            {
                linkTrackingParamOption.Value = SamleGroup;
                linkTrackingParamOption.IsDynamic = false;
            }
            return linkTrackingParamOption;
        }

        private string CreateSettingXml(bool allowCustomerOverride = false, bool defaultMainNode = true)
        {
            var xmlMainNode = defaultMainNode ? "Settings" : "Settings1";
            var element = new XElement(xmlMainNode,
             new XElement("Override", allowCustomerOverride),
             new XElement("QueryString", "a"),
             new XElement("Delimiter", ","),
             new XElement("AllowCustomerOverride", allowCustomerOverride));
            return element.ToString();
        }

        private DataTable CreateDataTableObjectForOmitor(string omnitureValue, string ltpoid)
        {
            var table = new DataTable();
            table.Columns.Add("ColumnName", typeof(string));
            table.Columns.Add("LTPOID", typeof(string));
            table.Columns.Add("CustomValue", typeof(string));
            table.Columns.Add("LTID", typeof(string));
            table.Rows.Add(omnitureValue, ltpoid, "100%20", "3");
            table.Rows.Add(omnitureValue, 1, "100%20", "3");
            return table;
        }

        private DataTable CreateDataTableObject()
        {
            var table = new DataTable();
            table.Columns.Add("ColumnName", typeof(string));
            table.Columns.Add("DisplayName", typeof(string));
            table.Columns.Add("LTID", typeof(string));
            table.Rows.Add("KnowledgeMarketing", "1", "3");
            table.Rows.Add("FolderName", "UnitTest", "3");
            table.Rows.Add("GroupName", "A", "3");
            table.Rows.Add("LayoutName", "AdminLayout", "3");
            table.Rows.Add("EmailSubject", "Unit Test", "3");
            table.Rows.Add("CustomValue", "100%20", "3");
            table.Rows.Add("BlastID", "1", "3");
            table.Rows.Add("eid", "1", "3");
            table.Rows.Add("bid", "1", "3");
            table.Rows.Add("default", "1", "3");
            return table;
        }
    }
}

