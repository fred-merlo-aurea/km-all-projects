using System;
using System.Web.Mvc;
using EmailMarketing.Site.Models;
using KMPlatform.Entity;
using KMPlatform.Object;
using NUnit.Framework;
using static KMPlatform.Enums;

namespace EmailMarketing.Site.Tests.Controllers
{
    public partial class LoginControllerTest
    {
        [Test]
        public void Index_ModelInvalid_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = null,
                Persist = true,
                User = null
            };
            var returnUrl = string.Empty;
            _loginController.ModelState.AddModelError("User", "Error");

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
        }

        [Test]
        public void Index_OnInvalidAuthentication_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            var returnUrl = string.Empty;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return null;
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
        }

        [Test]
        public void Index_OnValidAuthenticationReturnURLIsNull_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = false
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(actualResult);
            StringAssert.Contains(actualResult.Url, "main/default.aspx");
        }

        [Test]
        [Ignore("test fails when returnUrl is null, line 154")]
        public void Index_OnValidAuthenticationReturnURLIsEmpty_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            var returnUrl = "";
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = false
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(actualResult);
            StringAssert.Contains(actualResult.Url, "main/default.aspx");
        }

        [Test]
        public void Index_OnValidAuthenticationReturnURLNotNull_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            var returnUrl = "test.url";
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = false
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(actualResult);
            StringAssert.Contains(actualResult.Url, returnUrl);
        }

        [Test]
        public void Index_OnValidAuthenticationReturnURLIsEmailMarketing_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            var returnUrl = "logout";
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = false
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(actualResult);
            StringAssert.Contains(actualResult.Url, "main/default.aspx");
        }

        [Test]
        public void Index_OnValidAuthenticationReturnURLIsLogout_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            var returnUrl = "emailmarketing.site";
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = false
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectResult;

            // Assert
            Assert.IsNotNull(actualResult);
            StringAssert.Contains(actualResult.Url, "main/default.aspx");
        }

        [Test]
        public void Index_OnRequirePasswordReset_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   return new User()
                   {
                       UserID = 1,
                       RequirePasswordReset = true
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.RouteValues.ContainsKey("action"));
            Assert.AreEqual("Index", actualResult.RouteValues["action"]);
            Assert.IsTrue(actualResult.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("Reset", actualResult.RouteValues["controller"]);
            Assert.IsTrue(actualResult.RouteValues.ContainsKey("id"));
            Assert.AreEqual(1, actualResult.RouteValues["id"]);
        }

        [Test]
        public void Index_OnLockedUser_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.LockedUser
                   };
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
            Assert.IsTrue(actualResult.UserIsLocked);
        }

        [Test]
        public void Index_OnDisabledUser_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.DisabledUser
                   };
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
            Assert.IsFalse(actualResult.UserIsLocked);
            Assert.IsTrue(actualResult.UserIsDisabled);
        }

        [Test]
        public void Index_OnNoRoles_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.NoRoles
                   };
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
            Assert.IsFalse(actualResult.UserIsLocked);
            Assert.IsFalse(actualResult.UserIsDisabled);
            Assert.IsTrue(actualResult.NoActiveRoles);
        }

        [Test]
        public void Index_OnInvalidPassword_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.InvalidPassword
                   };
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
            Assert.IsFalse(actualResult.UserIsLocked);
            Assert.IsFalse(actualResult.UserIsDisabled);
            Assert.IsFalse(actualResult.NoActiveRoles);
            Assert.IsTrue(actualResult.InvalidUsername_Password);
        }

        [Test]
        public void Index_OnNoActiveClients_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.NoActiveClients
                   };
               };

            // Act	
            var viewResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;
            var actualResult = viewResult.Model as LoginUserViewModel;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(loginUserViewModel.Password, actualResult.Password);
            Assert.AreEqual(loginUserViewModel.User, actualResult.User);
            Assert.AreEqual(loginUserViewModel.Persist, actualResult.Persist);
            Assert.IsFalse(actualResult.UserIsLocked);
            Assert.IsFalse(actualResult.UserIsDisabled);
            Assert.IsFalse(actualResult.NoActiveRoles);
            Assert.IsFalse(actualResult.InvalidUsername_Password);
            Assert.IsTrue(actualResult.NoActive_Clients);
        }

        [Test]
        public void Index_OnShowProfileChangeMessage_ReturnView()
        {
            // Arrange
            var loginUserViewModel = new LoginUserViewModel
            {
                Password = "Password",
                Persist = true,
                User = "User"
            };
            string returnUrl = null;
            _stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
               (cookie, username, password, presist) =>
               {
                   throw new UserLoginException
                   {
                       UserStatus = UserLoginStatus.ShowProfileChangeMessage
                   };
               };

            // Act	
            var actualResult = _loginController.Index(loginUserViewModel, returnUrl) as ViewResult;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("Message", actualResult.ViewName);
        }
    }
}
