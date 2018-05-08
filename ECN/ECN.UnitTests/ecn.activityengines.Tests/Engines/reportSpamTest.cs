using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Caching.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimKMUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class reportSpamTest: PageHelper
    {
        private reportSpam _testedEntity;
        private PrivateObject _privateTestedObject;
        private ShimCache _shimCache;
        private KMPlatform.Entity.User _user;
        private DataTable _dtAbuseReport;        
        private const int DummyInt = 100;
        private const string BlastIdValue = "111";
        private const string BlastTypeValue = "LAYOUT";
        private const string PreviewKey = "preview";
        private const string PreviewValue = "5";
        private const string PreviewNegativeValue = "-1";
        private const string CustomerIdValue = "10";
        private const string EmailIdValue = "5";
        private const string EmailAddressValue = "email@email.com";
        private const string GroupIdValue = "11";
        private const string ParametersKey = "p";
        private const string ParametersValue = "{0},{1},{2},{3},{4}";
        private const string AccountsDBKey = "accountsdb";
        private const string ActivityDomainPathKey = "Activity_DomainPath";
        private const string Image_DomainPathKey = "Image_DomainPath";
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private const string ConnectionStringKey = "connString";
        private const string ConnectionStringValue = "test_connection_string";
        private const string ABUSERPTToEmailKey ="ABUSERPT_ToEmail";
        private const string ABUSERPTFromEmailKey = "ABUSERPT_FromEmail";
        private const string ABUSERPTSubjectKey = "ABUSERPT_Subject";
        private const string SmtpServerKey = "SmtpServer";
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string LabelHeaderId = "lblHeader";
        private const string LabelFooterId = "lblFooter";
        private const string LabelEmailAddressId = "lblEmailAddress";
        private const string LabelErrorMessageId = "lblErrorMessage";
        private const string ColumnBaseChannelName = "basechannelname";
        private const string ColumnCustomerName = "customerName";
        private const string ColumnSendTime = "sendtime";
        private const string ColumnEmailSubject = "EmailSubject";
        private const string DummyString = "dummyStringValue";
        private const string DummyPathString = "km/ecn";        
        private const string UrlValue = "http://localhost:8080";
        private const string urlQuery = "{0}={1}";

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();

            _user = new KMPlatform.Entity.User()
            {
                UserID = DummyInt
            };

            _testedEntity = new reportSpam();
            _privateTestedObject = new PrivateObject(_testedEntity);

            InitializeAllControls(_testedEntity);
            InitializeFakes();
            InitializeDtAbuseReport();
        }
        
        private void InitializeFakes()
        {
            var queryStringWithValues = String.Format(urlQuery,PreviewKey, PreviewValue);

            ShimHttpRequest.AllInstances.UrlGet = (req) => { return new Uri(UrlValue + "?" + queryStringWithValues); };            

            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {   
                   {ECNEngineAccessKey, DummyString },
                   {ABUSERPTToEmailKey, EmailAddressValue },
                   {ABUSERPTFromEmailKey, EmailAddressValue },
                   {ABUSERPTSubjectKey, DummyString },
                   {SmtpServerKey,DummyString },
                   {KMCommonApplicationKey, DummyInt.ToString()}
               };

            _shimCache = new ShimCache()
            {
                AddStringObjectCacheDependencyDateTimeTimeSpanCacheItemPriorityCacheItemRemovedCallback =
                (key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback) => new object()
            };
            _shimCache.Bind(new Dictionary<string, KMPlatform.Entity.User>());

            ShimKMUser.GetByAccessKeyStringBoolean = (key, getChildren) => _user;

            ShimPage.AllInstances.CacheGet = (page) => _shimCache;
            ShimPage.AllInstances.MasterGet = (p) => 
            {
                var master = new MasterPages.Activity();
                master.Controls.Add(new Label()
                {
                    ID = LabelHeaderId
                });
                master.Controls.Add(new Label()
                {
                    ID = LabelFooterId
                });

                return master;
            };

            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean =
               (blastID, getChildren) =>
               {
                   var blast = new ECN_Framework_Entities.Communicator.BlastLayout()
                   {
                       BlastType = "LAYOUT",
                       GroupID = int.Parse(GroupIdValue),
                       CustomerID = int.Parse(CustomerIdValue)
                   };
                   return blast;
               };
            ShimBlastSingle.GetRefBlastIDInt32Int32Int32String = (blastID, emailID, customerID, blastType) =>
            {
                return int.Parse(BlastIdValue);
            };
            
            ShimEmail.IsValidEmailAddressForBlastInt32StringInt32Int32Int32 = (EmailID, EmailAddress, CustomerID, refGroupID, refBlastID) => true;
            
            ShimCustomer.GetByCustomerIDInt32Boolean = (customerId, getChildren) =>
            {
                return new Customer
                {
                    CustomerName = DummyString
                };
            };
                        
            ShimLandingPageAssign.GetByLPAIDInt32Boolean = (lpaid, getChildren) =>
            {
                return new LandingPageAssign()
                {
                    Header = DummyString,
                    Footer = DummyString
                };
            };

            ShimBlast.GetBlastInfoForAbuseReportingInt32 = (blastId) => _dtAbuseReport;
        }
        
        private void InitializeDtAbuseReport()
        {
            _dtAbuseReport = new DataTable();
            _dtAbuseReport.Columns.Add(ColumnBaseChannelName);
            _dtAbuseReport.Columns.Add(ColumnCustomerName);
            _dtAbuseReport.Columns.Add(ColumnSendTime);
            _dtAbuseReport.Columns.Add(ColumnEmailSubject);

            _dtAbuseReport.Rows.Add(DummyString, DummyString, DummyString, DummyString);
        }
    }
}
