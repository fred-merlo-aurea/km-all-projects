using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using KMEntities;
using KMManagers.Fakes;
using KMPlatform.Entity;
using KMSite;
using KMWeb.Controllers;
using KMWeb.Services;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Shouldly;

namespace KMWeb.Tests.Controllers
{
    [TestFixture]
    public partial class FormsControllerTest
    {
        private IDisposable _shimObject;
        private FormsController _formsController;
        private static string _authenticationUserData;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            ShimFormManager.Constructor = manager => { };
            ShimControlManager.Constructor = manager => { };
            ShimNotificationManager.Constructor = manager => { };
            ShimCssFileManager.Constructor = manager => { };
            ShimRuleManager.Constructor = manager => { };
            ShimControlTypeManager.Constructor = manager => { };
            ShimSubscriberLoginManager.Constructor = manager => { };
            ShimAPIRunnerBase.Constructor = apirunnerbase => { };
            ShimApplicationLog.Behavior = ShimBehaviors.DefaultValue;

            var httpContext = MvcMockHelpers.MockHttpContext();

            var userCacheServiceMock = new Mock<IUserCacheService>();
            userCacheServiceMock.Setup(x => x.GetUser())
                .Returns(new User
                {
                    Password = "pass",
                    EmailAddress = "email"
                });

