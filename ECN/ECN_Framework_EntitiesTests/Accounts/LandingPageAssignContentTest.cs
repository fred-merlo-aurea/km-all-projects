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
    public class LandingPageAssignContentTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var landingPageAssignContent  = new LandingPageAssignContent();
            var lPACID = Fixture.Create<int>();
            var lPAID = Fixture.Create<int?>();
            var lPOID = Fixture.Create<int?>();
            var display = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var sortOrder = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            landingPageAssignContent.LPACID = lPACID;
            landingPageAssignContent.LPAID = lPAID;
            landingPageAssignContent.LPOID = lPOID;
            landingPageAssignContent.Display = display;
            landingPageAssignContent.CreatedUserID = createdUserID;
            landingPageAssignContent.CreatedDate = createdDate;
            landingPageAssignContent.UpdatedUserID = updatedUserID;
            landingPageAssignContent.UpdatedDate = updatedDate;
            landingPageAssignContent.SortOrder = sortOrder;
            landingPageAssignContent.IsDeleted = isDeleted;

            // Assert
            landingPageAssignContent.LPACID.ShouldBe(lPACID);
            landingPageAssignContent.LPAID.ShouldBe(lPAID);
            landingPageAssignContent.LPOID.ShouldBe(lPOID);
            landingPageAssignContent.Display.ShouldBe(display);
            landingPageAssignContent.CreatedUserID.ShouldBe(createdUserID);
            landingPageAssignContent.CreatedDate.ShouldBe(createdDate);
            landingPageAssignContent.UpdatedUserID.ShouldBe(updatedUserID);
            landingPageAssignContent.UpdatedDate.ShouldBe(updatedDate);
            landingPageAssignContent.SortOrder.ShouldBe(sortOrder);
            landingPageAssignContent.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : LandingPageAssignContent => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<bool>();

            // Act , Set
            landingPageAssignContent.IsDeleted = random;

            // Assert
            landingPageAssignContent.IsDeleted.ShouldBe(random);
            landingPageAssignContent.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.IsDeleted = null;

            // Assert
            landingPageAssignContent.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.IsDeleted.ShouldBeNull();
            landingPageAssignContent.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : LandingPageAssignContent => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(landingPageAssignContent, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : LandingPageAssignContent => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(landingPageAssignContent, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : LandingPageAssignContent => LPACID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPACID_Int_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var intType = landingPageAssignContent.LPACID.GetType();

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
        public void LandingPageAssignContent_Class_Invalid_Property_LPACID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPACID = "LPACID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constLPACID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPACID_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constLPACID = "LPACID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constLPACID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssignContent => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssignContent.CreatedUserID = random;

            // Assert
            landingPageAssignContent.CreatedUserID.ShouldBe(random);
            landingPageAssignContent.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.CreatedUserID = null;

            // Assert
            landingPageAssignContent.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.CreatedUserID.ShouldBeNull();
            landingPageAssignContent.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssignContent => LPAID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssignContent.LPAID = random;

            // Assert
            landingPageAssignContent.LPAID.ShouldBe(random);
            landingPageAssignContent.LPAID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.LPAID = null;

            // Assert
            landingPageAssignContent.LPAID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constLPAID = "LPAID";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constLPAID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.LPAID.ShouldBeNull();
            landingPageAssignContent.LPAID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_LPAID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPAID = "LPAID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constLPAID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPAID_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constLPAID = "LPAID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constLPAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssignContent => LPOID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssignContent.LPOID = random;

            // Assert
            landingPageAssignContent.LPOID.ShouldBe(random);
            landingPageAssignContent.LPOID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.LPOID = null;

            // Assert
            landingPageAssignContent.LPOID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constLPOID = "LPOID";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constLPOID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.LPOID.ShouldBeNull();
            landingPageAssignContent.LPOID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_LPOID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLPOID = "LPOID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constLPOID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LPOID_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constLPOID = "LPOID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constLPOID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssignContent => SortOrder

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssignContent.SortOrder = random;

            // Assert
            landingPageAssignContent.SortOrder.ShouldBe(random);
            landingPageAssignContent.SortOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.SortOrder = null;

            // Assert
            landingPageAssignContent.SortOrder.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constSortOrder = "SortOrder";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constSortOrder);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.SortOrder.ShouldBeNull();
            landingPageAssignContent.SortOrder.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_SortOrder_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSortOrder = "SortOrder";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constSortOrder));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortOrder_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constSortOrder = "SortOrder";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : LandingPageAssignContent => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var random = Fixture.Create<int>();

            // Act , Set
            landingPageAssignContent.UpdatedUserID = random;

            // Assert
            landingPageAssignContent.UpdatedUserID.ShouldBe(random);
            landingPageAssignContent.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();    

            // Act , Set
            landingPageAssignContent.UpdatedUserID = null;

            // Assert
            landingPageAssignContent.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo = landingPageAssignContent.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(landingPageAssignContent, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            landingPageAssignContent.UpdatedUserID.ShouldBeNull();
            landingPageAssignContent.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LandingPageAssignContent_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : LandingPageAssignContent => Display

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Display_String_Type_Verify_Test()
        {
            // Arrange
            var landingPageAssignContent = Fixture.Create<LandingPageAssignContent>();
            var stringType = landingPageAssignContent.Display.GetType();

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
        public void LandingPageAssignContent_Class_Invalid_Property_Display_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDisplay = "Display";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();

            // Act , Assert
            Should.NotThrow(() => landingPageAssignContent.GetType().GetProperty(constDisplay));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Display_Is_Present_In_LandingPageAssignContent_Class_As_Public_Test()
        {
            // Arrange
            const string constDisplay = "Display";
            var landingPageAssignContent  = Fixture.Create<LandingPageAssignContent>();
            var propertyInfo  = landingPageAssignContent.GetType().GetProperty(constDisplay);

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
            Should.NotThrow(() => new LandingPageAssignContent());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<LandingPageAssignContent>(2).ToList();
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
            var lPACID = -1;
            int? lPAID = null;
            int? lPOID = null;
            var display = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var landingPageAssignContent = new LandingPageAssignContent();    

            // Assert
            landingPageAssignContent.LPACID.ShouldBe(lPACID);
            landingPageAssignContent.LPAID.ShouldBeNull();
            landingPageAssignContent.LPOID.ShouldBeNull();
            landingPageAssignContent.Display.ShouldBe(display);
            landingPageAssignContent.CreatedUserID.ShouldBeNull();
            landingPageAssignContent.CreatedDate.ShouldBeNull();
            landingPageAssignContent.UpdatedUserID.ShouldBeNull();
            landingPageAssignContent.UpdatedDate.ShouldBeNull();
            landingPageAssignContent.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}