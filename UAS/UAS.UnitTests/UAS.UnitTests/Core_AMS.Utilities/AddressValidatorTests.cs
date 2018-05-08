using System;
using System.Collections.Generic;
using System.Fakes;
using System.IO;
using System.Linq;
using System.Net.Fakes;
using System.Text;
using System.Xml;
using Core_AMS.Utilities;
using Core_AMS.Utilities.Fakes;
using MapPoint;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using AddressLocation = Core_AMS.Utilities.AddressLocation;
using AddressValidator = Core_AMS.Utilities.AddressValidator;

namespace UAS.UnitTests.Core_AMS.Utilities
{
    public class AddressValidatorTests
    {
        private IDisposable _shimObject;
        private const string HttpStatusOk = "OK";
        private const string HttpStatusNotOk = "INVALID_REQUEST";
        private const string LocationTypeRooftop = "ROOFTOP";
        private const string LocationTypeRange = "RANGE_INTERPOLATED";
        private const string LocationTypeGeometricCenter = "GEOMETRIC_CENTER";
        private const string LocationTypeApproximate = "APPROXIMATE";
        private const string SamplePostalCode = "12345";
        private const char WhiteSpace = ' ';

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            ShimDateTime.NowGet = () => DateTime.MinValue;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase("address", "address")]
        [TestCase("address address", "address")]
        [TestCase("address1      address2", "address1 address2")]
        [TestCase("address1 address2", "address1 address2")]
        [TestCase("addres.s1 address1 address2", "address1 address2")]
        [TestCase("a<>?;:|^&`()\\$d{}[]d+re~s@/s.1 !ad,d#r%e'ss*2", "address1 address2")]
        [TestCase("", "")]
        [TestCase("  ", "")]
        [TestCase(null, "")]
        public void CleanAddress_DifferentInputs_ReturnsCleanedAddress(
            string address, 
            string expectedResult)
        {
            // Arrange
            var addressValidator = new AddressValidator();

            // Act
            var actualResult = addressValidator.CleanAddress(address);

            // Assert
            actualResult.ShouldBe(expectedResult);
        }
        
        [Test]
        public void ValidateAddresses_NullAddresses_ReturnsEmptyAddressesList()
        {
            // Arrange
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = false;
            var addressValidator = new AddressValidator();
            var expectedCount = 0;

            // Act   
            var actualResult = addressValidator.ValidateAddresses(null, validateExisting, useMapPoint, useBingGoogle);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.Count.ShouldBe(expectedCount);
        }

