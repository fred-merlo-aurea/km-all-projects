using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class SampleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Sample) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var sampleId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var sampleName = Fixture.Create<string>();
            var winningBlastId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var aBWinnerType = Fixture.Create<string>();
            var didNotReceiveAB = Fixture.Create<bool?>();
            var deliveredOrOpened = Fixture.Create<string>();

            // Act
            sample.SampleID = sampleId;
            sample.CustomerID = customerId;
            sample.SampleName = sampleName;
            sample.WinningBlastID = winningBlastId;
            sample.CreatedUserID = createdUserId;
            sample.UpdatedUserID = updatedUserId;
            sample.IsDeleted = isDeleted;
            sample.ABWinnerType = aBWinnerType;
            sample.DidNotReceiveAB = didNotReceiveAB;
            sample.DeliveredOrOpened = deliveredOrOpened;

            // Assert
            sample.SampleID.ShouldBe(sampleId);
            sample.CustomerID.ShouldBe(customerId);
            sample.SampleName.ShouldBe(sampleName);
            sample.WinningBlastID.ShouldBe(winningBlastId);
            sample.CreatedUserID.ShouldBe(createdUserId);
            sample.CreatedDate.ShouldBeNull();
            sample.UpdatedUserID.ShouldBe(updatedUserId);
            sample.UpdatedDate.ShouldBeNull();
            sample.IsDeleted.ShouldBe(isDeleted);
            sample.ABWinnerType.ShouldBe(aBWinnerType);
            sample.DidNotReceiveAB.ShouldBe(didNotReceiveAB);
            sample.DeliveredOrOpened.ShouldBe(deliveredOrOpened);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (ABWinnerType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_ABWinnerType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            sample.ABWinnerType = Fixture.Create<string>();
            var stringType = sample.ABWinnerType.GetType();

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

        #region General Getters/Setters : Class (Sample) => Property (ABWinnerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_ABWinnerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerTypeNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameABWinnerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_ABWinnerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerType";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameABWinnerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var sample = Fixture.Create<Sample>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(sample, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<int>();

            // Act , Set
            sample.CreatedUserID = random;

            // Assert
            sample.CreatedUserID.ShouldBe(random);
            sample.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.CreatedUserID = null;

            // Assert
            sample.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.CreatedUserID.ShouldBeNull();
            sample.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<int>();

            // Act , Set
            sample.CustomerID = random;

            // Assert
            sample.CustomerID.ShouldBe(random);
            sample.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.CustomerID = null;

            // Assert
            sample.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.CustomerID.ShouldBeNull();
            sample.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (DeliveredOrOpened) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DeliveredOrOpened_Property_String_Type_Verify_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            sample.DeliveredOrOpened = Fixture.Create<string>();
            var stringType = sample.DeliveredOrOpened.GetType();

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

        #region General Getters/Setters : Class (Sample) => Property (DeliveredOrOpened) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_DeliveredOrOpenedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredOrOpened = "DeliveredOrOpenedNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameDeliveredOrOpened));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DeliveredOrOpened_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDeliveredOrOpened = "DeliveredOrOpened";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameDeliveredOrOpened);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (DidNotReceiveAB) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DidNotReceiveAB_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sample.DidNotReceiveAB = random;

            // Assert
            sample.DidNotReceiveAB.ShouldBe(random);
            sample.DidNotReceiveAB.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DidNotReceiveAB_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.DidNotReceiveAB = null;

            // Assert
            sample.DidNotReceiveAB.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DidNotReceiveAB_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDidNotReceiveAB = "DidNotReceiveAB";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameDidNotReceiveAB);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.DidNotReceiveAB.ShouldBeNull();
            sample.DidNotReceiveAB.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (DidNotReceiveAB) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_DidNotReceiveABNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDidNotReceiveAB = "DidNotReceiveABNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameDidNotReceiveAB));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_DidNotReceiveAB_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDidNotReceiveAB = "DidNotReceiveAB";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameDidNotReceiveAB);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sample.IsDeleted = random;

            // Assert
            sample.IsDeleted.ShouldBe(random);
            sample.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.IsDeleted = null;

            // Assert
            sample.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.IsDeleted.ShouldBeNull();
            sample.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (SampleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_SampleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            sample.SampleID = Fixture.Create<int>();
            var intType = sample.SampleID.GetType();

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

        #region General Getters/Setters : Class (Sample) => Property (SampleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_SampleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleIDNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameSampleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_SampleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleID";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameSampleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (SampleName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_SampleName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            sample.SampleName = Fixture.Create<string>();
            var stringType = sample.SampleName.GetType();

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

        #region General Getters/Setters : Class (Sample) => Property (SampleName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_SampleNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSampleName = "SampleNameNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameSampleName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_SampleName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSampleName = "SampleName";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameSampleName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var sample = Fixture.Create<Sample>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(sample, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<int>();

            // Act , Set
            sample.UpdatedUserID = random;

            // Assert
            sample.UpdatedUserID.ShouldBe(random);
            sample.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.UpdatedUserID = null;

            // Assert
            sample.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.UpdatedUserID.ShouldBeNull();
            sample.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (WinningBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_WinningBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();
            var random = Fixture.Create<int>();

            // Act , Set
            sample.WinningBlastID = random;

            // Assert
            sample.WinningBlastID.ShouldBe(random);
            sample.WinningBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_WinningBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var sample = Fixture.Create<Sample>();

            // Act , Set
            sample.WinningBlastID = null;

            // Assert
            sample.WinningBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_WinningBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameWinningBlastID = "WinningBlastID";
            var sample = Fixture.Create<Sample>();
            var propertyInfo = sample.GetType().GetProperty(propertyNameWinningBlastID);

            // Act , Set
            propertyInfo.SetValue(sample, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sample.WinningBlastID.ShouldBeNull();
            sample.WinningBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Sample) => Property (WinningBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_Class_Invalid_Property_WinningBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWinningBlastID = "WinningBlastIDNotPresent";
            var sample  = Fixture.Create<Sample>();

            // Act , Assert
            Should.NotThrow(() => sample.GetType().GetProperty(propertyNameWinningBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Sample_WinningBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWinningBlastID = "WinningBlastID";
            var sample  = Fixture.Create<Sample>();
            var propertyInfo  = sample.GetType().GetProperty(propertyNameWinningBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Sample) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Sample_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Sample());
        }

        #endregion

        #region General Constructor : Class (Sample) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Sample_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSample = Fixture.CreateMany<Sample>(2).ToList();
            var firstSample = instancesOfSample.FirstOrDefault();
            var lastSample = instancesOfSample.Last();

            // Act, Assert
            firstSample.ShouldNotBeNull();
            lastSample.ShouldNotBeNull();
            firstSample.ShouldNotBeSameAs(lastSample);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Sample_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSample = new Sample();
            var secondSample = new Sample();
            var thirdSample = new Sample();
            var fourthSample = new Sample();
            var fifthSample = new Sample();
            var sixthSample = new Sample();

            // Act, Assert
            firstSample.ShouldNotBeNull();
            secondSample.ShouldNotBeNull();
            thirdSample.ShouldNotBeNull();
            fourthSample.ShouldNotBeNull();
            fifthSample.ShouldNotBeNull();
            sixthSample.ShouldNotBeNull();
            firstSample.ShouldNotBeSameAs(secondSample);
            thirdSample.ShouldNotBeSameAs(firstSample);
            fourthSample.ShouldNotBeSameAs(firstSample);
            fifthSample.ShouldNotBeSameAs(firstSample);
            sixthSample.ShouldNotBeSameAs(firstSample);
            sixthSample.ShouldNotBeSameAs(fourthSample);
        }

        #endregion

        #region General Constructor : Class (Sample) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Sample_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var sampleId = -1;
            var sampleName = string.Empty;
            var deliveredOrOpened = string.Empty;

            // Act
            var sample = new Sample();

            // Assert
            sample.SampleID.ShouldBe(sampleId);
            sample.CustomerID.ShouldBeNull();
            sample.SampleName.ShouldBe(sampleName);
            sample.WinningBlastID.ShouldBeNull();
            sample.CreatedUserID.ShouldBeNull();
            sample.CreatedDate.ShouldBeNull();
            sample.UpdatedUserID.ShouldBeNull();
            sample.UpdatedDate.ShouldBeNull();
            sample.IsDeleted.ShouldBeNull();
            sample.ABWinnerType.ShouldBeNull();
            sample.DidNotReceiveAB.ShouldBeNull();
            sample.DeliveredOrOpened.ShouldBe(deliveredOrOpened);
        }

        #endregion

        #endregion

        #endregion
    }
}