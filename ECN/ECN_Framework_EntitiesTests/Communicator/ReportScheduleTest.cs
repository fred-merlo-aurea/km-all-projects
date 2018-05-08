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
    public class ReportScheduleTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ReportSchedule) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var reportScheduleId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var reportId = Fixture.Create<int?>();
            var startTime = Fixture.Create<string>();
            var startDate = Fixture.Create<string>();
            var endDate = Fixture.Create<string>();
            var scheduleType = Fixture.Create<string>();
            var runSunday = Fixture.Create<bool?>();
            var runMonday = Fixture.Create<bool?>();
            var runTuesday = Fixture.Create<bool?>();
            var runWednesday = Fixture.Create<bool?>();
            var runThursday = Fixture.Create<bool?>();
            var runFriday = Fixture.Create<bool?>();
            var runSaturday = Fixture.Create<bool?>();
            var monthScheduleDay = Fixture.Create<int?>();
            var monthLastDay = Fixture.Create<bool?>();
            var fromEmail = Fixture.Create<string>();
            var fromName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var recurrenceType = Fixture.Create<string>();
            var toEmail = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var reportParameters = Fixture.Create<string>();
            var report = Fixture.Create<ECN_Framework_Entities.Communicator.Reports>();
            var blastId = Fixture.Create<int?>();
            var exportFormat = Fixture.Create<string>();

            // Act
            reportSchedule.ReportScheduleID = reportScheduleId;
            reportSchedule.CustomerID = customerId;
            reportSchedule.ReportID = reportId;
            reportSchedule.StartTime = startTime;
            reportSchedule.StartDate = startDate;
            reportSchedule.EndDate = endDate;
            reportSchedule.ScheduleType = scheduleType;
            reportSchedule.RunSunday = runSunday;
            reportSchedule.RunMonday = runMonday;
            reportSchedule.RunTuesday = runTuesday;
            reportSchedule.RunWednesday = runWednesday;
            reportSchedule.RunThursday = runThursday;
            reportSchedule.RunFriday = runFriday;
            reportSchedule.RunSaturday = runSaturday;
            reportSchedule.MonthScheduleDay = monthScheduleDay;
            reportSchedule.MonthLastDay = monthLastDay;
            reportSchedule.FromEmail = fromEmail;
            reportSchedule.FromName = fromName;
            reportSchedule.EmailSubject = emailSubject;
            reportSchedule.RecurrenceType = recurrenceType;
            reportSchedule.ToEmail = toEmail;
            reportSchedule.CreatedUserID = createdUserId;
            reportSchedule.CreatedDate = createdDate;
            reportSchedule.UpdatedUserID = updatedUserId;
            reportSchedule.UpdatedDate = updatedDate;
            reportSchedule.IsDeleted = isDeleted;
            reportSchedule.ReportParameters = reportParameters;
            reportSchedule.Report = report;
            reportSchedule.BlastID = blastId;
            reportSchedule.ExportFormat = exportFormat;

            // Assert
            reportSchedule.ReportScheduleID.ShouldBe(reportScheduleId);
            reportSchedule.CustomerID.ShouldBe(customerId);
            reportSchedule.ReportID.ShouldBe(reportId);
            reportSchedule.StartTime.ShouldBe(startTime);
            reportSchedule.StartDate.ShouldBe(startDate);
            reportSchedule.EndDate.ShouldBe(endDate);
            reportSchedule.ScheduleType.ShouldBe(scheduleType);
            reportSchedule.RunSunday.ShouldBe(runSunday);
            reportSchedule.RunMonday.ShouldBe(runMonday);
            reportSchedule.RunTuesday.ShouldBe(runTuesday);
            reportSchedule.RunWednesday.ShouldBe(runWednesday);
            reportSchedule.RunThursday.ShouldBe(runThursday);
            reportSchedule.RunFriday.ShouldBe(runFriday);
            reportSchedule.RunSaturday.ShouldBe(runSaturday);
            reportSchedule.MonthScheduleDay.ShouldBe(monthScheduleDay);
            reportSchedule.MonthLastDay.ShouldBe(monthLastDay);
            reportSchedule.FromEmail.ShouldBe(fromEmail);
            reportSchedule.FromName.ShouldBe(fromName);
            reportSchedule.EmailSubject.ShouldBe(emailSubject);
            reportSchedule.RecurrenceType.ShouldBe(recurrenceType);
            reportSchedule.ToEmail.ShouldBe(toEmail);
            reportSchedule.CreatedUserID.ShouldBe(createdUserId);
            reportSchedule.CreatedDate.ShouldBe(createdDate);
            reportSchedule.UpdatedUserID.ShouldBe(updatedUserId);
            reportSchedule.UpdatedDate.ShouldBe(updatedDate);
            reportSchedule.IsDeleted.ShouldBe(isDeleted);
            reportSchedule.ReportParameters.ShouldBe(reportParameters);
            reportSchedule.Report.ShouldBe(report);
            reportSchedule.BlastID.ShouldBe(blastId);
            reportSchedule.ExportFormat.ShouldBe(exportFormat);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (BlastID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_BlastID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.BlastID = random;

            // Assert
            reportSchedule.BlastID.ShouldBe(random);
            reportSchedule.BlastID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_BlastID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.BlastID = null;

            // Assert
            reportSchedule.BlastID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_BlastID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastID = "BlastID";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameBlastID);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.BlastID.ShouldBeNull();
            reportSchedule.BlastID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(reportSchedule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.CreatedUserID = random;

            // Assert
            reportSchedule.CreatedUserID.ShouldBe(random);
            reportSchedule.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.CreatedUserID = null;

            // Assert
            reportSchedule.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.CreatedUserID.ShouldBeNull();
            reportSchedule.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.CustomerID = random;

            // Assert
            reportSchedule.CustomerID.ShouldBe(random);
            reportSchedule.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.CustomerID = null;

            // Assert
            reportSchedule.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.CustomerID.ShouldBeNull();
            reportSchedule.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.EmailSubject = Fixture.Create<string>();
            var stringType = reportSchedule.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (EndDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_EndDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.EndDate = Fixture.Create<string>();
            var stringType = reportSchedule.EndDate.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (EndDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_EndDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDateNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameEndDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_EndDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDate";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ExportFormat) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ExportFormat_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.ExportFormat = Fixture.Create<string>();
            var stringType = reportSchedule.ExportFormat.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (ExportFormat) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ExportFormatNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameExportFormat = "ExportFormatNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameExportFormat));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ExportFormat_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameExportFormat = "ExportFormat";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameExportFormat);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (FromEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_FromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.FromEmail = Fixture.Create<string>();
            var stringType = reportSchedule.FromEmail.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (FromEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_FromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmailNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_FromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmail";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (FromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.FromName = Fixture.Create<string>();
            var stringType = reportSchedule.FromName.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (FromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.IsDeleted = random;

            // Assert
            reportSchedule.IsDeleted.ShouldBe(random);
            reportSchedule.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.IsDeleted = null;

            // Assert
            reportSchedule.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.IsDeleted.ShouldBeNull();
            reportSchedule.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (MonthLastDay) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthLastDay_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.MonthLastDay = random;

            // Assert
            reportSchedule.MonthLastDay.ShouldBe(random);
            reportSchedule.MonthLastDay.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthLastDay_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.MonthLastDay = null;

            // Assert
            reportSchedule.MonthLastDay.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthLastDay_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMonthLastDay = "MonthLastDay";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameMonthLastDay);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.MonthLastDay.ShouldBeNull();
            reportSchedule.MonthLastDay.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (MonthLastDay) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_MonthLastDayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMonthLastDay = "MonthLastDayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameMonthLastDay));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthLastDay_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMonthLastDay = "MonthLastDay";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameMonthLastDay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (MonthScheduleDay) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthScheduleDay_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.MonthScheduleDay = random;

            // Assert
            reportSchedule.MonthScheduleDay.ShouldBe(random);
            reportSchedule.MonthScheduleDay.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthScheduleDay_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.MonthScheduleDay = null;

            // Assert
            reportSchedule.MonthScheduleDay.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthScheduleDay_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMonthScheduleDay = "MonthScheduleDay";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameMonthScheduleDay);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.MonthScheduleDay.ShouldBeNull();
            reportSchedule.MonthScheduleDay.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (MonthScheduleDay) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_MonthScheduleDayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMonthScheduleDay = "MonthScheduleDayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameMonthScheduleDay));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_MonthScheduleDay_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMonthScheduleDay = "MonthScheduleDay";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameMonthScheduleDay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RecurrenceType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RecurrenceType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.RecurrenceType = Fixture.Create<string>();
            var stringType = reportSchedule.RecurrenceType.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (RecurrenceType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RecurrenceTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRecurrenceType = "RecurrenceTypeNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRecurrenceType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RecurrenceType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRecurrenceType = "RecurrenceType";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRecurrenceType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (Report) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Report_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameReport = "Report";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameReport);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(reportSchedule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (Report) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ReportNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReport = "ReportNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameReport));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Report_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReport = "Report";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameReport);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.ReportID = random;

            // Assert
            reportSchedule.ReportID.ShouldBe(random);
            reportSchedule.ReportID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.ReportID = null;

            // Assert
            reportSchedule.ReportID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameReportID = "ReportID";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameReportID);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.ReportID.ShouldBeNull();
            reportSchedule.ReportID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ReportIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameReportID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameReportID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportParameters) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportParameters_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.ReportParameters = Fixture.Create<string>();
            var stringType = reportSchedule.ReportParameters.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportParameters) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ReportParametersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportParameters = "ReportParametersNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameReportParameters));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportParameters_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportParameters = "ReportParameters";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameReportParameters);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportScheduleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportScheduleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.ReportScheduleID = Fixture.Create<int>();
            var intType = reportSchedule.ReportScheduleID.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (ReportScheduleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ReportScheduleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportScheduleID = "ReportScheduleIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameReportScheduleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ReportScheduleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportScheduleID = "ReportScheduleID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameReportScheduleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunFriday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunFriday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunFriday = random;

            // Assert
            reportSchedule.RunFriday.ShouldBe(random);
            reportSchedule.RunFriday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunFriday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunFriday = null;

            // Assert
            reportSchedule.RunFriday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunFriday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunFriday = "RunFriday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunFriday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunFriday.ShouldBeNull();
            reportSchedule.RunFriday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunFriday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunFridayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunFriday = "RunFridayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunFriday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunFriday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunFriday = "RunFriday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunFriday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunMonday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunMonday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunMonday = random;

            // Assert
            reportSchedule.RunMonday.ShouldBe(random);
            reportSchedule.RunMonday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunMonday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunMonday = null;

            // Assert
            reportSchedule.RunMonday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunMonday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunMonday = "RunMonday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunMonday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunMonday.ShouldBeNull();
            reportSchedule.RunMonday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunMonday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunMondayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunMonday = "RunMondayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunMonday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunMonday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunMonday = "RunMonday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunMonday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunSaturday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSaturday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunSaturday = random;

            // Assert
            reportSchedule.RunSaturday.ShouldBe(random);
            reportSchedule.RunSaturday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSaturday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunSaturday = null;

            // Assert
            reportSchedule.RunSaturday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSaturday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunSaturday = "RunSaturday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunSaturday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunSaturday.ShouldBeNull();
            reportSchedule.RunSaturday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunSaturday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunSaturdayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunSaturday = "RunSaturdayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunSaturday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSaturday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunSaturday = "RunSaturday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunSaturday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunSunday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSunday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunSunday = random;

            // Assert
            reportSchedule.RunSunday.ShouldBe(random);
            reportSchedule.RunSunday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSunday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunSunday = null;

            // Assert
            reportSchedule.RunSunday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSunday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunSunday = "RunSunday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunSunday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunSunday.ShouldBeNull();
            reportSchedule.RunSunday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunSunday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunSundayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunSunday = "RunSundayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunSunday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunSunday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunSunday = "RunSunday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunSunday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunThursday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunThursday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunThursday = random;

            // Assert
            reportSchedule.RunThursday.ShouldBe(random);
            reportSchedule.RunThursday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunThursday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunThursday = null;

            // Assert
            reportSchedule.RunThursday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunThursday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunThursday = "RunThursday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunThursday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunThursday.ShouldBeNull();
            reportSchedule.RunThursday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunThursday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunThursdayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunThursday = "RunThursdayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunThursday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunThursday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunThursday = "RunThursday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunThursday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunTuesday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunTuesday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunTuesday = random;

            // Assert
            reportSchedule.RunTuesday.ShouldBe(random);
            reportSchedule.RunTuesday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunTuesday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunTuesday = null;

            // Assert
            reportSchedule.RunTuesday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunTuesday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunTuesday = "RunTuesday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunTuesday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunTuesday.ShouldBeNull();
            reportSchedule.RunTuesday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunTuesday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunTuesdayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunTuesday = "RunTuesdayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunTuesday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunTuesday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunTuesday = "RunTuesday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunTuesday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunWednesday) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunWednesday_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<bool>();

            // Act , Set
            reportSchedule.RunWednesday = random;

            // Assert
            reportSchedule.RunWednesday.ShouldBe(random);
            reportSchedule.RunWednesday.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunWednesday_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.RunWednesday = null;

            // Assert
            reportSchedule.RunWednesday.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunWednesday_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRunWednesday = "RunWednesday";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameRunWednesday);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.RunWednesday.ShouldBeNull();
            reportSchedule.RunWednesday.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (RunWednesday) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_RunWednesdayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRunWednesday = "RunWednesdayNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameRunWednesday));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_RunWednesday_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRunWednesday = "RunWednesday";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameRunWednesday);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ScheduleType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ScheduleType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.ScheduleType = Fixture.Create<string>();
            var stringType = reportSchedule.ScheduleType.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (ScheduleType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ScheduleTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameScheduleType = "ScheduleTypeNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameScheduleType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ScheduleType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameScheduleType = "ScheduleType";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameScheduleType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (StartDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_StartDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.StartDate = Fixture.Create<string>();
            var stringType = reportSchedule.StartDate.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (StartDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_StartDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDateNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameStartDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_StartDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (StartTime) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_StartTime_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.StartTime = Fixture.Create<string>();
            var stringType = reportSchedule.StartTime.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (StartTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_StartTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTimeNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameStartTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_StartTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameStartTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (ToEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ToEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            reportSchedule.ToEmail = Fixture.Create<string>();
            var stringType = reportSchedule.ToEmail.GetType();

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

        #region General Getters/Setters : Class (ReportSchedule) => Property (ToEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_ToEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameToEmail = "ToEmailNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameToEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_ToEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameToEmail = "ToEmail";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameToEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(reportSchedule, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var random = Fixture.Create<int>();

            // Act , Set
            reportSchedule.UpdatedUserID = random;

            // Assert
            reportSchedule.UpdatedUserID.ShouldBe(random);
            reportSchedule.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var reportSchedule = Fixture.Create<ReportSchedule>();

            // Act , Set
            reportSchedule.UpdatedUserID = null;

            // Assert
            reportSchedule.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var reportSchedule = Fixture.Create<ReportSchedule>();
            var propertyInfo = reportSchedule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(reportSchedule, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            reportSchedule.UpdatedUserID.ShouldBeNull();
            reportSchedule.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ReportSchedule) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var reportSchedule  = Fixture.Create<ReportSchedule>();

            // Act , Assert
            Should.NotThrow(() => reportSchedule.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportSchedule_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var reportSchedule  = Fixture.Create<ReportSchedule>();
            var propertyInfo  = reportSchedule.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ReportSchedule) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportSchedule_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ReportSchedule());
        }

        #endregion

        #region General Constructor : Class (ReportSchedule) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportSchedule_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfReportSchedule = Fixture.CreateMany<ReportSchedule>(2).ToList();
            var firstReportSchedule = instancesOfReportSchedule.FirstOrDefault();
            var lastReportSchedule = instancesOfReportSchedule.Last();

            // Act, Assert
            firstReportSchedule.ShouldNotBeNull();
            lastReportSchedule.ShouldNotBeNull();
            firstReportSchedule.ShouldNotBeSameAs(lastReportSchedule);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportSchedule_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstReportSchedule = new ReportSchedule();
            var secondReportSchedule = new ReportSchedule();
            var thirdReportSchedule = new ReportSchedule();
            var fourthReportSchedule = new ReportSchedule();
            var fifthReportSchedule = new ReportSchedule();
            var sixthReportSchedule = new ReportSchedule();

            // Act, Assert
            firstReportSchedule.ShouldNotBeNull();
            secondReportSchedule.ShouldNotBeNull();
            thirdReportSchedule.ShouldNotBeNull();
            fourthReportSchedule.ShouldNotBeNull();
            fifthReportSchedule.ShouldNotBeNull();
            sixthReportSchedule.ShouldNotBeNull();
            firstReportSchedule.ShouldNotBeSameAs(secondReportSchedule);
            thirdReportSchedule.ShouldNotBeSameAs(firstReportSchedule);
            fourthReportSchedule.ShouldNotBeSameAs(firstReportSchedule);
            fifthReportSchedule.ShouldNotBeSameAs(firstReportSchedule);
            sixthReportSchedule.ShouldNotBeSameAs(firstReportSchedule);
            sixthReportSchedule.ShouldNotBeSameAs(fourthReportSchedule);
        }

        #endregion

        #region General Constructor : Class (ReportSchedule) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportSchedule_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var reportScheduleId = -1;
            var startTime = string.Empty;
            var startDate = string.Empty;
            var endDate = string.Empty;
            var scheduleType = string.Empty;
            var fromEmail = string.Empty;
            var fromName = string.Empty;
            var emailSubject = string.Empty;
            var toEmail = string.Empty;
            var recurrenceType = string.Empty;
            var reportParameters = string.Empty;
            var exportFormat = string.Empty;

            // Act
            var reportSchedule = new ReportSchedule();

            // Assert
            reportSchedule.ReportScheduleID.ShouldBe(reportScheduleId);
            reportSchedule.CustomerID.ShouldBeNull();
            reportSchedule.ReportID.ShouldBeNull();
            reportSchedule.StartTime.ShouldBe(startTime);
            reportSchedule.StartDate.ShouldBe(startDate);
            reportSchedule.EndDate.ShouldBe(endDate);
            reportSchedule.ScheduleType.ShouldBe(scheduleType);
            reportSchedule.RunSunday.ShouldBeNull();
            reportSchedule.RunMonday.ShouldBeNull();
            reportSchedule.RunTuesday.ShouldBeNull();
            reportSchedule.RunWednesday.ShouldBeNull();
            reportSchedule.RunThursday.ShouldBeNull();
            reportSchedule.RunFriday.ShouldBeNull();
            reportSchedule.RunSaturday.ShouldBeNull();
            reportSchedule.MonthScheduleDay.ShouldBeNull();
            reportSchedule.MonthLastDay.ShouldBeNull();
            reportSchedule.FromEmail.ShouldBe(fromEmail);
            reportSchedule.FromName.ShouldBe(fromName);
            reportSchedule.EmailSubject.ShouldBe(emailSubject);
            reportSchedule.ToEmail.ShouldBe(toEmail);
            reportSchedule.CreatedUserID.ShouldBeNull();
            reportSchedule.CreatedDate.ShouldBeNull();
            reportSchedule.UpdatedUserID.ShouldBeNull();
            reportSchedule.UpdatedDate.ShouldBeNull();
            reportSchedule.IsDeleted.ShouldBeNull();
            reportSchedule.RecurrenceType.ShouldBe(recurrenceType);
            reportSchedule.Report.ShouldBeNull();
            reportSchedule.ReportParameters.ShouldBe(reportParameters);
            reportSchedule.BlastID.ShouldBeNull();
            reportSchedule.ExportFormat.ShouldBe(exportFormat);
        }

        #endregion

        #endregion

        #endregion
    }
}