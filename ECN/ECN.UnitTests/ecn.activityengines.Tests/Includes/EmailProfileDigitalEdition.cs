using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.includes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Includes
{
    [TestFixture]
    public class EmailProfileDigitalEditionTest
    {
        private const string GetFromQueryStringMethodName = "GetFromQueryString";
        private const string EmailIdQueryStringKey = "eID";
        private const string EmailId = "100";
        private const string EmailIdErrorMessage = "EmailId specified does not exist.";
        private const string MessageLabelName = "messageLabel";

        private emailProfile_DigitalEdition _emailProfileDigitalEditionObject;
        private PrivateObject _emailProfileDigitalEditionPrivateObject;
        private IDisposable _shimsContext;
        private ShimHttpRequest _shimHttpRequest;
        private HttpRequest _httpRequest;
        private NameValueCollection _queryStringCollection;
        private Label _messageLabel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _shimsContext = ShimsContext.Create();
            ShimUserControl.AllInstances.RequestGet = (userControl) => { return _httpRequest; };
            ShimHttpRequest.AllInstances.QueryStringGet = (request) => { return _queryStringCollection; };

            _shimHttpRequest = new ShimHttpRequest();
            _emailProfileDigitalEditionObject = new emailProfile_DigitalEdition();
            _httpRequest = _shimHttpRequest.Instance;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _shimsContext.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _queryStringCollection = new NameValueCollection();
            _emailProfileDigitalEditionObject = new emailProfile_DigitalEdition();
            _emailProfileDigitalEditionPrivateObject = new PrivateObject(_emailProfileDigitalEditionObject);

            _messageLabel = new Label();
            _emailProfileDigitalEditionPrivateObject.SetField(MessageLabelName, _messageLabel);
        }

        [TearDown]
        public void TearDown()
        {
            _messageLabel.Dispose();
        }

        [Test]
        public void GetFromQueryString_IfEmailIdProvidedInQueryString_ReturnsEmailId()
        {
            // Arrange
            _httpRequest.QueryString[EmailIdQueryStringKey] = EmailId;

            // Act
            var returnedEmailId = (string)_emailProfileDigitalEditionPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedEmailId.ShouldBe(EmailId);
        }

        [Test]
        public void GetFromQueryString_IfExpectedKeyDoesNotExistInQueryString_ReturnsEmptyStringAndSetMessageLabel()
        {
            // Arrange
            // set no query string to get error

            // Act
            var returnedValue = (string)_emailProfileDigitalEditionPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedValue.ShouldBeEmpty();
            _messageLabel.ShouldSatisfyAllConditions(
                () => _messageLabel.Visible.ShouldBeTrue(),
                () => _messageLabel.Text.ShouldEndWith(EmailIdErrorMessage));
        }
    }
}