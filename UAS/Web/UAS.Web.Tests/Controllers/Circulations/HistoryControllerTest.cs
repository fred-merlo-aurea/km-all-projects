using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrameworkUAD.DataAccess.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.DataAccess.Fakes;
using FrameworkUAD_Lookup.Entity;
using KMPlatform.DataAccess.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.Web.Controllers.Circulations;

namespace UAS.Web.Tests.Controllers.Circulations
{
    /// <summary>
    ///     Unit tests for <see cref=HistoryController/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HistoryControllerTest : ControllerTestBase
    {
        private Dictionary<string, string> _sampleJSONs;
        private Dictionary<string, int> _sizeDifferenceList;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Initialize(new HistoryController());
            InitializeSampleJSONs();
            InitializeDifferences();
        }

        [Test]
        public void JsonComparer_AllPropertiesSame_Success([Values("ProductSubscription", "Subscription", "SubscriptionPaid", "PaidBillTo", "MarketingMap", "SubscriptionResponseMap", "ProductSubscriptionDetail", "PubSubscriptionAdHoc")]string type)
        {
            // Arrange
            var fromJSON = _sampleJSONs[type];
            var toJSON = _sampleJSONs[type];
            var deliverCodeTypeID = new CodeType { };
            var actionList = new List<Action> { new Action { } };
            var cat = new List<CategoryCode> { new CategoryCode { } };
            var catType = new List<CategoryCodeType> { new CategoryCodeType { } };
            var trans = new List<TransactionCode> { new TransactionCode { } };
            var sStatusList = new List<SubscriptionStatus> { new SubscriptionStatus { } };
            var qSourceList = new List<Code> { new Code { } };
            var parList = new List<Code> { };
            var deliver = new List<Code> { new Code { } };
            var subSource = new List<Code> { new Code { } };
            var address = new List<Code> { new Code { } };
            var marketingList = new List<Code> { new Code { } };
            var countryList = new List<Country> { new Country { } };
            var regionList = new List<Region> { new Region { } };
            var questions = new List<ResponseGroup> { new ResponseGroup { } };
            var answers = new List<CodeSheet> { new CodeSheet { } };

            // Act
            var result = HistoryController.JsonComparer(type, fromJSON, toJSON, deliverCodeTypeID, actionList, cat, catType, trans,
                sStatusList, qSourceList, parList, deliver, subSource, address, marketingList, countryList, regionList, questions, answers) as List<HistoryController.HistoryData>;

            // Assert
            if (type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail")
            {
                result.ShouldSatisfyAllConditions(
                    () => result.Count.ShouldBe(1),
                    () => result[0].DisplayText.ShouldBe(" TO "));
            }
            else
            {
                result.ShouldBeEmpty();
            }
        }

        [Test]
        public void JsonComparer_AllProperties_FirstNull_Success([Values("ProductSubscription", "Subscription", "SubscriptionPaid", "PaidBillTo", "MarketingMap", "SubscriptionResponseMap", "ProductSubscriptionDetail", "PubSubscriptionAdHoc")]string type)
        {
            // Arrange
            var fromJSON = string.Empty;
            var toJSON = _sampleJSONs[type];
            var deliverCodeTypeID = new CodeType { };
            var actionList = new List<Action> { new Action { } };
            var cat = new List<CategoryCode> { new CategoryCode { } };
            var catType = new List<CategoryCodeType> { new CategoryCodeType { } };
            var trans = new List<TransactionCode> { new TransactionCode { } };
            var sStatusList = new List<SubscriptionStatus> { new SubscriptionStatus { } };
            var qSourceList = new List<Code> { new Code { } };
            var parList = new List<Code> { };
            var deliver = new List<Code> { new Code { } };
            var subSource = new List<Code> { new Code { } };
            var address = new List<Code> { new Code { } };
            var marketingList = new List<Code> { new Code { } };
            var countryList = new List<Country> { new Country { } };
            var regionList = new List<Region> { new Region { } };
            var questions = new List<ResponseGroup> { new ResponseGroup { } };
            var answers = new List<CodeSheet> { new CodeSheet { } };

            // Act
            var result = HistoryController.JsonComparer(type, fromJSON, toJSON, deliverCodeTypeID, actionList, cat, catType, trans,
                sStatusList, qSourceList, parList, deliver, subSource, address, marketingList, countryList, regionList, questions, answers) as List<HistoryController.HistoryData>;

            // Assert
            result.Count.ShouldBe(type == "ProductSubscription" ? 27 : _sizeDifferenceList[type]);
        }

        [Test]
        public void JsonComparer_AllProperties_SecondNull_Success([Values("ProductSubscription", "Subscription", "SubscriptionPaid", "PaidBillTo", "MarketingMap", "SubscriptionResponseMap", "ProductSubscriptionDetail", "PubSubscriptionAdHoc")]string type)
        {
            // Arrange
            var fromJSON = _sampleJSONs[type];
            var toJSON = string.Empty;
            var deliverCodeTypeID = new CodeType { };
            var actionList = new List<Action> { new Action { } };
            var cat = new List<CategoryCode> { new CategoryCode { CategoryCodeID = 1, CategoryCodeTypeID = 1 } };
            var catType = new List<CategoryCodeType> { new CategoryCodeType { } };
            var trans = new List<TransactionCode> { new TransactionCode { } };
            var sStatusList = new List<SubscriptionStatus> { new SubscriptionStatus { } };
            var qSourceList = new List<Code> { new Code { } };
            var parList = new List<Code> { };
            var deliver = new List<Code> { new Code { } };
            var subSource = new List<Code> { new Code { } };
            var address = new List<Code> { new Code { } };
            var marketingList = new List<Code> { new Code { } };
            var countryList = new List<Country> { new Country { } };
            var regionList = new List<Region> { new Region { } };
            var questions = new List<ResponseGroup> { new ResponseGroup { } };
            var answers = new List<CodeSheet> { new CodeSheet { } };

            // Act
            var result = HistoryController.JsonComparer(type, fromJSON, toJSON, deliverCodeTypeID, actionList, cat, catType, trans,
                sStatusList, qSourceList, parList, deliver, subSource, address, marketingList, countryList, regionList, questions, answers) as List<HistoryController.HistoryData>;

            // Assert
            result.Count.ShouldBe(_sizeDifferenceList[type]);
        }

        [Test]
        public void CreateDetailsCSV_ReturnsString()
        {
            // Arrange
            var controller = Controller as HistoryController;
            ShimFinalizeBatch.SelectBatchIdInt32ClientConnections = (p1, p2) => new FinalizeBatch { };
            ShimBatchHistoryDetail.SelectBatchIDBooleanClientConnectionsStringInt32 = (p1, p2, p3, p4) => new List<BatchHistoryDetail> { new BatchHistoryDetail { Object = "test", ToObjectValues = "test"} };
            ShimCodeType.SelectEnumsCodeType = (p) => new CodeType { };
            ShimAction.Select = () => new List<Action> { new Action { } };
            ShimCategoryCode.Select = () => new List<CategoryCode> { new CategoryCode { } };
            ShimCategoryCodeType.Select = () => new List<CategoryCodeType> { new CategoryCodeType { } };
            ShimTransactionCode.Select = () => new List<TransactionCode> { new TransactionCode { } };
            ShimSubscriptionStatus.Select = () => new List<SubscriptionStatus> { new SubscriptionStatus { } };
            ShimCountry.Select = () => new List<Country> { new Country { } };
            ShimRegion.Select = () => new List<Region> { new Region { } };
            ShimCode.SelectEnumsCodeType = (p) => new List<Code> { new Code { } };
            SessionMock.Setup(s => s[KeyCurrentClient]).Returns(new Client { Products = new List<KMPlatform.Object.Product> { new KMPlatform.Object.Product { } } });

            // Act
            var result = controller.CreateDetailsCSV(1, 1, true);

            // Assert
            result.ShouldBe(" ,  ,   ,    ,Edit 1,\r\nBatch #: 0,,,,\r\nUser Name:,,,,\r\nClient:,,,,\r\nProduct:,,,,\r\nDate Created:,,,,\r\nDate Finalized:,,,,\r\nBatch Count: 0,,,,\r\n,,,,\r\nSubscriber:,Sequence #: 0,SubscriptionID: 0,Date Updated:,\r\n");            
        }

        [Test]
        public void CreateDetail_ReturnsDataTable()
        {
            // Arrange
            var controller = Controller as HistoryController;
            ShimClient.SelectInt32 = (p) => new Client { };
            ShimProduct.SelectClientConnections = (p) => new List<Product> { new Product { } };
            ShimFinalizeBatch.SelectBatchIdInt32ClientConnections = (p1, p2) => new FinalizeBatch { };
            ShimBatchHistoryDetail.SelectBatchIDInt32BooleanClientConnectionsStringInt32 = (p1, p2, p3, p4, p5) => new List<BatchHistoryDetail> { new BatchHistoryDetail { Object = "test", ToObjectValues = "test" } };
            ShimCodeType.SelectEnumsCodeType = (p) => new CodeType { };
            ShimAction.Select = () => new List<Action> { new Action { } };
            ShimCategoryCode.Select = () => new List<CategoryCode> { new CategoryCode { } };
            ShimCategoryCodeType.Select = () => new List<CategoryCodeType> { new CategoryCodeType { } };
            ShimTransactionCode.Select = () => new List<TransactionCode> { new TransactionCode { } };
            ShimSubscriptionStatus.Select = () => new List<SubscriptionStatus> { new SubscriptionStatus { } };
            ShimCountry.Select = () => new List<Country> { new Country { } };
            ShimRegion.Select = () => new List<Region> { new Region { } };
            ShimCode.SelectEnumsCodeType = (p) => new List<Code> { new Code { } };
            SessionMock.Setup(s => s[KeyCurrentClient]).Returns(new Client { Products = new List<KMPlatform.Object.Product> { new KMPlatform.Object.Product { } } });

            // Act
            var result = controller.CreateDetail(1);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Columns.Count.ShouldBe(5),
                () => result.Columns.Contains("Detail1"),
                () => result.Columns.Contains("Detail2"),
                () => result.Columns.Contains("Detail3"),
                () => result.Columns.Contains("Detail4"),
                () => result.Columns.Contains("Edit1"),
                () => result.Rows.Count.ShouldBe(8));
        }

