using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Configuration.Fakes;
using System.Collections.Specialized;
using AMS_Operations;
using FrameworkSubGen.Entity;
using FrameworkSubGen.Object;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkSubGen.BusinessLogic.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UADEntity = FrameworkUAD.Entity;
using UASEntity = FrameworkUAS.Entity;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    /// Unit Test for <see cref="Operations.Convert_ImportSubscriber_to_SubscriberTrans"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class OperationTest
    {
        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithGerenalParamteres_ReturnsEmptyList()
        {
            // Arrange
            var importSubscriberList = GetTestImportSubscriber();
            var account = new Account();
            var parametes = new object[]
            {
                importSubscriberList,
                account
            };
            SetUpFakes();

            var publications = _operations.GetFieldValue("publications") as List<Publication>;
            var importErrors = new List<ImportError>();
            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) =>
                {
                    var error = new ImportError()
                    {
                        DataRow = row,
                        Method = method,
                        ErrorMsg = msg,
                        ImportFile = file,
                        SubGenImportSubscriber = sub,
                        UADSubscriberTransformed = st
                    };
                    importErrors.Add(error);
                },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            importErrors.ShouldNotBeNull();
            importErrors.ShouldNotBeEmpty();
            importErrors.Count.ShouldBe(1);
            importErrors.ShouldContain(x => x.ErrorMsg.Contains("Record did not meet minimum requirements"));
            importErrors.ShouldContain(x =>
                  x.ErrorMsg.
                  Contains($"SubGenSubscriberID > 0 - SubGenSubscriberID value - {importSubscriberList[0].SystemSubscriberID}"));
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithMailingAddressUS_ReturnsSubscriberTransformedList()
        {
            // Arrange
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressFirstName = SampleFirstName;
            importSubscriberList[0].MailingAddressLastName = SampleLastName;
            importSubscriberList[0].MailingAddressCompany = SampleCompany;
            importSubscriberList[0].MailingAddressZip = "1234";
            importSubscriberList[0].SubscriptionType = SubscriptionType.Digital.ToString();
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionCreatedDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.NQO.ToString();
            importSubscriberList[0].TransactionID = -1;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.OTH.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringNo;
                }
            }
            SetUpFakes(mapperCustomField: CustomMapperField);
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) => { },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(SampleFirstName);
            result[0].LName.ShouldBe(SampleLastName);
            result[0].Company.ShouldBe(SampleCompany);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(CountryUS);
            result[0].CategoryID.ShouldBe(32);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(1);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("B");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(2);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == OtherDimensionField);
            result[0].DemographicTransformedList.ShouldContain(x => x.ResponseOther == SampleDimensionResponse);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == CustomMapperField.ToUpper());
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithMailingAddressTitle_ReturnsSubscriberTransformed()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressTitle = SampleTitle;
            importSubscriberList[0].MailingAddressCompany = SampleCompany;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].SubscriptionType = SubscriptionType.Print.ToString();
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PI.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.CDW.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            importSubscriberList[0].Dimensions.Dimensions.RemoveAll(x => x.DimensionField.StartsWith("other"));
            importSubscriberList[0].Dimensions.Dimensions.Add(
                new ImportDimensionDetail { DimensionField = "same-demo", DimensionValue = SampleDimensionResponse });
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) => { },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(string.Empty);
            result[0].LName.ShouldBe(string.Empty);
            result[0].Title.ShouldBe(SampleTitle);
            result[0].Company.ShouldBe(SampleCompany);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(CountryUS);
            result[0].CategoryID.ShouldBe(20);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(60);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("A");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(2);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == OtherDimensionField);
            result[0].DemographicTransformedList.ShouldContain(x => x.PubID == 1);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == CustomMapperField.ToUpper());
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithBillingAddressFirstName_ReturnsSubscriberTransformed()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField, isMultiValue: true, multiValueResponse: "12345-67 89");
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].BillingAddressFirstName = SampleFirstName;
            importSubscriberList[0].BillingAddressLastName = SampleLastName;
            importSubscriberList[0].MailingAddressZip = "123";
            importSubscriberList[0].SubscriptionType = SubscriptionType.Print.ToString();
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PB.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.CDI.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            importSubscriberList[0].Dimensions.Dimensions.RemoveAll(x => x.DimensionField.StartsWith("other"));
            importSubscriberList[0].Dimensions.Dimensions.Add(
                new ImportDimensionDetail { DimensionField = "same-demo1", DimensionValue = SampleDimensionResponse });
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) => { },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(SampleFirstName);
            result[0].LName.ShouldBe(SampleLastName);
            result[0].Title.ShouldBe(SampleTitle);
            result[0].Company.ShouldBe(string.Empty);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(CountryUS);
            result[0].CategoryID.ShouldBe(21);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(60);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("A");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(1);
            result[0].DemographicTransformedList.ShouldContain(x => x.Value == SampleDimensionResponse);
            result[0].DemographicTransformedList.ShouldContain(x => x.PubID == 1);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == CustomMapperField);
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithMailingCountryCanada_ReturnsSubscriberTransformed()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressCountry = CountryCanada;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].MailingAddressState = StateCanada;
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PM.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.PDI.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            importSubscriberList[0].Dimensions.Dimensions.RemoveAll(x => x.DimensionField.StartsWith("other"));
            importSubscriberList[0].Dimensions.Dimensions.Add(
                new ImportDimensionDetail { DimensionField = "same-demo", DimensionValue = SampleDimensionResponse });
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) => { },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(string.Empty);
            result[0].LName.ShouldBe(string.Empty);
            result[0].Title.ShouldBe(SampleTitle);
            result[0].Company.ShouldBe(string.Empty);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(CountryCanada);
            result[0].State.ShouldBe(StateCanada);
            result[0].CategoryID.ShouldBe(27);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(60);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("C");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(2);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == OtherDimensionField);
            result[0].DemographicTransformedList.ShouldContain(x => x.PubID == 1);
            result[0].DemographicTransformedList.ShouldContain(x => 
                    x.MAFField == CustomMapperField.ToUpper() && x.Value == "xxNVxx");
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithMailingCountryOther_ReturnsSubscriberTransformed()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressCountry = CountryBrazil;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].MailingAddressState = StateBrazil;
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PQ.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.PDT.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            importSubscriberList[0].Dimensions.Dimensions.RemoveAll(x => x.DimensionField.StartsWith("other"));
            importSubscriberList[0].Dimensions.Dimensions.Add(
                new ImportDimensionDetail { DimensionField = "same-demo", DimensionValue = SampleDimensionResponse });
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) => { },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(string.Empty);
            result[0].LName.ShouldBe(string.Empty);
            result[0].Title.ShouldBe(SampleTitle);
            result[0].Company.ShouldBe(string.Empty);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(CountryBrazil);
            result[0].State.ShouldBe("FO");
            result[0].CategoryID.ShouldBe(30);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(60);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("C");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(2);
            result[0].DemographicTransformedList.ShouldContain(x => x.MAFField == OtherDimensionField);
            result[0].DemographicTransformedList.ShouldContain(x => x.PubID == 1);
            result[0].DemographicTransformedList.ShouldContain(x =>
                    x.MAFField == CustomMapperField.ToUpper() && x.Value == "xxNVxx");
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithMailingCountryNull_ThrowsExceptionandisLogged()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressCountry = null;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].MailingAddressState = StateBrazil;
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PM.ToString();
            importSubscriberList[0].PublicationName = SamplePublication;
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.PDW.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            _privateOperationsObj.SetField(PublicationsField, new List<Publication> { new Publication() });
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;
            var importErrors = new List<ImportError>();
            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) =>
                {
                    var error = new ImportError()
                    {
                        DataRow = row,
                        Method = method,
                        ErrorMsg = msg,
                        ImportFile = file,
                        SubGenImportSubscriber = sub,
                        UADSubscriberTransformed = st
                    };
                    importErrors.Add(error);
                },
                publications
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            importErrors.ShouldNotBeNull();
            importErrors.ShouldNotBeEmpty();
            importErrors.Count.ShouldBe(1);
            importErrors.ShouldContain(x => x.ErrorMsg.Contains("Sequence contains no matching element"));
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithDifferentPublication_ThrowsExceptionandisLogged()
        {
            // Arrange
            SetUpFakes(mapperCustomField: CustomMapperField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressCountry = null;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].MailingAddressState = StateBrazil;
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PQ.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.PDW.ToString();
            importSubscriberList[0].PublicationName = SamplePublication;
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            var account = new Account();
            var importErrors = new List<ImportError>();
            var converter = new ImportSubscriberConverter(
                s => { },
                (method, msg, row, file, sub, st) =>
                {
                    var error = new ImportError()
                    {
                        DataRow = row,
                        Method = method,
                        ErrorMsg = msg,
                        ImportFile = file,
                        SubGenImportSubscriber = sub,
                        UADSubscriberTransformed = st
                    };
                    importErrors.Add(error);
                },
                new List<Publication> { new Publication { publication_id = 1 } }
            );

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
            importErrors.ShouldNotBeNull();
            importErrors.ShouldNotBeEmpty();
            importErrors.Count.ShouldBe(1);
            importErrors.ShouldContain(x => x.ErrorMsg.Contains("Sequence contains no matching element"));
        }

        [Test]
        public void Convert_ImportSubscriber_to_SubscriberTrans_WithAuditRequestTypeCodeOther_ReturnsSubscriberTransformed()
        {
            // Arrange
            SetUpFakes(mapperCustomField: OtherDimensionField);
            var importSubscriberList = GetTestImportSubscriber();
            importSubscriberList[0].SubscriberAccountLastName = string.Empty;
            importSubscriberList[0].SubscriberAccountFirstName = string.Empty;
            importSubscriberList[0].MailingAddressCountry = null;
            importSubscriberList[0].MailingAddressZip = "123456";
            importSubscriberList[0].MailingAddressState = StateBrazil;
            importSubscriberList[0].SubscriptionLastQualifiedDate = Convert.ToDateTime("1/1/1900");
            importSubscriberList[0].SubscriptionExpireDate = DateTime.Now;
            importSubscriberList[0].AuditCategoryCode = AuditCategoryCode.PQ.ToString();
            importSubscriberList[0].TransactionID = 0;
            importSubscriberList[0].AuditRequestTypeCode = AuditRequestTypeCode.QDW.ToString();
            importSubscriberList[0].SystemSubscriberID = 99999;
            foreach (var dimension in importSubscriberList[0].Dimensions.Dimensions)
            {
                if (dimension.DimensionField.StartsWith(OtherDimensionField))
                {
                    dimension.DimensionValue = StringMaybe;
                }
            }
            var account = new Account();
            var publications = _operations.GetFieldValue("publications") as List<Publication>;

            var converter = new ImportSubscriberConverter(
                s=> { },
                (method, msg, row, file, sub, st) => {  },
                publications);

            // Act
            var result = converter.Convert_ImportSubscriber_to_SubscriberTrans(importSubscriberList, account);
            
            // Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
            result[0].FName.ShouldBe(string.Empty);
            result[0].LName.ShouldBe(string.Empty);
            result[0].Title.ShouldBe(SampleTitle);
            result[0].Company.ShouldBe(string.Empty);
            result[0].PubCode.ShouldBe(SampleKMPubCode);
            result[0].Country.ShouldBe(string.Empty);
            result[0].State.ShouldBe(StateBrazil);
            result[0].CategoryID.ShouldBe(30);
            result[0].QSourceID.ShouldBe(1);
            result[0].TransactionID.ShouldBe(60);
            result[0].Source.ShouldBe(SampleSource);
            result[0].Email.ShouldBe(SampleEmail);
            result[0].AccountNumber.ShouldBe(importSubscriberList[0].SystemSubscriberID.ToString());
            result[0].SubGenSubscriberID.ShouldBe(importSubscriberList[0].SystemSubscriberID);
            result[0].Demo7.ShouldBe("C");
            result[0].DemographicTransformedList.ShouldNotBeEmpty();
            result[0].DemographicTransformedList.Count.ShouldBe(1);
            result[0].DemographicTransformedList.ShouldContain(x => 
                x.MAFField == OtherDimensionField && x.PubID == 1 && x.Value == SampleDimensionResponse);
        }

        private List<ImportSubscriber> GetTestImportSubscriber()
        {
            var importSubscriberList = new List<ImportSubscriber>
            {
                new ImportSubscriber
                {
                    SubscriberAccountFirstName = SampleFirstName,
                    SubscriberAccountLastName = SampleLastName,
                    MailingAddressTitle = SampleTitle,
                    SubscriberEmail = SampleEmail,
                    SubscriberPhone = SamplePhone,
                    SubscriberSource = SampleSource,
                    MailingAddressCountry = CountryUS,
                    MailingAddressState = StateUS,
                    MailingAddressZip = "123-456",
                    SubscriptionType = SubscriptionType.Both.ToString(),
                    SubscriptionLastQualifiedDate = DateTime.Now,
                    PublicationID = 0,
                    Copies = 2,
                    SubscriptionRenewDate = DateTime.Now.AddMonths(6),
                    SubscriptionExpireDate = DateTime.Now.AddYears(1),
                    AuditCategoryCode = AuditCategoryCode.NQP.ToString(),
                    AuditRequestTypeCode = AuditRequestTypeCode.NRI.ToString(),
                    Dimensions = new ImportDimension
                    {
                        Dimensions = new List<ImportDimensionDetail>
                        {
                            new ImportDimensionDetail { DimensionField = DemoFields[0],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = DemoFields[1],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = DemoFields[2],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = DemoFields[3],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = DemoFields[4],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = DemoFields[5],DimensionValue = StringYes },
                            new ImportDimensionDetail { DimensionField = FaxDimensionField,DimensionValue = SampleDimensionResponse },
                            new ImportDimensionDetail { DimensionField = "other-demo",DimensionValue = SampleDimensionResponse },
                        }
                    },
                }
            };
            return importSubscriberList;
        }

        private void SetUpFakes(
            string mapperCustomField = OtherDimensionField,
            bool isMultiValue = false,
            string multiValueResponse = SampleDimensionResponse)
        {
            ShimClient.AllInstances.SelectInt32Boolean = (c, i, b) =>
            {
                return new Client { };
            };
            ShimProduct.AllInstances.SelectClientConnectionsBoolean = (c, b, f) =>
            {
                return new List<UADEntity.Product>
                {
                    new UADEntity.Product {PubCode = SampleKMPubCode,PubID = 1 }
                };
            };
            ShimCodeSheet.AllInstances.SelectClientConnections = (c, f) =>
            {
                return new List<UADEntity.CodeSheet>
                {
                    new UADEntity.CodeSheet{ ResponseGroup = OtherDimensionField, ResponseDesc = multiValueResponse,PubID = 1}
                };
            };
            ShimResponseGroup.AllInstances.SelectClientConnections = (c, f) =>
            {
                return new List<UADEntity.ResponseGroup>
                {
                    new UADEntity.ResponseGroup{
                        ResponseGroupName = mapperCustomField,
                        PubCode = SampleKMPubCode,
                        IsRequired = true,
                        IsMultipleValue = isMultiValue,
                        PubID = 1,
                    }
                };
            };
            ShimProductSubscriptionsExtensionMapper.AllInstances.SelectAllClientConnections = (c, f) =>
            {
                return new List<UADEntity.ProductSubscriptionsExtensionMapper>
                {
                    new UADEntity.ProductSubscriptionsExtensionMapper{ CustomField = mapperCustomField,PubID =1 }
                };
            };
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Boolean = (c, b, f) =>
            {
                return new List<UASEntity.AdHocDimensionGroup>
                {
                    new UASEntity.AdHocDimensionGroup{ IsActive = true ,CreatedDimension = OtherDimensionField}
                };
            };
            ShimSourceFile.AllInstances.SelectInt32StringBoolean = (c, i, s, b) =>
            {
                return null;
            };
            ShimSourceFile.AllInstances.SaveSourceFileBoolean = (s, b, f) => { return 1; };
            ShimCode.AllInstances.SelectEnumsCodeType = (e, c) =>
            {
                return new List<Code>
                {
                    new Code {CodeId = 1, CodeName = "Recurring"},
                    new Code {CodeId = 2,CodeName = "Paid Transaction"}
                };
            };
            ShimCode.AllInstances.SelectCodeValueEnumsCodeTypeString = (c, e, b) =>
            {
                return new Code { CodeId = 1 };
            };
            ShimService.AllInstances.SelectEnumsServicesBoolean = (s, e, f) =>
            {
                return new Service
                {
                    ServiceFeatures = new List<ServiceFeature>
                    {
                        new ServiceFeature {SFName = "File Import"}
                    }
                };
            };
            ShimPayment.AllInstances.SelectInt32DateTime = (p, i, d) =>
            {
                return new Payment { order_id = 1 };
            };
            ShimPayment.AllInstances.Update_STRecordIdentifierInt32Guid = (p, o, s) => { return true; };
            ShimTransactionCode.AllInstances.SelectTransactionCodeValueInt32 = (c, v) => new TransactionCode { TransactionCodeID = 1 };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var nameValueCollection = new NameValueCollection();
                nameValueCollection.Add(FileImportFolderKey, $"{TestContext.CurrentContext.TestDirectory}\\");
                return nameValueCollection;
            };
            ShimImportSubscriber.AllInstances.UpdateMergedToUADListOfImportSubscriber = (sub, subList) =>
            {
                _isImportSubscriberUpdated = true;
                _updatedimportSubscriberList = subList;
                return _isImportSubscriberUpdated;
            };
        }
    }
}
