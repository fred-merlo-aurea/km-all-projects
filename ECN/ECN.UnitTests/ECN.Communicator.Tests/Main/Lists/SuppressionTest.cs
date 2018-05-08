using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ecn.communicator.main.lists;
using ecn.communicator.main.lists.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;


namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="Suppression"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SuppressionTest : BaseListsTest<Suppression>
    {
        private const string MethodDownloadFilteredEmailsButtonClick = "DownloadFilteredEmailsButton_Click";
        private StateBag _viewState;
        private string _fileName;
        private string _fileText;

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault_NoEmail()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
                string.Empty, string.Empty, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBeEmpty(),
                () => fileText.ShouldBeEmpty());
        }

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
                string.Empty, SampleEmail, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBeEmpty(),
                () => fileText.ShouldBeEmpty());
        }

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeAll_SearchTypeLike_DownloadTypeXls()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXls, SubscribeTypeAll,
                SearchTypeLike, SampleEmail, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBe(XlsContentType),
                () => fileText.ShouldBe(XlsFileText),
                () => responseHeader.ShouldContain(Emails + DownloadTypeXls),
                () => responseText.ShouldContain(Emails + DownloadTypeXls));
        }

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeDefault_SearchTypeEquals_DownloadTypeCsv()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeCsv, string.Empty,
                SearchTypeEquals, SampleEmail, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBe(CsvContentType),
                () => fileText.ShouldBe(TxtFileText),
                () => responseHeader.ShouldContain(Emails + DownloadTypeCsv),
                () => responseText.ShouldContain(Emails + DownloadTypeCsv));
        }

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeDefault_SearchTypeStarts_DownloadTypeXml()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXml, string.Empty,
                SearchTypeStarts, SampleEmail, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBe(XmlContentType),
                () => fileText.ShouldBeEmpty(),
                () => responseHeader.ShouldContain(Emails + DownloadTypeXml),
                () => responseText.ShouldContain(Emails + DownloadTypeXml));
        }

        [Test]
        public void DownloadFilteredEmailsButtonClick_SubscribeTypeDefault_SearchTypeEnds_DownloadTypeTxt()
        {
            // Arrange
            InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeTxt, string.Empty,
                SearchTypeEnds, SampleEmail, SampleFilterId1, string.Empty);

            // Act
            privateObject.Invoke(MethodDownloadFilteredEmailsButtonClick, null, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => contentType.ShouldBe(TxtContentType),
                () => fileText.ShouldBe(XlsFileText),
                () => responseHeader.ShouldContain(Emails + DownloadTypeTxt),
                () => responseText.ShouldContain(Emails + DownloadTypeTxt));
        }

        [Test]
        public void Page_Load_DefaultUser_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroup.GetByCustomerIDInt32UserString = (p1, p2, p3) => new List<Group> { new Group { MasterSupression = 1} };
            var dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable());
            dataSet.Tables.Add(new DataTable { Columns = { "id"}, Rows = { { "1" } } });
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32DateTimeDateTimeBooleanStringStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => dataSet;
            InitializeGrids();
            var gvDomainSuppression = privateObject.GetFieldOrProperty("gvDomainSuppression") as GridView;
            var gvSuppressionGroup = privateObject.GetFieldOrProperty("gvSuppressionGroup") as GridView;
            var gvChannelMasterSuppression = privateObject.GetFieldOrProperty("gvChannelMasterSuppression") as GridView;
            var TabMasterSuppression = privateObject.GetFieldOrProperty("TabMasterSuppression") as TabPanel;
            var TabDomainSuppression = privateObject.GetFieldOrProperty("TabDomainSuppression") as TabPanel;
            var TabChannelSuppression = privateObject.GetFieldOrProperty("TabChannelSuppression") as TabPanel;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            gvDomainSuppression.Columns[2].Visible.ShouldBeFalse();
            gvSuppressionGroup.Columns[6].Visible.ShouldBeFalse();
            gvChannelMasterSuppression.Columns[2].Visible.ShouldBeFalse();
            TabMasterSuppression.Visible.ShouldBeFalse();
            TabDomainSuppression.Visible.ShouldBeFalse();
            TabChannelSuppression.Visible.ShouldBeFalse();
        }

        [Test]
        public void Page_Load_Administrator_WithAccess_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroup.GetByCustomerIDInt32UserString = (p1, p2, p3) => new List<Group> { new Group { MasterSupression = 1 } };
            var dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable());
            dataSet.Tables.Add(new DataTable { Columns = { "id" }, Rows = { { "1" } } });
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32DateTimeDateTimeBooleanStringStringString = (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => dataSet;
            InitializeGrids();
            var gvDomainSuppression = privateObject.GetFieldOrProperty("gvDomainSuppression") as GridView;
            var gvSuppressionGroup = privateObject.GetFieldOrProperty("gvSuppressionGroup") as GridView;
            var gvChannelMasterSuppression = privateObject.GetFieldOrProperty("gvChannelMasterSuppression") as GridView;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimUser.IsAdministratorUser = (p) => true;
            ShimUser.IsChannelAdministratorUser = (p) => true;
            ShimUser.IsSystemAdministratorUser = (p) => true;
            var TabMasterSuppression = privateObject.GetFieldOrProperty("TabMasterSuppression") as TabPanel;
            var TabDomainSuppression = privateObject.GetFieldOrProperty("TabDomainSuppression") as TabPanel;
            var TabChannelSuppression = privateObject.GetFieldOrProperty("TabChannelSuppression") as TabPanel;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            gvDomainSuppression.Columns[2].Visible.ShouldBeTrue();
            gvSuppressionGroup.Columns[6].Visible.ShouldBeTrue();
            gvChannelMasterSuppression.Columns[2].Visible.ShouldBeTrue();
            TabMasterSuppression.Visible.ShouldBeTrue();
            TabDomainSuppression.Visible.ShouldBeTrue();
            TabChannelSuppression.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnAddChannelSuppressionEmails_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var txtemailAddresses = privateObject.GetFieldOrProperty("txtemailAddresses") as TextBox;
            txtemailAddresses.Text = "test@km.com";
            ShimEmailGroup.ImportEmailsToCSUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { "T", "1" }, { "T", "1" }, { "I", "1" }, { "U", "1" }, { "D", "1" }, { "S", "1" } }
            };
            var dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable { Columns = { "count" }, Rows = { { "1" } } });
            dataSet.Tables.Add(new DataTable { Columns = { "id" }, Rows = { { "1" } } });
            ShimChannelMasterSuppressionList.GetByEmailAddress_PagingInt32Int32Int32StringUser = (p1, p2, p3, p4, p5) => dataSet;
            var dgResults = privateObject.GetFieldOrProperty("dgResults") as DataGrid;

            // Act
            privateObject.Invoke("btnAddChannelSuppressionEmails_Click", new object[] { null, null });

            // Assert
            dgResults.ShouldSatisfyAllConditions(
                () => dgResults.Visible.ShouldBeTrue(),
                () => dgResults.Items.Count.ShouldBe(7));
        }

        [Test]
        public void BtnAddChannelSuppressionEmails_Click_NoEmail_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var txtemailAddresses = privateObject.GetFieldOrProperty("txtemailAddresses") as TextBox;
            txtemailAddresses.Text = string.Empty;
            var dgResults = privateObject.GetFieldOrProperty("dgResults") as DataGrid;
            var lblMessage = privateObject.GetFieldOrProperty("lblMessage") as Label;

            // Act
            privateObject.Invoke("btnAddChannelSuppressionEmails_Click", new object[] { null, null });

            // Assert
            dgResults.Visible.ShouldBeFalse();
            lblMessage.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnAddNoThresholdEmails_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var emailAddresses = privateObject.GetFieldOrProperty("emailAddresses") as TextBox;
            emailAddresses.Text = "test1@km.com,test2@km.com";
            ShimEmailGroup.ImportEmailsToNoThresholdUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { "T", "1" }, { "T", "1" }, { "I", "1" }, { "U", "1" }, { "D", "1" }, { "S", "1" } }
            };
            ShimChannelNoThresholdList.GetByEmailAddressInt32StringUser = (p1, p2, p3) => new List<ChannelNoThresholdList> { new ChannelNoThresholdList { } };
            var resultsGrid = privateObject.GetFieldOrProperty("ResultsGrid") as DataGrid;

            // Act
            privateObject.Invoke("btnAddNoThresholdEmails_Click", new object[] { null, null });

            // Assert
            resultsGrid.ShouldSatisfyAllConditions(
                () => resultsGrid.Visible.ShouldBeTrue(),
                () => resultsGrid.Items.Count.ShouldBe(7));
        }

        [Test]
        public void BtnAddNoThresholdEmails_Click_NoEmail_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var emailAddresses = privateObject.GetFieldOrProperty("emailAddresses") as TextBox;
            emailAddresses.Text = string.Empty;
            var resultsGrid = privateObject.GetFieldOrProperty("ResultsGrid") as DataGrid;
            var messageLabel = privateObject.GetFieldOrProperty("MessageLabel") as Label;

            // Act
            privateObject.Invoke("btnAddNoThresholdEmails_Click", new object[] { null, null });

            // Assert
            resultsGrid.Visible.ShouldBeFalse();
            messageLabel.Visible.ShouldBeTrue();
        }

        [Test]
        public void BtnAddGlobalSuppressionEmails_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var txtemailAddressesGlobal = privateObject.GetFieldOrProperty("txtemailAddressesGlobal") as TextBox;
            txtemailAddressesGlobal.Text = "test1@km.com,test2@km.com";
            ShimEmailGroup.ImportEmailsToGlobalMSUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { "T", "1" }, { "T", "1" }, { "I", "1" }, { "U", "1" }, { "D", "1" }, { "S", "1" } }
            };
            var dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable { Columns = { "count" }, Rows = { { "1" } } });
            dataSet.Tables.Add(new DataTable { Columns = { "id" }, Rows = { { "1" } } });
            ShimGlobalMasterSuppressionList.GetByEmailAddress_PagingInt32Int32StringUser = (p1, p2, p3, p4) => dataSet;
            var dgResultsGlobal = privateObject.GetFieldOrProperty("dgResultsGlobal") as DataGrid;

            // Act
            privateObject.Invoke("btnAddGlobalSuppressionEmails_Click", new object[] { null, null });

            // Assert
            dgResultsGlobal.ShouldSatisfyAllConditions(
                () => dgResultsGlobal.Visible.ShouldBeTrue(),
                () => dgResultsGlobal.Items.Count.ShouldBe(7));
        }

        [Test]
        public void BtnAddGlobalSuppressionEmails_Click_NoEmail_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var txtemailAddressesGlobal = privateObject.GetFieldOrProperty("txtemailAddressesGlobal") as TextBox;
            txtemailAddressesGlobal.Text = string.Empty;
            var dgResultsGlobal = privateObject.GetFieldOrProperty("dgResultsGlobal") as DataGrid;

            // Act
            privateObject.Invoke("btnAddGlobalSuppressionEmails_Click", new object[] { null, null });

            // Assert
            //This is a bug should be fixed and changed back to False
            dgResultsGlobal.Visible.ShouldBeTrue();
        }

        [Test]
        public void ExportEmailsBTN_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitilizeExportFakes();
            ShimChannelNoThresholdList.GetByEmailAddressInt32StringUser = (p1, p2, p3) => 
                new List<ChannelNoThresholdList> { new ChannelNoThresholdList { EmailAddress = "test@km.com" } };

            // Act
            privateObject.Invoke("exportEmailsBTN_Click", new object[] { null, null });

            // Assert
            _fileText.ShouldBe("test@km.com, ");
            _fileName.ShouldBe("-1_NoThreshold_Emails.CSV");
        }

        [Test]
        public void BtnexportEmailsGlobal_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitilizeExportFakes();
            ShimGlobalMasterSuppressionList.GetByEmailAddressStringUser = (p1, p2) =>
                new List<GlobalMasterSuppressionList> { new GlobalMasterSuppressionList { EmailAddress = "test@km.com" } };

            // Act
            privateObject.Invoke("btnexportEmailsGlobal_Click", new object[] { null, null });

            // Assert
            _fileText.ShouldBe("test@km.com, ");
            _fileName.ShouldBe("-1_MasterSuppressedGlobal_Emails.CSV");
        }

        [Test]
        public void BtnexportEmails_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitilizeExportFakes();
            ShimChannelMasterSuppressionList.GetByEmailAddressInt32StringUser = (p1, p2, p3) => 
                new List<ChannelMasterSuppressionList> { new ChannelMasterSuppressionList { EmailAddress = "test@km.com" } };

            // Act
            privateObject.Invoke("btnexportEmails_Click", new object[] { null, null });

            // Assert
            _fileText.ShouldBe("test@km.com, ");
            _fileName.ShouldBe("-1_MasterSuppressed_Emails.CSV");
        }

        [Test]
        public void SearchString_Get_Success([Values("like","equals","ends","starts","default")]string searchKey)
        {
            // Arrange
            InitilizeTestObjects();
            var subscribeTypeFilter = privateObject.GetFieldOrProperty("SubscribeTypeFilter") as DropDownList;
            subscribeTypeFilter.Items.Add("subcribeType");
            subscribeTypeFilter.SelectedIndex = 0;
            var emailFilter = privateObject.GetFieldOrProperty("EmailFilter") as TextBox;
            emailFilter.Text = "test@km.com";
            var searchEmailLike = privateObject.GetFieldOrProperty("SearchEmailLike") as DropDownList;
            searchEmailLike.Items.Add(searchKey);
            searchEmailLike.SelectedValue = searchKey;
            var email = searchKey == "equals" ? "test@km.com" :
                searchKey == "ends" ? "%test@km.com" :
                searchKey == "starts" ? "test@km.com%" : "%test@km.com%";

            // Act, Assert
            testObject.searchString.ShouldBe(" AND EmailAddress like '"+email+"' AND SubscribeTypeCode = 'subcribeType'");
        }

        [Test]
        public void BtnAddDomainSuppression_click_Success([Values(true,false)]bool withChannel)
        {
            // Arrange
            InitilizeTestObjects();
            var rbType = privateObject.GetFieldOrProperty("rbType") as RadioButtonList;
            rbType.Items.Add("Select");
            rbType.Items.Add("Customer");
            rbType.Items.Add("Channel");
            rbType.SelectedIndex = 0;
            var lblDomainSuppressionID = privateObject.GetFieldOrProperty("lblDomainSuppressionID") as Label;
            lblDomainSuppressionID.Text = "0";
            var btnAddDomainSuppression = privateObject.GetFieldOrProperty("btnAddDomainSuppression") as Button;
            if (withChannel)
            {
                rbType.SelectedValue = "Channel";
                lblDomainSuppressionID.Text = "1";
            }
            ShimDomainSuppression.SaveDomainSuppressionUser = (p1, p2) => { };
            ShimDomainSuppression.GetByDomainStringNullableOfInt32NullableOfInt32User = 
                (p1, p2, p3, p4) => new List<DomainSuppression> { new DomainSuppression { } };

            // Act
            privateObject.Invoke("btnAddDomainSuppression_click", new object[] { null, null });

            // Assert
            rbType.SelectedValue.ShouldBe("Customer");
            lblDomainSuppressionID.Text.ShouldBe("0");
            btnAddDomainSuppression.Text.ShouldBe("Add Domain Suppression");
        }

        [Test]
        public void BtnAddDomainSuppression_click_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            var rbType = privateObject.GetFieldOrProperty("rbType") as RadioButtonList;
            rbType.Items.Add("Select");
            rbType.Items.Add("Customer");
            rbType.Items.Add("Channel");
            rbType.SelectedIndex = 0;
            var lblDomainSuppressionID = privateObject.GetFieldOrProperty("lblDomainSuppressionID") as Label;
            lblDomainSuppressionID.Text = "0";
            ShimDomainSuppression.SaveDomainSuppressionUser = (p1, p2) =>
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.DomainSuppression,Enums.Method.Save, "Test Exception")});
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;


            // Act
            privateObject.Invoke("btnAddDomainSuppression_click", new object[] { null, null });

            // Assert
            phError.Visible.ShouldBeTrue();
            lblErrorMessage.Text.ShouldBe("<br/>DomainSuppression: Test Exception");
        }

        [Test]
        public void TabContainer_ActiveTabChanged_Success([Range(0,4)]int index)
        {
            // Arrange
            InitilizeTestObjects();
            var tabContainer = privateObject.GetFieldOrProperty("TabContainer") as TabContainer;
            tabContainer.ActiveTabIndex = index;
            var indexReturn = -1;
            ShimSuppression.AllInstances.loadSuppressionGroupGrid = (p) => indexReturn = 0;
            ShimSuppression.AllInstances.loadDomainSuppressionGrid = (p) => indexReturn = 1;
            ShimSuppression.AllInstances.loadChannelMasterSuppressionGrid = (p) => indexReturn = 2;
            ShimSuppression.AllInstances.loadNoThresholdEmailsGrid = (p) => indexReturn = 3;
            ShimSuppression.AllInstances.loadGlobalMasterSuppressionGrid = (p) => indexReturn = 4;

            // Act
            privateObject.Invoke("TabContainer_ActiveTabChanged", new object[] { null, null });

            // Assert
            indexReturn.ShouldBe(index);
        }

        [Test]
        public void EmailFilterButton_Click_Success([Values(true,false)]bool checkActivity)
        {
            // Arrange
            InitilizeTestObjects();
            var chkRecentActivity = privateObject.GetFieldOrProperty("chkRecentActivity") as CheckBox;
            chkRecentActivity.Checked = checkActivity;
            ShimSuppression.AllInstances.loadSuppressionGroupGrid = (p) => { };
            var emailsPager = privateObject.GetFieldOrProperty("EmailsPager") as ActiveUp.WebControls.PagerBuilder;
            ShimSuppression.AllInstances.searchStringGet = (p) => "search";

            // Act
            privateObject.Invoke("EmailFilterButton_Click", new object[] { null, null });

            // Assert
            emailsPager.CurrentPage.ShouldBe(1);
            emailsPager.CurrentIndex.ShouldBe(0);
        }

        [Test]
        public void StartOfWeek_Success([Range(0, 6)]int day)
        {
            // Arrange
            InitilizeTestObjects();
            var baseDate = new DateTime(2018, 4, 8);

            // Act
            var result = (DateTime)privateObject.Invoke("StartOfWeek", new object[] { baseDate.AddDays(day) });

            // Assert
            result.ShouldBe(baseDate);
        }

        [Test]
        public void EndOfWeek_Success([Range(0, 6)]int day)
        {
            // Arrange
            InitilizeTestObjects();
            var baseDate = new DateTime(2018, 4, 7);

            // Act
            var result = (DateTime)privateObject.Invoke("EndOfWeek", new object[] { baseDate.AddDays(day * -1) });

            // Assert
            result.ShouldBe(baseDate);
        }

        [Test]
        public void GvNoThreshold_RowDataBound_Pager_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var row = new GridViewRow(0, 0, DataControlRowType.Pager, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalRecordsNoThreshold" });
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalNumberOfPagesNoThreshold" });
            var txtGoToPageNoThreshold = new TextBox { ID = "txtGoToPageNoThreshold" };
            row.Cells[0].Controls.Add(txtGoToPageNoThreshold);
            var arg = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvNoThreshold_RowDataBound", new object[] { null, arg});

            // Assert
            txtGoToPageNoThreshold.Text.ShouldBe("1");
        }

        [Test]
        public void GvNoThreshold_RowDataBound_DataRow_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            var deleteEmailBTN = new LinkButton { ID = "deleteEmailBTN" };
            row.Cells[0].Controls.Add(deleteEmailBTN);
            var arg = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvNoThreshold_RowDataBound", new object[] { null, arg });

            // Assert
            deleteEmailBTN.Attributes["onclick"].ShouldBe("return confirm('Email Address \"\" will be removed from the No Threshold List!!\\nThis process will enable \"\" to be suppressed by Threshold Suppression for campaigns you have scheduled / will be sending in the future.\\n\\nAre you sure you want to contine?This process CANNOT be undone.');");
        }

        [Test]
        public void GvDomainSuppression_RowDataBound_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var row = new GridViewRow(0, 0, DataControlRowType.Pager, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalRecordsDomain" });
            row.Cells[0].Controls.Add(new Label { ID = "lblTotalNumberOfPagesDomain" });
            var txtGoToPageDomain = new TextBox { ID = "txtGoToPageDomain" };
            row.Cells[0].Controls.Add(txtGoToPageDomain);
            var arg = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvDomainSuppression_RowDataBound", new object[] { null, arg });

            // Assert
            txtGoToPageDomain.Text.ShouldBe("1");
        }

        [Test]
        public void GvChannelMasterSuppression_RowDataBound_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            var deleteEmailBTN = new LinkButton { ID = "deleteEmailBTN" };
            row.Cells[0].Controls.Add(deleteEmailBTN);
            var arg = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvChannelMasterSuppression_RowDataBound", new object[] { null, arg });

            // Assert
            deleteEmailBTN.Attributes["onclick"].ShouldBe("return confirm('Email Address \"\" will be removed from the Master Suppression List!\\nThis process will enable \"\" to start receiving the campaigns that you have scheduled / will be sending in the future.\\n\\nAre you sure you want to continue? This process CANNOT be undone.');");
        }

        [Test]
        public void GvGlobalMasterSuppression_RowDataBound_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.Cells.Add(new TableCell());
            var deleteEmailBTN = new LinkButton { ID = "deleteEmailBTN" };
            row.Cells[0].Controls.Add(deleteEmailBTN);
            var arg = new GridViewRowEventArgs(row);

            // Act
            privateObject.Invoke("gvGlobalMasterSuppression_RowDataBound", new object[] { null, arg });

            // Assert
            deleteEmailBTN.Attributes["onclick"].ShouldBe("return confirm('Email Address \"\" will be removed from the Global Suppression List!\\nThis process will enable \"\" to start receiving the campaigns that you have scheduled / will be sending in the future.\\n\\nAre you sure you want to continue? This process CANNOT be undone.');");
        }

        private void InitilizeExportFakes()
        {
            ShimDirectory.ExistsString = (p) => false;
            ShimDirectory.CreateDirectoryString = (p) => null;
            ShimFile.ExistsString = (p) => true;
            ShimFile.DeleteString = (p) => { };
            ShimHttpServerUtility.AllInstances.MapPathString = (p1, p2) => string.Empty;
            ShimFile.AppendTextString = (p) => new ShimStreamWriter();
            _fileText = string.Empty;
            ShimTextWriter.AllInstances.WriteLineString = (p1, p2) => _fileText = p2;
            _fileName = string.Empty;
            ShimHttpResponse.AllInstances.WriteFileString = (p1, p2) => _fileName = p2;
            ShimHttpResponse.AllInstances.End = (p) => { };
        }

        private void InitializeGrids()
        {
            var gvDomainSuppression = privateObject.GetFieldOrProperty("gvDomainSuppression") as GridView;
            gvDomainSuppression.Columns.Add(new BoundField());
            gvDomainSuppression.Columns.Add(new BoundField());
            gvDomainSuppression.Columns.Add(new BoundField());
            gvDomainSuppression.Columns.Add(new BoundField());
            var gvSuppressionGroup = privateObject.GetFieldOrProperty("gvSuppressionGroup") as GridView;
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            gvSuppressionGroup.Columns.Add(new BoundField());
            var gvChannelMasterSuppression = privateObject.GetFieldOrProperty("gvChannelMasterSuppression") as GridView;
            gvChannelMasterSuppression.Columns.Add(new BoundField());
            gvChannelMasterSuppression.Columns.Add(new BoundField());
            gvChannelMasterSuppression.Columns.Add(new BoundField());
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
        }

        private void InitilizeFakes()
        {
            _viewState = new StateBag();
            ShimControl.AllInstances.ViewStateGet = (p) => _viewState; 
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
        }

        protected override void InitializeFields(string chId, string custId, string grpId, string fileType, string subType,
			string srchType, string srchEm, string filterId, string profFilter)
		{
			var subscribeFilterType = new DropDownList();
			subscribeFilterType.Items.Add(subType);
			subscribeFilterType.Items[0].Selected = true;
			var emailFilter = new TextBox {Text = srchEm};
			var searchType = new DropDownList();
			searchType.Items.Add(srchType);
			searchType.Text = srchType;
			var chID_Hidden = new HtmlInputHidden {Value = chId};
			var custID_Hidden = new HtmlInputHidden {Value = custId};
			var grpID_Hidden = new HtmlInputHidden {Value = grpId};
			var filteredDownloadType = new DropDownList();
			filteredDownloadType.Items.Add(fileType);
			filteredDownloadType.Items[0].Selected = true;
			var dateFilter = new TextBox {Text = DateTime.Now.ToString()};
			var chkRecentActivity = new CheckBox {Checked = false};

			privateObject.SetField("SubscribeTypeFilter", subscribeFilterType);
			privateObject.SetField("EmailFilter", emailFilter);
			privateObject.SetField("SearchEmailLike", searchType);
			privateObject.SetField("chID_Hidden", chID_Hidden);
			privateObject.SetField("custID_Hidden", custID_Hidden);
			privateObject.SetField("grpID_Hidden", grpID_Hidden);
			privateObject.SetField("FilteredDownloadType", filteredDownloadType);
			privateObject.SetField("DateFromFilter", dateFilter);
			privateObject.SetField("DateToFilter", dateFilter);
			privateObject.SetField("chkRecentActivity", chkRecentActivity);
		}
	}
}