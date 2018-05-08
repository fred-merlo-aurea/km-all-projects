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
using CustomField = FrameworkSubGen.BusinessLogic.API.CustomField;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class CustomFieldTest
    {
        private const string ExpectedExceptionMessage = "MyThrownWebException";
        private const string ExpectedClassName = "FrameworkSubGen.BusinessLogic.API.CustomField";
        private const string ExpectedMethodName = "CustomField";
        private const string UpdateFieldOptionErrorString = "supply values for all required fields";
        private const string DisplayAsParameter = "display_as";
        private const string DisplayAsKey = "display_as";
        private const string ValueKey = "value";
        private const string OrderKey = "order";
        private const string DisqualifierKey = "disqualifier";
        private const string ActiveParameterKey = "active";
        private const string ValueParameter = "value";
        private const int FieldId = 24;
        private const int OrderParameter = 42;
        private const string NotEmptyValue = "not empty";
        private const string FieldOptionIdKey = "field_option_id";
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
        public void CreateFieldOption_InvalidFieldId_Return0()
        {
            // Arrange
            var invalidFieldId = -1;
            var customField = new CustomField();

            // Act
            var returnValue = customField.CreateFieldOption(
                Client.KM_API_Testing,
                invalidFieldId,
                true,
                NotEmptyValue,
                false,
                0,
                NotEmptyValue);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateFieldOption_InvalidDisplay_Return0()
        {
            // Arrange
            var invalidDisplay = string.Empty;
            var customField = new CustomField();

            // Act
            var returnValue = customField.CreateFieldOption(
                Client.KM_API_Testing,
                10,
                true,
                invalidDisplay,
                false,
                0,
                NotEmptyValue);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateFieldOption_InvalidValue_Return0()
        {
            // Arrange
            var invalidValue = string.Empty;
            var customField = new CustomField();

            // Act
            var returnValue = customField.CreateFieldOption(
                Client.KM_API_Testing,
                10,
                true,
                NotEmptyValue,
                false,
                0,
                invalidValue);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void CreateFieldOption_CorrectParameterNoData_PostToServerZeroResult(bool active, bool disqualifier)
        {
            // Arrange
            string actualAddress = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, 0);

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 address,
                 method,
                 data) =>
                {
                    actualAddress = address;
                    actualMethod = method;
                    actualCollection = data;
                    return null;
                };

            // Act
            var customField = new CustomField();
            var field = customField.CreateFieldOption(
                Client.KM_API_Testing,
                FieldId,
                active,
                DisplayAsParameter,
                disqualifier,
                OrderParameter,
                ValueParameter);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(5),
                () => actualCollection[ActiveParameterKey].ShouldBe(active.ToString()),
                () => actualCollection[DisplayAsKey].ShouldBe(DisplayAsParameter),
                () => actualCollection[DisqualifierKey].ShouldBe(disqualifier.ToString()),
                () => actualCollection[OrderKey].ShouldBe(OrderParameter.ToString()),
                () => actualCollection[ValueKey].ShouldBe(ValueParameter),
                () => actualAddress.ShouldBe(" https://api.knowledgemarketing.com/2/customfields/24/options"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(0));
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void CreateFieldOption_CorrectParameterData_PostToServerValidResult(bool active, bool disqualifier)
        {
            // Arrange
            const string serverResponse = "{ \"doesnotmatter\" : 4 }";
            string actualAddress = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;

            ShimForJsonFunction(string.Empty, 4);

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 address,
                 method,
                 data) =>
                {
                    actualAddress = address;
                    actualMethod = method;
                    actualCollection = data;
                    return Encoding.UTF8.GetBytes(serverResponse);
                };

            // Act
            var customField = new CustomField();
            var field = customField.CreateFieldOption(
                Client.KM_API_Testing,
                FieldId,
                active,
                DisplayAsParameter,
                disqualifier,
                OrderParameter,
                ValueParameter);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(5),
                () => actualCollection[ActiveParameterKey].ShouldBe(active.ToString()),
                () => actualCollection[DisplayAsKey].ShouldBe(DisplayAsParameter),
                () => actualCollection[DisqualifierKey].ShouldBe(disqualifier.ToString()),
                () => actualCollection[OrderKey].ShouldBe(OrderParameter.ToString()),
                () => actualCollection[ValueKey].ShouldBe(ValueParameter),
                () => actualAddress.ShouldBe(" https://api.knowledgemarketing.com/2/customfields/24/options"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(4));
        }

        [Test]
        public void CreateFieldOption_ExceptionRaised_ApiLogWritten()
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
            var customField = new CustomField();
            var actualField = customField.CreateFieldOption(
                Client.KM_API_Testing,
                FieldId,
                true,
                DisplayAsParameter,
                true,
                OrderParameter,
                ValueParameter);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => actualField.ShouldBe(0));
        }

        [Test]
        public void UpdateFieldOption_InvalidFieldId_ReturnErrorString()
        {
            // Arrange
            var invalidFieldId = -1;
            var customField = new CustomField();

            // Act
            var returnValue = customField.UpdateFieldOption(
                Client.KM_API_Testing,
                invalidFieldId,
                true,
                NotEmptyValue,
                false,
                0,
                NotEmptyValue);

            // Assert
            returnValue.ShouldBe(UpdateFieldOptionErrorString);
        }

        [Test]
        public void UpdateFieldOption_InvalidDisplay_ReturnErrorString()
        {
            // Arrange
            var invalidDisplay = string.Empty;
            var customField = new CustomField();

            // Act
            var returnValue = customField.UpdateFieldOption(
                Client.KM_API_Testing,
                10,
                true,
                invalidDisplay,
                false,
                0,
                NotEmptyValue);

            // Assert
            returnValue.ShouldBe(UpdateFieldOptionErrorString);
        }

        [Test]
        public void UpdateFieldOption_InvalidValue_ReturnErrorString()
        {
            // Arrange
            var invalidValue = string.Empty;
            var customField = new CustomField();

            // Act
            var returnValue = customField.UpdateFieldOption(
                Client.KM_API_Testing,
                10,
                true,
                NotEmptyValue,
                false,
                0,
                invalidValue);

            // Assert
            returnValue.ShouldBe(UpdateFieldOptionErrorString);
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void UpdateFieldOption_CorrectParameterData_PostToServerValidResult(bool active, bool disqualifier)
        {
            // Arrange
            const int fieldId = 666;
            const string serverResponse = "{ \"doesnotmatter\" : something }";

            string actualAddress = null;
            string actualMethod = null;
            NameValueCollection actualCollection = null;

            ShimWebClient.AllInstances.UploadValuesStringStringNameValueCollection =
                (webClient,
                 address,
                 method,
                 data) =>
                {
                    actualAddress = address;
                    actualMethod = method;
                    actualCollection = data;
                    return Encoding.UTF8.GetBytes(serverResponse);
                };

            // Act
            var customField = new CustomField();
            var result = customField.UpdateFieldOption(
                Client.KM_API_Testing,
                fieldId,
                active,
                DisplayAsParameter,
                disqualifier,
                OrderParameter,
                ValueParameter);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(6),
                () => actualCollection[FieldOptionIdKey].ShouldBe(fieldId.ToString()),
                () => actualCollection[ActiveParameterKey].ShouldBe(active.ToString()),
                () => actualCollection[DisplayAsKey].ShouldBe(DisplayAsParameter),
                () => actualCollection[DisqualifierKey].ShouldBe(disqualifier.ToString()),
                () => actualCollection[OrderKey].ShouldBe(OrderParameter.ToString()),
                () => actualCollection[ValueKey].ShouldBe(ValueParameter),
                () => actualAddress.ShouldBe(" https://api.knowledgemarketing.com/2/customfields//options/666"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => result.ShouldBe(serverResponse));
        }

        [Test]
        public void UpdateFieldOption_ExceptionRaised_ApiLogWritten()
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
            var customField = new CustomField();
            var actualField = customField.UpdateFieldOption(
                Client.KM_API_Testing,
                FieldId,
                true,
                DisplayAsParameter,
                true,
                OrderParameter,
                ValueParameter);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(ExpectedExceptionMessage),
                () => actualMethodName.ShouldBe(ExpectedMethodName),
                () => actualClassName.ShouldBe(ExpectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => actualField.ShouldBeEmpty());
        }

        private static void ShimForJsonFunction<T>(string expected, T entity) where T : new()
        {
            ShimJsonFunctions.AllInstances.ToJsonOf1M0<T>((_, __) => expected);

            ShimJsonFunctions.AllInstances.FromJsonOf1String<T>((_, __) => entity);
        }
    }
}
