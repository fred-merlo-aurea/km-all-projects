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
using ecn.menu;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using KM.Common.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using EntitiesTicket = ECN_Framework_Entities.Application.AuthenticationTicket;
using NUnit.Framework;
using Shouldly;

namespace Ecn.Menu.Tests
{
    /// <summary>
    ///     Unit Tests for <see cref="ecn.menu.MvcApplication"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class MvcApplicationTest
    {
        private const string CommandExecutedSuccessfully = "1";
        private const string CommunicatorVirtualPath = "1";
        private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
        private const string ErrorPageUrl = "1/Error?E=";
        private IDisposable _context;
        private MvcApplication _mvcApplicationObject;
        private Type _mvcApplicationType;
        private object[] _methodArgs;

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
            var redirectRoute = string.Empty;
            Initialize();
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(typeof(ECNException))]
        [TestCase(typeof(ArgumentException))]
        [TestCase(typeof(ViewStateException))]
        [TestCase(typeof(TransactionException))]
        [TestCase(typeof(SqlException))]
        public void Application_Error_OnException_LogsErrorAndRedirectsToErrorPage(Type innerException)
        {
            // Arrange
            var errorLogged = false;
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var innerEx = ReflectionHelper.CreateInstance(innerException);
                ReflectionHelper.SetField(innerEx, "Message", "inner error");
                return new Exception("error", innerEx);
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(typeof(ArgumentException))]
        [TestCase(typeof(ViewStateException))]
        [TestCase(typeof(TransactionException))]
        [TestCase(typeof(SqlException))]
        [TestCase(typeof(HttpException))]
        public void Application_Error_OnHandlingExceptionIfAnotherExceptionIsThrown_ExceptionIsHandled(Type innerException)
        {
            // Arrange
            var errorLogged = innerException == typeof(HttpException) ? true : false;
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var innerEx = ReflectionHelper.CreateInstance(innerException);
                ReflectionHelper.SetField(innerEx, "Message", "inner error");
                return new Exception("error", innerEx);
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };
            ShimHttpRequest.AllInstances.UserAgentGet = (x) => null;

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(typeof(HttpException))]
        [TestCase(typeof(TransactionException))]
        [TestCase(typeof(Exception))]
        public void Application_Error_OnHandlingGivenIfAnotherExceptionIsThrown_ExceptionIsHandled(Type exception)
        {
            // Arrange
            var errorPageAddress = exception == typeof(TransactionException) ? "/error.aspx?" : ErrorPageUrl;
            var errorLogged = false;
            var redirectRoute = string.Empty;
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
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(errorPageAddress));
        }

        [TestCase(typeof(HttpException))]
        [TestCase(typeof(Exception))]
        public void Application_Error_OnHttpExceptionWhenErrorCodeIsNot404_LogsErrorAndRedirectsToErrorPage(Type exception)
        {
            // Arrange
            var errorLogged = false;
            var redirectRoute = string.Empty;
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
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase("does not exist")]
        [TestCase("other")]
        public void Application_Error_OnNonHandledExceptions_RedirectsToErrorPage(string message)
        {
            // Arrange
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpException.AllInstances.GetHttpCode = (x) => { return 4; };
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var ex = new Exception(message);
                return ex;
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) => CommandExecutedSuccessfully;
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(400)]
        [TestCase(404)]
        [TestCase(500)]
        public void Application_Error_OnHttpException_OnlyRedirectsToErrorPage(int httpErrorCode)
        {
            // Arrange
            var errorLogged = false;
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpException.AllInstances.GetHttpCode = (x) => httpErrorCode;
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var ex = new Exception("httpError", new HttpException());
                return ex;
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeFalse(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase("does not exist", false)]
        [TestCase("A potentially dangerous Request", false)]
        [TestCase("ASP.NET session has expired or could not be found", false)]
        [TestCase("otherDummyMessage", true)]
        public void Application_Error_OnOtherException_OnlyRedirectsToErrorPage(string errorMessage, bool errorLogged)
        {
            // Arrange
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpException.AllInstances.GetHttpCode = (x) => 500;
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var ex = new NullReferenceException(errorMessage, new NullReferenceException());
                return ex;
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBe(errorLogged),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [Test]
        public void Application_Error_WhenEcnExceptionHasNull_ExceptionIsHandled()
        {
            // Arrange
            var errorLogged = false;
            var redirectRoute = string.Empty;
            Initialize();
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                return new ECNException(null);
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(typeof(HttpException))]
        public void Application_Error_OnAnyException_LogsErrorAndRedirectsToErrorPage(Type innerException)
        {
            // Arrange
            var errorLogged = innerException == typeof(HttpException) ? true : false;
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var ex = new Exception("httpError", new HttpException());
                return ex;
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            ShimHttpException.AllInstances.GetHttpCode = (x) => 404;

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeTrue(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(ErrorPageUrl));
        }

        [TestCase(Enums.SecurityExceptionType.RoleAccess, "/main/securityAccessError")]
        [TestCase(Enums.SecurityExceptionType.FeatureNotEnabled, "/main/featureAccessError.aspx")]
        public void Application_Error_OnSecurityException_OnlyRedirectsToErrorPage(Enums.SecurityExceptionType securityType, string errorUrl)
        {
            // Arrange
            var errorLogged = false;
            var redirectRoute = string.Empty;
            Initialize();
            HttpContext.Current.Request.ServerVariables.Add("HTTP_HOST", "newHost");
            ShimHttpException.AllInstances.GetHttpCode = (x) => 500;
            ShimHttpServerUtility.AllInstances.GetLastError = (x) =>
            {
                var innerException = new SecurityException();
                innerException.SecurityType = securityType;
                var ex = new Exception("httpError", innerException);
                return ex;
            };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) =>
            {
                errorLogged = true;
                return CommandExecutedSuccessfully;
            };
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (x, route, y) =>
            {
                redirectRoute = route;
            };

            // Act
            ReflectionHelper.CallMethod(_mvcApplicationType, "Application_Error", _methodArgs, _mvcApplicationObject);

            // Assert
            redirectRoute.ShouldSatisfyAllConditions(
                () => errorLogged.ShouldBeFalse(),
                () => redirectRoute.ShouldNotBeNullOrWhiteSpace(),
                () => redirectRoute.ShouldContain(errorUrl));
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
            ConfigurationManager.AppSettings["Communicator_VirtualPath"] = CommunicatorVirtualPath;
            ShimConfigurationManager.ConnectionStringsGet = () =>
            {
                var css = new ConnectionStringSettings("KMCommon", ConnectionString);
                var cssc = new ConnectionStringSettingsCollection();
                cssc.Add(css);
                return cssc;
            };
            ShimSqlConnection.AllInstances.Open = (x) => { };
            ShimSqlCommand.AllInstances.ExecuteScalar = (x) => "1";
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (x) => 1;
            ShimSqlConnection.AllInstances.Close = (x) => { };
            ShimApplicationLog.FormatExceptionException = (x) => { return "formated_exception"; };
            ShimHttpApplication.AllInstances.ApplicationGet = (x) => { return HttpContext.Current.Application; };
            ShimHttpApplication.AllInstances.RequestGet = (x) => { return HttpContext.Current.Request; };
            ShimHttpApplication.AllInstances.ResponseGet = (x) => { return HttpContext.Current.Response; };
            ShimHttpResponse.AllInstances.RedirectToRouteObject = (x, route) =>
            {
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
            ShimEmailFunctions.NotifyAdminApplicationLog = (x) => string.Empty;
            ECNSession es = ECNSession.CurrentSession();
            ShimECNSession.CurrentSession = () =>
            {
                KMPlatform.Entity.User user = ReflectionHelper.CreateInstance(typeof(KMPlatform.Entity.User));
                var ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
                ecnSession.CurrentUser = user;
                return ecnSession;
            };
            ShimAuthenticationTicket.getTicket = () =>
            {
                EntitiesTicket authTkt = ReflectionHelper.CreateInstance(typeof(EntitiesTicket));
                ReflectionHelper.SetField(authTkt, "CustomerID", 1);
                return authTkt;
            };
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
        }

        private void Initialize()
        {
            CreateShims();
            _methodArgs = new object[]
            {
                null,
                EventArgs.Empty
            };
            _methodArgs = null;
            _mvcApplicationType = typeof(MvcApplication);
            _mvcApplicationObject = ReflectionHelper.CreateInstance(_mvcApplicationType);
        }
    }
}