using System;
using System.Collections.Generic;
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
    public class ContentTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Content) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var contentId = Fixture.Create<int>();
            var folderId = Fixture.Create<int?>();
            var lockedFlag = Fixture.Create<string>();
            var contentSource = Fixture.Create<string>();
            var contentText = Fixture.Create<string>();
            var contentTypeCode = Fixture.Create<string>();
            var contentCode = Fixture.Create<string>();
            var contentTitle = Fixture.Create<string>();
            var customerId = Fixture.Create<int?>();
            var contentURL = Fixture.Create<string>();
            var contentFilePointer = Fixture.Create<string>();
            var sharing = Fixture.Create<string>();
            var masterContentId = Fixture.Create<int?>();
            var contentMobile = Fixture.Create<string>();
            var contentSMS = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var useWYSIWYGeditor = Fixture.Create<bool?>();
            var archived = Fixture.Create<bool?>();
            var isValidated = Fixture.Create<bool?>();
            var filterList = Fixture.Create<List<ContentFilter>>();
            var aliasList = Fixture.Create<List<LinkAlias>>();
            var dynamicTagList = Fixture.Create<List<DynamicTag>>();

            // Act
            content.ContentID = contentId;
            content.FolderID = folderId;
            content.LockedFlag = lockedFlag;
            content.ContentSource = contentSource;
            content.ContentText = contentText;
            content.ContentTypeCode = contentTypeCode;
            content.ContentCode = contentCode;
            content.ContentTitle = contentTitle;
            content.CustomerID = customerId;
            content.ContentURL = contentURL;
            content.ContentFilePointer = contentFilePointer;
            content.Sharing = sharing;
            content.MasterContentID = masterContentId;
            content.ContentMobile = contentMobile;
            content.ContentSMS = contentSMS;
            content.CreatedUserID = createdUserId;
            content.CreatedDate = createdDate;
            content.UpdatedUserID = updatedUserId;
            content.UpdatedDate = updatedDate;
            content.IsDeleted = isDeleted;
            content.UseWYSIWYGeditor = useWYSIWYGeditor;
            content.Archived = archived;
            content.IsValidated = isValidated;
            content.FilterList = filterList;
            content.AliasList = aliasList;
            content.DynamicTagList = dynamicTagList;

            // Assert
            content.ContentID.ShouldBe(contentId);
            content.FolderID.ShouldBe(folderId);
            content.LockedFlag.ShouldBe(lockedFlag);
            content.ContentSource.ShouldBe(contentSource);
            content.ContentText.ShouldBe(contentText);
            content.ContentTypeCode.ShouldBe(contentTypeCode);
            content.ContentCode.ShouldBe(contentCode);
            content.ContentTitle.ShouldBe(contentTitle);
            content.CustomerID.ShouldBe(customerId);
            content.ContentURL.ShouldBe(contentURL);
            content.ContentFilePointer.ShouldBe(contentFilePointer);
            content.Sharing.ShouldBe(sharing);
            content.MasterContentID.ShouldBe(masterContentId);
            content.ContentMobile.ShouldBe(contentMobile);
            content.ContentSMS.ShouldBe(contentSMS);
            content.CreatedUserID.ShouldBe(createdUserId);
            content.CreatedDate.ShouldBe(createdDate);
            content.UpdatedUserID.ShouldBe(updatedUserId);
            content.UpdatedDate.ShouldBe(updatedDate);
            content.IsDeleted.ShouldBe(isDeleted);
            content.UseWYSIWYGeditor.ShouldBe(useWYSIWYGeditor);
            content.Archived.ShouldBe(archived);
            content.IsValidated.ShouldBe(isValidated);
            content.FilterList.ShouldBe(filterList);
            content.AliasList.ShouldBe(aliasList);
            content.DynamicTagList.ShouldBe(dynamicTagList);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (AliasList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_AliasListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAliasList = "AliasListNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameAliasList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_AliasList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAliasList = "AliasList";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameAliasList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (Archived) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Archived_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<bool>();

            // Act , Set
            content.Archived = random;

            // Assert
            content.Archived.ShouldBe(random);
            content.Archived.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Archived_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.Archived = null;

            // Assert
            content.Archived.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Archived_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameArchived = "Archived";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameArchived);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.Archived.ShouldBeNull();
            content.Archived.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (Archived) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ArchivedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameArchived = "ArchivedNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameArchived));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Archived_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameArchived = "Archived";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameArchived);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentCode = Fixture.Create<string>();
            var stringType = content.ContentCode.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentCode = "ContentCodeNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentCode = "ContentCode";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentFilePointer) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentFilePointer_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentFilePointer = Fixture.Create<string>();
            var stringType = content.ContentFilePointer.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentFilePointer) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentFilePointerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentFilePointer = "ContentFilePointerNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentFilePointer));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentFilePointer_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentFilePointer = "ContentFilePointer";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentFilePointer);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentID = Fixture.Create<int>();
            var intType = content.ContentID.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentMobile) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentMobile_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentMobile = Fixture.Create<string>();
            var stringType = content.ContentMobile.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentMobile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentMobileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentMobile = "ContentMobileNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentMobile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentMobile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentMobile = "ContentMobile";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentMobile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentSMS) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentSMS_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentSMS = Fixture.Create<string>();
            var stringType = content.ContentSMS.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentSMS) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentSMSNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSMS = "ContentSMSNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentSMS));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentSMS_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSMS = "ContentSMS";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentSMS);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentSource) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentSource_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentSource = Fixture.Create<string>();
            var stringType = content.ContentSource.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentSource) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentSourceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentSource = "ContentSourceNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentSource));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentSource_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentSource = "ContentSource";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentText = Fixture.Create<string>();
            var stringType = content.ContentText.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentText = "ContentTextNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentText = "ContentText";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentTitle = Fixture.Create<string>();
            var stringType = content.ContentTitle.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitleNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitle";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentTypeCode = Fixture.Create<string>();
            var stringType = content.ContentTypeCode.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentTypeCode = "ContentTypeCodeNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentTypeCode = "ContentTypeCode";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (ContentURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.ContentURL = Fixture.Create<string>();
            var stringType = content.ContentURL.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (ContentURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_ContentURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentURL = "ContentURLNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameContentURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_ContentURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentURL = "ContentURL";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameContentURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = content.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(content, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<int>();

            // Act , Set
            content.CreatedUserID = random;

            // Assert
            content.CreatedUserID.ShouldBe(random);
            content.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.CreatedUserID = null;

            // Assert
            content.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.CreatedUserID.ShouldBeNull();
            content.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<int>();

            // Act , Set
            content.CustomerID = random;

            // Assert
            content.CustomerID.ShouldBe(random);
            content.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.CustomerID = null;

            // Assert
            content.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.CustomerID.ShouldBeNull();
            content.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (DynamicTagList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_DynamicTagListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicTagList = "DynamicTagListNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameDynamicTagList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_DynamicTagList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicTagList = "DynamicTagList";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameDynamicTagList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (FilterList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_FilterListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterList = "FilterListNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameFilterList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_FilterList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterList = "FilterList";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameFilterList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (FolderID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_FolderID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<int>();

            // Act , Set
            content.FolderID = random;

            // Assert
            content.FolderID.ShouldBe(random);
            content.FolderID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_FolderID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.FolderID = null;

            // Assert
            content.FolderID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_FolderID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFolderID = "FolderID";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameFolderID);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.FolderID.ShouldBeNull();
            content.FolderID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (FolderID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_FolderIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameFolderID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_FolderID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFolderID = "FolderID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameFolderID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<bool>();

            // Act , Set
            content.IsDeleted = random;

            // Assert
            content.IsDeleted.ShouldBe(random);
            content.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.IsDeleted = null;

            // Assert
            content.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.IsDeleted.ShouldBeNull();
            content.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (IsValidated) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsValidated_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<bool>();

            // Act , Set
            content.IsValidated = random;

            // Assert
            content.IsValidated.ShouldBe(random);
            content.IsValidated.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsValidated_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.IsValidated = null;

            // Assert
            content.IsValidated.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsValidated_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsValidated = "IsValidated";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameIsValidated);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.IsValidated.ShouldBeNull();
            content.IsValidated.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (IsValidated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_IsValidatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsValidated = "IsValidatedNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameIsValidated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_IsValidated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsValidated = "IsValidated";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameIsValidated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (LockedFlag) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_LockedFlag_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.LockedFlag = Fixture.Create<string>();
            var stringType = content.LockedFlag.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (LockedFlag) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_LockedFlagNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLockedFlag = "LockedFlagNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameLockedFlag));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_LockedFlag_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLockedFlag = "LockedFlag";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameLockedFlag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (MasterContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_MasterContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<int>();

            // Act , Set
            content.MasterContentID = random;

            // Assert
            content.MasterContentID.ShouldBe(random);
            content.MasterContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_MasterContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.MasterContentID = null;

            // Assert
            content.MasterContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_MasterContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMasterContentID = "MasterContentID";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameMasterContentID);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.MasterContentID.ShouldBeNull();
            content.MasterContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (MasterContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_MasterContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMasterContentID = "MasterContentIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameMasterContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_MasterContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMasterContentID = "MasterContentID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameMasterContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (Sharing) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Sharing_Property_String_Type_Verify_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            content.Sharing = Fixture.Create<string>();
            var stringType = content.Sharing.GetType();

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

        #region General Getters/Setters : Class (Content) => Property (Sharing) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_SharingNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSharing = "SharingNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameSharing));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Sharing_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSharing = "Sharing";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameSharing);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = content.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(content, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<int>();

            // Act , Set
            content.UpdatedUserID = random;

            // Assert
            content.UpdatedUserID.ShouldBe(random);
            content.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.UpdatedUserID = null;

            // Assert
            content.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.UpdatedUserID.ShouldBeNull();
            content.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UseWYSIWYGeditor) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UseWYSIWYGeditor_Property_Data_Without_Null_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var random = Fixture.Create<bool>();

            // Act , Set
            content.UseWYSIWYGeditor = random;

            // Assert
            content.UseWYSIWYGeditor.ShouldBe(random);
            content.UseWYSIWYGeditor.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UseWYSIWYGeditor_Property_Only_Null_Data_Test()
        {
            // Arrange
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();    

            // Act , Set
            content.UseWYSIWYGeditor = null;

            // Assert
            content.UseWYSIWYGeditor.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UseWYSIWYGeditor_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUseWYSIWYGeditor = "UseWYSIWYGeditor";
            var content = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo = content.GetType().GetProperty(propertyNameUseWYSIWYGeditor);

            // Act , Set
            propertyInfo.SetValue(content, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            content.UseWYSIWYGeditor.ShouldBeNull();
            content.UseWYSIWYGeditor.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Content) => Property (UseWYSIWYGeditor) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_Class_Invalid_Property_UseWYSIWYGeditorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUseWYSIWYGeditor = "UseWYSIWYGeditorNotPresent";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();

            // Act , Assert
            Should.NotThrow(() => content.GetType().GetProperty(propertyNameUseWYSIWYGeditor));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Content_UseWYSIWYGeditor_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUseWYSIWYGeditor = "UseWYSIWYGeditor";
            var content  = Fixture.Create<ECN_Framework_Entities.Communicator.Content>();
            var propertyInfo  = content.GetType().GetProperty(propertyNameUseWYSIWYGeditor);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Content) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Content_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ECN_Framework_Entities.Communicator.Content());
        }

        #endregion

        #region General Constructor : Class (Content) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Content_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfContent = Fixture.CreateMany<ECN_Framework_Entities.Communicator.Content>(2).ToList();
            var firstContent = instancesOfContent.FirstOrDefault();
            var lastContent = instancesOfContent.Last();

            // Act, Assert
            firstContent.ShouldNotBeNull();
            lastContent.ShouldNotBeNull();
            firstContent.ShouldNotBeSameAs(lastContent);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Content_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstContent = new ECN_Framework_Entities.Communicator.Content();
            var secondContent = new ECN_Framework_Entities.Communicator.Content();
            var thirdContent = new ECN_Framework_Entities.Communicator.Content();
            var fourthContent = new ECN_Framework_Entities.Communicator.Content();
            var fifthContent = new ECN_Framework_Entities.Communicator.Content();
            var sixthContent = new ECN_Framework_Entities.Communicator.Content();

            // Act, Assert
            firstContent.ShouldNotBeNull();
            secondContent.ShouldNotBeNull();
            thirdContent.ShouldNotBeNull();
            fourthContent.ShouldNotBeNull();
            fifthContent.ShouldNotBeNull();
            sixthContent.ShouldNotBeNull();
            firstContent.ShouldNotBeSameAs(secondContent);
            thirdContent.ShouldNotBeSameAs(firstContent);
            fourthContent.ShouldNotBeSameAs(firstContent);
            fifthContent.ShouldNotBeSameAs(firstContent);
            sixthContent.ShouldNotBeSameAs(firstContent);
            sixthContent.ShouldNotBeSameAs(fourthContent);
        }

        #endregion

        #region General Constructor : Class (Content) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Content_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var contentId = -1;
            var lockedFlag = string.Empty;
            var contentSource = string.Empty;
            var contentText = string.Empty;
            var contentTypeCode = string.Empty;
            var contentCode = string.Empty;
            var contentTitle = string.Empty;
            var contentURL = string.Empty;
            var contentFilePointer = string.Empty;
            var sharing = string.Empty;
            var contentMobile = string.Empty;
            var contentSMS = string.Empty;
            var filterList = new List<ContentFilter>();
            var aliasList = new List<LinkAlias>();
            var dynamicTagList = new List<DynamicTag>();
            var archived = false;

            // Act
            var content = new ECN_Framework_Entities.Communicator.Content();

            // Assert
            content.ContentID.ShouldBe(contentId);
            content.FolderID.ShouldBeNull();
            content.LockedFlag.ShouldBe(lockedFlag);
            content.ContentSource.ShouldBe(contentSource);
            content.ContentText.ShouldBe(contentText);
            content.ContentTypeCode.ShouldBe(contentTypeCode);
            content.ContentCode.ShouldBe(contentCode);
            content.ContentTitle.ShouldBe(contentTitle);
            content.CustomerID.ShouldBeNull();
            content.ContentURL.ShouldBe(contentURL);
            content.ContentFilePointer.ShouldBe(contentFilePointer);
            content.Sharing.ShouldBe(sharing);
            content.MasterContentID.ShouldBeNull();
            content.ContentMobile.ShouldBe(contentMobile);
            content.ContentSMS.ShouldBe(contentSMS);
            content.CreatedUserID.ShouldBeNull();
            content.CreatedDate.ShouldBeNull();
            content.UpdatedUserID.ShouldBeNull();
            content.UpdatedDate.ShouldBeNull();
            content.IsDeleted.ShouldBeNull();
            content.FilterList.ShouldBeEmpty();
            content.AliasList.ShouldBeEmpty();
            content.UseWYSIWYGeditor.ShouldBeNull();
            content.DynamicTagList.ShouldBeEmpty();
            content.Archived.ShouldBe(archived);
            content.IsValidated.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}