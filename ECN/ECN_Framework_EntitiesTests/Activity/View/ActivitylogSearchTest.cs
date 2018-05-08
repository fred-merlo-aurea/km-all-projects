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
using ECN_Framework_Entities.Activity.View;

namespace ECN_Framework_Entities.Activity.View
{
    [TestFixture]
    public class ActivitylogSearchTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ActivitylogSearch) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            var totalRowsCount = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var blastId = Fixture.Create<string>();
            var actionDate = Fixture.Create<DateTime?>();
            var actionTypeCode = Fixture.Create<string>();
            var actionValue = Fixture.Create<string>();

            // Act
            activitylogSearch.TotalRowsCount = totalRowsCount;
            activitylogSearch.EmailID = emailId;
            activitylogSearch.EmailSubject = emailSubject;
            activitylogSearch.BlastID = blastId;
            activitylogSearch.ActionDate = actionDate;
            activitylogSearch.ActionTypeCode = actionTypeCode;
            activitylogSearch.ActionValue = actionValue;

            // Assert
            activitylogSearch.TotalRowsCount.ShouldBe(totalRowsCount);
            activitylogSearch.EmailID.ShouldBe(emailId);
            activitylogSearch.EmailSubject.ShouldBe(emailSubject);
            activitylogSearch.BlastID.ShouldBe(blastId);
            activitylogSearch.ActionDate.ShouldBe(actionDate);
            activitylogSearch.ActionTypeCode.ShouldBe(actionTypeCode);
            activitylogSearch.ActionValue.ShouldBe(actionValue);
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = activitylogSearch.GetType().GetProperty(propertyNameActionDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(activitylogSearch, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.ActionTypeCode = Fixture.Create<string>();
            var stringType = activitylogSearch.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.ActionValue = Fixture.Create<string>();
            var stringType = activitylogSearch.ActionValue.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (ActionValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_ActionValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValueNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameActionValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_ActionValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValue";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameActionValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (BlastID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_BlastID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.BlastID = Fixture.Create<string>();
            var stringType = activitylogSearch.BlastID.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.EmailID = Fixture.Create<int>();
            var intType = activitylogSearch.EmailID.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.EmailSubject = Fixture.Create<string>();
            var stringType = activitylogSearch.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (TotalRowsCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_TotalRowsCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var activitylogSearch = Fixture.Create<ActivitylogSearch>();
            activitylogSearch.TotalRowsCount = Fixture.Create<int>();
            var intType = activitylogSearch.TotalRowsCount.GetType();

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

        #region General Getters/Setters : Class (ActivitylogSearch) => Property (TotalRowsCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_Class_Invalid_Property_TotalRowsCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalRowsCount = "TotalRowsCountNotPresent";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();

            // Act , Assert
            Should.NotThrow(() => activitylogSearch.GetType().GetProperty(propertyNameTotalRowsCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ActivitylogSearch_TotalRowsCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalRowsCount = "TotalRowsCount";
            var activitylogSearch  = Fixture.Create<ActivitylogSearch>();
            var propertyInfo  = activitylogSearch.GetType().GetProperty(propertyNameTotalRowsCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ActivitylogSearch) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ActivitylogSearch_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ActivitylogSearch());
        }

        #endregion

        #region General Constructor : Class (ActivitylogSearch) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ActivitylogSearch_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfActivitylogSearch = Fixture.CreateMany<ActivitylogSearch>(2).ToList();
            var firstActivitylogSearch = instancesOfActivitylogSearch.FirstOrDefault();
            var lastActivitylogSearch = instancesOfActivitylogSearch.Last();

            // Act, Assert
            firstActivitylogSearch.ShouldNotBeNull();
            lastActivitylogSearch.ShouldNotBeNull();
            firstActivitylogSearch.ShouldNotBeSameAs(lastActivitylogSearch);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ActivitylogSearch_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstActivitylogSearch = new ActivitylogSearch();
            var secondActivitylogSearch = new ActivitylogSearch();
            var thirdActivitylogSearch = new ActivitylogSearch();
            var fourthActivitylogSearch = new ActivitylogSearch();
            var fifthActivitylogSearch = new ActivitylogSearch();
            var sixthActivitylogSearch = new ActivitylogSearch();

            // Act, Assert
            firstActivitylogSearch.ShouldNotBeNull();
            secondActivitylogSearch.ShouldNotBeNull();
            thirdActivitylogSearch.ShouldNotBeNull();
            fourthActivitylogSearch.ShouldNotBeNull();
            fifthActivitylogSearch.ShouldNotBeNull();
            sixthActivitylogSearch.ShouldNotBeNull();
            firstActivitylogSearch.ShouldNotBeSameAs(secondActivitylogSearch);
            thirdActivitylogSearch.ShouldNotBeSameAs(firstActivitylogSearch);
            fourthActivitylogSearch.ShouldNotBeSameAs(firstActivitylogSearch);
            fifthActivitylogSearch.ShouldNotBeSameAs(firstActivitylogSearch);
            sixthActivitylogSearch.ShouldNotBeSameAs(firstActivitylogSearch);
            sixthActivitylogSearch.ShouldNotBeSameAs(fourthActivitylogSearch);
        }

        #endregion

        #region General Constructor : Class (ActivitylogSearch) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ActivitylogSearch_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var totalRowsCount = 0;
            var emailId = 0;
            var emailSubject = string.Empty;
            var blastId = string.Empty;
            var actionTypeCode = string.Empty;
            var actionValue = string.Empty;

            // Act
            var activitylogSearch = new ActivitylogSearch();

            // Assert
            activitylogSearch.TotalRowsCount.ShouldBe(totalRowsCount);
            activitylogSearch.EmailID.ShouldBe(emailId);
            activitylogSearch.ActionDate.ShouldBeNull();
            activitylogSearch.EmailSubject.ShouldBe(emailSubject);
            activitylogSearch.BlastID.ShouldBe(blastId);
            activitylogSearch.ActionTypeCode.ShouldBe(actionTypeCode);
            activitylogSearch.ActionValue.ShouldBe(actionValue);
        }

        #endregion

        #endregion

        #endregion
    }
}