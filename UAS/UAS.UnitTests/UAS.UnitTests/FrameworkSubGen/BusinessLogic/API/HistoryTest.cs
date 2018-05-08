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
using History = FrameworkSubGen.BusinessLogic.API.History;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class HistoryTest
    {
        private const int ValidHistoryId = 42;
        private const string ServerResponseJson = "{ \"doesnotmatter\" : 4 }";
        private const string ExpectedDeleteUri = " https://api.knowledgemarketing.com/2/history/";
        private const string HistoryIdParameterKey = "history_id";
        private const string ErrorMessageHistoryIdInvalid = "history_id is required";
        private const string ExceptionExceptionMessage = "MyThrownWebException";
        private const string ExpectedClassName = "FrameworkSubGen.BusinessLogic.API.History";
        private const string ExpectedMethodName = "History";

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
        public void Delete_HistoryIdInvalid_NoPostErrorMessageReturned(int historyId)
        {
            // Arrange
            var history = new History();

            // Act
            var result = history.Delete(Client.KM_API_Testing, historyId);

            // Assert
            result.ShouldBe(ErrorMessageHistoryIdInvalid);
        }

        [Test]
        public void Delete_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            string actualAccount = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;
            var history = new History();

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
            var result = history.Delete(Client.KM_API_Testing, ValidHistoryId);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(1),
                () => actualCollection[HistoryIdParameterKey].ShouldBe(ValidHistoryId.ToString()),
                () => actualAccount.ShouldBe(ExpectedDeleteUri),
                () => actualMethod.ShouldBe(HttpMethod.DELETE.ToString()),
                () => result.ShouldBe(ServerResponseJson));
        }

        [Test]
        public void Delete_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            var history = new History();
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
            var result = history.Delete(Client.KM_API_Testing, ValidHistoryId);

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
