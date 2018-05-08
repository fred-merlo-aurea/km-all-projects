using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ADMS.Services.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Assert = NUnit.Framework.Assert;
using Enums = FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CreateSubscriberTest : Fakes
    {
        private const int standarTypeID = 0;
        private const int demoTypeID = 1;
        private const int ignoredTypeID = 2;
        private const int demoDateTypeID = 4;
        private const int kmTransformTypeID = 5;
        private const string CreateSubscriberMethodName = "CreateSubscriber";
        private const string SomeDateText = "2018/1/1";

        private readonly AdmsLog admsLog = new AdmsLog();
        private readonly HashSet<AdHocDimensionGroup> ahdGroups = new HashSet<AdHocDimensionGroup>();

        private TestEntity testEntity;
        private PrivateObject privateObject;
        private Enums.FileTypes dft = Enums.FileTypes.Other;

        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);

            privateObject = new PrivateObject(new global::ADMS.Services.Validator.Validator());

            privateObject.SetField("demoDateTypeID", demoDateTypeID);
            privateObject.SetField("standarTypeID", standarTypeID);
            privateObject.SetField("demoTypeID", demoTypeID);
            privateObject.SetField("ignoredTypeID", ignoredTypeID);
            privateObject.SetField("kmTransformTypeID", kmTransformTypeID);

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
            var sourceFile = new SourceFile();
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" }
            };

            // Act
            var result = privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<SubscriberOriginal>(result);
        }

        [Test]
        public void CreateSubscriber_CheckSkips_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { "Test", "Test" },
                { "Demographics", "test" }
            };
            var sourceFile = new SourceFile
            {
                FieldMappings = new HashSet<FieldMapping>()
            };
            sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "demoDate",
                string.Empty,
                demoDateTypeID));
            sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Test",
                "firstname",
                standarTypeID));
            sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Tst",
                "zipcode",
                standarTypeID));
            sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Tst",
                "zipcode",
                demoTypeID));
            sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                "Demographics",
                "demogr1",
                demoTypeID));

            // Act
            var result = privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft) as SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.FName);
            Assert.AreEqual("", result.Zip);
            Assert.IsNotNull(result.DemographicOriginalList);
            Assert.AreEqual("test", result.DemographicOriginalList.First().Value);
        }

        [Test]
        public void CreateSubscriber_IterateOverStandardFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };
            var sourceFile = new SourceFile
            {
                FieldMappings = new HashSet<FieldMapping>()
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
                sourceFile.FieldMappings.Add(CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    standarTypeID));
            }

            // Act
            var result =
                privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft) as
                    SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test_pubcode",result.PubCode);
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
            Assert.AreEqual(new DateTime(1900,1,1), result.TransactionDate);
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
        public void CreateSubscriber_IterateOverMultimappingStandardFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };
            var sourceFile = new SourceFile
            {
                FieldMappings = new HashSet<FieldMapping>()
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
                    standarTypeID);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, standarTypeID));
                sourceFile.FieldMappings.Add(mapping);
            }

            // Act
            var result =
                privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft) as
                    SubscriberOriginal;

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
        public void CreateSubscriber_IterateOverNonNumericFields_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                {"OriginalImportRow", "100"},
            };
            var sourceFile = new SourceFile {FieldMappings = new HashSet<FieldMapping>()};
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
                    standarTypeID);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, standarTypeID));
                sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in phoneFields)
            {
                myRow.Add(field, "+78001002030");
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    standarTypeID);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, standarTypeID));
                sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in dateFields)
            {
                myRow.Add(field, SomeDateText);
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    standarTypeID);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, standarTypeID));
                sourceFile.FieldMappings.Add(mapping);
            }

            foreach (var field in boolFields)
            {
                myRow.Add(field, bool.TrueString);
                var mapping = CreateSubscriberTestHelper.CreateFieldMapping(
                    field,
                    field,
                    standarTypeID);
                mapping.FieldMultiMappings.Add(CreateSubscriberTestHelper.CreateMultiMapping(field, standarTypeID));
                sourceFile.FieldMappings.Add(mapping);
            }

            var dft = Enums.FileTypes.Other;

            // Act
            var result =
                privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft) as
                    SubscriberOriginal;

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
        public void CreateSubscriber_CheckDemographicsInMultimapping_ReturnsSubscriber()
        {
            // Arrange
            var myRow = new StringDictionary()
            {
                { "OriginalImportRow", "100" },
                { "Test", "Test" },
                { "Demographics", "test" }
            };
            var sourceFile = new SourceFile
            {
                FieldMappings = new HashSet<FieldMapping>()
            };
            var mapping = CreateSubscriberTestHelper.CreateFieldMapping("Test","firstname",standarTypeID);
            var multimapping = CreateSubscriberTestHelper.CreateMultiMapping("Test", demoTypeID);
            mapping.FieldMultiMappings.Add(multimapping);
            sourceFile.FieldMappings.Add(mapping);

            // Act
            var result = privateObject.Invoke(CreateSubscriberMethodName, myRow, ahdGroups, admsLog, sourceFile, dft) as SubscriberOriginal;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.FName);
            Assert.IsNotNull(result.DemographicOriginalList);
            Assert.AreEqual("Test", result.DemographicOriginalList.First().Value);
        }
    }
}
