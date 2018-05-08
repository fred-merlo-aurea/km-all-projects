using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.Security;
using System.Web.Security.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.Entity;
using KMWeb.Controllers;
using KMWeb.Models.Forms;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMWeb.Tests.Controllers
{
    [TestFixture]
    public partial class ErrorControllerTest
    {
        private IDisposable _shimObject;
        private ErrorController _errorsController;
        private static string _authenticationUserData;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();

            _errorsController = new ErrorController();
            _errorsController.SetMockControllerContext(httpContext);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
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
            _errorsController.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
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
            _errorsController.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }

    }
}
