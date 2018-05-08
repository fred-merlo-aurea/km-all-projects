using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using ChangeDataCapture = FrameworkSubGen.BusinessLogic.API.ChangeDataCapture;
using Entity = FrameworkSubGen.Entity;
using Enums = FrameworkSubGen.Entity.Enums;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class ChangeDataCaptureTest
    {
        private const string ExpectedExceptionMessage = "MyThrownWebException";
        private const string ExpectedClassName = "FrameworkSubGen.BusinessLogic.API.ChangeDataCapture";
        private const string ExpectedMethodName = "ChangeDataCapture";
        private const string ChangedSinceKey = "changed_since";
        private const string EntitiesKey = "entities";
        private const string BundlesEntities = "bundles";
        private const string SubscribersEntities = "subscribers";
        private const string GetListDownloadsEntities = "listdownloads";
        private const string GetAddressesEntities = "addresses";
        private const int ChangeDataCaptureAccountId = 10;
        private const string ChangeDataCaptureUriEndpoint = " https://api.knowledgemarketing.com/2/cdc/";
        private const string DummyServerResponse = "{ \"doesnotmatter\" : something }";

        private IDisposable context;

        [SetUp]
        public void SetUp()
        {
            context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void GetSubscribers_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            var entity = new Entity.ChangeDataCapture { account_id = ChangeDataCaptureAccountId };
            var expectedChangedSince = new DateTime(2010, 10, 5);
            string actualAddress = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, entity);

            ShimWebClient.AllInstances.DownloadStringString =
                (webClient, address) =>
                    {
                        actualAddress = address;
                        actualCollection = webClient.QueryString;
                        return DummyServerResponse;
                    };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var result = changeDataCapture.GetSubscribers(
                Enums.Client.KM_API_Testing,
                expectedChangedSince);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(2),
                () => actualCollection[EntitiesKey].ShouldBe(SubscribersEntities),
                () => actualCollection[ChangedSinceKey].ShouldBe(expectedChangedSince.ToString()),
                () => actualAddress.ShouldBe(ChangeDataCaptureUriEndpoint),
                () => result.account_id.ShouldBe(ChangeDataCaptureAccountId));
        }

        [Test]
        public void GetSubscribers_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimAuthentication.AllInstances.GetClientEnumsClient = (authentication, client) => throw new WebException(ExpectedExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
                {
                    actualException = exception;
                    actualMethodName = methodName;
                    actualClassName = className;
                    apiLogCounter++;
                };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var dataCapture = changeDataCapture.GetSubscribers(
                Enums.Client.KM_API_Testing,
                DateTime.Now);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => dataCapture.ShouldNotBeNull(),
                () => dataCapture.account_id.ShouldBe(0));
        }

        [Test]
        public void GetBundles_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            var entity = new Entity.ChangeDataCapture { account_id = ChangeDataCaptureAccountId };
            var expectedChangedSince = new DateTime(2010, 10, 5);
            string actualAddress = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, entity);

            ShimWebClient.AllInstances.DownloadStringString =
                (webClient, address) =>
                    {
                        actualAddress = address;
                        actualCollection = webClient.QueryString;
                        return DummyServerResponse;
                    };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var result = changeDataCapture.GetBundles(
                Enums.Client.KM_API_Testing,
                expectedChangedSince);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(2),
                () => actualCollection[EntitiesKey].ShouldBe(BundlesEntities),
                () => actualCollection[ChangedSinceKey].ShouldBe(expectedChangedSince.ToString()),
                () => actualAddress.ShouldBe(ChangeDataCaptureUriEndpoint),
                () => result.account_id.ShouldBe(ChangeDataCaptureAccountId));
        }

        [Test]
        public void GetBundles_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimAuthentication.AllInstances.GetClientEnumsClient = (authentication, client) => throw new WebException(ExpectedExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
            {
                actualException = exception;
                actualMethodName = methodName;
                actualClassName = className;
                apiLogCounter++;
            };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var dataCapture = changeDataCapture.GetBundles(
                Enums.Client.KM_API_Testing,
                DateTime.Now);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => dataCapture.ShouldNotBeNull(),
                () => dataCapture.account_id.ShouldBe(0));
        }

        [Test]
        public void GetListdownloads_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            var entity = new Entity.ChangeDataCapture { account_id = ChangeDataCaptureAccountId };
            var expectedChangedSince = new DateTime(2010, 10, 5);
            string actualAddress = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, entity);

            ShimWebClient.AllInstances.DownloadStringString =
                (webClient, address) =>
                    {
                        actualAddress = address;
                        actualCollection = webClient.QueryString;
                        return DummyServerResponse;
                    };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var result = changeDataCapture.GetListdownloads(
                Enums.Client.KM_API_Testing,
                expectedChangedSince);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(2),
                () => actualCollection[EntitiesKey].ShouldBe(GetListDownloadsEntities),
                () => actualCollection[ChangedSinceKey].ShouldBe(expectedChangedSince.ToString()),
                () => actualAddress.ShouldBe(ChangeDataCaptureUriEndpoint),
                () => result.account_id.ShouldBe(ChangeDataCaptureAccountId));
        }

        [Test]
        public void GetListdownloads_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimAuthentication.AllInstances.GetClientEnumsClient = (authentication, client) => throw new WebException(ExpectedExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
            {
                actualException = exception;
                actualMethodName = methodName;
                actualClassName = className;
                apiLogCounter++;
            };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var dataCapture = changeDataCapture.GetListdownloads(
                Enums.Client.KM_API_Testing,
                DateTime.Now);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => dataCapture.ShouldNotBeNull(),
                () => dataCapture.account_id.ShouldBe(0));
        }

        [Test]
        public void GetAddresses_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            var entity = new Entity.ChangeDataCapture { account_id = ChangeDataCaptureAccountId };
            var expectedChangedSince = new DateTime(2010, 10, 5);
            string actualAddress = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, entity);

            ShimWebClient.AllInstances.DownloadStringString =
                (webClient, address) =>
                    {
                        actualAddress = address;
                        actualCollection = webClient.QueryString;
                        return DummyServerResponse;
                    };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var result = changeDataCapture.GetAddresses(
                Enums.Client.KM_API_Testing,
                expectedChangedSince);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(2),
                () => actualCollection[EntitiesKey].ShouldBe(GetAddressesEntities),
                () => actualCollection[ChangedSinceKey].ShouldBe(expectedChangedSince.ToString()),
                () => actualAddress.ShouldBe(ChangeDataCaptureUriEndpoint),
                () => result.account_id.ShouldBe(ChangeDataCaptureAccountId));
        }

        [Test]
        public void GetAddresses_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimAuthentication.AllInstances.GetClientEnumsClient = (authentication, client) => throw new WebException(ExpectedExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
            {
                actualException = exception;
                actualMethodName = methodName;
                actualClassName = className;
                apiLogCounter++;
            };

            // Act
            var changeDataCapture = new ChangeDataCapture();
            var dataCapture = changeDataCapture.GetAddresses(
                Enums.Client.KM_API_Testing,
                DateTime.Now);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => dataCapture.ShouldNotBeNull(),
                () => dataCapture.account_id.ShouldBe(0));
        }

        private static void ShimForJsonFunction<T>(string expected, T entity) where T : new()
        {
            ShimJsonFunctions.AllInstances.ToJsonOf1M0<T>((_, __) => expected);

            ShimJsonFunctions.AllInstances.FromJsonOf1String<T>((_, __) => entity);
        }
    }
}
