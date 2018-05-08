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
    public class QuickTestBlastConfigTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (QuickTestBlastConfig) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var qTBCId = Fixture.Create<int>();
            var isDefault = Fixture.Create<bool?>();
            var baseChannelId = Fixture.Create<int?>();
            var baseChannelDoesOverride = Fixture.Create<bool?>();
            var customerCanOverride = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var customerDoesOverride = Fixture.Create<bool?>();
            var allowAdhocEmails = Fixture.Create<bool?>();
            var autoCreateGroup = Fixture.Create<bool?>();
            var autoArchiveGroup = Fixture.Create<bool?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();

            // Act
            quickTestBlastConfig.QTBCID = qTBCId;
            quickTestBlastConfig.IsDefault = isDefault;
            quickTestBlastConfig.BaseChannelID = baseChannelId;
            quickTestBlastConfig.BaseChannelDoesOverride = baseChannelDoesOverride;
            quickTestBlastConfig.CustomerCanOverride = customerCanOverride;
            quickTestBlastConfig.CustomerID = customerId;
            quickTestBlastConfig.CustomerDoesOverride = customerDoesOverride;
            quickTestBlastConfig.AllowAdhocEmails = allowAdhocEmails;
            quickTestBlastConfig.AutoCreateGroup = autoCreateGroup;
            quickTestBlastConfig.AutoArchiveGroup = autoArchiveGroup;
            quickTestBlastConfig.CreatedUserID = createdUserId;
            quickTestBlastConfig.CreatedDate = createdDate;
            quickTestBlastConfig.UpdatedUserID = updatedUserId;
            quickTestBlastConfig.UpdatedDate = updatedDate;

            // Assert
            quickTestBlastConfig.QTBCID.ShouldBe(qTBCId);
            quickTestBlastConfig.IsDefault.ShouldBe(isDefault);
            quickTestBlastConfig.BaseChannelID.ShouldBe(baseChannelId);
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBe(baseChannelDoesOverride);
            quickTestBlastConfig.CustomerCanOverride.ShouldBe(customerCanOverride);
            quickTestBlastConfig.CustomerID.ShouldBe(customerId);
            quickTestBlastConfig.CustomerDoesOverride.ShouldBe(customerDoesOverride);
            quickTestBlastConfig.AllowAdhocEmails.ShouldBe(allowAdhocEmails);
            quickTestBlastConfig.AutoCreateGroup.ShouldBe(autoCreateGroup);
            quickTestBlastConfig.AutoArchiveGroup.ShouldBe(autoArchiveGroup);
            quickTestBlastConfig.CreatedUserID.ShouldBe(createdUserId);
            quickTestBlastConfig.CreatedDate.ShouldBe(createdDate);
            quickTestBlastConfig.UpdatedUserID.ShouldBe(updatedUserId);
            quickTestBlastConfig.UpdatedDate.ShouldBe(updatedDate);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AllowAdhocEmails) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AllowAdhocEmails_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.AllowAdhocEmails = random;

            // Assert
            quickTestBlastConfig.AllowAdhocEmails.ShouldBe(random);
            quickTestBlastConfig.AllowAdhocEmails.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AllowAdhocEmails_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.AllowAdhocEmails = null;

            // Assert
            quickTestBlastConfig.AllowAdhocEmails.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AllowAdhocEmails_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAllowAdhocEmails = "AllowAdhocEmails";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameAllowAdhocEmails);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.AllowAdhocEmails.ShouldBeNull();
            quickTestBlastConfig.AllowAdhocEmails.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AllowAdhocEmails) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_AllowAdhocEmailsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAllowAdhocEmails = "AllowAdhocEmailsNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameAllowAdhocEmails));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AllowAdhocEmails_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAllowAdhocEmails = "AllowAdhocEmails";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameAllowAdhocEmails);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AutoArchiveGroup) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoArchiveGroup_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.AutoArchiveGroup = random;

            // Assert
            quickTestBlastConfig.AutoArchiveGroup.ShouldBe(random);
            quickTestBlastConfig.AutoArchiveGroup.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoArchiveGroup_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.AutoArchiveGroup = null;

            // Assert
            quickTestBlastConfig.AutoArchiveGroup.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoArchiveGroup_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAutoArchiveGroup = "AutoArchiveGroup";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameAutoArchiveGroup);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.AutoArchiveGroup.ShouldBeNull();
            quickTestBlastConfig.AutoArchiveGroup.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AutoArchiveGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_AutoArchiveGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAutoArchiveGroup = "AutoArchiveGroupNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameAutoArchiveGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoArchiveGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAutoArchiveGroup = "AutoArchiveGroup";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameAutoArchiveGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AutoCreateGroup) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoCreateGroup_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.AutoCreateGroup = random;

            // Assert
            quickTestBlastConfig.AutoCreateGroup.ShouldBe(random);
            quickTestBlastConfig.AutoCreateGroup.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoCreateGroup_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.AutoCreateGroup = null;

            // Assert
            quickTestBlastConfig.AutoCreateGroup.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoCreateGroup_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAutoCreateGroup = "AutoCreateGroup";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameAutoCreateGroup);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.AutoCreateGroup.ShouldBeNull();
            quickTestBlastConfig.AutoCreateGroup.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (AutoCreateGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_AutoCreateGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAutoCreateGroup = "AutoCreateGroupNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameAutoCreateGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_AutoCreateGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAutoCreateGroup = "AutoCreateGroup";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameAutoCreateGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (BaseChannelDoesOverride) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelDoesOverride_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.BaseChannelDoesOverride = random;

            // Assert
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBe(random);
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelDoesOverride_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.BaseChannelDoesOverride = null;

            // Assert
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelDoesOverride_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelDoesOverride = "BaseChannelDoesOverride";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelDoesOverride);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBeNull();
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (BaseChannelDoesOverride) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_BaseChannelDoesOverrideNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelDoesOverride = "BaseChannelDoesOverrideNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelDoesOverride));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelDoesOverride_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelDoesOverride = "BaseChannelDoesOverride";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelDoesOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            quickTestBlastConfig.BaseChannelID = random;

            // Assert
            quickTestBlastConfig.BaseChannelID.ShouldBe(random);
            quickTestBlastConfig.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.BaseChannelID = null;

            // Assert
            quickTestBlastConfig.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.BaseChannelID.ShouldBeNull();
            quickTestBlastConfig.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quickTestBlastConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            quickTestBlastConfig.CreatedUserID = random;

            // Assert
            quickTestBlastConfig.CreatedUserID.ShouldBe(random);
            quickTestBlastConfig.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.CreatedUserID = null;

            // Assert
            quickTestBlastConfig.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.CreatedUserID.ShouldBeNull();
            quickTestBlastConfig.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerCanOverride) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerCanOverride_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.CustomerCanOverride = random;

            // Assert
            quickTestBlastConfig.CustomerCanOverride.ShouldBe(random);
            quickTestBlastConfig.CustomerCanOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerCanOverride_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.CustomerCanOverride = null;

            // Assert
            quickTestBlastConfig.CustomerCanOverride.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerCanOverride_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerCanOverride = "CustomerCanOverride";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerCanOverride);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.CustomerCanOverride.ShouldBeNull();
            quickTestBlastConfig.CustomerCanOverride.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerCanOverride) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_CustomerCanOverrideNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerCanOverride = "CustomerCanOverrideNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerCanOverride));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerCanOverride_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerCanOverride = "CustomerCanOverride";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerCanOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerDoesOverride) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerDoesOverride_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.CustomerDoesOverride = random;

            // Assert
            quickTestBlastConfig.CustomerDoesOverride.ShouldBe(random);
            quickTestBlastConfig.CustomerDoesOverride.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerDoesOverride_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.CustomerDoesOverride = null;

            // Assert
            quickTestBlastConfig.CustomerDoesOverride.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerDoesOverride_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerDoesOverride = "CustomerDoesOverride";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerDoesOverride);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.CustomerDoesOverride.ShouldBeNull();
            quickTestBlastConfig.CustomerDoesOverride.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerDoesOverride) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_CustomerDoesOverrideNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerDoesOverride = "CustomerDoesOverrideNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerDoesOverride));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerDoesOverride_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerDoesOverride = "CustomerDoesOverride";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerDoesOverride);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            quickTestBlastConfig.CustomerID = random;

            // Assert
            quickTestBlastConfig.CustomerID.ShouldBe(random);
            quickTestBlastConfig.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.CustomerID = null;

            // Assert
            quickTestBlastConfig.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.CustomerID.ShouldBeNull();
            quickTestBlastConfig.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (IsDefault) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_IsDefault_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quickTestBlastConfig.IsDefault = random;

            // Assert
            quickTestBlastConfig.IsDefault.ShouldBe(random);
            quickTestBlastConfig.IsDefault.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_IsDefault_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.IsDefault = null;

            // Assert
            quickTestBlastConfig.IsDefault.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_IsDefault_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDefault = "IsDefault";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameIsDefault);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.IsDefault.ShouldBeNull();
            quickTestBlastConfig.IsDefault.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (IsDefault) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_IsDefaultNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefaultNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameIsDefault));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_IsDefault_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDefault = "IsDefault";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameIsDefault);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (QTBCID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_QTBCID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            quickTestBlastConfig.QTBCID = Fixture.Create<int>();
            var intType = quickTestBlastConfig.QTBCID.GetType();

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

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (QTBCID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_QTBCIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQTBCID = "QTBCIDNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameQTBCID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_QTBCID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQTBCID = "QTBCID";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameQTBCID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quickTestBlastConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            quickTestBlastConfig.UpdatedUserID = random;

            // Assert
            quickTestBlastConfig.UpdatedUserID.ShouldBe(random);
            quickTestBlastConfig.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();

            // Act , Set
            quickTestBlastConfig.UpdatedUserID = null;

            // Assert
            quickTestBlastConfig.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var quickTestBlastConfig = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo = quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(quickTestBlastConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quickTestBlastConfig.UpdatedUserID.ShouldBeNull();
            quickTestBlastConfig.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuickTestBlastConfig) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();

            // Act , Assert
            Should.NotThrow(() => quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuickTestBlastConfig_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var quickTestBlastConfig  = Fixture.Create<QuickTestBlastConfig>();
            var propertyInfo  = quickTestBlastConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (QuickTestBlastConfig) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuickTestBlastConfig_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new QuickTestBlastConfig());
        }

        #endregion

        #region General Constructor : Class (QuickTestBlastConfig) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuickTestBlastConfig_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfQuickTestBlastConfig = Fixture.CreateMany<QuickTestBlastConfig>(2).ToList();
            var firstQuickTestBlastConfig = instancesOfQuickTestBlastConfig.FirstOrDefault();
            var lastQuickTestBlastConfig = instancesOfQuickTestBlastConfig.Last();

            // Act, Assert
            firstQuickTestBlastConfig.ShouldNotBeNull();
            lastQuickTestBlastConfig.ShouldNotBeNull();
            firstQuickTestBlastConfig.ShouldNotBeSameAs(lastQuickTestBlastConfig);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuickTestBlastConfig_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstQuickTestBlastConfig = new QuickTestBlastConfig();
            var secondQuickTestBlastConfig = new QuickTestBlastConfig();
            var thirdQuickTestBlastConfig = new QuickTestBlastConfig();
            var fourthQuickTestBlastConfig = new QuickTestBlastConfig();
            var fifthQuickTestBlastConfig = new QuickTestBlastConfig();
            var sixthQuickTestBlastConfig = new QuickTestBlastConfig();

            // Act, Assert
            firstQuickTestBlastConfig.ShouldNotBeNull();
            secondQuickTestBlastConfig.ShouldNotBeNull();
            thirdQuickTestBlastConfig.ShouldNotBeNull();
            fourthQuickTestBlastConfig.ShouldNotBeNull();
            fifthQuickTestBlastConfig.ShouldNotBeNull();
            sixthQuickTestBlastConfig.ShouldNotBeNull();
            firstQuickTestBlastConfig.ShouldNotBeSameAs(secondQuickTestBlastConfig);
            thirdQuickTestBlastConfig.ShouldNotBeSameAs(firstQuickTestBlastConfig);
            fourthQuickTestBlastConfig.ShouldNotBeSameAs(firstQuickTestBlastConfig);
            fifthQuickTestBlastConfig.ShouldNotBeSameAs(firstQuickTestBlastConfig);
            sixthQuickTestBlastConfig.ShouldNotBeSameAs(firstQuickTestBlastConfig);
            sixthQuickTestBlastConfig.ShouldNotBeSameAs(fourthQuickTestBlastConfig);
        }

        #endregion

        #region General Constructor : Class (QuickTestBlastConfig) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuickTestBlastConfig_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var qTBCId = -1;
            var allowAdhocEmails = false;
            var autoCreateGroup = false;
            var autoArchiveGroup = false;

            // Act
            var quickTestBlastConfig = new QuickTestBlastConfig();

            // Assert
            quickTestBlastConfig.QTBCID.ShouldBe(qTBCId);
            quickTestBlastConfig.IsDefault.ShouldBeNull();
            quickTestBlastConfig.BaseChannelID.ShouldBeNull();
            quickTestBlastConfig.BaseChannelDoesOverride.ShouldBeNull();
            quickTestBlastConfig.CustomerCanOverride.ShouldBeNull();
            quickTestBlastConfig.CustomerID.ShouldBeNull();
            quickTestBlastConfig.CustomerDoesOverride.ShouldBeNull();
            quickTestBlastConfig.AllowAdhocEmails.ShouldBe(allowAdhocEmails);
            quickTestBlastConfig.AutoCreateGroup.ShouldBe(autoCreateGroup);
            quickTestBlastConfig.AutoArchiveGroup.ShouldBe(autoArchiveGroup);
            quickTestBlastConfig.CreatedUserID.ShouldBeNull();
            quickTestBlastConfig.CreatedDate.ShouldBeNull();
            quickTestBlastConfig.UpdatedUserID.ShouldBeNull();
            quickTestBlastConfig.UpdatedDate.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}