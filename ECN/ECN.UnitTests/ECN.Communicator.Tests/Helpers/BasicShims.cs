using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Web;
using System.Web.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using KM.Common.Fakes;

namespace ECN.Communicator.Tests.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class BasicShims
	{
		private const string appSettingKMCommon_Application = "1";
		private const string appSettingIsDemo = "1";
		private const string images_VirtualPath = "dummyString";
		private const string connectionString = "data source=127.0.0.1;Integrated Security=SSPI;";

		public static void CreateShims()
		{
			CreateAppShims();
			CreateWebShims();
			CreateDBShims();
			CreateEmailShims();
		}
		private static void CreateWebShims()
		{
			HttpContext.Current = MockHelpers.FakeHttpContext();
			ShimHttpApplication.AllInstances.ApplicationGet = (x) => HttpContext.Current.Application;
			ShimHttpApplication.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
			ShimHttpApplication.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) => {
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
			};
			ShimHttpRequest.AllInstances.ServerVariablesGet = (x) =>
			{
				var collection = new NameValueCollection();
				collection.Add("HTTP_HOST", "testHost");
				return collection;
			};
			ShimHttpRequest.AllInstances.UserAgentGet = (x) => "chrome";
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				var sampleEcnErrorObject = new ECNError();
				var sampleEcnErrorList = new List<ECNError>();
				sampleEcnErrorList.Add(sampleEcnErrorObject);
				return new ECNException(sampleEcnErrorList);
			};
			ShimHttpServerUtility.AllInstances.MapPathString = (x, y) => images_VirtualPath;
		}

		private static void CreateDBShims()
		{
			ShimSqlConnection.AllInstances.Open = (x) => {
				ReflectionHelper.SetField(x, "State", ConnectionState.Open);
			};
			ShimSqlConnection.AllInstances.Close = (x) => {
				ReflectionHelper.SetField(x, "State", ConnectionState.Closed);
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) => "1";
			ShimSqlCommand.AllInstances.ExecuteNonQuery = (x) => 1;
			ShimSqlCommand.AllInstances.ExecuteReader = (x) => null;
			ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (x, y) => null;
			ShimSqlConnection.AllInstances.StateGet = (x) => ConnectionState.Open;
		}

		private static void CreateAppShims()
		{
			ShimApplicationLog.FormatExceptionException = (x) => "formated_exception";
			ConfigurationManager.AppSettings["KMCommon_Application"] = appSettingKMCommon_Application;
			ConfigurationManager.AppSettings["IsDemo"] = appSettingIsDemo;
			ConfigurationManager.AppSettings["images_VirtualPath"] = images_VirtualPath; 
			ShimConfigurationManager.ConnectionStringsGet = () =>
			{
				var sampleConfigSettingCollection = new ConnectionStringSettingsCollection();
				var dummyConnectionString = new ConnectionStringSettings("Communicator", connectionString);
				var dummyConnectionString2 = new ConnectionStringSettings("KMCommon", connectionString);
				sampleConfigSettingCollection.Add(dummyConnectionString);
				sampleConfigSettingCollection.Add(dummyConnectionString2);
				return sampleConfigSettingCollection;
			};
		}

		private static void CreateEmailShims()
		{
			ShimEmailFunctions.NotifyAdminApplicationLog = (x) => string.Empty;
		}

		private static void CreateFileSystemShims()
		{
			ShimDirectory.CreateDirectoryString = (x) => null;
		}
	}
}