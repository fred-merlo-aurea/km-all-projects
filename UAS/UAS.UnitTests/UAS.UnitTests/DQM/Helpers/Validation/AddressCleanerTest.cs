using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Fakes;
using System.Text;
using System.Xml;
using DQM.Helpers.Validation;
using FrameworkUAD.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
namespace UAS.UnitTests.DQM.Helpers.Validation
{
    /// <summary>
    /// Unit test for <see cref="AddressCleaner"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AddressCleanerTest
    {
        private const string BingServer = "Bing";
        private const string Street = "Sundarnagar";
        private const string PostalCode = "175034";
        private const string Region = "North";
        private const string Latitude = "31.5332397";
        private const string Longitude = "76.8922729";
        private const string ValidLongitude = "-12.2020";
        private const string HttpStatusOk = "200";
        protected IDisposable _shimObject;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void DisposeContext()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void ValidateBingAddress_AddressLocationIsNull_ReturnsEmptyObject()
        {
            // Arrange
            var location = new AddressLocation();
            // Act
            var result = AddressCleaner.ValidateBingAddress(location);

            // Assert
            result.ShouldNotBeNull();
            result.Country.ShouldBeNull();
            result.City.ShouldBeEmpty();
            result.IsAddressValidated.ShouldBeFalse();
            result.Latitude.ShouldBe(0);
            result.Longitude.ShouldBe(0);
            result.PostalCode.ShouldBeEmpty();
            result.Region.ShouldBeEmpty();
            result.Street.ShouldBeEmpty();
            result.UpdatedByUserID.ShouldBe(1);
            result.AddressValidationMessage.ShouldBeNull();
            result.AddressValidationSource.ShouldBeSameAs(BingServer);
        }

        [Test]
        public void ValidateBingAddress_LongitudeGreaterThenZero_ReturnsLocationObject()
        {
            // Arrange
            var location = CreateAddressLocationObject();
            CreateShimWebRequest(Latitude, Longitude);

            // Act
            var result = AddressCleaner.ValidateBingAddress(location);

            // Assert
            result.ShouldNotBeNull();
            result.City.ShouldBe(Street);
            result.IsAddressValidated.ShouldBeFalse();
        }

        [Test]
        public void ValidateBingAddress_LongitudeLessThenZero_ReturnsSuccessObject()
        {
            var location = CreateAddressLocationObject();
            CreateShimWebRequest(Latitude, ValidLongitude);

            // Act
            var result = AddressCleaner.ValidateBingAddress(location);

            // Assert
            result.ShouldNotBeNull();
            result.City.ShouldBe(Street);
            result.IsAddressValidated.ShouldBeTrue();
            result.Latitude.ShouldBe(Convert.ToDecimal(Latitude));
            result.Longitude.ShouldBe(Convert.ToDecimal(ValidLongitude));
        }

        private AddressLocation CreateAddressLocationObject()
        {
            // Arrange
            return new AddressLocation(_PostalCode: PostalCode, _City: Street, _Street: Street, _Region: Region);
        }

        private void CreateShimWebRequest(string latitudeCode, string longitudeCode)
        {
            var requestShim = new ShimHttpWebRequest();
            var responseShim = new ShimHttpWebResponse()
            {
                GetResponseStream = () => CreateOutputStream(latitudeCode, longitudeCode)
            };
            ShimWebRequest.CreateString = (uri) => requestShim.Instance;
            requestShim.GetRequestStream = () => new MemoryStream();
            requestShim.GetResponse = () => responseShim.Instance;
            responseShim.GetResponseStream = () => CreateOutputStream(latitudeCode, longitudeCode);
        }

        private Stream CreateOutputStream(string latitudeCode, string longitudeCode)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("packet");
            var statusCode = doc.CreateElement("StatusCode");
            statusCode.InnerText = HttpStatusOk;
            root.AppendChild(statusCode);
            var confidence = doc.CreateElement("Confidence");
            confidence.InnerText = "High";
            root.AppendChild(confidence);
            var latitude = doc.CreateElement("Latitude");
            latitude.InnerText = latitudeCode;
            root.AppendChild(latitude);
            var longitude = doc.CreateElement("Longitude");
            longitude.InnerText = longitudeCode;
            root.AppendChild(longitude);
            var postalCode = doc.CreateElement("PostalCode");
            root.AppendChild(postalCode);
            doc.AppendChild(root);
            var byteArray = Encoding.UTF8.GetBytes(doc.InnerXml);
            var stream = new MemoryStream(byteArray);
            return stream;
        }
    }
}

