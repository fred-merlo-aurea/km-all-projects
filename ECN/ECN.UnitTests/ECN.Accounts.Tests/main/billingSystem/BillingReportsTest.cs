using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.main.billingSystem;
using ecn.accounts.main.billingSystem.Fakes;
using ECN.Accounts.Tests.Helper;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Accounts.Tests.main.billingSystem
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.accounts.main.billingSystem.BillingReports"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class BillingReportsTest : PageHelper
    {
        private BaseChannel _testChannel = new BaseChannel { BaseChannelName = "TestChannel", BaseChannelID = 1 };
        private Customer _testCustomer = new Customer { CustomerID = 1, CustomerName = "TestCustomer" };
        private ListBox _lstbxCustomers;
        private ListBox _lstbxBlastColumns;
        private TextBox _txtRunToDate;
        private TextBox _txtRunFromDate;
        private RadioButtonList _rblCustomer;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;
        private List<BillingReport> _listBillingReport;
        private List<BillingReportItem> _listBillingReportItem;
        private PrivateType _testType;
        private PrivateObject _privateTestObject;
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private void InitilizeTest()
        {
            _testType = new PrivateType(typeof(BillingReports));
            _testType.SetStaticField("currentReport", new BillingReport { });
            var billingReport = new BillingReports();
            _privateTestObject = new PrivateObject(billingReport);
            InitializeAllControls(billingReport);
            InitilizeFakes();
            _privateTestObject.Invoke("Page_Load", new object[] { null, null });
            _listBillingReport = new List<BillingReport>();
            _listBillingReportItem = new List<BillingReportItem>();
            BillingReports.dtFlatRateItems.Rows.Clear();
            var dataRow = BillingReports.dtFlatRateItems.NewRow();
            dataRow["ID"] = "-1";
            dataRow["ItemName"] = "1";
            dataRow["Amount"] = "1";
            dataRow["IsDeleted"] = "false";
            BillingReports.dtFlatRateItems.Rows.Add(dataRow);
            dataRow = BillingReports.dtFlatRateItems.NewRow();
            dataRow["ID"] = "1";
            dataRow["ItemName"] = "1";
            dataRow["Amount"] = "1";
            dataRow["IsDeleted"] = "false";
            BillingReports.dtFlatRateItems.Rows.Add(dataRow);
            dataRow = BillingReports.dtFlatRateItems.NewRow();
            dataRow["ID"] = "1";
            dataRow["ItemName"] = "1";
            dataRow["Amount"] = "1";
            dataRow["IsDeleted"] = "true";
            BillingReports.dtFlatRateItems.Rows.Add(dataRow);
            _lstbxCustomers.SelectedIndex = 0; 
            _lstbxBlastColumns.Items.Add("sendtime");
            _lstbxBlastColumns.Items.Add("blastfield1");
            _lstbxBlastColumns.Items.Add("blastfield2");
            _lstbxBlastColumns.Items.Add("blastfield3");
            _lstbxBlastColumns.Items.Add("blastfield4");
            _lstbxBlastColumns.Items.Add("blastfield5");
            _lstbxBlastColumns.Items.Add("fromemail");
            _lstbxBlastColumns.Items.Add("fromname");
            _lstbxBlastColumns.Items.Add("emailsubject");
            _lstbxBlastColumns.Items.Add("groupname");
            _lstbxBlastColumns.SelectedIndex = 0;
            _txtRunToDate.Text = "1.1.2018";
            _txtRunFromDate.Text = "1.1.2018";
        }

        private void GetControls()
        {
            _lstbxCustomers = (ListBox)_privateTestObject.GetField("lstbxCustomers");
            _lstbxBlastColumns = (ListBox)_privateTestObject.GetField("lstbxBlastColumns");
            _txtRunToDate = (TextBox)_privateTestObject.GetField("txtRunToDate");
            _txtRunFromDate = (TextBox)_privateTestObject.GetField("txtRunFromDate");
            _rblCustomer = (RadioButtonList)_privateTestObject.GetField("rblCustomer");
            _rblCustomer.Items.Add("all");
            _lblErrorMessage = (Label)_privateTestObject.GetField("lblErrorMessage");
            _phError = (PlaceHolder)_privateTestObject.GetField("phError");
        }

        private void InitilizeFakes()
        {
            ShimCurrentUser();
            ShimMasterPage();
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (user) => true;
            ShimBaseChannel.GetAll = () =>
                new List<BaseChannel> { _testChannel };
            ShimBillingReport.GetALL = () =>
                new List<BillingReport> { new BillingReport { } };
            ShimBillingReport.SaveBillingReportBillingReport = (currentReport) =>
            {
                _listBillingReport.Add(currentReport);
                return 0;
            };
            ShimCustomer.GetByBaseChannelIDInt32 = (id) =>
                new List<Customer> { _testCustomer };
            ShimBillingReportItem.SaveBillingReportItem = (item) =>
            {
                _listBillingReportItem.Add(item);
                return 0;
            };
            ShimBillingReportItem.GetByBillingReportItemIDInt32 = (id) => new BillingReportItem { };
            ShimBillingReportItem.GetEmailUsageByCustomerStringDateTimeDateTimeStringString =
                (custIDs, startDate, endDate, columnsString, columns) => new List<BillingReportItem> {
                    new BillingReportItem { BlastField1 = string.Empty, BlastField2 = string.Empty, BlastField3 = string.Empty,
                        BlastField4 = string.Empty, BlastField5 = string.Empty, FromEmail = string.Empty,
                        FromName = string.Empty, EmailSubject = string.Empty  } };
            ShimDirectory.ExistsString = (path) => false;
            ShimDirectory.CreateDirectoryString = (path) => null;
            ShimFile.ExistsString = (filePath) => true;
            ShimFile.DeleteString = (filePath) => { };
            ShimFile.AppendTextString = (fileName) =>
            {
                return new ShimStreamWriter();
            };
            ShimTextWriter.AllInstances.WriteLineString = (instance, text) => { _result += text; };
            ShimHttpResponse.AllInstances.WriteFileString = (instance, fileName) => { };
            ShimHttpResponse.AllInstances.End = (instance) => { };
            GetControls();
        }
        private void ShimMasterPage()
        {
            ShimBillingReports.AllInstances.MasterGet = (p) => new ecn.accounts.MasterPages.Accounts();
        }

        private void ShimCurrentUser()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { };
            shimSession.Instance.CurrentUser = new KMPlatform.Entity.User { UserID = 1, UserName = "TestUser" };
            shimSession.Instance.CurrentBaseChannel = _testChannel;
            shimSession.Instance.CurrentCustomer = _testCustomer;
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }
    }
}
