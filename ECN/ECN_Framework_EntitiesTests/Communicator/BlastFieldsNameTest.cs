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
using ECN_Framework_Entities.Communicator;

namespace ECN_Framework_Entities.Communicator
{
    [TestFixture]
    public class BlastFieldsNameTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastFieldsName  = new BlastFieldsName();
            var blastFieldsNameID = Fixture.Create<int>();
            var blastFieldID = Fixture.Create<int>();
            var customerID = Fixture.Create<int?>();
            var name = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            blastFieldsName.BlastFieldsNameID = blastFieldsNameID;
            blastFieldsName.BlastFieldID = blastFieldID;
            blastFieldsName.CustomerID = customerID;
            blastFieldsName.Name = name;
            blastFieldsName.CreatedUserID = createdUserID;
            blastFieldsName.CreatedDate = createdDate;
            blastFieldsName.UpdatedUserID = updatedUserID;
            blastFieldsName.UpdatedDate = updatedDate;
            blastFieldsName.IsDeleted = isDeleted;

            // Assert
            blastFieldsName.BlastFieldsNameID.ShouldBe(blastFieldsNameID);
            blastFieldsName.BlastFieldID.ShouldBe(blastFieldID);
            blastFieldsName.CustomerID.ShouldBe(customerID);
            blastFieldsName.Name.ShouldBe(name);
            blastFieldsName.CreatedUserID.ShouldBe(createdUserID);
            blastFieldsName.CreatedDate.ShouldBe(createdDate);
            blastFieldsName.UpdatedUserID.ShouldBe(updatedUserID);
            blastFieldsName.UpdatedDate.ShouldBe(updatedDate);
            blastFieldsName.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : BlastFieldsName => BlastFieldID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_BlastFieldID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var intType = blastFieldsName.BlastFieldID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_BlastFieldIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFieldID = "BlastFieldIDNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameBlastFieldID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_BlastFieldID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFieldID = "BlastFieldID";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameBlastFieldID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : BlastFieldsName => BlastFieldsNameID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_BlastFieldsNameID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var intType = blastFieldsName.BlastFieldsNameID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_BlastFieldsNameIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFieldsNameID = "BlastFieldsNameIDNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameBlastFieldsNameID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_BlastFieldsNameID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFieldsNameID = "BlastFieldsNameID";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameBlastFieldsNameID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BlastFieldsName => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFieldsName, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastFieldsName => CreatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsName.CreatedUserID = random;

            // Assert
            blastFieldsName.CreatedUserID.ShouldBe(random);
            blastFieldsName.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();    

            // Act , Set
            blastFieldsName.CreatedUserID = null;

            // Assert
            blastFieldsName.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsName, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsName.CreatedUserID.ShouldBeNull();
            blastFieldsName.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastFieldsName => CustomerID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsName.CustomerID = random;

            // Assert
            blastFieldsName.CustomerID.ShouldBe(random);
            blastFieldsName.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();    

            // Act , Set
            blastFieldsName.CustomerID = null;

            // Assert
            blastFieldsName.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsName, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsName.CustomerID.ShouldBeNull();
            blastFieldsName.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastFieldsName => IsDeleted

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastFieldsName.IsDeleted = random;

            // Assert
            blastFieldsName.IsDeleted.ShouldBe(random);
            blastFieldsName.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();    

            // Act , Set
            blastFieldsName.IsDeleted = null;

            // Assert
            blastFieldsName.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastFieldsName, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsName.IsDeleted.ShouldBeNull();
            blastFieldsName.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : BlastFieldsName => Name

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var stringType = blastFieldsName.Name.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BlastFieldsName => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFieldsName, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastFieldsName => UpdatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsName.UpdatedUserID = random;

            // Assert
            blastFieldsName.UpdatedUserID.ShouldBe(random);
            blastFieldsName.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsName = Fixture.Create<BlastFieldsName>();    

            // Act , Set
            blastFieldsName.UpdatedUserID = null;

            // Assert
            blastFieldsName.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFieldsName = Fixture.Create<BlastFieldsName>();
            var propertyInfo = blastFieldsName.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsName, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsName.UpdatedUserID.ShouldBeNull();
            blastFieldsName.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsName.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsName_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFieldsName  = Fixture.Create<BlastFieldsName>();
            var propertyInfo  = blastFieldsName.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsName_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastFieldsName());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<BlastFieldsName>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsName_Instantiated_With_Default_Assignments_No_Change_Test()
        {
            // Arrange
            var blastFieldsNameID = -1;
            var blastFieldID = -1;
            var customerID = -1;
            var name = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;

            // Act
            var blastFieldsName = new BlastFieldsName();

            // Assert
            blastFieldsName.BlastFieldsNameID.ShouldBe(blastFieldsNameID);
            blastFieldsName.BlastFieldID.ShouldBe(blastFieldID);
            blastFieldsName.CustomerID.ShouldBe(customerID);
            blastFieldsName.Name.ShouldBe(name);
            blastFieldsName.CreatedUserID.ShouldBeNull();
            blastFieldsName.CreatedDate.ShouldBeNull();
            blastFieldsName.UpdatedUserID.ShouldBeNull();
            blastFieldsName.UpdatedDate.ShouldBeNull();
            blastFieldsName.IsDeleted.ShouldBeNull();
        }

        #endregion


        #endregion


        #endregion
    }
}