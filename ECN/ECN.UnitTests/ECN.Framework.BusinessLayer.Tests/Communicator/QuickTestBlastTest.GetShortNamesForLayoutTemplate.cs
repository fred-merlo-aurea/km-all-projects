using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using EntitiesQuickTestBlast = ECN_Framework_BusinessLayer.Communicator.QuickTestBlast;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
	[TestFixture]
	public partial class QuickTestBlastTest
	{
		private const string SampleHost = "km.com";
		private const string SampleHttpHost = "http://km.com";
		private const string SampleHostPath = "http://km.com/addTemplate";
		private const string SampleUserAgent = "http://km.com/addTemplate";
		private const int CampaignItemID = 1;
		private const string KMCommon_Application = "1";
		private const string MethodGetShortNamesForLayoutTemplate = "GetShortNamesForLayoutTemplate";
		private const string MethodGetShortNamesForDynamicStrings = "GetShortNamesForDynamicStrings";
		private const string DummyString = "dummy string";
		private List<string> _layoutNames;
		private string _layoutNameValid = string.Format("%%{0}%%", DummyString);
		private string _layoutNamesWithFormatErrors = string.Format("%%{0}%", DummyString);
		private string _layoutNamesWithInvalidCharacters = string.Format("%%{0}$-#@%%", DummyString);
		private int _groupId;
		private User _user;
		private object[] _methodArgs;
		private Type _typeQuickBlastTest;
		private EntitiesQuickTestBlast _objectQuickBlastTest;

		[Test]
		public void GetShortNamesForDynamicStrings_WhenListContainsValidStrings_ReturnsShortNames()
		{
			// Arrange
			Initialize();
			CreateShims(MethodGetShortNamesForLayoutTemplate);
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
		public void GetShortNamesForDynamicStrings_WhenListContainsInvalidForamtStrings_ShowsErrors()
		{
			// Arrange
			Initialize();
			CreateShims(MethodGetShortNamesForLayoutTemplate);
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

		private void Initialize()
		{
			_typeQuickBlastTest = typeof(EntitiesQuickTestBlast);
			_objectQuickBlastTest = CreateInstance(_typeQuickBlastTest);
			_layoutNames = new List<string>();
			InitializeSession();
		}

		private void CreateShims(string method)
		{
			var columnNamesTable = new DataTable();
			columnNamesTable.Columns.Add("columnName");
			columnNamesTable.Rows.Add("dummyString");
			var groupDataFields = CreateInstance(typeof(GroupDataFields));
			var groupDataFieldsList = new List<GroupDataFields> { groupDataFields };
			ShimEmail.GetColumnNames = () => columnNamesTable;
			ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (x) => groupDataFieldsList;
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
			InitializeAllControls(_objectQuickBlastTest);
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
			return ReflectionHelper.GetFieldValue(obj, fieldName);
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
