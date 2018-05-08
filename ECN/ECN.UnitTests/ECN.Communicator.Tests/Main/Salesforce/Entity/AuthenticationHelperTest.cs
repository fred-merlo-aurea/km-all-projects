using Moq;
using NUnit.Framework;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AuthenticationHelperTest : SalesForceTestBase
    {
        private const string Endpoint = "endpoint";
        private const string ConsumerKey = "consumer";
        private const string ConsumerSecret = "secret";
        private const string RedirectUrl = "redirect";
        private const string AuthCode = "auth";
        private const string RefreshToken = "refresh";
        private const string HttpPost = "POST";
        private const string ContentType = "application/x-www-form-urlencoded";

        [Test]
        public void GetTokenUrl_ReturnsValidUrl()
        {
            // Act
            var result = AuthenticationHelper.GetTokenUrl(Endpoint, ConsumerKey, ConsumerSecret, RedirectUrl, AuthCode);

            // Assert
            result.ShouldBe("endpoint?grant_type=authorization_code&client_id=consumer&client_secret=secret&redirect_uri=redirect&code=auth");
        }

        [Test]
        public void GetRefreshTokenUrl_ReturnsValidUrl()
        {
            // Act
            var result = AuthenticationHelper.GetRefreshTokenUrl(Endpoint, RefreshToken, ConsumerKey, ConsumerSecret);

            // Assert
            result.ShouldBe("endpoint?grant_type=refresh_token&refresh_token=refresh&client_id=consumer&client_secret=secret&format=json");
        }

        [Test]
        public void GetLoginUrl_ReturnsValidUrl()
        {
            // Act
            var result = AuthenticationHelper.GetLoginUrl(Endpoint, ConsumerKey, RedirectUrl);

            // Assert
            result.ShouldBe("endpoint?response_type=code&client_id=consumer&redirect_uri=redirect&display=popup-Compact");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetToken_PassEmptyAuthecationCode_ReturnsNull(string authecationCode)
        {
            // Act
            var result = AuthenticationHelper.GetToken<SF_Token>(Endpoint, ConsumerKey, ConsumerSecret, RedirectUrl, authecationCode);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GetToken_PassNotNullAuthecationCode_ReturnNotNullToken()
        {
            // Arrange
            var expectedToken = new SF_Token();
            var utility = BuildWithToken(expectedToken);
            AuthenticationHelper.InitUtilities(utility.Object);

            // Act
            var result = AuthenticationHelper.GetToken<SF_Token>(Endpoint, ConsumerKey, ConsumerSecret, RedirectUrl, AuthCode);

            // Assert
            result.ShouldBe(expectedToken);
            utility.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
            utility.Verify(x => x.ReadToken<SF_Token>(
                It.Is<WebRequest>(
                    r => r.Method == HttpPost &&
                    r.ContentType == ContentType)),
                    Times.Once);
        }

        [Test]
        public void GetToken_ExceptionOnReadToken_ReturnNull()
        {
            // Arrange
            var utility = BuildWithException();
            AuthenticationHelper.InitUtilities(utility.Object);

            // Act
            var result = AuthenticationHelper.GetToken<SF_Token>(Endpoint, ConsumerKey, ConsumerSecret, RedirectUrl, AuthCode);

            // Assert
            result.ShouldBeNull();
            utility.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetRefreshToken_PassNotNullAuthecationCode_ReturnNotNullToken()
        {
            // Arrange
            var expectedToken = new SF_Token();
            var utility = BuildWithToken(expectedToken);
            AuthenticationHelper.InitUtilities(utility.Object);

            // Act
            var result = AuthenticationHelper.GetRefreshToken<SF_Token>(Endpoint, RefreshToken, ConsumerKey, ConsumerSecret);

            // Assert
            result.ShouldBe(expectedToken);
            utility.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
            utility.Verify(x => x.ReadToken<SF_Token>(
                It.Is<WebRequest>(
                    r => r.Method == HttpPost &&
                    r.ContentType == ContentType)),
                    Times.Once);
        }

        [Test]
        public void GetRefreshToken_ExceptionOnReadToken_ReturnNull()
        {
            // Arrange
            var utility = BuildWithException();
            AuthenticationHelper.InitUtilities(utility.Object);

            // Act
            var result = AuthenticationHelper.GetRefreshToken<SF_Token>(Endpoint, RefreshToken, ConsumerKey, ConsumerSecret);

            // Assert
            result.ShouldBeNull();
            utility.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Login_PassValidParameters_ReturnUrlToRedirect()
        {
            // Arrange
            const string redirectUrl = "http://redirect/";
            var utility = new Mock<ISFUtilities>();
            var request = new Mock<WebRequest>();
            var response = new Mock<WebResponse>();
            response.Setup(x => x.ResponseUri).Returns(new System.Uri(redirectUrl));
            request.Setup(x => x.GetResponse()).Returns(response.Object);
            utility.Setup(x => x.CreateWebRequest(It.IsAny<string>())).Returns(request.Object);
            AuthenticationHelper.InitUtilities(utility.Object);
            
            // Act
            var result = AuthenticationHelper.Login(Endpoint, ConsumerKey, redirectUrl);

            // Assert
            result.ShouldBe(redirectUrl);
        }

        [Test]
        public void Login_WebExceptionOnResponse_LogException()
        {
            // Arrange
            var utility = new Mock<ISFUtilities>();
            var request = new Mock<WebRequest>();
            request.Setup(x => x.GetResponse()).Throws(new WebException());
            utility.Setup(x => x.CreateWebRequest(It.IsAny<string>())).Returns(request.Object);
            AuthenticationHelper.InitUtilities(utility.Object);

            // Act
            var result = AuthenticationHelper.Login(Endpoint, ConsumerKey, RedirectUrl);

            // Assert
            result.ShouldBeNull();
            utility.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Once);
        }

        private Mock<ISFUtilities> BuildWithToken(SF_Token token)
        {
            var utility = new Mock<ISFUtilities>();
            var request = new Mock<WebRequest>();
            request.SetupProperty(x => x.Method);
            request.SetupProperty(x => x.ContentType);
            utility.Setup(x => x.ReadToken<SF_Token>(It.IsAny<WebRequest>())).Returns(token);
            utility.Setup(x => x.CreateWebRequest(It.IsAny<string>())).Returns(request.Object);
            return utility;
        }

        private Mock<ISFUtilities> BuildWithException()
        {
            var utility = new Mock<ISFUtilities>();
            var request = new Mock<WebRequest>();
            utility.Setup(x => x.ReadToken<SF_Token>(It.IsAny<WebRequest>())).Throws(new WebException());
            utility.Setup(x => x.CreateWebRequest(It.IsAny<string>())).Returns(request.Object);
            return utility;
        }
    }
}


