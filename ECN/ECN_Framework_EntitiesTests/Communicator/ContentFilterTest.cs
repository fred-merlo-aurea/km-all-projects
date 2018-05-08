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
    public class ContentFilterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ContentFilter) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var filterId = Fixture.Create<int>();
            var layoutId = Fixture.Create<int?>();
            var slotNumber = Fixture.Create<int?>();
            var contentId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var filterName = Fixture.Create<string>();
            var whereClause = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var contentTitle = Fixture.Create<string>();
            var detailList = Fixture.Create<List<ContentFilterDetail>>();

            // Act
            contentFilter.FilterID = filterId;
            contentFilter.LayoutID = layoutId;
            contentFilter.SlotNumber = slotNumber;
            contentFilter.ContentID = contentId;
            contentFilter.GroupID = groupId;
            contentFilter.FilterName = filterName;
            contentFilter.WhereClause = whereClause;
            contentFilter.CreatedUserID = createdUserId;
            contentFilter.UpdatedUserID = updatedUserId;
            contentFilter.IsDeleted = isDeleted;
            contentFilter.CustomerID = customerId;
            contentFilter.ContentTitle = contentTitle;
            contentFilter.DetailList = detailList;

            // Assert
            contentFilter.FilterID.ShouldBe(filterId);
            contentFilter.LayoutID.ShouldBe(layoutId);
            contentFilter.SlotNumber.ShouldBe(slotNumber);
            contentFilter.ContentID.ShouldBe(contentId);
            contentFilter.GroupID.ShouldBe(groupId);
            contentFilter.FilterName.ShouldBe(filterName);
            contentFilter.WhereClause.ShouldBe(whereClause);
            contentFilter.CreatedUserID.ShouldBe(createdUserId);
            contentFilter.CreatedDate.ShouldBeNull();
            contentFilter.UpdatedUserID.ShouldBe(updatedUserId);
            contentFilter.UpdatedDate.ShouldBeNull();
            contentFilter.IsDeleted.ShouldBe(isDeleted);
            contentFilter.CustomerID.ShouldBe(customerId);
            contentFilter.ContentTitle.ShouldBe(contentTitle);
            contentFilter.DetailList.ShouldBe(detailList);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (ContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.ContentID = random;

            // Assert
            contentFilter.ContentID.ShouldBe(random);
            contentFilter.ContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.ContentID = null;

            // Assert
            contentFilter.ContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentID = "ContentID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameContentID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.ContentID.ShouldBeNull();
            contentFilter.ContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (ContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_ContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (ContentTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            contentFilter.ContentTitle = Fixture.Create<string>();
            var stringType = contentFilter.ContentTitle.GetType();

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

        #region General Getters/Setters : Class (ContentFilter) => Property (ContentTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_ContentTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitleNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameContentTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_ContentTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentTitle = "ContentTitle";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameContentTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var contentFilter = Fixture.Create<ContentFilter>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contentFilter, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.CreatedUserID = random;

            // Assert
            contentFilter.CreatedUserID.ShouldBe(random);
            contentFilter.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.CreatedUserID = null;

            // Assert
            contentFilter.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.CreatedUserID.ShouldBeNull();
            contentFilter.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.CustomerID = random;

            // Assert
            contentFilter.CustomerID.ShouldBe(random);
            contentFilter.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.CustomerID = null;

            // Assert
            contentFilter.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.CustomerID.ShouldBeNull();
            contentFilter.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (DetailList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_DetailListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDetailList = "DetailListNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameDetailList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_DetailList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDetailList = "DetailList";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameDetailList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (FilterID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_FilterID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            contentFilter.FilterID = Fixture.Create<int>();
            var intType = contentFilter.FilterID.GetType();

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

        #region General Getters/Setters : Class (ContentFilter) => Property (FilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_FilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_FilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            contentFilter.FilterName = Fixture.Create<string>();
            var stringType = contentFilter.FilterName.GetType();

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

        #region General Getters/Setters : Class (ContentFilter) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.GroupID = random;

            // Assert
            contentFilter.GroupID.ShouldBe(random);
            contentFilter.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.GroupID = null;

            // Assert
            contentFilter.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.GroupID.ShouldBeNull();
            contentFilter.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<bool>();

            // Act , Set
            contentFilter.IsDeleted = random;

            // Assert
            contentFilter.IsDeleted.ShouldBe(random);
            contentFilter.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.IsDeleted = null;

            // Assert
            contentFilter.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.IsDeleted.ShouldBeNull();
            contentFilter.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (LayoutID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_LayoutID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.LayoutID = random;

            // Assert
            contentFilter.LayoutID.ShouldBe(random);
            contentFilter.LayoutID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_LayoutID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.LayoutID = null;

            // Assert
            contentFilter.LayoutID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_LayoutID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLayoutID = "LayoutID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameLayoutID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.LayoutID.ShouldBeNull();
            contentFilter.LayoutID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (LayoutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_LayoutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameLayoutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_LayoutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameLayoutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (SlotNumber) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_SlotNumber_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.SlotNumber = random;

            // Assert
            contentFilter.SlotNumber.ShouldBe(random);
            contentFilter.SlotNumber.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_SlotNumber_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.SlotNumber = null;

            // Assert
            contentFilter.SlotNumber.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_SlotNumber_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSlotNumber = "SlotNumber";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameSlotNumber);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.SlotNumber.ShouldBeNull();
            contentFilter.SlotNumber.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (SlotNumber) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_SlotNumberNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSlotNumber = "SlotNumberNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameSlotNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_SlotNumber_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSlotNumber = "SlotNumber";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameSlotNumber);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var contentFilter = Fixture.Create<ContentFilter>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contentFilter, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilter.UpdatedUserID = random;

            // Assert
            contentFilter.UpdatedUserID.ShouldBe(random);
            contentFilter.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();    

            // Act , Set
            contentFilter.UpdatedUserID = null;

            // Assert
            contentFilter.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var contentFilter = Fixture.Create<ContentFilter>();
            var propertyInfo = contentFilter.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(contentFilter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilter.UpdatedUserID.ShouldBeNull();
            contentFilter.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilter) => Property (WhereClause) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_WhereClause_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilter = Fixture.Create<ContentFilter>();
            contentFilter.WhereClause = Fixture.Create<string>();
            var stringType = contentFilter.WhereClause.GetType();

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

        #region General Getters/Setters : Class (ContentFilter) => Property (WhereClause) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_Class_Invalid_Property_WhereClauseNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClauseNotPresent";
            var contentFilter  = Fixture.Create<ContentFilter>();

            // Act , Assert
            Should.NotThrow(() => contentFilter.GetType().GetProperty(propertyNameWhereClause));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilter_WhereClause_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClause";
            var contentFilter  = Fixture.Create<ContentFilter>();
            var propertyInfo  = contentFilter.GetType().GetProperty(propertyNameWhereClause);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ContentFilter) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilter_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ContentFilter());
        }

        #endregion

        #region General Constructor : Class (ContentFilter) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilter_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfContentFilter = Fixture.CreateMany<ContentFilter>(2).ToList();
            var firstContentFilter = instancesOfContentFilter.FirstOrDefault();
            var lastContentFilter = instancesOfContentFilter.Last();

            // Act, Assert
            firstContentFilter.ShouldNotBeNull();
            lastContentFilter.ShouldNotBeNull();
            firstContentFilter.ShouldNotBeSameAs(lastContentFilter);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilter_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstContentFilter = new ContentFilter();
            var secondContentFilter = new ContentFilter();
            var thirdContentFilter = new ContentFilter();
            var fourthContentFilter = new ContentFilter();
            var fifthContentFilter = new ContentFilter();
            var sixthContentFilter = new ContentFilter();

            // Act, Assert
            firstContentFilter.ShouldNotBeNull();
            secondContentFilter.ShouldNotBeNull();
            thirdContentFilter.ShouldNotBeNull();
            fourthContentFilter.ShouldNotBeNull();
            fifthContentFilter.ShouldNotBeNull();
            sixthContentFilter.ShouldNotBeNull();
            firstContentFilter.ShouldNotBeSameAs(secondContentFilter);
            thirdContentFilter.ShouldNotBeSameAs(firstContentFilter);
            fourthContentFilter.ShouldNotBeSameAs(firstContentFilter);
            fifthContentFilter.ShouldNotBeSameAs(firstContentFilter);
            sixthContentFilter.ShouldNotBeSameAs(firstContentFilter);
            sixthContentFilter.ShouldNotBeSameAs(fourthContentFilter);
        }

        #endregion

        #region General Constructor : Class (ContentFilter) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilter_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var filterId = -1;
            var filterName = string.Empty;
            var whereClause = string.Empty;
            var contentTitle = string.Empty;

            // Act
            var contentFilter = new ContentFilter();

            // Assert
            contentFilter.FilterID.ShouldBe(filterId);
            contentFilter.LayoutID.ShouldBeNull();
            contentFilter.SlotNumber.ShouldBeNull();
            contentFilter.ContentID.ShouldBeNull();
            contentFilter.GroupID.ShouldBeNull();
            contentFilter.FilterName.ShouldBe(filterName);
            contentFilter.WhereClause.ShouldBe(whereClause);
            contentFilter.CreatedUserID.ShouldBeNull();
            contentFilter.CreatedDate.ShouldBeNull();
            contentFilter.UpdatedUserID.ShouldBeNull();
            contentFilter.UpdatedDate.ShouldBeNull();
            contentFilter.IsDeleted.ShouldBeNull();
            contentFilter.ContentTitle.ShouldBe(contentTitle);
            contentFilter.DetailList.ShouldBeNull();
            contentFilter.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}