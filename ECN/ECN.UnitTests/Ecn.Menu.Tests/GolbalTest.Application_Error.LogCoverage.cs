using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;



namespace Ecn.Menu.Tests
{
    /// <summary>
    ///     Unit tests for covering Application_Error method of <see cref="ecn.collector.Global"/>
    ///     Focus of this tests is on redirects and log messages
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class MvcApplicationTest_ErrorLogCoverage
    {
        private const string ErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n";
        private const string ShortErrorMessage =
           "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n";
        private const string ECNErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity: FormsSpecificAPI\r\n<BR>Method: Validate\r\n<BR>Message: \r\n";        
        private const string ExpectedSourceMethod = "Global.Application_Error";
        private const int ExpectedApplicationId = 1;
        private const string ApplicationStateErrorEntryKey = "err";

        private const string RedirectDestination = "/Error?E=";
        private const string HardErrror_ErrorType = "HardError";
        private const string ValidationError_ErrorType = "ValidationError";
        private const string TimeoutError_ErrorType = "Timeout";
        private const string InvalidLinkError_ErrorType = "InvalidLink";
        private const string Application_Error_MethodName = "Application_Error";

        private PrivateObject _mvcApplication = new PrivateObject(new ecn.menu.MvcApplication());
        private string _redirectionPath = String.Empty;
        private string _errorMessage = String.Empty;
        private string _sourceMethod;
        private int _statusCode;
        private IDisposable _shimObject;
        private int _applicationID;
        private Dictionary<string, object> _applicationState = new Dictionary<string, object>();

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _mvcApplication = new PrivateObject(new ecn.menu.MvcApplication());
            _redirectionPath = String.Empty;
            _errorMessage = String.Empty;
            ShimHttpApplication.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectStringBoolean = (url, endResponse) => _redirectionPath = url,
                StatusCodeSetInt32 = (statusCode) => _statusCode = statusCode
            };

            ShimHttpApplication.AllInstances.ApplicationGet = (instance) => new ShimHttpApplicationState();
            ShimHttpApplicationState.AllInstances.ItemSetStringObject = (@this, key, val) => { _applicationState[key] = val; };
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new KMPlatform.Entity.Fakes.ShimUser();
                return session;
            };

            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    _applicationID = applicationID;
                    _sourceMethod = sourceMethod;
                };

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    _applicationID = applicationID;
                    _sourceMethod = sourceMethod;
                    return 0;
                };

            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                ServerVariablesGet = () => new NameValueCollection { { "HTTP_HOST", string.Empty } }
            };

            ShimHttpApplication.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                HttpMethodGet = () => "post",
                UrlGet = () => new Uri("http://KMWeb/"),
                InputStreamGet = () => new MemoryStream(Encoding.ASCII.GetBytes("Dummy")),
                RawUrlGet = () => String.Empty,
                UserAgentGet = () => String.Empty,
                UrlReferrerGet = () => new Uri("http://dummy/"),
                HeadersGet = () => new NameValueCollection { { "Dummy", "Dummy" } }
            };

            ShimConfigurationManager.AppSettingsGet =
                () => new NameValueCollection
                {
                    { "marketingautomation_VirtualPath", String.Empty },
                    { "KMCommon_Application", ExpectedApplicationId.ToString() }
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _errorMessage = null;
            _applicationState.Clear();
            _applicationID = default(int);
            _shimObject.Dispose();
        }

        [Test]
        public void ApplicationError_SecurityException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.RoleAccess
            });

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe("/main/securityAccessError.aspx");
        }

        [Test]
        public void ApplicationError_EcnException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + ValidationError_ErrorType);
            _errorMessage.ShouldBe(ECNErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(ECNException));
        }

        [Test]
        public void ApplicationError_EcnException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + ValidationError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(ECNException));
        }

        [Test]
        public void ApplicationError_ArgumentException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new ArgumentException());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + HardErrror_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ArgumentException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new ArgumentException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + HardErrror_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ViewStateException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_TransactionException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe("/error.aspx?E=" + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(TransactionException));
        }

        [Test]
        public void ApplicationError_TransactionException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe("/error.aspx?E=" + TimeoutError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(TransactionException));
        }

        [Test]
        public void ApplicationError_SqlException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }


        [Test]
        public void ApplicationError_TransactionAbortedException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new TransactionAbortedException());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_SqlException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_HttpException_CodeException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => throw new Exception()
            };

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _mvcApplication.ShouldSatisfyAllConditions (
                ()=>_redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType),
                ()=>_errorMessage.ShouldBe(ErrorMessage),
                ()=>_applicationID.ShouldBe(1),
                ()=>_applicationState.ShouldContainKey(ApplicationStateErrorEntryKey));
        }

        [Test]
        public void ApplicationError_HttpException_PageNotFound()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.NotFound
            };

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_InvalidLink()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            };

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ViewStateException()));

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ArgumentException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ArgumentException()));

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_HttpException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new HttpException());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + InvalidLinkError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            _applicationID.ShouldBe(0);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ShortErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_StreamReaderException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            ShimHttpApplication.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                HttpMethodGet = () => "post",
                UrlGet = () => new Uri("http://KMWeb/"),
                InputStreamGet = () => throw new Exception(),
                RawUrlGet = () => String.Empty,
                UserAgentGet = () => String.Empty,
                UrlReferrerGet = () => new Uri("http://dummy/"),
                HeadersGet = () => new NameValueCollection { { "Dummy", "Dummy" } }
            };

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + TimeoutError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination+ TimeoutError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + InvalidLinkError_ErrorType);
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + InvalidLinkError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_DoNotExist()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("does not exist");

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + InvalidLinkError_ErrorType);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_SessionExpired()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            _mvcApplication.Invoke(Application_Error_MethodName);

            // Assert
            _redirectionPath.ShouldBe(RedirectDestination + HardErrror_ErrorType);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        private void AssertThatApplicationIdAndSourceMethodAreAsExpected()
        {
            _applicationID.ShouldBe(ExpectedApplicationId);
            _sourceMethod.ShouldBe(ExpectedSourceMethod);
        }
    }
}
