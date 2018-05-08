using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Fakes;
using System.Text;
using Core_AMS.Utilities.Fakes;
using FrameworkSubGen.BusinessLogic.API.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static FrameworkSubGen.Entity.Enums;
using Account = FrameworkSubGen.BusinessLogic.API.Account;
using Entity = FrameworkSubGen.Entity;
using Enums = FrameworkSubGen.Entity.Enums;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class AccountTest
    {
        private const string AccountEntityCompany = "AccountEntityCompany";
        private const string AccountEntityEmail = "AccountEntityEmail";
        private const Enums.Plan AccountEntityPlan = Enums.Plan.Standard;
        private const string AccountEntityWebsite = "AccountEntityWebsite";
        private const string CompanyParameterKey = "company_name";
        private const string EmailParameterKey = "email";
        private const string FirstNameParameterKey = "first_name";
        private const string LastNameParameterKey = "last_name";
        private const string PasswordParameterKey = "password";
        private const string PlanParameterKey = "plan";
        private const string WebsiteParameterKey = "website";
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Password = "Password";
        
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
        public void CreateAccount_AccountNull_Return0()
        {
            // Arrange
            var accountBusinessObject = new Account();
            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
                {
                };

            // Act
            var returnValue = accountBusinessObject.Create(
                Enums.Client.KM_API_Testing,
                null);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAccount_CorrectParameterNoData_PostToServerZeroResult()
        {
            // Arrange
            var accountEntity = CreateAccountEntity();
            string actualAccount = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, 0);

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 account,
                 method,
                 data) =>
                {
                    actualAccount = account;
                    actualMethod = method;
                    actualCollection = data;
                    return null;
                };

            // Act
            var accountBusinessObject = new Account();
            var field = accountBusinessObject.Create(
                Enums.Client.KM_API_Testing,
                accountEntity,
                FirstName,
                LastName,
                Password);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(7),
                () => actualCollection[CompanyParameterKey].ShouldBe(accountEntity.company_name),
                () => actualCollection[EmailParameterKey].ShouldBe(accountEntity.email),
                () => actualCollection[PlanParameterKey].ShouldBe(accountEntity.plan.ToString()),
                () => actualCollection[WebsiteParameterKey].ShouldBe(accountEntity.website),
                () => actualCollection[FirstNameParameterKey].ShouldBe(FirstName),
                () => actualCollection[LastNameParameterKey].ShouldBe(LastName),
                () => actualCollection[PasswordParameterKey].ShouldBe(Password),
                () => actualAccount.ShouldBe($" https://api.knowledgemarketing.com/2/accounts/"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(0));
        }

        [Test]
        public void CreateAccount_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            const string serverResponse = "{ \"doesnotmatter\" : 4 }";

            var accountEntity = CreateAccountEntity();
            string actualAccount = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, 4);

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 account,
                 method,
                 data) =>
                {
                    actualAccount = account;
                    actualMethod = method;
                    actualCollection = data;
                    return Encoding.UTF8.GetBytes(serverResponse);
                };

            // Act
            var accountBusinessObject = new Account();
            var field = accountBusinessObject.Create(
                Enums.Client.KM_API_Testing,
                accountEntity,
                FirstName,
                LastName,
                Password);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(7),
                () => actualCollection[CompanyParameterKey].ShouldBe(accountEntity.company_name),
                () => actualCollection[EmailParameterKey].ShouldBe(accountEntity.email),
                () => actualCollection[PlanParameterKey].ShouldBe(accountEntity.plan.ToString()),
                () => actualCollection[WebsiteParameterKey].ShouldBe(accountEntity.website),
                () => actualCollection[FirstNameParameterKey].ShouldBe(FirstName),
                () => actualCollection[LastNameParameterKey].ShouldBe(LastName),
                () => actualCollection[PasswordParameterKey].ShouldBe(Password),
                () => actualAccount.ShouldBe($" https://api.knowledgemarketing.com/2/accounts/"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(4));
        }

        [Test]
        public void CreateAccount_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            const string expectedExceptionMessage = "MyThrownWebException";
            const string expectedClassName = "FrameworkSubGen.BusinessLogic.API.Account";
            const string expectedMethodName = "Create";
            var accountEntity = CreateAccountEntity();
            string actualClassName = null;
            string actualMethodName = null;
            Exception actualException = null;
            var apiLogCounter = 0;

            ShimAuthentication.AllInstances.GetClientEnumsClient = (authentication, client) => throw new WebException(expectedExceptionMessage);

            ShimAuthentication.SaveApiLogExceptionStringString = (exception, className, methodName) =>
            {
                actualException = exception;
                actualMethodName = methodName;
                actualClassName = className;
                apiLogCounter++;
            };

            // Act
            var accountBusinessObject = new Account();
            var actualField = accountBusinessObject.Create(
                Enums.Client.KM_API_Testing,
                accountEntity,
                FirstName,
                LastName,
                Password);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(expectedExceptionMessage),
                () => actualMethodName.ShouldBe(expectedMethodName),
                () => actualClassName.ShouldBe(expectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => actualField.ShouldBe(0));
        }

        private static Entity.Account CreateAccountEntity()
        {
            var account = new Entity.Account
            {
                company_name = AccountEntityCompany,
                email = AccountEntityEmail,
                plan = AccountEntityPlan,
                website = AccountEntityWebsite
            };

            return account;
        }

        private static void ShimForJsonFunction<T>(string expected, T entity) where T : new()
        {
            ShimJsonFunctions.AllInstances.ToJsonOf1M0<T>((_, __) => expected);

            ShimJsonFunctions.AllInstances.FromJsonOf1String<T>((_, __) => entity);
        }
    }
}
