using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Transactions.Fakes;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KmCommonFake = KM.Common.Fakes;
using UadBusinessLogic = FrameworkUAD.BusinessLogic.Fakes;
using UadDataAccess = FrameworkUAD.DataAccess.Fakes;
using UadLookupBusinessLogic = FrameworkUAD_Lookup.BusinessLogic.Fakes;

namespace FrameworkUAD.Tests.BusinessLogic
{
    /// <summary>
    /// Unit test for <see cref="ProductSubscription"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProductSubscriptionTests
    {
        private const string UadWsAddSubscriber = "UAD_WS_AddSubscriber";
        private const string SampleFaxNumber = "212-9876543";
        private const string SamplePhoneNumber = "01905259335";
        private const string SampleMobileNumber = "9872267361";
        private const string UadApi = "UAD Api";
        private const string MailPermission = "MailPermission";
        private const string FaxPermission = "FaxPermission";
        private const string PhonePermission = "PhonePermission";
        private const string OtherProductsPermission = "OtherProductsPermission";
        private const string ThirdPartyPermission = "ThirdPartyPermission";
        private const string TextPermission = "TextPermission";
        private const string EmailRenewPermission = "EmailRenewPermission";
        private const string DefaultText = "Unit Test";
        private const string DemoText = "Demo Text";
        private const int TestOne = 1;
        private const int BatchCountLessTheHundred = 99;
        private const int BatchCountGreaterTheHundred = 101;
        private const int ClientId = 1;
        private const int FileRecurrenceTypeId = 1;
        private const int DatabaseFileTypeId = 1;
        private const int UserLogTypeId = 1;
        private const int ApplicationId = 1;
        private ProductSubscription _productSubscription;
        private IDisposable _shimContext;
        private bool _productSubscriptionSave;
        private bool _saveResponses;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _productSubscription = new ProductSubscription();
            UadDataAccess.ShimProductSubscription.GetSqlCommand = (x) => CreateProductSubscription();
            UadDataAccess.ShimDataFunctions.GetClientSqlConnectionClient = (x) => new SqlConnection();
            UadDataAccess.ShimDataFunctions.ExecuteScalarSqlCommand = (x) => TestOne;
            KmCommonFake.ShimDataFunctions.ExecuteNonQuerySqlCommand = (x) => true;
            ShimSourceFile.AllInstances.GetCustomPropertiesSourceFile = (sender, sqlCommand) => new SourceFile();
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (x, y, z, m, n, k, o) => TestOne;
            UadDataAccess.ShimProductSubscription.SaveProductSubscriptionClientConnections = (x, y) =>
            {
                _productSubscriptionSave = true;
                return TestOne;
            };
            ShimTransactionScope.AllInstances.Complete = (sender) => { };
            ShimUserLog.AllInstances.SaveUserLog = (x, y) => TestOne;
            ShimSourceFile.AllInstances.SaveSourceFileBoolean = (sender, objectResult, defaultRules) => TestOne;
            ShimSourceFile.AllInstances.SelectInt32StringBoolean = (sender, id, fileName, includeCustomProperty) =>
            {
                if (fileName.Equals(UadWsAddSubscriber))
                {
                    return null;
                }
                return new SourceFile();
            };
            ShimService.AllInstances.SelectEnumsServicesBoolean = (sender, service, includeObjects) => CreateServiceObject();
            ShimFieldMapping.AllInstances.SelectInt32Boolean = (x, y, z) => CreateFieldMappingObject();
            UadDataAccess.ShimSubscriberTransformed.StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBoolean = (z, x, c, v, b, n, m, a, s, d) => true;
            UadDataAccess.ShimSubscriberTransformed.StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (z, x, c, v, b, n, m, a, s, d, f, g, h, j) => true;
            UadBusinessLogic.ShimSubscriberOriginal.AllInstances.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = (x, y, z) => true;
            UadBusinessLogic.ShimSubscriberTransformed.AllInstances.SaveBulkSqlInsertListOfSubscriberTransformedClientConnectionsBoolean = (x, y, z, m) => true;
            UadBusinessLogic.ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString = (sender, clientConnection, code, fileType) => true;
            UadDataAccess.ShimProductSubscription.CountryRegionCleanseInt32StringClientConnections = (x, y, z) => true;
            UadDataAccess.ShimProductSubscription.UpdateMasterDBClientConnectionsString = (x, y) => true;
            UadDataAccess.ShimProductSubscription.DedupeMasterDBClientConnectionsString = (x, y) => true;
            UadDataAccess.ShimProductSubscription.SelectProcessCodeStringClientConnectionsString = (x, y, z) => CreateProductSubscriptionObject();
            UadLookupBusinessLogic.ShimCode.AllInstances.SelectEnumsCodeType = (sender, codeType) => CreateCodeObject();
            ShimClient.AllInstances.SelectInt32Boolean = (x, y, z) => new KMPlatform.Entity.Client { ClientConnections = new KMPlatform.Object.ClientConnections() };
            ShimPubSubscriptionsExtension.GetListSqlCommand = (x) => new List<string> { DefaultText };
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void SaveDQM_ProductSubscriptionIsNotNull_ReturnsObjectValue()
        {
            var productSubscription = CreateProductSubscription();
            var processCode = string.Empty;
            var clientName = string.Empty;
            var clientDisplayName = string.Empty;
            var clientId = ClientId;
            var client = new KMPlatform.Object.ClientConnections();
            var fileRecurrenceTypeId = FileRecurrenceTypeId;
            var databaseFileTypeId = DatabaseFileTypeId;

            // Act
            var result = _productSubscription.SaveDQM(productSubscription, processCode, clientName, clientDisplayName, clientId, client, fileRecurrenceTypeId, databaseFileTypeId);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(TestOne),
                () => result.Keys.FirstOrDefault(x => x == TestOne),
                () => result.Values.FirstOrDefault(x => x == TestOne));
        }

