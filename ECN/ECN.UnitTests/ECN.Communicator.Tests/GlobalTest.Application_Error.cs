using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.UI;
using System.Transactions;
using System.Data.SqlClient.Fakes;
using System.Web;
using System.Web.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN.Communicator.Tests
{
    public partial class GlobalTest
    {
        private const string TestErrorMessage = "SampleError";
        private const string TestDemoException = "demo exception";

        [Test]
        public void Application_Error_InnerExceptionSecurityError_PageRedirected()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => new Exception("dummy exception", new SecurityException());

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain("/main/securityAccessError.aspx"));
        }

        [Test]
        public void Application_Error_InnerExceptionSecurityErrorAndSecurityTypeFeatureNotEnabled_PageRedirected()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => new Exception(
                TestDemoException, 
                new SecurityException
                {
                    SecurityType = SecurityExceptionType.FeatureNotEnabled
                });

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain("/main/featureAccessError.aspx"));
        }

        [Test]
        public void Application_Error_InnerExceptionECNException_PageRedirected()
        {
            // Arrange
            var exp = new ECNException(new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = TestErrorMessage,
                    Entity = Entity.Blast,
                    Method = Method.Get
                }
            });
            SetUpFakes();
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.ValidationError.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain(TestErrorMessage),
                () => _logDictionary[exp].ShouldContain(Entity.Blast.ToString()),
                () => _logDictionary[exp].ShouldContain(Method.Get.ToString()));
        }

        [Test]
        public void Application_Error_InnerExceptionArgumentException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new ArgumentException());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.HardError.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain(SampleUserAgent));
        }

        [Test]
        public void Application_Error_InnerExceptionViewStateException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new ViewStateException());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.Timeout.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain(SampleUserAgent));
        }

        [Test]
        public void Application_Error_WhenTransactionException_PageRedirected()
        {
            // Arrange
            var exp = new TransactionException();
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.Timeout.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain(SampleUserAgent));
        }

        [Test]
        public void Application_Error_WhenSqlException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new ShimSqlException().Instance);
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.Timeout.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain(SampleUserAgent));
        }

        [Test]
        public void Application_Error_WhenHttpException404_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new HttpException(404, TestDemoException));
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.PageNotFound.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldBeEmpty();
        }

        [Test]
        public void Application_Error_WhenHttpException400_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new HttpException(400, TestDemoException));
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.InvalidLink.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldBeEmpty();
        }

        [Test]
        public void Application_Error_WhenHttpExceptionBaseExceptionViewStateException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new TestHelperException<ViewStateException>());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.Timeout.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp.InnerException),
                () => _logDictionary[exp.InnerException].ShouldBeNullOrWhiteSpace());
        }

        [Test]
        public void Application_Error_WhenHttpExceptionBaseExceptionArgumentException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new TestHelperException<ArgumentException>());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.InvalidLink.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp.InnerException),
                () => _logDictionary[exp.InnerException].ShouldBeNullOrWhiteSpace());
        }

        [Test]
        public void Application_Error_WhenHttpExceptionBaseExceptionHttpException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new TestHelperException<HttpException>());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.InvalidLink.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldBeEmpty();
        }

        [Test]
        public void Application_Error_WhenHttpExceptionBaseExceptionOtherException_PageRedirected()
        {
            // Arrange
            var exp = new Exception(TestDemoException, new TestHelperException<Exception>());
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.HardError.ToString()));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp.InnerException));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp.InnerException),
                () => _logDictionary[exp.InnerException].ShouldContain(SampleHost),
                () => _logDictionary[exp.InnerException].ShouldContain(SampleHostPath));
        }

        [Test]
        public void Application_Error_WhenGeneralException_PageRedirected()
        {
            // Arrange
            var exp = new Exception("does not exist");
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.InvalidLink.ToString()),
                () => RedirectUrl.ShouldContain("/error.aspx?"),
                () => RedirectUrl.ShouldContain(SampleHostPath));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldBeEmpty();
        }

        [Test]
        public void Application_Error_WhenGeneralAspNetException_PageRedirected()
        {
            // Arrange
            var exp = new Exception("ASP.NET session has expired or could not be found");
            
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.HardError.ToString()),
                () => RedirectUrl.ShouldContain(SampleHostPath));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldBeEmpty();
        }

        [Test]
        public void Application_Error_WhenUserCurrentSessionException_PageRedirected()
        {
            // Arrange
            var exp = new Exception();
           
            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);
            
            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.Timeout.ToString()),
                () => RedirectUrl.ShouldContain(SampleHostPath));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain($"CustomerID: {1}"),
                () => _logDictionary[exp].ShouldContain($"TestUser"));
        }

        [Test]
        public void Application_Error_WhenDangerousRequest_PageRedirected()
        {
            // Arrange
            var exp = new Exception("A potentially dangerous Request");

            ShimHttpServerUtility.AllInstances.GetLastError = (u) => exp;
            SetUpFakes();

            // Act
            _privateObject.Invoke(ApplicationErrorMethodName, this, EventArgs.Empty);

            // Assert
            PageRedirect.ShouldBeTrue();
            RedirectUrl.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                () => RedirectUrl.ShouldContain(ErrorMessage.InvalidLink.ToString()),
                () => RedirectUrl.ShouldContain(SampleHostPath));
            ApplicationState.ShouldSatisfyAllConditions(
                () => ApplicationState.ShouldNotBeEmpty(),
                () => ApplicationState.ShouldContainKeyAndValue("err", exp));
            _logDictionary.ShouldSatisfyAllConditions(
                () => _logDictionary.ShouldNotBeEmpty(),
                () => _logDictionary.ShouldContainKey(exp),
                () => _logDictionary[exp].ShouldContain($"CustomerID: {1}"),
                () => _logDictionary[exp].ShouldContain($"TestUser"));
        }

        private void SetUpFakes()
        {
            var config = new NameValueCollection();
            config.Add("KMCommon_Application", "1");
            config.Add("Communicator_VirtualPath", SampleHostPath);
            ShimConfigurationManager.AppSettingsGet = () => config;
            QueryString.Add("HTTP_HOST", SampleHttpHost);

            ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
            ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);

            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, w, e, desc, t, y) => 
            {
                _logDictionary.Add(ex, desc);
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, w, e, desc, t, y) => 
            {
                _logDictionary.Add(ex, desc);
                return 1;
            };

            var shim = new ShimECNSession();
            shim.Instance.CurrentUser = new KMPlatform.Entity.User { CustomerID = 1, UserName = "TestUser" };
            ShimECNSession.CurrentSession = () => shim;
        }
    }
}
