using System;
using System.Web.Mvc;
using EmailMarketing.Site.Models;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;

namespace EmailMarketing.Site.Tests.Controllers
{
    public partial class ResetControllerTest
    {
        [Test]
        public void Index_OnInvalidTempPassword_ReturnView()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = "Password",
                NewPassword = "Password",
                TempPassword = "TempPassword",
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = "Password"
                };
            };

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeTrue();
        }

        [Test]
        public void Index_OnInvalidPasswordCompare_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password1";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeTrue();
        }

        [Test]
        public void Index_OnLockedUser_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    throw new UserLoginException
                    {
                        UserStatus = UserLoginStatus.LockedUser
                    };
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeFalse();
            actualResult.UserIsLocked.ShouldBeTrue();
        }

        [Test]
        public void Index_OnDisabledUser_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    throw new UserLoginException
                    {
                        UserStatus = UserLoginStatus.DisabledUser
                    };
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeFalse();
            actualResult.UserIsLocked.ShouldBeFalse();
            actualResult.UserIsDisabled.ShouldBeTrue();
        }

        [Test]
        public void Index_OnNoRoles_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    throw new UserLoginException
                    {
                        UserStatus = UserLoginStatus.NoRoles
                    };
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeFalse();
            actualResult.UserIsLocked.ShouldBeFalse();
            actualResult.UserIsDisabled.ShouldBeFalse();
            actualResult.NoActiveRoles.ShouldBeTrue();
        }

        [Test]
        public void Index_OnInvalidPassword_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    throw new UserLoginException
                    {
                        UserStatus = UserLoginStatus.InvalidPassword
                    };
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeFalse();
            actualResult.UserIsLocked.ShouldBeFalse();
            actualResult.UserIsDisabled.ShouldBeFalse();
            actualResult.NoActiveRoles.ShouldBeFalse();
            actualResult.InvalidUsername_Password.ShouldBeTrue();
        }

        [Test]
        public void Index_OnValidAuthentication_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var viewName = "ResetSuccess";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    return new User()
                    {
                        UserID = 1,
                        Password = tempPassword
                    };
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            viewResult.ViewName.ShouldBe(viewName);
            actualResult.InvalidTemp_Password.ShouldBeFalse();
            actualResult.PasswordCompare.ShouldBeFalse();
            actualResult.UserIsLocked.ShouldBeFalse();
            actualResult.UserIsDisabled.ShouldBeFalse();
            actualResult.NoActiveRoles.ShouldBeFalse();
            actualResult.InvalidUsername_Password.ShouldBeFalse();
            actualResult.UserID.ShouldBe(resetPasswordViewModel.UserID);
        }

        [Test]
        public void Index_OnInvalidAuthentication_ReturnView()
        {
            // Arrange
            var tempPassword = "TempPassword";
            var confirmPassword = "Password";
            var newPassword = "Password";
            var viewName = "";
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                ConfirmPassword = confirmPassword,
                NewPassword = newPassword,
                TempPassword = tempPassword,
                UserID = 1
            };
            ShimUser.GetByUserIDInt32Boolean = (id, child) =>
            {
                return new User()
                {
                    UserID = 1,
                    Password = tempPassword
                };
            };
            stubIAuthenticationProvider.AuthenticateActionOfHttpCookieStringStringBoolean =
                (cookie, username, password, presist) =>
                {
                    return null;
                };
            ShimUser.AllInstances.SaveUser = (obj, user) => 1;

            // Act	
            var viewResult = resetController.Index(resetPasswordViewModel) as ViewResult;
            var actualResult = viewResult.Model as ResetPasswordViewModel;

            // Assert
            viewResult.ViewName.ShouldBe(viewName);
            actualResult.UserID.ShouldBe(resetPasswordViewModel.UserID);
        }
    }
}
