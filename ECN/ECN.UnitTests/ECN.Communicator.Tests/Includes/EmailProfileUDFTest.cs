using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.includes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Includes
{
    [TestFixture]
    public class EmailProfileUDFTest
    {
        private const string GetFromQueryStringMethodName = "GetFromQueryString";
        private const string EmailIdQueryStringKey = "eID";
        private const string EmailId = "100";
        private const string EmailIdErrorMessage = "EmailId specified does not exist.";
        private const string GroupIdQueryStringKey = "gID";
        private const string GroupId = "200";
        private const string GroupIdErrorMessage = "GroupId specified does not exist.";
        private const string CustomerIdQueryStringKey = "cID";
        private const string CustomerId = "300";
        private const string CustomerIdErrorMessage = "CustomerId specified does not exist.";
        private const string MessageLabelName = "MessageLabel";

        private emailProfile_UDF _emailProfileUDFObject;
        private PrivateObject _emailProfileUDFPrivateObject;
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
            _emailProfileUDFObject = new emailProfile_UDF();
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
            _emailProfileUDFObject = new emailProfile_UDF();
            _emailProfileUDFPrivateObject = new PrivateObject(_emailProfileUDFObject);

            _messageLabel = new Label();
            _emailProfileUDFPrivateObject.SetField(MessageLabelName, _messageLabel);
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
            var returnedEmailId = (string)_emailProfileUDFPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedEmailId.ShouldBe(EmailId);
        }

        [Test]
        public void GetFromQueryString_IfCustomerIdProvidedInQueryString_ReturnsCustomerId()
        {
            // Arrange
            _httpRequest.QueryString[CustomerIdQueryStringKey] = CustomerId;

            // Act
            var returnedCustomerId = (string)_emailProfileUDFPrivateObject.Invoke(GetFromQueryStringMethodName, CustomerIdQueryStringKey, CustomerIdErrorMessage);

            // Assert
            returnedCustomerId.ShouldBe(CustomerId);
        }

        [Test]
        public void GetFromQueryString_IfGroupIdProvidedInQueryString_ReturnsGroupId()
        {
            // Arrange
            _httpRequest.QueryString[GroupIdQueryStringKey] = GroupId;

            // Act
            var returnedGroupId = (string)_emailProfileUDFPrivateObject.Invoke(GetFromQueryStringMethodName, GroupIdQueryStringKey, GroupIdErrorMessage);

            // Assert
            returnedGroupId.ShouldBe(GroupId);
        }

        [Test]
        public void GetFromQueryString_IfExpectedKeyDoesNotExistInQueryString_ReturnsEmptyStringAndSetMessageLabel()
        {
            // Arrange
            // set no query string to get error

            // Act
            var returnedValue = (string)_emailProfileUDFPrivateObject.Invoke(GetFromQueryStringMethodName, EmailIdQueryStringKey, EmailIdErrorMessage);

            // Assert
            returnedValue.ShouldBeEmpty();
            _messageLabel.ShouldSatisfyAllConditions(
                () => _messageLabel.Visible.ShouldBeTrue(),
                () => _messageLabel.Text.ShouldEndWith(EmailIdErrorMessage));
        }
    }
}