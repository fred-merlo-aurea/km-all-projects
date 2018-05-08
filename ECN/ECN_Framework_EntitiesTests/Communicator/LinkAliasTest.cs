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
    public class LinkAliasTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkAlias) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var aliasId = Fixture.Create<int>();
            var contentId = Fixture.Create<int?>();
            var link = Fixture.Create<string>();
            var alias = Fixture.Create<string>();
            var linkOwnerId = Fixture.Create<int?>();
            var linkTypeId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var linkOwner = Fixture.Create<ECN_Framework_Entities.Communicator.LinkOwnerIndex>();

            // Act
            linkAlias.AliasID = aliasId;
            linkAlias.ContentID = contentId;
            linkAlias.Link = link;
            linkAlias.Alias = alias;
            linkAlias.LinkOwnerID = linkOwnerId;
            linkAlias.LinkTypeID = linkTypeId;
            linkAlias.CreatedUserID = createdUserId;
            linkAlias.UpdatedUserID = updatedUserId;
            linkAlias.IsDeleted = isDeleted;
            linkAlias.CustomerID = customerId;
            linkAlias.linkOwner = linkOwner;

            // Assert
            linkAlias.AliasID.ShouldBe(aliasId);
            linkAlias.ContentID.ShouldBe(contentId);
            linkAlias.Link.ShouldBe(link);
            linkAlias.Alias.ShouldBe(alias);
            linkAlias.LinkOwnerID.ShouldBe(linkOwnerId);
            linkAlias.LinkTypeID.ShouldBe(linkTypeId);
            linkAlias.CreatedUserID.ShouldBe(createdUserId);
            linkAlias.CreatedDate.ShouldBeNull();
            linkAlias.UpdatedUserID.ShouldBe(updatedUserId);
            linkAlias.UpdatedDate.ShouldBeNull();
            linkAlias.IsDeleted.ShouldBe(isDeleted);
            linkAlias.CustomerID.ShouldBe(customerId);
            linkAlias.linkOwner.ShouldBe(linkOwner);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (Alias) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Alias_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            linkAlias.Alias = Fixture.Create<string>();
            var stringType = linkAlias.Alias.GetType();

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

        #region General Getters/Setters : Class (LinkAlias) => Property (Alias) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_AliasNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAlias = "AliasNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameAlias));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Alias_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAlias = "Alias";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameAlias);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (AliasID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_AliasID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            linkAlias.AliasID = Fixture.Create<int>();
            var intType = linkAlias.AliasID.GetType();

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

        #region General Getters/Setters : Class (LinkAlias) => Property (AliasID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_AliasIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAliasID = "AliasIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameAliasID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_AliasID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAliasID = "AliasID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameAliasID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (ContentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_ContentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.ContentID = random;

            // Assert
            linkAlias.ContentID.ShouldBe(random);
            linkAlias.ContentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_ContentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.ContentID = null;

            // Assert
            linkAlias.ContentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_ContentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameContentID = "ContentID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameContentID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.ContentID.ShouldBeNull();
            linkAlias.ContentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (ContentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_ContentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameContentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_ContentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContentID = "ContentID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameContentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkAlias = Fixture.Create<LinkAlias>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkAlias, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.CreatedUserID = random;

            // Assert
            linkAlias.CreatedUserID.ShouldBe(random);
            linkAlias.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.CreatedUserID = null;

            // Assert
            linkAlias.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.CreatedUserID.ShouldBeNull();
            linkAlias.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.CustomerID = random;

            // Assert
            linkAlias.CustomerID.ShouldBe(random);
            linkAlias.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.CustomerID = null;

            // Assert
            linkAlias.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.CustomerID.ShouldBeNull();
            linkAlias.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkAlias.IsDeleted = random;

            // Assert
            linkAlias.IsDeleted.ShouldBe(random);
            linkAlias.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.IsDeleted = null;

            // Assert
            linkAlias.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.IsDeleted.ShouldBeNull();
            linkAlias.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (Link) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Link_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            linkAlias.Link = Fixture.Create<string>();
            var stringType = linkAlias.Link.GetType();

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

        #region General Getters/Setters : Class (LinkAlias) => Property (Link) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_LinkNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLink = "LinkNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameLink));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Link_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLink = "Link";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameLink);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (linkOwner) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_linkOwner_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNamelinkOwner = "linkOwner";
            var linkAlias = Fixture.Create<LinkAlias>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNamelinkOwner);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkAlias, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (linkOwner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_linkOwnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamelinkOwner = "linkOwnerNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNamelinkOwner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_linkOwner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamelinkOwner = "linkOwner";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNamelinkOwner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (LinkOwnerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkOwnerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.LinkOwnerID = random;

            // Assert
            linkAlias.LinkOwnerID.ShouldBe(random);
            linkAlias.LinkOwnerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkOwnerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.LinkOwnerID = null;

            // Assert
            linkAlias.LinkOwnerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkOwnerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLinkOwnerID = "LinkOwnerID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameLinkOwnerID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.LinkOwnerID.ShouldBeNull();
            linkAlias.LinkOwnerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (LinkOwnerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_LinkOwnerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerID = "LinkOwnerIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameLinkOwnerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkOwnerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerID = "LinkOwnerID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameLinkOwnerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (LinkTypeID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkTypeID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.LinkTypeID = random;

            // Assert
            linkAlias.LinkTypeID.ShouldBe(random);
            linkAlias.LinkTypeID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkTypeID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.LinkTypeID = null;

            // Assert
            linkAlias.LinkTypeID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkTypeID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameLinkTypeID = "LinkTypeID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameLinkTypeID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.LinkTypeID.ShouldBeNull();
            linkAlias.LinkTypeID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (LinkTypeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_LinkTypeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkTypeID = "LinkTypeIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameLinkTypeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_LinkTypeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkTypeID = "LinkTypeID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameLinkTypeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkAlias = Fixture.Create<LinkAlias>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkAlias, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkAlias.UpdatedUserID = random;

            // Assert
            linkAlias.UpdatedUserID.ShouldBe(random);
            linkAlias.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkAlias = Fixture.Create<LinkAlias>();

            // Act , Set
            linkAlias.UpdatedUserID = null;

            // Assert
            linkAlias.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkAlias = Fixture.Create<LinkAlias>();
            var propertyInfo = linkAlias.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkAlias, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkAlias.UpdatedUserID.ShouldBeNull();
            linkAlias.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkAlias) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkAlias  = Fixture.Create<LinkAlias>();

            // Act , Assert
            Should.NotThrow(() => linkAlias.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkAlias_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkAlias  = Fixture.Create<LinkAlias>();
            var propertyInfo  = linkAlias.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkAlias) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkAlias_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkAlias());
        }

        #endregion

        #region General Constructor : Class (LinkAlias) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkAlias_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkAlias = Fixture.CreateMany<LinkAlias>(2).ToList();
            var firstLinkAlias = instancesOfLinkAlias.FirstOrDefault();
            var lastLinkAlias = instancesOfLinkAlias.Last();

            // Act, Assert
            firstLinkAlias.ShouldNotBeNull();
            lastLinkAlias.ShouldNotBeNull();
            firstLinkAlias.ShouldNotBeSameAs(lastLinkAlias);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkAlias_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkAlias = new LinkAlias();
            var secondLinkAlias = new LinkAlias();
            var thirdLinkAlias = new LinkAlias();
            var fourthLinkAlias = new LinkAlias();
            var fifthLinkAlias = new LinkAlias();
            var sixthLinkAlias = new LinkAlias();

            // Act, Assert
            firstLinkAlias.ShouldNotBeNull();
            secondLinkAlias.ShouldNotBeNull();
            thirdLinkAlias.ShouldNotBeNull();
            fourthLinkAlias.ShouldNotBeNull();
            fifthLinkAlias.ShouldNotBeNull();
            sixthLinkAlias.ShouldNotBeNull();
            firstLinkAlias.ShouldNotBeSameAs(secondLinkAlias);
            thirdLinkAlias.ShouldNotBeSameAs(firstLinkAlias);
            fourthLinkAlias.ShouldNotBeSameAs(firstLinkAlias);
            fifthLinkAlias.ShouldNotBeSameAs(firstLinkAlias);
            sixthLinkAlias.ShouldNotBeSameAs(firstLinkAlias);
            sixthLinkAlias.ShouldNotBeSameAs(fourthLinkAlias);
        }

        #endregion

        #region General Constructor : Class (LinkAlias) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkAlias_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var aliasId = -1;
            var link = string.Empty;
            var alias = string.Empty;

            // Act
            var linkAlias = new LinkAlias();

            // Assert
            linkAlias.AliasID.ShouldBe(aliasId);
            linkAlias.ContentID.ShouldBeNull();
            linkAlias.Link.ShouldBe(link);
            linkAlias.Alias.ShouldBe(alias);
            linkAlias.LinkOwnerID.ShouldBeNull();
            linkAlias.LinkTypeID.ShouldBeNull();
            linkAlias.CreatedUserID.ShouldBeNull();
            linkAlias.CreatedDate.ShouldBeNull();
            linkAlias.UpdatedUserID.ShouldBeNull();
            linkAlias.UpdatedDate.ShouldBeNull();
            linkAlias.IsDeleted.ShouldBeNull();
            linkAlias.linkOwner.ShouldBeNull();
            linkAlias.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}