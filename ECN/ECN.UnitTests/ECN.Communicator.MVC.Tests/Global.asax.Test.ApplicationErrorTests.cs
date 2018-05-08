using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using ecn.communicator.mvc;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using KMPlatform.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    public partial class MvcApplicationTest
    {
        private const string ErrorMessage = "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n";
        private const string HardErrorMessage = "<BR>Page URL: \r\n<BR>SPY Info:&nbsp;[] / []\r\n<BR>Referring URL: http://dummy/\r\n<BR>HEADERS\r\n<BR>Dummy:Dummy\r\n";
        private const string ECNErrorMessage = "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity: FormsSpecificAPI\r\n<BR>Method: Validate\r\n<BR>Message: \r\n";
        private PrivateObject _mvcApplication = new PrivateObject(new MvcApplication());
        private string _redirectionPath = String.Empty;
        private string _errorMessage = String.Empty;

        [Test]
        public void ApplicationError_SecurityException_RoleAccess()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.RoleAccess
            };

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error/FeatureAccess");
        }

        [Test]
        public void ApplicationError_SecurityException_FeatureNotEnabled()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.FeatureNotEnabled
            };

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error/FeatureAccess");
        }

        [Test]
        public void ApplicationError_SecurityException_Security()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.Security
            };

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error/SecurityAccess");
        }

        [Test]
        public void ApplicationError_EcnException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=ValidationError");
            _errorMessage.ShouldBe(ECNErrorMessage);
        }

        [Test]
        public void ApplicationError_EcnException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=ValidationError");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_ArgumentException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ArgumentException();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=HardError");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_ArgumentException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ArgumentException();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=HardError");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_ViewStateException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Timeout");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_ViewStateException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Timeout");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_SqlException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Timeout");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_SqlException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Timeout");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_TransactionException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/error.aspx?E=Timeout");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_TransactionException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/error.aspx?E=Timeout");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_CodeException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => throw new Exception()
            });

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBeEmpty();
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_PageNotFound()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.NotFound
            });

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=PageNotFound");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_InvalidLink()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            });

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => 
            new Exception(String.Empty, new HttpException(String.Empty, new ViewStateException()));            

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Timeout");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_ArgumentException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new ArgumentException()));

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_HttpException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new HttpException()));

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception()));

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=HardError");
            _errorMessage.ShouldBe(HardErrorMessage);
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new HttpException(String.Empty, new Exception()));
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=HardError");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Unknown");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_UnknownException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=Unknown");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBe(ErrorMessage);
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_DoNotExist()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("does not exist");

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=InvalidLink");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_SessionExpired()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=HardError");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_NotOnController()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("was not found on controller");

            // Act
            _mvcApplication.Invoke("Application_Error", null);

            // Assert
            _redirectionPath.ShouldBe("/Error?E=PageNotFound");
            _errorMessage.ShouldBeEmpty();
        }

        private void InitilizeExportDataTests()
        {
            _mvcApplication = new PrivateObject(new MvcApplication());
            _redirectionPath = String.Empty;
            _errorMessage = String.Empty;
            ShimHttpApplication.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectStringBoolean = (url, endResponse) => _redirectionPath = url
            };

            ShimHttpApplication.AllInstances.ApplicationGet = (instance) => new ShimHttpApplicationState();
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new ShimUser();
                return session;
            };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) => 
                {
                    _errorMessage = note;
                };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    return 0;
                };

            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                ServerVariablesGet = () => new NameValueCollection { { "HTTP_HOST", String.Empty } }
            };

            ShimHttpApplication.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                RawUrlGet = () => String.Empty,
                UserAgentGet = () => String.Empty,
                UrlReferrerGet = () => new Uri("http://dummy/"),
                HeadersGet = () => new NameValueCollection { { "Dummy", "Dummy"} }
            };

            ShimConfigurationManager.AppSettingsGet =
                () => new NameValueCollection
                {
                    { "Communicator_VirtualPath", String.Empty },
                    { "KMCommon_Application", "1" }
                };
        }
    }
}
