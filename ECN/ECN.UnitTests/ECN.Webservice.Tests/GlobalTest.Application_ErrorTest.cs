using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace ECN.Webservice.Tests
{
    public partial class GlobalTest
    {
        [Test]
        public void ApplicationError_ArgumentException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(String.Empty, new ArgumentException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ViewStateException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_TransactionException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new TransactionException();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_Win32Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(string.Empty, new Win32Exception());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_SqlException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(String.Empty, new ShimSqlException());

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_CodeException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(string.Empty, new ShimHttpException
            {
                GetHttpCode = () => throw new Exception()
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_PageNotFound()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(string.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.NotFound
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_BadRequest()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            };

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_InvalidLink()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception(string.Empty, new ShimHttpException
            {
                GetHttpCode = () => (int)HttpStatusCode.BadRequest
            });

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_ViewStateException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(string.Empty, new HttpException(String.Empty, new Exception(String.Empty, new ViewStateException())));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_ArgumentException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(string.Empty, new HttpException(String.Empty, new Exception(String.Empty, new ArgumentException())));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_HttpException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(string.Empty, new HttpException(String.Empty, new HttpException()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new Exception(string.Empty, new HttpException(String.Empty, new Exception()));

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_HttpException_UnknownException_Exception()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) =>
            new HttpException(String.Empty, new Exception());
            ShimHttpContext.CurrentGet = () => throw new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception();

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_PotentiallyDangerous()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("A potentially dangerous Request.Path");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_DoNotExist()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("does not exist");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_SessionExpired()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("ASP.NET session has expired or could not be found");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }

        [Test]
        public void ApplicationError_UnknownException_RequestFormat()
        {
            // Arrange
            ShimHttpServerUtility.AllInstances.GetLastError = (instance) => new Exception("Request format is unrecognized");

            // Act
            _mvcApplication.Invoke("Application_Error", new object[] { null, null });

            // Assert
            _redirectionPath.ShouldBe("error.aspx");
            _errorMessage.ShouldBeEmpty();
        }
    }
}
