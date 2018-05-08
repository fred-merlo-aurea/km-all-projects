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
    public class NotificationTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var notification  = new Notification();
            var backGroundColor = Fixture.Create<string>();
            var closeButtonColor = Fixture.Create<string>();
            var notificationID = Fixture.Create<int>();
            var notificationName = Fixture.Create<string>();
            var notificationText = Fixture.Create<string>();
            var startDate = Fixture.Create<string>();
            var startTime = Fixture.Create<string>();
            var endDate = Fixture.Create<string>();
            var endTime = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            notification.BackGroundColor = backGroundColor;
            notification.CloseButtonColor = closeButtonColor;
            notification.NotificationID = notificationID;
            notification.NotificationName = notificationName;
            notification.NotificationText = notificationText;
            notification.StartDate = startDate;
            notification.StartTime = startTime;
            notification.EndDate = endDate;
            notification.EndTime = endTime;
            notification.CreatedUserID = createdUserID;
            notification.UpdatedUserID = updatedUserID;
            notification.IsDeleted = isDeleted;

            // Assert
            notification.BackGroundColor.ShouldBe(backGroundColor);
            notification.CloseButtonColor.ShouldBe(closeButtonColor);
            notification.NotificationID.ShouldBe(notificationID);
            notification.NotificationName.ShouldBe(notificationName);
            notification.NotificationText.ShouldBe(notificationText);
            notification.StartDate.ShouldBe(startDate);
            notification.StartTime.ShouldBe(startTime);
            notification.EndDate.ShouldBe(endDate);
            notification.EndTime.ShouldBe(endTime);
            notification.CreatedUserID.ShouldBe(createdUserID);
            notification.UpdatedUserID.ShouldBe(updatedUserID);
            notification.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : Notification => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var random = Fixture.Create<bool>();

            // Act , Set
            notification.IsDeleted = random;

            // Assert
            notification.IsDeleted.ShouldBe(random);
            notification.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();    

            // Act , Set
            notification.IsDeleted = null;

            // Assert
            notification.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var notification = Fixture.Create<Notification>();
            var propertyInfo = notification.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(notification, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            notification.IsDeleted.ShouldBeNull();
            notification.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Notification => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var notification = Fixture.Create<Notification>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = notification.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(notification, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Notification => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var notification = Fixture.Create<Notification>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = notification.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(notification, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Notification => NotificationID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationID_Int_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var intType = notification.NotificationID.GetType();

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
        public void Notification_Class_Invalid_Property_NotificationID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constNotificationID = "NotificationID";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constNotificationID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationID_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constNotificationID = "NotificationID";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constNotificationID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Notification => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var random = Fixture.Create<int>();

            // Act , Set
            notification.CreatedUserID = random;

            // Assert
            notification.CreatedUserID.ShouldBe(random);
            notification.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();    

            // Act , Set
            notification.CreatedUserID = null;

            // Assert
            notification.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var notification = Fixture.Create<Notification>();
            var propertyInfo = notification.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(notification, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            notification.CreatedUserID.ShouldBeNull();
            notification.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Notification => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var random = Fixture.Create<int>();

            // Act , Set
            notification.UpdatedUserID = random;

            // Assert
            notification.UpdatedUserID.ShouldBe(random);
            notification.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();    

            // Act , Set
            notification.UpdatedUserID = null;

            // Assert
            notification.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var notification = Fixture.Create<Notification>();
            var propertyInfo = notification.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(notification, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            notification.UpdatedUserID.ShouldBeNull();
            notification.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Notification_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => BackGroundColor

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BackGroundColor_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.BackGroundColor.GetType();

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
        public void Notification_Class_Invalid_Property_BackGroundColor_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBackGroundColor = "BackGroundColor";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constBackGroundColor));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BackGroundColor_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constBackGroundColor = "BackGroundColor";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constBackGroundColor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => CloseButtonColor

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CloseButtonColor_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.CloseButtonColor.GetType();

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
        public void Notification_Class_Invalid_Property_CloseButtonColor_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCloseButtonColor = "CloseButtonColor";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constCloseButtonColor));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CloseButtonColor_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constCloseButtonColor = "CloseButtonColor";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constCloseButtonColor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => EndDate

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndDate_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.EndDate.GetType();

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
        public void Notification_Class_Invalid_Property_EndDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEndDate = "EndDate";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constEndDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndDate_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constEndDate = "EndDate";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => EndTime

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndTime_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.EndTime.GetType();

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
        public void Notification_Class_Invalid_Property_EndTime_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEndTime = "EndTime";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constEndTime));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_EndTime_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constEndTime = "EndTime";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constEndTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => NotificationName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationName_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.NotificationName.GetType();

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
        public void Notification_Class_Invalid_Property_NotificationName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constNotificationName = "NotificationName";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constNotificationName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationName_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constNotificationName = "NotificationName";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constNotificationName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => NotificationText

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationText_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.NotificationText.GetType();

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
        public void Notification_Class_Invalid_Property_NotificationText_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constNotificationText = "NotificationText";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constNotificationText));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_NotificationText_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constNotificationText = "NotificationText";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constNotificationText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => StartDate

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartDate_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.StartDate.GetType();

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
        public void Notification_Class_Invalid_Property_StartDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStartDate = "StartDate";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constStartDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartDate_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constStartDate = "StartDate";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Notification => StartTime

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartTime_String_Type_Verify_Test()
        {
            // Arrange
            var notification = Fixture.Create<Notification>();
            var stringType = notification.StartTime.GetType();

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
        public void Notification_Class_Invalid_Property_StartTime_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStartTime = "StartTime";
            var notification  = Fixture.Create<Notification>();

            // Act , Assert
            Should.NotThrow(() => notification.GetType().GetProperty(constStartTime));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StartTime_Is_Present_In_Notification_Class_As_Public_Test()
        {
            // Arrange
            const string constStartTime = "StartTime";
            var notification  = Fixture.Create<Notification>();
            var propertyInfo  = notification.GetType().GetProperty(constStartTime);

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
            Should.NotThrow(() => new Notification());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Notification>(2).ToList();
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
            var notificationID = -1;
            var notificationName = string.Empty;
            var notificationText = string.Empty;
            string startDate = null;
            string startTime = null;
            string endDate = null;
            string endTime = null;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;
            string backGroundColor = null;
            string closeButtonColor = null;    

            // Act
            var notification = new Notification();    

            // Assert
            notification.NotificationID.ShouldBe(notificationID);
            notification.NotificationName.ShouldBe(notificationName);
            notification.NotificationText.ShouldBe(notificationText);
            notification.StartDate.ShouldBeNull();
            notification.StartTime.ShouldBeNull();
            notification.EndDate.ShouldBeNull();
            notification.EndTime.ShouldBeNull();
            notification.CreatedUserID.ShouldBeNull();
            notification.CreatedDate.ShouldBeNull();
            notification.UpdatedUserID.ShouldBeNull();
            notification.UpdatedDate.ShouldBeNull();
            notification.IsDeleted.ShouldBeNull();
            notification.BackGroundColor.ShouldBeNull();
            notification.CloseButtonColor.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}