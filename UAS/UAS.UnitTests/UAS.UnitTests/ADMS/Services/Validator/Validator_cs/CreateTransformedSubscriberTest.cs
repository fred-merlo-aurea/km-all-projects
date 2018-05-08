using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ADMS.Services.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using static Core_AMS.Utilities.Enums;
using static KMPlatform.BusinessLogic.Enums;
using Assert = NUnit.Framework.Assert;
using ColumnDelimiter = KM.Common.Enums.ColumnDelimiter;
using Enums = FrameworkUAD_Lookup.Enums;
using KmCommonShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CreateTransformedSubscriberTest : Fakes
    {
        private const string CreateTransformedSubscriber = "CreateTransformedSubscriber";
        private const string IsDemo = "IsDemo";
        private const string IsNetworkDeployed = "IsNetworkDeployed";
        private const string UpdateInLiveDate = "updateinlivedate";
        private const string DqmProcessDate = "dqmprocessdate";
        private const string QualificationDate = "qualificationdate";
        private const string IsupdatedInlive  = "isupdatedinlive";
        private const string QdateFormat = "MDDYY";
        private const string DemoDate = "_DemoDate";
        private const string Pubcode = "pubcode";
        private const int IgnoredTypeId = 2;
        private const int DemoDateTypeId = 4;
        private const int KmTransformTypeId = 5;
        private const int DemographicUpdateCodeId = 3;
        private const int StandarTypeId = 0;
        private const int DemoTypeId = 1;
        private const int DemoRespOtherTypeId = 6;
        private const string CreateTransformedSubscriberMethodName = "CreateTransformedSubscriber";
        private const int SampleFieldMappingId = 1;
        private const string SampleValue = "Test";

        private PrivateObject _privateObject;
        private TestEntity _testEntity;
        private readonly ServiceFeature _serviceFeature = new ServiceFeature();
        private HashSet<TransformSplit> _allTransformSplit;
        private StringDictionary myRow;
        private int originalRowNumber = 0;
        private int transformedRowNumber = 0;
        private Dictionary<int, Guid> _subOriginalDictionary;
        private HashSet<AdHocDimensionGroup> _ahdGroups;
        private HashSet<TransformationFieldMap> _splitTranFieldMappings = null;
        private HashSet<Transformation> _splitTrans = null;
        private int _qSourceId = 0;
        private int _emailStatusId = 0;
        private List<Code> _demoUpdateCodes;
        private AdmsLog _admsLog;
        private SourceFile _sourceFile;
        private Enums.FileTypes _dft = Enums.FileTypes.Other;
        

        [SetUp]
        public void Setup()
        {
            SetupFakes(null);
            _privateObject = new PrivateObject(new global::ADMS.Services.Validator.Validator());
            _privateObject.SetField("demoDateTypeID", DemoDateTypeId);
            _privateObject.SetField("standarTypeID", StandarTypeId);
            _privateObject.SetField("demoTypeID", DemoTypeId);
            _privateObject.SetField("ignoredTypeID", IgnoredTypeId);
            _privateObject.SetField("kmTransformTypeID", KmTransformTypeId);
            _privateObject.SetField("demoRespOtherTypeID", DemoRespOtherTypeId);

             CreateAppSettingsObject();
            _demoUpdateCodes = new List<Code>()
            {
                new Code() { CodeName = "Append" , CodeId = 0},
                new Code() { CodeName = "Replace" , CodeId = 1},
                new Code() { CodeName = "Overwrite" , CodeId = 2}
            };
            _admsLog = new AdmsLog();
            _sourceFile = new SourceFile();
            _subOriginalDictionary = new Dictionary<int, Guid>();
            _ahdGroups = new HashSet<AdHocDimensionGroup>();

             KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
             {
                 return CreateDataTableString();
             };

             ShimServiceBase.AllInstances.clientPubCodesGet = _ =>
                new Dictionary<int, string>()
                {
                    {1, "Test_pubcode"}
                };
        }

        [Test]
        public void CreateTransformedSubscriber_FieldMappingsEmpty_ReturnsTransformedSubscription()
        {
            //Arrange

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft
            ) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
            result.DemographicTransformedList.Count.ShouldBe(0);
            result.FName.ShouldBe(string.Empty);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckSkips_ReturnsSubscriber()
        {
            // Arrange
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { "Test", "Test" },
                { "Demographics", "test" }
            };
            _sourceFile.FieldMappings = new HashSet<FieldMapping>
            {
                CreateSubscriberTestHelper.CreateFieldMapping(
                    "demoDate",
                    string.Empty,
                    DemoDateTypeId),
                CreateSubscriberTestHelper.CreateFieldMapping(
                    "Test",
                    "firstname",
                    StandarTypeId),
                CreateSubscriberTestHelper.CreateFieldMapping(
                    "Tst",
                    "zipcode",
                    StandarTypeId),
                CreateSubscriberTestHelper.CreateFieldMapping(
                    "Tst",
                    "zipcode",
                    DemoTypeId),
                CreateSubscriberTestHelper.CreateFieldMapping(
                    "Demographics",
                    "demogr1",
                    DemoTypeId)
            };

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft
            ) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
            result.FName.ShouldBe("Test");
            result.Zip.ShouldBeEmpty();
            result.DemographicTransformedList.ShouldNotBeNull();
            result.DemographicTransformedList.First().Value.ShouldBe("test");
        }

        [Test]
        public void CreateTransformedSubscriber_IterateOverStandardFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };            

            var standardFields = new[]
            {
                "pubcode", "sequenceid", "firstname", "lastname","title", "company", "address1", "address2",
                "city","regioncode", "zipcode", "plus4","forzip","county", "country", "countryid",
                "phone", "fax", "email", "pubcategoryid","pubtransactionid","pubtransactiondate",
                "qualificationdate","pubqsourceid", "regcode", "verify", "subscribersourcecode",
                "origssrc", "par3cid", "mailpermission", "faxpermission", "phonepermission",
                "otherproductspermission", "thirdpartypermission", "emailrenewpermission", "textpermission",
                "source", "priority", "sic", "siccode", "gender", "address3", "home_work_address", "demo7",
                "mobile", "latitude", "longitude", "score", "emailstatusid", "importrownumber", "isactive",
                "processcode", "externalkeyid", "accountnumber", "emailid", "copies", "graceissues",
                "iscomp", "ispaid", "issubscribed", "islatlonvalid", "latlonmsg", "occupation",
                "subscriptionstatusid", "subsrcid", "website"
            };

            foreach (var field in standardFields)
            {
                myRow.Add(field, "Test_" + field);
                _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId));
            }

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;

            //Assert
            result.ShouldNotBeNull();
            Assert.AreEqual("Test_pubcode", result.PubCode);
            Assert.AreEqual(0, result.Sequence);
            Assert.AreEqual("Test_firstname", result.FName);
            Assert.AreEqual("Test_lastname", result.LName);
            Assert.AreEqual("Test_title", result.Title);
            Assert.AreEqual("Test_company", result.Company);
            Assert.AreEqual("Test_address1", result.Address);
            Assert.AreEqual("Test_address2", result.MailStop);
            Assert.AreEqual("Test_city", result.City);
            Assert.AreEqual("Test_regioncode", result.State);
            Assert.AreEqual("Test_zipcode", result.Zip);
            Assert.AreEqual("Test_plus4", result.Plus4);
            Assert.AreEqual("Test_forzip", result.ForZip);
            Assert.AreEqual("Test_county", result.County);
            Assert.AreEqual("Test_country", result.Country);
            Assert.AreEqual(0, result.CountryID);
            Assert.AreEqual("Test_email", result.Email);
            Assert.AreEqual(0, result.CategoryID);
            Assert.AreEqual(0, result.TransactionID);
            Assert.AreEqual(new DateTime(1900, 1, 1), result.TransactionDate);            
            Assert.AreEqual(DateTime.Today.Date, result.QDate);
            Assert.AreEqual(0, result.QSourceID);
            Assert.AreEqual("Test_regcode", result.RegCode);
            Assert.AreEqual("Test_verify", result.Verified);
            Assert.AreEqual("Test_origssrc", result.OrigsSrc);
            Assert.AreEqual("Test_par3cid", result.Par3C);
            Assert.IsNull(result.MailPermission);
            Assert.IsNull(result.FaxPermission);
            Assert.IsNull(result.PhonePermission);
            Assert.IsNull(result.OtherProductsPermission);
            Assert.IsNull(result.ThirdPartyPermission);
            Assert.IsNull(result.EmailRenewPermission);
            Assert.IsNull(result.TextPermission);
            Assert.AreEqual("Test_source", result.Source);
            Assert.AreEqual("Test_priority", result.Priority);
            Assert.AreEqual("Test_sic", result.Sic);
            Assert.AreEqual("Test_siccode", result.SicCode);
            Assert.AreEqual("Test_gender", result.Gender);
            Assert.AreEqual("Test_address3", result.Address3);
            Assert.AreEqual("Test_home_work_address", result.Home_Work_Address);
            Assert.AreEqual("Test_demo7", result.Demo7);
            Assert.AreEqual(0, result.Latitude);
            Assert.AreEqual(0, result.Longitude);
            Assert.AreEqual(0, result.EmailStatusID);
            Assert.AreEqual(0, result.ImportRowNumber);
            Assert.IsFalse(result.IsActive);
            Assert.AreEqual("Test_processcode", result.ProcessCode);
            Assert.AreEqual(0, result.ExternalKeyId);
            Assert.AreEqual("Test_accountnumber", result.AccountNumber);
            Assert.AreEqual(0, result.EmailID);
            Assert.AreEqual(0, result.Copies);
            Assert.AreEqual(0, result.GraceIssues);
            Assert.IsFalse(result.IsComp);
            Assert.IsFalse(result.IsPaid);
            Assert.IsFalse(result.IsSubscribed);
            Assert.AreEqual("Test_occupation", result.Occupation);
            Assert.AreEqual(0, result.SubscriptionStatusID);
            Assert.AreEqual("Test_website", result.Website);

            Assert.AreEqual(string.Empty, result.Mobile);
            Assert.AreEqual(string.Empty, result.Phone);
            Assert.AreEqual(string.Empty, result.Fax);

            Assert.AreEqual("Test_latlonmsg", result.LatLonMsg);
            Assert.IsFalse(result.IsLatLonValid);
            Assert.AreEqual(0, result.OriginalImportRow);
        }

        [Test]
        public void CreateTransformedSubscriber_IterateOverMultimappingStandardFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };

            var standardFields = new[]
            {
                "pubcode", "sequenceid", "firstname", "lastname", "title", "company", "address1", "address2",
                "city", "regioncode", "zipcode", "plus4", "forzip", "county", "country", "countryid", "phone",
                "fax", "email", "pubcategoryid", "pubtransactionid", "pubtransactiondate", "qualificationdate",
                "pubqsourceid", "regcode", "verify", "subscribersourcecode", "origssrc", "par3cid",
                "mailpermission", "faxpermission", "phonepermission", "otherproductspermission",
                "thirdpartypermission", "emailrenewpermission", "textpermission", "source", "priority", "sic",
                "siccode", "gender", "address3", "home_work_address", "demo7", "mobile", "latitude", "longitude",
                "score", "emailstatusid", "importrownumber", "isactive", "processcode", "externalkeyid",
                "accountnumber", "emailid", "copies", "graceissues", "iscomp", "ispaid", "issubscribed",
                "occupation", "subscriptionstatusid", "subsrcid", "website", "islatlonvalid", "latlonmsg",
            };

            foreach (var field in standardFields)
            {
                myRow.Add(field, "Test_" + field);
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test_pubcode", result.PubCode);
            Assert.AreEqual(0, result.Sequence);
            Assert.AreEqual("Test_firstname", result.FName);
            Assert.AreEqual("Test_lastname", result.LName);
            Assert.AreEqual("Test_title", result.Title);
            Assert.AreEqual("Test_company", result.Company);
            Assert.AreEqual("Test_address1", result.Address);
            Assert.AreEqual("Test_address2", result.MailStop);
            Assert.AreEqual("Test_city", result.City);
            Assert.AreEqual("Test_regioncode", result.State);
            Assert.AreEqual("Test_zipcode", result.Zip);
            Assert.AreEqual("Test_plus4", result.Plus4);
            Assert.AreEqual("Test_forzip", result.ForZip);
            Assert.AreEqual("Test_county", result.County);
            Assert.AreEqual("Test_country", result.Country);
            Assert.AreEqual(0, result.CountryID);
            Assert.AreEqual("Test_email", result.Email);
            Assert.AreEqual(0, result.CategoryID);
            Assert.AreEqual(0, result.TransactionID);
            Assert.AreEqual(new DateTime(1900, 1, 1), result.TransactionDate);
            Assert.AreEqual(DateTime.Today.Date, result.QDate);
            Assert.AreEqual(0, result.QSourceID);
            Assert.AreEqual("Test_regcode", result.RegCode);
            Assert.AreEqual("Test_verify", result.Verified);            
            Assert.AreEqual("Test_origssrc", result.OrigsSrc);
            Assert.AreEqual("Test_par3cid", result.Par3C);
            Assert.IsNull(result.MailPermission);
            Assert.IsNull(result.FaxPermission);
            Assert.IsNull(result.PhonePermission);
            Assert.IsNull(result.OtherProductsPermission);
            Assert.IsNull(result.ThirdPartyPermission);
            Assert.IsNull(result.EmailRenewPermission);
            Assert.IsNull(result.TextPermission);
            Assert.AreEqual("Test_source", result.Source);
            Assert.AreEqual("Test_priority", result.Priority);
            Assert.AreEqual("Test_sic", result.Sic);
            Assert.AreEqual("Test_siccode", result.SicCode);
            Assert.AreEqual("Test_gender", result.Gender);
            Assert.AreEqual("Test_address3", result.Address3);
            Assert.AreEqual("Test_home_work_address", result.Home_Work_Address);
            Assert.AreEqual("Test_demo7", result.Demo7);
            Assert.AreEqual(0, result.Latitude);
            Assert.AreEqual(0, result.Longitude);
            Assert.AreEqual(0, result.EmailStatusID);
            Assert.AreEqual(0, result.ImportRowNumber);
            Assert.IsFalse(result.IsActive);
            Assert.AreEqual("Test_processcode", result.ProcessCode);
            Assert.AreEqual(0, result.ExternalKeyId);
            Assert.AreEqual("Test_accountnumber", result.AccountNumber);
            Assert.AreEqual(0, result.EmailID);
            Assert.AreEqual(0, result.Copies);
            Assert.AreEqual(0, result.GraceIssues);
            Assert.IsFalse(result.IsComp);
            Assert.IsFalse(result.IsPaid);
            Assert.IsFalse(result.IsSubscribed);
            Assert.AreEqual("Test_occupation", result.Occupation);
            Assert.AreEqual(0, result.SubscriptionStatusID);
            Assert.AreEqual("Test_website", result.Website);

            Assert.AreEqual(string.Empty, result.Mobile);
            Assert.AreEqual(string.Empty, result.Phone);
            Assert.AreEqual(string.Empty, result.Fax);

            Assert.AreEqual("Test_latlonmsg", result.LatLonMsg);
            Assert.IsFalse(result.IsLatLonValid);
            Assert.AreEqual(0, result.OriginalImportRow);
        }

        [Test]
        public void CreateTransformedSubscriber_IterateOverNonNumericFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };

            var numericFields = new[]
            {
                "sequenceid","countryid", "pubcategoryid", "pubtransactionid", "pubqsourceid", "latitude", "longitude", "score",
                "emailstatusid", "importrownumber", "emailid", "copies", "subscriptionstatusid", "subsrcid"
            };

            var phoneFields = new[]
            {
                "phone", "fax", "mobile"
            };

            var dateFields = new[]
            {
                "pubtransactiondate", "qualificationdate"
            };

            var boolFields = new[]
            {
                "verify", "mailpermission", "isactive", "faxpermission", "phonepermission","graceissues", "iscomp", "ispaid", "issubscribed",
                "otherproductspermission", "thirdpartypermission", "emailrenewpermission", "textpermission", "islatlonvalid"
            };

            foreach (var field in numericFields)
            {
                myRow.Add(field, "999");
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in phoneFields)
            {
                myRow.Add(field, "+78001002030");
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in dateFields)
            {
                myRow.Add(field, "2018/1/1");
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in boolFields)
            {
                myRow.Add(field, "true");
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(999, result.Sequence);
            Assert.AreEqual(999, result.CountryID);
            Assert.AreEqual(999, result.CategoryID);
            Assert.AreEqual(999, result.TransactionID);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.TransactionDate);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.QDate);
            Assert.AreEqual(999, result.QSourceID);
            Assert.IsTrue(result.MailPermission);
            Assert.IsTrue(result.FaxPermission);
            Assert.IsTrue(result.PhonePermission);
            Assert.IsTrue(result.OtherProductsPermission);
            Assert.IsTrue(result.ThirdPartyPermission);
            Assert.IsTrue(result.EmailRenewPermission);
            Assert.IsTrue(result.TextPermission);
            Assert.AreEqual(999, result.Latitude);
            Assert.AreEqual(999, result.Longitude);
            Assert.AreEqual(999, result.EmailStatusID);
            Assert.AreEqual(999, result.ImportRowNumber);
            Assert.AreEqual(0, result.ExternalKeyId);
            Assert.AreEqual(999, result.EmailID);
            Assert.AreEqual(999, result.Copies);
            Assert.AreEqual(0, result.GraceIssues);
            Assert.IsTrue(result.IsComp);
            Assert.IsTrue(result.IsPaid);
            Assert.IsTrue(result.IsSubscribed);
            Assert.AreEqual(999, result.SubscriptionStatusID);
            Assert.IsTrue(result.IsActive);
            Assert.AreEqual("78001002030", result.Mobile);
            Assert.AreEqual("78001002030", result.Phone);
            Assert.AreEqual("78001002030", result.Fax);

            Assert.IsTrue(result.IsLatLonValid);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemographicsInMultimapping_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" }
            };

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping(SampleValue, "firstname", StandarTypeId);
            var multimapping = CreateSubscriberTestHelper.CreateMultiMapping(SampleValue, DemoTypeId);
            mapping.FieldMultiMappings.Add(multimapping);

            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            result.ShouldNotBeNull();
            result.FName.ShouldBe(SampleValue);
            result.DemographicTransformedList.ShouldNotBeNull();
            result.DemographicTransformedList.First().Value.ShouldBe("Test");
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemographicsWithSpecialSplit_ReturnsSubscriber()
        {
            // Arrange
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping(SampleValue, "firstname", DemoTypeId);
            _sourceFile.FieldMappings.Add(mapping);

            ShimEnums.GetUADFeatureString = _ => KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split;

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.FName);
            Assert.AreEqual(string.Empty, result.City);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(SampleValue, result.DemographicTransformedList.First().Value);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemographicsWithSpecialSplitAndTransformationFieldMapMentioned_ReturnsSubscriber()
        {
            // Arrange
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping(SampleValue, "firstname", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            ShimEnums.GetUADFeatureString = _ => KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split;
            _splitTranFieldMappings.Add(new TransformationFieldMap() { FieldMappingID = SampleFieldMappingId });
            _splitTrans = new HashSet<Transformation>()
            {
                new Transformation()
                {
                    TransformationID = 0,
                    TransformationName = string.Empty,
                    TransformationDescription = string.Empty
                }
            };
            _allTransformSplit = new HashSet<TransformSplit>() {
                new TransformSplit()
                {
                    Delimiter = ","
                }
            };

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(SampleValue, result.DemographicTransformedList.First().Value);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoDate_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "2018/1/1" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018,1,1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_MultiMapCheckDemoDate_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "2018/1/1" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("TestDate", DemoDateTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);


            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoFieldWithDateMapping_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "test", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoFieldWithDateMappingEmptyDate_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "test", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoFieldWithDateMultiMapping_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "test", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("test", DemoTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("test_DEMODATE", DemoDateTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoFieldWithDateMultimappingMappingEmptyDate_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "test", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("Test", DemoTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("Test_DemoDate", DemoDateTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemoDateSetDefault_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_MultiMapCheckDemoDateSetDefault_ReturnsSubscriber()
        {
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_DemoDate", "" },
                { "QUALIFICATIONDATE", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_DemoDate", "test_DEMODATE", DemoDateTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("TestDate", DemoDateTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("QUALIFICATIONDATE", "QUALIFICATIONDATE", StandarTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            _sourceFile.FieldMappings.Add(mapping);


            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.DemographicTransformedList.First().DateCreated);
        }

        [Test]
        public void CreateTransformedSubscriber_CheckDemographicsMultiMappingWithSpecialSplit_ReturnsSubscriber()
        {
            // Arrange
            myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { SampleValue, SampleValue },
                { "Demographics", "test" },
                { "Test_RESPONSEOTHER", "2018/1/1" }
            };
            _splitTranFieldMappings = new HashSet<TransformationFieldMap>();

            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "test", DemoTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("test", DemoTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test_RESPONSEOTHER", "test_RESPONSEOTHER", DemoRespOtherTypeId);
            mapping.FieldMappingID = SampleFieldMappingId;
            mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping("test_RESPONSEOTHER", DemoRespOtherTypeId));
            _sourceFile.FieldMappings.Add(mapping);

            _splitTranFieldMappings.Add(new TransformationFieldMap()
            {
                FieldMappingID = SampleFieldMappingId
            });

            ShimEnums.GetUADFeatureString = _ => KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split;
            _splitTrans = new HashSet<Transformation>
            {
                new Transformation()
                {
                    TransformationID = 0,
                    TransformationName = SampleValue,
                    TransformationDescription = string.Empty
                }
            };
            _allTransformSplit = new HashSet<TransformSplit>
            {
                new TransformSplit()
                {
                    TransformationID = 0,
                    Delimiter = ":"
                }
            };

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriberMethodName,
                _serviceFeature,
                _allTransformSplit,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                _subOriginalDictionary,
                _ahdGroups,
                _splitTranFieldMappings,
                _splitTrans,
                _qSourceId,
                _emailStatusId,
                _demoUpdateCodes,
                _admsLog,
                _sourceFile,
                _dft) as SubscriberTransformed;


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DemographicTransformedList);
            Assert.AreEqual(SampleValue, result.DemographicTransformedList.First().Value);
        }
        
        [Test]
        public void CreateTransformedSubscriber_FieldMappingsHaveStandardTypeID_ReturnsSubscriberTransformedObject()
        {
            // Arrange
            var myServFeat = new ServiceFeature
            {
                SFName = UADFeatures.Special_Split.ToString()
            };
            var allTransformSplit = CreateAllTransformSplitObject();
            var myRow = new StringDictionary();
            var originalRowNumber = 1;
            var transformedRowNumber = 1;
            var qSourceId = 1;
            var emailStatusId = 1;
            var subOriginalDictionary = new Dictionary<int, Guid>();
            subOriginalDictionary.Add(1, Guid.NewGuid());
            var ahdGroups = new HashSet<AdHocDimensionGroup>();
            var splitTranFieldMappings = CreateTransformationFieldMap();
            var splitTrans = CreateTransformationObject();
            var demoUpdateCodes = CreateCodeObject();
            var admsLog = new AdmsLog
            {
                SourceFileId = 1,
                ProcessCode = Pubcode
            };
            SourceFile sourceFile = CreateSourceFileControl(myRow, StandarTypeId, 0);

            var dft = new FrameworkUAD_Lookup.Enums.FileTypes();

            var paramenters = new object[]
            {
                myServFeat,
                allTransformSplit ,
                myRow,
                originalRowNumber,
                transformedRowNumber,
                subOriginalDictionary,
                ahdGroups,
                splitTranFieldMappings,
                splitTrans,
                qSourceId,
                emailStatusId,
                demoUpdateCodes,
                admsLog,
                sourceFile,
                dft
            };

            // Act
            var result = _privateObject.Invoke(
                CreateTransformedSubscriber,
                paramenters) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void CreateTransformedSubscriber_FieldMappingsHaveDemoDateTypeID_ReturnsSubscriberTransformedObject()
        {
            // Arrange
            var myServFeat = new ServiceFeature();
            var allTransformSplit = new HashSet<TransformSplit>();
            var originalRowNumber = 1;
            var transformedRowNumber = 1;
            var subOriginalDictionary = new Dictionary<int, Guid>();
            subOriginalDictionary.Add(1, Guid.NewGuid());
            var splitTranFieldMappings = new HashSet<TransformationFieldMap>();
            var splitTrans = new HashSet<Transformation>();
            var qSourceId = 1;
            var emailStatusId = 1;
            var demoUpdateCodes = CreateCodeObject();
            var admsLog = new AdmsLog
            {
                SourceFileId = 1,
                ProcessCode = Pubcode
            };
            var ahdGroups = CreateAdHocDimensionGroupObject();
            var myRow = new StringDictionary();
            myRow.Add(string.Empty, "91118");
            myRow.Add(MAFFieldStandardFields.QUALIFICATIONDATE.ToString(), "91118");
            var sourceFile = CreateDummySourceFile();

            var dft = new FrameworkUAD_Lookup.Enums.FileTypes();
            var paramenters = CreateParamenters(myServFeat,
                allTransformSplit,
                originalRowNumber,
                transformedRowNumber,
                subOriginalDictionary,
                splitTranFieldMappings,
                splitTrans,
                qSourceId,
                emailStatusId,
                demoUpdateCodes,
                admsLog,
                ahdGroups,
                myRow,
                sourceFile,
                dft);

            // Act
            var result = _privateObject.Invoke(
                 CreateTransformedSubscriber,
                 paramenters) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void CreateTransformedSubscriber_FieldMappingsHavingDemoTypeIdAndOverwriteValue_ReturnsSubscriberTransformedObject()
        {
            // Arrange
            var myServFeat = new ServiceFeature
            {
                SFName = UADFeatures.Special_Split.ToString()
            };
            var allTransformSplit = CreateAllTransformSplitObject();
            var myRow = new StringDictionary();
            var originalRowNumber = 1;
            var transformedRowNumber = 1;
            var subOriginalDictionary = new Dictionary<int, Guid>();
            subOriginalDictionary.Add(1, Guid.NewGuid());
            var ahdGroups = new HashSet<AdHocDimensionGroup>();
            var splitTranFieldMappings = CreateTransformationFieldMap();
            var splitTrans = CreateTransformationObject();
            var qSourceId = 1;
            var emailStatusId = 1;
            var demoUpdateCodes = CreateCodeObject();
            var admsLog = new AdmsLog
            {
                SourceFileId = 1,
                ProcessCode = Pubcode
            };
            var sourceFile = CreateSourceFileControl(myRow, 1, 3);

            var dft = FrameworkUAD_Lookup.Enums.FileTypes.QuickFill;

            var paramenters = CreateParamenters(myServFeat,
                allTransformSplit,
                originalRowNumber,
                transformedRowNumber,
                subOriginalDictionary,
                splitTranFieldMappings,
                splitTrans,
                qSourceId,
                emailStatusId,
                demoUpdateCodes,
                admsLog,
                ahdGroups,
                myRow,
                sourceFile,
                dft);

            // Act
            var result = _privateObject.Invoke(
                 CreateTransformedSubscriber,
                 paramenters) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void CreateTransformedSubscriber_UADFeaturesIsConsensusDimension_ReturnsSubscriberTransformedObject()
        {
            // Arrange
            var myServFeat = new ServiceFeature
            {
                SFName = UADFeatures.Consensus_Dimension.ToString()
            };
            var allTransformSplit = CreateAllTransformSplitObject();
            var myRow = new StringDictionary();
            var originalRowNumber = 1;
            var transformedRowNumber = 1;
            var subOriginalDictionary = new Dictionary<int, Guid>();
            subOriginalDictionary.Add(1, Guid.NewGuid());
            var ahdGroups = new HashSet<AdHocDimensionGroup>();
            var splitTranFieldMappings = CreateTransformationFieldMap();
            var splitTrans = CreateTransformationObject();
            var qSourceId = 1;
            var emailStatusId = 1;
            var demoUpdateCodes = CreateCodeObject();
            var admsLog = new AdmsLog
            {
                SourceFileId = 1,
                ProcessCode = Pubcode
            };
            var sourceFile = CreateSourceFileControl(myRow, 1, 3);

            var dft = FrameworkUAD_Lookup.Enums.FileTypes.QuickFill;

            var paramenters = CreateParamenters(myServFeat,
                allTransformSplit,
                originalRowNumber,
                transformedRowNumber,
                subOriginalDictionary,
                splitTranFieldMappings,
                splitTrans,
                qSourceId,
                emailStatusId,
                demoUpdateCodes,
                admsLog,
                ahdGroups,
                myRow,
                sourceFile,
                dft);

            // Act
            var result = _privateObject.Invoke(
                 CreateTransformedSubscriber,
                 paramenters) as SubscriberTransformed;

            // Assert
            result.ShouldNotBeNull();
        }

        private HashSet<AdHocDimensionGroup> CreateAdHocDimensionGroupObject()
        {
            return new HashSet<AdHocDimensionGroup>
            {
                new AdHocDimensionGroup
                {
                    CreatedDimension=MAFFieldStandardFields.QUALIFICATIONDATE.ToString()
                },
                new AdHocDimensionGroup
                {
                   CreatedDimension=string.Empty
                }
             };
        }

        private SourceFile CreateDummySourceFile()
        {
            return new SourceFile
            {
                QDateFormat = QdateFormat,
                FieldMappings = new HashSet<FieldMapping>
                {
                    new FieldMapping
                    {
                        MAFField ="Dummy",
                        IncomingField ="Dummy",
                        FieldMappingTypeID = DemoDateTypeId,
                        IsNonFileColumn = true,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                        FieldMappingID=0
                    },
                    new FieldMapping
                    {
                        MAFField ="_DEMODATE",
                        IncomingField = "_DEMODATE",
                        FieldMappingTypeID = DemoDateTypeId,
                        IsNonFileColumn = true,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                        FieldMappingID=1
                    },
                     new FieldMapping
                    {
                        MAFField =MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                        IncomingField = MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                        FieldMappingTypeID = DemoDateTypeId,
                        IsNonFileColumn = true,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                        FieldMappingID=2
                    }
                }
            };
        }

        private List<FrameworkUAD_Lookup.Entity.Code> CreateCodeObject()
        {
            return new List<FrameworkUAD_Lookup.Entity.Code>
            {
                new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeName = "Append",
                    CodeId = 1
                },
                new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeName = "Replace",
                    CodeId = 2
                },
                 new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeName = "Overwrite",
                    CodeId = 3
                }
            };
        }

        private SourceFile CreateSourceFileControl(StringDictionary stringDictionary, int fieldMappingTypeID, int demographicUpdateCodeId)
        {
            var record = new SubscriberTransformed();
            record.MailPermission = true;
            record.FaxPermission = true;
            record.PhonePermission = true;
            record.OtherProductsPermission = true;
            record.ThirdPartyPermission = true;
            record.EmailRenewPermission = true;
            record.TextPermission = true;
            record.SubGenSubscriptionRenewDate = DateTime.Now;
            record.SubGenSubscriptionExpireDate = DateTime.Now.AddDays(10);
            record.SubGenSubscriptionLastQualifiedDate = DateTime.Now;

            var filedMapping = new HashSet<FieldMapping>();
            PropertyInfo[] properties = typeof(SubscriberTransformed).GetProperties();
            var i = 0;
            foreach (PropertyInfo property in properties)
            {
                if (stringDictionary[property.Name] == null)
                {
                    stringDictionary.Add(property.Name, property.GetValue(record, null).ToString());
                }
                if (!filedMapping.Any(x => x.MAFField.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    filedMapping.Add(new FieldMapping
                    {
                        MAFField = property.Name,
                        IncomingField = property.Name,
                        FieldMappingTypeID = fieldMappingTypeID,
                        IsNonFileColumn = true,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                        FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                        FieldMappingID = i,
                        DemographicUpdateCodeId = demographicUpdateCodeId
                    });

                    if (demographicUpdateCodeId == DemographicUpdateCodeId)
                    {
                        filedMapping.Add(new FieldMapping
                        {
                            MAFField = property.Name + DemoDate,
                            IncomingField = property.Name,
                            FieldMappingTypeID = fieldMappingTypeID,
                            IsNonFileColumn = true,
                            ColumnOrder = 1,
                            DataType = string.Empty,
                            PreviewData = string.Empty,
                            FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                            FieldMappingID = i,
                            DemographicUpdateCodeId = demographicUpdateCodeId
                        });
                    }
                    i++;
                }
            }

            foreach (MAFFieldStandardFields mAFFieldStandardFields in Enum.GetValues(typeof(MAFFieldStandardFields)))
            {
                if (!filedMapping.Any(x => x.MAFField.Equals(mAFFieldStandardFields.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    filedMapping.Add(new FieldMapping
                    {
                        MAFField = mAFFieldStandardFields.ToString(),
                        IncomingField = mAFFieldStandardFields.ToString(),
                        FieldMappingTypeID = fieldMappingTypeID,
                        IsNonFileColumn = true,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                        FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                        FieldMappingID = i,
                        DemographicUpdateCodeId = demographicUpdateCodeId
                    });
                    if (demographicUpdateCodeId == DemographicUpdateCodeId)
                    {
                        filedMapping.Add(new FieldMapping
                        {
                            MAFField = mAFFieldStandardFields.ToString() + DemoDate,
                            IncomingField = mAFFieldStandardFields.ToString(),
                            FieldMappingTypeID = fieldMappingTypeID,
                            IsNonFileColumn = true,
                            ColumnOrder = 1,
                            DataType = string.Empty,
                            PreviewData = string.Empty,
                            FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                            FieldMappingID = i,
                            DemographicUpdateCodeId = demographicUpdateCodeId
                        });
                    }
                    i++;
                }
                var mAFFieldStandardFieldsAsString = mAFFieldStandardFields.ToString();
                if (stringDictionary[mAFFieldStandardFieldsAsString] == null)
                {
                    stringDictionary.Add(mAFFieldStandardFieldsAsString, mAFFieldStandardFieldsAsString);
                }
            }
            if (!filedMapping.Any(x => x.MAFField.Equals(QualificationDate, StringComparison.CurrentCultureIgnoreCase)))
            {
                AddFieldMapping(stringDictionary, fieldMappingTypeID, demographicUpdateCodeId, filedMapping, i, QualificationDate);
                i++;
            }
            if (!filedMapping.Any(x => x.MAFField.Equals(DqmProcessDate, StringComparison.CurrentCultureIgnoreCase)))
            {
                AddFieldMapping(stringDictionary, fieldMappingTypeID, demographicUpdateCodeId, filedMapping, i, DqmProcessDate);
                i++;
            }
            if (!filedMapping.Any(x => x.MAFField.Equals(IsupdatedInlive, StringComparison.CurrentCultureIgnoreCase)))
            {
                filedMapping.Add(new FieldMapping
                {
                    MAFField = IsupdatedInlive,
                    IncomingField = IsupdatedInlive,
                    FieldMappingTypeID = fieldMappingTypeID,
                    IsNonFileColumn = true,
                    ColumnOrder = 1,
                    DataType = string.Empty,
                    PreviewData = string.Empty,
                    FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                    DemographicUpdateCodeId = demographicUpdateCodeId
                });
                if (stringDictionary[IsupdatedInlive] == null)
                {
                    stringDictionary.Add(IsupdatedInlive, bool.TrueString.ToLower());
                }
            }
            if (!filedMapping.Any(x => x.MAFField.Equals(UpdateInLiveDate, StringComparison.CurrentCultureIgnoreCase)))
            {
                AddFieldMapping(stringDictionary, fieldMappingTypeID, demographicUpdateCodeId, filedMapping, i, UpdateInLiveDate);
                i++;
            }
            return new SourceFile
            {
                QDateFormat = QdateFormat,
                FieldMappings = filedMapping
            };
        }

        private void AddFieldMapping(StringDictionary stringDictionary, int fieldMappingTypeID, int demographicUpdateCodeId, HashSet<FieldMapping> filedMapping, int i, string mAFField)
        {
            filedMapping.Add(new FieldMapping
            {
                MAFField = mAFField,
                IncomingField = mAFField,
                FieldMappingTypeID = fieldMappingTypeID,
                IsNonFileColumn = true,
                ColumnOrder = 1,
                DataType = string.Empty,
                PreviewData = string.Empty,
                FieldMultiMappings = CreateFieldMultiMap(fieldMappingTypeID),
                FieldMappingID = i,
                DemographicUpdateCodeId = demographicUpdateCodeId
            });
            if (stringDictionary[mAFField] == null)
            {
                stringDictionary.Add(mAFField, DateTime.Now.ToString());
            }
        }

        private DataTable CreateDataTableString()
        {
            var dataTable = new DataTable();
            var dataColumn = new DataColumn("PubID", typeof(int));
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("PubCode", typeof(String));
            dataTable.Columns.Add(dataColumn);
            dataTable.Rows.Add(new Object[] { 1, "1992" });
            dataTable.Rows.Add(new Object[] { 2, ColumnDelimiter.comma.ToString() });
            dataTable.Rows.Add(new Object[] { 3, ColumnDelimiter.semicolon.ToString() });
            dataTable.Rows.Add(new Object[] { 4, ColumnDelimiter.tab.ToString() });
            dataTable.Rows.Add(new Object[] { 5, ColumnDelimiter.tild.ToString() });
            dataTable.Rows.Add(new Object[] { 6, ColumnDelimiter.pipe.ToString() });
            return dataTable;
        }

        private void CreateAppSettingsObject()
        {
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(IsDemo, bool.TrueString.ToString().ToLower());
            nameValueCollection.Add(IsNetworkDeployed, bool.FalseString.ToString().ToLower());
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                return nameValueCollection;
            };
        }

        private HashSet<FieldMultiMap> CreateFieldMultiMap(int fieldMappingTypeID)
        {
            var fieldMultiMapHashSet = new HashSet<FieldMultiMap>();
            var properties = typeof(SubscriberTransformed).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!fieldMultiMapHashSet.Any(x => x.MAFField.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    fieldMultiMapHashSet.Add(new FieldMultiMap
                    {
                        MAFField = property.Name,
                        FieldMappingTypeID = fieldMappingTypeID,
                        ColumnOrder = 1,
                        DataType = string.Empty,
                        PreviewData = string.Empty,
                    });
                }
            }
            return fieldMultiMapHashSet;
        }

        private HashSet<Transformation> CreateTransformationObject()
        {
            var transformation = new HashSet<Transformation>();
            for (int i = 0; i < 7; i++)
            {
                transformation.Add(new Transformation
                {
                    TransformationID = i,
                    TransformationName = string.Empty,
                    TransformationDescription = string.Empty
                });
            }
            return transformation;
        }

        private HashSet<TransformationFieldMap> CreateTransformationFieldMap()
        {
            var transformation = new HashSet<TransformationFieldMap>();
            for (int i = 0; i < 7; i++)
            {
                transformation.Add(new TransformationFieldMap
                {
                    FieldMappingID = i,
                    TransformationID = i
                });
            }
            return transformation;
        }

        private HashSet<TransformSplit> CreateAllTransformSplitObject()
        {
            return new HashSet<TransformSplit>()
            {
                new TransformSplit
                {
                    TransformationID = 0,
                    Delimiter = ColumnDelimiter.colon.ToString()
                },
                 new TransformSplit
                {
                    TransformationID = 1,
                    Delimiter = ColumnDelimiter.comma.ToString()
                },
                 new TransformSplit
                {
                    TransformationID = 2,
                    Delimiter = ColumnDelimiter.semicolon.ToString()
                },
                 new TransformSplit
                {
                    TransformationID = 3,
                    Delimiter = ColumnDelimiter.tab.ToString()
                },
                 new TransformSplit
                {
                    TransformationID = 4,
                    Delimiter = ColumnDelimiter.tild.ToString()
                },
                 new TransformSplit
                {
                    TransformationID = 5,
                    Delimiter = ColumnDelimiter.pipe.ToString()
                },
                new TransformSplit
                {
                    TransformationID = 6,
                    Delimiter = ColumnDelimiter.pipe.ToString()
                }
            };
        }

        private static object[] CreateParamenters(
            KMPlatform.Entity.ServiceFeature myServFeat,
            HashSet<TransformSplit> allTransformSplit,
            int originalRowNumber,
            int transformedRowNumber,
            Dictionary<int, Guid> subOriginalDictionary,
            HashSet<TransformationFieldMap> splitTranFieldMappings,
            HashSet<Transformation> splitTrans,
            int qSourceId,
            int emailStatusId,
            List<FrameworkUAD_Lookup.Entity.Code> demoUpdateCodes,
            AdmsLog admsLog, HashSet<AdHocDimensionGroup> ahdGroups,
            StringDictionary myRow,
            SourceFile sourceFile,
            FrameworkUAD_Lookup.Enums.FileTypes fileType)
        {
            return new object[]
            {
                    myServFeat,
                    allTransformSplit ,
                    myRow,
                    originalRowNumber,
                    transformedRowNumber,
                    subOriginalDictionary,
                    ahdGroups,
                    splitTranFieldMappings,
                    splitTrans,
                    qSourceId,
                    emailStatusId,
                    demoUpdateCodes,
                    admsLog,
                    sourceFile,
                    fileType
            };
        }

    }
}
