using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.communicator.main.ECNWizard.Content.Fakes;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using EntitiesGroup = ECN_Framework_Entities.Communicator.Group;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.OtherControls
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class AddTemplateTest
	{
		private const string SampleHost = "km.com";
		private const string SampleHttpHost = "http://km.com";
		private const string SampleHostPath = "http://km.com/addTemplate";
		private const string SampleUserAgent = "http://km.com/addTemplate";
		private const string CampaignItemTemplateID = "1";
		private const string KMCommon_Application = "1";
		private const string MethodLoadData = "loadData";
		private const string MethodSave = "Save";
		private const string MethodReset = "Reset"; 
		private const string MethodSuppressionGroupClick = "lnkSelectSuppressionGroup_Click"; 
		private const string MethodLinkSelectGroupClick = "lnkSelectGroup_Click"; 
		private const string MethodImgCleanClick = "imgCleanSelectedLayout_Click";
		private const string MethodImgLayoutTrigger = "imgSelectLayoutTrigger_Click";
		private const string MethodSelectedGroupsRowDataBound = "gvSelectedGroups_RowDataBound";
		private const string MehtodLoadOmnitureFieldsData = "loadOmnitureFieldsData";
		private const string XMLConfig = "<Settings><AllowCustomerOverride>false</AllowCustomerOverride><Override>false</Override></Settings>";
		private const string XMLConfigWithAllowOverride = "<Settings><AllowCustomerOverride>true</AllowCustomerOverride><Override>true</Override></Settings>";
		private LinkTrackingParamSettings _linkTrackingParamSettings;
		private StateBag _viewState;
		private Page _page;
		private CampaignItemTemplate _campaignItemTemplate;
		private IDisposable _context;
		private object[] _methodArgs;
		private Type _typeAddTemplate;
		private AddTemplate _objectAddTemplate;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		private void Initialize()
		{
			_typeAddTemplate = typeof(AddTemplate);
			_objectAddTemplate = CreateInstance(_typeAddTemplate);
			InitializeSession();
		}

		[Test]
		public void LoadData_Successfull_DataIsLoadedInView()
		{
			// Arrange
			Initialize();
			var isDataLoaded = false;
			CreateShims(MethodLoadData);
			_methodArgs = null;
			ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (x, y, z) =>
			{
				isDataLoaded = true;
				return _campaignItemTemplate;
			};

			// Act
			CallMethod(_typeAddTemplate, MethodLoadData, _methodArgs, _objectAddTemplate);

			// Assert
			isDataLoaded.ShouldBeTrue();
		}

		[Test]
		public void LoadData_WhenOptOutIsMasterSuppression_RespectiveCheckIsEnabled()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_methodArgs = null;
			_campaignItemTemplate.OptOutSpecificGroup = (bool?)false;
			_campaignItemTemplate.OptOutMasterSuppression = (bool?)true;
			// Act
			CallMethod(_typeAddTemplate, MethodLoadData, _methodArgs, _objectAddTemplate);

			// Assert
			var masterSuppressionChecked = (GetField(_objectAddTemplate, "chkOptOutMasterSuppression") as CheckBox)?.Checked ?? false;
			masterSuppressionChecked.ShouldBeTrue();
		}

		[Test]
		public void LoadData_WhenchkOptOutIsSpecificGroup_RespectiveCheckIsEnabled()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_methodArgs = null;
			_campaignItemTemplate.OptOutMasterSuppression = (bool?)false;
			_campaignItemTemplate.OptOutSpecificGroup = (bool?)true;

			// Act
			CallMethod(_typeAddTemplate, MethodLoadData, _methodArgs, _objectAddTemplate);

			// Assert
			var specificGroupChecked = (GetField(_objectAddTemplate, "chkOptOutSpecificGroup") as CheckBox)?.Checked ?? false;
			specificGroupChecked.ShouldBeTrue();
		}

		[Test]
		public void lnkSelectGroup_Click_Success_SelectionModeChangesToGroup()
		{
			// Arrange
			Initialize();
			var mode = "SelectGroup";
			_methodArgs = new object[] { null, EventArgs.Empty };
			SetField(_objectAddTemplate, "modalPopupGroupExplorer", new ModalPopupExtender());
			ShimgroupsLookup.AllInstances.LoadControlBoolean = (x, y) => { };

			// Act
			CallMethod(_typeAddTemplate, MethodLinkSelectGroupClick, _methodArgs, _objectAddTemplate);

			// Assert
			var selectionMode = (GetField(_objectAddTemplate, "hfGroupSelectionMode") as HiddenField).Value;
			selectionMode.ShouldBe(mode);
		}

		[Test] 
		public void lnkSelectSuppressionGroup_Click_Success_SelectionModeChangesToSuppressGroup()
		{
			// Arrange
			Initialize();
			var mode = "SuppressGroup";
			_methodArgs = new object[] { null, EventArgs.Empty };
			SetField(_objectAddTemplate, "modalPopupGroupExplorer", new ModalPopupExtender());
			ShimgroupsLookup.AllInstances.LoadControlBoolean = (x, y) => { };

			// Act
			CallMethod(_typeAddTemplate, MethodSuppressionGroupClick, _methodArgs, _objectAddTemplate);

			// Assert
			var selectionMode = (GetField(_objectAddTemplate, "hfGroupSelectionMode") as HiddenField).Value;
			selectionMode.ShouldBe(mode);
		}

		[Test]
		public void imgCleanSelectedLayout_Click_Success_TriggerValueChanges()
		{
			// Arrange
			Initialize();
			var layoutTrigger = "-No Message Selected-";
			_methodArgs = new object[] { null, EventArgs.Empty };
			SetField(_objectAddTemplate, "modalPopupGroupExplorer", new ModalPopupExtender());
			ShimgroupsLookup.AllInstances.LoadControlBoolean = (x, y) => { };

			// Act
			CallMethod(_typeAddTemplate, MethodImgCleanClick, _methodArgs, _objectAddTemplate);

			// Assert
			var triggerText = (GetField(_objectAddTemplate, "lblSelectedLayoutTrigger") as Label).Text;
			triggerText.ShouldBe(layoutTrigger);
		}

		[Test]
		public void imgSelectLayoutTrigger_Click_Success_LayoutValueChanges()
		{
			// Arrange
			Initialize();
			var layoutTrigger = "Trigger";
			_methodArgs = new object[] { null, EventArgs.Empty };
			ShimlayoutExplorer.AllInstances.reset = (x) => { };
			ShimlayoutExplorer.AllInstances.enableShowArchivedOnlyMode = (x) => { }; 

			// Act
			CallMethod(_typeAddTemplate, MethodImgLayoutTrigger, _methodArgs, _objectAddTemplate);

			// Assert
			var triggerText = (GetField(_objectAddTemplate, "hfWhichLayout") as HiddenField).Value;
			triggerText.ShouldBe(layoutTrigger);
		}

		[Test]
		public void gvSelectedGroups_RowDataBound_Success_FiltersForGridAreSet()
		{
			// Arrange
			Initialize();
			var filtersSet = false;
			_methodArgs = new object[] { null, GetGridViewArgument() };
			ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => CreateInstance(typeof(EntitiesGroup));
			Shimfiltergrid.AllInstances.SetFiltersGroupObjectInt32Boolean = (a, b, c, d) => 
			{
				filtersSet = true;
			};

			// Act
			CallMethod(_typeAddTemplate, MethodSelectedGroupsRowDataBound, _methodArgs, _objectAddTemplate);

			// Assert
			filtersSet.ShouldBeTrue();
		}

		[Test]
		public void Save_Successfull_CampaignItemGetsSaved()
		{
			// Arrange
			Initialize();
			var isDataSaved = false;
			CreateShims(MethodSave);
			SetDefaults();
			_methodArgs = null;
			ShimCampaignItemTemplate.SaveCampaignItemTemplateUser = (x, y) =>
			{
				isDataSaved = true;
				return 0;
			};

			// Act
			CallMethod(_typeAddTemplate, MethodSave, _methodArgs, _objectAddTemplate);

			// Assert
			isDataSaved.ShouldBeTrue();
		}

		[Test]
		public void Save_WithSelectedGroups_SelectedGroupsGetsSaved()
		{
			// Arrange
			Initialize();
			var selectedGroupSaved = false;
			CreateShims(MethodSave);
			SetDefaults();
			_methodArgs = null;
			SetField(_objectAddTemplate, "chkOptOutSpecificGroup", new CheckBox { Checked = true });
			ShimCampaignItemTemplateGroup.SaveCampaignItemTemplateGroupUser = (x, y) =>
			{
				selectedGroupSaved = true;
				return 0;
			};

			// Act
			CallMethod(_typeAddTemplate, MethodSave, _methodArgs, _objectAddTemplate);

			// Assert
			selectedGroupSaved.ShouldBeTrue();
		}

		[Test]
		public void Save_WithSelectedOptOutGroups_SelectedGroupsGetsSaved()
		{
			// Arrange
			Initialize();
			var optOutGroupSaved = false;
			CreateShims(MethodSave);
			SetDefaults();
			_methodArgs = null;
			SetField(_objectAddTemplate, "chkOptOutSpecificGroup", new CheckBox { Checked = true });
			ShimCampaignItemTemplateOptoutGroup.SaveCampaignItemTemplateOptoutGroupUser = (x, y) =>
			{
				optOutGroupSaved = true;
				return 0;
			};

			// Act
			CallMethod(_typeAddTemplate, MethodSave, _methodArgs, _objectAddTemplate);

			// Assert
			optOutGroupSaved.ShouldBeTrue();
		}

		[Test]
		public void Reset_Successful_AllFieldAreReset()
		{
			// Arrange
			Initialize();
			CreateShims(MethodReset);
			SetDefaults();
			_methodArgs = null;

			// Act
			CallMethod(_typeAddTemplate, MethodReset, _methodArgs, _objectAddTemplate);

			// Assert
			var fromEmail = (GetField(_objectAddTemplate, "txtFromEmail") as TextBox).Text;
			var fromName = (GetField(_objectAddTemplate, "txtFromName") as TextBox).Text;
			var replyTo = (GetField(_objectAddTemplate, "txtReplyTo") as TextBox).Text;
			var subject = (GetField(_objectAddTemplate, "txtSubject") as TextBox).Text;
			_objectAddTemplate.ShouldSatisfyAllConditions(
				() => fromEmail.ShouldBe(string.Empty),
				() => fromName.ShouldBe(string.Empty),
				() => replyTo.ShouldBe(string.Empty),
				() => subject.ShouldBe(string.Empty));
		}


		[Test]
		public void loadOmnitureFieldsData_WhenAllowOverride_CustomerIdIsUsedToGetData()
		{
			// Arrange
			Initialize();
			CreateShims(MehtodLoadOmnitureFieldsData);
			SetDefaults();
			var loadedWithCustomerId = false;
			ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
			{
				loadedWithCustomerId = true;
				return _linkTrackingParamSettings;
			};

			// Act
			CallMethod(_typeAddTemplate, MehtodLoadOmnitureFieldsData, null, _objectAddTemplate);

			// Assert
			loadedWithCustomerId.ShouldBeTrue();
		}

		[Test]
		public void loadOmnitureFieldsData_WhenAllowOverride_BaseChannelIdIsUsedToGetData()
		{
			// Arrange
			Initialize();
			CreateShims(MehtodLoadOmnitureFieldsData);
			SetDefaults();
			var loadedWithBaseChannelId = false;
			var linkTrackingSettings = CreateInstance(typeof(LinkTrackingSettings));
			linkTrackingSettings.XMLConfig = XMLConfig;
			ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => linkTrackingSettings;
			ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (x, y) =>
			{
				loadedWithBaseChannelId = true;
				return _linkTrackingParamSettings;
			};

			// Act
			CallMethod(_typeAddTemplate, MehtodLoadOmnitureFieldsData, null, _objectAddTemplate);

			// Assert
			loadedWithBaseChannelId.ShouldBeTrue();
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void CreateShims(string method)
		{
			if (method == MethodLoadData || method == MethodSave || method == MethodReset || method == MehtodLoadOmnitureFieldsData)
			{
				_campaignItemTemplate = CreateInstance(typeof(CampaignItemTemplate));
				var campaignItemTemplateGroupObject = CreateInstance(typeof(CampaignItemTemplateGroup));
				var campaignItemTemplateGroupObjectList = new List<CampaignItemTemplateGroup> { campaignItemTemplateGroupObject };
				var layout = CreateInstance(typeof(Layout));
				var campaign = CreateInstance(typeof(Campaign));
				var campaignDropDownList = new DropDownList();
				var campaignItemTemplateSuppressionGroupObject = CreateInstance(typeof(CampaignItemTemplateSuppressionGroup));
				var campaignItemTemplateSuppressionGroupList = new List<CampaignItemTemplateSuppressionGroup> { campaignItemTemplateSuppressionGroupObject };
				var campaignItemTemplateOptoutGroupObject = CreateInstance(typeof(CampaignItemTemplateOptoutGroup));
				var campaignItemTemplateOptoutGroupList = new List<CampaignItemTemplateOptoutGroup> { campaignItemTemplateOptoutGroupObject };
				campaignDropDownList.Items.Add(campaign.CampaignName);
				SetField(_objectAddTemplate, "drpdownCampaign", campaignDropDownList);
				InitializeOmnitureDropDownLists(_campaignItemTemplate);
				ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (x, y, z) => _campaignItemTemplate;
				ShimCampaignItemTemplateGroup.GetByCampaignItemTemplateIDInt32 = (x) => campaignItemTemplateGroupObjectList;
				ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (x, y) => layout;
				ShimCampaign.GetByCampaignID_NoAccessCheckInt32Boolean = (x, y) => campaign;
				ShimCampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateIDInt32User = (x, y) => campaignItemTemplateSuppressionGroupList; //casehere for loadSuppressionGroupsGrid else
				ShimCampaignItemTemplateOptoutGroup.GetByCampaignItemTemplateIDInt32 = (x) => campaignItemTemplateOptoutGroupList;// casehere for loadOptoutGroupsGrid else
			}
			if (method == MethodSave)
			{
				ShimCampaignItemTemplate.SaveCampaignItemTemplateUser = (x, y) => 0;
				ShimCampaignItemTemplateGroup.DeleteByCampaignItemTemplateIDInt32User = (x, y) => { };
				ShimCampaignItemTemplateSuppressionGroup.DeleteByCampaignItemTemplateIDInt32User = (x, y) => { };
				ShimCampaignItemTemplateFilter.DeleteByCampaignItemTemplateIDInt32User = (x, y) => { };
				ShimCampaignItemTemplateOptoutGroup.DeleteByCampaignItemTemplateIDInt32User = (x, y) => { };
				ShimCampaignItemTemplateGroup.SaveCampaignItemTemplateGroupUser = (x, y) => 0;
				ShimCampaignItemTemplateSuppressionGroup.SaveCampaignItemTemplateSuppressionGroupUser = (x, y) => { };
				ShimCampaignItemTemplateOptoutGroup.SaveCampaignItemTemplateOptoutGroupUser = (x, y) => 0;
				ShimControl.AllInstances.ViewStateGet = (x) => _viewState;
			}
			if (method == MehtodLoadOmnitureFieldsData)
			{
				var linkTrackingObject = CreateInstance(typeof(LinkTracking));
				linkTrackingObject.DisplayName = "Omniture";
				var linkTrackingList = new List<LinkTracking>{ linkTrackingObject };
				var linkTrackingSettings = CreateInstance(typeof(LinkTrackingSettings));
				linkTrackingSettings.XMLConfig = XMLConfigWithAllowOverride;
				var linkTrackingParamOption = CreateInstance(typeof(LinkTrackingParamOption));
				var linkTrackingParamOptionList = new List<LinkTrackingParamOption> { linkTrackingParamOption };
				_linkTrackingParamSettings = CreateInstance(typeof(LinkTrackingParamSettings));
				ShimLinkTracking.GetAll = () => linkTrackingList;
				ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => linkTrackingSettings;
				ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) => linkTrackingSettings;
				ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (x) => GetTrackingParams();
				ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) => _linkTrackingParamSettings;
				ShimLinkTrackingParamOption.Get_LTPID_CustomerIDInt32Int32 = (x, y) => linkTrackingParamOptionList;
				ShimLinkTrackingParamSettings.Get_LTPID_BaseChannelIDInt32Int32 = (x, y) => _linkTrackingParamSettings;
				ShimLinkTrackingParamOption.Get_LTPID_BaseChannelIDInt32Int32 = (x, y) => linkTrackingParamOptionList;
			}
		}

		private List<LinkTrackingParam> GetTrackingParams()
		{
			var linkTrackingParamList = new List<LinkTrackingParam>();
			for (var i = 0; i < 11; i++)
			{
				var linkTrackingParam = CreateInstance(typeof(LinkTrackingParam));
				linkTrackingParam.DisplayName = string.Format("omniture{0}", i);
				linkTrackingParamList.Add(linkTrackingParam);
			}
			return linkTrackingParamList;
		}

		private void InitializeOmnitureDropDownLists(CampaignItemTemplate campaignItemTemplate)
		{
			var ddlOmnitures = new DropDownList();
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture1);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture2);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture3);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture4);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture5);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture6);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture7);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture8);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture9);
			ddlOmnitures.Items.Add(campaignItemTemplate.Omniture10);
			SetField(_objectAddTemplate, "ddlOmniture1", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture2", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture3", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture4", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture5", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture6", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture7", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture8", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture9", ddlOmnitures);
			SetField(_objectAddTemplate, "ddlOmniture10", ddlOmnitures);
		}

		private void SetDefaults()
		{
			ShimGridView.AllInstances.DataBind = (x) => { };
			var pnlOptOutSpecificGroups = CreateInstance(typeof(UpdatePanel));
			pnlOptOutSpecificGroups.UpdateMode = UpdatePanelUpdateMode.Conditional;
			SetField(_objectAddTemplate, "pnlOptOutSpecificGroups", pnlOptOutSpecificGroups);
			SetField(_objectAddTemplate, "upMain", pnlOptOutSpecificGroups);
			_viewState = new StateBag();
			var campaignItemTemplateGroupObject = CreateInstance(typeof(CampaignItemTemplateGroup));
			var campaignItemTemplateGroupList = new List<CampaignItemTemplateGroup> { campaignItemTemplateGroupObject };
			var campaignItemTemplateSuppressionGroup = CreateInstance(typeof(CampaignItemTemplateSuppressionGroup));
			var campaignItemTemplateSuppressionGroupList = new List<CampaignItemTemplateSuppressionGroup> { campaignItemTemplateSuppressionGroup };
			var campaignItemTemplateOptoutGroup = CreateInstance(typeof(CampaignItemTemplateOptoutGroup));
			var campaignItemTemplateOptoutGroupList = new List<CampaignItemTemplateOptoutGroup> { campaignItemTemplateOptoutGroup };
			_viewState["SelectedGroups"] = campaignItemTemplateGroupList;
			_viewState["SelectedSuppressionGroups"] = campaignItemTemplateSuppressionGroupList;
			_viewState["OptoutGroups_DT"] = campaignItemTemplateOptoutGroupList;
			SetField(_objectAddTemplate, "ViewState", _viewState);
			SetField(_objectAddTemplate, "drpdownCampaign", new DropDownList
			{
				Items =
				{
					new ListItem
					{
						Selected = true,
						Value = CampaignItemTemplateID
					}
				}
			});
		}

		private void InitializeSession()
		{
			ShimECNSession.AllInstances.RefreshSession = (item) => { };
			ShimECNSession.AllInstances.ClearSession = (itme) => { };
			var CustomerID = 1;
			var UserID = 1;
			var config = new NameValueCollection();
			var reqParams = new NameValueCollection();
			var queryString = new NameValueCollection();
			var dummyCustormer = CreateInstance(typeof(Customer));
			var dummyUser = CreateInstance(typeof(User));
			var authTkt = CreateInstance(typeof(ECN_Framework_Entities.Application.AuthenticationTicket));
			var ecnSession = CreateInstance(typeof(ECNSession));
			var baseChannel = CreateInstance(typeof(BaseChannel));
			config.Add("KMCommon_Application", KMCommon_Application);
			queryString.Add("HTTP_HOST", SampleHttpHost);
			queryString.Add("CampaignItemTemplateID", CampaignItemTemplateID);
			dummyCustormer.CustomerID = CustomerID;
			dummyUser.UserID = UserID;
			baseChannel.BaseChannelID = UserID;
			SetField(authTkt, "CustomerID", CustomerID);
			SetField(ecnSession, "CurrentUser", dummyUser);
			SetField(ecnSession, "CurrentCustomer", dummyCustormer);
			SetField(ecnSession, "CurrentBaseChannel", baseChannel);
			 _page = new Page();
			HttpContext.Current = MockHelpers.FakeHttpContext();
			ShimECNSession.CurrentSession = () => ecnSession;
			ShimAuthenticationTicket.getTicket = () => authTkt;
			ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
			ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
			ShimConfigurationManager.AppSettingsGet = () => config;
			ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
			ShimHttpRequest.AllInstances.QueryStringGet = (h) => queryString;
			ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
			ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);
			ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
			ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
			ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
			ShimControl.AllInstances.ParentGet = (control) => _page;
			InitializeAllControls(_objectAddTemplate);
			SetDefaults();
		}

		private void InitializeAllControls(object page)
		{
			var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (var field in fields)
			{
				if (field.GetValue(page) == null)
				{
					var constructor = field.FieldType.GetConstructor(new Type[0]);
					if (constructor != null)
					{
						var obj = constructor.Invoke(new object[0]);
						if (obj != null)
						{
							field.SetValue(page, obj);
							TryLinkFieldWithPage(obj, page);
						}
					}
				}
			}
		}

		private void TryLinkFieldWithPage(object field, object page)
		{
			if (page is Page)
			{
				var fieldType = field.GetType().GetField("_page", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				if (fieldType != null)
				{
					try
					{
						fieldType.SetValue(field, page);
					}
					catch (Exception ex)
					{
						Trace.TraceError($"Unable to set value as :{ex}");
					}
				}
			}
		}

		private GridViewRowEventArgs GetGridViewArgument()
		{
			var gridViewRow = new GridViewRow(1, 1, DataControlRowType.DataRow, DataControlRowState.Insert);
			gridViewRow.DataItem = CreateInstance(typeof(CampaignItemTemplateGroup));
			gridViewRow.Cells.Add(new TableCell());
			gridViewRow.Cells.Add(new TableCell());
			gridViewRow.Cells.Add(new TableCell());
			gridViewRow.Cells[0].Controls.Add(new Label { ID = "lblGroupName" });
			gridViewRow.Cells[1].Controls.Add(new ImageButton { ID = "imgbtnDeleteGroup" });
			gridViewRow.Cells[2].Controls.Add(new filtergrid { ID = "fgGroupFilterGrid" });
			var gridViewArgsObject = new GridViewRowEventArgs(gridViewRow);
			return gridViewArgsObject;
		}

		private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private dynamic GetField(dynamic obj, string fieldName)
		{
			return ReflectionHelper.GetField(obj, fieldName);
		}

		private void SetSessionVariable(string name, object value)
		{
			HttpContext.Current.Session.Add(name, value);
		}

		private void SetProperty(dynamic instance, string propertyName, dynamic value)
		{
			ReflectionHelper.SetProperty(instance, propertyName, value);
		}

		private dynamic GetProperty(dynamic instance, string propertyName)
		{
			return ReflectionHelper.GetPropertyValue(instance, propertyName);
		}
	}
}