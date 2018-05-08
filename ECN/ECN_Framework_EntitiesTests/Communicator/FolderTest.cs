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
    public class FolderTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Folder) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var folderId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var folderName = Fixture.Create<string>();
            var folderDescription = Fixture.Create<string>();
            var folderType = Fixture.Create<string>();
            var isSystem = Fixture.Create<bool?>();
            var parentId = Fixture.Create<int>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            folder.FolderID = folderId;
            folder.CustomerID = customerId;
            folder.FolderName = folderName;
            folder.FolderDescription = folderDescription;
            folder.FolderType = folderType;
            folder.IsSystem = isSystem;
            folder.ParentID = parentId;
            folder.CreatedUserID = createdUserId;
            folder.CreatedDate = createdDate;
            folder.UpdatedUserID = updatedUserId;
            folder.IsDeleted = isDeleted;

            // Assert
            folder.FolderID.ShouldBe(folderId);
            folder.CustomerID.ShouldBe(customerId);
            folder.FolderName.ShouldBe(folderName);
            folder.FolderDescription.ShouldBe(folderDescription);
            folder.FolderType.ShouldBe(folderType);
            folder.IsSystem.ShouldBe(isSystem);
            folder.ParentID.ShouldBe(parentId);
            folder.CreatedUserID.ShouldBe(createdUserId);
            folder.CreatedDate.ShouldBe(createdDate);
            folder.UpdatedUserID.ShouldBe(updatedUserId);
            folder.UpdatedDate.ShouldBeNull();
            folder.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var folder = Fixture.Create<Folder>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(folder, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var random = Fixture.Create<int>();

            // Act , Set
            folder.CreatedUserID = random;

            // Assert
            folder.CreatedUserID.ShouldBe(random);
            folder.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();

            // Act , Set
            folder.CreatedUserID = null;

            // Assert
            folder.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var folder = Fixture.Create<Folder>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(folder, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            folder.CreatedUserID.ShouldBeNull();
            folder.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var random = Fixture.Create<int>();

            // Act , Set
            folder.CustomerID = random;

            // Assert
            folder.CustomerID.ShouldBe(random);
            folder.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();

            // Act , Set
            folder.CustomerID = null;

            // Assert
            folder.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var folder = Fixture.Create<Folder>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(folder, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            folder.CustomerID.ShouldBeNull();
            folder.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (FolderDescription) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderDescription_Property_String_Type_Verify_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            folder.FolderDescription = Fixture.Create<string>();
            var stringType = folder.FolderDescription.GetType();

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

        #region General Getters/Setters : Class (Folder) => Property (FolderDescription) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_FolderDescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderDescription = "FolderDescriptionNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameFolderDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderDescription_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderDescription = "FolderDescription";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameFolderDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (FolderID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            folder.FolderID = Fixture.Create<int>();
            var intType = folder.FolderID.GetType();

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

        #region General Getters/Setters : Class (Folder) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (FolderName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            folder.FolderName = Fixture.Create<string>();
            var stringType = folder.FolderName.GetType();

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

        #region General Getters/Setters : Class (Folder) => Property (FolderName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_FolderNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderNameNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameFolderName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderName";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameFolderName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (FolderType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            folder.FolderType = Fixture.Create<string>();
            var stringType = folder.FolderType.GetType();

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

        #region General Getters/Setters : Class (Folder) => Property (FolderType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_FolderTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderType = "FolderTypeNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameFolderType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_FolderType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderType = "FolderType";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameFolderType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var random = Fixture.Create<bool>();

            // Act , Set
            folder.IsDeleted = random;

            // Assert
            folder.IsDeleted.ShouldBe(random);
            folder.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();

            // Act , Set
            folder.IsDeleted = null;

            // Assert
            folder.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var folder = Fixture.Create<Folder>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(folder, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            folder.IsDeleted.ShouldBeNull();
            folder.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (IsSystem) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsSystem_Property_Data_Without_Null_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var random = Fixture.Create<bool>();

            // Act , Set
            folder.IsSystem = random;

            // Assert
            folder.IsSystem.ShouldBe(random);
            folder.IsSystem.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsSystem_Property_Only_Null_Data_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();

            // Act , Set
            folder.IsSystem = null;

            // Assert
            folder.IsSystem.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsSystem_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsSystem = "IsSystem";
            var folder = Fixture.Create<Folder>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameIsSystem);

            // Act , Set
            propertyInfo.SetValue(folder, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            folder.IsSystem.ShouldBeNull();
            folder.IsSystem.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (IsSystem) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_IsSystemNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsSystem = "IsSystemNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameIsSystem));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_IsSystem_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsSystem = "IsSystem";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameIsSystem);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (ParentID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_ParentID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            folder.ParentID = Fixture.Create<int>();
            var intType = folder.ParentID.GetType();

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

        #region General Getters/Setters : Class (Folder) => Property (ParentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_ParentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameParentID = "ParentIDNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameParentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_ParentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameParentID = "ParentID";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameParentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var folder = Fixture.Create<Folder>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(folder, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();
            var random = Fixture.Create<int>();

            // Act , Set
            folder.UpdatedUserID = random;

            // Assert
            folder.UpdatedUserID.ShouldBe(random);
            folder.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var folder = Fixture.Create<Folder>();

            // Act , Set
            folder.UpdatedUserID = null;

            // Assert
            folder.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var folder = Fixture.Create<Folder>();
            var propertyInfo = folder.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(folder, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            folder.UpdatedUserID.ShouldBeNull();
            folder.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Folder) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var folder  = Fixture.Create<Folder>();

            // Act , Assert
            Should.NotThrow(() => folder.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Folder_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var folder  = Fixture.Create<Folder>();
            var propertyInfo  = folder.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Folder) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Folder_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Folder());
        }

        #endregion

        #region General Constructor : Class (Folder) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Folder_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfFolder = Fixture.CreateMany<Folder>(2).ToList();
            var firstFolder = instancesOfFolder.FirstOrDefault();
            var lastFolder = instancesOfFolder.Last();

            // Act, Assert
            firstFolder.ShouldNotBeNull();
            lastFolder.ShouldNotBeNull();
            firstFolder.ShouldNotBeSameAs(lastFolder);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Folder_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstFolder = new Folder();
            var secondFolder = new Folder();
            var thirdFolder = new Folder();
            var fourthFolder = new Folder();
            var fifthFolder = new Folder();
            var sixthFolder = new Folder();

            // Act, Assert
            firstFolder.ShouldNotBeNull();
            secondFolder.ShouldNotBeNull();
            thirdFolder.ShouldNotBeNull();
            fourthFolder.ShouldNotBeNull();
            fifthFolder.ShouldNotBeNull();
            sixthFolder.ShouldNotBeNull();
            firstFolder.ShouldNotBeSameAs(secondFolder);
            thirdFolder.ShouldNotBeSameAs(firstFolder);
            fourthFolder.ShouldNotBeSameAs(firstFolder);
            fifthFolder.ShouldNotBeSameAs(firstFolder);
            sixthFolder.ShouldNotBeSameAs(firstFolder);
            sixthFolder.ShouldNotBeSameAs(fourthFolder);
        }

        #endregion

        #region General Constructor : Class (Folder) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Folder_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var folderId = -1;
            var folderName = string.Empty;
            var folderDescription = string.Empty;
            var folderType = string.Empty;
            var parentId = 0;

            // Act
            var folder = new Folder();

            // Assert
            folder.FolderID.ShouldBe(folderId);
            folder.CustomerID.ShouldBeNull();
            folder.FolderName.ShouldBe(folderName);
            folder.FolderDescription.ShouldBe(folderDescription);
            folder.FolderType.ShouldBe(folderType);
            folder.IsSystem.ShouldBeNull();
            folder.ParentID.ShouldBe(parentId);
            folder.CreatedUserID.ShouldBeNull();
            folder.CreatedDate.ShouldBeNull();
            folder.UpdatedUserID.ShouldBeNull();
            folder.UpdatedDate.ShouldBeNull();
            folder.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}