        private void InitializeSampleJSONs()
        {
            _sampleJSONs = new Dictionary<string, string> { };
            _sampleJSONs.Add("ProductSubscription", "{ProductSubscription : {\"Copies\":1,\"GraceIssues\":1,\"OnBehalfOf\":\"\",\"SubscriberSourceCode\":\"\",\"OrigsSrc\":\"\",\"SequenceID\":1,\"AccountNumber\":\"5\",\"Verify\":\"\",\"State\":\"\",\"IGrp_No\":\"11111111111111111111111111111111\",\"ReqFlag\":1,\"ClientName\":\"\",\"FullName\":\"\",\"FullAddress\":\"\",\"EmailID\":1,\"SubscriberProductDemographics\":[],\"AddRemoveID\":1,\"EmailStatusID\":1,\"FullZip\":\"\",\"IMBSeq\":\"\",\"IsActive\":true,\"IsComp\":false,\"IsPaid\":false,\"IsSubscribed\":false,\"MarketingMapList\":[],\"MemberGroup\":\"\",\"Par3CID\":1,\"PhoneCode\":1,\"ProductMapList\":[],\"ProspectList\":[],\"SFRecordIdentifier\":\"11111111111111111111111111111111\",\"SubSrcID\":1,\"tmpSubscriptionID\":1,\"SubGenSubscriberID\":1,\"SubGenSubscriptionID\":1,\"SubGenPublicationID\":1,\"SubGenMailingAddressId\":1,\"SubGenBillingAddressId\":1,\"IssuesLeft\":1,\"UnearnedReveue\":1,\"SubGenIsLead\":false,\"SubGenRenewalCode\":\"\",\"AdHocFields\":[],\"PublicationToolTip\":\"\",\"SubscriptionID\":1,\"IsNewSubscription\":false,\"AddressTypeID\":1,\"RegionID\":1,\"CountryID\":1,\"IsAddressValidated\":false,\"AddressValidationSource\":\"\",\"AddressValidationMessage\":\"\",\"IsLocked\":false,\"LockedByUserID\":1,\"IsInActiveWaveMailing\":false,\"WaveMailingID\":1,\"AddressTypeCodeId\":1,\"AddressUpdatedSourceTypeCodeId\":1,\"CreatedByUserID\":1,\"UpdatedByUserID\":1,\"ExternalKeyID\":1,\"FirstName\":\"\",\"LastName\":\"\",\"Company\":\"\",\"Title\":\"\",\"Occupation\":\"\",\"Address1\":\"\",\"Address2\":\"\",\"Address3\":\"\",\"City\":\"\",\"ZipCode\":\"\",\"Plus4\":\"\",\"CarrierRoute\":\"\",\"County\":\"\",\"Country\":\"\",\"Latitude\":1,\"Longitude\":1,\"Phone\":\"\",\"Fax\":\"\",\"Mobile\":\"\",\"Website\":\"\",\"Birthdate\":\"\\/Date(-2218996111111-1111)\\/\",\"Age\":1,\"Income\":\"\",\"Gender\":\"\",\"PhoneExt\":\"\",\"Email\":\"\",\"QDate\":\"\\/Date(1524136776618+1311)\\/\",\"QSourceID\":1,\"CategoryID\":1,\"TransactionID\":1,\"StatusUpdatedDate\":\"\\/Date(1524136776618+1311)\\/\",\"StatusUpdatedReason\":\"Subscribed\",\"DateCreated\":\"\\/Date(1524136776619+1311)\\/\",\"EmailStatus\":\"Active\",\"Name\":\"\",\"PubCode\":\"\",\"PubType\":\"\",\"PubID\":1,\"PubSubscriptionID\":1,\"SubscriptionStatusID\":1,\"Demo7\":\"\"}}");
            _sampleJSONs.Add("Subscription", "{Subscription : {\"ExternalKeyID\":1,\"FirstName\":\"\",\"LastName\":\"\",\"Title\":\"\",\"Company\":\"\",\"Address\":\"\",\"Address2\":\"\",\"City\":\"\",\"State\":\"\",\"Zip\":\"\",\"Plus4\":\"\",\"ForZip\":\"\",\"County\":\"\",\"Country\":\"\",\"Phone\":\"\",\"Fax\":\"\",\"Income\":\"\",\"Gender\":\"\",\"Address3\":\"\",\"Mobile\":\"\",\"Score\":1,\"Latitude\":1,\"Longitude\":1,\"Demo7\":\"\",\"IGrp_No\":\"11111111111111111111111111111111\",\"Email\":\"\",\"SubscriptionID\":1,\"IsMailable\":false,\"Age\":1,\"Occupation\":\"\",\"PhoneExt\":\"\",\"Website\":\"\",\"FullAddress\":\"\",\"AccountNumber\":\"\",\"ExternalKeyId\":1,\"EmailID\":1,\"SubscriberConsensusDemographics\":[],\"ProductList\":[],\"SubscriptionSearchResults\":[],\"MarketingMapList\":[],\"SuppressedEmail\":\"\",\"InSuppression\":false,\"Home_Work_Address\":\"\",\"Par3C\":\"\",\"RegionID\":1,\"WaveMailingID\":1,\"IsInActiveWaveMailing\":false,\"PublicationToolTip\":\"\",\"FullName\":\" \",\"FullZip\":\"\",\"PhoneCode\":1,\"CarrierRoute\":\"\",\"IsAddressValidated\":false,\"IsLocked\":false,\"IsNewSubscription\":false,\"LockedByUserID\":1,\"AddressValidationMessage\":\"\",\"AddressValidationSource\":\"\",\"tmpSubscriptionID\":1,\"ProductID\":1,\"ProductCode\":\"\",\"ClientName\":\"\",\"IsActive\":false,\"AddressTypeCodeId\":1,\"AddressUpdatedSourceTypeCodeId\":1,\"IsComp\":false,\"CountryID\":1,\"PhoneExists\":false,\"FaxExists\":false,\"EmailExists\":false,\"CategoryID\":1,\"TransactionID\":1,\"QSourceID\":1,\"RegCode\":\"\",\"Verified\":\"\",\"SubSrc\":\"\",\"OrigsSrc\":\"\",\"Source\":\"\",\"Priority\":\"\",\"IGrp_Cnt\":1,\"CGrp_No\":\"11111111111111111111111111111111\",\"CGrp_Cnt\":1,\"StatList\":false,\"Sic\":\"\",\"SicCode\":\"\",\"IGrp_Rank\":\"\",\"CGrp_Rank\":\"\",\"PubIDs\":\"\",\"IsExcluded\":false,\"IsLatLonValid\":false,\"LatLonMsg\":\"\",\"DateCreated\":\"\\/Date(1524161466691+1311)\\/\",\"CreatedByUserID\":1}}");
            _sampleJSONs.Add("SubscriptionPaid", "{SubscriptionPaid : {\"SubscriptionPaidID\":1,\"PubSubscriptionID\":1,\"PriceCodeID\":1,\"StartIssueDate\":\"\\/Date(1524161655685+1311)\\/\",\"ExpireIssueDate\":\"\\/Date(1524161655685+1311)\\/\",\"CPRate\":1,\"Amount\":1,\"AmountPaid\":1,\"BalanceDue\":1,\"PaidDate\":\"\\/Date(1524161655686+1311)\\/\",\"TotalIssues\":1,\"CheckNumber\":\"\",\"CCNumber\":\"\",\"CCExpirationMonth\":\"\",\"CCExpirationYear\":\"\",\"CCHolderName\":\"\",\"CreditCardTypeID\":1,\"PaymentTypeID\":1,\"DeliverID\":1,\"GraceIssues\":1,\"WriteOffAmount\":1,\"OtherType\":\"\",\"DateCreated\":\"\\/Date(1524161655687+1311)\\/\",\"CreatedByUserID\":1,\"Frequency\":1,\"Term\":1}");
            _sampleJSONs.Add("PaidBillTo", "{PaidBillTo : {\"PaidBillToID\":1,\"SubscriptionPaidID\":1,\"PubSubscriptionID\":1,\"FirstName\":\"\",\"LastName\":\"\",\"Company\":\"\",\"Title\":\"\",\"AddressTypeId\":1,\"Address1\":\"\",\"Address2\":\"\",\"Address3\":\"\",\"City\":\"\",\"RegionCode\":\"\",\"RegionID\":1,\"ZipCode\":\"\",\"Plus4\":\"\",\"CarrierRoute\":\"\",\"County\":\"\",\"Country\":\"\",\"CountryID\":1,\"Latitude\":1,\"Longitude\":1,\"IsAddressValidated\":false,\"AddressValidationDate\":\"\\/Date(1524161994949+1311)\\/\",\"AddressValidationSource\":\"\",\"AddressValidationMessage\":\"\",\"Email\":\"\",\"Phone\":\"\",\"PhoneExt\":\"\",\"Fax\":\"\",\"Mobile\":\"\",\"Website\":\"\",\"DateCreated\":\"\\/Date(1524161994951+1311)\\/\",\"CreatedByUserID\":1}}");
            _sampleJSONs.Add("MarketingMap", "{MarketingMap : {\"MarketingID\":1,\"PubSubscriptionID\":1,\"PublicationID\":1,\"IsActive\":false,\"DateCreated\":\"\\/Date(-62135596811111-1111)\\/\",\"CreatedByUserID\":1}}");
            _sampleJSONs.Add("SubscriptionResponseMap", "{\"PubSubscriptionDetailID\":1,\"PubSubscriptionID\":1,\"SubscriptionID\":1,\"CodeSheetID\":0,\"DateCreated\":\"\\/Date(1524142881321+1311)\\/\",\"CreatedByUserID\":1,\"ResponseOther\":\"\"}");
            _sampleJSONs.Add("ProductSubscriptionDetail", "{\"PubSubscriptionDetailID\":1,\"PubSubscriptionID\":1,\"SubscriptionID\":1,\"CodeSheetID\":0,\"DateCreated\":\"\\/Date(1524142814336+1311)\\/\",\"CreatedByUserID\":1,\"ResponseOther\":\"\"}");
            _sampleJSONs.Add("PubSubscriptionAdHoc", "{PubSubscriptionAdHoc : {\"AdHocField\":\"test\",\"Value\":\"test\"}}");
        }

        private void InitializeDifferences()
        {
            _sizeDifferenceList = new Dictionary<string, int> { };
            _sizeDifferenceList.Add("ProductSubscription", 29);
            _sizeDifferenceList.Add("Subscription", 1);
            _sizeDifferenceList.Add("SubscriptionPaid", 16);
            _sizeDifferenceList.Add("PaidBillTo", 2);
            _sizeDifferenceList.Add("MarketingMap", 1);
            _sizeDifferenceList.Add("SubscriptionResponseMap", 1);
            _sizeDifferenceList.Add("ProductSubscriptionDetail", 1);
            _sizeDifferenceList.Add("PubSubscriptionAdHoc", 0);
        }
    }
}
