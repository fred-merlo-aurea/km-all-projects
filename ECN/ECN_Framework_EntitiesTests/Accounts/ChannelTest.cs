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
    public class ChannelTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var channel  = new Channel();
            var channelID = Fixture.Create<int>();
            var baseChannelID = Fixture.Create<int?>();
            var channelName = Fixture.Create<string>();
            var assetsPath = Fixture.Create<string>();
            var virtualPath = Fixture.Create<string>();
            var pickupPath = Fixture.Create<string>();
            var headerSource = Fixture.Create<string>();
            var footerSource = Fixture.Create<string>();
            var channelTypeCodeID = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode>();
            var channelTypeCode = Fixture.Create<string>();
            var active = Fixture.Create<string>();
            var mailingIP = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            channel.ChannelID = channelID;
            channel.BaseChannelID = baseChannelID;
            channel.ChannelName = channelName;
            channel.AssetsPath = assetsPath;
            channel.VirtualPath = virtualPath;
            channel.PickupPath = pickupPath;
            channel.HeaderSource = headerSource;
            channel.FooterSource = footerSource;
            channel.ChannelTypeCodeID = channelTypeCodeID;
            channel.ChannelTypeCode = channelTypeCode;
            channel.Active = active;
            channel.MailingIP = mailingIP;
            channel.CreatedUserID = createdUserID;
            channel.UpdatedUserID = updatedUserID;
            channel.IsDeleted = isDeleted;

            // Assert
            channel.ChannelID.ShouldBe(channelID);
            channel.BaseChannelID.ShouldBe(baseChannelID);
            channel.ChannelName.ShouldBe(channelName);
            channel.AssetsPath.ShouldBe(assetsPath);
            channel.VirtualPath.ShouldBe(virtualPath);
            channel.PickupPath.ShouldBe(pickupPath);
            channel.HeaderSource.ShouldBe(headerSource);
            channel.FooterSource.ShouldBe(footerSource);
            channel.ChannelTypeCodeID.ShouldBe(channelTypeCodeID);
            channel.ChannelTypeCode.ShouldBe(channelTypeCode);
            channel.Active.ShouldBe(active);
            channel.MailingIP.ShouldBe(mailingIP);
            channel.CreatedUserID.ShouldBe(createdUserID);
            channel.UpdatedUserID.ShouldBe(updatedUserID);
            channel.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : Channel => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var random = Fixture.Create<bool>();

            // Act , Set
            channel.IsDeleted = random;

            // Assert
            channel.IsDeleted.ShouldBe(random);
            channel.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();    

            // Act , Set
            channel.IsDeleted = null;

            // Assert
            channel.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var channel = Fixture.Create<Channel>();
            var propertyInfo = channel.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(channel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channel.IsDeleted.ShouldBeNull();
            channel.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Channel => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var channel = Fixture.Create<Channel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channel.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channel, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Channel => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var channel = Fixture.Create<Channel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channel.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channel, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Channel => ChannelTypeCodeID

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelTypeCodeID_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constChannelTypeCodeID = "ChannelTypeCodeID";
            var channel = Fixture.Create<Channel>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = channel.GetType().GetProperty(constChannelTypeCodeID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(channel, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_ChannelTypeCodeID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constChannelTypeCodeID = "ChannelTypeCodeID";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constChannelTypeCodeID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelTypeCodeID_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constChannelTypeCodeID = "ChannelTypeCodeID";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constChannelTypeCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Channel => ChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelID_Int_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var intType = channel.ChannelID.GetType();

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
        public void Channel_Class_Invalid_Property_ChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constChannelID = "ChannelID";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelID_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constChannelID = "ChannelID";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Channel => BaseChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Data_Without_Null_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var random = Fixture.Create<int>();

            // Act , Set
            channel.BaseChannelID = random;

            // Assert
            channel.BaseChannelID.ShouldBe(random);
            channel.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Only_Null_Data_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();    

            // Act , Set
            channel.BaseChannelID = null;

            // Assert
            channel.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constBaseChannelID = "BaseChannelID";
            var channel = Fixture.Create<Channel>();
            var propertyInfo = channel.GetType().GetProperty(constBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(channel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channel.BaseChannelID.ShouldBeNull();
            channel.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_BaseChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constBaseChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Channel => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var random = Fixture.Create<int>();

            // Act , Set
            channel.CreatedUserID = random;

            // Assert
            channel.CreatedUserID.ShouldBe(random);
            channel.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();    

            // Act , Set
            channel.CreatedUserID = null;

            // Assert
            channel.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var channel = Fixture.Create<Channel>();
            var propertyInfo = channel.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(channel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channel.CreatedUserID.ShouldBeNull();
            channel.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Channel => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var random = Fixture.Create<int>();

            // Act , Set
            channel.UpdatedUserID = random;

            // Assert
            channel.UpdatedUserID.ShouldBe(random);
            channel.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();    

            // Act , Set
            channel.UpdatedUserID = null;

            // Assert
            channel.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var channel = Fixture.Create<Channel>();
            var propertyInfo = channel.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(channel, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            channel.UpdatedUserID.ShouldBeNull();
            channel.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Channel_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => Active

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.Active.GetType();

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
        public void Channel_Class_Invalid_Property_Active_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constActive = "Active";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constActive = "Active";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => AssetsPath

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AssetsPath_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.AssetsPath.GetType();

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
        public void Channel_Class_Invalid_Property_AssetsPath_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAssetsPath = "AssetsPath";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constAssetsPath));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AssetsPath_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constAssetsPath = "AssetsPath";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constAssetsPath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => ChannelName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelName_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.ChannelName.GetType();

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
        public void Channel_Class_Invalid_Property_ChannelName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constChannelName = "ChannelName";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constChannelName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelName_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constChannelName = "ChannelName";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constChannelName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => ChannelTypeCode

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelTypeCode_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.ChannelTypeCode.GetType();

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
        public void Channel_Class_Invalid_Property_ChannelTypeCode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constChannelTypeCode = "ChannelTypeCode";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constChannelTypeCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ChannelTypeCode_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constChannelTypeCode = "ChannelTypeCode";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constChannelTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => FooterSource

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FooterSource_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.FooterSource.GetType();

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
        public void Channel_Class_Invalid_Property_FooterSource_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFooterSource = "FooterSource";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constFooterSource));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FooterSource_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constFooterSource = "FooterSource";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constFooterSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => HeaderSource

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HeaderSource_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.HeaderSource.GetType();

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
        public void Channel_Class_Invalid_Property_HeaderSource_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constHeaderSource = "HeaderSource";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constHeaderSource));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HeaderSource_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constHeaderSource = "HeaderSource";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constHeaderSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => MailingIP

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_MailingIP_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.MailingIP.GetType();

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
        public void Channel_Class_Invalid_Property_MailingIP_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constMailingIP = "MailingIP";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constMailingIP));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_MailingIP_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constMailingIP = "MailingIP";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constMailingIP);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => PickupPath

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PickupPath_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.PickupPath.GetType();

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
        public void Channel_Class_Invalid_Property_PickupPath_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constPickupPath = "PickupPath";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constPickupPath));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PickupPath_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constPickupPath = "PickupPath";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constPickupPath);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Channel => VirtualPath

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_VirtualPath_String_Type_Verify_Test()
        {
            // Arrange
            var channel = Fixture.Create<Channel>();
            var stringType = channel.VirtualPath.GetType();

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
        public void Channel_Class_Invalid_Property_VirtualPath_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constVirtualPath = "VirtualPath";
            var channel  = Fixture.Create<Channel>();

            // Act , Assert
            Should.NotThrow(() => channel.GetType().GetProperty(constVirtualPath));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_VirtualPath_Is_Present_In_Channel_Class_As_Public_Test()
        {
            // Arrange
            const string constVirtualPath = "VirtualPath";
            var channel  = Fixture.Create<Channel>();
            var propertyInfo  = channel.GetType().GetProperty(constVirtualPath);

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
            Should.NotThrow(() => new Channel());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Channel>(2).ToList();
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
            var channelID = -1;
            int? baseChannelID = null;
            var channelName = string.Empty;
            var assetsPath = string.Empty;
            var virtualPath = string.Empty;
            var pickupPath = string.Empty;
            var headerSource = string.Empty;
            var footerSource = string.Empty;
            var channelTypeCodeID = ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode.accounts;
            var channelTypeCode = string.Empty;
            var active = string.Empty;
            var mailingIP = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var channel = new Channel();    

            // Assert
            channel.ChannelID.ShouldBe(channelID);
            channel.BaseChannelID.ShouldBeNull();
            channel.ChannelName.ShouldBe(channelName);
            channel.AssetsPath.ShouldBe(assetsPath);
            channel.VirtualPath.ShouldBe(virtualPath);
            channel.PickupPath.ShouldBe(pickupPath);
            channel.HeaderSource.ShouldBe(headerSource);
            channel.FooterSource.ShouldBe(footerSource);
            channel.ChannelTypeCodeID.ShouldBe(channelTypeCodeID);
            channel.ChannelTypeCode.ShouldBe(channelTypeCode);
            channel.Active.ShouldBe(active);
            channel.MailingIP.ShouldBe(mailingIP);
            channel.CreatedUserID.ShouldBeNull();
            channel.CreatedDate.ShouldBeNull();
            channel.UpdatedUserID.ShouldBeNull();
            channel.UpdatedDate.ShouldBeNull();
            channel.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}