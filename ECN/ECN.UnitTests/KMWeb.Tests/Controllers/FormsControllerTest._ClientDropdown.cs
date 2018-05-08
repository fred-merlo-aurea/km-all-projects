using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.Security;
using System.Web.Security.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMWeb.Models.Forms;
using NUnit.Framework;
using Shouldly;

namespace KMWeb.Tests.Controllers
{
    partial class FormsControllerTest
    {
        const string VirtualPathConfigurationKey = "Forms_VirtualPath";

        [Test]
        public void _ClientDropDown_AuthorisedUserClientGroupChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 3,
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } }
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => true;

            // Act
            _formsController._ClientDropDown(clientDropdownModel);

            // Assert
            _formsController.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,3,10,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_AuthorisedUserClientChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, true, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _formsController._ClientDropDown(clientDropdownModel);

            // Assert
            _formsController.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void _ClientDropDown_UnAuthorisedUser_DoesNotSet_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, false, false);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _formsController._ClientDropDown(clientDropdownModel);

            // Assert
            _formsController.Response.Cookies[FormsAuthentication.FormsCookieName].ShouldBeNull();
        }

        [Test]
        public void _ClientDropDown_SystemAdministratorClientChange_sets_AuthenticationTicketInCookie()
        {
            // Arrange
            const int clientGroupId = 5;
            var clientDropdownModel = new ClientDropDown()
            {
                ClientGroups = new List<ClientGroup>() { new ClientGroup() { ClientGroupID = clientGroupId } },
                SelectedClientID = 4,
                CurrentClientID = 6,
                CurrentClientGroupID = 99,
                SelectedClientGroupID = 99
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(clientGroupId, expectedAuthenticationTicketHash, false, true);
            ShimUser.IsChannelAdministratorUser = (_) => false;

            // Act
            _formsController._ClientDropDown(clientDropdownModel);

            // Assert
            _formsController.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,5,20,00000000-0000-0000-0000-000000000000");
        }

        private static void SetupShimsFor_ClientDropdownTest(
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

            ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                (instance, _) => new List<Client> { new Client() { ClientID = clientId } };

            ShimFormsAuthentication.SetAuthCookieStringBoolean = (_, __) => { };
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = t =>
            {
                _authenticationUserData = t.UserData;
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
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection() { { VirtualPathConfigurationKey, string.Empty } };
        }
    }
}
