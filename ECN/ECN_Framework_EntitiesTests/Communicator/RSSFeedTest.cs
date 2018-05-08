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
    public class RSSFeedTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (RSSFeed) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            var feedId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var uRL = Fixture.Create<string>();
            var storiesToShow = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            rSSFeed.FeedID = feedId;
            rSSFeed.CustomerID = customerId;
            rSSFeed.Name = name;
            rSSFeed.URL = uRL;
            rSSFeed.StoriesToShow = storiesToShow;
            rSSFeed.CreatedUserID = createdUserId;
            rSSFeed.CreatedDate = createdDate;
            rSSFeed.UpdatedUserID = updatedUserId;
            rSSFeed.UpdatedDate = updatedDate;
            rSSFeed.IsDeleted = isDeleted;

            // Assert
            rSSFeed.FeedID.ShouldBe(feedId);
            rSSFeed.CustomerID.ShouldBe(customerId);
            rSSFeed.Name.ShouldBe(name);
            rSSFeed.URL.ShouldBe(uRL);
            rSSFeed.StoriesToShow.ShouldBe(storiesToShow);
            rSSFeed.CreatedUserID.ShouldBe(createdUserId);
            rSSFeed.CreatedDate.ShouldBe(createdDate);
            rSSFeed.UpdatedUserID.ShouldBe(updatedUserId);
            rSSFeed.UpdatedDate.ShouldBe(updatedDate);
            rSSFeed.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(rSSFeed, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            var random = Fixture.Create<int>();

            // Act , Set
            rSSFeed.CreatedUserID = random;

            // Assert
            rSSFeed.CreatedUserID.ShouldBe(random);
            rSSFeed.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();

            // Act , Set
            rSSFeed.CreatedUserID = null;

            // Assert
            rSSFeed.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(rSSFeed, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rSSFeed.CreatedUserID.ShouldBeNull();
            rSSFeed.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            rSSFeed.CustomerID = Fixture.Create<int>();
            var intType = rSSFeed.CustomerID.GetType();

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

        #region General Getters/Setters : Class (RSSFeed) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (FeedID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_FeedID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            rSSFeed.FeedID = Fixture.Create<int>();
            var intType = rSSFeed.FeedID.GetType();

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

        #region General Getters/Setters : Class (RSSFeed) => Property (FeedID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_FeedIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFeedID = "FeedIDNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameFeedID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_FeedID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFeedID = "FeedID";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameFeedID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            var random = Fixture.Create<bool>();

            // Act , Set
            rSSFeed.IsDeleted = random;

            // Assert
            rSSFeed.IsDeleted.ShouldBe(random);
            rSSFeed.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();

            // Act , Set
            rSSFeed.IsDeleted = null;

            // Assert
            rSSFeed.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(rSSFeed, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rSSFeed.IsDeleted.ShouldBeNull();
            rSSFeed.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            rSSFeed.Name = Fixture.Create<string>();
            var stringType = rSSFeed.Name.GetType();

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

        #region General Getters/Setters : Class (RSSFeed) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (StoriesToShow) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_StoriesToShow_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            var random = Fixture.Create<int>();

            // Act , Set
            rSSFeed.StoriesToShow = random;

            // Assert
            rSSFeed.StoriesToShow.ShouldBe(random);
            rSSFeed.StoriesToShow.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_StoriesToShow_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();

            // Act , Set
            rSSFeed.StoriesToShow = null;

            // Assert
            rSSFeed.StoriesToShow.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_StoriesToShow_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameStoriesToShow = "StoriesToShow";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameStoriesToShow);

            // Act , Set
            propertyInfo.SetValue(rSSFeed, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rSSFeed.StoriesToShow.ShouldBeNull();
            rSSFeed.StoriesToShow.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (StoriesToShow) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_StoriesToShowNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStoriesToShow = "StoriesToShowNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameStoriesToShow));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_StoriesToShow_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStoriesToShow = "StoriesToShow";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameStoriesToShow);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(rSSFeed, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            var random = Fixture.Create<int>();

            // Act , Set
            rSSFeed.UpdatedUserID = random;

            // Assert
            rSSFeed.UpdatedUserID.ShouldBe(random);
            rSSFeed.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();

            // Act , Set
            rSSFeed.UpdatedUserID = null;

            // Assert
            rSSFeed.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var rSSFeed = Fixture.Create<RSSFeed>();
            var propertyInfo = rSSFeed.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(rSSFeed, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            rSSFeed.UpdatedUserID.ShouldBeNull();
            rSSFeed.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RSSFeed) => Property (URL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_URL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var rSSFeed = Fixture.Create<RSSFeed>();
            rSSFeed.URL = Fixture.Create<string>();
            var stringType = rSSFeed.URL.GetType();

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

        #region General Getters/Setters : Class (RSSFeed) => Property (URL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_Class_Invalid_Property_URLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameURL = "URLNotPresent";
            var rSSFeed  = Fixture.Create<RSSFeed>();

            // Act , Assert
            Should.NotThrow(() => rSSFeed.GetType().GetProperty(propertyNameURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RSSFeed_URL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameURL = "URL";
            var rSSFeed  = Fixture.Create<RSSFeed>();
            var propertyInfo  = rSSFeed.GetType().GetProperty(propertyNameURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (RSSFeed) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RSSFeed_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new RSSFeed());
        }

        #endregion

        #region General Constructor : Class (RSSFeed) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RSSFeed_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfRSSFeed = Fixture.CreateMany<RSSFeed>(2).ToList();
            var firstRSSFeed = instancesOfRSSFeed.FirstOrDefault();
            var lastRSSFeed = instancesOfRSSFeed.Last();

            // Act, Assert
            firstRSSFeed.ShouldNotBeNull();
            lastRSSFeed.ShouldNotBeNull();
            firstRSSFeed.ShouldNotBeSameAs(lastRSSFeed);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RSSFeed_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstRSSFeed = new RSSFeed();
            var secondRSSFeed = new RSSFeed();
            var thirdRSSFeed = new RSSFeed();
            var fourthRSSFeed = new RSSFeed();
            var fifthRSSFeed = new RSSFeed();
            var sixthRSSFeed = new RSSFeed();

            // Act, Assert
            firstRSSFeed.ShouldNotBeNull();
            secondRSSFeed.ShouldNotBeNull();
            thirdRSSFeed.ShouldNotBeNull();
            fourthRSSFeed.ShouldNotBeNull();
            fifthRSSFeed.ShouldNotBeNull();
            sixthRSSFeed.ShouldNotBeNull();
            firstRSSFeed.ShouldNotBeSameAs(secondRSSFeed);
            thirdRSSFeed.ShouldNotBeSameAs(firstRSSFeed);
            fourthRSSFeed.ShouldNotBeSameAs(firstRSSFeed);
            fifthRSSFeed.ShouldNotBeSameAs(firstRSSFeed);
            sixthRSSFeed.ShouldNotBeSameAs(firstRSSFeed);
            sixthRSSFeed.ShouldNotBeSameAs(fourthRSSFeed);
        }

        #endregion

        #region General Constructor : Class (RSSFeed) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RSSFeed_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var feedId = -1;
            var customerId = -1;
            var name = string.Empty;
            var uRL = string.Empty;
            var isDeleted = false;

            // Act
            var rSSFeed = new RSSFeed();

            // Assert
            rSSFeed.FeedID.ShouldBe(feedId);
            rSSFeed.CustomerID.ShouldBe(customerId);
            rSSFeed.Name.ShouldBe(name);
            rSSFeed.URL.ShouldBe(uRL);
            rSSFeed.StoriesToShow.ShouldBeNull();
            rSSFeed.CreatedDate.ShouldBeNull();
            rSSFeed.CreatedUserID.ShouldBeNull();
            rSSFeed.UpdatedDate.ShouldBeNull();
            rSSFeed.UpdatedUserID.ShouldBeNull();
            rSSFeed.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #endregion

        #endregion
    }
}