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
    public class ContentFilterDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ContentFilterDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var fDId = Fixture.Create<int>();
            var filterId = Fixture.Create<int?>();
            var fieldType = Fixture.Create<string>();
            var compareType = Fixture.Create<string>();
            var fieldName = Fixture.Create<string>();
            var comparator = Fixture.Create<string>();
            var compareValue = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            contentFilterDetail.FDID = fDId;
            contentFilterDetail.FilterID = filterId;
            contentFilterDetail.FieldType = fieldType;
            contentFilterDetail.CompareType = compareType;
            contentFilterDetail.FieldName = fieldName;
            contentFilterDetail.Comparator = comparator;
            contentFilterDetail.CompareValue = compareValue;
            contentFilterDetail.CreatedUserID = createdUserId;
            contentFilterDetail.UpdatedUserID = updatedUserId;
            contentFilterDetail.IsDeleted = isDeleted;
            contentFilterDetail.CustomerID = customerId;

            // Assert
            contentFilterDetail.FDID.ShouldBe(fDId);
            contentFilterDetail.FilterID.ShouldBe(filterId);
            contentFilterDetail.FieldType.ShouldBe(fieldType);
            contentFilterDetail.CompareType.ShouldBe(compareType);
            contentFilterDetail.FieldName.ShouldBe(fieldName);
            contentFilterDetail.Comparator.ShouldBe(comparator);
            contentFilterDetail.CompareValue.ShouldBe(compareValue);
            contentFilterDetail.CreatedUserID.ShouldBe(createdUserId);
            contentFilterDetail.CreatedDate.ShouldBeNull();
            contentFilterDetail.UpdatedUserID.ShouldBe(updatedUserId);
            contentFilterDetail.UpdatedDate.ShouldBeNull();
            contentFilterDetail.IsDeleted.ShouldBe(isDeleted);
            contentFilterDetail.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (Comparator) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Comparator_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.Comparator = Fixture.Create<string>();
            var stringType = contentFilterDetail.Comparator.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (Comparator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_ComparatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComparator = "ComparatorNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameComparator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Comparator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComparator = "Comparator";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameComparator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CompareType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CompareType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.CompareType = Fixture.Create<string>();
            var stringType = contentFilterDetail.CompareType.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CompareType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CompareTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompareType = "CompareTypeNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCompareType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CompareType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompareType = "CompareType";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCompareType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CompareValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CompareValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.CompareValue = Fixture.Create<string>();
            var stringType = contentFilterDetail.CompareValue.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CompareValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CompareValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompareValue = "CompareValueNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCompareValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CompareValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompareValue = "CompareValue";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCompareValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contentFilterDetail, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilterDetail.CreatedUserID = random;

            // Assert
            contentFilterDetail.CreatedUserID.ShouldBe(random);
            contentFilterDetail.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();    

            // Act , Set
            contentFilterDetail.CreatedUserID = null;

            // Assert
            contentFilterDetail.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.CreatedUserID.ShouldBeNull();
            contentFilterDetail.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilterDetail.CustomerID = random;

            // Assert
            contentFilterDetail.CustomerID.ShouldBe(random);
            contentFilterDetail.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();    

            // Act , Set
            contentFilterDetail.CustomerID = null;

            // Assert
            contentFilterDetail.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.CustomerID.ShouldBeNull();
            contentFilterDetail.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FDID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FDID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.FDID = Fixture.Create<int>();
            var intType = contentFilterDetail.FDID.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FDID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_FDIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFDID = "FDIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameFDID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FDID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFDID = "FDID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameFDID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FieldName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FieldName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.FieldName = Fixture.Create<string>();
            var stringType = contentFilterDetail.FieldName.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FieldName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_FieldNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldNameNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameFieldName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FieldName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldName = "FieldName";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameFieldName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FieldType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FieldType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            contentFilterDetail.FieldType = Fixture.Create<string>();
            var stringType = contentFilterDetail.FieldType.GetType();

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

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FieldType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_FieldTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldTypeNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameFieldType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FieldType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldType";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameFieldType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FilterID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilterDetail.FilterID = random;

            // Assert
            contentFilterDetail.FilterID.ShouldBe(random);
            contentFilterDetail.FilterID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();    

            // Act , Set
            contentFilterDetail.FilterID = null;

            // Assert
            contentFilterDetail.FilterID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFilterID = "FilterID";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameFilterID);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.FilterID.ShouldBeNull();
            contentFilterDetail.FilterID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (FilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_FilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_FilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<bool>();

            // Act , Set
            contentFilterDetail.IsDeleted = random;

            // Assert
            contentFilterDetail.IsDeleted.ShouldBe(random);
            contentFilterDetail.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();    

            // Act , Set
            contentFilterDetail.IsDeleted = null;

            // Assert
            contentFilterDetail.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.IsDeleted.ShouldBeNull();
            contentFilterDetail.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contentFilterDetail, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            contentFilterDetail.UpdatedUserID = random;

            // Assert
            contentFilterDetail.UpdatedUserID.ShouldBe(random);
            contentFilterDetail.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();    

            // Act , Set
            contentFilterDetail.UpdatedUserID = null;

            // Assert
            contentFilterDetail.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var contentFilterDetail = Fixture.Create<ContentFilterDetail>();
            var propertyInfo = contentFilterDetail.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(contentFilterDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contentFilterDetail.UpdatedUserID.ShouldBeNull();
            contentFilterDetail.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ContentFilterDetail) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();

            // Act , Assert
            Should.NotThrow(() => contentFilterDetail.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ContentFilterDetail_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var contentFilterDetail  = Fixture.Create<ContentFilterDetail>();
            var propertyInfo  = contentFilterDetail.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ContentFilterDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ContentFilterDetail());
        }

        #endregion

        #region General Constructor : Class (ContentFilterDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfContentFilterDetail = Fixture.CreateMany<ContentFilterDetail>(2).ToList();
            var firstContentFilterDetail = instancesOfContentFilterDetail.FirstOrDefault();
            var lastContentFilterDetail = instancesOfContentFilterDetail.Last();

            // Act, Assert
            firstContentFilterDetail.ShouldNotBeNull();
            lastContentFilterDetail.ShouldNotBeNull();
            firstContentFilterDetail.ShouldNotBeSameAs(lastContentFilterDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstContentFilterDetail = new ContentFilterDetail();
            var secondContentFilterDetail = new ContentFilterDetail();
            var thirdContentFilterDetail = new ContentFilterDetail();
            var fourthContentFilterDetail = new ContentFilterDetail();
            var fifthContentFilterDetail = new ContentFilterDetail();
            var sixthContentFilterDetail = new ContentFilterDetail();

            // Act, Assert
            firstContentFilterDetail.ShouldNotBeNull();
            secondContentFilterDetail.ShouldNotBeNull();
            thirdContentFilterDetail.ShouldNotBeNull();
            fourthContentFilterDetail.ShouldNotBeNull();
            fifthContentFilterDetail.ShouldNotBeNull();
            sixthContentFilterDetail.ShouldNotBeNull();
            firstContentFilterDetail.ShouldNotBeSameAs(secondContentFilterDetail);
            thirdContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            fourthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            fifthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            sixthContentFilterDetail.ShouldNotBeSameAs(firstContentFilterDetail);
            sixthContentFilterDetail.ShouldNotBeSameAs(fourthContentFilterDetail);
        }

        #endregion

        #region General Constructor : Class (ContentFilterDetail) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ContentFilterDetail_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var fDId = -1;
            var fieldType = string.Empty;
            var compareType = string.Empty;
            var fieldName = string.Empty;
            var comparator = string.Empty;
            var compareValue = string.Empty;

            // Act
            var contentFilterDetail = new ContentFilterDetail();

            // Assert
            contentFilterDetail.FDID.ShouldBe(fDId);
            contentFilterDetail.FilterID.ShouldBeNull();
            contentFilterDetail.FieldType.ShouldBe(fieldType);
            contentFilterDetail.CompareType.ShouldBe(compareType);
            contentFilterDetail.FieldName.ShouldBe(fieldName);
            contentFilterDetail.Comparator.ShouldBe(comparator);
            contentFilterDetail.CompareValue.ShouldBe(compareValue);
            contentFilterDetail.CreatedUserID.ShouldBeNull();
            contentFilterDetail.CreatedDate.ShouldBeNull();
            contentFilterDetail.UpdatedUserID.ShouldBeNull();
            contentFilterDetail.UpdatedDate.ShouldBeNull();
            contentFilterDetail.IsDeleted.ShouldBeNull();
            contentFilterDetail.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}