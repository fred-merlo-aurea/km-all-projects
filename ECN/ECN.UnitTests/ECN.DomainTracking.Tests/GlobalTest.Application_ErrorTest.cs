using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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

namespace ECN.DomainTracking.Tests
{
    public partial class GlobalTest
    {
        private const string ApplicationErrorMethodName = "Application_Error";

        private const string ErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n";
        private const string HardErrorMessage =
            "<BR>Page URL: \r\n<BR>SPY Info:&nbsp;[] / []\r\n<BR>Referring URL: http://dummy/\r\n<BR>HEADERS\r\n<BR>Dummy:Dummy\r\n";
        private const string ECNErrorMessage =
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity: FormsSpecificAPI\r\n<BR>Method: Validate\r\n<BR>Message: \r\n";

        [Test]
        public void ApplicationError_SecurityException_SecurityAccessError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.RoleAccess
            });

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/main/securityAccessError.aspx",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_EcnException_ValidationError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new ECNException(new List<ECNError> { new ECNError() });

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=ValidationError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ECNErrorMessage);
        }

        [Test]
        public void ApplicationError_EcnExceptionInnerException_ValidationError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=ValidationError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_ArgumentException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new Exception(String.Empty, new ArgumentException());

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_ArgumentExceptionAndCurrentSessionException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new Exception(String.Empty, new ArgumentException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_ViewStateException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new Exception(String.Empty, new ViewStateException());

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_ViewStateExceptionAndCurrentSessionException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new Exception(String.Empty, new ViewStateException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_TransactionException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_TransactionExceptionAndCurrentSessionException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError =
                (instance) => new TransactionException();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_SqlException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError =
                (instance) => new Exception(String.Empty, new ShimSqlException());

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_SqlExceptionAndCurrentSessionException_Timeout()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError =
                (instance) => new Exception(String.Empty, new ShimSqlException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
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
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBeEmpty();
            _errorMessage.ShouldBeEmpty();
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
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=PageNotFound",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
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
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=InvalidLink",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ViewStateException()));

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=Timeout",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpExceptionAndInnerArgumentException_InvalidLink()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ArgumentException()));

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=InvalidLink",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpExceptionAndInnerHttpException_InvalidLink()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new HttpException());

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=InvalidLink",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpExceptionWithInnerUnknownException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(HardErrorMessage);
        }

        [Test]
        public void ApplicationError_HttpExceptionWithInnerUnknownExceptionAndHttpContextCurrentException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_UnknownExceptionAndStreamReaderException_HardError()
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
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_UnknownExceptionAndCurrentSessionException_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownExceptionWithDoNotExistMessage_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("does not exist");

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownExceptionWithSessionExpiredMessage_HardError()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = 
                (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            CallApplicationErrorMethod();

            // Assert
            _redirectionPath.ShouldBe(
                string.Format(
                    "{0}/error.aspx?E=HardError",
                    ConfigurationManager.AppSettings[AccountsVirtualPathKey]));
            _errorMessage.ShouldBeEmpty();
        }

        private void CallApplicationErrorMethod()
        {
            var parameters = new object[] { null, null };
            _mvcApplication.Invoke(ApplicationErrorMethodName, parameters);
        }
    }
}
