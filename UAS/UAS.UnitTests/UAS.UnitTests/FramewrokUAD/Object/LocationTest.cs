using System;
using System.IO;
using System.Net.Fakes;
using System.Text;
using FrameworkUAD.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.FramewrokUAD.Object
{
    [TestClass]
    public class LocationTest
    {
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void ValidateBingAddress_OnException_ReturnEmptyLocation()
        {
            // Arrange
            var loc = new Location();
            const string Country = "UNITED STATES";
            ShimWebRequest.CreateString = (str) => throw new Exception();

            // Act	
            var actualResult = Location.ValidateBingAddress(loc, Country);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.ShouldBe(loc);
        }

        [Test]
        public void ValidateBingAddress_LatIsPositiveAndIsNegative_ReturnLocation()
        {
            // Arrange
            var loc = new Location();
            const string Country = "Egypt";
            var shimHttpWebRequest = new ShimHttpWebRequest();
            ShimWebRequest.CreateString = (str) => shimHttpWebRequest.Instance;
            var shimHttpWebResponse = new ShimHttpWebResponse();
            var validationMsg = "Success - Good Address - Bing ";
            const int  Longitude = -12233;
            const int atitude = 56789;
            const string PostalCode = "2222";
            var xmlRes = new StringBuilder("<xml><root>")
                .Append("<StatusCode>200</StatusCode>")
                .Append($"<Latitude>{atitude}</Latitude>")
                .Append($"<Longitude>{Longitude}</Longitude>")
                .Append($"<PostalCode>{PostalCode}</PostalCode>")
                .Append("</root>")
                .Append("</xml>")
                .ToString();

            var xmlResWithConf = new StringBuilder("<xml><root>")
                .Append("<StatusCode>200</StatusCode>")
                .Append($"<Latitude>{atitude}</Latitude>")
                .Append($"<Longitude>{Longitude}</Longitude>")
                .Append($"<PostalCode>{PostalCode}</PostalCode>")
                .Append("<Confidence>High</Confidence>")
                .Append("</root>")
                .Append("</xml>")
                .ToString();
            ShimHttpWebRequest.AllInstances.GetResponse = (obj) => shimHttpWebResponse.Instance;
            var tries = 1;
            ShimHttpWebResponse.AllInstances.GetResponseStream = (obj) =>
            {
                if (tries == 9)
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(xmlResWithConf));
                }
                tries++;
                return new MemoryStream(Encoding.UTF8.GetBytes(xmlRes));
            };
            ShimHttpWebResponse.AllInstances.Close = (obj) => { };

            // Act	
            var actualResult = Location.ValidateBingAddress(loc, Country);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.IsValid.ShouldBeTrue();
            actualResult.Latitude.ShouldBe(atitude);
            actualResult.Longitude.ShouldBe(Longitude);
            actualResult.PostalCode.ShouldBe(PostalCode);
            actualResult.ValidationMessage.ShouldContain(validationMsg);
        }

        [Test]
        public void ValidateBingAddress_LatLongArePositive_InvalidLocation()
        {
            // Arrange
            var loc = new Location();
            const string Country = "Egypt";
            var shimHttpWebRequest = new ShimHttpWebRequest();
            ShimWebRequest.CreateString = (str) => shimHttpWebRequest.Instance;
            var shimHttpWebResponse = new ShimHttpWebResponse();
            var validationMsg = "BING NO ADDRESS";
            const int Longitude = 12233;
            const int  Latitude = 56789;
            const string PostalCode = "2222";
            var xmlRes = new StringBuilder("<xml><root>")
                 .Append("<StatusCode>200</StatusCode>")
                 .Append($"<Latitude>{Latitude}</Latitude>")
                 .Append($"<Longitude>{Longitude}</Longitude>")
                 .Append($"<PostalCode>{PostalCode}</PostalCode>")
                 .Append("</root>")
                 .Append("</xml>")
                 .ToString();

            var xmlResWithConf = new StringBuilder("<xml><root>")
                .Append("<StatusCode>200</StatusCode>")
                .Append($"<Latitude>{Latitude}</Latitude>")
                .Append($"<Longitude>{Longitude}</Longitude>")
                .Append($"<PostalCode>{PostalCode}</PostalCode>")
                .Append("<Confidence>High</Confidence>")
                .Append("</root>")
                .Append("</xml>")
                .ToString();
            ShimHttpWebRequest.AllInstances.GetResponse = (obj) => shimHttpWebResponse.Instance;
            var tries = 1;
            ShimHttpWebResponse.AllInstances.GetResponseStream = (obj) =>
            {
                if (tries == 9)
                {
                    return new MemoryStream(Encoding.UTF8.GetBytes(xmlResWithConf));
                }
                tries++;
                return new MemoryStream(Encoding.UTF8.GetBytes(xmlRes));
            };
            ShimHttpWebResponse.AllInstances.Close = (obj) => { };

            // Act	
            var actualResult = Location.ValidateBingAddress(loc, Country);

            // Assert
            actualResult.ShouldNotBeNull();
            actualResult.IsValid.ShouldBeFalse();
            actualResult.ValidationMessage.ShouldContain(validationMsg);
        }
    }
}