        [TestCase(true, BatchCountLessTheHundred)]
        [TestCase(false, BatchCountGreaterTheHundred)]
        public void ProfileSave_NewSubscriptionIsTrue_ReturnsResultFromMethod(bool isNewSubscription, int batchCount)
        {
            // Arrange
            var productSubscription = new Entity.ProductSubscription
            {
                IsNewSubscription = isNewSubscription,
                CreatedByUserID = TestOne,
                Fax = SampleFaxNumber,
                Phone = SamplePhoneNumber,
                Mobile = SampleMobileNumber,
                IsInActiveWaveMailing = isNewSubscription,
                PubID = TestOne
            };
            var original = productSubscription;
            var saveWaveMailing = true;
            var applicationId = ApplicationId;
            var UserLogTypes = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            var userLogTypeId = UserLogTypeId;
            var batch = new FrameworkUAS.Object.Batch
            {
                PublicationID = TestOne,
                IsActive = true,
                BatchID = TestOne,
                BatchCount = batchCount
            };
            var client = new KMPlatform.Object.ClientConnections();
            var waveMail = productSubscription;
            var waveMailDetail = new Entity.WaveMailingDetail();

            // Act
            var result = _productSubscription.ProfileSave(productSubscription, original, saveWaveMailing, applicationId, UserLogTypes, userLogTypeId, batch, client, waveMail, waveMailDetail);

            // Assert
            result.ShouldBe(TestOne);
            _productSubscriptionSave.ShouldBeTrue();
        }

