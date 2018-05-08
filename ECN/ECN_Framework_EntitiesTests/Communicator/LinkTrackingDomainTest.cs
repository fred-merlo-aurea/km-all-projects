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
    public class LinkTrackingDomainTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkTrackingDomain) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var linkTrackingDomainId = Fixture.Create<int>();
            var lTId = Fixture.Create<int>();
            var domain = Fixture.Create<string>();
            var customerId = Fixture.Create<int>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            linkTrackingDomain.LinkTrackingDomainID = linkTrackingDomainId;
            linkTrackingDomain.LTID = lTId;
            linkTrackingDomain.Domain = domain;
            linkTrackingDomain.CustomerID = customerId;
            linkTrackingDomain.CreatedUserID = createdUserId;
            linkTrackingDomain.UpdatedUserID = updatedUserId;
            linkTrackingDomain.IsDeleted = isDeleted;

            // Assert
            linkTrackingDomain.LinkTrackingDomainID.ShouldBe(linkTrackingDomainId);
            linkTrackingDomain.LTID.ShouldBe(lTId);
            linkTrackingDomain.Domain.ShouldBe(domain);
            linkTrackingDomain.CustomerID.ShouldBe(customerId);
            linkTrackingDomain.CreatedUserID.ShouldBe(createdUserId);
            linkTrackingDomain.CreatedDate.ShouldBeNull();
            linkTrackingDomain.UpdatedUserID.ShouldBe(updatedUserId);
            linkTrackingDomain.UpdatedDate.ShouldBeNull();
            linkTrackingDomain.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingDomain.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingDomain, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingDomain.CreatedUserID = random;

            // Assert
            linkTrackingDomain.CreatedUserID.ShouldBe(random);
            linkTrackingDomain.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();

            // Act , Set
            linkTrackingDomain.CreatedUserID = null;

            // Assert
            linkTrackingDomain.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo = linkTrackingDomain.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingDomain, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingDomain.CreatedUserID.ShouldBeNull();
            linkTrackingDomain.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            linkTrackingDomain.CustomerID = Fixture.Create<int>();
            var intType = linkTrackingDomain.CustomerID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (Domain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Domain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            linkTrackingDomain.Domain = Fixture.Create<string>();
            var stringType = linkTrackingDomain.Domain.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (Domain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_DomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomain = "DomainNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Domain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomain = "Domain";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkTrackingDomain.IsDeleted = random;

            // Assert
            linkTrackingDomain.IsDeleted.ShouldBe(random);
            linkTrackingDomain.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();

            // Act , Set
            linkTrackingDomain.IsDeleted = null;

            // Assert
            linkTrackingDomain.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo = linkTrackingDomain.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(linkTrackingDomain, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingDomain.IsDeleted.ShouldBeNull();
            linkTrackingDomain.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (LinkTrackingDomainID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_LinkTrackingDomainID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            linkTrackingDomain.LinkTrackingDomainID = Fixture.Create<int>();
            var intType = linkTrackingDomain.LinkTrackingDomainID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (LinkTrackingDomainID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_LinkTrackingDomainIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkTrackingDomainID = "LinkTrackingDomainIDNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameLinkTrackingDomainID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_LinkTrackingDomainID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkTrackingDomainID = "LinkTrackingDomainID";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameLinkTrackingDomainID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (LTID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_LTID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            linkTrackingDomain.LTID = Fixture.Create<int>();
            var intType = linkTrackingDomain.LTID.GetType();

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

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (LTID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_LTIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTIDNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameLTID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_LTID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLTID = "LTID";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameLTID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkTrackingDomain, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkTrackingDomain.UpdatedUserID = random;

            // Assert
            linkTrackingDomain.UpdatedUserID.ShouldBe(random);
            linkTrackingDomain.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();

            // Act , Set
            linkTrackingDomain.UpdatedUserID = null;

            // Assert
            linkTrackingDomain.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingDomain = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo = linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkTrackingDomain, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkTrackingDomain.UpdatedUserID.ShouldBeNull();
            linkTrackingDomain.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkTrackingDomain) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();

            // Act , Assert
            Should.NotThrow(() => linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkTrackingDomain_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkTrackingDomain  = Fixture.Create<LinkTrackingDomain>();
            var propertyInfo  = linkTrackingDomain.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkTrackingDomain) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingDomain_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkTrackingDomain());
        }

        #endregion

        #region General Constructor : Class (LinkTrackingDomain) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingDomain_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkTrackingDomain = Fixture.CreateMany<LinkTrackingDomain>(2).ToList();
            var firstLinkTrackingDomain = instancesOfLinkTrackingDomain.FirstOrDefault();
            var lastLinkTrackingDomain = instancesOfLinkTrackingDomain.Last();

            // Act, Assert
            firstLinkTrackingDomain.ShouldNotBeNull();
            lastLinkTrackingDomain.ShouldNotBeNull();
            firstLinkTrackingDomain.ShouldNotBeSameAs(lastLinkTrackingDomain);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingDomain_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkTrackingDomain = new LinkTrackingDomain();
            var secondLinkTrackingDomain = new LinkTrackingDomain();
            var thirdLinkTrackingDomain = new LinkTrackingDomain();
            var fourthLinkTrackingDomain = new LinkTrackingDomain();
            var fifthLinkTrackingDomain = new LinkTrackingDomain();
            var sixthLinkTrackingDomain = new LinkTrackingDomain();

            // Act, Assert
            firstLinkTrackingDomain.ShouldNotBeNull();
            secondLinkTrackingDomain.ShouldNotBeNull();
            thirdLinkTrackingDomain.ShouldNotBeNull();
            fourthLinkTrackingDomain.ShouldNotBeNull();
            fifthLinkTrackingDomain.ShouldNotBeNull();
            sixthLinkTrackingDomain.ShouldNotBeNull();
            firstLinkTrackingDomain.ShouldNotBeSameAs(secondLinkTrackingDomain);
            thirdLinkTrackingDomain.ShouldNotBeSameAs(firstLinkTrackingDomain);
            fourthLinkTrackingDomain.ShouldNotBeSameAs(firstLinkTrackingDomain);
            fifthLinkTrackingDomain.ShouldNotBeSameAs(firstLinkTrackingDomain);
            sixthLinkTrackingDomain.ShouldNotBeSameAs(firstLinkTrackingDomain);
            sixthLinkTrackingDomain.ShouldNotBeSameAs(fourthLinkTrackingDomain);
        }

        #endregion

        #region General Constructor : Class (LinkTrackingDomain) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkTrackingDomain_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var lTId = -1;
            var linkTrackingDomainId = -1;
            var domain = string.Empty;
            var customerId = -1;

            // Act
            var linkTrackingDomain = new LinkTrackingDomain();

            // Assert
            linkTrackingDomain.LTID.ShouldBe(lTId);
            linkTrackingDomain.LinkTrackingDomainID.ShouldBe(linkTrackingDomainId);
            linkTrackingDomain.Domain.ShouldBe(domain);
            linkTrackingDomain.CustomerID.ShouldBe(customerId);
            linkTrackingDomain.CreatedUserID.ShouldBeNull();
            linkTrackingDomain.CreatedDate.ShouldBeNull();
            linkTrackingDomain.UpdatedUserID.ShouldBeNull();
            linkTrackingDomain.UpdatedDate.ShouldBeNull();
            linkTrackingDomain.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}