using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class SubscriptionManagementReasonTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var subscriptionManagementReason  = new SubscriptionManagementReason();
            var subscriptionManagementReasonID = Fixture.Create<int>();
            var subscriptionManagementID = Fixture.Create<int>();
            var reason = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool?>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var sortOrder = Fixture.Create<int?>();

            // Act
            subscriptionManagementReason.SubscriptionManagementReasonID = subscriptionManagementReasonID;
            subscriptionManagementReason.SubscriptionManagementID = subscriptionManagementID;
            subscriptionManagementReason.Reason = reason;
            subscriptionManagementReason.IsDeleted = isDeleted;
            subscriptionManagementReason.CreatedUserID = createdUserID;
            subscriptionManagementReason.CreatedDate = createdDate;
            subscriptionManagementReason.UpdatedUserID = updatedUserID;
            subscriptionManagementReason.UpdatedDate = updatedDate;
            subscriptionManagementReason.SortOrder = sortOrder;

            // Assert
            subscriptionManagementReason.SubscriptionManagementReasonID.ShouldBe(subscriptionManagementReasonID);
            subscriptionManagementReason.SubscriptionManagementID.ShouldBe(subscriptionManagementID);
            subscriptionManagementReason.Reason.ShouldBe(reason);
            subscriptionManagementReason.IsDeleted.ShouldBe(isDeleted);
            subscriptionManagementReason.CreatedUserID.ShouldBe(createdUserID);
            subscriptionManagementReason.CreatedDate.ShouldBe(createdDate);
            subscriptionManagementReason.UpdatedUserID.ShouldBe(updatedUserID);
            subscriptionManagementReason.UpdatedDate.ShouldBe(updatedDate);
            subscriptionManagementReason.SortOrder.ShouldBe(sortOrder);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : SubscriptionManagementReason => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var random = Fixture.Create<bool>();

            // Act , Set
            subscriptionManagementReason.IsDeleted = random;

            // Assert
            subscriptionManagementReason.IsDeleted.ShouldBe(random);
            subscriptionManagementReason.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();    

            // Act , Set
            subscriptionManagementReason.IsDeleted = null;

            // Assert
            subscriptionManagementReason.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementReason, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementReason.IsDeleted.ShouldBeNull();
            subscriptionManagementReason.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubscriptionManagementReason => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagementReason, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubscriptionManagementReason => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subscriptionManagementReason, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubscriptionManagementReason => SubscriptionManagementID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementID_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var intType = subscriptionManagementReason.SubscriptionManagementID.GetType();

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
        public void SubscriptionManagementReason_Class_Invalid_Property_SubscriptionManagementID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSubscriptionManagementID = "SubscriptionManagementID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constSubscriptionManagementID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementID_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constSubscriptionManagementID = "SubscriptionManagementID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constSubscriptionManagementID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubscriptionManagementReason => SubscriptionManagementReasonID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementReasonID_Int_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var intType = subscriptionManagementReason.SubscriptionManagementReasonID.GetType();

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
        public void SubscriptionManagementReason_Class_Invalid_Property_SubscriptionManagementReasonID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSubscriptionManagementReasonID = "SubscriptionManagementReasonID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constSubscriptionManagementReasonID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementReasonID_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constSubscriptionManagementReasonID = "SubscriptionManagementReasonID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constSubscriptionManagementReasonID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagementReason => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagementReason.CreatedUserID = random;

            // Assert
            subscriptionManagementReason.CreatedUserID.ShouldBe(random);
            subscriptionManagementReason.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();    

            // Act , Set
            subscriptionManagementReason.CreatedUserID = null;

            // Assert
            subscriptionManagementReason.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementReason, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementReason.CreatedUserID.ShouldBeNull();
            subscriptionManagementReason.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagementReason => SortOrder

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagementReason.SortOrder = random;

            // Assert
            subscriptionManagementReason.SortOrder.ShouldBe(random);
            subscriptionManagementReason.SortOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();    

            // Act , Set
            subscriptionManagementReason.SortOrder = null;

            // Assert
            subscriptionManagementReason.SortOrder.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constSortOrder = "SortOrder";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constSortOrder);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementReason, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementReason.SortOrder.ShouldBeNull();
            subscriptionManagementReason.SortOrder.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_SortOrder_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSortOrder = "SortOrder";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constSortOrder));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constSortOrder = "SortOrder";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubscriptionManagementReason => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var random = Fixture.Create<int>();

            // Act , Set
            subscriptionManagementReason.UpdatedUserID = random;

            // Assert
            subscriptionManagementReason.UpdatedUserID.ShouldBe(random);
            subscriptionManagementReason.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();    

            // Act , Set
            subscriptionManagementReason.UpdatedUserID = null;

            // Assert
            subscriptionManagementReason.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo = subscriptionManagementReason.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(subscriptionManagementReason, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subscriptionManagementReason.UpdatedUserID.ShouldBeNull();
            subscriptionManagementReason.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubscriptionManagementReason_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubscriptionManagementReason => Reason

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Reason_String_Type_Verify_Test()
        {
            // Arrange
            var subscriptionManagementReason = Fixture.Create<SubscriptionManagementReason>();
            var stringType = subscriptionManagementReason.Reason.GetType();

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
        public void SubscriptionManagementReason_Class_Invalid_Property_Reason_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constReason = "Reason";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();

            // Act , Assert
            Should.NotThrow(() => subscriptionManagementReason.GetType().GetProperty(constReason));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Reason_Is_Present_In_SubscriptionManagementReason_Class_As_Public_Test()
        {
            // Arrange
            const string constReason = "Reason";
            var subscriptionManagementReason  = Fixture.Create<SubscriptionManagementReason>();
            var propertyInfo  = subscriptionManagementReason.GetType().GetProperty(constReason);

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
            Should.NotThrow(() => new SubscriptionManagementReason());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SubscriptionManagementReason>(2).ToList();
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
            var subscriptionManagementReasonID = -1;
            var subscriptionManagementID = -1;
            var reason = string.Empty;
            var isDeleted = false;
            int? createdUserID = null;
            DateTime? createdDate = null;
            DateTime? updatedDate = null;
            int? updatedUserID = null;
            int? sortOrder = null;    

            // Act
            var subscriptionManagementReason = new SubscriptionManagementReason();    

            // Assert
            subscriptionManagementReason.SubscriptionManagementReasonID.ShouldBe(subscriptionManagementReasonID);
            subscriptionManagementReason.SubscriptionManagementID.ShouldBe(subscriptionManagementID);
            subscriptionManagementReason.Reason.ShouldBe(reason);
            subscriptionManagementReason.IsDeleted.ShouldBe(isDeleted);
            subscriptionManagementReason.CreatedUserID.ShouldBeNull();
            subscriptionManagementReason.CreatedDate.ShouldBeNull();
            subscriptionManagementReason.UpdatedDate.ShouldBeNull();
            subscriptionManagementReason.UpdatedUserID.ShouldBeNull();
            subscriptionManagementReason.SortOrder.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}