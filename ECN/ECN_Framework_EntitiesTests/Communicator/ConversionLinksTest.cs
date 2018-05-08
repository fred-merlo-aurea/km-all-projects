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
    public class ConversionLinksTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ConversionLinks) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var linkId = Fixture.Create<int>();
            var layoutId = Fixture.Create<int?>();
            var linkURL = Fixture.Create<string>();
            var linkParams = Fixture.Create<string>();
            var linkName = Fixture.Create<string>();
            var isActive = Fixture.Create<string>();
            var sortOrder = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            conversionLinks.LinkID = linkId;
            conversionLinks.LayoutID = layoutId;
            conversionLinks.LinkURL = linkURL;
            conversionLinks.LinkParams = linkParams;
            conversionLinks.LinkName = linkName;
            conversionLinks.IsActive = isActive;
            conversionLinks.SortOrder = sortOrder;
            conversionLinks.CreatedUserID = createdUserId;
            conversionLinks.UpdatedUserID = updatedUserId;
            conversionLinks.IsDeleted = isDeleted;
            conversionLinks.CustomerID = customerId;

            // Assert
            conversionLinks.LinkID.ShouldBe(linkId);
            conversionLinks.LayoutID.ShouldBe(layoutId);
            conversionLinks.LinkURL.ShouldBe(linkURL);
            conversionLinks.LinkParams.ShouldBe(linkParams);
            conversionLinks.LinkName.ShouldBe(linkName);
            conversionLinks.IsActive.ShouldBe(isActive);
            conversionLinks.SortOrder.ShouldBe(sortOrder);
            conversionLinks.CreatedUserID.ShouldBe(createdUserId);
            conversionLinks.CreatedDate.ShouldBeNull();
            conversionLinks.UpdatedUserID.ShouldBe(updatedUserId);
            conversionLinks.UpdatedDate.ShouldBeNull();
            conversionLinks.IsDeleted.ShouldBe(isDeleted);
            conversionLinks.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(conversionLinks, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<int>();

            // Act , Set
            conversionLinks.CreatedUserID = random;

            // Assert
            conversionLinks.CreatedUserID.ShouldBe(random);
            conversionLinks.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.CreatedUserID = null;

            // Assert
            conversionLinks.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.CreatedUserID.ShouldBeNull();
            conversionLinks.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<int>();

            // Act , Set
            conversionLinks.CustomerID = random;

            // Assert
            conversionLinks.CustomerID.ShouldBe(random);
            conversionLinks.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.CustomerID = null;

            // Assert
            conversionLinks.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.CustomerID.ShouldBeNull();
            conversionLinks.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (IsActive) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsActive_Property_String_Type_Verify_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            conversionLinks.IsActive = Fixture.Create<string>();
            var stringType = conversionLinks.IsActive.GetType();

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

        #region General Getters/Setters : Class (ConversionLinks) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<bool>();

            // Act , Set
            conversionLinks.IsDeleted = random;

            // Assert
            conversionLinks.IsDeleted.ShouldBe(random);
            conversionLinks.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.IsDeleted = null;

            // Assert
            conversionLinks.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.IsDeleted.ShouldBeNull();
            conversionLinks.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LayoutID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LayoutID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<int>();

            // Act , Set
            conversionLinks.LayoutID = random;

            // Assert
            conversionLinks.LayoutID.ShouldBe(random);
            conversionLinks.LayoutID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LayoutID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.LayoutID = null;

            // Assert
            conversionLinks.LayoutID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LayoutID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLayoutID = "LayoutID";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameLayoutID);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.LayoutID.ShouldBeNull();
            conversionLinks.LayoutID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LayoutID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_LayoutIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutIDNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameLayoutID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LayoutID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutID = "LayoutID";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameLayoutID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            conversionLinks.LinkID = Fixture.Create<int>();
            var intType = conversionLinks.LinkID.GetType();

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

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_LinkIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkID = "LinkIDNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameLinkID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkID = "LinkID";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameLinkID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            conversionLinks.LinkName = Fixture.Create<string>();
            var stringType = conversionLinks.LinkName.GetType();

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

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_LinkNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkName = "LinkNameNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameLinkName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkName = "LinkName";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameLinkName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkParams) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkParams_Property_String_Type_Verify_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            conversionLinks.LinkParams = Fixture.Create<string>();
            var stringType = conversionLinks.LinkParams.GetType();

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

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkParams) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_LinkParamsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkParams = "LinkParamsNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameLinkParams));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkParams_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkParams = "LinkParams";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameLinkParams);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            conversionLinks.LinkURL = Fixture.Create<string>();
            var stringType = conversionLinks.LinkURL.GetType();

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

        #region General Getters/Setters : Class (ConversionLinks) => Property (LinkURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_LinkURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURLNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameLinkURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_LinkURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkURL = "LinkURL";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameLinkURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (SortOrder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_SortOrder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<int>();

            // Act , Set
            conversionLinks.SortOrder = random;

            // Assert
            conversionLinks.SortOrder.ShouldBe(random);
            conversionLinks.SortOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_SortOrder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.SortOrder = null;

            // Assert
            conversionLinks.SortOrder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_SortOrder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSortOrder = "SortOrder";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameSortOrder);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.SortOrder.ShouldBeNull();
            conversionLinks.SortOrder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(conversionLinks, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var random = Fixture.Create<int>();

            // Act , Set
            conversionLinks.UpdatedUserID = random;

            // Assert
            conversionLinks.UpdatedUserID.ShouldBe(random);
            conversionLinks.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var conversionLinks = Fixture.Create<ConversionLinks>();    

            // Act , Set
            conversionLinks.UpdatedUserID = null;

            // Assert
            conversionLinks.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var conversionLinks = Fixture.Create<ConversionLinks>();
            var propertyInfo = conversionLinks.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(conversionLinks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            conversionLinks.UpdatedUserID.ShouldBeNull();
            conversionLinks.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ConversionLinks) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var conversionLinks  = Fixture.Create<ConversionLinks>();

            // Act , Assert
            Should.NotThrow(() => conversionLinks.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ConversionLinks_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var conversionLinks  = Fixture.Create<ConversionLinks>();
            var propertyInfo  = conversionLinks.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ConversionLinks) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConversionLinks_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ConversionLinks());
        }

        #endregion

        #region General Constructor : Class (ConversionLinks) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConversionLinks_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfConversionLinks = Fixture.CreateMany<ConversionLinks>(2).ToList();
            var firstConversionLinks = instancesOfConversionLinks.FirstOrDefault();
            var lastConversionLinks = instancesOfConversionLinks.Last();

            // Act, Assert
            firstConversionLinks.ShouldNotBeNull();
            lastConversionLinks.ShouldNotBeNull();
            firstConversionLinks.ShouldNotBeSameAs(lastConversionLinks);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConversionLinks_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstConversionLinks = new ConversionLinks();
            var secondConversionLinks = new ConversionLinks();
            var thirdConversionLinks = new ConversionLinks();
            var fourthConversionLinks = new ConversionLinks();
            var fifthConversionLinks = new ConversionLinks();
            var sixthConversionLinks = new ConversionLinks();

            // Act, Assert
            firstConversionLinks.ShouldNotBeNull();
            secondConversionLinks.ShouldNotBeNull();
            thirdConversionLinks.ShouldNotBeNull();
            fourthConversionLinks.ShouldNotBeNull();
            fifthConversionLinks.ShouldNotBeNull();
            sixthConversionLinks.ShouldNotBeNull();
            firstConversionLinks.ShouldNotBeSameAs(secondConversionLinks);
            thirdConversionLinks.ShouldNotBeSameAs(firstConversionLinks);
            fourthConversionLinks.ShouldNotBeSameAs(firstConversionLinks);
            fifthConversionLinks.ShouldNotBeSameAs(firstConversionLinks);
            sixthConversionLinks.ShouldNotBeSameAs(firstConversionLinks);
            sixthConversionLinks.ShouldNotBeSameAs(fourthConversionLinks);
        }

        #endregion

        #region General Constructor : Class (ConversionLinks) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ConversionLinks_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var linkId = -1;
            var linkURL = string.Empty;
            var linkParams = string.Empty;
            var linkName = string.Empty;
            var isActive = string.Empty;

            // Act
            var conversionLinks = new ConversionLinks();

            // Assert
            conversionLinks.LinkID.ShouldBe(linkId);
            conversionLinks.LayoutID.ShouldBeNull();
            conversionLinks.LinkURL.ShouldBe(linkURL);
            conversionLinks.LinkParams.ShouldBe(linkParams);
            conversionLinks.LinkName.ShouldBe(linkName);
            conversionLinks.IsActive.ShouldBe(isActive);
            conversionLinks.SortOrder.ShouldBeNull();
            conversionLinks.CreatedUserID.ShouldBeNull();
            conversionLinks.CreatedDate.ShouldBeNull();
            conversionLinks.UpdatedUserID.ShouldBeNull();
            conversionLinks.UpdatedDate.ShouldBeNull();
            conversionLinks.IsDeleted.ShouldBeNull();
            conversionLinks.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}