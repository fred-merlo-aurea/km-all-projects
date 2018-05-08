using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.Security;
using System.Web.Security.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using MVC.Standards.Controllers;
using MVC.Standards.Models;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;

namespace MVC.Standards.Test.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HomeControllerTest
    {
        private IDisposable _shimObject;
        private HomeController _controller;
        private string _authenticationUserData;
        const int ClientGroupId = 5;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();

            _controller = new HomeController();
            _controller.SetMockControllerContext(httpContext);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void _ClientDropDown_AuthorisedUserClientGroupChange_SetsAuthenticationTicketInCookie()
        {
            // Arrange
            var clientDropdownModel = new ClientDropDown()
            {
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 3,
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = ClientGroupId } }
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(ClientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => true;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,3,10,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_AuthorisedUserClientChange_SetsAuthenticationTicketInCookie()
        {
            // Arrange
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = ClientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(ClientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_UnAuthorisedUser_DoesNotSetAuthenticationTicketInCookie()
        {
            // Arrange
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = ClientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(ClientGroupId, expectedAuthenticationTicketHash, false, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].ShouldBeNull();
        }

        [Test]
        public void _ClientDropDown_SystemAdministratorClientChange_SetsAuthenticationTicketInCookie()
        {
            // Arrange
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = ClientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(ClientGroupId, expectedAuthenticationTicketHash, false, true);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _controller._ClientDropDown(clientDropdownModel);

            // Assert
            _controller.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void HasAuthorized_AuthorizedClient_ReturnsTrue()
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
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
        }

        [Test]
        public void HasAuthorized_AuthorizedClient_ReturnsFalse()
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
            _controller.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }

        private void SetupShimsFor_ClientDropdownTest(
            int clientGroupId,
            string expectedHash,
            bool isUserAuthorised,
            bool isSysytemAdministrator)
        {
            const int clientId = 10;
            ShimBaseChannel.GetAll = () => new List<ECN_Framework_Entities.Accounts.BaseChannel>
            {
                new ECN_Framework_Entities.Accounts.BaseChannel()
                {
                    PlatformClientGroupID = clientGroupId
                }
            };

            ShimClientGroup.AllInstances.SelectForAMSBoolean = (_, __) =>
            {
                return new List<ClientGroup>();
            };

            ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                (instance, _) => new List<Client> { new Client() { ClientID = clientId, IsAMS = true } };

            ShimFormsAuthentication.SetAuthCookieStringBoolean = (_, __) => { };
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = ticket =>
            {
                _authenticationUserData = ticket.UserData;
                return expectedHash;
            };

            ShimFormsAuthentication.SignOut = () => { };

            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => { return new ECN_Framework_Entities.Accounts.Customer(); };

            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User();
            session.Instance.CurrentUserClientGroupClients = new List<Client>();
            session.ClientIDGet = () => 20;
            session.ClientGroupIDGet = () => clientGroupId;

            ShimUser.IsSystemAdministratorUser = (_) => isSysytemAdministrator;
            if (isUserAuthorised)
            {
                session.Instance.CurrentUser.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID =  clientId},
                    new UserClientSecurityGroupMap() { ClientID =  session.Instance.ClientID},
                };
            }

            ShimECNSession.CurrentSession = () => session;
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection() { { "UAS_VirtualPath", string.Empty } };
        }
    }
}
