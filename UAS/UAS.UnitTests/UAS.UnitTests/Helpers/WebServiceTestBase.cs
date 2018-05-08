using System;
using System.Diagnostics.CodeAnalysis;
using Core_AMS.Utilities.Fakes;
using FrameworkUAS.Service;
using Shouldly;
using EntityClient = KMPlatform.Entity.Client;
using EntityUser = KMPlatform.Entity.User;
using ResponseStatus = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes;
using ShimClient = KMPlatform.BusinessLogic.Fakes.ShimClient;
using ShimUser = KMPlatform.BusinessLogic.Fakes.ShimUser;

namespace UAS.UnitTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public abstract class WebServiceTestBase
    {
        private const string SampleString = "sample";
        private const string MessageSuccess = "Success";
        private const string MessageError = "Error";

        protected string ClientCode { get; set; } = "KM";

        protected virtual void ShimForAuthenticate()
        {
            ShimClient.AllInstances.SelectDefaultGuidBoolean = (_, accessKey, __) => new EntityClient
            {
                AccessKey = accessKey,
                ClientCode = ClientCode
            };
            ShimUser.AllInstances.LogInGuidBoolean = (_, accessKey, __) => new EntityUser
            {
                AccessKey = accessKey
            };
        }

        protected virtual void ShimForJsonFunction<T>(string expected = SampleString)
        {
            // Avoid ServiceStack.LicenseException: The free-quota limit on '20 ServiceStack.Text Types'.
            ShimJsonFunctions.AllInstances.ToJsonOf1M0<T>((_, __) => expected);
        }

        protected void VerifySuccessResponse<T>(Response<T> response, T expectedResult)
        {
            VerifyResponse(response, expectedResult, MessageSuccess, ResponseStatus.Success);
        }

        protected void VerifyErrorResponse<T>(Response<T> response, T expectedResult, string message = MessageError)
        {
            VerifyResponse(response, expectedResult, message, ResponseStatus.Error);
        }

        protected void VerifyResponse<T>(Response<T> response, T expectedResult, string message, ResponseStatus status)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (typeof(T).IsValueType)
            {
                response.ShouldSatisfyAllConditions(
                    () => response.ShouldNotBeNull(),
                    () => response.Status.ShouldBe(status),
                    () => response.Message.ShouldBe(message),
                    () => response.Result.ShouldBe(expectedResult)
                );
            }
            else
            {
                response.ShouldSatisfyAllConditions(
                    () => response.ShouldNotBeNull(),
                    () => response.Status.ShouldBe(status),
                    () => response.Message.ShouldBe(message),
                    () => response.Result.ShouldBeSameAs(expectedResult)
                );
            }
        }
    }
}
