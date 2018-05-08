using System;
using System.Configuration.Fakes;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using ecn.webservice.Facades.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Communicator;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using EcnCommonObject = ECN_Framework_Common.Objects;
using EcnCommonClasses = ecn.common.classes;
using EcnCommonClassesFakes = ecn.common.classes.Fakes;
using KmEntityFakes = KM.Common.Entity.Fakes;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private DataTable _subscriberDataTable;
        private IDisposable _shims;
        private const string SubscriberInvalidKey = "key";
        private const string Type = "type";
        private const string StatusType = "Y";
        private const string CompositeKey = "user1";
        private const string Success = "MSSkipped xmlns";
        private const string InvalidCompositeKey = "INVALID COMPOSITE KEY";
        private const string InvalidEcnKey = "INVALID ECN ACCESS KEY FORMAT";
        private const string KmCommon = "KMCommon_Application";
        private const string SecurityException = "SECURITY VIOLATION";
        private const string InvalidXml = "INVALID XML STRING";
        private const string InvalidXmlString = "INVALID LIST ID / XML STRING";
        private const string AuthenticationFailure = "LOGIN AUTHENTICATION FAILED";
        private const string ResponseBody = "ResponseBody";
        private const int ListId = 10;
        private const int ErrorCode = 1;

        [SetUp]
        public void SetupSubscriber()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDownSubscriber()
        {
            _shims?.Dispose();
            _subscriberDataTable?.Dispose();
        }

        [Test]
        public void AddSubscribers_ForInvalidKey_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.AddSubscribers(SubscriberInvalidKey, ListId, Type, Type, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void AddSubscribers_ForValidKeyAndAutoGenerateFalse_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Success));
        }

        [Test]
        public void AddSubscribersGenerateUDF_ForValidKeyAndAutoGenerateTrue_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersGenerateUDF(SampleEcnAccessKey, ListId, Type, Type, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Success));
        }

        [Test]
        public void AddSubscribers_ForValidKeyAndImportSuccessFalse_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ImportDataUserInt32StringStringDataTableString = (x, y, z, q, w, e, r) => false;

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForValidKeyAndNullDataTable_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForValidKeyAndXmlStringIsNull_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForValidKeyAndNullUser_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => null;

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForSecurityException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.SecurityException();

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribers_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimWebMethodExecutionWrapper.AllInstances.LogUnspecifiedExceptionExceptionString = (x, y, z) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.AddSubscribers(SampleEcnAccessKey, ListId, Type, Type, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupes_ForInvalidKey_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.AddSubscribersWithDupes(SubscriberInvalidKey, ListId, Type, Type, Type, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndInvalidComposeKey_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, Type, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidCompositeKey));

        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndImportSuccessTrue_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Success));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndImportSuccessFalse_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListManager.AllInstances.importErrorGet = (x) => InvalidEcnKey;
            ShimListFacade.AllInstances.ImportDataWithDupesUserInt32StringStringStringBooleanDataTableString =
                (x, q, w, e, r, t, y, u, i) => false;
            var collection = new NameValueCollection()
            {
                { KmCommon, ListId.ToString()}
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            KmEntityFakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (x, y, z, a, q, w) => ListId;

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndHasServiceFeatureFalse_ReturnSecurityException()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => false;

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(SecurityException));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndNullDataTable_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, Type);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXml));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndNullXmlString_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXmlString));
        }

        [Test]
        public void AddSubscribersWithDupes_ForValidAccessKeyAndNullUser_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => null;

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(AuthenticationFailure));
        }

        [Test]
        public void AddSubscribersWithDupes_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupes_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupes_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimWebMethodExecutionWrapper.AllInstances.LogUnspecifiedExceptionExceptionString = (x, y, z) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.AddSubscribersWithDupes(SampleEcnAccessKey, ListId, Type, Type, CompositeKey, true, string.Empty);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForInvalidAccessKey_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SubscriberInvalidKey, ListId, Type, 
                Type, Type, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForValidAccessKeyAndInvalidCompositeKey_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, Type, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidCompositeKey));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForValidAccessKeyAndCompositeKey_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Success));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForImportSuccessFalse_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ImportDataWithDupesUserInt32StringStringStringBooleanDataTableString =
                (x, q, w, e, r, t, y, u, i) => false;

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForServiceFeatureFalse_ReturnSecurityException()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => false;

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(SecurityException));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForNullDataTable_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXml));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForNullXmlString_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, string.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXmlString));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForNullUser_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => null;

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, string.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(AuthenticationFailure));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, string.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, string.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscribersWithDupesUsingSmartForm_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimWebMethodExecutionWrapper.AllInstances.LogUnspecifiedExceptionExceptionString = (x, y, z) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.AddSubscribersWithDupesUsingSmartForm(SampleEcnAccessKey, ListId, Type,
                Type, CompositeKey, true, string.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForInvalidAccessKey_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SubscriberInvalidKey, ListId, Type, Type, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKey_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(Success));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKeyAndImportFalse_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimListFacade.AllInstances.ImportDataUserInt32StringStringDataTableString = 
                (x, y, z, q, w, e, r) => false; 

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKeyAndNullDataTable_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, Type, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXml));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKeyAndNullXmlString_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => null;

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidXmlString));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKeyAndNullUser_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => null;

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(AuthenticationFailure));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForValidAccessKeyAndNullCustomer_ThrowSecurityException()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) => null;

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(SecurityException));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void AddSubscriberUsingSmartForm_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscribers();
            InitializeEmailFromSf();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimWebMethodExecutionWrapper.AllInstances.LogUnspecifiedExceptionExceptionString = (x, y, z) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.AddSubscriberUsingSmartForm(SampleEcnAccessKey, ListId, Type, Type, String.Empty, ListId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        private void InitializeEmailFromSf()
        {
            ShimListFacade.AllInstances.ValidateCustomerIdsInt32UserInt32 = (x, y, z, q) => new ListManager();
            ShimEmail.GetByEmailIDInt32User = (x, y) => new Email()
            {
                EmailID = ListId,
                EmailAddress = ResponseBody,
                Title = ResponseBody,
                FirstName = ResponseBody,
                LastName = ResponseBody,
                FullName = ResponseBody,
                Company = ResponseBody,
                Occupation = ResponseBody,
                Address = ResponseBody,
                Address2 = ResponseBody,
                City = ResponseBody,
                State = ResponseBody,
                Zip = ResponseBody,
                Country = ResponseBody,
                Voice = ResponseBody,
                Mobile = ResponseBody,
                Fax = ResponseBody,
                Website = ResponseBody,
                Age = ResponseBody,
                Income = ResponseBody,
                Birthdate = new DateTime(),
                UserEvent1Date = new DateTime(),
                UserEvent2Date = new DateTime()
            };
            ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
            {
                GroupID = ListId,
                GroupName = ResponseBody
            };
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (x, y, z) => new SmartFormsHistory()
            {
                Response_UserMsgBody = ResponseBody,
                Response_FromEmail = ResponseBody,
                Response_AdminEmail = ResponseBody,
                Response_UserMsgSubject = ResponseBody,
                Response_AdminMsgSubject = ResponseBody,
                Response_AdminMsgBody = ResponseBody
            };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x, y, z) => new List<GroupDataFields>
            {
                new GroupDataFields()
                {
                    ShortName = ResponseBody,
                    GroupDataFieldsID = ListId
                }
            };
            ShimEmailDirect.SaveEmailDirect = (x) => ListId;
            ShimSmartFormActivityLog.InsertSmartFormActivityLogUser = (x, y) => ListId;
            EcnCommonClassesFakes.ShimLicenseCheck.AllInstances.UpdateUsedInt32StringInt32 = 
                (x, y, z, q) => new EcnCommonClasses.LicenseCheck();
        }

        private void InitializeSubscribers()
        {
            ShimAPILogging.InsertAPILogging = (x) => ListId;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => new User()
            {
                UserID = ListId,
                DefaultClientID = ListId,
                CustomerID = ListId
            };
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer()
            {
                CustomerID = ListId
            };
            ShimGroup.GetByGroupIDInt32User = (x, y) => new Group()
            {
                CustomerID = ListId
            };
            _subscriberDataTable = new DataTable()
            {
                Columns = { "USER_", "emailaddress", "Action", "Counts", "user1", "_value" },
                Rows = { { "Row", "address", "M", ListId, "user", "user_ResponseBody" } }
            };
            ShimEmail.GetColumnNames = () => _subscriberDataTable;
            ShimListFacade.AllInstances.ExtractColumnNamesFromXmlStringStringInt32Int32 = (x, y, z, q) => _subscriberDataTable;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (x, y, z) => new List<GroupDataFields>
            {
                new GroupDataFields()
                {
                    ShortName = string.Empty,
                    GroupDataFieldsID = ListId
                }
            };
            ShimEmailGroup.ImportEmailsUserInt32Int32StringStringStringStringBooleanStringString =
                (x, y, z, q, w, e, r, t, u, i) => _subscriberDataTable;
            ShimLayoutPlans.GetByGroupID_NoAccessCheckInt32Int32 = (x, y) => new List<LayoutPlans>
            {
                new LayoutPlans()
                {
                    LayoutPlanID = ListId,
                    Status = StatusType,
                    BlastID = ListId,
                    EventType = Enums.ActionTypeCode.Submit.ToString(),
                    Criteria = Type,
                    CustomerID = ListId,
                    Period = ListId,
                    CreatedUserID = ListId
                }
            };
            ShimEmailGroup.GetByEmailAddressGroupID_NoAccessCheckStringInt32 = (x, y) => new EmailGroup()
            {
                EmailID = ListId,
                SubscribeTypeCode = Type
            };
            ShimEmailGroup.GetByEmailIDGroupID_NoAccessCheckInt32Int32 = (x, y) => new EmailGroup()
            {
                EmailID = ListId,
                SubscribeTypeCode = Type
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimEventOrganizer.FireEventLayoutPlansInt32User = (x, y, z) => new LayoutPlans();
            ShimBlastSingle.ExistsByBlastEmailLayoutPlanInt32Int32Int32Int32 = (x, y, z, q) => false;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => ListId;
            DataLayerCommunicatorFakes.ShimTriggerPlans.GetListSqlCommand = (x) => new List<TriggerPlans>
            {
                new TriggerPlans()
                {
                    Period = ListId,
                    BlastID = ListId,
                    TriggerPlanID = ListId
                }
            };
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (x, y, z) => true;
            ShimEmailGroup.ImportEmailsWithDupes_NoAccessCheckUserInt32StringStringStringStringBooleanStringBooleanString = 
                (x, y, z, q, w, e, r, t, u, i) => _subscriberDataTable;
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) => new EmailGroup() { EmailID = ListId };
            ShimEmailGroup.GetEmailIDFromCompositeInt32Int32StringStringStringUser = (x, y, z, q, w, e) => ListId;
            ShimEmailActivityLog.InsertEmailActivityLogUser = (x, y) => ListId;
            ShimLayoutPlans.GetBySmartFormID_NoAccessCheckInt32Int32 = (x, y) => new List<LayoutPlans>
            {
                new LayoutPlans()
                {
                    GroupID = ListId
                }
            };
            ShimEmailGroup.GetEmailIDFromWhatEmailInt32Int32StringUser = (x, y, z, q) => ListId;
        }
    }
}
