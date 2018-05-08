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
    public class AutoRespondersTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var autoResponders  = new AutoResponders();
            var autoResponderID = Fixture.Create<int>();
            var blastID = Fixture.Create<int?>();
            var mailServer = Fixture.Create<string>();
            var accountName = Fixture.Create<string>();
            var accountPasswd = Fixture.Create<string>();
            var forwardTo = Fixture.Create<string>();
            var customerID = Fixture.Create<int?>();
            var createdUserID = Fixture.Create<int?>();

            var updatedUserID = Fixture.Create<int?>();

            var isDeleted = Fixture.Create<bool?>();

            // Act
            autoResponders.AutoResponderID = autoResponderID;
            autoResponders.BlastID = blastID;
            autoResponders.MailServer = mailServer;
            autoResponders.AccountName = accountName;
            autoResponders.AccountPasswd = accountPasswd;
            autoResponders.ForwardTo = forwardTo;
            autoResponders.CustomerID = customerID;
            autoResponders.CreatedUserID = createdUserID;

            autoResponders.UpdatedUserID = updatedUserID;

            autoResponders.IsDeleted = isDeleted;

            // Assert
            autoResponders.AutoResponderID.ShouldBe(autoResponderID);
            autoResponders.BlastID.ShouldBe(blastID);
            autoResponders.MailServer.ShouldBe(mailServer);
            autoResponders.AccountName.ShouldBe(accountName);
            autoResponders.AccountPasswd.ShouldBe(accountPasswd);
            autoResponders.ForwardTo.ShouldBe(forwardTo);
            autoResponders.CustomerID.ShouldBe(customerID);
            autoResponders.CreatedUserID.ShouldBe(createdUserID);
            autoResponders.CreatedDate.ShouldBeNull();
            autoResponders.UpdatedUserID.ShouldBe(updatedUserID);
            autoResponders.UpdatedDate.ShouldBeNull();
            autoResponders.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region string property type test : AutoResponders => AccountName

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AccountName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var stringType = autoResponders.AccountName.GetType();

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
        public void AutoResponders_Class_Invalid_Property_AccountNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountName = "AccountNameNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameAccountName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AccountName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountName = "AccountName";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameAccountName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : AutoResponders => AccountPasswd

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AccountPasswd_Property_String_Type_Verify_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var stringType = autoResponders.AccountPasswd.GetType();

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
        public void AutoResponders_Class_Invalid_Property_AccountPasswdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountPasswd = "AccountPasswdNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameAccountPasswd));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AccountPasswd_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountPasswd = "AccountPasswd";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameAccountPasswd);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : AutoResponders => AutoResponderID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AutoResponderID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var intType = autoResponders.AutoResponderID.GetType();

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
        public void AutoResponders_Class_Invalid_Property_AutoResponderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAutoResponderID = "AutoResponderIDNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameAutoResponderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_AutoResponderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAutoResponderID = "AutoResponderID";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameAutoResponderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : AutoResponders => BlastID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var random = Fixture.Create<int>();

            // Act , Set
            autoResponders.BlastID = random;

            // Assert
            autoResponders.BlastID.ShouldBe(random);
            autoResponders.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();    

            // Act , Set
            autoResponders.BlastID = null;

            // Assert
            autoResponders.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var autoResponders = Fixture.Create<AutoResponders>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(autoResponders, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            autoResponders.BlastID.ShouldBeNull();
            autoResponders.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : AutoResponders => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var autoResponders = Fixture.Create<AutoResponders>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(autoResponders, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : AutoResponders => CreatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var random = Fixture.Create<int>();

            // Act , Set
            autoResponders.CreatedUserID = random;

            // Assert
            autoResponders.CreatedUserID.ShouldBe(random);
            autoResponders.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();    

            // Act , Set
            autoResponders.CreatedUserID = null;

            // Assert
            autoResponders.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var autoResponders = Fixture.Create<AutoResponders>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(autoResponders, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            autoResponders.CreatedUserID.ShouldBeNull();
            autoResponders.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : AutoResponders => CustomerID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var random = Fixture.Create<int>();

            // Act , Set
            autoResponders.CustomerID = random;

            // Assert
            autoResponders.CustomerID.ShouldBe(random);
            autoResponders.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();    

            // Act , Set
            autoResponders.CustomerID = null;

            // Assert
            autoResponders.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var autoResponders = Fixture.Create<AutoResponders>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(autoResponders, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            autoResponders.CustomerID.ShouldBeNull();
            autoResponders.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : AutoResponders => ForwardTo

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_ForwardTo_Property_String_Type_Verify_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var stringType = autoResponders.ForwardTo.GetType();

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
        public void AutoResponders_Class_Invalid_Property_ForwardToNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameForwardTo = "ForwardToNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameForwardTo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_ForwardTo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameForwardTo = "ForwardTo";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameForwardTo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : AutoResponders => IsDeleted

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var random = Fixture.Create<bool>();

            // Act , Set
            autoResponders.IsDeleted = random;

            // Assert
            autoResponders.IsDeleted.ShouldBe(random);
            autoResponders.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();    

            // Act , Set
            autoResponders.IsDeleted = null;

            // Assert
            autoResponders.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var autoResponders = Fixture.Create<AutoResponders>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(autoResponders, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            autoResponders.IsDeleted.ShouldBeNull();
            autoResponders.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : AutoResponders => MailServer

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_MailServer_Property_String_Type_Verify_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var stringType = autoResponders.MailServer.GetType();

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
        public void AutoResponders_Class_Invalid_Property_MailServerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMailServer = "MailServerNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameMailServer));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_MailServer_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMailServer = "MailServer";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameMailServer);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : AutoResponders => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var autoResponders = Fixture.Create<AutoResponders>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(autoResponders, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : AutoResponders => UpdatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();
            var random = Fixture.Create<int>();

            // Act , Set
            autoResponders.UpdatedUserID = random;

            // Assert
            autoResponders.UpdatedUserID.ShouldBe(random);
            autoResponders.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var autoResponders = Fixture.Create<AutoResponders>();    

            // Act , Set
            autoResponders.UpdatedUserID = null;

            // Assert
            autoResponders.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var autoResponders = Fixture.Create<AutoResponders>();
            var propertyInfo = autoResponders.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(autoResponders, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            autoResponders.UpdatedUserID.ShouldBeNull();
            autoResponders.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var autoResponders  = Fixture.Create<AutoResponders>();

            // Act , Assert
            Should.NotThrow(() => autoResponders.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AutoResponders_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var autoResponders  = Fixture.Create<AutoResponders>();
            var propertyInfo  = autoResponders.GetType().GetProperty(propertyNameUpdatedUserID);

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
        public void Constructor_AutoResponders_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new AutoResponders());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<AutoResponders>(2).ToList();
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
        public void Constructor_AutoResponders_Instantiated_With_Default_Assignments_No_Change_Test()
        {
            // Arrange
            var autoResponderID = -1;
            int? blastID = null;
            var mailServer = string.Empty;
            var accountName = string.Empty;
            var accountPasswd = string.Empty;
            var forwardTo = string.Empty;
            int? customerID = null;
            int? createdUserID = null;

            int? updatedUserID = null;

            bool? isDeleted = null;

            // Act
            var autoResponders = new AutoResponders();

            // Assert
            autoResponders.AutoResponderID.ShouldBe(autoResponderID);
            autoResponders.BlastID.ShouldBeNull();
            autoResponders.MailServer.ShouldBe(mailServer);
            autoResponders.AccountName.ShouldBe(accountName);
            autoResponders.AccountPasswd.ShouldBe(accountPasswd);
            autoResponders.ForwardTo.ShouldBe(forwardTo);
            autoResponders.CustomerID.ShouldBeNull();
            autoResponders.CreatedUserID.ShouldBeNull();
            autoResponders.CreatedDate.ShouldBeNull();
            autoResponders.UpdatedUserID.ShouldBeNull();
            autoResponders.UpdatedDate.ShouldBeNull();
            autoResponders.IsDeleted.ShouldBeNull();
        }

        #endregion


        #endregion


        #endregion
    }
}