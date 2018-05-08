using System;
using System.Collections.Generic;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using ecn.webservice.Facades.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Object;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using EcnCommonObject = ECN_Framework_Common.Objects;
using KmBusinessLogicFakes = KMPlatform.BusinessLogic.Fakes;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private const string InvalidEmailAddress = "INVALID NEW OR OLD EMAIL ADDRESS";
        private const string UnauthorizedAccess = "UNAUTHORIZED ACCESS TO LIST";

        [Test]
        public void UpdateEmailAddress_ForInvalidAccessKey_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.UpdateEmailAddress(SubscriberInvalidKey, ListId, Type, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKey_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, Type, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ResponseBody));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKeyAndImportSuccessFalse_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            ShimListManager.AllInstances.importDataWithUpdateUserDataTableStringStringInt32String = 
                (q, w, e, r, t, y, u) => false;

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, Type, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKeyAndNullDataTable_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, Type, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()),
                () => result.ShouldContain(InvalidXml));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKeyAndNullXmlString_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()),
                () => result.ShouldContain(InvalidXml));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKeyAndInValidEmailAddress_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            ShimEmail.IsValidEmailAddressString = (x) => false;

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()),
                () => result.ShouldContain(InvalidEmailAddress));
        }

        [Test]
        public void UpdateEmailAddress_ForValidAccessKeyAndNullGroup_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            ShimGroup.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()),
                () => result.ShouldContain(UnauthorizedAccess));
        }

        [Test]
        public void UpdateEmailAddress_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            KmBusinessLogicFakes.ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void UpdateEmailAddress_ForSecurityException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            KmBusinessLogicFakes.ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.SecurityException();

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void UpdateEmailAddress_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            KmBusinessLogicFakes.ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void UpdateEmailAddress_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeUpdateEmail();
            KmBusinessLogicFakes.ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimListManager.AllInstances.LogUnspecifiedExceptionExceptionStringString = (x, y, z, q) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.UpdateEmailAddress(SampleEcnAccessKey, ListId, string.Empty, ResponseBody, ResponseBody, ListId);

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        private void InitializeUpdateEmail()
        {
            InitializeSubscribers();
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimEmail.IsValidEmailAddressString = (x) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, q, w, e) => true;
            ShimDataFunctions.ExecuteSqlCommandString = (x, y) => ListId;
            ShimListManager.AllInstances.SendEmailFromSFInt32DataRowInt32Int32Int32StringUser = 
                (x, q, w, e, r, t, y, u) => new ListManager();
            ShimListManager.AllInstances.importResultsGet = (x) => ResponseBody;
        }
    }
}
