using System;
using System.Collections.Generic;
using System.Configuration.Fakes;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Data.SqlClient.Fakes;
using System.Transactions;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using ecn.gateway;
using ECN.Tests.Helpers;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Gateway.Tests
{
	/// <summary>
	///     Unit Tests for <see cref="ecn.gateway.MvcApplication"/>
	/// </summary>
	[ExcludeFromCodeCoverage]
	[TestFixture]
	public class MvcApplicationTest
	{
		private IDisposable _context;
		private MvcApplication _mvcApplication;
		private Type _mvcApplicationType;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
			HttpContext.Current = MockHelpers.FakeHttpContext();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}
		
		[Test]
		public void Application_Error_OnEcnException_LogsErrorAndRedirectsToErrorPage()
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) => 
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) => 
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}

		[TestCase(typeof(ECNException))]
		[TestCase(typeof(ArgumentException))]
		[TestCase(typeof(ViewStateException))]
		[TestCase(typeof(TransactionException))]
		[TestCase(typeof(SqlException))]
		[TestCase(typeof(HttpException))]
		public void Application_Error_OnException_LogsErrorAndRedirectsToErrorPage(Type innerException)
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			var redirectAction = "";
			var redirectController = "";
			var redirectMessage = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				Exception innerEx = CreateInstance(innerException);
				SetField(innerEx, "Message", "inner error");
				return new Exception("error", innerEx);
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				redirectController = (string)ReflectionHelper.GetPropertyValue(routeObj, "controller");
				redirectAction = (string)ReflectionHelper.GetPropertyValue(routeObj, "action");
				redirectMessage = (string)ReflectionHelper.GetPropertyValue(routeObj, "message");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectMessage.ShouldNotBeNullOrWhiteSpace();
			redirectController.ShouldContain("Error");
			redirectAction.ShouldContain("Error");
		}

		[TestCase(typeof(ArgumentException))]
		[TestCase(typeof(ViewStateException))]
		[TestCase(typeof(TransactionException))]
		[TestCase(typeof(SqlException))]
		[TestCase(typeof(HttpException))]
		public void Application_Error_OnHandlingExceptionIfAnotherExceptionIsThrown_ExceptionIsHandled(Type innerException)
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				Exception innerEx = CreateInstance(innerException);
				SetField(innerEx, "Message", "inner error");
				return new Exception("error", innerEx);
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};
			ShimHttpRequest.AllInstances.UserAgentGet = (x) =>
			{
				return null;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}

		[TestCase(typeof(HttpException))]
		[TestCase(typeof(TransactionException))]
		[TestCase(typeof(Exception))]
		public void Application_Error_OnHandlingGivenIfAnotherExceptionIsThrown_ExceptionIsHandled(Type exception)
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				if (exception == typeof(HttpException))
				{
					var ex = new HttpException("httpError", new Exception());
					return ex;
				}
				if (exception == typeof(TransactionException))
				{
					var ex = new TransactionException("transaction_failed");
					return ex;
				}
				return new Exception();
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}

		[TestCase(typeof(HttpException))]
		[TestCase(typeof(TransactionException))]
		[TestCase(typeof(Exception))]
		public void Application_Error_OnHttpExceptionWhenErrorCodeIsNot404_LogsErrorAndRedirectsToErrorPage(Type exception)
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				if (exception == typeof(HttpException))
				{
					var ex = new HttpException("httpError", new Exception());
					return ex;
				}
				if (exception == typeof(TransactionException))
				{
					var ex = new TransactionException("transaction_failed") ;
					return ex;
				}
				return new Exception();
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}
		
		[TestCase("does not exist")]
		[TestCase("other")]
		public void Application_Error_OnNonHandledExceptions_RedirectsToErrorPage(string message)
		{
			// Arrange
			var redirectRoute = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpException.AllInstances.GetHttpCode = (x) => { return 4; };
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				var ex = new Exception(message);
				return ex;
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}

		[TestCase(typeof(HttpException))]
		public void Application_Error_OnHttpExceptionWhenErrorCodeIs404_OnlyRedirectsToErrorPage(Type exception)
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
			ShimHttpException.AllInstances.GetHttpCode = (x) => { return 404; };
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				HttpException ex = new HttpException("httpError", new Exception());
				return ex;
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeFalse();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}

		[Test]
		public void Application_Error_WhenEcnExceptionHasNull_ExceptionIsHandled()
		{
			// Arrange
			var errorLogged = false;
			var redirectRoute = "";
			Initialize();
			ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
			{
				return new ECNException(null);
			};
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
			{
				errorLogged = true;
				return "1";
			};
			ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
			{
				object routeObj = route;
				var routeStr = "";
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "controller");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "action");
				routeStr += ReflectionHelper.GetPropertyValue(routeObj, "message");
				redirectRoute = routeStr;
			};

			// Act
			CallMethod(_mvcApplicationType, "Application_Error", null, _mvcApplication);

			// Assert
			errorLogged.ShouldBeTrue();
			redirectRoute.ShouldNotBeNullOrWhiteSpace();
			redirectRoute.ShouldContain("Error");
		}


		private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void CreateShims()
		{
			ShimHttpServerUtility.AllInstances.GetLastError = (x) => 
			{
				var ecnErr = new ECNError();
				var ecnErrList = new List<ECNError>();
				ecnErrList.Add(ecnErr);
				return new ECNException(ecnErrList);
			};
			ConfigurationManager.AppSettings["KMCommon_Application"] = "1";
			ShimConfigurationManager.ConnectionStringsGet = () =>
			{
				var css = new ConnectionStringSettings("KMCommon", "data source=127.0.0.1;Integrated Security=SSPI;");
				var cssc = new ConnectionStringSettingsCollection();
				cssc.Add(css);
				return cssc;
			};
			ShimSqlConnection.AllInstances.Open = (x) => { };
			ShimSqlCommand.AllInstances.ExecuteScalar = (x) => 
			{
				return "1";
			};
			ShimSqlCommand.AllInstances.ExecuteNonQuery = (x) =>
			{
				return 1;
			};
			ShimSqlConnection.AllInstances.Close = (x) => { };
			ShimApplicationLog.FormatExceptionException = (x) => { return "formated_exception"; };
			ShimHttpApplication.AllInstances.ApplicationGet = (x) => { return HttpContext.Current.Application; };
			ShimHttpApplication.AllInstances.RequestGet = (x) => { return HttpContext.Current.Request; };
			ShimHttpApplication.AllInstances.ResponseGet = (x) => { return HttpContext.Current.Response; };
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
			ShimHttpRequest.AllInstances.UserAgentGet = (x) =>
			{
				return "chrome";
			};
			KM.Common.Fakes.ShimEmailFunctions.NotifyAdminApplicationLog = (x) => { return ""; }; 
		}

		private void Initialize()
		{
			CreateShims();
			_mvcApplicationType = typeof(MvcApplication);
			_mvcApplication = CreateInstance(_mvcApplicationType);
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}

		private void SetSessionVariable(string name, object value)
		{
			HttpContext.Current.Session.Add(name, value);
		}
	}
}