        [TestCase(true, BatchCountLessTheHundred)]
        [TestCase(false, BatchCountGreaterTheHundred)]
        public void FullSave_AdHocFieldsIsNotNull_RetursObjectValue(bool isNewSubscription, int batchCount)
        {
            // Arrange
            var productSubscriptioncurrent = new Entity.ProductSubscription
            {
                IsNewSubscription = isNewSubscription,
                CreatedByUserID = TestOne,
                Fax = SampleFaxNumber,
                Phone = SamplePhoneNumber,
                Mobile = SampleMobileNumber,
                IsInActiveWaveMailing = isNewSubscription,
                PubID = TestOne,
                AdHocFields = new List<Object.PubSubscriptionAdHoc> { new Object.PubSubscriptionAdHoc { AdHocField = DefaultText, Value = DefaultText } },
                IsPaid = true
            };
            var productSubscriptionoriginal = productSubscriptioncurrent;
            var saveWaveMailing = true;
            var applicationId = ApplicationId;
            var userLogType = KMPlatform.BusinessLogic.Enums.UserLogTypes.Add;
            var userLogTypeId = UserLogTypeId;
            var batch = new FrameworkUAS.Object.Batch
            {
                PublicationID = TestOne,
                IsActive = true,
                BatchID = TestOne,
                BatchCount = batchCount
            };
            var clientId = ClientId;
            var madeResponseChange = true;
            var madePaidChange = true;
            var madeBillToChange = true;
            var answers = new List<Entity.ProductSubscriptionDetail>();
            var waveMail = new Entity.ProductSubscription();
            var waveMailDetail = new Entity.WaveMailingDetail();
            var subPaid = new Entity.SubscriptionPaid { SubscriptionPaidID = TestOne, Amount = 100 };
            var billTo = new Entity.PaidBillTo { SubscriptionPaidID = TestOne };
            var subscriberAnswers = new List<Entity.ProductSubscriptionDetail>();
            ShimPubSubscriptionsExtension.GetAdHocClientConnectionsInt32Int32 = (x, y, z) => new System.Data.DataTable();
            UadBusinessLogic.ShimProductSubscription.AllInstances.SaveResponsesIEnumerableOfProductSubscriptionDetailInt32Int32Int32Int32Int32ClientInt32 = (x, y, z, c, v, b, n, m, a) =>
            {
                _saveResponses = true;
                return Enumerable.Range(1, 10).ToList();
            };
            UadBusinessLogic.ShimProductSubscription.AllInstances.Get_AdHocsInt32Int32ClientConnections = (x, y, z, m) =>
                new List<Object.PubSubscriptionAdHoc>()
                {
                    new Object.PubSubscriptionAdHoc
                    {
                        AdHocField = DefaultText,
                        Value = DemoText
                    }
                };
            UadBusinessLogic.ShimProductSubscription.AllInstances.SelectProductSubscriptionInt32ClientConnectionsString = (x, y, z, m) => CreateProductSubscription();
            UadLookupBusinessLogic.ShimCode.AllInstances.SelectCodeNameEnumsCodeTypeString = (x, y, z) => new Code { CodeId = TestOne, CodeName = DefaultText };
            UadDataAccess.ShimBatch.GetSqlCommand = (x) =>
                new Entity.Batch
                {
                    BatchID = 1,
                    BatchCount = 101,
                    BatchNumber = 1,
                    IsActive = true
                };

            // Act
            var result = _productSubscription.FullSave(productSubscriptioncurrent, productSubscriptionoriginal, saveWaveMailing,
                applicationId, userLogType, userLogTypeId, batch, clientId, madeResponseChange, madePaidChange, madeBillToChange,
                answers, waveMail, waveMailDetail, subPaid, billTo, subscriberAnswers);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldBe(TestOne),
                () => _productSubscriptionSave.ShouldBeTrue(),
                () => _saveResponses.ShouldBeTrue());
        }

        private static Entity.ProductSubscription CreateProductSubscription()
        {
            // Arrange
            return new Entity.ProductSubscription
            {
                Fax = SampleFaxNumber,
                Phone = SamplePhoneNumber,
                Mobile = SampleMobileNumber,
                Address1 = null,
                Address2 = null,
                Address3 = null,
                City = null,
                Company = null,
                Country = null,
                County = null,
                FirstName = null,
                Gender = null,
                LastName = null,
                Plus4 = null,
                RegionCode = null,
                Title = null,
                ZipCode = null,
                Demo7 = null,
                Email = null,
                EmailStatusID = 0,
                PubCode = null,
                PubName = null,
                PubTypeDisplayName = null,
                QualificationDate = null,
                Status = null,
                StatusUpdatedReason = null,
                PubID = TestOne,
                IsPaid = true
            };
        }

        private static List<Code> CreateCodeObject()
        {
            return new List<Code>
            {
                new Code
                {
                    CodeId = TestOne,
                    CodeName = FrameworkUAD_Lookup.Enums.CodeType.Database_File.ToString()
                }
            };
        }

        private static Service CreateServiceObject()
        {
            return new Service
            {
                ServiceFeatures = new List<ServiceFeature>
                {
                    new ServiceFeature
                    {
                        SFName = UadApi,
                        SFCode =UadApi
                    }
                }
            };
        }

        private static Entity.ProductSubscription CreateProductSubscriptionObject()
        {
            return new Entity.ProductSubscription
            {
                SubscriptionID = TestOne,
                PubSubscriptionID = TestOne
            };
        }

        private static List<FieldMapping> CreateFieldMappingObject()
        {
            return new List<FieldMapping>
                {
                    new FieldMapping { MAFField = MailPermission },
                    new FieldMapping { MAFField = FaxPermission },
                    new FieldMapping { MAFField = PhonePermission },
                    new FieldMapping { MAFField = OtherProductsPermission },
                    new FieldMapping { MAFField = ThirdPartyPermission },
                    new FieldMapping { MAFField = ThirdPartyPermission },
                    new FieldMapping { MAFField = TextPermission },
                    new FieldMapping { MAFField = EmailRenewPermission}
                };
        }
    }
}
