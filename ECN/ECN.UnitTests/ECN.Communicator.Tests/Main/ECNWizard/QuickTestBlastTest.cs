using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard;
using ecn.communicator.main.ECNWizard.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using EntitiesGroup = ECN_Framework_Entities.Communicator.Group;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class QuickTestBlastTest
	{
		private const string SampleHost = "km.com";
		private const string SampleHttpHost = "http://km.com";
		private const string SampleHostPath = "http://km.com/addTemplate";
		private const string SampleUserAgent = "http://km.com/addTemplate";
		private const int CampaignItemID = 1;
		private const string KMCommon_Application = "1";
		private const string MethodLoadData = "loadData";
		private const string MethodGetShortNamesForLayoutTemplate = "GetShortNamesForLayoutTemplate";
		private const string MethodGetShortNamesForDynamicStrings = "GetShortNamesForDynamicStrings";
		private const string MethodSubmitQTBClick = "btnSubmitQTB_onclick";
		private const string DummyString = "dummy string";
		private const string EmptyString = "";
		private const string NegativeString = "-1";
		private const string NumberString = "1";
		private List<string> _layoutNames;
		private string _layoutNameValid = string.Format("%%{0}%%", DummyString);
		private string _layoutNamesWithFormatErrors = string.Format("%%{0}%", DummyString);
		private string _layoutNamesWithInvalidCharacters = string.Format("%%{0}$-#@%%", DummyString);
		private bool _errorThrown;
		private string _selectedCampaign;
		private bool _campaignChecked;
		private string _campaignName;
		private string _campaignItemName;
		private string _selectedGroup;
		private bool _groupChecked;
		private string _groupName;
		private string _address;
		private string _selectedLayoutTrigger;
		private int _groupId;
		private User _user;
		private Page _page;
		private IDisposable _context;
		private object[] _methodArgs;
		private Type _typeQuickBlastTest;
		private quicktestblast _objectQuickBlastTest;

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

		[Test]
		public void LoadData_WhenCampaignIDGreaterThanZero_CampaignIdIsUsedToLoadData()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_methodArgs = new object[] { CampaignItemID };
			var loadedUsingCampaignId = false;
			ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (x, y, z) =>
			{
				loadedUsingCampaignId = true;
				return CreateInstance(typeof(CampaignItem));
			};

			// Act
			CallMethod(_typeQuickBlastTest, MethodLoadData, _methodArgs, _objectQuickBlastTest);

			// Assert
			loadedUsingCampaignId.ShouldBeTrue();
		}

		[Test]
		public void LoadData_WhenCampaignIDLessThanZero_CustomerIdIsUsedToLoadData()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			var campaignItemID = 0;
			_methodArgs = new object[] { campaignItemID };
			var loadedUsingCustomerId = false;
			ShimCampaign.GetByCustomerIDInt32UserBoolean = (x, y, z) =>
			{
				loadedUsingCustomerId = true;
				var campaign = CreateInstance(typeof(Campaign));
				var campaignList = new List<Campaign> { campaign };
				return campaignList;
			};

			// Act
			CallMethod(_typeQuickBlastTest, MethodLoadData, _methodArgs, _objectQuickBlastTest);

			// Assert
			loadedUsingCustomerId.ShouldBeTrue();
		}

		[Test]
		public void LoadData_WhenTestBlastIdIsLessThanZero_CampaignItemDataIsUsed()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			var campaignItemTestBlastID = 0;
			_methodArgs = new object[] { CampaignItemID };
			var campaignItem = CreateInstance(typeof(CampaignItem));
			var campaignItemBlast = CreateInstance(typeof(CampaignItemBlast));
			var campaignItemBlastList = new List<CampaignItemBlast> { campaignItemBlast };
			campaignItem.BlastList = campaignItemBlastList;
			ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => campaignItem;
			ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (x, y, z) =>
			{
				var blast = CreateInstance(typeof(BlastRegular));
				var layout = CreateInstance(typeof(Layout));
				blast.Layout = layout;
				var campaignItemTestBlast = CreateInstance(typeof(CampaignItemTestBlast));
				campaignItemTestBlast.Blast = blast;
				campaignItemTestBlast.CampaignItemTestBlastID = campaignItemTestBlastID;
				var campaignItemTestBlastList = new List<CampaignItemTestBlast> { campaignItemTestBlast };
				return campaignItemTestBlastList;
			};

			// Act
			CallMethod(_typeQuickBlastTest, MethodLoadData, _methodArgs, _objectQuickBlastTest);

			// Assert
			var emailFrom = (GetField(_objectQuickBlastTest, "txtEmailFrom") as TextBox).Text;
			var eamilReplyTo = (GetField(_objectQuickBlastTest, "txtReplyTo") as TextBox).Text;
			var campaignEmailFrom = campaignItem?.FromEmail as string;
			var campaignEmailSubject = campaignItem?.ReplyTo as string;
			_objectQuickBlastTest.ShouldSatisfyAllConditions(
				() => emailFrom.ShouldBe(campaignEmailFrom),
				() => eamilReplyTo.ShouldBe(campaignEmailSubject));
		}

		[Test]
		public void GetShortNamesForLayoutTemplate_WhenListContainsValidStrings_ReturnsShortNames()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_layoutNames.Add(_layoutNameValid);
			_groupId = 1;
			_methodArgs = new object[] { _layoutNames, _groupId };

			// Act
			var result = CallMethod(_typeQuickBlastTest, MethodGetShortNamesForLayoutTemplate, _methodArgs, _objectQuickBlastTest) as List<string>;

			// Assert
			result.Contains(DummyString);
		}

		[Test]
		public void GetShortNamesForLayoutTemplate_WhenListContainsInvalidFormatStrings_ShowsErrors()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_layoutNames.Add(_layoutNamesWithFormatErrors);
			_groupId = 1;
			_methodArgs = new object[] { _layoutNames, _groupId };
			var errorShown = false;
			ShimECNError.ConstructorEnumsEntityEnumsMethodString = (a, b, c, d) =>
			{
				errorShown = true;
			};

			// Act
			var result = CallMethod(_typeQuickBlastTest, MethodGetShortNamesForLayoutTemplate, _methodArgs, _objectQuickBlastTest) as List<string>;

			// Assert
			errorShown.ShouldBeTrue();
		}

		[Test]
		public void GetShortNamesForLayoutTemplate_WhenListContainsInvalidStrings_ShowsErrors()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_layoutNames.Add(_layoutNamesWithInvalidCharacters);
			_groupId = 1;
			_methodArgs = new object[] { _layoutNames, _groupId };
			var errorShown = false;
			ShimECNError.ConstructorEnumsEntityEnumsMethodString = (a, b, c, d) =>
			{
				errorShown = true;
			};

			// Act
			var result = CallMethod(_typeQuickBlastTest, MethodGetShortNamesForLayoutTemplate, _methodArgs, _objectQuickBlastTest) as List<string>;

			// Assert
			errorShown.ShouldBeTrue();
		}

		[Test]
		public void GetShortNamesForDynamicStrings_WhenListContainsValidStrings_ReturnsShortNames()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_layoutNames.Add(_layoutNameValid);
			_groupId = 1;
			_user = CreateInstance(typeof(User));
			_methodArgs = new object[] { _layoutNames, _groupId, _user };

			// Act
			var result = CallMethod(_typeQuickBlastTest, MethodGetShortNamesForDynamicStrings, _methodArgs, _objectQuickBlastTest) as List<string>;

			// Assert
			result.Contains(DummyString);
		}

		[Test]
		public void GetShortNamesForDynamicStrings_WhenListContainsInvalidFormatStrings_ShowsErrors()
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_layoutNames.Add(_layoutNamesWithFormatErrors);
			_groupId = 1;
			_user = CreateInstance(typeof(User));
			_methodArgs = new object[] { _layoutNames, _groupId, _user };
			var errorShown = false;
			ShimECNError.ConstructorEnumsEntityEnumsMethodString = (a, b, c, d) =>
			{
				errorShown = true;
			};

			// Act
			var result = CallMethod(_typeQuickBlastTest, MethodGetShortNamesForDynamicStrings, _methodArgs, _objectQuickBlastTest) as List<string>;

			// Assert
			errorShown.ShouldBeTrue();
		}

		[TestCase(true, EmptyString, DummyString, DummyString, false, DummyString, DummyString, DummyString, DummyString)]
		[TestCase(true, DummyString, EmptyString, DummyString, false, DummyString, DummyString, DummyString, DummyString)]
		[TestCase(false, DummyString, DummyString, NegativeString, false, DummyString, DummyString, DummyString, DummyString)]
		[TestCase(false, DummyString, EmptyString, NumberString, false, EmptyString, DummyString, DummyString, DummyString)]
		[TestCase(false, DummyString, DummyString, NumberString, true, EmptyString, DummyString, DummyString, DummyString)]
		[TestCase(false, DummyString, DummyString, NumberString, false, DummyString, DummyString, EmptyString, DummyString)]
		[TestCase(false, DummyString, DummyString, NumberString, false, DummyString, EmptyString, DummyString, DummyString)]
		[TestCase(false, DummyString, DummyString, NumberString, false, DummyString, DummyString, DummyString, EmptyString)]
		public void btnSubmitQTB_onclick_WhenMissingData_DisplaysError(
			bool campaignChecked,
			string campaignName,
			string campaignItemName,
			string selectedCampaign,
			bool groupChecked,
			string selectedGroup,
			string groupName,
			string address,
			string selectedLayoutTrigger)
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_campaignChecked = campaignChecked;
			_campaignName = campaignName;
			_campaignItemName = campaignItemName;
			_selectedCampaign = selectedCampaign;
			_groupChecked = groupChecked;
			_selectedGroup = selectedGroup;
			_groupName = groupName;
			_address = address;
			_selectedLayoutTrigger = selectedLayoutTrigger;
			SetDefaults();
			_methodArgs = new object[] { null, EventArgs.Empty };

			// Act
			CallMethod(_typeQuickBlastTest, MethodSubmitQTBClick, _methodArgs, _objectQuickBlastTest);

			// Assert
			_errorThrown.ShouldBeTrue();
		}

		[TestCase(false, DummyString, DummyString, NumberString, false, DummyString, DummyString, DummyString, DummyString)]
		public void btnSubmitQTB_onclick_Success_QTBIsSubmitted(
			bool campaignChecked,
			string campaignName,
			string campaignItemName,
			string selectedCampaign,
			bool groupChecked,
			string selectedGroup,
			string groupName,
			string address,
			string selectedLayoutTrigger)
		{
			// Arrange
			Initialize();
			CreateShims(MethodLoadData);
			_campaignChecked = campaignChecked;
			_campaignName = campaignName;
			_campaignItemName = campaignItemName;
			_selectedCampaign = selectedCampaign;
			_groupChecked = groupChecked;
			_selectedGroup = selectedGroup;
			_groupName = groupName;
			_address = address;
			_selectedLayoutTrigger = selectedLayoutTrigger;
			SetDefaults();
			_methodArgs = new object[] { null, EventArgs.Empty };
			var qtbSubmitted = false;
			ShimHttpResponse.AllInstances.RedirectString = (x, y) => { };
			ShimQuickTestBlast.CreateQuickTestBlastInt32Int32NullableOfInt32StringStringInt32NullableOfInt32StringNullableOfInt32StringBooleanBooleanBooleanStringStringStringStringUser = (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r) =>
			{
				qtbSubmitted = true;
				return 0;
			};

			// Act
			CallMethod(_typeQuickBlastTest, MethodSubmitQTBClick, _methodArgs, _objectQuickBlastTest);

			// Assert
			qtbSubmitted.ShouldBeTrue();
		}

		private void Initialize()
		{
			_typeQuickBlastTest = typeof(quicktestblast);
			_objectQuickBlastTest = CreateInstance(_typeQuickBlastTest);
			_layoutNames = new List<string>();
			InitializeSession();
		}

		private void CreateShims(string method)
		{
			if (method == MethodLoadData)
			{
				var blast = CreateInstance(typeof(BlastRegular));
				var layout = CreateInstance(typeof(Layout));
				blast.Layout = layout;
				var quickTestBlastConfig = CreateInstance(typeof(QuickTestBlastConfig));
				quickTestBlastConfig.CustomerCanOverride = true;
				var blastEnvelope = CreateInstance(typeof(BlastEnvelope));
				var blastEnvelopeList = new List<BlastEnvelope> { blastEnvelope };
				var campaign = CreateInstance(typeof(Campaign));
				var campaignList = new List<Campaign> { campaign };
				var campaignItem = CreateInstance(typeof(CampaignItem));
				var campaignItemTestBlast = CreateInstance(typeof(CampaignItemTestBlast));
				campaignItemTestBlast.Blast = blast;
				var campaignItemTestBlastList = new List<CampaignItemTestBlast> { campaignItemTestBlast };
				var entitiesGroup = CreateInstance(typeof(EntitiesGroup));
				var columnNamesTable = new DataTable();
				columnNamesTable.Columns.Add("columnName");
				columnNamesTable.Rows.Add("dummyString");
				var groupDataFields = CreateInstance(typeof(GroupDataFields));
				var groupDataFieldsList = new List<GroupDataFields> { groupDataFields };
				ShimQuickTestBlastConfig.GetByCustomerIDInt32 = (x) => quickTestBlastConfig;
				ShimQuickTestBlastConfig.GetByBaseChannelIDInt32 = (x) => quickTestBlastConfig;
				ShimBlastEnvelope.GetByCustomerID_NoAccessCheckInt32 = (x) => blastEnvelopeList;
				ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => campaignItem;
				ShimCampaign.GetByCampaignIDInt32UserBoolean = (x, y, z) => campaign;
				ShimCampaignItemTestBlast.GetByCampaignItemIDInt32UserBoolean = (x, y, z) => campaignItemTestBlastList;
				ShimGroup.GetByGroupID_NoAccessCheckInt32 = (x) => entitiesGroup;
				ShimCampaign.GetByCustomerIDInt32UserBoolean = (x, y, z) => campaignList;
				ShimEmail.GetColumnNames = () => columnNamesTable;
				ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (x) => groupDataFieldsList;
			}
		}

		private void SetDefaults()
		{
			SetField(_objectQuickBlastTest, "ddlCampaigns", new DropDownList
			{
				Items =
				{
					new ListItem
					{
						Selected = true,
						Text = _selectedCampaign
					}
				},
			});
			SetField(_objectQuickBlastTest, "rbGroupChoice1", new RadioButton
			{
				Checked = _groupChecked
			});

			SetField(_objectQuickBlastTest, "rbCampaignChoice1", new RadioButton
			{
				Checked = _campaignChecked
			});
			SetField(_objectQuickBlastTest, "txtCampaignItemName", new TextBox
			{
				Text = _campaignItemName
			});
			SetField(_objectQuickBlastTest, "hfSelectGroupID", new HiddenField
			{
				Value = _selectedGroup
			});
			SetField(_objectQuickBlastTest, "hfSelectedLayoutTrigger", new HiddenField
			{
				Value = _selectedLayoutTrigger
			});
			SetField(_objectQuickBlastTest, "txtCampaignName", new TextBox
			{
				Text = _campaignName
			});
			SetField(_objectQuickBlastTest, "tbGroupName", new TextBox
			{
				Text = _groupName
			});
			SetField(_objectQuickBlastTest, "taAddresses", new TextBox
			{
				Text = _address
			});
			SetField(_objectQuickBlastTest, "drpEmailFromName", new DropDownList
			{
				Items =
				{
					new ListItem
					{
						Selected = true,
						Text = DummyString
					}
				},
			});
			SetField(_objectQuickBlastTest, "drpReplyTo", new DropDownList
			{
				Items =
				{
					new ListItem
					{
						Selected = true,
						Text = DummyString
					}
				},
			});
			SetField(_objectQuickBlastTest, "drpEmailFrom", new DropDownList
			{
				Items =
				{
					new ListItem
					{
						Selected = true,
						Text = DummyString
					}
				},
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
			var communicatorMasterPage = new ecn.communicator.MasterPages.Communicator();
			config.Add("KMCommon_Application", KMCommon_Application);
			queryString.Add("HTTP_HOST", SampleHttpHost);
			dummyCustormer.CustomerID = CustomerID;
			dummyUser.UserID = UserID;
			baseChannel.BaseChannelID = UserID;
			SetField(authTkt, "CustomerID", CustomerID);
			SetField(ecnSession, "CurrentUser", dummyUser);
			SetField(ecnSession, "CurrentCustomer", dummyCustormer);
			SetField(ecnSession, "CurrentBaseChannel", baseChannel);
			HttpContext.Current = MockHelpers.FakeHttpContext();
			ShimECNSession.CurrentSession = () => ecnSession;
			ShimAuthenticationTicket.getTicket = () => authTkt;
			ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
			ShimPage.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
			ShimConfigurationManager.AppSettingsGet = () => config;
			ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
			ShimHttpRequest.AllInstances.QueryStringGet = (h) => queryString;
			ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
			ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);
			ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
			ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
			ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
			ShimPage.AllInstances.MasterGet = page => communicatorMasterPage;
			Shimquicktestblast.AllInstances.DoTwemoji = (x) =>
			{
				_errorThrown = true;
			};
			InitializeAllControls(_objectQuickBlastTest);
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
				var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
																  BindingFlags.NonPublic |
																  BindingFlags.Static |
																  BindingFlags.Instance);
				if (fieldType != null)
				{
					try
					{
						fieldType.SetValue(field, page);
					}
					catch (Exception ex)
					{
						// ignored
						Trace.TraceError($"Unable to set value as :{ex}");
					}
				}
			}
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private dynamic CreateInstanceWithValues(Type type, object[] valuesObject)
		{
			return ReflectionHelper.CreateInstanceWithValues(type, valuesObject);
		}

		private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
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
