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
    public class CampaignItemBlastRefBlastTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var campaignItemBlastRefBlastId = Fixture.Create<int>();
            var campaignItemBlastId = Fixture.Create<int?>();
            var refBlastId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            campaignItemBlastRefBlast.CampaignItemBlastRefBlastID = campaignItemBlastRefBlastId;
            campaignItemBlastRefBlast.CampaignItemBlastID = campaignItemBlastId;
            campaignItemBlastRefBlast.RefBlastID = refBlastId;
            campaignItemBlastRefBlast.CreatedUserID = createdUserId;
            campaignItemBlastRefBlast.UpdatedUserID = updatedUserId;
            campaignItemBlastRefBlast.IsDeleted = isDeleted;
            campaignItemBlastRefBlast.CustomerID = customerId;

            // Assert
            campaignItemBlastRefBlast.CampaignItemBlastRefBlastID.ShouldBe(campaignItemBlastRefBlastId);
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBe(campaignItemBlastId);
            campaignItemBlastRefBlast.RefBlastID.ShouldBe(refBlastId);
            campaignItemBlastRefBlast.CreatedUserID.ShouldBe(createdUserId);
            campaignItemBlastRefBlast.CreatedDate.ShouldBeNull();
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBe(updatedUserId);
            campaignItemBlastRefBlast.UpdatedDate.ShouldBeNull();
            campaignItemBlastRefBlast.IsDeleted.ShouldBe(isDeleted);
            campaignItemBlastRefBlast.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CampaignItemBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastRefBlast.CampaignItemBlastID = random;

            // Assert
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBe(random);
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.CampaignItemBlastID = null;

            // Assert
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastID";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCampaignItemBlastID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBeNull();
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CampaignItemBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_CampaignItemBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCampaignItemBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastID = "CampaignItemBlastID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCampaignItemBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CampaignItemBlastRefBlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastRefBlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            campaignItemBlastRefBlast.CampaignItemBlastRefBlastID = Fixture.Create<int>();
            var intType = campaignItemBlastRefBlast.CampaignItemBlastRefBlastID.GetType();

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

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CampaignItemBlastRefBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_CampaignItemBlastRefBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastRefBlastID = "CampaignItemBlastRefBlastIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCampaignItemBlastRefBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CampaignItemBlastRefBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemBlastRefBlastID = "CampaignItemBlastRefBlastID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCampaignItemBlastRefBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemBlastRefBlast, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastRefBlast.CreatedUserID = random;

            // Assert
            campaignItemBlastRefBlast.CreatedUserID.ShouldBe(random);
            campaignItemBlastRefBlast.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.CreatedUserID = null;

            // Assert
            campaignItemBlastRefBlast.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.CreatedUserID.ShouldBeNull();
            campaignItemBlastRefBlast.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastRefBlast.CustomerID = random;

            // Assert
            campaignItemBlastRefBlast.CustomerID.ShouldBe(random);
            campaignItemBlastRefBlast.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.CustomerID = null;

            // Assert
            campaignItemBlastRefBlast.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.CustomerID.ShouldBeNull();
            campaignItemBlastRefBlast.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<bool>();

            // Act , Set
            campaignItemBlastRefBlast.IsDeleted = random;

            // Assert
            campaignItemBlastRefBlast.IsDeleted.ShouldBe(random);
            campaignItemBlastRefBlast.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.IsDeleted = null;

            // Assert
            campaignItemBlastRefBlast.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.IsDeleted.ShouldBeNull();
            campaignItemBlastRefBlast.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (RefBlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_RefBlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastRefBlast.RefBlastID = random;

            // Assert
            campaignItemBlastRefBlast.RefBlastID.ShouldBe(random);
            campaignItemBlastRefBlast.RefBlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_RefBlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.RefBlastID = null;

            // Assert
            campaignItemBlastRefBlast.RefBlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_RefBlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRefBlastID = "RefBlastID";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameRefBlastID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.RefBlastID.ShouldBeNull();
            campaignItemBlastRefBlast.RefBlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (RefBlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_RefBlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRefBlastID = "RefBlastIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameRefBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_RefBlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRefBlastID = "RefBlastID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameRefBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignItemBlastRefBlast, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var random = Fixture.Create<int>();

            // Act , Set
            campaignItemBlastRefBlast.UpdatedUserID = random;

            // Assert
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBe(random);
            campaignItemBlastRefBlast.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();    

            // Act , Set
            campaignItemBlastRefBlast.UpdatedUserID = null;

            // Assert
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemBlastRefBlast = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(campaignItemBlastRefBlast, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBeNull();
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignItemBlastRefBlast) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();

            // Act , Assert
            Should.NotThrow(() => campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignItemBlastRefBlast_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var campaignItemBlastRefBlast  = Fixture.Create<CampaignItemBlastRefBlast>();
            var propertyInfo  = campaignItemBlastRefBlast.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignItemBlastRefBlast) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastRefBlast_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignItemBlastRefBlast());
        }

        #endregion

        #region General Constructor : Class (CampaignItemBlastRefBlast) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastRefBlast_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignItemBlastRefBlast = Fixture.CreateMany<CampaignItemBlastRefBlast>(2).ToList();
            var firstCampaignItemBlastRefBlast = instancesOfCampaignItemBlastRefBlast.FirstOrDefault();
            var lastCampaignItemBlastRefBlast = instancesOfCampaignItemBlastRefBlast.Last();

            // Act, Assert
            firstCampaignItemBlastRefBlast.ShouldNotBeNull();
            lastCampaignItemBlastRefBlast.ShouldNotBeNull();
            firstCampaignItemBlastRefBlast.ShouldNotBeSameAs(lastCampaignItemBlastRefBlast);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastRefBlast_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();
            var secondCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();
            var thirdCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();
            var fourthCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();
            var fifthCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();
            var sixthCampaignItemBlastRefBlast = new CampaignItemBlastRefBlast();

            // Act, Assert
            firstCampaignItemBlastRefBlast.ShouldNotBeNull();
            secondCampaignItemBlastRefBlast.ShouldNotBeNull();
            thirdCampaignItemBlastRefBlast.ShouldNotBeNull();
            fourthCampaignItemBlastRefBlast.ShouldNotBeNull();
            fifthCampaignItemBlastRefBlast.ShouldNotBeNull();
            sixthCampaignItemBlastRefBlast.ShouldNotBeNull();
            firstCampaignItemBlastRefBlast.ShouldNotBeSameAs(secondCampaignItemBlastRefBlast);
            thirdCampaignItemBlastRefBlast.ShouldNotBeSameAs(firstCampaignItemBlastRefBlast);
            fourthCampaignItemBlastRefBlast.ShouldNotBeSameAs(firstCampaignItemBlastRefBlast);
            fifthCampaignItemBlastRefBlast.ShouldNotBeSameAs(firstCampaignItemBlastRefBlast);
            sixthCampaignItemBlastRefBlast.ShouldNotBeSameAs(firstCampaignItemBlastRefBlast);
            sixthCampaignItemBlastRefBlast.ShouldNotBeSameAs(fourthCampaignItemBlastRefBlast);
        }

        #endregion

        #region General Constructor : Class (CampaignItemBlastRefBlast) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignItemBlastRefBlast_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var campaignItemBlastRefBlastId = 0;

            // Act
            var campaignItemBlastRefBlast = new CampaignItemBlastRefBlast();

            // Assert
            campaignItemBlastRefBlast.CampaignItemBlastRefBlastID.ShouldBe(campaignItemBlastRefBlastId);
            campaignItemBlastRefBlast.CampaignItemBlastID.ShouldBeNull();
            campaignItemBlastRefBlast.RefBlastID.ShouldBeNull();
            campaignItemBlastRefBlast.CreatedUserID.ShouldBeNull();
            campaignItemBlastRefBlast.CreatedDate.ShouldBeNull();
            campaignItemBlastRefBlast.UpdatedUserID.ShouldBeNull();
            campaignItemBlastRefBlast.UpdatedDate.ShouldBeNull();
            campaignItemBlastRefBlast.IsDeleted.ShouldBeNull();
            campaignItemBlastRefBlast.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}