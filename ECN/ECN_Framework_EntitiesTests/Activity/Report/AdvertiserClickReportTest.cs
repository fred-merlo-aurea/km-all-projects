using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class AdvertiserClickReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (AdvertiserClickReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var sendTime = Fixture.Create<DateTime>();
            var alias = Fixture.Create<string>();
            var linkURL = Fixture.Create<string>();
            var linkOwner = Fixture.Create<string>();
            var linkType = Fixture.Create<string>();
            var uniqueCount = Fixture.Create<int>();
            var totalCount = Fixture.Create<int>();

            // Act
            advertiserClickReport.BlastID = blastId;
            advertiserClickReport.EmailSubject = emailSubject;
            advertiserClickReport.SendTime = sendTime;
            advertiserClickReport.Alias = alias;
            advertiserClickReport.LinkURL = linkURL;
            advertiserClickReport.LinkOwner = linkOwner;
            advertiserClickReport.LinkType = linkType;
            advertiserClickReport.UniqueCount = uniqueCount;
            advertiserClickReport.TotalCount = totalCount;

            // Assert
            advertiserClickReport.BlastID.ShouldBe(blastId);
            advertiserClickReport.EmailSubject.ShouldBe(emailSubject);
            advertiserClickReport.SendTime.ShouldNotBeNull();
            advertiserClickReport.Date.ShouldNotBeNull();
            advertiserClickReport.Alias.ShouldBe(alias);
            advertiserClickReport.LinkURL.ShouldBe(linkURL);
            advertiserClickReport.LinkOwner.ShouldBe(linkOwner);
            advertiserClickReport.LinkType.ShouldBe(linkType);
            advertiserClickReport.UniqueCount.ShouldBe(uniqueCount);
            advertiserClickReport.TotalCount.ShouldBe(totalCount);
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (Alias) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Alias_Property_String_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.Alias = Fixture.Create<string>();
            var stringType = advertiserClickReport.Alias.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (Alias) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_AliasNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAlias = "AliasNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameAlias));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Alias_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAlias = "Alias";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameAlias);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.BlastID = Fixture.Create<int>();
            var intType = advertiserClickReport.BlastID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (Date) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_DateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDate = "DateNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Date_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDate = "Date";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.EmailSubject = Fixture.Create<string>();
            var stringType = advertiserClickReport.EmailSubject.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkOwner) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkOwner_Property_String_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.LinkOwner = Fixture.Create<string>();
            var stringType = advertiserClickReport.LinkOwner.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkOwner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_LinkOwnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkOwner = "LinkOwnerNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameLinkOwner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkOwner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkOwner = "LinkOwner";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameLinkOwner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.LinkType = Fixture.Create<string>();
            var stringType = advertiserClickReport.LinkType.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_LinkTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkType = "LinkTypeNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameLinkType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkType = "LinkType";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameLinkType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.LinkURL = Fixture.Create<string>();
            var stringType = advertiserClickReport.LinkURL.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (LinkURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_LinkURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURLNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameLinkURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_LinkURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURL";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameLinkURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var advertiserClickReport = new AdvertiserClickReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(advertiserClickReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.TotalCount = Fixture.Create<int>();
            var intType = advertiserClickReport.TotalCount.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (UniqueCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_UniqueCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var advertiserClickReport = new AdvertiserClickReport();
            advertiserClickReport.UniqueCount = Fixture.Create<int>();
            var intType = advertiserClickReport.UniqueCount.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AdvertiserClickReport) => Property (UniqueCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_Class_Invalid_Property_UniqueCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueCount = "UniqueCountNotPresent";
            var advertiserClickReport = new AdvertiserClickReport();

            // Act , Assert
            Should.NotThrow(() => advertiserClickReport.GetType().GetProperty(propertyNameUniqueCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AdvertiserClickReport_UniqueCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueCount = "UniqueCount";
            var advertiserClickReport = new AdvertiserClickReport();
            var propertyInfo = advertiserClickReport.GetType().GetProperty(propertyNameUniqueCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (AdvertiserClickReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_AdvertiserClickReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new AdvertiserClickReport());
        }

        #endregion

        #region General Constructor : Class (AdvertiserClickReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_AdvertiserClickReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstAdvertiserClickReport = new AdvertiserClickReport();
            var secondAdvertiserClickReport = new AdvertiserClickReport();
            var thirdAdvertiserClickReport = new AdvertiserClickReport();
            var fourthAdvertiserClickReport = new AdvertiserClickReport();
            var fifthAdvertiserClickReport = new AdvertiserClickReport();
            var sixthAdvertiserClickReport = new AdvertiserClickReport();

            // Act, Assert
            firstAdvertiserClickReport.ShouldNotBeNull();
            secondAdvertiserClickReport.ShouldNotBeNull();
            thirdAdvertiserClickReport.ShouldNotBeNull();
            fourthAdvertiserClickReport.ShouldNotBeNull();
            fifthAdvertiserClickReport.ShouldNotBeNull();
            sixthAdvertiserClickReport.ShouldNotBeNull();
            firstAdvertiserClickReport.ShouldNotBeSameAs(secondAdvertiserClickReport);
            thirdAdvertiserClickReport.ShouldNotBeSameAs(firstAdvertiserClickReport);
            fourthAdvertiserClickReport.ShouldNotBeSameAs(firstAdvertiserClickReport);
            fifthAdvertiserClickReport.ShouldNotBeSameAs(firstAdvertiserClickReport);
            sixthAdvertiserClickReport.ShouldNotBeSameAs(firstAdvertiserClickReport);
            sixthAdvertiserClickReport.ShouldNotBeSameAs(fourthAdvertiserClickReport);
        }

        #endregion

        #endregion

        #endregion
    }
}