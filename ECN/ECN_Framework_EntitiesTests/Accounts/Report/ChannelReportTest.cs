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
using ECN_Framework_Entities.Accounts.Report;

namespace ECN_Framework_Entities.Accounts.Report
{
    [TestFixture]
    public class ChannelReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ChannelReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            var basechannelId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var customerType = Fixture.Create<string>();
            var cCount = Fixture.Create<int>();
            var uCount = Fixture.Create<int>();
            var mtdcount = Fixture.Create<int>();
            var ytdcount = Fixture.Create<int>();
            var mtdsent = Fixture.Create<int>();
            var ytdsent = Fixture.Create<int>();

            // Act
            channelReport.BasechannelID = basechannelId;
            channelReport.CustomerID = customerId;
            channelReport.CustomerType = customerType;
            channelReport.cCount = cCount;
            channelReport.uCount = uCount;
            channelReport.mtdcount = mtdcount;
            channelReport.ytdcount = ytdcount;
            channelReport.mtdsent = mtdsent;
            channelReport.ytdsent = ytdsent;

            // Assert
            channelReport.BasechannelID.ShouldBe(basechannelId);
            channelReport.CustomerID.ShouldBe(customerId);
            channelReport.CustomerType.ShouldBe(customerType);
            channelReport.cCount.ShouldBe(cCount);
            channelReport.uCount.ShouldBe(uCount);
            channelReport.mtdcount.ShouldBe(mtdcount);
            channelReport.ytdcount.ShouldBe(ytdcount);
            channelReport.mtdsent.ShouldBe(mtdsent);
            channelReport.ytdsent.ShouldBe(ytdsent);
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (BasechannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_BasechannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.BasechannelID = Fixture.Create<int>();
            var intType = channelReport.BasechannelID.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (BasechannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_BasechannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBasechannelID = "BasechannelIDNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameBasechannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_BasechannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBasechannelID = "BasechannelID";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameBasechannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (cCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_cCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.cCount = Fixture.Create<int>();
            var intType = channelReport.cCount.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (cCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_cCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecCount = "cCountNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNamecCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_cCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecCount = "cCount";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNamecCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.CustomerID = Fixture.Create<int>();
            var intType = channelReport.CustomerID.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (CustomerType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_CustomerType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.CustomerType = Fixture.Create<string>();
            var stringType = channelReport.CustomerType.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (CustomerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_CustomerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerType = "CustomerTypeNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameCustomerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_CustomerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerType = "CustomerType";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameCustomerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (mtdcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_mtdcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.mtdcount = Fixture.Create<int>();
            var intType = channelReport.mtdcount.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (mtdcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_mtdcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamemtdcount = "mtdcountNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNamemtdcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_mtdcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamemtdcount = "mtdcount";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNamemtdcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (mtdsent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_mtdsent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.mtdsent = Fixture.Create<int>();
            var intType = channelReport.mtdsent.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (mtdsent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_mtdsentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamemtdsent = "mtdsentNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNamemtdsent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_mtdsent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamemtdsent = "mtdsent";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNamemtdsent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (uCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_uCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.uCount = Fixture.Create<int>();
            var intType = channelReport.uCount.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (uCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_uCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuCount = "uCountNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameuCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_uCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuCount = "uCount";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameuCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (ytdcount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_ytdcount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.ytdcount = Fixture.Create<int>();
            var intType = channelReport.ytdcount.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (ytdcount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_ytdcountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameytdcount = "ytdcountNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameytdcount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_ytdcount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameytdcount = "ytdcount";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameytdcount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChannelReport) => Property (ytdsent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_ytdsent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var channelReport = Fixture.Create<ChannelReport>();
            channelReport.ytdsent = Fixture.Create<int>();
            var intType = channelReport.ytdsent.GetType();

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

        #region General Getters/Setters : Class (ChannelReport) => Property (ytdsent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_Class_Invalid_Property_ytdsentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameytdsent = "ytdsentNotPresent";
            var channelReport  = Fixture.Create<ChannelReport>();

            // Act , Assert
            Should.NotThrow(() => channelReport.GetType().GetProperty(propertyNameytdsent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChannelReport_ytdsent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameytdsent = "ytdsent";
            var channelReport  = Fixture.Create<ChannelReport>();
            var propertyInfo  = channelReport.GetType().GetProperty(propertyNameytdsent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ChannelReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ChannelReport());
        }

        #endregion

        #region General Constructor : Class (ChannelReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfChannelReport = Fixture.CreateMany<ChannelReport>(2).ToList();
            var firstChannelReport = instancesOfChannelReport.FirstOrDefault();
            var lastChannelReport = instancesOfChannelReport.Last();

            // Act, Assert
            firstChannelReport.ShouldNotBeNull();
            lastChannelReport.ShouldNotBeNull();
            firstChannelReport.ShouldNotBeSameAs(lastChannelReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChannelReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstChannelReport = new ChannelReport();
            var secondChannelReport = new ChannelReport();
            var thirdChannelReport = new ChannelReport();
            var fourthChannelReport = new ChannelReport();
            var fifthChannelReport = new ChannelReport();
            var sixthChannelReport = new ChannelReport();

            // Act, Assert
            firstChannelReport.ShouldNotBeNull();
            secondChannelReport.ShouldNotBeNull();
            thirdChannelReport.ShouldNotBeNull();
            fourthChannelReport.ShouldNotBeNull();
            fifthChannelReport.ShouldNotBeNull();
            sixthChannelReport.ShouldNotBeNull();
            firstChannelReport.ShouldNotBeSameAs(secondChannelReport);
            thirdChannelReport.ShouldNotBeSameAs(firstChannelReport);
            fourthChannelReport.ShouldNotBeSameAs(firstChannelReport);
            fifthChannelReport.ShouldNotBeSameAs(firstChannelReport);
            sixthChannelReport.ShouldNotBeSameAs(firstChannelReport);
            sixthChannelReport.ShouldNotBeSameAs(fourthChannelReport);
        }

        #endregion

        #endregion

        #endregion
    }
}