        [Test]
        [TestCase("validation_message", "validation_message")]
        [TestCase("", "Invalid Address {0}")]
        [TestCase("   ", "Invalid Address {0}")]
        [TestCase(null, "Invalid Address {0}")]
        public void ValidateAddresses_NoExistingNoMapPointNoUseBing_ReturnsValidAddresses(
            string validationMessage, 
            string expectedValidationMessageFormat)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = false;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            addressesToValidate.First().IsValid = false;
            addressesToValidate.First().ValidationMessage = validationMessage;

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = false;
            firstExpectedResult.ValidationMessage = 
                String.Format(expectedValidationMessageFormat, DateTime.MinValue);

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate, 
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().ValidationMessage.ShouldBe(
                        expectedResult.First().ValidationMessage));
        }

        [Test]
        [TestCase(HttpStatusOk, LocationTypeGeometricCenter)]
        [TestCase(HttpStatusNotOk, LocationTypeRooftop)]
        [TestCase(HttpStatusNotOk, LocationTypeRange)]
        [TestCase(HttpStatusNotOk, LocationTypeApproximate)]
        [TestCase(HttpStatusNotOk, LocationTypeGeometricCenter)]
        public void ValidateAddresses_UseBingNonValidAddress_AddressNotFound(
            string apiResponse, 
            string locationType)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = true;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            
            const string latitude = "12233";
            const string longitude = "56789";            
            CreateShimWebRequest(apiResponse, latitude, longitude, String.Empty, locationType);            

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = false;
            firstExpectedResult.ValidationMessage += 
                $"\n\nGOOGLE NO ADDRESS - Status:  {apiResponse}] {DateTime.MinValue}";

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().ValidationMessage.Equals(
                    expectedResult.First().ValidationMessage, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ValidateAddresses_UseBingThrowsError_ReturnsValidAddresses()
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = true;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            firstAddressesToValidate.ErrorOccured = false;

            const string formattedAddress = "";
            const string latitude = "";
            const string longitude = "";
            CreateShimWebRequest(HttpStatusOk, latitude, longitude, formattedAddress, LocationTypeRooftop, true);

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = false;
            firstExpectedResult.ErrorOccured = true;

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().ErrorOccured.ShouldBe(firstExpectedResult.ErrorOccured));
        }

        [Test]
        [TestCase(HttpStatusOk, LocationTypeRange)]
        [TestCase(HttpStatusOk, LocationTypeApproximate)]
        public void ValidateAddresses_UseBingValidAddress_AddressFound(
            string apiResponse, 
            string locationType)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = true;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            
            const double latitude = 12233;
            const double longitude = 56789;
            const string formattedAddress = "1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA";
            CreateShimWebRequest(
                apiResponse, 
                latitude.ToString(),  
                longitude.ToString(), 
                formattedAddress, 
                locationType);
            ShimDateTime.NowGet = () => DateTime.MinValue;

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = true;
            firstExpectedResult.Latitude = latitude;
            firstExpectedResult.Longitude = longitude;
            firstExpectedResult.ValidationMessage = 
                $"Street Level - Good Address - Google {DateTime.MinValue}";

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().IsValid.ShouldBe(firstExpectedResult.IsValid),
                () => actualResult.First().Latitude.ShouldBe(firstExpectedResult.Latitude),
                () => actualResult.First().Longitude.ShouldBe(firstExpectedResult.Longitude),                
                () => actualResult.First().ValidationMessage.Equals(
                    expectedResult.First().ValidationMessage, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9, USA", "9")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94, USA", "94")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940, USA", "940")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404, USA", "9404")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA", "94043")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940431, USA", "940431")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404312, USA", "9404312")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94043123, USA", "94043", "123")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940431234, USA", "94043", "1234")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404312345, USA", "94043", "12345")]
        public void ValidateAddresses_UseBingValidAddressLocationRooftop_AddressFound(
            string formattedAddress, 
            string expectedPostalCode, 
            string expectedPostalCodePlus4 = "")
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var useBingGoogle = true;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            firstAddressesToValidate.PostalCodePlusFour = expectedPostalCodePlus4;

            const double latitude = 12233;
            const double longitude = 56789;
            CreateShimWebRequest(
                HttpStatusOk, 
                latitude.ToString(),
                longitude.ToString(), 
                formattedAddress, 
                LocationTypeRooftop);
            ShimDateTime.NowGet = () => DateTime.MinValue;

            var addressParts = formattedAddress.Split(',');
            var stateZip = addressParts[2].Trim().Split(WhiteSpace);
            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = true;
            firstExpectedResult.Latitude = latitude;
            firstExpectedResult.Longitude = longitude;
            firstExpectedResult.Street = addressParts[0];
            firstExpectedResult.City = addressParts[1];
            firstExpectedResult.Country = addressParts[3];
            firstExpectedResult.Region = stateZip[0];
            firstExpectedResult.PostalCode = expectedPostalCode;
            firstExpectedResult.PostalCodePlusFour = expectedPostalCodePlus4;
            firstExpectedResult.ValidationMessage =
                $"Rooftop - Good Address - Google {DateTime.MinValue}";

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().IsValid.ShouldBe(firstExpectedResult.IsValid),
                () => actualResult.First().Latitude.ShouldBe(firstExpectedResult.Latitude),
                () => actualResult.First().Longitude.ShouldBe(firstExpectedResult.Longitude),
                () => actualResult.First().ValidationMessage.Equals(
                    expectedResult.First().ValidationMessage, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [TestCase("country", "country", "PO BOX", "", "", "")]
        [TestCase("country", "country", "P O BOX", "", "test", "")]
        [TestCase("country", "country", "BOX ", "", "new", "")]
        [TestCase("country", "country", "PO ", "", "T X", "")]
        [TestCase("country", "country", "stree&t", "", " NT", "")]
        public void ValidateAddresses_UseMapPoint_AddressFound(
            string country, 
            string expectedCountry,
            string street, 
            string expectedStreet, 
            string region, 
            string expectedRegion)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = true;
            var useBingGoogle = false;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            firstAddressesToValidate.Country = country;
            firstAddressesToValidate.Street = street;
            firstAddressesToValidate.Region = region;
            
            CreateMapMock(GeoCountry.geoCountryUnitedStates, 1, 1);

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = false;
            firstExpectedResult.Street = expectedStreet;
            firstExpectedResult.Country = expectedCountry;
            firstExpectedResult.Region = expectedRegion;
            firstExpectedResult.ValidationMessage = $"Invalid from MapPoint {DateTime.MinValue}";

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate, 
                validateExisting, 
                useMapPoint, 
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().IsValid.ShouldBe(firstExpectedResult.IsValid),
                () => actualResult.First().Country.ShouldBe(firstExpectedResult.Country),
                () => actualResult.First().Street.ShouldBe(firstExpectedResult.Street),
                () => actualResult.First().Region.ShouldBe(firstExpectedResult.Region),
                () => actualResult.First().ValidationMessage.Equals(
                    expectedResult.First().ValidationMessage, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [TestCase("", null, 1)]
        [TestCase(null, null, 1)]
        [TestCase("United States", GeoCountry.geoCountryUnitedStates, 1)]
        [TestCase("United 123 States of America", GeoCountry.geoCountryUnitedStates, 1)]
        [TestCase("Canada", GeoCountry.geoCountryCanada, 1)]
        [TestCase("invalid country", GeoCountry.geoCountryCanada, 0)]
        public void ValidateAddresses_UseMapPoint_MapCalled(
            string country,
            GeoCountry? geoCountry,
            int numberOfMapCalls)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = true;
            var useBingGoogle = false;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;
            firstAddressesToValidate.Country = country;
           
            var mapMock = CreateMapMock(geoCountry, 1, 1);

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = false;

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting,
                useMapPoint,
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => mapMock.Verify(x => x.FindAddressResults(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<GeoCountry?>(c => c == null || c == geoCountry.Value)),
                    numberOfMapCalls > 0 ?
                    Times.AtLeast(numberOfMapCalls) :
                    Times.Never())
                );
        }

        [Test]
        [TestCase("", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("United States", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("United States", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoFirstResultGood)]
        [TestCase("Canada", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("Canada", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoFirstResultGood)]
        [TestCase("Canada", "Zipcode Only - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAmbiguousResults)]
        public void ValidateAddresses_UseMapPoint_AddressFound(
            string country, 
            string validationMessageFormat,
            GeoFindResultsQuality resultsQuality)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = true;
            var useBingGoogle = false;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var firstAddressesToValidate = addressesToValidate.First();
            firstAddressesToValidate.IsValid = false;            
            firstAddressesToValidate.Country = country;

            CreateMapMock(null, 1, 1, resultsQuality);

            var expectedResult = CreateAddressLocations(numberOfAddresses).ToList();
            var firstExpectedResult = expectedResult.First();
            firstExpectedResult.IsValid = true;
            firstExpectedResult.ValidationMessage = String.Format(validationMessageFormat, DateTime.MinValue);

            // Act   
            var actualResult = addressValidator.ValidateAddresses(
                addressesToValidate,
                validateExisting,
                useMapPoint,
                useBingGoogle);

            // Assert
            actualResult.ShouldSatisfyAllConditions(
                () => actualResult.ShouldNotBeNull(),
                () => actualResult.Count.ShouldBe(expectedResult.Count),
                () => actualResult.First().IsValid.ShouldBe(firstExpectedResult.IsValid),
                () => actualResult.First().ValidationMessage.ShouldBe(firstExpectedResult.ValidationMessage)
                );
        }

        [Test]
        [TestCase("", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("United States", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("United States", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoFirstResultGood)]
        [TestCase("Canada", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAllResultsValid)]
        [TestCase("Canada", "Rooftop - Good Address - MapPoint {0}", GeoFindResultsQuality.geoFirstResultGood)]
        [TestCase("Canada", "Zipcode Only - Good Address - MapPoint {0}", GeoFindResultsQuality.geoAmbiguousResults)]
        public void ValidateAddress_UseMapPoint_AddressFound(
           string country,
           string validationMessageFormat,
           GeoFindResultsQuality resultsQuality)
        {
            // Arrange
            const int numberOfAddresses = 1;
            var useMapPoint = true;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var addressToValidate = addressesToValidate.First();
            addressToValidate.IsValid = false;
            addressToValidate.Country = country;

            CreateMapMock(null, 1, 1, resultsQuality);

            var expectedResultIsValid = true;
            var expectedResultValidationMessage = String.Format(validationMessageFormat, DateTime.MinValue);

            // Act   
            addressValidator.ValidateAddress(ref addressToValidate, useMapPoint);

            // Assert
            addressToValidate.ShouldSatisfyAllConditions(
                () => addressToValidate.IsValid.ShouldBe(expectedResultIsValid),
                () => addressToValidate.ValidationMessage.ShouldBe(expectedResultValidationMessage)
                );
        }

        [Test]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9, USA", "9")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94, USA", "94")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940, USA", "940")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404, USA", "9404")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA", "94043")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940431, USA", "940431")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404312, USA", "9404312")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 94043123, USA", "94043", "123")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 940431234, USA", "94043", "1234")]
        [TestCase("1600 Amphitheatre Pkwy, Mountain View, CA 9404312345, USA", "94043", "12345")]
        public void ValidateAddress_DontUseMapPoint_AddressFound(
            string formattedAddress,
            string expectedPostalCode,
            string expectedPostalCodePlus4 = "")
        {
            // Arrange
            const int numberOfAddresses = 1;
            var validateExisting = false;
            var useMapPoint = false;
            var addressValidator = new AddressValidator();

            var addressesToValidate = CreateAddressLocations(numberOfAddresses).ToList();
            var addressToValidate = addressesToValidate.First();
            addressToValidate.IsValid = false;
            addressToValidate.PostalCodePlusFour = expectedPostalCodePlus4;

            const double latitude = 12233;
            const double longitude = 56789;
            CreateShimWebRequest(
                HttpStatusOk,
                latitude.ToString(),
                longitude.ToString(),
                formattedAddress,
                LocationTypeRooftop);
            ShimDateTime.NowGet = () => DateTime.MinValue;
            
            var expectedResultIsValid = true;
            var expectedResultLatitude = latitude;
            var expectedResultLongitude = longitude;
            var expectedResultValidationMessage =
                $"Rooftop - Good Address - Google {DateTime.MinValue}";

            // Act   
            addressValidator.ValidateAddress(ref addressToValidate, useMapPoint);

            // Assert
            addressToValidate.ShouldSatisfyAllConditions(
                () => addressToValidate.IsValid.ShouldBe(expectedResultIsValid),
                () => addressToValidate.Latitude.ShouldBe(expectedResultLatitude),
                () => addressToValidate.Longitude.ShouldBe(expectedResultLongitude),
                () => addressToValidate.ValidationMessage.Equals(
                    expectedResultValidationMessage, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<AddressLocation> CreateAddressLocations(int count)
        {
            if (count <= 0)
            {
                yield break;
            }

            var differentiator = 0;
            for(var i = 0; i < count; i++)
            {
                differentiator++;
                yield return new AddressLocation
                {
                    City = $"city{differentiator}",
                    Country = $"country{differentiator}",
                    County = $"county{differentiator}",
                    IsValid = true,
                    Latitude = differentiator,
                    Longitude = differentiator,
                    MailStop = $"mail_stop{differentiator}",
                    PostalCode = $"123{differentiator}",
                    Street = $"street{differentiator}",
                    StreetName = $"street_name{differentiator}",
                    StreetNumber = $"{differentiator}",
                    Region = $"region{differentiator}",
                    DateUpdated = DateTime.Now,
                    ErrorOccured = false,
                    PostalCodePlusFour = $"123{differentiator}",
                    ValidationMessage = $"validation_message{differentiator}"
                };
            }
        }

        private void CreateShimWebRequest(
            string responseCode,
            string latitudeCode, 
            string longitudeCode, 
            string address, 
            string locationType, 
            bool throwException = false)
        {
            var requestShim = new ShimHttpWebRequest();
            var responseShim = new ShimHttpWebResponse()
            {
                GetResponseStream = () => CreateOutputStream(responseCode, 
                    latitudeCode, longitudeCode, address, locationType)
            };

            ShimWebRequest.CreateString = (uri) => requestShim.Instance;
            requestShim.GetRequestStream = () => new MemoryStream();
            requestShim.GetResponse = () => responseShim.Instance;
            responseShim.GetResponseStream = () =>
            {
                if (throwException)
                {
                    throw new Exception();
                }

                return CreateOutputStream(responseCode, latitudeCode, longitudeCode, address, locationType);
            };
        }

        private Stream CreateOutputStream(
            string responseCode, 
            string latitudeCode, 
            string longitudeCode, 
            string address, 
            string locationType)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("packet");

            var statusCodeNode = doc.CreateElement("status");
            statusCodeNode.InnerText = responseCode;
            root.AppendChild(statusCodeNode);

            var locationTypeNode = doc.CreateElement("location_type");
            locationTypeNode.InnerText = locationType;
            root.AppendChild(locationTypeNode);

            var latitudeNode = doc.CreateElement("lat");
            latitudeNode.InnerText = latitudeCode;
            root.AppendChild(latitudeNode);

            var longitudeNode = doc.CreateElement("lng");
            longitudeNode.InnerText = longitudeCode;
            root.AppendChild(longitudeNode);

            var formattedAddressNode = doc.CreateElement("formatted_address");
            formattedAddressNode.InnerText = address;
            root.AppendChild(formattedAddressNode);

            doc.AppendChild(root);

            var byteArray = Encoding.UTF8.GetBytes(doc.InnerXml);
            var stream = new MemoryStream(byteArray);

            // stream should not be closed here, because it is needed in the response deserialization
            return stream;
        }

        private Mock<Map> CreateMapMock(
            GeoCountry? country,
            double latitude, 
            double longitude,
            GeoFindResultsQuality resultsQuality = GeoFindResultsQuality.geoAllResultsValid)
        {
            var streetAddressMock = new Mock<StreetAddress>();
            streetAddressMock.SetupAllProperties();
            streetAddressMock
                .Setup(x => x.Country)
                .Returns(country ?? GeoCountry.geoCountryUnitedKingdom);
            streetAddressMock
                .Setup(x => x.City)
                .Returns(It.IsAny<string>);
            streetAddressMock
                .Setup(x => x.OtherCity)
                .Returns(It.IsAny<string>);
            streetAddressMock
                .Setup(x => x.Region)
                .Returns(It.IsAny<string>);
            streetAddressMock
                .Setup(x => x.PostalCode)
                .Returns(SamplePostalCode);
            streetAddressMock
                .Setup(x => x.Street)
                .Returns(It.IsAny<string>);

            var placeCategoryMock = new Mock<PlaceCategory>();
            placeCategoryMock
                .Setup(x => x.Name)
                .Returns(It.IsAny<string>);
            placeCategoryMock
                .Setup(x => x.Index)
                .Returns(It.IsAny<int>);

            var locationMock = new Mock<Location>();
            locationMock.SetupAllProperties();
            locationMock
                .Setup(x => x.StreetAddress)
                .Returns(streetAddressMock.Object);
            locationMock
                .Setup(x => x.Latitude)
                .Returns(latitude);
            locationMock
                .Setup(x => x.Longitude)
                .Returns(longitude);
            locationMock
                .Setup(x => x.PlaceCategory)
                .Returns(placeCategoryMock.Object);

            var locations = new List<Location> { locationMock.Object };
            var resultsMock = new Mock<FindResults>();
            resultsMock
                .Setup(x => x.ResultsQuality)
                .Returns(resultsQuality);
            resultsMock
                .Setup(m => m.Count)
                .Returns(() => locations.Count);
            resultsMock
                .Setup(m => m.GetEnumerator())
                .Returns(() => locations.GetEnumerator());

            var mapMock = new Mock<Map>();
            mapMock.Setup(x => x.FindAddressResults(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
                .Returns(resultsMock.Object);
            var applicationServiceMock = new Mock<Application>();
            applicationServiceMock
                .Setup(x => x.NewMap(It.IsAny<string>()))
                .Returns(mapMock.Object);

            var myApp = new MapApplication(applicationServiceMock.Object);
            ShimMapPointService.AllInstances.CreateApplication = instance => myApp;

            return mapMock;
        }
    }
}
