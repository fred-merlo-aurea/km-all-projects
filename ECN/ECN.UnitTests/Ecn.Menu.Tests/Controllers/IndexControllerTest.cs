using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Web.Security;
using System.Web.Security.Fakes;
using ecn.menu.Controllers;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.MVCModels;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace Ecn.Menu.Tests.Controllers
{
    [TestFixture]
    public class IndexControllerTest
    {
        private IDisposable _shimObject;
        private IndexController _indexController;
        private static string _authenticationUserData;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();
            _indexController = new IndexController();
            _indexController.SetMockControllerContext(httpContext);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [TestCase(10, 11, 20, 20, 30, 30, false, false)]
        [TestCase(10, 10, 20, 21, 30, 30, false, false)]
        [TestCase(10, 10, 20, 20, 30, 31, false, false)]
        [TestCase(10, 10, 20, 20, 30, 30, true, true)]
        public void ClientChange_does_not_set_AuthenticationTicketInCookie(
            int SelectedClientGroupID,
            int CurrentClientGroupID,
            int SelectedClientID,
            int CurrentClientID,
            int SelectedProductID,
            int CurrentProductID,
            bool isAuthorised,
            bool isAdministrator)
        {
            // Arrange
            var clientDropdownModel = new PostModels.Menu.ClientDropDownViewModel()
            {
                SelectedClientGroupID = SelectedClientGroupID,
                CurrentClientGroupID = CurrentClientGroupID,
                SelectedClientID = SelectedClientID,
                CurrentClientID = CurrentClientID,
                SelectedProductID = SelectedProductID,
                CurrentProductID = CurrentProductID,
                AccountChange = string.Empty
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(
                clientDropdownModel.CurrentClientID,
                clientDropdownModel.CurrentClientGroupID,
                expectedAuthenticationTicketHash,
                clientDropdownModel.SelectedClientID,
                isAuthorised,
                isAdministrator);

            // Act
            _indexController.ClientChange(clientDropdownModel);

            // Assert
            _indexController.Response.Cookies[FormsAuthentication.FormsCookieName].ShouldBeNull();
        }

        [TestCase(10, 11, 20, 20, 30, 30, true, false)]
        [TestCase(10, 10, 20, 21, 30, 30, true, false)]
        [TestCase(10, 10, 20, 20, 30, 31, true, false)]
        [TestCase(10, 11, 20, 20, 30, 30, false, true)]
        [TestCase(10, 10, 20, 21, 30, 30, false, true)]
        [TestCase(10, 10, 20, 20, 30, 31, false, true)]
        public void ClientChange_sets_AuthenticationTicketInCookie(
            int SelectedClientGroupID,
            int CurrentClientGroupID,
            int SelectedClientID,
            int CurrentClientID,
            int SelectedProductID,
            int CurrentProductID,
            bool isAuthorised,
            bool isAdministrator)
        {
            // Arrange
            var clientDropdownModel = new PostModels.Menu.ClientDropDownViewModel()
            {
                SelectedClientGroupID = SelectedClientGroupID,
                CurrentClientGroupID = CurrentClientGroupID,
                SelectedClientID = SelectedClientID,
                CurrentClientID = CurrentClientID,
                SelectedProductID = SelectedProductID,
                CurrentProductID = CurrentProductID,
                AccountChange = string.Empty
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(
                clientDropdownModel.CurrentClientID,
                clientDropdownModel.CurrentClientGroupID,
                expectedAuthenticationTicketHash,
                clientDropdownModel.SelectedClientID,
                isAuthorised,
                isAdministrator);

            // Act
            _indexController.ClientChange(clientDropdownModel);

            // Assert
            _indexController.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash);
            _authenticationUserData.ShouldBe("-1,,10,20,00000000-0000-0000-0000-000000000000,30");
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
            _indexController.HasAuthorized(0, authorisedUserId).ShouldBeTrue();
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
            _indexController.HasAuthorized(0, authorisedUserId).ShouldBeFalse();
        }

        public static void SetupShimsFor_ClientDropdownTest(
            int currentClientId,
            int currentClientGroupId,
            string expectedHash,
            int selectedClientId,
            bool isUserAuthorised,
            bool isSystemAdministrator)
        {
            ShimBaseChannel.GetAll = () => new List<ECN_Framework_Entities.Accounts.BaseChannel>
            {
                new ECN_Framework_Entities.Accounts.BaseChannel()
                {
                    PlatformClientGroupID = currentClientGroupId
                }
            };

            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                (instance, _) => new List<Client> { new Client() { ClientID = currentClientId } };

            ShimFormsAuthentication.SetAuthCookieStringBoolean = (_, __) => { };
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = t =>
            {
                _authenticationUserData = t.UserData;
                return expectedHash;
            };

            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                        (instance, _) => new List<Client> { new Client() };

            ShimFormsAuthentication.SignOut = () => { };

            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => { return new ECN_Framework_Entities.Accounts.Customer(); };

            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User();
            session.Instance.CurrentUserClientGroupClients = new List<Client>();
            session.ClientIDGet = () => currentClientId;
            session.ClientGroupIDGet = () => currentClientGroupId;

            KMPlatform.BusinessLogic.Fakes.ShimUser.IsSystemAdministratorUser = (_) => isSystemAdministrator;

            if (isUserAuthorised)
            {
                session.Instance.CurrentUser.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID =  selectedClientId}
                };
            }

            ShimECNSession.CurrentSession = () => session;
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection() { { "Forms_VirtualPath", string.Empty } };
        }
    }
}
