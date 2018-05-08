using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using static FrameworkUAD_Lookup.Enums;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMService = KMPlatform.Entity.Service;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Models.Circulations;
using UAS.Web.Models.Circulations.Fakes;
using UADObject = FrameworkUAD.Object;
using UADEntity = FrameworkUAD_Lookup.Entity;

namespace UAS.Web.Tests.Models.Circulations
{
    /// <summary>
    ///     Unit tests for <see cref=SubscriptionManager/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SubscriptionManagerTest
    {
        private IDisposable _shimContext;
        private PrivateObject _smPrivateObject;
        private SubscriptionManager _subscriptionManager;
        private Client _publisher;
        private User _user;
        private Product _product;
        private ProductSubscription _prodSubscription;
        private ShimEntityLists _shimEntitiesList;
        private ProductSubscription _orgSubscription;
        private List<Country> _countryList;
        private List<Region> _regionList;
        private List<ResponseGroup> _responseGroups;
        private List<CodeSheet> _codeSheet;
        private int _saveNewReturnValue;
        private bool _criticalErrorWasLogged;
        private List<UADEntity.SubscriptionStatus> _sstList;
        private List<CategoryCode> _categoryCodeList;
        private bool _isPaidUpFunctionsSetUp;
        private const string DummyStringValue = "dummyStringValue";        
        private const string DummyEmailAddress = "dummy@dummy.com";        
        private const string PubCode = "PubCode";
        private const string ApplicationNameCirculation = "Circulation";
        private const string ValueForErrorList = "<value>";
        private const string SamplePubCode = "SamplePubCode";
        private const string FieldSubVM = "subVM";
        private const string FieldAnswers = "answers";
        private const string SampleAdhocField = "SampleAdhocField";
        private const string SampleValue = "SampleValue";
        private const string AnotherAdhocField = "AnotherAdhocField";
        private const string EscapedWhiteSpace = "_";
        private const string WhiteSpace = " ";
        private const string BindModulesMethodName = "BindModules";
        private const string PrivateField_VM = "subVM";
        private const string CountryName = "Brazil";
        private const string CountryFullZip = "22071-000";
        private const string ContryUSFullZip = "NY 10024";
        private const string RegionCode = "RJA";
        private const int ClientID = 1;
        private const int PubID = 10;
        private const int UserID = 20;
        private const int DummyInt = 36;
        private const int SaveNewSuccessReturnValue = 4;
        private const int SaveNewErrorReturnValue = 0;
        private const int CountryIDUS = 1;
        private const int CountryIDCanada = 2;
        private const int RegionID = 21;
        private const int CountryID = 55;
        private const int CountryPrefix = 55;
        private const int CategoryValue = 11;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            InitShims();
            InstantiateSubscriptionManager();
            _isPaidUpFunctionsSetUp = false;
            _smPrivateObject = new PrivateObject(_subscriptionManager);
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }

        private void InitShims()
        {
            InitiateProductShims();
            InitiateEntitiesListShims();
            InitiateRegionalShims();
            InitiateResponseGroupShim();
            InitiateCodeSheetsShim();
            InitiatePaidShims();
            InitiateBatchShims();

            ShimApplicationLog.Constructor = (appLog) =>
            {
                new ShimApplicationLog(appLog)
                {
                    LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (accessKey, formatException, app, name, logClientId, subject) => DummyInt
                };
                _criticalErrorWasLogged = true;
            };
        }

