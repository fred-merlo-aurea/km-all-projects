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
using Address = FrameworkSubGen.BusinessLogic.API.Address;
using Entity = FrameworkSubGen.Entity;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic.API
{
    [TestFixture]
    public class AddressTest
    {
        private const string UpdateAddressErrorString = "address, city, state and subscriber_id are required";
        private const string AddressEntityCity = "EntityCity";
        private const string AddressEntityAddress = "EntityAddress";
        private const string AddressEntityCountry = "EntityCountry";
        private const string AddressEntityFirstName = "EntityFirstName";
        private const string AddressEntityLastName = "EntityLastName";
        private const string AddressEntityState = "AddressEntityState";
        private const string AddressEntityZipCode = "AdressEntityZipCode";
        private const string AddressEntityCompany = "AddressEntityCompany";
        private const string AddressEntityAddressLine2 = "AddressEntityAddressLine2";
        private const int AddressEntitysubscriberId = 42;
        private const int AddressEntityAddressId = 123;

        private const string AddressParameterKey = "address";
        private const string AdressLine2ParameterKey = "address_line_2";
        private const string CityParameterKey = "city";
        private const string StateParameterKey = "state";
        private const string SubscriberIdParameterKey = "subscriber_id";
        private const string ZipCodeParameterKey = "zip_code";
        private const string CompanyParameterKey = "company";
        private const string CountryParameterKey = "country";
        private const string FirstNameParameterKey = "first_name";
        private const string LastNameParameterKey = "last_name";

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
        public void CreateAddress_AddressNull_Return0()
        {
            // Arrange
            var addressBusinessObject = new Address();

            // Act
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                null);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_AddressEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.address = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_CityEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.city = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_StateEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.state = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_SubscriberIdInvalid_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.subscriber_id = -1;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_ZipCodeEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.zip_code = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(0);
        }

        [Test]
        public void CreateAddress_CorrectParameterNoData_PostToServerZeroResult()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
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
            var addressBusinessObject = new Address();
            var field = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(10),
                () => actualCollection[AddressParameterKey].ShouldBe(addressEntity.address),
                () => actualCollection[AdressLine2ParameterKey].ShouldBe(addressEntity.address_line_2),
                () => actualCollection[CityParameterKey].ShouldBe(addressEntity.city),
                () => actualCollection[StateParameterKey].ShouldBe(addressEntity.state),
                () => actualCollection[SubscriberIdParameterKey].ShouldBe(addressEntity.subscriber_id.ToString()),
                () => actualCollection[ZipCodeParameterKey].ShouldBe(addressEntity.zip_code),
                () => actualCollection[CompanyParameterKey].ShouldBe(addressEntity.company),
                () => actualCollection[CountryParameterKey].ShouldBe(addressEntity.country),
                () => actualCollection[FirstNameParameterKey].ShouldBe(addressEntity.first_name),
                () => actualCollection[LastNameParameterKey].ShouldBe(addressEntity.last_name),
                () => actualAddress.ShouldBe($" https://api.knowledgemarketing.com/2/addresses/"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(0));
        }

        [Test]
        public void CreateAddress_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            const string serverResponse = "{ \"doesnotmatter\" : 4 }";

            var addressEntity = CreateAddressEntity();
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
            var addressBusinessObject = new Address();
            var field = addressBusinessObject.Create(
                Client.KM_API_Testing, addressEntity);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(10),
                () => actualCollection[AddressParameterKey].ShouldBe(addressEntity.address),
                () => actualCollection[AdressLine2ParameterKey].ShouldBe(addressEntity.address_line_2),
                () => actualCollection[CityParameterKey].ShouldBe(addressEntity.city),
                () => actualCollection[StateParameterKey].ShouldBe(addressEntity.state),
                () => actualCollection[SubscriberIdParameterKey].ShouldBe(addressEntity.subscriber_id.ToString()),
                () => actualCollection[ZipCodeParameterKey].ShouldBe(addressEntity.zip_code),
                () => actualCollection[CompanyParameterKey].ShouldBe(addressEntity.company),
                () => actualCollection[CountryParameterKey].ShouldBe(addressEntity.country),
                () => actualCollection[FirstNameParameterKey].ShouldBe(addressEntity.first_name),
                () => actualCollection[LastNameParameterKey].ShouldBe(addressEntity.last_name),
                () => actualAddress.ShouldBe($" https://api.knowledgemarketing.com/2/addresses/"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => field.ShouldBe(4));
        }

        [Test]
        public void CreateAddress_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            const string expectedExceptionMessage = "MyThrownWebException";
            const string expectedClassName = "FrameworkSubGen.BusinessLogic.API.Address";
            const string expectedMethodName = "Create";
            var addressEntity = CreateAddressEntity();
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
            var addressBusinessObject = new Address();
            var actualField = addressBusinessObject.Create(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(expectedExceptionMessage),
                () => actualMethodName.ShouldBe(expectedMethodName),
                () => actualClassName.ShouldBe(expectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => actualField.ShouldBe(0));
        }

        [Test]
        public void UpdateAddress_AddressEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.address = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(UpdateAddressErrorString);
        }

        [Test]
        public void UpdateAddress_CityEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.city = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(UpdateAddressErrorString);
        }

        [Test]
        public void UpdateAddress_StateEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.state = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(UpdateAddressErrorString);
        }

        [Test]
        public void UpdateAddress_ZipCodeEmpty_Return0()
        {
            // Arrange
            var addressEntity = CreateAddressEntity();
            addressEntity.zip_code = string.Empty;

            // Act
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            returnValue.ShouldBe(UpdateAddressErrorString);
        }

        [Test]
        public void UpdateAddress_CorrectParameterData_PostToServerValidResult()
        {
            // Arrange
            const string serverResponse = "{ \"doesnotmatter\" : something }";

            var addressEntity = CreateAddressEntity();
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
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            actualCollection.ShouldSatisfyAllConditions(
                () => actualCollection.Count.ShouldBe(10),
                () => actualCollection[AddressParameterKey].ShouldBe(addressEntity.address),
                () => actualCollection[AdressLine2ParameterKey].ShouldBe(addressEntity.address_line_2),
                () => actualCollection[CityParameterKey].ShouldBe(addressEntity.city),
                () => actualCollection[StateParameterKey].ShouldBe(addressEntity.state),
                () => actualCollection[SubscriberIdParameterKey].ShouldBe(addressEntity.subscriber_id.ToString()),
                () => actualCollection[ZipCodeParameterKey].ShouldBe(addressEntity.zip_code),
                () => actualCollection[CompanyParameterKey].ShouldBe(addressEntity.company),
                () => actualCollection[CountryParameterKey].ShouldBe(addressEntity.country),
                () => actualCollection[FirstNameParameterKey].ShouldBe(addressEntity.first_name),
                () => actualCollection[LastNameParameterKey].ShouldBe(addressEntity.last_name),
                () => actualAddress.ShouldBe($" https://api.knowledgemarketing.com/2/addresses/{AddressEntityAddressId}"),
                () => actualMethod.ShouldBe(HttpMethod.POST.ToString()),
                () => returnValue.ShouldBe(serverResponse));
        }

        [Test]
        public void UpdateAddress_ExceptionRaised_ApiLogWritten()
        {
            // Arrange
            const string expectedExceptionMessage = "MyThrownWebException";
            const string expectedClassName = "FrameworkSubGen.BusinessLogic.API.Address";
            const string expectedMethodName = "Update";
            var addressEntity = CreateAddressEntity();
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
            var addressBusinessObject = new Address();
            var returnValue = addressBusinessObject.Update(
                Client.KM_API_Testing,
                addressEntity);

            // Assert
            actualException.ShouldSatisfyAllConditions(
                () => actualException.ShouldBeOfType(typeof(WebException)),
                () => actualException.Message.ShouldBe(expectedExceptionMessage),
                () => actualMethodName.ShouldBe(expectedMethodName),
                () => actualClassName.ShouldBe(expectedClassName),
                () => apiLogCounter.ShouldBe(1),
                () => returnValue.ShouldBe("error"));
        }

        private static Entity.Address CreateAddressEntity()
        {
            var address = new Entity.Address
            {
                city = AddressEntityCity,
                address = AddressEntityAddress,
                country = AddressEntityCountry,
                first_name = AddressEntityFirstName,
                last_name = AddressEntityLastName,
                state = AddressEntityState,
                zip_code = AddressEntityZipCode,
                subscriber_id = AddressEntitysubscriberId,
                company = AddressEntityCompany,
                address_line_2 = AddressEntityAddressLine2,
                address_id = AddressEntityAddressId
            };

            return address;
        }

        private static void ShimForJsonFunction<T>(string expected, T entity) where T : new()
        {
            ShimJsonFunctions.AllInstances.ToJsonOf1M0<T>((_, __) => expected);

            ShimJsonFunctions.AllInstances.FromJsonOf1String<T>((_, __) => entity);
        }
    }
}
