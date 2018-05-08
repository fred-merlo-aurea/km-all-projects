using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class PersonalizationContentControllerTest
    {
        private PersonalizationContentController _testObject;
        private PrivateObject _pTestObject;

        [Test]
        public void ValidateAndInsertContent_Success()
        {
            // Arrange
            CreateController();
            var model = CreateModel();
            InitilizeFakes();

            // Act
            _pTestObject.Invoke("ValidateAndInsertContent", new object[] { model, 1 });
            var resultsDictionary = _pTestObject.GetField("resultsDictionary") as ConcurrentDictionary<string,int>;

            // Assert
            resultsDictionary["Created"].ShouldBe(1);
        }

        [Test]
        public void ValidateAndInsertContent_EmptyModelException()
        {
            // Arrange
            CreateController();
            var model = new PersonalizationEmail();
            InitilizeFakes();

            // Act
            _pTestObject.Invoke("ValidateAndInsertContent", new object[] { model, 1 });
            var ccDictionary = _pTestObject.GetField("ccDictionary") as ConcurrentDictionary<string, HashSet<PersonalizationContentErrorCodes>>;

            // Assert
            ccDictionary.ShouldSatisfyAllConditions(
                () => ccDictionary[""].Any(x => x.ErrorCode == 2).ShouldBeTrue(),
                () => ccDictionary[""].Any(x => x.ErrorCode == 3).ShouldBeTrue(),
                () => ccDictionary[""].Any(x => x.ErrorCode == 4).ShouldBeTrue(),
                () => ccDictionary[""].Any(x => x.ErrorCode == 5).ShouldBeTrue()
            );
        }

        [Test]
        public void ValidateAndInsertContent_InvalidEmail_HtmlFormat_Exceptions()
        {
            // Arrange
            CreateController();
            var model = CreateModel();
            model.HTMLContent = "<a href ='' /><base/><head><head></body><body><body><></html><html><html>";
            InitilizeFakes();
            ShimEmail.IsValidEmailAddressString = (email) => false;

            // Act
            _pTestObject.Invoke("ValidateAndInsertContent", new object[] { model, 1 });
            var ccDictionary = _pTestObject.GetField("ccDictionary") as ConcurrentDictionary<string, HashSet<PersonalizationContentErrorCodes>>;

            // Assert
            ccDictionary.ShouldSatisfyAllConditions(
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 6).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 7).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 8).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 9).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 10).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 12).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 13).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 14).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 15).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 16).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 17).ShouldBeTrue()
            );
        }

        [Test]
        public void ValidateAndInsertContent_InvalidSubject_ForbiddenValuesExceptions()
        {
            // Arrange
            CreateController();
            var model = CreateModel();
            model.EmailSubject = "subject\rsubject";
            model.HTMLContent = "</head>ECN.RSSFEED%%publicview%%ecn_id=ECN.DynamicTag.";
            InitilizeFakes();
            ShimEmail.IsValidEmailAddressString = (email) => false;

            // Act
            _pTestObject.Invoke("ValidateAndInsertContent", new object[] { model, 1 });
            var ccDictionary = _pTestObject.GetField("ccDictionary") as ConcurrentDictionary<string, HashSet<PersonalizationContentErrorCodes>>;

            // Assert
            ccDictionary.ShouldSatisfyAllConditions(
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 11).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 18).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 19).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 20).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 21).ShouldBeTrue(),
                () => ccDictionary["test@km.com"].Any(x => x.ErrorCode == 22).ShouldBeTrue()
            );
        }

        private PersonalizationContentErrorCodes GetErrorCode(int code)
        {
            return new PersonalizationContentErrorCodes() { ErrorCode = code, ErrorMessage = code.ToString() };
        }

        private void CreateController()
        {
            var _testObject = new PersonalizationContentController();
            _pTestObject = new PrivateObject(_testObject);

            var globalErrorCodeList = new HashSet<PersonalizationContentErrorCodes>();
            for (int i = 1; i <= 22; i++)
            {
                globalErrorCodeList.Add(GetErrorCode(i));
            }
            _pTestObject.SetField("globalErrorCodeList", globalErrorCodeList);

            _testObject.Request = new HttpRequestMessage();
        }

        private PersonalizationEmail CreateModel()
        {
            return new PersonalizationEmail
            {
                EmailAddress = "test@km.com",
                TextContent = "content",
                EmailSubject = "subject",
                HTMLContent = "<body></body>"
            };
        }

        private void InitilizeFakes()
        {
            ShimEmail.IsValidEmailAddressString = (email) => true;
            ECN_Framework_BusinessLayer.Content.Fakes.ShimPersonalizedContent.SavePersonalizedContentUser = (content, user) => 0;
            ShimHttpRequestMessage.AllInstances.PropertiesGet = (instance) =>
            new Dictionary<string, object>
            {
                { Strings.Headers.APIAccessKeyHeader, string.Empty},
                { Strings.Properties.APIUserStashKey, new KMPlatform.Entity.User { } }
            };
        }
    }
}
