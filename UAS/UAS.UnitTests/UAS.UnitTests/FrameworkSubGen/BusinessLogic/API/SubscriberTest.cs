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
using Subscriber = FrameworkSubGen.BusinessLogic.API.Subscriber;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class SubscriberTest
    {
        private const int ValidSubscriberId = 42;
        private const string ServerResponseJson = "{ \"doesnotmatter\" : 4 }";
        private const string ExpectedDeleteUri = " https://api.knowledgemarketing.com/2/subscribers/42";
        private const string SubscriberIdParameterKey = "subscriber_id";
        private const string ErrorMessageSubscriberIdInvalid = "subscriber_id is required";
        private const string ExceptionExceptionMessage = "MyThrownWebException";
        private const string ExpectedClassName = "FrameworkSubGen.BusinessLogic.API.Subscriber";
        private const string ExpectedMethodName = "Subscriber";

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
        public void DeleteSubscriber_SubscriberIdInvalid_NoPostErrorMessageReturned(int subscriberId)
        {
            // Arrange
            var subscriber = new Subscriber();

            // Act
            var result = subscriber.DeleteSubscriber(Client.KM_API_Testing, subscriberId);

            // Assert
            result.ShouldBe(ErrorMessageSubscriberIdInvalid);
        }

        [Test]
        public void DeleteSubscriber_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            string actualAccount = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;
            var subscriber = new Subscriber();

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
            var result = subscriber.DeleteSubscriber(Client.KM_API_Testing, ValidSubscriberId);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(1),
                () => actualCollection[SubscriberIdParameterKey].ShouldBe(ValidSubscriberId.ToString()),
                () => actualAccount.ShouldBe(ExpectedDeleteUri),
                () => actualMethod.ShouldBe(HttpMethod.DELETE.ToString()),
                () => result.ShouldBe(ServerResponseJson));
        }

        [Test]
        public void DeleteSubscriber_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            var subscriber = new Subscriber();
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
            var result = subscriber.DeleteSubscriber(Client.KM_API_Testing, ValidSubscriberId);

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
