using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UAS.Web.Controllers.Circulations;
using UAS.Web.Models.Circulations;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using KM.Platform.Fakes;
using NUnit.Framework;

namespace UAS.Web.Tests.Controllers.Circulations
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class RequalsBatchSetupControllerTest : ControllerTestBase
    {
        private RequalsBatchSetupController _controller;
        private ProductSubscription _prodSubscription;
        private ResponseGroup _responseGroup;
        private List<CodeSheet> _codeSheets;
        private ProductSubscriptionDetailVM _pubSubDetailVM;
        private bool _rgIsMultipleValue = true;
        private List<CategoryCode> _catCodes;
        private List<CategoryCodeType> _catTypes;
        private List<TransactionCode> _tranctionCode;
        private List<ProductSubscriptionDetail> _prodSubDetails;
        private bool _hasAccessReturn= true;
        private const string DummyStringValue = "dummyStringValue";
        private const string ApplicationName = "AMSCircMVC";
        private const int PubID = 10;
        private const int DummyInt = 36;
        private const int AnotherDummyInt = 50;
        private const int TransactionCodeValue = 22;
        private const int SaveSuccessReturnValue = 4;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _controller = new RequalsBatchSetupController();
            Initialize(_controller);
            InitEntities();
            InitShims();
        }

        private void InitEntities()
        {
            _prodSubscription = new ProductSubscription()
            {
                PubID = PubID,
                AdHocFields = new List<FrameworkUAD.Object.PubSubscriptionAdHoc>(),
                PubCategoryID = DummyInt,
                SubscriptionID = DummyInt,
                WaveMailingID = DummyInt,
                OrigsSrc = DummyStringValue,
                IsInActiveWaveMailing = true
            };
            _hasAccessReturn = true;
        }

        private void InitShims()
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (currentUser, service, feature, access) => _hasAccessReturn;
            InitProductShims();
            InitResponseGroupShims();
            InitCodeSheetsShim();
            InitCategroiesShims();
            InitCodeShims();
            InitApplicationShims();
            InitBatchShims();
        }

        private void InitBatchShims()
        {
            ShimBatch.Constructor = (batch) =>
            {
                new ShimBatch(batch)
                {
                    SelectClientConnections = (conn) => new List<Batch>()
                    {
                        new Batch()
                        {
                            PublicationID = PubID,
                            IsActive = true,
                            UserID = this.UserId
                        }
                    }
                };
            };
        }

        private static void InitApplicationShims()
        {
            KMPlatform.BusinessLogic.Fakes.ShimApplication.Constructor = (app) =>
            {
                new KMPlatform.BusinessLogic.Fakes.ShimApplication(app)
                {
                    Select = () => new List<KMPlatform.Entity.Application>()
                    {
                        new KMPlatform.Entity.Application()
                        {
                            ApplicationName = ApplicationName,
                            ApplicationID = DummyInt
                        }
                    }
                };
            };
        }

        private void InitCodeShims()
        {
            _tranctionCode = new List<TransactionCode>()
            {
                new TransactionCode()
                {
                    TransactionCodeValue = TransactionCodeValue,
                    TransactionCodeID = DummyInt
                }
            };

            ShimTransactionCode.Constructor = (transCode) =>
            {
                new ShimTransactionCode(transCode)
                {
                    SelectActiveIsFreeBoolean = (isFree) => _tranctionCode
                };
            };

            ShimCode.Constructor = (code) =>
            {
                new ShimCode(code)
                {
                    Select = () => new List<Code>()
                    {
                        new Code()
                        {
                            CodeId = DummyInt,
                            CodeName = FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString()
                        }
                    }
                };
            };
        }

        private void InitCategroiesShims()
        {
            _catCodes = new List<CategoryCode>()
            {
                new CategoryCode()
                {
                    CategoryCodeID = DummyInt,
                    CategoryCodeValue = DummyInt,
                    CategoryCodeTypeID = DummyInt
                }
            };

            ShimCategoryCode.Constructor = (catCode) =>
            {
                new ShimCategoryCode(catCode)
                {
                    Select = () => _catCodes
                };
            };

            _catTypes = new List<CategoryCodeType>()
            {
                new CategoryCodeType()
                {
                    CategoryCodeTypeName = FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "),
                    CategoryCodeTypeID = DummyInt
                }
            };

            ShimCategoryCodeType.Constructor = (codeType) =>
            {
                new ShimCategoryCodeType(codeType)
                {
                    Select = () => _catTypes
                };
            };
        }

        private void InitCodeSheetsShim()
        {
            _codeSheets = new List<CodeSheet>()
            {
                new CodeSheet()
                {
                    ResponseGroupID = DummyInt,
                    CodeSheetID = DummyInt
                }
            };

            ShimCodeSheet.Constructor = (cs) =>
            {
                new ShimCodeSheet(cs)
                {
                    SelectByResponseGroupIDInt32ClientConnections = (pubId, conn) => _codeSheets
                };
            };
        }

        private void InitResponseGroupShims()
        {
            _responseGroup = new ResponseGroup()
            {
                ResponseGroupID = DummyInt,
                IsMultipleValue = _rgIsMultipleValue
            };

            ShimResponseGroup.Constructor = (rg) =>
            {
                new ShimResponseGroup(rg)
                {
                    SelectByIDInt32ClientConnections = (pubId, conn) => _responseGroup
                };
            };
        }

        private void InitProductShims()
        {
            ShimProductSubscription.Constructor = (prodSub) =>
            {
                new ShimProductSubscription(prodSub)
                {
                    SelectProductSubscriptionInt32ClientConnectionsString = (pubSubID, clientConnections, clientName) => _prodSubscription,
                    FullSaveProductSubscriptionProductSubscriptionBooleanInt32EnumsUserLogTypesInt32BatchInt32BooleanBooleanBooleanListOfProductSubscriptionDetailProductSubscriptionWaveMailingDetailSubscriptionPaidPaidBillToListOfProductSubscriptionDetail
                    = (myProductSubscription, originalSubscription, saveWaveMailing, applicationID, ult, userLogTypeId, batch, clientID, madeResponseChange,
                        madePaidChange, madePaidBillToChange, productMapList, waveMailSubscriber, myWMDetail, mySubscriptionPaid, MyPaidBillTo, subscriptionDetails) =>
                    {
                        return SaveSuccessReturnValue;
                    }
                };
            };

            _prodSubDetails = new List<ProductSubscriptionDetail>()
            {
                new ProductSubscriptionDetail()
                {
                    CodeSheetID = DummyInt,
                    ResponseOther = DummyStringValue
                }
            };

            ShimProductSubscriptionDetail.Constructor = (psd) =>
            {
                new ShimProductSubscriptionDetail(psd)
                {
                    SelectInt32ClientConnections = (pubSubId, conn) => _prodSubDetails
                };
            };
        }

        private RequalBatchDetailsViewModel CreateVM(bool isDemoChecked = true)
        {
            _pubSubDetailVM = new ProductSubscriptionDetailVM()
            {
                ResponseGroupID = DummyInt,
                CodeSheetID = DummyInt,
                DemoChecked = isDemoChecked,
                PubSubscriptionID = PubID,
                SubscriptionID = DummyInt,
                ResponseOther = DummyStringValue,
                DateCreated = DateTime.Now,

            };

            var rqBatchVM = new RequalBatchDetailsViewModel()
            {
                PubSubscriptionID = PubID,
                PubSubDetails = new List<ProductSubscriptionDetailVM>()
                {
                    _pubSubDetailVM
                },
                Par3CID = DummyInt,
                QSourceID = DummyInt,
                SubSrc = DummyStringValue,
                QDate = DateTime.Now
            };

            return rqBatchVM;
        }
    }
}
