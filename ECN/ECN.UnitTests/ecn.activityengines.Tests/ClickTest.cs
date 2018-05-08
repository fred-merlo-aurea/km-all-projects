using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using ShimKMUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using ecn.activityengines.Tests.Helpers;
using ecn.communicator.classes.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;

namespace ecn.activityengines.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ClickTest: PageHelper
    {
        private Click _testedEntity;
        private PrivateObject _privateTestedObject;        
        private KMPlatform.Entity.User _user;        
        private const int DummyInt = 100;
        private const string BlastLinkIdKey = "lid";
        private const string BlastLinkIdValue = "6";
        private const string BlastIdKey = "b";
        private const string BlastIdValue = "111";
        private const string BlastTypeValue = "LAYOUT";
        private const string CustomerIdValue = "10";
        private const string EmailIdKey = "e";
        private const string EmailIdValue = "5";
        private const string EmailAddressValue = "email@email.com";
        private const string GroupIdValue = "11";
        private const string ParametersKey = "p";
        private const string ParametersValue = "{0},{1},{2},{3},{4}";
        private const string AccountsDBKey = "accountsdb";
        private const string ActivityDomainPathKey = "Activity_DomainPath";
        private const string Image_DomainPathKey = "Image_DomainPath";
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private const string AdminToEmailKey = "Admin_ToEmail";
        private const string AdminFromEmailKey = "Admin_FromEmail";                
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string AdmimNotifyKey = "Admin_Notify";
        private const string AdmimNotifyValue = "true";
        private const string HttpPostKey = "HTTP_HOST";
        private const string DummyString = "dummyStringValue";        
        private const string UrlValue = "http://localhost:8080";
        private const string UdfName = "valueToReplace";
        private const string UdfNewValue = "newValue";
        private const string UrlQueryUdfRegexMatch = "topic=%%valueToReplace%%";
        private const string UrlQuery = "valueToReplace&{0}={1}&{2}={3}&{4}={5}";

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();

            _user = new KMPlatform.Entity.User()
            {
                UserID = DummyInt
            };

            _testedEntity = new Click();
            _privateTestedObject = new PrivateObject(_testedEntity);

            InitializeAllControls(_testedEntity);
            InitializeFakes();            
        }

        private void InitializeFakes()
        {
            ShimKMUser.GetByAccessKeyStringBoolean = (key, getChildren) => _user;

            var queryStringWithValues = String.Format(UrlQuery,
                                                        BlastIdKey, BlastIdValue,
                                                        EmailIdKey, EmailIdValue,
                                                        BlastLinkIdKey, BlastLinkIdValue);

            var urlWithQueryString = UrlValue + "?" + queryStringWithValues;            

            ShimHttpRequest.AllInstances.UrlGet = (req) => { return new Uri(urlWithQueryString); };
            ShimHttpRequest.AllInstances.UserHostAddressGet = (req) => DummyString;
            ShimHttpRequest.AllInstances.UserAgentGet = (req) => DummyString;
            ShimHttpServerUtility.AllInstances.UrlDecodeString = (req, decode) => decode;
            communicator.classes.Fakes.ShimEmailActivityLog.InsertClickInt32Int32StringString = (EmailID, BlastID, urlToInsert, spyinfo) => DummyInt;

            ShimEncryption.GetCurrentByApplicationIDInt32 = (DummyInt) => new Encryption();
            KM.Common.Fakes.ShimEncryption.DecryptStringEncryption = (text, ec) => urlWithQueryString;

            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {
                   {ECNEngineAccessKey, DummyString },
                   {AdminToEmailKey, EmailAddressValue },
                   {AdminFromEmailKey, EmailAddressValue },                                      
                   {AdmimNotifyKey,AdmimNotifyValue }, 
                   {KMCommonApplicationKey, DummyInt.ToString()} 
               };

            ShimBlast.GetByBlastIDInt32UserBoolean =(blastID, user,getChildren) =>
            {
                var blast = new ECN_Framework_Entities.Communicator.BlastLayout()
                {
                    BlastType = "LAYOUT",
                    GroupID = int.Parse(GroupIdValue),
                    CustomerID = int.Parse(CustomerIdValue)
                };
                return blast;
            };
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlastSingle.GetRefBlastIDInt32Int32Int32String = (blastID, emailID, customerID, blastType) =>
            {
                return int.Parse(BlastIdValue);
            };
            ShimBlastLink.GetByBlastLinkIDInt32Int32 = (blastId, blastLinkId) =>
            {
                return new ECN_Framework_Entities.Communicator.BlastLink()
                {
                    LinkURL = UrlValue + "?" + UrlQueryUdfRegexMatch
                };
            };
            ShimBlastActivity.FilterEmailsAllWithSmartSegmentInt32Int32 = (emailID, blastID) => 
            { 
                var dataTableNames = new DataTable();
                dataTableNames.Columns.Add(UdfName);
                dataTableNames.Rows.Add(UdfNewValue);
                return dataTableNames;
            };
            
            ShimEmailGroup.ValidForTrackingInt32Int32 = (blastId, emailId) => true;
            ShimEmailDataValues.RecordTopicsValueInt32Int32String = (RefBlastID, EmailID, udfValue) => 0;
            ShimEmailFunctions.Constructor = (emailF) =>
            {
                var shimFunction = new ShimEmailFunctions(emailF)
                {
                    SimpleSendStringStringStringString = (to, from, subject, body) => { }
                };
            };

            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 = (error, sourceMethod, applicationId, note, charityId, customerId) => DummyInt;
        }

    }
}
