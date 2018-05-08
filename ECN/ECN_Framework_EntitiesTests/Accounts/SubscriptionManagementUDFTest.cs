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
    public class SubsriptionManagementUDFTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var subsriptionManagementUDF  = new SubsriptionManagementUDF();
            var subscriptionManagementUDFID = Fixture.Create<int>();
            var subscriptionManagementGroupID = Fixture.Create<int>();
            var groupDataFieldsID = Fixture.Create<int>();
            var staticValue = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();

            // Act
            subsriptionManagementUDF.SubscriptionManagementUDFID = subscriptionManagementUDFID;
            subsriptionManagementUDF.SubscriptionManagementGroupID = subscriptionManagementGroupID;
            subsriptionManagementUDF.GroupDataFieldsID = groupDataFieldsID;
            subsriptionManagementUDF.StaticValue = staticValue;
            subsriptionManagementUDF.IsDeleted = isDeleted;
            subsriptionManagementUDF.CreatedUserID = createdUserID;
            subsriptionManagementUDF.CreatedDate = createdDate;
            subsriptionManagementUDF.UpdatedUserID = updatedUserID;
            subsriptionManagementUDF.UpdatedDate = updatedDate;

            // Assert
            subsriptionManagementUDF.SubscriptionManagementUDFID.ShouldBe(subscriptionManagementUDFID);
            subsriptionManagementUDF.SubscriptionManagementGroupID.ShouldBe(subscriptionManagementGroupID);
            subsriptionManagementUDF.GroupDataFieldsID.ShouldBe(groupDataFieldsID);
            subsriptionManagementUDF.StaticValue.ShouldBe(staticValue);
            subsriptionManagementUDF.IsDeleted.ShouldBe(isDeleted);
            subsriptionManagementUDF.CreatedUserID.ShouldBe(createdUserID);
            subsriptionManagementUDF.CreatedDate.ShouldBe(createdDate);
            subsriptionManagementUDF.UpdatedUserID.ShouldBe(updatedUserID);
            subsriptionManagementUDF.UpdatedDate.ShouldBe(updatedDate);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : SubsriptionManagementUDF => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Bool_Type_Verify_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var boolType = subsriptionManagementUDF.IsDeleted.GetType();

            // Act
            var isTypeBool = typeof(bool).Equals(boolType);    
            var isTypeNullableBool = typeof(bool?).Equals(boolType);
            var isTypeString = typeof(string).Equals(boolType);
            var isTypeInt = typeof(int).Equals(boolType);
            var isTypeDecimal = typeof(decimal).Equals(boolType);
            var isTypeLong = typeof(long).Equals(boolType);
            var isTypeDouble = typeof(double).Equals(boolType);
            var isTypeFloat = typeof(float).Equals(boolType);
            var isTypeIntNullable = typeof(int?).Equals(boolType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(boolType);
            var isTypeLongNullable = typeof(long?).Equals(boolType);
            var isTypeDoubleNullable = typeof(double?).Equals(boolType);
            var isTypeFloatNullable = typeof(float?).Equals(boolType);


            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubsriptionManagementUDF => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subsriptionManagementUDF.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subsriptionManagementUDF, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : SubsriptionManagementUDF => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = subsriptionManagementUDF.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(subsriptionManagementUDF, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubsriptionManagementUDF => GroupDataFieldsID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_GroupDataFieldsID_Int_Type_Verify_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var intType = subsriptionManagementUDF.GroupDataFieldsID.GetType();

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
        public void SubsriptionManagementUDF_Class_Invalid_Property_GroupDataFieldsID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constGroupDataFieldsID = "GroupDataFieldsID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constGroupDataFieldsID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_GroupDataFieldsID_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constGroupDataFieldsID = "GroupDataFieldsID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constGroupDataFieldsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubsriptionManagementUDF => SubscriptionManagementGroupID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementGroupID_Int_Type_Verify_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var intType = subsriptionManagementUDF.SubscriptionManagementGroupID.GetType();

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
        public void SubsriptionManagementUDF_Class_Invalid_Property_SubscriptionManagementGroupID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSubscriptionManagementGroupID = "SubscriptionManagementGroupID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constSubscriptionManagementGroupID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementGroupID_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constSubscriptionManagementGroupID = "SubscriptionManagementGroupID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constSubscriptionManagementGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : SubsriptionManagementUDF => SubscriptionManagementUDFID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementUDFID_Int_Type_Verify_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var intType = subsriptionManagementUDF.SubscriptionManagementUDFID.GetType();

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
        public void SubsriptionManagementUDF_Class_Invalid_Property_SubscriptionManagementUDFID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSubscriptionManagementUDFID = "SubscriptionManagementUDFID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constSubscriptionManagementUDFID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SubscriptionManagementUDFID_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constSubscriptionManagementUDFID = "SubscriptionManagementUDFID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constSubscriptionManagementUDFID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubsriptionManagementUDF => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var random = Fixture.Create<int>();

            // Act , Set
            subsriptionManagementUDF.CreatedUserID = random;

            // Assert
            subsriptionManagementUDF.CreatedUserID.ShouldBe(random);
            subsriptionManagementUDF.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();    

            // Act , Set
            subsriptionManagementUDF.CreatedUserID = null;

            // Assert
            subsriptionManagementUDF.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo = subsriptionManagementUDF.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(subsriptionManagementUDF, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subsriptionManagementUDF.CreatedUserID.ShouldBeNull();
            subsriptionManagementUDF.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : SubsriptionManagementUDF => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var random = Fixture.Create<int>();

            // Act , Set
            subsriptionManagementUDF.UpdatedUserID = random;

            // Assert
            subsriptionManagementUDF.UpdatedUserID.ShouldBe(random);
            subsriptionManagementUDF.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();    

            // Act , Set
            subsriptionManagementUDF.UpdatedUserID = null;

            // Assert
            subsriptionManagementUDF.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo = subsriptionManagementUDF.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(subsriptionManagementUDF, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            subsriptionManagementUDF.UpdatedUserID.ShouldBeNull();
            subsriptionManagementUDF.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SubsriptionManagementUDF_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : SubsriptionManagementUDF => StaticValue

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StaticValue_String_Type_Verify_Test()
        {
            // Arrange
            var subsriptionManagementUDF = Fixture.Create<SubsriptionManagementUDF>();
            var stringType = subsriptionManagementUDF.StaticValue.GetType();

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
        public void SubsriptionManagementUDF_Class_Invalid_Property_StaticValue_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStaticValue = "StaticValue";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();

            // Act , Assert
            Should.NotThrow(() => subsriptionManagementUDF.GetType().GetProperty(constStaticValue));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StaticValue_Is_Present_In_SubsriptionManagementUDF_Class_As_Public_Test()
        {
            // Arrange
            const string constStaticValue = "StaticValue";
            var subsriptionManagementUDF  = Fixture.Create<SubsriptionManagementUDF>();
            var propertyInfo  = subsriptionManagementUDF.GetType().GetProperty(constStaticValue);

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
            Should.NotThrow(() => new SubsriptionManagementUDF());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<SubsriptionManagementUDF>(2).ToList();
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
            var subscriptionManagementUDFID = -1;
            var subscriptionManagementGroupID = -1;
            var groupDataFieldsID = -1;
            var staticValue = string.Empty;
            var isDeleted = false;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;    

            // Act
            var subsriptionManagementUDF = new SubsriptionManagementUDF();    

            // Assert
            subsriptionManagementUDF.SubscriptionManagementUDFID.ShouldBe(subscriptionManagementUDFID);
            subsriptionManagementUDF.SubscriptionManagementGroupID.ShouldBe(subscriptionManagementGroupID);
            subsriptionManagementUDF.GroupDataFieldsID.ShouldBe(groupDataFieldsID);
            subsriptionManagementUDF.StaticValue.ShouldBe(staticValue);
            subsriptionManagementUDF.IsDeleted.ShouldBeFalse();
            subsriptionManagementUDF.CreatedUserID.ShouldBeNull();
            subsriptionManagementUDF.CreatedDate.ShouldBeNull();
            subsriptionManagementUDF.UpdatedUserID.ShouldBeNull();
            subsriptionManagementUDF.UpdatedDate.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}