using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class LandingPageAssignTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var landingPageAssign  = new LandingPageAssign();
            var lPAID = Fixture.Create<int>();
            var lPID = Fixture.Create<int?>();
            var isDefault = Fixture.Create<bool?>();
            var baseChannelID = Fixture.Create<int?>();
            var customerCanOverride = Fixture.Create<bool?>();
            var customerID = Fixture.Create<int?>();
            var customerDoesOverride = Fixture.Create<bool?>();
            var label = Fixture.Create<string>();
            var header = Fixture.Create<string>();
            var footer = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var baseChannelDoesOverride = Fixture.Create<bool?>();
            var assignContentList = Fixture.Create<List<LandingPageAssignContent>>();

            // Act
            landingPageAssign.LPAID = lPAID;
            landingPageAssign.LPID = lPID;
            landingPageAssign.IsDefault = isDefault;
            landingPageAssign.BaseChannelID = baseChannelID;
            landingPageAssign.CustomerCanOverride = customerCanOverride;
            landingPageAssign.CustomerID = customerID;
            landingPageAssign.CustomerDoesOverride = customerDoesOverride;
            landingPageAssign.Label = label;
            landingPageAssign.Header = header;
            landingPageAssign.Footer = footer;
            landingPageAssign.CreatedUserID = createdUserID;
            landingPageAssign.CreatedDate = createdDate;
            landingPageAssign.UpdatedUserID = updatedUserID;
            landingPageAssign.UpdatedDate = updatedDate;
            landingPageAssign.BaseChannelDoesOverride = baseChannelDoesOverride;
            landingPageAssign.AssignContentList = assignContentList;

            // Assert
            landingPageAssign.LPAID.ShouldBe(lPAID);
            landingPageAssign.LPID.ShouldBe(lPID);
            landingPageAssign.IsDefault.ShouldBe(isDefault);
            landingPageAssign.BaseChannelID.ShouldBe(baseChannelID);
            landingPageAssign.CustomerCanOverride.ShouldBe(customerCanOverride);
            landingPageAssign.CustomerID.ShouldBe(customerID);
            landingPageAssign.CustomerDoesOverride.ShouldBe(customerDoesOverride);
            landingPageAssign.Label.ShouldBe(label);
            landingPageAssign.Header.ShouldBe(header);
            landingPageAssign.Footer.ShouldBe(footer);
            landingPageAssign.CreatedUserID.ShouldBe(createdUserID);
            landingPageAssign.CreatedDate.ShouldBe(createdDate);
            landingPageAssign.UpdatedUserID.ShouldBe(updatedUserID);
            landingPageAssign.UpdatedDate.ShouldBe(updatedDate);
            landingPageAssign.BaseChannelDoesOverride.ShouldBe(baseChannelDoesOverride);
            landingPageAssign.AssignContentList.ShouldBe(assignContentList);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : LandingPageAssign => BaseChannelDoesOverride

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelDoesOverride_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageAssign.BaseChannelDoesOverride = random;

            // Assert
            landingPageAssign.BaseChannelDoesOverride.ShouldBe(random);
            landingPageAssign.BaseChannelDoesOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelDoesOverride_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.BaseChannelDoesOverride = null;

            // Assert
            landingPageAssign.BaseChannelDoesOverride.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelDoesOverride_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constBaseChannelDoesOverride = "BaseChannelDoesOverride";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constBaseChannelDoesOverride);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.BaseChannelDoesOverride.ShouldBeNull();
            landingPageAssign.BaseChannelDoesOverride.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_BaseChannelDoesOverride_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelDoesOverride = "BaseChannelDoesOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constBaseChannelDoesOverride));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelDoesOverride_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelDoesOverride = "BaseChannelDoesOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constBaseChannelDoesOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => CustomerCanOverride

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageAssign.CustomerCanOverride = random;

            // Assert
            landingPageAssign.CustomerCanOverride.ShouldBe(random);
            landingPageAssign.CustomerCanOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.CustomerCanOverride = null;

            // Assert
            landingPageAssign.CustomerCanOverride.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerCanOverride = "CustomerCanOverride";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constCustomerCanOverride);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.CustomerCanOverride.ShouldBeNull();
            landingPageAssign.CustomerCanOverride.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_CustomerCanOverride_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerCanOverride = "CustomerCanOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constCustomerCanOverride));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerCanOverride = "CustomerCanOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constCustomerCanOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => CustomerDoesOverride

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageAssign.CustomerDoesOverride = random;

            // Assert
            landingPageAssign.CustomerDoesOverride.ShouldBe(random);
            landingPageAssign.CustomerDoesOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.CustomerDoesOverride = null;

            // Assert
            landingPageAssign.CustomerDoesOverride.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constCustomerDoesOverride);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.CustomerDoesOverride.ShouldBeNull();
            landingPageAssign.CustomerDoesOverride.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_CustomerDoesOverride_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constCustomerDoesOverride));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constCustomerDoesOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => IsDefault

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDefault_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageAssign.IsDefault = random;

            // Assert
            landingPageAssign.IsDefault.ShouldBe(random);
            landingPageAssign.IsDefault.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDefault_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.IsDefault = null;

            // Assert
            landingPageAssign.IsDefault.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDefault_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDefault = "IsDefault";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constIsDefault);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.IsDefault.ShouldBeNull();
            landingPageAssign.IsDefault.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_IsDefault_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDefault = "IsDefault";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constIsDefault));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDefault_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDefault = "IsDefault";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constIsDefault);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : LandingPageAssign => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(landingPageAssign, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : LandingPageAssign => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(landingPageAssign, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : LandingPageAssign => LPAID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Int_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var intType = landingPageAssign.LPAID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_LPAID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPAID = "LPAID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constLPAID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constLPAID = "LPAID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constLPAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => BaseChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssign.BaseChannelID = random;

            // Assert
            landingPageAssign.BaseChannelID.ShouldBe(random);
            landingPageAssign.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.BaseChannelID = null;

            // Assert
            landingPageAssign.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constBaseChannelID = "BaseChannelID";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.BaseChannelID.ShouldBeNull();
            landingPageAssign.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_BaseChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constBaseChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssign.CreatedUserID = random;

            // Assert
            landingPageAssign.CreatedUserID.ShouldBe(random);
            landingPageAssign.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.CreatedUserID = null;

            // Assert
            landingPageAssign.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.CreatedUserID.ShouldBeNull();
            landingPageAssign.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssign.CustomerID = random;

            // Assert
            landingPageAssign.CustomerID.ShouldBe(random);
            landingPageAssign.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.CustomerID = null;

            // Assert
            landingPageAssign.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerID = "CustomerID";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constCustomerID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.CustomerID.ShouldBeNull();
            landingPageAssign.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => LPID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssign.LPID = random;

            // Assert
            landingPageAssign.LPID.ShouldBe(random);
            landingPageAssign.LPID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.LPID = null;

            // Assert
            landingPageAssign.LPID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constLPID = "LPID";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constLPID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.LPID.ShouldBeNull();
            landingPageAssign.LPID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_LPID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPID = "LPID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constLPID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constLPID = "LPID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constLPID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssign => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssign.UpdatedUserID = random;

            // Assert
            landingPageAssign.UpdatedUserID.ShouldBe(random);
            landingPageAssign.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();    

            // Act , Set
            landingPageAssign.UpdatedUserID = null;

            // Assert
            landingPageAssign.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var propertyInfo = landingPageAssign.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssign, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssign.UpdatedUserID.ShouldBeNull();
            landingPageAssign.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_AssignContentList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAssignContentList = "AssignContentList";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constAssignContentList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AssignContentList_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constAssignContentList = "AssignContentList";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constAssignContentList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageAssign => Footer

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Footer_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var stringType = landingPageAssign.Footer.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_Footer_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFooter = "Footer";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constFooter));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Footer_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constFooter = "Footer";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constFooter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageAssign => Header

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Header_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var stringType = landingPageAssign.Header.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_Header_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constHeader = "Header";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constHeader));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Header_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constHeader = "Header";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageAssign => Label

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Label_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssign = Fixture.Create<LandingPageAssign>();
            var stringType = landingPageAssign.Label.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssign_Class_Invalid_Property_Label_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLabel = "Label";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssign.GetType().GetProperty(constLabel));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Label_Is_Present_In_LandingPageAssign_Class_As_Public_Test()
        {
            // Arrange
            const string constLabel = "Label";
            var landingPageAssign  = Fixture.Create<LandingPageAssign>();
            var propertyInfo  = landingPageAssign.GetType().GetProperty(constLabel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LandingPageAssign());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<LandingPageAssign>(2).ToList();
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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var lPAID = -1;
            int? lPID = null;
            bool? isDefault = null;
            int? baseChannelID = null;
            bool? customerCanOverride = null;
            int? customerID = null;
            bool? customerDoesOverride = null;
            var label = string.Empty;
            var header = string.Empty;
            var footer = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? baseChannelDoesOverride = null;    

            // Act
            var landingPageAssign = new LandingPageAssign();    

            // Assert
            landingPageAssign.LPAID.ShouldBe(lPAID);
            landingPageAssign.LPID.ShouldBeNull();
            landingPageAssign.IsDefault.ShouldBeNull();
            landingPageAssign.BaseChannelID.ShouldBeNull();
            landingPageAssign.CustomerCanOverride.ShouldBeNull();
            landingPageAssign.CustomerID.ShouldBeNull();
            landingPageAssign.CustomerDoesOverride.ShouldBeNull();
            landingPageAssign.Label.ShouldBe(label);
            landingPageAssign.Header.ShouldBe(header);
            landingPageAssign.Footer.ShouldBe(footer);
            landingPageAssign.CreatedUserID.ShouldBeNull();
            landingPageAssign.CreatedDate.ShouldBeNull();
            landingPageAssign.UpdatedUserID.ShouldBeNull();
            landingPageAssign.UpdatedDate.ShouldBeNull();
            landingPageAssign.BaseChannelDoesOverride.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}