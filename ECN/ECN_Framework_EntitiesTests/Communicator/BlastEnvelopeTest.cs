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
    public class BlastEnvelopeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastEnvelope  = new BlastEnvelope();
            var blastEnvelopeID = Fixture.Create<int>();
            var fromName = Fixture.Create<string>();
            var fromEmail = Fixture.Create<string>();
            var customerID = Fixture.Create<int?>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            blastEnvelope.BlastEnvelopeID = blastEnvelopeID;
            blastEnvelope.FromName = fromName;
            blastEnvelope.FromEmail = fromEmail;
            blastEnvelope.CustomerID = customerID;
            blastEnvelope.CreatedUserID = createdUserID;
            blastEnvelope.CreatedDate = createdDate;
            blastEnvelope.UpdatedUserID = updatedUserID;
            blastEnvelope.UpdatedDate = updatedDate;
            blastEnvelope.IsDeleted = isDeleted;

            // Assert
            blastEnvelope.BlastEnvelopeID.ShouldBe(blastEnvelopeID);
            blastEnvelope.FromName.ShouldBe(fromName);
            blastEnvelope.FromEmail.ShouldBe(fromEmail);
            blastEnvelope.CustomerID.ShouldBe(customerID);
            blastEnvelope.CreatedUserID.ShouldBe(createdUserID);
            blastEnvelope.CreatedDate.ShouldBe(createdDate);
            blastEnvelope.UpdatedUserID.ShouldBe(updatedUserID);
            blastEnvelope.UpdatedDate.ShouldBe(updatedDate);
            blastEnvelope.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : BlastEnvelope => BlastEnvelopeID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_BlastEnvelopeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var intType = blastEnvelope.BlastEnvelopeID.GetType();

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
        public void BlastEnvelope_Class_Invalid_Property_BlastEnvelopeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastEnvelopeID = "BlastEnvelopeIDNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameBlastEnvelopeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_BlastEnvelopeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastEnvelopeID = "BlastEnvelopeID";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameBlastEnvelopeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BlastEnvelope => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastEnvelope, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastEnvelope => CreatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastEnvelope.CreatedUserID = random;

            // Assert
            blastEnvelope.CreatedUserID.ShouldBe(random);
            blastEnvelope.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();    

            // Act , Set
            blastEnvelope.CreatedUserID = null;

            // Assert
            blastEnvelope.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastEnvelope, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastEnvelope.CreatedUserID.ShouldBeNull();
            blastEnvelope.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastEnvelope => CustomerID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastEnvelope.CustomerID = random;

            // Assert
            blastEnvelope.CustomerID.ShouldBe(random);
            blastEnvelope.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();    

            // Act , Set
            blastEnvelope.CustomerID = null;

            // Assert
            blastEnvelope.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastEnvelope, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastEnvelope.CustomerID.ShouldBeNull();
            blastEnvelope.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : BlastEnvelope => FromEmail

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_FromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var stringType = blastEnvelope.FromEmail.GetType();

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
        public void BlastEnvelope_Class_Invalid_Property_FromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmailNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_FromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmail";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : BlastEnvelope => FromName

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var stringType = blastEnvelope.FromName.GetType();

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
        public void BlastEnvelope_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastEnvelope => IsDeleted

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastEnvelope.IsDeleted = random;

            // Assert
            blastEnvelope.IsDeleted.ShouldBe(random);
            blastEnvelope.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();    

            // Act , Set
            blastEnvelope.IsDeleted = null;

            // Assert
            blastEnvelope.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastEnvelope, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastEnvelope.IsDeleted.ShouldBeNull();
            blastEnvelope.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : BlastEnvelope => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastEnvelope, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : BlastEnvelope => UpdatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastEnvelope.UpdatedUserID = random;

            // Assert
            blastEnvelope.UpdatedUserID.ShouldBe(random);
            blastEnvelope.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastEnvelope = Fixture.Create<BlastEnvelope>();    

            // Act , Set
            blastEnvelope.UpdatedUserID = null;

            // Assert
            blastEnvelope.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastEnvelope = Fixture.Create<BlastEnvelope>();
            var propertyInfo = blastEnvelope.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastEnvelope, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastEnvelope.UpdatedUserID.ShouldBeNull();
            blastEnvelope.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();

            // Act , Assert
            Should.NotThrow(() => blastEnvelope.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastEnvelope_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastEnvelope  = Fixture.Create<BlastEnvelope>();
            var propertyInfo  = blastEnvelope.GetType().GetProperty(propertyNameUpdatedUserID);

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
        public void Constructor_BlastEnvelope_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastEnvelope());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<BlastEnvelope>(2).ToList();
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
        public void Constructor_BlastEnvelope_Instantiated_With_Default_Assignments_No_Change_Test()
        {
            // Arrange
            var blastEnvelopeID = -1;
            int? customerID = null;
            var fromName = string.Empty;
            var fromEmail = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;

            // Act
            var blastEnvelope = new BlastEnvelope();

            // Assert
            blastEnvelope.BlastEnvelopeID.ShouldBe(blastEnvelopeID);
            blastEnvelope.CustomerID.ShouldBeNull();
            blastEnvelope.FromName.ShouldBe(fromName);
            blastEnvelope.FromEmail.ShouldBe(fromEmail);
            blastEnvelope.CreatedUserID.ShouldBeNull();
            blastEnvelope.CreatedDate.ShouldBeNull();
            blastEnvelope.UpdatedUserID.ShouldBeNull();
            blastEnvelope.UpdatedDate.ShouldBeNull();
            blastEnvelope.IsDeleted.ShouldBeNull();
        }

        #endregion


        #endregion


        #endregion
    }
}