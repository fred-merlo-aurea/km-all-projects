using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.Entity.Validation;
using System.Data.Entity.Validation.Fakes;
using System.Data.SqlClient.Fakes;
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
using KMPlatform.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMWeb.Tests
{
    public partial class MvcApplicationTest
    {
        private const string ErrorMessage = 
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n";
        private const string HardErrorMessage = 
            "<BR>Page URL: \r\n<BR>SPY Info:&nbsp;[] / []\r\n<BR>Referring URL: http://dummy/\r\n<BR>HEADERS\r\n<BR>Dummy:Dummy\r\n";
        private const string ECNErrorMessage = 
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity: FormsSpecificAPI\r\n<BR>Method: Validate\r\n<BR>Message: \r\n";
        private const string UnknownErrorMessage = 
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Request Body: Dummy\r\n";
        private const string ValidationErrorMessage = 
            "<BR><BR>CustomerID: 0\r\n<BR>UserName: \r\n<BR>Page URL: \r\n<BR>User Agent: \r\n<BR>Entity Validation Error: Type \"Object\" in state \"0\" has the following validation errors:\n- Property: \"Dummy\", Value: \"\", Error: \"Dummy\"\r\n\r\n";
        private const string ApplicationStateErrorEntryKey = "err";
        private const string ExpectedSourceMethod = "Global.Application_Error";
        private const int ExpectedApplicationId = 1;

        private PrivateObject _mvcApplication = new PrivateObject(new MvcApplication());
        private string _redirectionPath = String.Empty;
        private string _errorMessage = String.Empty;
        private int _applicationID;
        private Dictionary<string, object> _applicationState = new Dictionary<string, object>();
        private string _sourceMethod;

        [Test]
        public void ApplicationError_SecurityException_RoleAccess()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.RoleAccess
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/main/securityAccessError.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_SecurityException_FeatureNotEnabled()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty,new ShimSecurityException
            {
                SecurityTypeGet = () => Enums.SecurityExceptionType.FeatureNotEnabled
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/main/featureAccessError.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_EcnException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(ECNErrorMessage);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(ECNException));
        }

        [Test]
        public void ApplicationError_EcnException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ECNException(new List<ECNError> { new ECNError() });
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(ECNException));
        }

        [Test]
        public void ApplicationError_ArgumentException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => 
            new Exception(String.Empty, new ArgumentException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ArgumentException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => 
            new Exception(String.Empty, new ArgumentException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ViewStateException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/Timeout");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_ViewStateException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/Timeout");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_TransactionException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/Timeout");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(TransactionException));
        }

        [Test]
        public void ApplicationError_TransactionException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/Timeout");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(TransactionException));
        }

        [Test]
        public void ApplicationError_SqlException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_SqlException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_HttpException_CodeException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => throw new Exception()
            };

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBeEmpty();
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_PageNotFound()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.NotFound
            };

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/PageNotFound");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_InvalidLink()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            };

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/InvalidLink");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ViewStateException()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/Timeout");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_ArgumentException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception(String.Empty, new ArgumentException()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/InvalidLink");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_HttpException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new HttpException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/InvalidLink");
            _errorMessage.ShouldBeEmpty();
            _applicationID.ShouldBe(0);
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(HardErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(HttpException));
        }

        [Test]
        public void ApplicationError_UnknownException()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(UnknownErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_StreamReaderException()
        {
            // Arrange
            InitilizeExportDataTests();
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
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/InvalidLink");
            _errorMessage.ShouldBe(ErrorMessage);
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request");
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/InvalidLink");
            _errorMessage.ShouldBeEmpty();
            AssertThatApplicationIdAndSourceMethodAreAsExpected();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_DoNotExist()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("does not exist");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
            _applicationState[ApplicationStateErrorEntryKey].ShouldBeOfType(typeof(Exception));
        }

        [Test]
        public void ApplicationError_UnknownException_SessionExpired()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_ValidationFailed()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
                new DbEntityValidationException("Validation failed for one or more entities",
                new List<DbEntityValidationResult>{
                    new ShimDbEntityValidationResult{
                        EntryGet =  () => new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry{ EntityGet = () => new object(),
                            CurrentValuesGet = () => new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues() },
                        ValidationErrorsGet = () => new List<DbValidationError>{ new DbValidationError("Dummy","Dummy")} } });
                
            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBe(ValidationErrorMessage);
        }

        [Test]
        public void ApplicationError_UnknownException_ValidationFailed_Exception()
        {
            // Arrange
            InitilizeExportDataTests();
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
                new DbEntityValidationException("Validation failed for one or more entities");
            ShimECNSession.CurrentSession = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("/Error/HardError");
            _errorMessage.ShouldBeEmpty(); ;
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
            ShimHttpApplicationState.AllInstances.ItemSetStringObject = (instance, key, val) => { _applicationState[key] = val; };

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
                    _applicationID = applicationID;
                    _sourceMethod = sourceMethod;
                };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
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
                ServerVariablesGet = () => new NameValueCollection { { "HTTP_HOST", String.Empty } }
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
                    { "Communicator_VirtualPath", String.Empty },
                    { "KMCommon_Application", "1" }
                };
        }

        private void AssertThatApplicationIdAndSourceMethodAreAsExpected()
        {
            _applicationID.ShouldBe(ExpectedApplicationId);
            _sourceMethod.ShouldBe(ExpectedSourceMethod);
        }
    }
}
