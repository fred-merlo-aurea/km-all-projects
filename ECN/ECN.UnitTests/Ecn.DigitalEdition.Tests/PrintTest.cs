using System.Collections.Specialized;
using System.Web;
using ecn.digitaledition;
using ECN.Tests.Helpers;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Ecn.DigitalEdition.Tests
{
    [TestFixture]
    public class PrintTest
    {
        private const string BlastIdQueryStringKey = "b";
        private const string EmailIdQueryStringKey = "e";
        private const string EditionIdQueryStringKey = "eID";
        private const string SessionIdQueryStringKey = "s";

        private const string InvalidInt = "abc";
        private const int NonZero = 1;
        private const int Zero = 0;
        private Mock<HttpRequestBase> _request;
        private print _print;

        [SetUp]
        public void SetUp()
        {
            _request = new Mock<HttpRequestBase>();
            _print = new print(_request.Object);
        }

        [Test]
        public void GetBlastID_ValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(BlastIdQueryStringKey, NonZero.ToString());
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actual = (int)ReflectionHelper.ExecuteMethod(_print, "getBlastID", null);

            // Assert
            actual.ShouldBe(NonZero);
        }

        [Test]
        public void GetBlastID_InValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(BlastIdQueryStringKey, InvalidInt);
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actaul = (int)ReflectionHelper.ExecuteMethod(_print, "getBlastID", null);

            // Assert
            actaul.ShouldBe(Zero);
        }

        [Test]
        public void GetEmailID_ValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(EmailIdQueryStringKey, NonZero.ToString());
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actual = (int)ReflectionHelper.ExecuteMethod(_print, "getEmailID", null);

            // Assert
            actual.ShouldBe(NonZero);
        }

        [Test]
        public void GetEmailID_InValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(EmailIdQueryStringKey, InvalidInt);
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actaul = (int)ReflectionHelper.ExecuteMethod(_print, "getEmailID", null);

            // Assert
            actaul.ShouldBe(Zero);
        }

        [Test]
        public void GetEditionID_ValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(EditionIdQueryStringKey, NonZero.ToString());
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actual = (int)ReflectionHelper.ExecuteMethod(_print, "getEditionID", null);

            // Assert
            actual.ShouldBe(NonZero);
        }

        [Test]
        public void GetEditionID_InValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(EditionIdQueryStringKey, InvalidInt);
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actaul = (int)ReflectionHelper.ExecuteMethod(_print, "getEditionID", null);

            // Assert
            actaul.ShouldBe(Zero);
        }

        [Test]
        public void GetSessionID_ValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(SessionIdQueryStringKey, NonZero.ToString());
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actual = ReflectionHelper.ExecuteMethod(_print, "getSessionID", null) as string;

            // Assert
            actual.ShouldBe(NonZero.ToString());
        }

        [Test]
        public void GetSessionID_InValidIntergerInQueryString_ReturnsNonZero()
        {
            // Arrange
            var queryString = CreateQueryString(SessionIdQueryStringKey, null);
            _request.SetupGet(x => x.QueryString).Returns(queryString);

            // Act
            var actaul = ReflectionHelper.ExecuteMethod(_print, "getSessionID", null) as string;

            // Assert
            actaul.ShouldBe(string.Empty);
        }

        private NameValueCollection CreateQueryString(string name, string value)
        {
            var queryString = new NameValueCollection();
            queryString.Add(name, value);

            return queryString;
        }
    }
}
