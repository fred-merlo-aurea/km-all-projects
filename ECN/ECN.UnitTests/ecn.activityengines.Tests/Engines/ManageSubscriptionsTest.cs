using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.activityengines.Tests.Helpers;
using CommonFakes = ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionHelper = ECN.Tests.Helpers.ReflectionHelper;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    /// <summary>
    /// Unit test for <see cref="ManageSubscriptions"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class ManageSubscriptionsTest : PageHelper
    {
        private const string MethodPageLoad = "Page_Load";
        private const string AccountDb = "accountdb";
        private const string AccountDbValue = "dummyString";
        private const string DummyString = "dummyString";
        private const string One = "1";
        private const string Male = "male";
        private const string Female = "female";
        private const string LinkToStore = "http://km.all.com?a=test";
        private const string KMCommonApplication = "KMCommon_Application";
        private const string KMCommonApplicationValue = "1";
        private ManageSubscriptions _manageSubscriptions;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _manageSubscriptions = new ManageSubscriptions();
            _privateObject = new PrivateObject(_manageSubscriptions);
            base.InitializeAllControls(_manageSubscriptions);
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => { return new CampaignItem { CustomerID = 1 }; };
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => { return new Customer { BaseChannelID = 1, CustomerID = 1 }; };
            var appSettings = new NameValueCollection();
            appSettings.Add(KMCommonApplication, KMCommonApplicationValue);
            appSettings.Add(AccountDb, AccountDbValue);
            ShimConfigurationManager.AppSettingsGet = () => { return appSettings; };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32
                = (error, sourceMethod, applicationId, note, charityId, customerId) => { return 1; };
        }

        [Test]
        public void Page_Load_WhenNotPostBackRequest_PanelIsDisplayed()
        {
            // Arrange
            QueryString.Clear();
            QueryString.Add("e", "dummyEmail@km.com, 1, 1");
            QueryString.Add("prefrence", "email");
            CommonFakes.ShimDataFunctions.GetDataTableString = (x) => CreateCustomersTable();
            CommonFakes.ShimDataFunctions.ExecuteScalarString = (x) => string.Empty;

            // Act
            _privateObject.Invoke(MethodPageLoad, null, EventArgs.Empty);

            //// Assert
            var managePanel = ReflectionHelper.GetFieldValue(_manageSubscriptions, "pnlManange") as Panel;
            _manageSubscriptions.ShouldSatisfyAllConditions(
                 () => managePanel.ShouldNotBeNull(),
                 () => managePanel.Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_WhenExceptionIsThrown_ErrorIsDisplayed()
        {
            // Arrange
            QueryString.Clear();
            QueryString.Add("e", "dummyEmail@km.com, 1, 1");
            QueryString.Add("prefrence", "both");
            CommonFakes.ShimDataFunctions.GetDataTableString = (x) => CreateCustomersTable(Female);
            CommonFakes.ShimDataFunctions.ExecuteScalarString = (x) =>
            {
                return x.Contains("SELECT HeaderSource as FooterSource FROM") == true ? throw new Exception() : string.Empty;
            };
            var errorMessage = "Error loading Subscription Manager";
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (a, b, c, d, e, f) => 1;

            // Act
            _privateObject.Invoke(MethodPageLoad, null, EventArgs.Empty);

            // Assert
            var managePanel = ReflectionHelper.GetFieldValue(_manageSubscriptions, "pnlManange") as Panel;
            var message = ReflectionHelper.GetFieldValue(_manageSubscriptions, "lbMessage") as Label;
            _manageSubscriptions.ShouldSatisfyAllConditions(
                 () => managePanel.ShouldNotBeNull(),
                 () => message.ShouldNotBeNull(),
                 () => managePanel.Visible.ShouldBeFalse(),
                 () => message.Text.ShouldContain(errorMessage));
        }

        private DataTable CreateCustomersTable(string gender = Male)
        {
            var table = new DataTable();
            table.Columns.Add("CustomerID", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("Lastname", typeof(string));
            table.Columns.Add("title", typeof(string));
            table.Columns.Add("company", typeof(string));
            table.Columns.Add("address", typeof(string));
            table.Columns.Add("city", typeof(string));
            table.Columns.Add("state", typeof(string));
            table.Columns.Add("zip", typeof(string));
            table.Columns.Add("country", typeof(string));
            table.Columns.Add("mobile", typeof(string));
            table.Columns.Add("voice", typeof(string));
            table.Columns.Add("BirthDate", typeof(string));
            table.Columns.Add("gender", typeof(string));
            table.Rows.Add(One, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, DummyString, gender);
            return table;
        }
    }
}
