using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using ADMS.Services.Fakes;
using AMS_Operations;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Assert = NUnit.Framework.Assert;

namespace UAS.UnitTests.AMS_Operations.FileValidator_cs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class CreateSubscriberTest : Fakes
    {
        private const int StandarTypeId = 0;
        private const int DemoTypeId = 1;
        private const int DemoDateTypeId = 2;
        private const string CreateSubscriberMethodName = "CreateSubscriber";
        private const string SomeDateText = "2018/1/1";

        private readonly List<AdHocDimensionGroup> _ahdGroups = new List<AdHocDimensionGroup>();
        private readonly int _pubCodeId = 0;
        private readonly int _originalRowNumber = 0;
        private readonly int _transformedRowNumber = 0;

        private TestEntity _testEntity;
        private PrivateObject _privateObject;        
        private List<TransformSplit> _allTransformSplit = null;
        private SourceFile _sourceFile = new SourceFile { FieldMappings = new HashSet<FieldMapping>() };

        [SetUp]
        public void Setup()
        {
            _testEntity = new TestEntity();
            SetupFakes(_testEntity.Mocks);

            _privateObject = new PrivateObject(new FileValidator());
            _privateObject.SetField("standarTypeID", StandarTypeId);
            _privateObject.SetField("demoTypeID", DemoTypeId);
            _privateObject.SetField("serviceFeature", new KMPlatform.Entity.ServiceFeature());
            _privateObject.SetField("sourceFile", _sourceFile);

            ShimServiceBase.AllInstances.clientPubCodesGet = _ =>
                new Dictionary<int, string>()
                {
                    {1, "Test_pubcode"}
                };
        }

        [Test]
        public void CreateSubscriber_FieldMappingsEmpty_ReturnsOriginalSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" }
            };

            // Act
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<SubscriberOriginal>(result);
        }

        [Test]
        public void CreateSubscriber_CheckSkips_ReternsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { "Test", "Test" },
                { "Demographics", "test" }
            };
            
            _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "demoDate",
                string.Empty,
                DemoDateTypeId));
            _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Test",
                "firstname",
                StandarTypeId));
            _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Tst",
                "zipcode",
                StandarTypeId));
            _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Tst",
                "zipcode",
                DemoTypeId));
            _sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Demographics",
                "demogr1",
                DemoTypeId));

            // Act
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups) as SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.FName);
            Assert.AreEqual("", result.Zip);
            Assert.IsNotNull(result.DemographicOriginalList);
            Assert.AreEqual("Test", result.DemographicOriginalList.First().Value);
        }


        [Test]
        public void CreateSubscriber_IterateOverStandardFields_ReternsSubscriber()
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
                "occupation", "subscriptionstatusid", "subsrcid", "website"
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
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups) as SubscriberOriginal;

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
            Assert.AreEqual(new DateTime(1900, 1, 1), result.QDate);
            Assert.AreEqual(0, result.QSourceID);
            Assert.AreEqual("Test_regcode", result.RegCode);
            Assert.AreEqual("Test_verify", result.Verified);
            Assert.AreEqual(-1, result.SubscriberOriginalID);
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
            Assert.AreEqual(0, result.Score);
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
        }


        [Test]
        public void CreateSubscriber_IterateOverMultimappingStandardFields_ReternsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };            

            var standardFields = new[]
            {
                "pubcode", "sequenceid", "firstname", "lastname", "title", "company", "address1", "address2",
                "city",
                "regioncode", "zipcode", "plus4", "forzip",
                "county", "country", "countryid", "phone", "fax", "email", "pubcategoryid", "pubtransactionid",
                "pubtransactiondate", "qualificationdate",
                "pubqsourceid", "regcode", "verify", "subscribersourcecode", "origssrc", "par3cid",
                "mailpermission",
                "faxpermission", "phonepermission",
                "otherproductspermission", "thirdpartypermission", "emailrenewpermission", "textpermission",
                "source",
                "priority", "sic", "siccode",
                "gender", "address3", "home_work_address", "demo7", "mobile", "latitude", "longitude", "score",
                "emailstatusid", "importrownumber",
                "isactive", "processcode", "externalkeyid", "accountnumber", "emailid", "copies", "graceissues",
                "iscomp", "ispaid", "issubscribed",
                "occupation", "subscriptionstatusid", "subsrcid", "website"
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
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups) as SubscriberOriginal;

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
            Assert.AreEqual(new DateTime(1900, 1, 1), result.QDate);
            Assert.AreEqual(0, result.QSourceID);
            Assert.AreEqual("Test_regcode", result.RegCode);
            Assert.AreEqual("Test_verify", result.Verified);
            Assert.AreEqual(-1, result.SubscriberOriginalID);
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
            Assert.AreEqual(0, result.Score);
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
        }

        [Test]
        public void CreateSubscriber_IterateOverNonNumericFields_ReternsSubscriber()
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
                "otherproductspermission", "thirdpartypermission", "emailrenewpermission", "textpermission"
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
                myRow.Add(field, SomeDateText);
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in boolFields)
            {
                myRow.Add(field, bool.TrueString);
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    StandarTypeId);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, StandarTypeId));
                _sourceFile.FieldMappings.Add(mapping);
            }            

            // Act
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups) as SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(999, result.Sequence);
            Assert.AreEqual(999, result.CountryID);
            Assert.AreEqual(999, result.CategoryID);
            Assert.AreEqual(999, result.TransactionID);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.TransactionDate);
            Assert.AreEqual(new DateTime(2018, 1, 1), result.QDate);
            Assert.AreEqual(999, result.QSourceID);
            Assert.AreEqual(-1, result.SubscriberOriginalID);
            Assert.IsTrue(result.MailPermission);
            Assert.IsTrue(result.FaxPermission);
            Assert.IsTrue(result.PhonePermission);
            Assert.IsTrue(result.OtherProductsPermission);
            Assert.IsTrue(result.ThirdPartyPermission);
            Assert.IsTrue(result.EmailRenewPermission);
            Assert.IsTrue(result.TextPermission);
            Assert.AreEqual(999, result.Latitude);
            Assert.AreEqual(999, result.Longitude);
            Assert.AreEqual(999, result.Score);
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
        }

        [Test]
        public void CreateSubscriber_CheckDemographicsInMultimapping_ReternsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { "Test", "Test" },
                { "Demographics", "test" }
            };            
            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test", "firstname", StandarTypeId);
            var multimapping = CreateSubscriberTestHelper.CreateMultiMapping("Test", DemoTypeId);
            mapping.FieldMultiMappings.Add(multimapping);
            _sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = _privateObject.Invoke(CreateSubscriberMethodName, myRow, _pubCodeId, _ahdGroups) as SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.FName);
            Assert.IsNotNull(result.DemographicOriginalList);
            Assert.AreEqual("Test", result.DemographicOriginalList.First().Value);
        }
    }
}
