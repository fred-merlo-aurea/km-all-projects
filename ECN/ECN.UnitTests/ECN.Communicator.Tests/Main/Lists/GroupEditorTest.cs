using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ecn.communicator.main.lists.reports;
using ecn.communicator.main.lists.reports.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
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
    /// UT for <see cref="groupeditor" />
    /// </summary>
    [TestFixture]
	[ExcludeFromCodeCoverage]
	public partial class GroupEditorTest : BaseListsTest<groupeditor>
	{
		private const string MethodBtnExportGroupclick = "btnExportGroup_Click";

		[Test]
		public void BtnExportGroupClick_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault_NoEmail()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
				string.Empty, string.Empty, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBeEmpty(),
				() => fileText.ShouldBeEmpty());
		}

		[Test]
		public void BtnExportGroupClick_SubscribeTypeDefault_SearchTypeDefault_DownloadTypeDefault()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, string.Empty, string.Empty,
				string.Empty, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBeEmpty(),
				() => fileText.ShouldBeEmpty());
		}

		[Test]
		public void BtnExportGroupClick_SubscribeTypeAll_SearchTypeLike_DownloadTypeXls()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXls, SubscribeTypeAll,
				SearchTypeLike, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(XlsContentType),
				() => fileText.ShouldBe(XlsFileText),
				() => responseHeader.ShouldContain(Emails + DownloadTypeXls),
				() => responseText.ShouldContain(Emails + DownloadTypeXls));
		}

		[Test]
		public void BtnExportGroupClick_SubscribeTypeDefault_SearchTypeEquals_DownloadTypeCsv()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeCsv, string.Empty,
				SearchTypeEquals, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(CsvContentType),
				() => fileText.ShouldBe(TxtFileText),
				() => responseHeader.ShouldContain(Emails + DownloadTypeCsv),
				() => responseText.ShouldContain(Emails + DownloadTypeCsv));
		}

		[Test]
		public void BtnExportGroupClick_SubscribeTypeDefault_SearchTypeStarts_DownloadTypeXml()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeXml, string.Empty,
				SearchTypeStarts, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

			// Assert
			testObject.ShouldSatisfyAllConditions(
				() => contentType.ShouldBe(XmlContentType),
				() => fileText.ShouldBeEmpty(),
				() => responseHeader.ShouldContain(Emails + DownloadTypeXml),
				() => responseText.ShouldContain(Emails + DownloadTypeXml));
		}

		[Test]
		public void BtnExportGroupClick_SubscribeTypeDefault_SearchTypeEnds_DownloadTypeTxt()
		{
			// Arrange
			InitializeTests(SampleChannelId1, SampleCustomerId1, SampleGroupId1, DownloadTypeTxt, string.Empty,
				SearchTypeEnds, SampleEmail, SampleFilterId1, string.Empty);

			// Act
			privateObject.Invoke(MethodBtnExportGroupclick, null, new EventArgs());

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

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void Page_Load_HasAccess_HasFeature_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { FolderID = 1 };
            InitializePageLoadFields();
            var SeedListPanel = privateObject.GetFieldOrProperty("SeedListPanel") as Panel;
            var FilterPanel = privateObject.GetFieldOrProperty("FilterPanel") as Panel;            

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBeEmpty(),
                () => SeedListPanel.Visible.ShouldBeTrue(),
                () => FilterPanel.Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_HasAccess_NoFeature_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => false;
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { FolderID = 1, MasterSupression = 1 };
            InitializePageLoadFields();
            var seedListPanel = privateObject.GetFieldOrProperty("SeedListPanel") as Panel;
            var filterPanel = privateObject.GetFieldOrProperty("FilterPanel") as Panel;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBeEmpty(),
                () => seedListPanel.Visible.ShouldBeFalse(),
                () => filterPanel.Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_HasAccess_NoGroup_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => false;
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { FolderID = 1, MasterSupression = 1 };
            InitializePageLoadFields();
            var SeedListPanel = privateObject.GetFieldOrProperty("SeedListPanel") as Panel;
            var FilterPanel = privateObject.GetFieldOrProperty("FilterPanel") as Panel;
            QueryString["GroupID"] = "0";

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBeEmpty(),
                () => SeedListPanel.Visible.ShouldBeFalse(),
                () => FilterPanel.Visible.ShouldBeFalse());
        }

        [Test]
        public void CreateGroup_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializeGroupTestFields();
            ShimGroup.SaveGroupUser = (p1, p2) => 0;
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;
            phError.Visible = false;

            // Act
            privateObject.Invoke("CreateGroup", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("default.aspx"),
                () => phError.Visible.ShouldBeFalse(),
                () => lblErrorMessage.Text.ShouldBeEmpty());
        }

        [Test]
        public void CreateGroup_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            InitializeGroupTestFields();
            ShimGroup.SaveGroupUser = (p1, p2) => throw new ECNException(new List<ECNError> { new ECNError( Enums.Entity.Customer, Enums.Method.Create, "Test Exception") });
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;
            phError.Visible = false;

            // Act
            privateObject.Invoke("CreateGroup", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBeEmpty(),
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Customer: Test Exception"));
        }

        [Test]
        public void UpdateGroup_Success()
        {
            // Arrange
            InitilizeTestObjects();
            InitializeGroupTestFields();
            (privateObject.GetFieldOrProperty("GroupID") as TextBox).Text = "1";
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { };
            ShimGroup.SaveGroupUser = (p1, p2) => 0;
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;
            phError.Visible = false;

            // Act
            privateObject.Invoke("UpdateGroup", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("default.aspx"),
                () => phError.Visible.ShouldBeFalse(),
                () => lblErrorMessage.Text.ShouldBeEmpty());
        }

        [Test]
        public void UpdateGroup_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            InitializeGroupTestFields();
            (privateObject.GetFieldOrProperty("GroupID") as TextBox).Text = "1";
            ShimGroup.GetByGroupIDInt32User = (p1, p2) => new Group { };
            ShimGroup.SaveGroupUser = (p1, p2) => throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Create, "Test Exception") });
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;
            phError.Visible = false;

            // Act
            privateObject.Invoke("UpdateGroup", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBeEmpty(),
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Customer: Test Exception"));
        }

        [Test]
        public void SearchString_Get_Success([Values("like", "equals", "ends", "starts", "default")]string searchKey)
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
            testObject.searchString.ShouldBe(" AND EmailAddress like '" + email + "' AND SubscribeTypeCode = 'subcribeType'");
        }

        private void InitializeGroupTestFields()
        {
            (privateObject.GetFieldOrProperty("PublicFolder") as CheckBox).Checked = true;
            var folderID = privateObject.GetFieldOrProperty("FolderID") as DropDownList;
            folderID.Items.Add("1");
            folderID.SelectedValue = "1";
            var rbSeedList = privateObject.GetFieldOrProperty("rbSeedList") as RadioButtonList;
            rbSeedList.Items.Add("True");
            rbSeedList.SelectedValue = "True";
        }

        private void InitializePageLoadFields()
        {
            var dataSet = new DataSet { };
            var dataTable = new DataTable { Columns = { "counts" }, Rows = { { "1" } } };
            dataSet.Tables.Add(dataTable);
            dataSet.Tables.Add(new DataTable());
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32String = (p1, p2, p3, p4, p5) => dataSet;
            ShimFolder.GetByTypeInt32StringUser = (p1, p2, p3) => new List<Folder> { new Folder { } };
            var rbSeedList = privateObject.GetFieldOrProperty("rbSeedList") as RadioButtonList;
            rbSeedList.Items.Add("True");
            rbSeedList.Items.Add("False");
            var SearchEmailLike = privateObject.GetFieldOrProperty("SearchEmailLike") as DropDownList;
            SearchEmailLike.Items.Add("like");
            SearchEmailLike.SelectedValue = "like";
            var SubscribeTypeFilter = privateObject.GetFieldOrProperty("SubscribeTypeFilter") as DropDownList;
            SubscribeTypeFilter.Items.Add("*");
            SubscribeTypeFilter.SelectedValue = "*";
            var emailsGrid = privateObject.GetFieldOrProperty("EmailsGrid") as ecnGridView;
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            emailsGrid.Columns.Add(new BoundField());
            QueryString.Add("GroupID", "1");
            QueryString.Add("Value", "value");
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            QueryString = new System.Collections.Specialized.NameValueCollection { };
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
			ShimGroupExportUdfSetting.AllInstances.selectedGet = obj => profFilter;
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
			var ddlFilteredDownloadOnly = new GroupExportUdfSetting();

			privateObject.SetField("SubscribeTypeFilter", subscribeFilterType);
			privateObject.SetField("EmailFilter", emailFilter);
			privateObject.SetField("SearchEmailLike", searchType);
			privateObject.SetField("chID_Hidden", chID_Hidden);
			privateObject.SetField("custID_Hidden", custID_Hidden);
			privateObject.SetField("grpID_Hidden", grpID_Hidden);
			privateObject.SetField("FilteredDownloadType", filteredDownloadType);
			privateObject.SetField("ddlFilteredDownloadOnly", ddlFilteredDownloadOnly);
		}
	}
}