using System;
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
    public class SFSettingsTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var sFSettings  = new SFSettings();
            var sFSettingsID = Fixture.Create<int>();
            var baseChannelID = Fixture.Create<int?>();
            var customerID = Fixture.Create<int?>();
            var customerCanOverride = Fixture.Create<bool?>();
            var customerDoesOverride = Fixture.Create<bool?>();
            var refreshToken = Fixture.Create<string>();
            var consumerSecret = Fixture.Create<string>();
            var consumerKey = Fixture.Create<string>();
            var pushChannelMasterSuppression = Fixture.Create<bool?>();
            var sandboxMode = Fixture.Create<bool?>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();

            // Act
            sFSettings.SFSettingsID = sFSettingsID;
            sFSettings.BaseChannelID = baseChannelID;
            sFSettings.CustomerID = customerID;
            sFSettings.CustomerCanOverride = customerCanOverride;
            sFSettings.CustomerDoesOverride = customerDoesOverride;
            sFSettings.RefreshToken = refreshToken;
            sFSettings.ConsumerSecret = consumerSecret;
            sFSettings.ConsumerKey = consumerKey;
            sFSettings.PushChannelMasterSuppression = pushChannelMasterSuppression;
            sFSettings.SandboxMode = sandboxMode;
            sFSettings.CreatedUserID = createdUserID;
            sFSettings.UpdatedUserID = updatedUserID;

            // Assert
            sFSettings.SFSettingsID.ShouldBe(sFSettingsID);
            sFSettings.BaseChannelID.ShouldBe(baseChannelID);
            sFSettings.CustomerID.ShouldBe(customerID);
            sFSettings.CustomerCanOverride.ShouldBe(customerCanOverride);
            sFSettings.CustomerDoesOverride.ShouldBe(customerDoesOverride);
            sFSettings.RefreshToken.ShouldBe(refreshToken);
            sFSettings.ConsumerSecret.ShouldBe(consumerSecret);
            sFSettings.ConsumerKey.ShouldBe(consumerKey);
            sFSettings.PushChannelMasterSuppression.ShouldBe(pushChannelMasterSuppression);
            sFSettings.SandboxMode.ShouldBe(sandboxMode);
            sFSettings.CreatedUserID.ShouldBe(createdUserID);
            sFSettings.CreatedDate.ShouldBeNull();
            sFSettings.UpdatedUserID.ShouldBe(updatedUserID);
            sFSettings.UpdatedDate.ShouldBeNull();
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : SFSettings => CustomerCanOverride

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sFSettings.CustomerCanOverride = random;

            // Assert
            sFSettings.CustomerCanOverride.ShouldBe(random);
            sFSettings.CustomerCanOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.CustomerCanOverride = null;

            // Assert
            sFSettings.CustomerCanOverride.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerCanOverride = "CustomerCanOverride";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constCustomerCanOverride);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.CustomerCanOverride.ShouldBeNull();
            sFSettings.CustomerCanOverride.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_CustomerCanOverride_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerCanOverride = "CustomerCanOverride";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constCustomerCanOverride));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerCanOverride_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerCanOverride = "CustomerCanOverride";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constCustomerCanOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => CustomerDoesOverride

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sFSettings.CustomerDoesOverride = random;

            // Assert
            sFSettings.CustomerDoesOverride.ShouldBe(random);
            sFSettings.CustomerDoesOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.CustomerDoesOverride = null;

            // Assert
            sFSettings.CustomerDoesOverride.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constCustomerDoesOverride);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.CustomerDoesOverride.ShouldBeNull();
            sFSettings.CustomerDoesOverride.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_CustomerDoesOverride_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constCustomerDoesOverride));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerDoesOverride_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerDoesOverride = "CustomerDoesOverride";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constCustomerDoesOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => PushChannelMasterSuppression

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PushChannelMasterSuppression_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sFSettings.PushChannelMasterSuppression = random;

            // Assert
            sFSettings.PushChannelMasterSuppression.ShouldBe(random);
            sFSettings.PushChannelMasterSuppression.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PushChannelMasterSuppression_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.PushChannelMasterSuppression = null;

            // Assert
            sFSettings.PushChannelMasterSuppression.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PushChannelMasterSuppression_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constPushChannelMasterSuppression = "PushChannelMasterSuppression";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constPushChannelMasterSuppression);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.PushChannelMasterSuppression.ShouldBeNull();
            sFSettings.PushChannelMasterSuppression.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_PushChannelMasterSuppression_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constPushChannelMasterSuppression = "PushChannelMasterSuppression";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constPushChannelMasterSuppression));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PushChannelMasterSuppression_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constPushChannelMasterSuppression = "PushChannelMasterSuppression";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constPushChannelMasterSuppression);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => SandboxMode

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SandboxMode_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<bool>();

            // Act , Set
            sFSettings.SandboxMode = random;

            // Assert
            sFSettings.SandboxMode.ShouldBe(random);
            sFSettings.SandboxMode.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SandboxMode_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.SandboxMode = null;

            // Assert
            sFSettings.SandboxMode.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SandboxMode_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constSandboxMode = "SandboxMode";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constSandboxMode);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.SandboxMode.ShouldBeNull();
            sFSettings.SandboxMode.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_SandboxMode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSandboxMode = "SandboxMode";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constSandboxMode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SandboxMode_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constSandboxMode = "SandboxMode";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constSandboxMode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SFSettings => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var sFSettings = Fixture.Create<SFSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = sFSettings.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(sFSettings, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SFSettings => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var sFSettings = Fixture.Create<SFSettings>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = sFSettings.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(sFSettings, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SFSettings => SFSettingsID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SFSettingsID_Int_Type_Verify_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var intType = sFSettings.SFSettingsID.GetType();

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
        public void SFSettings_Class_Invalid_Property_SFSettingsID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSFSettingsID = "SFSettingsID";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constSFSettingsID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SFSettingsID_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constSFSettingsID = "SFSettingsID";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constSFSettingsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => BaseChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            sFSettings.BaseChannelID = random;

            // Assert
            sFSettings.BaseChannelID.ShouldBe(random);
            sFSettings.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.BaseChannelID = null;

            // Assert
            sFSettings.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constBaseChannelID = "BaseChannelID";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.BaseChannelID.ShouldBeNull();
            sFSettings.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_BaseChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constBaseChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            sFSettings.CreatedUserID = random;

            // Assert
            sFSettings.CreatedUserID.ShouldBe(random);
            sFSettings.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.CreatedUserID = null;

            // Assert
            sFSettings.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.CreatedUserID.ShouldBeNull();
            sFSettings.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            sFSettings.CustomerID = random;

            // Assert
            sFSettings.CustomerID.ShouldBe(random);
            sFSettings.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.CustomerID = null;

            // Assert
            sFSettings.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerID = "CustomerID";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constCustomerID);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.CustomerID.ShouldBeNull();
            sFSettings.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SFSettings => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var random = Fixture.Create<int>();

            // Act , Set
            sFSettings.UpdatedUserID = random;

            // Assert
            sFSettings.UpdatedUserID.ShouldBe(random);
            sFSettings.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();    

            // Act , Set
            sFSettings.UpdatedUserID = null;

            // Assert
            sFSettings.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var sFSettings = Fixture.Create<SFSettings>();
            var propertyInfo = sFSettings.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(sFSettings, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            sFSettings.UpdatedUserID.ShouldBeNull();
            sFSettings.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SFSettings_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SFSettings => ConsumerKey

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ConsumerKey_String_Type_Verify_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var stringType = sFSettings.ConsumerKey.GetType();

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
        public void SFSettings_Class_Invalid_Property_ConsumerKey_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constConsumerKey = "ConsumerKey";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constConsumerKey));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ConsumerKey_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constConsumerKey = "ConsumerKey";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constConsumerKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SFSettings => ConsumerSecret

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ConsumerSecret_String_Type_Verify_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var stringType = sFSettings.ConsumerSecret.GetType();

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
        public void SFSettings_Class_Invalid_Property_ConsumerSecret_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constConsumerSecret = "ConsumerSecret";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constConsumerSecret));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ConsumerSecret_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constConsumerSecret = "ConsumerSecret";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constConsumerSecret);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SFSettings => RefreshToken

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RefreshToken_String_Type_Verify_Test()
        {
            // Arrange
            var sFSettings = Fixture.Create<SFSettings>();
            var stringType = sFSettings.RefreshToken.GetType();

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
        public void SFSettings_Class_Invalid_Property_RefreshToken_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRefreshToken = "RefreshToken";
            var sFSettings  = Fixture.Create<SFSettings>();

            // Act , Assert
            Should.NotThrow(() => sFSettings.GetType().GetProperty(constRefreshToken));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RefreshToken_Is_Present_In_SFSettings_Class_As_Public_Test()
        {
            // Arrange
            const string constRefreshToken = "RefreshToken";
            var sFSettings  = Fixture.Create<SFSettings>();
            var propertyInfo  = sFSettings.GetType().GetProperty(constRefreshToken);

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
            Should.NotThrow(() => new SFSettings());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SFSettings>(2).ToList();
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
            var sFSettingsID = -1;
            int? customerID = null;
            int? baseChannelID = null;
            bool? customerCanOverride = null;
            bool? customerDoesOverride = null;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            var refreshToken = string.Empty;
            var sandboxMode = false;
            var pushChannelMasterSuppression = false;
            var consumerSecret = string.Empty;
            var consumerKey = string.Empty;    

            // Act
            var sFSettings = new SFSettings();    

            // Assert
            sFSettings.SFSettingsID.ShouldBe(sFSettingsID);
            sFSettings.CustomerID.ShouldBeNull();
            sFSettings.BaseChannelID.ShouldBeNull();
            sFSettings.CustomerCanOverride.ShouldBeNull();
            sFSettings.CustomerDoesOverride.ShouldBeNull();
            sFSettings.CreatedUserID.ShouldBeNull();
            sFSettings.CreatedDate.ShouldBeNull();
            sFSettings.UpdatedUserID.ShouldBeNull();
            sFSettings.UpdatedDate.ShouldBeNull();
            sFSettings.RefreshToken.ShouldBe(refreshToken);
            sFSettings.SandboxMode.ShouldBe(sandboxMode);
            sFSettings.PushChannelMasterSuppression.ShouldBe(pushChannelMasterSuppression);
            sFSettings.ConsumerSecret.ShouldBe(consumerSecret);
            sFSettings.ConsumerKey.ShouldBe(consumerKey);
        }

        #endregion

        #endregion

        #endregion
    }
}