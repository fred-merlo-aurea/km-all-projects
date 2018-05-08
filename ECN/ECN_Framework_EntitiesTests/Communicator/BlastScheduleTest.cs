using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class BlastScheduleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastSchedule) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var blastScheduleId = Fixture.Create<int?>();
            var schedTime = Fixture.Create<string>();
            var schedStartDate = Fixture.Create<string>();
            var schedEndDate = Fixture.Create<string>();
            var period = Fixture.Create<string>();
            var createdBy = Fixture.Create<int?>();
            var updatedBy = Fixture.Create<int?>();
            var daysList = Fixture.Create<List<BlastScheduleDays>>();
            var splitType = Fixture.Create<string>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            blastSchedule.BlastScheduleID = blastScheduleId;
            blastSchedule.SchedTime = schedTime;
            blastSchedule.SchedStartDate = schedStartDate;
            blastSchedule.SchedEndDate = schedEndDate;
            blastSchedule.Period = period;
            blastSchedule.CreatedBy = createdBy;
            blastSchedule.UpdatedBy = updatedBy;
            blastSchedule.DaysList = daysList;
            blastSchedule.SplitType = splitType;
            blastSchedule.ErrorList = errorList;

            // Assert
            blastSchedule.BlastScheduleID.ShouldBe(blastScheduleId);
            blastSchedule.SchedTime.ShouldBe(schedTime);
            blastSchedule.SchedStartDate.ShouldBe(schedStartDate);
            blastSchedule.SchedEndDate.ShouldBe(schedEndDate);
            blastSchedule.Period.ShouldBe(period);
            blastSchedule.CreatedBy.ShouldBe(createdBy);
            blastSchedule.CreatedDate.ShouldBeNull();
            blastSchedule.UpdatedBy.ShouldBe(updatedBy);
            blastSchedule.UpdatedDate.ShouldBeNull();
            blastSchedule.DaysList.ShouldBe(daysList);
            blastSchedule.SplitType.ShouldBe(splitType);
            blastSchedule.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (BlastScheduleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_BlastScheduleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSchedule.BlastScheduleID = random;

            // Assert
            blastSchedule.BlastScheduleID.ShouldBe(random);
            blastSchedule.BlastScheduleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_BlastScheduleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();    

            // Act , Set
            blastSchedule.BlastScheduleID = null;

            // Assert
            blastSchedule.BlastScheduleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_BlastScheduleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var propertyInfo = blastSchedule.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act , Set
            propertyInfo.SetValue(blastSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSchedule.BlastScheduleID.ShouldBeNull();
            blastSchedule.BlastScheduleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (BlastScheduleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_BlastScheduleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleIDNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameBlastScheduleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_BlastScheduleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastScheduleID = "BlastScheduleID";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameBlastScheduleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (CreatedBy) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedBy_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSchedule.CreatedBy = random;

            // Assert
            blastSchedule.CreatedBy.ShouldBe(random);
            blastSchedule.CreatedBy.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedBy_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();    

            // Act , Set
            blastSchedule.CreatedBy = null;

            // Assert
            blastSchedule.CreatedBy.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedBy_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedBy = "CreatedBy";
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var propertyInfo = blastSchedule.GetType().GetProperty(propertyNameCreatedBy);

            // Act , Set
            propertyInfo.SetValue(blastSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSchedule.CreatedBy.ShouldBeNull();
            blastSchedule.CreatedBy.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (CreatedBy) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_CreatedByNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedBy = "CreatedByNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameCreatedBy));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedBy_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedBy = "CreatedBy";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameCreatedBy);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSchedule.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSchedule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (DaysList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_DaysListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDaysList = "DaysListNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameDaysList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_DaysList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDaysList = "DaysList";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameDaysList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (ErrorList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (Period) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Period_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            blastSchedule.Period = Fixture.Create<string>();
            var stringType = blastSchedule.Period.GetType();

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

        #region General Getters/Setters : Class (BlastSchedule) => Property (Period) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_PeriodNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePeriod = "PeriodNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNamePeriod));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Period_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePeriod = "Period";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNamePeriod);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedEndDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedEndDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            blastSchedule.SchedEndDate = Fixture.Create<string>();
            var stringType = blastSchedule.SchedEndDate.GetType();

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

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedEndDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_SchedEndDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSchedEndDate = "SchedEndDateNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameSchedEndDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedEndDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSchedEndDate = "SchedEndDate";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameSchedEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedStartDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedStartDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            blastSchedule.SchedStartDate = Fixture.Create<string>();
            var stringType = blastSchedule.SchedStartDate.GetType();

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

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedStartDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_SchedStartDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSchedStartDate = "SchedStartDateNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameSchedStartDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedStartDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSchedStartDate = "SchedStartDate";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameSchedStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedTime) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedTime_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            blastSchedule.SchedTime = Fixture.Create<string>();
            var stringType = blastSchedule.SchedTime.GetType();

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

        #region General Getters/Setters : Class (BlastSchedule) => Property (SchedTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_SchedTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSchedTime = "SchedTimeNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameSchedTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SchedTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSchedTime = "SchedTime";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameSchedTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (SplitType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SplitType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            blastSchedule.SplitType = Fixture.Create<string>();
            var stringType = blastSchedule.SplitType.GetType();

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

        #region General Getters/Setters : Class (BlastSchedule) => Property (SplitType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_SplitTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSplitType = "SplitTypeNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameSplitType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_SplitType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSplitType = "SplitType";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameSplitType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (UpdatedBy) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedBy_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastSchedule.UpdatedBy = random;

            // Assert
            blastSchedule.UpdatedBy.ShouldBe(random);
            blastSchedule.UpdatedBy.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedBy_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastSchedule = Fixture.Create<BlastSchedule>();    

            // Act , Set
            blastSchedule.UpdatedBy = null;

            // Assert
            blastSchedule.UpdatedBy.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedBy_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedBy = "UpdatedBy";
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var propertyInfo = blastSchedule.GetType().GetProperty(propertyNameUpdatedBy);

            // Act , Set
            propertyInfo.SetValue(blastSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastSchedule.UpdatedBy.ShouldBeNull();
            blastSchedule.UpdatedBy.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (UpdatedBy) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_UpdatedByNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedBy = "UpdatedByNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameUpdatedBy));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedBy_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedBy = "UpdatedBy";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameUpdatedBy);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastSchedule = Fixture.Create<BlastSchedule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastSchedule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastSchedule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastSchedule) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastSchedule  = Fixture.Create<BlastSchedule>();

            // Act , Assert
            Should.NotThrow(() => blastSchedule.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastSchedule_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastSchedule  = Fixture.Create<BlastSchedule>();
            var propertyInfo  = blastSchedule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastSchedule) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSchedule_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastSchedule());
        }

        #endregion

        #region General Constructor : Class (BlastSchedule) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSchedule_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastSchedule = Fixture.CreateMany<BlastSchedule>(2).ToList();
            var firstBlastSchedule = instancesOfBlastSchedule.FirstOrDefault();
            var lastBlastSchedule = instancesOfBlastSchedule.Last();

            // Act, Assert
            firstBlastSchedule.ShouldNotBeNull();
            lastBlastSchedule.ShouldNotBeNull();
            firstBlastSchedule.ShouldNotBeSameAs(lastBlastSchedule);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSchedule_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastSchedule = new BlastSchedule();
            var secondBlastSchedule = new BlastSchedule();
            var thirdBlastSchedule = new BlastSchedule();
            var fourthBlastSchedule = new BlastSchedule();
            var fifthBlastSchedule = new BlastSchedule();
            var sixthBlastSchedule = new BlastSchedule();

            // Act, Assert
            firstBlastSchedule.ShouldNotBeNull();
            secondBlastSchedule.ShouldNotBeNull();
            thirdBlastSchedule.ShouldNotBeNull();
            fourthBlastSchedule.ShouldNotBeNull();
            fifthBlastSchedule.ShouldNotBeNull();
            sixthBlastSchedule.ShouldNotBeNull();
            firstBlastSchedule.ShouldNotBeSameAs(secondBlastSchedule);
            thirdBlastSchedule.ShouldNotBeSameAs(firstBlastSchedule);
            fourthBlastSchedule.ShouldNotBeSameAs(firstBlastSchedule);
            fifthBlastSchedule.ShouldNotBeSameAs(firstBlastSchedule);
            sixthBlastSchedule.ShouldNotBeSameAs(firstBlastSchedule);
            sixthBlastSchedule.ShouldNotBeSameAs(fourthBlastSchedule);
        }

        #endregion

        #region General Constructor : Class (BlastSchedule) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastSchedule_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var schedTime = string.Empty;
            var schedStartDate = string.Empty;
            var schedEndDate = string.Empty;
            var period = string.Empty;
            var splitType = string.Empty;
            var errorList = new List<ECNError>();

            // Act
            var blastSchedule = new BlastSchedule();

            // Assert
            blastSchedule.BlastScheduleID.ShouldBeNull();
            blastSchedule.SchedTime.ShouldBe(schedTime);
            blastSchedule.SchedStartDate.ShouldBe(schedStartDate);
            blastSchedule.SchedEndDate.ShouldBe(schedEndDate);
            blastSchedule.Period.ShouldBe(period);
            blastSchedule.CreatedBy.ShouldBeNull();
            blastSchedule.UpdatedBy.ShouldBeNull();
            blastSchedule.UpdatedDate.ShouldBeNull();
            blastSchedule.DaysList.ShouldBeNull();
            blastSchedule.SplitType.ShouldBe(splitType);
            blastSchedule.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #endregion

        #endregion
    }
}