            _formsController = new FormsController(
                new ApplicationManagersFactory(),
                new UserSelfServicing(userCacheServiceMock.Object),
                new KMAuthenticationManager());
            _formsController.SetMockControllerContext(httpContext);            
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void PublicFormSendPassword_NullFormIdAndGroupId_ReturnsNull()
        {
            // Arrange
            var emailAddress = "";
            var other = "";
            int? groupId = null;
            int? formId = null;
            var otherIdentification = "";
            object expectedResult = null;

            // Act	
            var actualResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "not_empty")]
        [TestCase("not_empty", "")]
        public void PublicFormSendPassword_EmptyEmailAddress_ReturnsValidJson(string other, string otherIdentification)
        {
            // Arrange
            var emailAddress = "";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            var expectedResult = new List<string>
            {
                "404",
                "The SubscriberID you entered doesn't exist."
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormSendPassword_EmptyEmailAddressWithDefaultEmailId_ReturnsValidJson()
        {
            // Arrange
            var emailAddress = "";
            var other = "not_empty";
            var otherIdentification = "not_empty";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 0;
            var expectedResult = new List<string>
            {
                "500",
                $"The {otherIdentification} you entered doesn't exist."
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        [TestCase("test1@kmpsgroup.com")]
        [TestCase("Test2@kmpsgroup.com")]
        public void PublicFormSendPassword_InvalidEmailAddress_ReturnsValidJson(string emailAddress)
        {
            // Arrange
            int? groupId = 0;
            int? formId = 0;
            string other = "not_empty";
            string otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            List<string> expectedResult = new List<string> { "404", "Please Enter a Valid Email Address" };

            // Act	
            JsonResult jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormSendPassword_ValidUnsubscribedEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "a"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => null;
            var expectedResult = new List<string>
            {
                "500",
                "Email Address is Unsubscribed"
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormSendPassword_ValidNonExistingEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "s"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => null;
            var expectedResult = new List<string>
            {
                "500",
                "Email Address not found"
            };

            // Act	
            JsonResult jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormSendPassword_ValidExistingEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "s"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => new Email
            {
                EmailAddress = emailAddress,
                Password = "pass"

            };
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (user, i, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new DataTable();
            ShimEmailDirect.SaveEmailDirect = direct => 1;
            ShimSubscriberLoginManager.AllInstances.GetByIDInt32 =
                (manager, i) => new SubscriberLogin
                {
                    ForgotPasswordMessageHTML = "ForgotPasswordNotificationHTML %%Email%%",
                    ForgotPasswordNotificationHTML = ""
                };

            var expectedResult = new List<string> { "200", $"ForgotPasswordNotificationHTML {emailAddress}" };

            // Act	
            var jsonResult =
                _formsController.PublicFormSendPassword(emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_NullFormIdAndGroupId_ReturnsNull()
        {
            // Arrange
            var newEmailAddress = "";
            var emailAddress = "";
            var other = "";
            int? groupId = null;
            int? formId = null;
            var otherIdentification = "";
            object expectedResult = null;

            // Act	
            var actualResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress,
                    other, groupId, formId, otherIdentification);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_EmptyNewEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "";
            var emailAddress = "";
            var other = "";
            var otherIdentification = "";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            var expectedResult = new List<string> { "500", "Email Address cannot be empty." };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other,
                    groupId, formId, otherIdentification) as JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_NonEmptyExistingNewEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "";
            var other = "";
            var otherIdentification = "";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => new Email();
            var expectedResult = new List<string>
            {
                "500",
                "Email Address already exist."
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other,
                    groupId, formId, otherIdentification) as JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "not_empty")]
        [TestCase("not_empty", "")]
        public void PublicFormUpdateProfileAndSendPassword_EmptyEmailAddress_ReturnsValidJson(string other, string otherIdentification)
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => null;
            var expectedResult = new List<string>
            {
                "404",
                "The SubscriberID you entered doesn't exist."
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_EmptyEmailAddressWithDefaultEmailId_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "";
            var other = "not_empty";
            var otherIdentification = "not_empty";
            int? groupId = 0;
            int? formId = 0;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => null;
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 0;
            var expectedResult = new List<string>
            {
                "500",
                $"The {otherIdentification} you entered doesn't exist."
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_ValidUnsubscribedEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => null;
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "a"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => null;
            var expectedResult = new List<string>
            {
                "500",
                "Email Address is Unsubscribed"
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_ValidNonExistingEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => null;
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "s"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => null;
            var expectedResult = new List<string>
            {
                "500",
                "Email Address not found"
            };

            // Act	
            var jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormUpdateProfileAndSendPassword_ValidExistingEmailAddress_ReturnsValidJson()
        {
            // Arrange
            var newEmailAddress = "newEmail@email.com";
            var emailAddress = "email@email.com";
            int? groupId = 1;
            int? formId = 1;
            var other = "not_empty";
            var otherIdentification = "not_empty";
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = i => new Group();
            ShimEmail.GetByEmailAddressStringInt32 = (s, i) => null;
            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
                (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 1;
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (s, i) => new EmailGroup
            {
                SubscribeTypeCode = "s"
            };
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (i, i1) => new Email
            {
                EmailAddress = emailAddress,
                Password = "pass"
            };
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (user, i, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new DataTable();
            ShimEmail.SaveEmail = email => { };
            ShimEmailDirect.SaveEmailDirect = direct => 1;
            ShimSubscriberLoginManager.AllInstances.GetByIDInt32 =
                (manager, i) => new SubscriberLogin
                {
                    ForgotPasswordMessageHTML = "ForgotPasswordNotificationHTML %%Email%%",
                    ForgotPasswordNotificationHTML = ""
                };

            var expectedResult = new List<string>
            {
                "200",
                $"ForgotPasswordNotificationHTML {newEmailAddress}"
            };

            // Act	
            JsonResult jsonResult =
                _formsController.PublicFormUpdateProfileAndSendPassword(newEmailAddress, emailAddress, other, groupId, formId, otherIdentification) as
                    JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        public void PublicFormLogin_EmptyOtherIdentification_ReturnsValidJson()
        {
            // Arrange
            var emailAddress = "email@email.com";
            var other = "not_empty";
            var otherIdentification = "";
            var password = "pass";
            var passRequired = true;
            int? groupId = 1;
            var expectedResult = new List<string> { "404", $"The SubscriberID you entered doesn't exist." };

            // Act	
            var jsonResult =
                _formsController.PublicFormLogin(emailAddress, other, password, passRequired,
                groupId, otherIdentification) as JsonResult;
            var actualResult = jsonResult?.Data as List<string>;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            foreach (var expectedResultItem in expectedResult)
            {
                Assert.Contains(expectedResultItem, actualResult);
            }
        }

        [Test]
        [TestCase("other", "1", 1, "other", "", "", "", "", "", "")]
        [TestCase("other", "user1", 0, "", "other", "", "", "", "", "")]
        [TestCase("other", "user2", 0, "", "", "other", "", "", "", "")]
        [TestCase("other", "user3", 0, "", "", "", "other", "", "", "")]
        [TestCase("other", "user4", 0, "", "", "", "", "other", "", "")]
        [TestCase("other", "user5", 0, "", "", "", "", "", "other", "")]
        [TestCase("other", "user6", 0, "", "", "", "", "", "", "other")]
        public void PublicFormLogin_NonEmptyIdentification_ReturnsValidJson(
            string other, string otherIdentification, int udfId, string udfValue,
            string user1, string user2, string user3, string user4, string user5, string user6)
        {
            // Arrange
            var emailAddress = "email@email.com";
            var password = "pass";
            var passRequired = true;
            var groupId = 1;

            var actualUdfId = 0;
            var actualUdFValue = String.Empty;
            var actualUser1 = String.Empty;
            var actualUser2 = String.Empty;
            var actualUser3 = String.Empty;
            var actualUser4 = String.Empty;
            var actualUser5 = String.Empty;
            var actualUser6 = String.Empty;

            ShimEmailGroup.FDSubscriberLoginInt32StringInt32StringStringStringStringStringStringStringString =
               (i, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
               {
                   actualUdfId = arg3;
                   actualUdFValue = arg4;
                   actualUser1 = arg6;
                   actualUser2 = arg7;
                   actualUser3 = arg8;
                   actualUser4 = arg9;
                   actualUser5 = arg10;
                   actualUser6 = arg11;

                   return 1;
               };

            // Act	
            _formsController.PublicFormLogin(emailAddress, other, password,
                passRequired, groupId, otherIdentification);

            // Assert
            Assert.AreEqual(udfId, actualUdfId);
            Assert.AreEqual(actualUdFValue, udfValue);
            Assert.AreEqual(actualUser1, user1);
            Assert.AreEqual(actualUser2, user2);
            Assert.AreEqual(actualUser3, user3);
            Assert.AreEqual(actualUser4, user4);
            Assert.AreEqual(actualUser5, user5);
            Assert.AreEqual(actualUser6, user6);
        }

        [Test]
        public void HasAuthorised_AuthorisedClient_true()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID = authorisedUserId}
                }
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _formsController.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
        }

        [Test]
        public void HasAuthorised_NotAuthorisedClient_false()
        {
            // Arrange
            var authorisedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _formsController.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }
    }
}