        private static void InitiateBatchShims()
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
                            UserID = UserID,
                            IsActive = true
                        }
                    }
                };
            };
            ShimBatchNew.ConstructorListOfResponseGroupListOfCodeSheetClientProductProductSubscriptionEntityLists =
                (batchNew, questions, answers, myClient, MyProduct, myProductSubscription, CommonList) =>
                {
                    new ShimBatchNew(batchNew)
                    {
                        GetHistoryListProductSubscription = (ps) =>
                        {
                            return new List<HistoryContainer>();
                        }
                    };
                };
        }

        private static void InitiatePaidShims()
        {
            ShimSubscriptionPaid.Constructor = (sp) =>
            {
                new ShimSubscriptionPaid(sp)
                {
                    SelectInt32ClientConnections = (pubId, conn) => new SubscriptionPaid()
                };
            };

            ShimPaidBillTo.Constructor = (paidbillTo) =>
            {
                new ShimPaidBillTo(paidbillTo)
                {
                    SelectInt32ClientConnections = (pubId, conn) => null
                };
            };
        }

        private void InitiateCodeSheetsShim()
        {
            _codeSheet = new List<CodeSheet>()
            {
                new CodeSheet()
                {
                    ResponseGroupID = DummyInt,
                    ResponseValue = DummyStringValue,
                    CodeSheetID = DummyInt,
                    IsActive = true
                }
            };
            ShimCodeSheet.Constructor = (cs) =>
            {
                new ShimCodeSheet(cs)
                {
                    SelectInt32ClientConnections = (pubId, conn) => _codeSheet
                };
            };
        }

        private void InitiateResponseGroupShim()
        {
            _responseGroups = new List<ResponseGroup>()
            {
                new ResponseGroup()
                {
                    ResponseGroupName = PubCode.ToUpper(),
                    ResponseGroupID = DummyInt,
                    DisplayName = DummyStringValue,
                    ResponseGroupTypeId = DummyInt,
                    IsActive = true,
                    DisplayOrder = 1,
                    IsMultipleValue = false
                }
            };
            ShimResponseGroup.Constructor = (rg) =>
            {
                new ShimResponseGroup(rg)
                {
                    SelectInt32ClientConnections = (pubId, conn) => _responseGroups
                };
            };
        }

        private void InitiateRegionalShims()
        {
            _countryList = new List<Country>();
            ShimCountry.Constructor = (country) =>
            {
                new ShimCountry(country)
                {
                    Select = () => _countryList
                };
            };

            _regionList = new List<Region>();
            ShimRegion.Constructor = (region) =>
            {
                new ShimRegion(region)
                {
                    Select = () => _regionList
                };
            };
        }

        private void InitiateEntitiesListShims()
        {
            ShimEntityLists.Constructor = (entityLists) =>
            {
                _shimEntitiesList = new ShimEntityLists(entityLists)
                {
                    codeListGet = () => CreateCodeEntityList(),
                    countryListGet = () => _countryList,
                    sstListGet = () => _sstList,
                    categoryCodeListGet = () => _categoryCodeList,
                    codeTypeListGet = () => new List<UADEntity.CodeType>()
                    {
                        new UADEntity.CodeType()
                        {
                            CodeTypeName = FrameworkUAD_Lookup.Enums.CodeType.Action.ToString(),
                            CodeTypeId = DummyInt
                        }
                    },
                    actionListGet = () => new List<FrameworkUAD_Lookup.Entity.Action>()
                    {
                        new FrameworkUAD_Lookup.Entity.Action()
                        {
                            ActionID = DummyInt,
                            ActionTypeID = DummyInt,
                            CategoryCodeID = DummyInt,
                            TransactionCodeID = DummyInt
                        }
                    },
                    catTypeListGet = () => new List<UADEntity.CategoryCodeType>()
                    {
                        new UADEntity.CategoryCodeType()
                        {
                            CategoryCodeTypeID = DummyInt,
                            CategoryCodeTypeName =  FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")
                        }
                    },
                    transCodeListGet = () => new List<UADEntity.TransactionCode>()
                    {
                        new UADEntity.TransactionCode()
                        {
                            TransactionCodeName = FrameworkUAD_Lookup.Enums.TransactionCode.Free_PO_Hold.ToString().Replace("_", " "),
                            TransactionCodeID = DummyInt
                        }
                    }
                };
            };
        }

        private void InitiateProductShims()
        {
            ShimProductSubscription.Constructor = (productSub) =>
            {
                new ShimProductSubscription(productSub)
                {
                    FullSaveProductSubscriptionProductSubscriptionBooleanInt32EnumsUserLogTypesInt32BatchInt32BooleanBooleanBooleanListOfProductSubscriptionDetailProductSubscriptionWaveMailingDetailSubscriptionPaidPaidBillToListOfProductSubscriptionDetail
                    = (myProductSubscription, originalSubscription, saveWaveMailing, applicationID, ult, userLogTypeId, batch, clientID, madeResponseChange,
                        madePaidChange, madePaidBillToChange, productMapList, waveMailSubscriber, myWMDetail, mySubscriptionPaid, MyPaidBillTo, subscriptionDetails) =>
                    {
                        return _saveNewReturnValue;
                    },
                    Get_AdHocsInt32Int32ClientConnections = (pubId, pubSubscritpionID, conn) => new List<FrameworkUAD.Object.PubSubscriptionAdHoc>()
                    {
                        new FrameworkUAD.Object.PubSubscriptionAdHoc(DummyStringValue, DummyStringValue)
                    },
                    Get_AdHocsInt32ClientConnections = (pubId, conn) => new List<string>()
                    {
                        DummyStringValue
                    }
                };
            };

            ShimProductSubscriptionDetail.Constructor = (psd) =>
            {
                new ShimProductSubscriptionDetail(psd)
                {
                    SelectInt32ClientConnections = (pubSubId, conn) =>
                    {
                        return new List<ProductSubscriptionDetail>()
                        {
                            new ProductSubscriptionDetail()
                        };
                    }
                };
            };

            ShimProduct.Constructor = (product) =>
            {
                new ShimProduct(product)
                {
                    SelectInt32ClientConnectionsBooleanBoolean = (pubId, conn, includeCP, getLatest) => _product
                };
            };
        }

        private void InstantiateSubscriptionManager()
        {
            _publisher = new Client()
            {
                ClientID = ClientID,
                HasPaid = true
            };

            _user = new User()
            {
                UserID = UserID,
                AccessKey = new Guid()
            };

            _product = new Product()
            {
                PubID = PubID,
                PubCode = PubCode,
                AllowDataEntry = false
            };

            _prodSubscription = new ProductSubscription()
            {
                PubID = PubID,
                AdHocFields = new List<FrameworkUAD.Object.PubSubscriptionAdHoc>(),
                ProductMapList = new List<ProductSubscriptionDetail>(),
                PubCategoryID = DummyInt,
                PubTransactionID = DummyInt
            };

            _orgSubscription = new ProductSubscription()
            {
                DateCreated = DateTime.Now
            };

            _subscriptionManager = new SubscriptionManager(_publisher, _user, _product, _prodSubscription, new EntityLists(), _orgSubscription);

            _criticalErrorWasLogged = false;

            _sstList = new List<UADEntity.SubscriptionStatus>();
            _categoryCodeList = new List<CategoryCode>()
            {
                new CategoryCode()
                {
                    CategoryCodeID = DummyInt,
                    CategoryCodeTypeID = DummyInt
                }
            };
        }

        private static ClientGroup CreateClientGroup()
        {
            return new ClientGroup()
            {
                SecurityGroups = new List<SecurityGroup>()
                {
                    new SecurityGroup()
                    {
                        Services = new List<KMService>()
                        {
                            new KMService()
                            {
                                Applications = new List<Application>()
                                {
                                    new Application()
                                    {
                                        ApplicationName = ApplicationNameCirculation,
                                        ApplicationID = DummyInt
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<Code> CreateCodeEntityList()
        {
            return new List<Code>()
            {
                new Code()
                {
                    CodeName = UserLogTypes.Edit.ToString(),
                    CodeId = DummyInt
                },
                new Code()
                {
                    CodeName = UserLogTypes.Add.ToString(),
                    CodeId = DummyInt
                },
                new Code
                {
                    CodeName = ResponseGroupTypes.Circ_and_UAD.ToString().Replace(EscapedWhiteSpace, WhiteSpace),
                    CodeId = DummyInt
                },
                new Code()
                {
                    CodeName = FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace(EscapedWhiteSpace, WhiteSpace),
                    CodeId = DummyInt,
                    CodeTypeId = DummyInt
                }
            };
        }

        private SubscriberViewModel CreateSubscribeVM()
        {
            var subscriberVM = new SubscriberViewModel()
            {
                PubSubscription = _prodSubscription,
                MadeResponseChange = true,
                ProductResponseList = new List<ProductSubscriptionDetail>()
                {
                    new ProductSubscriptionDetail()
                    {
                        ResponseOther = ValueForErrorList
                    }
                },
                ErrorList = new Dictionary<string, string>()
            };

            return subscriberVM;
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void BindModules_WhenProductMapListIsNotNull_SetsFieldMembers(bool isNewSubscription)
        {
            // Arrange
            InitializeFakesForBindModules();
            _prodSubscription.IsLocked = true;
            _prodSubscription.IsNewSubscription = isNewSubscription;
            _subscriptionManager = new SubscriptionManager(_publisher, _user, _product, _prodSubscription, _shimEntitiesList, _orgSubscription);
            _smPrivateObject = new PrivateObject(_subscriptionManager);

            // Act
            _smPrivateObject.Invoke(BindModulesMethodName);
            var subscriberVM = _smPrivateObject.GetFieldOrProperty(FieldSubVM) as SubscriberViewModel;
            var answers = _smPrivateObject.GetFieldOrProperty(FieldAnswers) as List<CodeSheet>;

            // Assert
            _smPrivateObject.ShouldSatisfyAllConditions(
                () => subscriberVM.ShouldNotBeNull(),
                () => subscriberVM.ReactivateButtonEnabled.ShouldBeFalse(),
                () => subscriberVM.CategoryFreePaidEnabled.ShouldBe(isNewSubscription),
                () => subscriberVM.CategoryCodeEnabled.ShouldBe(isNewSubscription),
                () => answers.ShouldNotBeNull(),
                () => answers.ShouldNotBeEmpty(),
                () => answers.Count.ShouldBe(2),
                () => answers[0].CodeSheetID.ShouldBe(1),
                () => answers[1].CodeSheetID.ShouldBe(2),
                () => subscriberVM.QuestionList.ShouldNotBeNull(),
                () => subscriberVM.QuestionList.ShouldNotBeEmpty(),
                () => subscriberVM.QuestionList.Count.ShouldBe(1),
                () => subscriberVM.QuestionList[0].Answers.Count.ShouldBe(2),
                () => subscriberVM.QuestionList[0].SelectedAnswers.Count.ShouldBe(1),
                () => subscriberVM.QuestionList[0].SelectedAnswers[0].CodeSheetID.ShouldBe(2),
                () => subscriberVM.BatchHistoryList.ShouldBeEmpty(),
                () => _isPaidUpFunctionsSetUp.ShouldBeTrue());
        }

        [Test]
        public void BindModules_WhenProductMapListIsNull_SetsFieldMembers()
        {
            // Arrange
            InitializeFakesForBindModules();
            _prodSubscription.IsLocked = true;
            _prodSubscription.ProductMapList = null;
            _subscriptionManager = new SubscriptionManager(_publisher, _user, _product, _prodSubscription, _shimEntitiesList, _orgSubscription);
            _smPrivateObject = new PrivateObject(_subscriptionManager);            

            // Act
            _smPrivateObject.Invoke(BindModulesMethodName);
            var subscriberVM = _smPrivateObject.GetFieldOrProperty(FieldSubVM) as SubscriberViewModel;
            var answers = _smPrivateObject.GetFieldOrProperty(FieldAnswers) as List<CodeSheet>;

            // Assert
            _smPrivateObject.ShouldSatisfyAllConditions(
                () => subscriberVM.ShouldNotBeNull(),
                () => subscriberVM.ReactivateButtonEnabled.ShouldBeFalse(),
                () => subscriberVM.CategoryFreePaidEnabled.ShouldBeFalse(),
                () => subscriberVM.CategoryCodeEnabled.ShouldBeFalse(),
                () => answers.ShouldNotBeNull(),
                () => answers.ShouldNotBeEmpty(),
                () => answers.Count.ShouldBe(2),
                () => answers[0].CodeSheetID.ShouldBe(1),
                () => answers[1].CodeSheetID.ShouldBe(2),
                () => subscriberVM.QuestionList.ShouldNotBeNull(),
                () => subscriberVM.QuestionList.ShouldNotBeEmpty(),
                () => subscriberVM.QuestionList.Count.ShouldBe(1),
                () => subscriberVM.QuestionList[0].Answers.Count.ShouldBe(2),
                () => subscriberVM.QuestionList[0].SelectedAnswers.ShouldBeEmpty(),
                () => subscriberVM.BatchHistoryList.ShouldBeEmpty(),
                () => _isPaidUpFunctionsSetUp.ShouldBeTrue());
        }

        [Test]
        public void CompareSubscriber_CheckmyWMDetail_Success()
        {
            // Arrange
            var orgSubscription = new ProductSubscription { FirstName = "original", LastName = "original", Title = "original", Company = "original", Address1 = "original", Address2 = "original", Address3 = "original", AddressTypeCodeId = 1, City = "original", RegionCode = "original", RegionID = 1, ZipCode = "original", Plus4 = "original", County = "original", Country = "original", CountryID = 1, Email = "original", Phone = "original", Fax = "original", Mobile = "original", Demo7 = "original", PubCategoryID = 1, PubTransactionID = 1, IsSubscribed = true, SubscriptionStatusID = 1, Copies = 1, PhoneExt = "original", IsPaid = true };
            var prodSubscription = new ProductSubscription { FirstName = "product", LastName = "product", Title = "product", Company = "product", Address1 = "product", Address2 = "product", Address3 = "product", AddressTypeCodeId = 2, City = "product", RegionCode = "product", RegionID = 2, ZipCode = "product", Plus4 = "product", County = "product", Country = "product", CountryID = 2, Email = "product", Phone = "product", Fax = "product", Mobile = "product", Demo7 = "product", PubCategoryID = 2, PubTransactionID = 2, IsSubscribed = false, SubscriptionStatusID = 2, Copies = 2, PhoneExt = "product", IsPaid = false };
            var testEntity = new SubscriptionManager(_publisher, _user, _product, prodSubscription, new EntityLists(), orgSubscription);
            var privateTestObject = new PrivateObject(testEntity);
            InitializeFakesForBindModules();

            // Act
            privateTestObject.Invoke("CompareSubscriber");
            var myWMDetail = privateTestObject.GetFieldOrProperty("myWMDetail") as WaveMailingDetail;

            // Assert
            privateTestObject.ShouldSatisfyAllConditions(
                () => myWMDetail.ShouldNotBeNull(),
                () => myWMDetail.FirstName.ShouldBe("product"),
                () => myWMDetail.LastName.ShouldBe("product"),
                () => myWMDetail.Title.ShouldBe("product"),
                () => myWMDetail.Company.ShouldBe("product"),
                () => myWMDetail.Address1.ShouldBe("product"),
                () => myWMDetail.Address2.ShouldBe("product"),
                () => myWMDetail.Address3.ShouldBe("product"),
                () => myWMDetail.AddressTypeID.ShouldBe(2),
                () => myWMDetail.RegionCode.ShouldBe("product"),
                () => myWMDetail.RegionID.ShouldBe(2),
                () => myWMDetail.ZipCode.ShouldBe("product"),
                () => myWMDetail.Plus4.ShouldBe("product"),
                () => myWMDetail.County.ShouldBe("product"),
                () => myWMDetail.Country.ShouldBe("product"),
                () => myWMDetail.CountryID.ShouldBe(2),
                () => myWMDetail.Email.ShouldBe("product"),
                () => myWMDetail.Phone.ShouldBe("product"),
                () => myWMDetail.Fax.ShouldBe("product"),
                () => myWMDetail.Mobile.ShouldBe("product"),
                () => myWMDetail.Demo7.ShouldBe("product"),
                () => myWMDetail.PubCategoryID.ShouldBe(2),
                () => myWMDetail.PubTransactionID.ShouldBe(2),
                () => myWMDetail.IsSubscribed.ShouldBe(false),
                () => myWMDetail.SubscriptionStatusID.ShouldBe(2),
                () => myWMDetail.Copies.ShouldBe(2),
                () => myWMDetail.PhoneExt.ShouldBe("product"),
                () => myWMDetail.IsPaid.ShouldBe(false));
        }

        [Test]
        public void CompareSubscriber_ProdSubscription_Success()
        {
            // Arrange
            var orgSubscription = new ProductSubscription { FirstName = "original", LastName = "original", Title = "original", Company = "original", Address1 = "original", Address2 = "original", Address3 = "original", AddressTypeCodeId = 1, City = "original", RegionCode = "original", RegionID = 1, ZipCode = "original", Plus4 = "original", County = "original", Country = "original", CountryID = 1, Email = "original", Phone = "original", Fax = "original", Mobile = "original", Demo7 = "original", PubCategoryID = 1, PubTransactionID = 1, IsSubscribed = true, SubscriptionStatusID = 1, Copies = 1, PhoneExt = "original", IsPaid = true };
            var prodSubscription = new ProductSubscription { FirstName = "product", LastName = "product", Title = "product", Company = "product", Address1 = "product", Address2 = "product", Address3 = "product", AddressTypeCodeId = 2, City = "product", RegionCode = "product", RegionID = 2, ZipCode = "product", Plus4 = "product", County = "product", Country = "product", CountryID = 2, Email = "product", Phone = "product", Fax = "product", Mobile = "product", Demo7 = "product", PubCategoryID = 2, PubTransactionID = 2, IsSubscribed = false, SubscriptionStatusID = 2, Copies = 2, PhoneExt = "product", IsPaid = false };
            var testEntity = new SubscriptionManager(_publisher, _user, _product, prodSubscription, new EntityLists(), orgSubscription);
            var privateTestObject = new PrivateObject(testEntity);
            InitializeFakesForBindModules();

            // Act
            privateTestObject.Invoke("CompareSubscriber");

            // Assert
            privateTestObject.ShouldSatisfyAllConditions(
                () => prodSubscription.ShouldNotBeNull(),
                () => prodSubscription.FirstName.ShouldBe("original"),
                () => prodSubscription.LastName.ShouldBe("original"),
                () => prodSubscription.Title.ShouldBe("original"),
                () => prodSubscription.Company.ShouldBe("original"),
                () => prodSubscription.Address1.ShouldBe("original"),
                () => prodSubscription.Address2.ShouldBe("original"),
                () => prodSubscription.Address3.ShouldBe("original"),
                () => prodSubscription.AddressTypeCodeId.ShouldBe(1),
                () => prodSubscription.RegionCode.ShouldBe("original"),
                () => prodSubscription.RegionID.ShouldBe(1),
                () => prodSubscription.ZipCode.ShouldBe("original"),
                () => prodSubscription.Plus4.ShouldBe("original"),
                () => prodSubscription.County.ShouldBe("original"),
                () => prodSubscription.Country.ShouldBe("original"),
                () => prodSubscription.CountryID.ShouldBe(1),
                () => prodSubscription.Email.ShouldBe("original"),
                () => prodSubscription.Phone.ShouldBe("original"),
                () => prodSubscription.Fax.ShouldBe("original"),
                () => prodSubscription.Mobile.ShouldBe("original"),
                () => prodSubscription.Demo7.ShouldBe("original"),
                () => prodSubscription.PubCategoryID.ShouldBe(1),
                () => prodSubscription.PubTransactionID.ShouldBe(1),
                () => prodSubscription.IsSubscribed.ShouldBe(true),
                () => prodSubscription.SubscriptionStatusID.ShouldBe(1),
                () => prodSubscription.Copies.ShouldBe(1),
                () => prodSubscription.PhoneExt.ShouldBe("original"),
                () => prodSubscription.IsPaid.ShouldBe(true));
        }

        private void InitializeFakesForBindModules()
        {
            _prodSubscription.ProductMapList = new List<ProductSubscriptionDetail>
            {
                new ProductSubscriptionDetail
                {
                    CodeSheetID = 1,
                    PubSubscriptionDetailID = 1,
                    SubscriptionID = 1,
                    PubSubscriptionID = 1,
                    DateCreated = DateTime.UtcNow
                },
                new ProductSubscriptionDetail
                {
                    CodeSheetID = 2,
                    PubSubscriptionDetailID = 2,
                    SubscriptionID = 2,
                    PubSubscriptionID = 2,
                    DateCreated = DateTime.UtcNow.AddDays(1)
                }
            };

            ShimProductSubscription.AllInstances.Get_AdHocsInt32Int32ClientConnections = (ps, p, c, conn) => new List<UADObject.PubSubscriptionAdHoc>
            {
                new UADObject.PubSubscriptionAdHoc
                {
                    AdHocField = SampleAdhocField,
                    Value = SampleValue
                }
            };
            ShimProductSubscription.AllInstances.Get_AdHocsInt32ClientConnections = (ps, p, conn) => new List<string>
            {
                SampleAdhocField, AnotherAdhocField
            };

            _responseGroups = new List<ResponseGroup>() {
                new ResponseGroup
                {
                    IsActive = true,
                    PubID = 1,
                    ResponseGroupID = 1,
                    ResponseGroupTypeId = DummyInt,
                    PubCode = SamplePubCode,
                    IsMultipleValue = false
                }
            };

            _codeSheet = new List<CodeSheet>()
            {
                new CodeSheet
                {
                    CodeSheetID = 1,
                    IsActive = true,
                    PubID = 1,
                    ReportGroupID = 1,
                    ResponseGroupID = 1
                },
                new CodeSheet
                {
                    CodeSheetID = 2,
                    IsActive = true,
                    PubID = 1,
                    ReportGroupID = 1,
                    ResponseGroupID = 1
                }
            };

            ShimBatchNew.AllInstances.GetHistoryListProductSubscription = (b, ps) => new List<HistoryContainer>();

            ShimSubscriptionManager.AllInstances.SetUpPaidFunctions = (s) => { _isPaidUpFunctionsSetUp = true; };
        }

        private void AddCountry(int countryID, string countryName, int countryPrefix)
        {
            _countryList.Add(new Country()
            {
                CountryID = countryID,
                ShortName = countryName,
                PhonePrefix = countryPrefix
            });
        }
    }
}