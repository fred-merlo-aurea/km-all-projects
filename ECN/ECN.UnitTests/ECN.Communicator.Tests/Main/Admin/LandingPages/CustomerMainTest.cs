using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.landingpages;
using ecn.communicator.main.admin.landingpages.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using Microsoft.QualityTools.Testing.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Admin.LandingPages
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CustomerMainTest
    {
        private const string CurrentCustomer = "CurrentCustomer";
        private const string TempURL = "http://www.tempuri.org";
        private const string MethodLandingPage = "gvLandingPage_RowCommand";
        private const string CommandName = "Edit";
        private const string PlaceHolderError = "phError";
        private const string LblErrorMessage = "lblErrorMessage";
        private const string ThrowEcnException = "throwECNException";
        private const string Message = "error";
        private const string LandingPageError = "<br/>LandingPage: error";
        private const string MethodGetPreview = "getPreviewParams";
        private const string MethodLandingPageRowData = "gvLandingPage_RowDataBound";
        private const string ActivityDomainPath = "Activity_DomainPath";
        private const string Path = "path";
        private const string Unsubscribe = "CustomerUnsubscribe.aspx";
        private const string Error = "CustomerError.aspx";
        private const string ForwardToFriend = "CustomerForwardToFriend.aspx";
        private const string Abuse = "CustomerAbuse.aspx";
        private const string TestCase1 = "1";
        private const string TestCase2 = "2";
        private const string TestCase3 = "3";
        private const string TestCase4 = "4";
        private const string TestCase5 = "5";
        private const string CurrentBaseChannel = "CurrentBaseChannel";
        private const int Case1 = 1;
        private const int Case2 = 2;
        private const int Case3 = 3;
        private const int Case4 = 4;
        private const int Case5 = 5;
        private CustomerMain _customer;
        private IDisposable _shims;

        [SetUp]
        public void SetUp()
        {
            _shims = ShimsContext.Create();
            _customer = new CustomerMain();
            CreateMaster();
        }

        [TearDown]
        public void TearDown()
        {
            _shims.Dispose();
        }

        [Test]
        [TestCase(TestCase1, Unsubscribe)]
        [TestCase(TestCase2, Error)]
        [TestCase(TestCase3, ForwardToFriend)]
        [TestCase(TestCase4, Abuse)]
        public void GvLandingPage_RowCommand(string argument, string page)
        {
            // Arrange
            var commandEventArgs = new CommandEventArgs(CommandName, argument);
            var gridViewCommandEventArgs = new GridViewCommandEventArgs(argument, commandEventArgs);
            ShimPage.AllInstances.ResponseGet = (x) => new ShimHttpResponse();

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodLandingPage, new object[] { this, gridViewCommandEventArgs });

            // Assert
            gridViewCommandEventArgs.ShouldSatisfyAllConditions(
                () => gridViewCommandEventArgs.CommandName.ShouldBe(CommandName),
                () => gridViewCommandEventArgs.CommandArgument.ShouldBe(argument));
        }

        [Test]
        public void ThrowEcnException_ForErrorList_ShouldSetECNError()
        {
            // Arrange
            InitializePageAndControls();

            // Act
            ReflectionHelper.ExecuteMethod(_customer, ThrowEcnException, new object[] { Message });

            // Assert
            AssertLabel(LblErrorMessage, LandingPageError);
        }

        [Test]
        [TestCase(Case1)]
        [TestCase(Case2)]
        [TestCase(Case3)]
        [TestCase(Case4)]
        public void GetPreviewParams_ForDataTable_ReturnParameters(int lpid)
        {
            // Arrange
            SetUpPreview(lpid);

            // Act
            var result = ReflectionHelper.ExecuteMethod(_customer, MethodGetPreview, new object[] { Case1 });

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        [TestCase(Case1)]
        [TestCase(Case3)]
        [TestCase(Case4)]
        public void GvLandingPage_RowDataBound_ForLandingPageAssign_ShouldNavigate(int param)
        {
            // Arrange
            var gridViewRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Edit);
            gridViewRow.DataItem = new LandingPage()
            {
                LPID = param
            };
            var gridViewRowEventArgs = new GridViewRowEventArgs(gridViewRow);
            var landingPageAssign = new LandingPageAssign()
            {
                LPAID = param
            };
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(ActivityDomainPath, Path);
            ShimConfigurationManager.AppSettingsGet = () => nameValueCollection;
            var hyperLink = new HyperLink();
            ShimControl.AllInstances.FindControlString = (x, y) => hyperLink;
            ShimCustomerMain.AllInstances.getPreviewParamsInt32 = (x, y) => Path;

            // Act
            ReflectionHelper.ExecuteMethod(_customer, MethodLandingPageRowData, new object[] { this, gridViewRowEventArgs });

            // Assert
            hyperLink.ShouldSatisfyAllConditions(
                () => hyperLink.Visible.ShouldBeTrue(),
                () => hyperLink.NavigateUrl.ShouldNotBeNull());
        }

        private void SetUpPreview(int lpid)
        {
            DataLayerFakes.ShimLandingPageAssign.GetPreviewParametersInt32Int32 = (x, y) => CreateDataTable();
            var landingPageAssign = new LandingPageAssign()
            {
                LPAID = 1,
                LPID = lpid
            };
            DataLayerFakes.ShimLandingPageAssign.GetSqlCommand = (x) => landingPageAssign;
            var landingPageAssignContent = new LandingPageAssignContent()
            {
                LPAID = 1
            };
            var landingpageAssignContentList = new List<LandingPageAssignContent> { landingPageAssignContent };
            DataLayerFakes.ShimLandingPageAssignContent.GetListSqlCommand = (x) => landingpageAssignContentList;
        }

        private static DataTable CreateDataTable()
        {
            var datatable = new DataTable();
            datatable.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column1", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column2", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column3", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column4", typeof(string)));
            datatable.Columns.Add(new DataColumn("Column5", typeof(string)));
            var dataRow = datatable.NewRow();
            dataRow["RowNumber"] = Case1;
            dataRow["Column1"] = TestCase1;
            dataRow["Column2"] = TestCase1;
            dataRow["Column3"] = TestCase1;
            dataRow["Column4"] = string.Empty;
            dataRow["Column5"] = string.Empty;
            datatable.Rows.Add(dataRow);
            return datatable;
        }

        private void AssertLabel(string labelName, string message)
        {
            var label = ReflectionHelper.GetFieldInfoFromInstanceByName(_customer, labelName)
                .GetValue(_customer) as Label;
            label.ShouldSatisfyAllConditions(
                () => label.ShouldNotBeNull(),
                () => label.Text.ShouldBe(message));
        }

        private void InitializePageAndControls()
        {
            ReflectionHelper.SetValue(_customer, PlaceHolderError, new PlaceHolder());
            ReflectionHelper.SetValue(_customer, LblErrorMessage, new Label());
        }

        private void CreateMaster()
        {
            var shimMaster = new ShimCommunicator();
            ShimCustomerUnsubscribe.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            ShimPage.AllInstances.MasterGet = (obj) => shimMaster.Instance;
            var shimECNSession = new ShimECNSession();
            var fieldCurrentCustomer = typeof(ECNSession).GetField(CurrentCustomer);
            if (fieldCurrentCustomer == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentCustomer should not be null");
            }
            fieldCurrentCustomer.SetValue(shimECNSession.Instance, new Customer
            {
                CustomerID = Case1
            });
            var fieldCurrentBaseChannel = typeof(ECNSession).GetField(CurrentBaseChannel);
            if (fieldCurrentBaseChannel == null)
            {
                throw new InvalidOperationException($"Field fieldCurrentBaseChannel should not be null");
            }
            fieldCurrentBaseChannel.SetValue(shimECNSession.Instance, new BaseChannel
            {
                BaseChannelID = Case1
            });
            ShimCommunicator.AllInstances.UserSessionGet = (obj) => shimECNSession.Instance;
            ShimHttpContext.CurrentGet = () =>
            {
                var context = new HttpContext(new HttpRequest(null, TempURL, null), new HttpResponse(null));
                return context;
            };
        }
    }
}