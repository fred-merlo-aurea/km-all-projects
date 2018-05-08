using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Fakes;
using System.Text;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static FrameworkSubGen.Entity.Enums;
using Subscription = FrameworkSubGen.BusinessLogic.API.Subscription;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class SubscriptionTest
    {
        private const int ValidSubscriptionId = 42;
        private const string ServerResponseJson = "{ \"doesnotmatter\" : 4 }";
        private const string ExpectedDeleteUri = " https://api.knowledgemarketing.com/2/subscriptions/";
        private const string SubscriptionIdParameterKey = "subscription_id";
        private const string ErrorMessageSubscriptionIdInvalid = "subscription_id is required";
        private const string ExceptionExceptionMessage = "MyThrownWebException";
        private const string ExpectedClassName = "FrameworkSubGen.BusinessLogic.API.Subscription";
        private const string ExpectedMethodName = "Subscription";

        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Delete_SubscriptionIdInvalid_NoPostErrorMessageReturned(int subscriptionId)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            var result = subscription.Delete(Client.KM_API_Testing, subscriptionId);

            // Assert
            result.ShouldBe(ErrorMessageSubscriptionIdInvalid);
        }

        [Test]
        public void Delete_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            string actualAccount = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;
            var subscription = new Subscription();

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 account,
                 method,
                 data) =>
                {
                    actualAccount = account;
                    actualMethod = method;
                    actualCollection = data;
                    return Encoding.UTF8.GetBytes(ServerResponseJson);
                };

            // Act
            var result = subscription.Delete(Client.KM_API_Testing, ValidSubscriptionId);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(1),
                () => actualCollection[SubscriptionIdParameterKey].ShouldBe(ValidSubscriptionId.ToString()),
                () => actualAccount.ShouldBe(ExpectedDeleteUri),
                () => actualMethod.ShouldBe(HttpMethod.DELETE.ToString()),
                () => result.ShouldBe(ServerResponseJson));
        }

        [Test]
        public void Delete_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            var subscription = new Subscription();
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimWebClientRaiseException();

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
            {
                actualException = exception;
                actualMethodName = methodName;
                actualClassName = className;
                apiLogCounter++;
            };

            // Act
            var result = subscription.Delete(Client.KM_API_Testing, ValidSubscriptionId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExceptionExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => result.ShouldBe(string.Empty));
        }

        private static void ShimWebClientRaiseException()
        {
            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient, account, method, data) => throw new WebException(ExceptionExceptionMessage);
        }
    }
}
