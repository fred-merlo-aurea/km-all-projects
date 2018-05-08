using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Collector;

namespace ECN_Framework_Entities.Collector
{
    [TestFixture]
    public class PageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var page  = new Page();
            var pageID = Fixture.Create<int>();
            var surveyID = Fixture.Create<int>();
            var pageHeader = Fixture.Create<string>();
            var pageDesc = Fixture.Create<string>();
            var number = Fixture.Create<int>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();

            var isDeleted = Fixture.Create<bool>();
            var questionList = Fixture.Create<List<Question>>();
            var isSkipped = Fixture.Create<bool>();
            var isThankYouPage = Fixture.Create<bool>();
            var isIntroPage = Fixture.Create<bool>();

            // Act
            page.PageID = pageID;
            page.SurveyID = surveyID;
            page.PageHeader = pageHeader;
            page.PageDesc = pageDesc;
            page.Number = number;
            page.CreatedUserID = createdUserID;
            page.CreatedDate = createdDate;
            page.UpdatedUserID = updatedUserID;

            page.IsDeleted = isDeleted;
            page.QuestionList = questionList;
            page.IsSkipped = isSkipped;
            page.IsThankYouPage = isThankYouPage;
            page.IsIntroPage = isIntroPage;

            // Assert
            page.PageID.ShouldBe(pageID);
            page.SurveyID.ShouldBe(surveyID);
            page.PageHeader.ShouldBe(pageHeader);
            page.PageDesc.ShouldBe(pageDesc);
            page.Number.ShouldBe(number);
            page.CreatedUserID.ShouldBe(createdUserID);
            page.CreatedDate.ShouldBe(createdDate);
            page.UpdatedUserID.ShouldBe(updatedUserID);
            page.UpdatedDate.ShouldBeNull();
            page.IsDeleted.ShouldBe(isDeleted);
            page.QuestionList.ShouldBe(questionList);
            page.IsSkipped.ShouldBe(isSkipped);
            page.IsThankYouPage.ShouldBe(isThankYouPage);
            page.IsIntroPage.ShouldBe(isIntroPage);   
        }

        #endregion

        #region Non-String Property Type Test : Page => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var page = Fixture.Create<Page>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = page.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(page, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Page => CreatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var random = Fixture.Create<int>();

            // Act , Set
            page.CreatedUserID = random;

            // Assert
            page.CreatedUserID.ShouldBe(random);
            page.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();    

            // Act , Set
            page.CreatedUserID = null;

            // Assert
            page.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var page = Fixture.Create<Page>();
            var propertyInfo = page.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(page, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            page.CreatedUserID.ShouldBeNull();
            page.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Page => IsDeleted

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var boolType = page.IsDeleted.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Page => IsIntroPage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsIntroPage_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var boolType = page.IsIntroPage.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_IsIntroPageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsIntroPage = "IsIntroPageNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameIsIntroPage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsIntroPage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsIntroPage = "IsIntroPage";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameIsIntroPage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Page => IsSkipped

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsSkipped_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var boolType = page.IsSkipped.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_IsSkippedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsSkipped = "IsSkippedNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameIsSkipped));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsSkipped_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsSkipped = "IsSkipped";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameIsSkipped);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : Page => IsThankYouPage

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsThankYouPage_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var boolType = page.IsThankYouPage.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);    
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_IsThankYouPageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsThankYouPage = "IsThankYouPageNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameIsThankYouPage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_IsThankYouPage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsThankYouPage = "IsThankYouPage";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameIsThankYouPage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Page => Number

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Number_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var intType = page.Number.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_NumberNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNumber = "NumberNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Number_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNumber = "Number";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameNumber);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Page => PageDesc

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageDesc_Property_String_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var stringType = page.PageDesc.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_PageDescNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageDesc = "PageDescNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNamePageDesc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageDesc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageDesc = "PageDesc";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNamePageDesc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Page => PageHeader

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageHeader_Property_String_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var stringType = page.PageHeader.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_PageHeaderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageHeader = "PageHeaderNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNamePageHeader));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageHeader_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageHeader = "PageHeader";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNamePageHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Page => PageID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var intType = page.PageID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_PageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageIDNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNamePageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_PageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePageID = "PageID";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNamePageID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_QuestionListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuestionList = "QuestionListNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameQuestionList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_QuestionList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuestionList = "QuestionList";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameQuestionList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Page => SurveyID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_SurveyID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var intType = page.SurveyID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyIDNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameSurveyID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyID";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameSurveyID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Page => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var page = Fixture.Create<Page>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = page.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(page, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Page => UpdatedUserID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();
            var random = Fixture.Create<int>();

            // Act , Set
            page.UpdatedUserID = random;

            // Assert
            page.UpdatedUserID.ShouldBe(random);
            page.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var page = Fixture.Create<Page>();    

            // Act , Set
            page.UpdatedUserID = null;

            // Assert
            page.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var page = Fixture.Create<Page>();
            var propertyInfo = page.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(page, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            page.UpdatedUserID.ShouldBeNull();
            page.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var page  = Fixture.Create<Page>();

            // Act , Assert
            Should.NotThrow(() => page.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Page_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var page  = Fixture.Create<Page>();
            var propertyInfo  = page.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Page_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Page());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Page>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region Contructor Default Assignment Test : Page => Page()

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Page_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var pageID = -1;
            var surveyID = -1;
            var pageHeader = string.Empty;
            var pageDesc = string.Empty;
            var number = -1;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            var isDeleted = false;
            var isIntroPage = false;
            var isThankYouPage = false;
            var isSkipped = false;

            // Act
            var page = new Page();

            // Assert
            page.PageID.ShouldBe(pageID);
            page.SurveyID.ShouldBe(surveyID);
            page.PageHeader.ShouldBe(pageHeader);
            page.PageDesc.ShouldBe(pageDesc);
            page.Number.ShouldBe(number);
            page.CreatedUserID.ShouldBeNull();
            page.CreatedDate.ShouldBeNull();
            page.UpdatedUserID.ShouldBeNull();
            page.UpdatedDate.ShouldBeNull();
            page.IsDeleted.ShouldBeFalse();
            page.IsIntroPage.ShouldBeFalse();
            page.IsThankYouPage.ShouldBeFalse();
            page.IsSkipped.ShouldBeFalse();
        }

        #endregion


        #endregion


        #endregion
    }
}