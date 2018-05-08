using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ecn.activity.Areas.User.Controllers;
using ecn.activity.Areas.User.Models;
using ECN.TestHelpers;
using KMPlatform;
using NUnit.Framework;
using Shouldly;
using Entity = KMPlatform.Entity;

namespace ecn.activity.Tests.Areas.User.Controllers
{
    /// <summary>
    ///     Unit Tests for <see cref="IndexController.Accept(UserAcceptModel)"/>
    /// </summary>
    public partial class IndexControllerTest
    {
        [Test]
        public void Accept_WhenCalledWithNewUser_SaveAndVerifyNewUser()
        {
            // Arrange
            SetUserAcceptModel();

            _userClientSg.IsActive = true;
            _userClientSg.InactiveReason = string.Empty;
            _userClientSg.DateUpdated = DateTime.Now;
            _userClientSg.UpdatedByUserID = UserId;

            var expectedViewName = "~/Areas/User/Views/Main/ThankYou.cshtml";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            string loginUrl = result.ViewBag.LoginURL;

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => loginUrl.ShouldBe(KmPlatformLogin));
        }

        [Test]
        public void Accept_WhenCalledWithExistingUserWithDefaultPassword_SaveAndVerifyNewUser()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.IsExistingUser = true;
            _returnOldUser = true;
            _userId = -1;

            _currentUser = new Entity.User
            {
                EmailAddress = _oldUser.EmailAddress,
                FirstName = _oldUser.FirstName,
                LastName = _oldUser.LastName,
                CreatedByUserID = _oldUser.CreatedByUserID,
                IsPlatformAdministrator = _oldUser.IsPlatformAdministrator,
                Phone = _oldUser.Phone,
                IsAccessKeyValid = true,
                UserName = _userAcceptModel.UserName,
                Password = _userAcceptModel.Password,
                Status = Enums.UserStatus.Active,
                IsActive = true,
                DefaultClientGroupID = _clientGrpMap.ClientGroupID,
                DefaultClientID = _clientGrpMap.ClientID
            };

            _userClientSg.UserID = UserId;
            _userClientSg.IsActive = true;
            _userClientSg.InactiveReason = string.Empty;

            var expectedViewName = "~/Areas/User/Views/Main/ThankYou.cshtml";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            string loginUrl = result.ViewBag.LoginURL;

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => loginUrl.ShouldBe(KmPlatformLogin));
        }

        [Test]
        public void Accept_WhenCalledWithExistingUserAndUserNameAlreadyExist_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.IsExistingUser = true;
            _validateUser = true;
            _userId = -1;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "Username already exists";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithNewUserWithRoleAlreadyExist_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _roleExist = true;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage =
                "A role already exists for this username/customer combination. Please contact your administrator for assistance.";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithNewUserAndUserNameAlreadyExist_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _validateUser = true;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "Username already exists";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithNewUserWithInvalidUserName_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.UserName = "user-name  ";

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "Invalid format. Please remove any leading and trailing white spaces from Username.";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithExistingUser_DeleteAndSaveExistingUser()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.NewUser = false;

            _currentUser.Status = Enums.UserStatus.Active;

            _securityGroupsToAccept.UserID = UserId;
            _securityGroupsToAccept.HasAccepted = true;

            _userClientSg.IsActive = true;
            _userClientSg.InactiveReason = string.Empty;

            var expectedViewName = "~/Areas/User/Views/Main/ThankYou.cshtml";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            string loginUrl = result.ViewBag.LoginURL;

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => loginUrl.ShouldBe(KmPlatformLogin));
        }

        [Test]
        public void Accept_WhenCalledWithExistingUserRoleAlreadyExist_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.NewUser = false;

            _currentUser.Status = Enums.UserStatus.Active;
            _roleExist = true;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage =
                "A role already exists for this username/customer combination. Please contact your administrator for assistance.";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithUserUnderSystemRole_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.NewUser = false;

            _currentUser.Status = Enums.UserStatus.Active;
            _currentUser.IsPlatformAdministrator = true;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "User is a System Administrator and cannot have roles assigned to them.";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;
            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledWithLockedUser_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.NewUser = false;

            _currentUser.Status = Enums.UserStatus.Locked;

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "User is locked and cannot have roles assigned to them.";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        [Test]
        public void Accept_WhenCalledUserWithInvalidPassword_ReturnAcceptViewWithErrorMessage()
        {
            // Arrange
            SetUserAcceptModel();
            _userAcceptModel.NewUser = false;

            _currentUser.Status = Enums.UserStatus.Active;
            _currentUser.Password = "invalid-password";

            var expectedViewName = "~/Areas/User/Views/Main/Accept.cshtml";
            var errorMessage = "Username/password is invalid";

            // Act
            ViewResult result = _controller.Accept(_userAcceptModel) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            UserAcceptModel actual = GetAcceptModel(result.Model);

            result.ShouldSatisfyAllConditions(
                () => result.ViewName.ShouldBe(expectedViewName),
                () => actual.ErrorMessage.ShouldBe(errorMessage));
        }

        private void SetUserAcceptModel()
        {
            _userAcceptModel = typeof(UserAcceptModel).CreateInstance();
            _userAcceptModel.UserName = UserName;
            _userAcceptModel.Password = Password;
            _userAcceptModel.IsExistingUser = false;
            _userAcceptModel.NewUser = true;
            _userAcceptModel.setID = _setId;
            _userAcceptModel.UserID = UserId;
            _userAcceptModel.CreatedByUserID = UserId;
        }

        private UserAcceptModel GetAcceptModel(object resultModel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<UserAcceptModel>(js.Serialize(resultModel));
        }
    }
}
