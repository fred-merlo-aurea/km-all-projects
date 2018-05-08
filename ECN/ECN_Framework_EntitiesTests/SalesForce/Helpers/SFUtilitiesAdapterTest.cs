using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Convertors;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ECN_Framework_EntitiesTests.SalesForce.Helpers
{
    [TestFixture]
    public class SFUtilitiesAdapterTest
    {
        private const string WhereParam = "where";
        private const string AccessToken = "accessToken";
        private const string BaseUrl = "http://localhost";
        private const string ContentType = "application/json";
        private const string HttpPost = "POST";
        private const string HttpGet = "GET";
        private const string AuthHeader = "Authorization";
        private const string PrettyPrintHeader = "X-PrettyPrint";
        private const SF_Utilities.SFObject ObjectType = SF_Utilities.SFObject.Account;
        private const string PrettyPrintValue = "1";
        private readonly string _authValue = $"OAuth {AccessToken}";
        private readonly string _expectedUrl = $"{BaseUrl}/services/data/v28.0/sobjects/{ObjectType.ToString()}/";
        private readonly string _expectedWhereUrl = $"{BaseUrl}/services/data/v28.0/query/?q=SELECT%20Id%20FROM%20JustForUnitTest%20where";
        private IDisposable _shimsContext;

        [SetUp]
        public void SetUp()
        {
            _shimsContext = ShimsContext.Create();
            SF_Authentication.Token = new SF_Token
            {
                instance_url = BaseUrl
            };
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
            SF_Authentication.Token = null;
        }

        [Test]
        public void Insert_PassJsonWithSuccessProperty_ReturnsTrue()
        {
            // Arrange
            var utility = new SFUtilitiesAdapter();
            var request = BuildRequestWithResult(GetResult(true));
            string requestUrl = null;
            ShimWebRequest.CreateString = u =>
            {
                requestUrl = u;
                return request;
            };

            // Act
            var result = utility.Insert(AccessToken, new object(), ObjectType);

            // Asssert
            result.ShouldBeTrue();
            requestUrl.ShouldBe(_expectedUrl);
            request.ShouldSatisfyAllConditions(
                () => request.Headers[AuthHeader].ShouldBe(_authValue),
                () => request.Method.ShouldBe(HttpPost),
                () => request.ContentType.ShouldBe(ContentType));
        }

        [Test]
        public void Insert_PassJsonWithErrors_ReturnsFalse()
        {
            // Arrange
            var utility = new SFUtilitiesAdapter();
            var request = BuildRequestWithResult(GetResult(false));
            string requestUrl = null;
            ShimWebRequest.CreateString = u =>
            {
                requestUrl = u;
                return request;
            };

            // Act
            var result = utility.Insert(AccessToken, new object(), ObjectType);

            // Asssert
            result.ShouldBeFalse();
            requestUrl.ShouldBe(_expectedUrl);
            request.ShouldSatisfyAllConditions(
                () => request.Headers[AuthHeader].ShouldBe(_authValue),
                () => request.Method.ShouldBe(HttpPost),
                () => request.ContentType.ShouldBe(ContentType));
        }

        [Test]
        public void Insert_WithWebExceptionOnResponse_ReturnsFalse()
        {
            // Arrange
            var utility = new SFUtilitiesAdapter();
            var request = BuildRequestWithWebException();
            string requestUrl = null;
            ShimWebRequest.CreateString = u =>
            {
                requestUrl = u;
                return request;
            };

            // Act
            var result = utility.Insert(AccessToken, new object(), ObjectType);

            // Asssert
            result.ShouldBeFalse();
            requestUrl.ShouldBe(_expectedUrl);
            request.ShouldSatisfyAllConditions(
                () => request.Headers[AuthHeader].ShouldBe(_authValue),
                () => request.Method.ShouldBe(HttpPost),
                () => request.ContentType.ShouldBe(ContentType));
        }

        [Test]
        public void GetSingle_PassValidParams_ReturnNotNullEntity()
        {
            // Arrange
            var utility = new SFUtilitiesAdapter();
            var converterMock = new Mock<EntityConverterBase>();
            converterMock.Setup(x => x.Convert<JustForUnitTest>(It.IsAny<IEnumerable<string>>())).Returns(new[] { new JustForUnitTest() });
            var request = BuildRequestWithResult(string.Empty);
            string requestUrl = null;
            ShimWebRequest.CreateString = u =>
            {
                requestUrl = u;
                return request;
            };

            // Act
            var result = utility.GetSingle<JustForUnitTest>(AccessToken, WhereParam, converterMock.Object);

            // Asssert
            result.ShouldNotBeNull();
            requestUrl.ShouldBe(_expectedWhereUrl);
            converterMock.Verify(x => x.Convert<JustForUnitTest>(It.IsAny<IEnumerable<string>>()), Times.Once);
            request.ShouldSatisfyAllConditions(
                () => request.Headers[AuthHeader].ShouldBe(_authValue),
                () => request.Headers[PrettyPrintHeader].ShouldBe(PrettyPrintValue),
                () => request.Method.ShouldBe(HttpGet),
                () => request.ContentType.ToLowerInvariant().ShouldBe(ContentType));
        }

        [Test]
        public void GetSingle_WithWebExceptionOnResponse_ReturnsNull()
        {
            // Arrange
            var utility = new SFUtilitiesAdapter();
            var converterMock = new Mock<EntityConverterBase>();
            converterMock.Setup(x => x.Convert<JustForUnitTest>(It.IsAny<IEnumerable<string>>())).Returns(new[] { new JustForUnitTest() });
            var request = BuildRequestWithWebException();
            string requestUrl = null;
            ShimWebRequest.CreateString = u =>
            {
                requestUrl = u;
                return request;
            };

            // Act
            var result = utility.GetSingle<JustForUnitTest>(AccessToken, WhereParam, converterMock.Object);

            // Asssert
            result.ShouldBeNull();
            requestUrl.ShouldBe(_expectedWhereUrl);
            converterMock.Verify(x => x.Convert<JustForUnitTest>(It.IsAny<IEnumerable<string>>()), Times.Never);
            request.ShouldSatisfyAllConditions(
                () => request.Headers[AuthHeader].ShouldBe(_authValue),
                () => request.Headers[PrettyPrintHeader].ShouldBe(PrettyPrintValue),
                () => request.Method.ShouldBe(HttpGet),
                () => request.ContentType.ToLowerInvariant().ShouldBe(ContentType));
        }

        private WebRequest BuildRequestWithResult(string result)
        {
            var request = BuildRequestMock();
            var response = new Mock<WebResponse>();
            response.Setup(x => x.GetResponseStream()).Returns(GenerateStreamFromString(result));
            request.Setup(x => x.GetResponse()).Returns(response.Object);

            return request.Object;
        }

        private WebRequest BuildRequestWithWebException()
        {
            var request = BuildRequestMock();
            request.Setup(x => x.GetResponse()).Throws(new WebException());

            return request.Object;
        }

        private Mock<WebRequest> BuildRequestMock()
        {
            var request = new Mock<WebRequest>();
            request.SetupAllProperties();
            request.Setup(x => x.GetRequestStream()).Returns(Mock.Of<Stream>());
            request.Setup(x => x.Headers).Returns(new WebHeaderCollection());

            return request;
        }

        private string GetResult(bool isSuccess)
        {
            var result = new SF_Utilities.InsertResult { success = isSuccess };
            return JsonConvert.SerializeObject(result);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private class JustForUnitTest
        {
            public int Id { get; set; }
        }
    }
}
