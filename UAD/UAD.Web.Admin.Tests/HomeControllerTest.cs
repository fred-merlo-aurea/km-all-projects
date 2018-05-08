using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Security;
using System.Web.Security.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.MVCModels;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using UAD.Web.Admin.Controllers;

namespace UAD.Web.Admin.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HomeControllerTest
    {
        private IDisposable _shimObject;
        private HomeController _indexController;
        private string _authenticationUserData;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            var httpContext = MvcMockHelpers.MockHttpContext();
            _indexController = new HomeController();
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
        public void ClientChange_DoesNotSetAuthenticationTicketInCookie(
            int selectedClientGroupId,
            int currentClientGroupId,
            int selectedClientId,
            int currentClientId,
            int selectedProductID,
            int currentProductID,
            bool isAuthorized,
            bool isAdministrator)
        {
            // Arrange
            var clientDropdownModel = new PostModels.Menu.ClientDropDownViewModel()
            {
                SelectedClientGroupID = selectedClientGroupId,
                CurrentClientGroupID = currentClientGroupId,
                SelectedClientID = selectedClientId,
                CurrentClientID = currentClientId,
                SelectedProductID = selectedProductID,
                CurrentProductID = currentProductID,
                AccountChange = string.Empty
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(
                clientDropdownModel.CurrentClientID,
                clientDropdownModel.CurrentClientGroupID,
                expectedAuthenticationTicketHash,
                clientDropdownModel.SelectedClientID,
                isAuthorized,
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
        public void ClientChange_SetsAuthenticationTicketInCookie(
            int selectedClientGroupID,
            int currentClientGroupID,
            int selectedClientID,
            int currentClientID,
            int selectedProductID,
            int currentProductID,
            bool isAuthorized,
            bool isAdministrator)
        {
            // Arrange
            var clientDropdownModel = new PostModels.Menu.ClientDropDownViewModel()
            {
                SelectedClientGroupID = selectedClientGroupID,
                CurrentClientGroupID = currentClientGroupID,
                SelectedClientID = selectedClientID,
                CurrentClientID = currentClientID,
                SelectedProductID = selectedProductID,
                CurrentProductID = currentProductID,
                AccountChange = string.Empty
            };

            var expectedAuthenticationTicketHash = "Authentication ticket hash";
            SetupShimsFor_ClientDropdownTest(
                clientDropdownModel.CurrentClientID,
                clientDropdownModel.CurrentClientGroupID,
                expectedAuthenticationTicketHash,
                clientDropdownModel.SelectedClientID,
                isAuthorized,
                isAdministrator);

            // Act
            _indexController.ClientChange(clientDropdownModel);

            // Assert
            _indexController.Response.ShouldSatisfyAllConditions(
                () => _indexController.Response.Cookies[FormsAuthentication.FormsCookieName].ShouldNotBeNull(),
                () => _indexController.Response.Cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe(expectedAuthenticationTicketHash));
            _authenticationUserData.ShouldBe("-1,,10,20,00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void HasAuthorized_AuthorizedClient_ReturnsTrue()
        {
            // Arrange
            var authorizedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID = authorizedUserId}
                }
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _indexController.HasAuthorized(0, authorizedUserId).ShouldBeTrue();
        }

        [Test]
        public void HasAuthorized_NotAuthorizedClient_ReturnsFalse()
        {
            // Arrange
            var authorizedUserId = 10;
            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User()
            {
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
            };
            ShimECNSession.CurrentSession = () => session;

            // Act and  Assert
            _indexController.HasAuthorized(0, authorizedUserId).ShouldBeFalse();
        }

        public void SetupShimsFor_ClientDropdownTest(
            int currentClientId,
            int currentClientGroupId,
            string expectedHash,
            int selectedClientId,
            bool isUserAuthorized,
            bool isSystemAdministrator)
        {
            ShimBaseChannel.GetAll = () => new List<ECN_Framework_Entities.Accounts.BaseChannel>
            {
                new ECN_Framework_Entities.Accounts.BaseChannel()
                {
                    PlatformClientGroupID = currentClientGroupId
                }
            };

            ShimFormsAuthentication.SetAuthCookieStringBoolean = (_, __) => { };
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = ticket =>
            {
                _authenticationUserData = ticket.UserData;
                return expectedHash;
            };

            KMPlatform.BusinessLogic.Fakes.ShimClient.AllInstances.SelectActiveForClientGroupLiteInt32 =
                        (instance, _) => new List<Client> {
                            new Client()
                            {
                                IsAMS = true,
                                IsActive = true,
                                ClientID = selectedClientId,
                                Products = new List<Product>()
                                {
                                    new Product(){ ProductID = 500, IsCirc = true}
                                }
                            }
                        };

            ShimFormsAuthentication.SignOut = () => { };

            ShimCustomer.GetByClientIDInt32Boolean = (_, __) => { return new ECN_Framework_Entities.Accounts.Customer(); };

            var session = new ShimECNSession();
            session.Instance.CurrentUser = new User();
            session.Instance.CurrentUserClientGroupClients = new List<Client>();
            session.ClientIDGet = () => currentClientId;
            session.ClientGroupIDGet = () => currentClientGroupId;

            KMPlatform.BusinessLogic.Fakes.ShimUser.IsSystemAdministratorUser = (_) => isSystemAdministrator;

            if (isUserAuthorized)
            {
                session.Instance.CurrentUser.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>()
                {
                    new UserClientSecurityGroupMap() { ClientID =  selectedClientId}
                };
            }

            ShimECNSession.CurrentSession = () => session;
        }
    }
}
