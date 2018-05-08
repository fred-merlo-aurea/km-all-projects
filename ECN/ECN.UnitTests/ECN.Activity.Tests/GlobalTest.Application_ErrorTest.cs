using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient.Fakes;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Activity.Tests
{
    public partial class MvcApplicationTest
    {
        private const string ErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n";
        private const string HardErrorMessage =
            "<BR>Page URL: \r\n<BR>SPY Info:&nbsp;[] / []\r\n<BR>Referring URL: http://dummy/\r\n<BR>HEADERS\r\n<BR>Dummy:Dummy\r\n";
        private const string ECNErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity: FormsSpecificAPI\r\n<BR>Method: Validate\r\n<BR>Message: \r\n";

        private const string ApplicationStateErrorEntryKey = "err";
        private const string ExpectedSourceMethod = "Global.Application_Error";
        private const int ExpectedApplicationId = 1;

        private const string RedirectPathBase = "Error/Error/";
        private const string HardErrorRedirectPath = RedirectPathBase+"HardError";
        private const string ValidationErrorRedirectPath = RedirectPathBase + "ValidationError";
        private const string TimeoutErrorRedirectPath = RedirectPathBase + "Timeout";
        private const string PageNotFoundErrorRedirctPath = RedirectPathBase + "PageNotFound";
        private const string InvalidLinkErrorRedirectPath = RedirectPathBase + "InvalidLink";
        private const string WeirdPageNotFoundRedirectPath = "Error.aspx?E=PageNotFound";
        private int _applicationID;
        private Dictionary<string, object> _applicationState = new Dictionary<string, object>();
        private string _sourceMethod;

        [Test]
        public void ApplicationError_SecurityException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.RoleAccess
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState.ShouldNotContainKey(ApplicationStateErrorEntryKey);
        }

        [Test]
        public void ApplicationError_EcnException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(ValidationErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(ValidationErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_HttpException_CodeException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => throw new Exception()
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _mvcApplication.ShouldSatisfyAllConditions (
                ()=>_redirectionPath.ShouldBeEmpty(),
                ()=>_errorMessage.ShouldBeEmpty(),
                ()=>_applicationID.ShouldBe(0), // no logging for one of the condition so this stays as default
                ()=>_applicationState.ShouldNotContainKey(ApplicationStateErrorEntryKey));
        }

        [Test]
        public void ApplicationError_HttpException_PageNotFound()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.NotFound
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(PageNotFoundErrorRedirctPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_InvalidLink()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception(String.Empty, new ViewStateException())));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ArgumentException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception(String.Empty, new ArgumentException())));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_HttpException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new HttpException()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationID.ShouldBe(0);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
            _errorMessage.ShouldBe(HardErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception()));
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(TimeoutErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
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
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(InvalidLinkErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_SessionExpired()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(HardErrorRedirectPath);
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_Invalid()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("This is an invalid");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe(WeirdPageNotFoundRedirectPath);
            _errorMessage.ShouldBeEmpty();
        }

        private void AssertThatApplicationIdAndSourceMethodAreAsExpected()
        {
            _applicationID.ShouldBe(ExpectedApplicationId);
            _sourceMethod.ShouldBe(ExpectedSourceMethod);
        }
    }
}
