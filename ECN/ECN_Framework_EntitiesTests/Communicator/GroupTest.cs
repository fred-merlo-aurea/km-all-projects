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
    public class GroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Group) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var groupId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var folderId = Fixture.Create<int?>();
            var groupName = Fixture.Create<string>();
            var groupDescription = Fixture.Create<string>();
            var ownerTypeCode = Fixture.Create<string>();
            var masterSupression = Fixture.Create<int?>();
            var publicFolder = Fixture.Create<int?>();
            var optinHTML = Fixture.Create<string>();
            var optinFields = Fixture.Create<string>();
            var allowUDFHistory = Fixture.Create<string>();
            var isSeedList = Fixture.Create<bool?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var archived = Fixture.Create<bool?>();
            var folderName = Fixture.Create<string>();

            // Act
            group.GroupID = groupId;
            group.CustomerID = customerId;
            group.FolderID = folderId;
            group.GroupName = groupName;
            group.GroupDescription = groupDescription;
            group.OwnerTypeCode = ownerTypeCode;
            group.MasterSupression = masterSupression;
            group.PublicFolder = publicFolder;
            group.OptinHTML = optinHTML;
            group.OptinFields = optinFields;
            group.AllowUDFHistory = allowUDFHistory;
            group.IsSeedList = isSeedList;
            group.CreatedUserID = createdUserId;
            group.CreatedDate = createdDate;
            group.UpdatedUserID = updatedUserId;
            group.Archived = archived;
            group.FolderName = folderName;

            // Assert
            group.GroupID.ShouldBe(groupId);
            group.CustomerID.ShouldBe(customerId);
            group.FolderID.ShouldBe(folderId);
            group.GroupName.ShouldBe(groupName);
            group.GroupDescription.ShouldBe(groupDescription);
            group.OwnerTypeCode.ShouldBe(ownerTypeCode);
            group.MasterSupression.ShouldBe(masterSupression);
            group.PublicFolder.ShouldBe(publicFolder);
            group.OptinHTML.ShouldBe(optinHTML);
            group.OptinFields.ShouldBe(optinFields);
            group.AllowUDFHistory.ShouldBe(allowUDFHistory);
            group.IsSeedList.ShouldBe(isSeedList);
            group.CreatedUserID.ShouldBe(createdUserId);
            group.CreatedDate.ShouldBe(createdDate);
            group.UpdatedUserID.ShouldBe(updatedUserId);
            group.UpdatedDate.ShouldBeNull();
            group.Archived.ShouldBe(archived);
            group.FolderName.ShouldBe(folderName);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (AllowUDFHistory) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_AllowUDFHistory_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.AllowUDFHistory = Fixture.Create<string>();
            var stringType = group.AllowUDFHistory.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (AllowUDFHistory) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_AllowUDFHistoryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAllowUDFHistory = "AllowUDFHistoryNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameAllowUDFHistory));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_AllowUDFHistory_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAllowUDFHistory = "AllowUDFHistory";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameAllowUDFHistory);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (Archived) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Archived_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<bool>();

            // Act , Set
            group.Archived = random;

            // Assert
            group.Archived.ShouldBe(random);
            group.Archived.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Archived_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.Archived = null;

            // Assert
            group.Archived.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Archived_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameArchived = "Archived";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameArchived);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.Archived.ShouldBeNull();
            group.Archived.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (Archived) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_ArchivedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameArchived = "ArchivedNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameArchived));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Archived_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameArchived = "Archived";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameArchived);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var group = Fixture.Create<Group>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = group.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(group, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<int>();

            // Act , Set
            group.CreatedUserID = random;

            // Assert
            group.CreatedUserID.ShouldBe(random);
            group.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.CreatedUserID = null;

            // Assert
            group.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.CreatedUserID.ShouldBeNull();
            group.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.CustomerID = Fixture.Create<int>();
            var intType = group.CustomerID.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (FolderID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<int>();

            // Act , Set
            group.FolderID = random;

            // Assert
            group.FolderID.ShouldBe(random);
            group.FolderID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.FolderID = null;

            // Assert
            group.FolderID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFolderID = "FolderID";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameFolderID);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.FolderID.ShouldBeNull();
            group.FolderID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (FolderName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.FolderName = Fixture.Create<string>();
            var stringType = group.FolderName.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (FolderName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_FolderNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderNameNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameFolderName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_FolderName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderName = "FolderName";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameFolderName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (GroupDescription) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupDescription_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.GroupDescription = Fixture.Create<string>();
            var stringType = group.GroupDescription.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (GroupDescription) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_GroupDescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupDescription = "GroupDescriptionNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameGroupDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupDescription_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupDescription = "GroupDescription";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameGroupDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.GroupID = Fixture.Create<int>();
            var intType = group.GroupID.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.GroupName = Fixture.Create<string>();
            var stringType = group.GroupName.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (IsSeedList) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_IsSeedList_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<bool>();

            // Act , Set
            group.IsSeedList = random;

            // Assert
            group.IsSeedList.ShouldBe(random);
            group.IsSeedList.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_IsSeedList_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.IsSeedList = null;

            // Assert
            group.IsSeedList.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_IsSeedList_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsSeedList = "IsSeedList";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameIsSeedList);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.IsSeedList.ShouldBeNull();
            group.IsSeedList.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (IsSeedList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_IsSeedListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsSeedList = "IsSeedListNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameIsSeedList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_IsSeedList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsSeedList = "IsSeedList";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameIsSeedList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (MasterSupression) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_MasterSupression_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<int>();

            // Act , Set
            group.MasterSupression = random;

            // Assert
            group.MasterSupression.ShouldBe(random);
            group.MasterSupression.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_MasterSupression_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.MasterSupression = null;

            // Assert
            group.MasterSupression.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_MasterSupression_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMasterSupression = "MasterSupression";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameMasterSupression);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.MasterSupression.ShouldBeNull();
            group.MasterSupression.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (MasterSupression) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_MasterSupressionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMasterSupression = "MasterSupressionNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameMasterSupression));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_MasterSupression_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMasterSupression = "MasterSupression";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameMasterSupression);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (OptinFields) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OptinFields_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.OptinFields = Fixture.Create<string>();
            var stringType = group.OptinFields.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (OptinFields) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_OptinFieldsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptinFields = "OptinFieldsNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameOptinFields));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OptinFields_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptinFields = "OptinFields";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameOptinFields);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (OptinHTML) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OptinHTML_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.OptinHTML = Fixture.Create<string>();
            var stringType = group.OptinHTML.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (OptinHTML) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_OptinHTMLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOptinHTML = "OptinHTMLNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameOptinHTML));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OptinHTML_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOptinHTML = "OptinHTML";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameOptinHTML);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (OwnerTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OwnerTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            group.OwnerTypeCode = Fixture.Create<string>();
            var stringType = group.OwnerTypeCode.GetType();

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

        #region General Getters/Setters : Class (Group) => Property (OwnerTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_OwnerTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOwnerTypeCode = "OwnerTypeCodeNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameOwnerTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_OwnerTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOwnerTypeCode = "OwnerTypeCode";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameOwnerTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (PublicFolder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_PublicFolder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<int>();

            // Act , Set
            group.PublicFolder = random;

            // Assert
            group.PublicFolder.ShouldBe(random);
            group.PublicFolder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_PublicFolder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.PublicFolder = null;

            // Assert
            group.PublicFolder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_PublicFolder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePublicFolder = "PublicFolder";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNamePublicFolder);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.PublicFolder.ShouldBeNull();
            group.PublicFolder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (PublicFolder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_PublicFolderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublicFolder = "PublicFolderNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNamePublicFolder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_PublicFolder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublicFolder = "PublicFolder";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNamePublicFolder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var group = Fixture.Create<Group>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = group.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(group, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();
            var random = Fixture.Create<int>();

            // Act , Set
            group.UpdatedUserID = random;

            // Assert
            group.UpdatedUserID.ShouldBe(random);
            group.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var group = Fixture.Create<Group>();

            // Act , Set
            group.UpdatedUserID = null;

            // Assert
            group.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var group = Fixture.Create<Group>();
            var propertyInfo = group.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(group, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            group.UpdatedUserID.ShouldBeNull();
            group.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Group) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var group  = Fixture.Create<Group>();

            // Act , Assert
            Should.NotThrow(() => group.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Group_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var group  = Fixture.Create<Group>();
            var propertyInfo  = group.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Group) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Group_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Group());
        }

        #endregion

        #region General Constructor : Class (Group) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Group_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGroup = Fixture.CreateMany<Group>(2).ToList();
            var firstGroup = instancesOfGroup.FirstOrDefault();
            var lastGroup = instancesOfGroup.Last();

            // Act, Assert
            firstGroup.ShouldNotBeNull();
            lastGroup.ShouldNotBeNull();
            firstGroup.ShouldNotBeSameAs(lastGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Group_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGroup = new Group();
            var secondGroup = new Group();
            var thirdGroup = new Group();
            var fourthGroup = new Group();
            var fifthGroup = new Group();
            var sixthGroup = new Group();

            // Act, Assert
            firstGroup.ShouldNotBeNull();
            secondGroup.ShouldNotBeNull();
            thirdGroup.ShouldNotBeNull();
            fourthGroup.ShouldNotBeNull();
            fifthGroup.ShouldNotBeNull();
            sixthGroup.ShouldNotBeNull();
            firstGroup.ShouldNotBeSameAs(secondGroup);
            thirdGroup.ShouldNotBeSameAs(firstGroup);
            fourthGroup.ShouldNotBeSameAs(firstGroup);
            fifthGroup.ShouldNotBeSameAs(firstGroup);
            sixthGroup.ShouldNotBeSameAs(firstGroup);
            sixthGroup.ShouldNotBeSameAs(fourthGroup);
        }

        #endregion

        #region General Constructor : Class (Group) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Group_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var groupId = -1;
            var customerId = -1;
            var groupName = string.Empty;
            var groupDescription = string.Empty;
            var ownerTypeCode = string.Empty;
            var optinHTML = string.Empty;
            var optinFields = string.Empty;
            var allowUDFHistory = string.Empty;
            var archived = false;
            var folderName = "";

            // Act
            var group = new Group();

            // Assert
            group.GroupID.ShouldBe(groupId);
            group.CustomerID.ShouldBe(customerId);
            group.FolderID.ShouldBeNull();
            group.GroupName.ShouldBe(groupName);
            group.GroupDescription.ShouldBe(groupDescription);
            group.OwnerTypeCode.ShouldBe(ownerTypeCode);
            group.MasterSupression.ShouldBeNull();
            group.PublicFolder.ShouldBeNull();
            group.OptinHTML.ShouldBe(optinHTML);
            group.OptinFields.ShouldBe(optinFields);
            group.AllowUDFHistory.ShouldBe(allowUDFHistory);
            group.IsSeedList.ShouldBeNull();
            group.CreatedUserID.ShouldBeNull();
            group.CreatedDate.ShouldBeNull();
            group.UpdatedUserID.ShouldBeNull();
            group.UpdatedDate.ShouldBeNull();
            group.Archived.ShouldBe(archived);
            group.FolderName.ShouldBe(folderName);
        }

        #endregion

        #endregion

        #endregion
    }
}