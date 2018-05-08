using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc;
using ecn.gateway.Controllers.Fakes;
using ecn.gateway.Models;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.Gateway.Tests
{
    public partial class AccountControllerTest
    {
        private bool isEmailImported = false;

        [Test]
        public void Login_GatewayLogin_SavesUserProfileAndSession()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBe(loginModel.EMail));
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            result.ShouldBeOfType(typeof(RedirectToRouteResult));
            var routeValues = ((RedirectToRouteResult)result).RouteValues.Values;
            routeValues.ShouldSatisfyAllConditions(
                () => routeValues.ShouldContain("ConfirmationPage"),
                () => routeValues.ShouldContain("Account"),
                () => routeValues.ShouldContain(SamplePubCode),
                () => routeValues.ShouldContain(SampleTypeCode));
        }

        [Test]
        public void Login_WhenEmailAddressNull_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (e, g) => null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.Password");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("The Email Address you entered is invalid."))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WHenEmailNull_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (e, g) => null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.Password");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("The Email Address you entered is invalid."))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_CookiesHasGateWayValueAndModelHasBlankFields_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            loginModel.Gateway.GatewayValues[0].Value = string.Empty;
            SetCookie(loginModel.EMail);

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBe(loginModel.EMail));
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.GVP.blankFields");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Required Field(s) missing"))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_ValidateCustomFieldsIsFalse_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            ShimAccountController.AllInstances.ValidateCustomFieldsLoginModelGatewayGateway = (a, e, dg, g) => false;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBe(loginModel.EMail));
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.GateWayVSProfile");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Login failed."))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenPasswordMatchFalse_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            loginModel.Password = "SomeOtherPassword";

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.Password");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("The Email address or password you entered is invalid."))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenEmailAddressMatchFalse_AddsModelStateError()
        {
            // Arrange
            SetUpFakes();
            var loginModel = GetLoginModel();
            loginModel.EMail = "abc@abc.com";

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.Password");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("The Email Address or Password you entered is invalid."))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenGatewayCapture_SavesUserProfileAndSession()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ToString().ShouldBeNullOrWhiteSpace());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            isEmailImported.ShouldBeTrue();
            result.ShouldBeOfType(typeof(RedirectToRouteResult));
            var routeValues = ((RedirectToRouteResult)result).RouteValues.Values;
            routeValues.ShouldSatisfyAllConditions(
                () => routeValues.ShouldContain("ConfirmationPage"),
                () => routeValues.ShouldContain("Account"),
                () => routeValues.ShouldContain(SamplePubCode),
                () => routeValues.ShouldContain(SampleTypeCode));
        }

        [Test]
        public void Login_WhenGatewayCaptureAndCookiesHasValueAndBlankFields_AddsModelStateError()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            loginModel.Gateway.GatewayValues[0].Value = string.Empty;
            SetCookie(loginModel.EMail);

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.GVP.blankFields");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Required Field(s) missing"))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenGatewayCaptureAndEmailGroupNull_SavesUserProfileAndSession()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (e, g) => null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey]?.ToString().ShouldBeNullOrWhiteSpace());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            isEmailImported.ShouldBeTrue();
            result.ShouldBeOfType(typeof(RedirectToRouteResult));
            var routeValues = ((RedirectToRouteResult)result).RouteValues.Values;
            routeValues.ShouldSatisfyAllConditions(
                () => routeValues.ShouldContain("ConfirmationPage"),
                () => routeValues.ShouldContain("Account"),
                () => routeValues.ShouldContain(SamplePubCode),
                () => routeValues.ShouldContain(SampleTypeCode));
        }

        [Test]
        public void Login_WhenGatewayCaptureAndEmailGroupNullAndCookiesHasValueAndBlankFields_AddsModelStateError()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            loginModel.Gateway.GatewayValues[0].Value = string.Empty;
            SetCookie(loginModel.EMail);
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (e, g) => null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey]?.ToString().ShouldBeNullOrWhiteSpace());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.GVP.blankFields");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Required Field(s) missing"))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenGatewayCaptureAndEmailNull_SavesUserProfileAndSession()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            ShimEmail.GetByEmailAddressStringInt32 = (e, g) => isEmailImported ? new CommunicatorEntities.Email() : null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBe(loginModel.EMail));
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            isEmailImported.ShouldBeTrue();
            result.ShouldBeOfType(typeof(RedirectToRouteResult));
            var routeValues = ((RedirectToRouteResult)result).RouteValues.Values;
            routeValues.ShouldSatisfyAllConditions(
                () => routeValues.ShouldContain("ConfirmationPage"),
                () => routeValues.ShouldContain("Account"),
                () => routeValues.ShouldContain(SamplePubCode),
                () => routeValues.ShouldContain(SampleTypeCode));
        }

        [Test]
        public void Login_WhenGatewayCaptureAndEmailNullAndCookiesHaveValueAndBlankFields_AddsModelStateError()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            loginModel.Gateway.GatewayValues[0].Value = string.Empty;
            SetCookie(loginModel.EMail);
            ShimEmail.GetByEmailAddressStringInt32 = (e, g) => isEmailImported ? new CommunicatorEntities.Email() : null;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBe(loginModel.EMail));
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.GVP.blankFields");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Required Field(s) missing"))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenGatewayCaptureEmptyModelEmailAddress_AddModelStateErrors()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "CAPTURE");
            var loginModel = GetLoginModel();
            loginModel.EMail = string.Empty;

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldNotBeEmpty();
            _testEntity.ModelState.Keys.ShouldContain("LoginModel.Email");
            _testEntity.ModelState.Values.Any(x => x.Errors.Any(
                e => e.ErrorMessage.Contains("Please enter an email address"))).ShouldBeTrue();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Login_WhenGatewayisNotLoginOrCapture_AddModelStateErrors()
        {
            // Arrange
            SetUpFakes(defaultLoginOrCapture: "AUTHORIZE");
            var loginModel = GetLoginModel();

            // Act
            var result = _testEntity.Login(loginModel, string.Empty, string.Empty, SamplePubCode, SampleTypeCode);

            // Assert
            _testEntity.Session.ShouldSatisfyAllConditions(
                () => _testEntity.Session.ShouldNotBeNull(),
                () => _testEntity.Session.Keys.Count.ShouldBeGreaterThan(0),
                () => _testEntity.Session[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.Session[TypeCodeKey].ShouldBe(SampleTypeCode),
                () => _testEntity.Session[UserNameKey].ShouldBeNull());
            _testEntity.ViewData.ShouldSatisfyAllConditions(
                () => _testEntity.ViewData.ShouldNotBeNull(),
                () => _testEntity.ViewData.ContainsKey(PubCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData.ContainsKey(TypeCodeKey).ShouldBeTrue(),
                () => _testEntity.ViewData[PubCodeKey].ShouldBe(SamplePubCode),
                () => _testEntity.ViewData[TypeCodeKey].ShouldBe(SampleTypeCode));
            _testEntity.ModelState.Values.ShouldBeEmpty();
            result.ShouldBeOfType(typeof(ViewResult));
        }

        private void SetUpFakes(string defaultLoginOrCapture = "LOGIN")
        {
            _testEntity.Session.Clear();
            isEmailImported = false;
            var config = new NameValueCollection();
            config.Add("ECNEngineAccessKey", Guid.Empty.ToString());
            ShimConfigurationManager.AppSettingsGet = () => config;

            ShimUser.GetByAccessKeyStringBoolean = (k, b) => new User();
            ShimGateway.GetByGatewayIDInt32 = (g) => new CommunicatorEntities.Gateway
            {
                ValidatePassword = true,
                LoginOrCapture = defaultLoginOrCapture,
                GatewayValues = new List<CommunicatorEntities.GatewayValue>
                {
                    new CommunicatorEntities.GatewayValue
                    {
                        Value = "SampleValue",
                        Label = "SampleLabel",
                        GatewayID = 1,
                        GatewayValueID = 1
                    }
                }
            };
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (e, g) => new CommunicatorEntities.EmailGroup();
            ShimEmail.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (e, g) => new CommunicatorEntities.Email
            {
                EmailAddress = SampleEmailAddress,
                Password = "SamplePassword"
            };
            ShimEmail.GetByEmailAddressStringInt32 = (e, g) => new CommunicatorEntities.Email();
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (g, h, j, k, t, u, i, a, p, s) =>
                {
                    isEmailImported = true;
                    return new DataTable();
                };
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString =
                (u, i, o, r, w, t, y, z, c, g) =>
                {
                    isEmailImported = true;
                    return new DataTable();
                };
            ShimEventOrganizer.EventInt32Int32Int32UserNullableOfInt32 = (c, p, q, s, g) => { };
            ShimGatewayValue.GetByGatewayIDInt32 = (g) => new List<CommunicatorEntities.GatewayValue>
            {
                new CommunicatorEntities.GatewayValue { Value = "SampleGatewayValue",Label= "samplelabel" }
            };
            ShimAccountController.AllInstances.ValidateCustomFieldsLoginModelGatewayGateway = (a, m, dg, g) => true;
            ShimAccountController.AllInstances.getUDFsForListInt32User = (a, g, u) => new System.Collections.Hashtable
            {
                ["sampledatafield"] = 1,
                ["samplelabel"] = 1
            };
        }

        private LoginModel GetLoginModel()
        {
            var loginModel = new LoginModel
            {
                UserName = "SampleUserName",
                Password = "SamplePassword",
                Gateway = new CommunicatorEntities.Gateway()
                {
                    GatewayID = 1,
                    LoginOrCapture = "LOGIN",
                    GatewayValues = new List<CommunicatorEntities.GatewayValue>
                    {
                        new CommunicatorEntities.GatewayValue
                        {
                            GatewayID = 1,
                            GatewayValueID = 1,
                            Value = "SampleGatewayValue",
                            Label = "SampleLabel"
                        }
                    }
                },
                EMail = SampleEmailAddress,
                RememberMe = true
            };
            return loginModel;
        }

        private void SetCookie(string email)
        {
            HttpCookie myCookie = new HttpCookie("Gateway_Email");
            myCookie.Expires = DateTime.Now.AddDays(364);
            myCookie.Value = email;
            var cookies = new HttpCookieCollection();
            cookies.Add(myCookie);
            ShimHttpRequest.AllInstances.CookiesGet = (r) => cookies;
        }
    }
}
