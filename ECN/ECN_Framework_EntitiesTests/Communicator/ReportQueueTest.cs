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
    public class ReportQueueTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ReportQueue) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            var reportQueueId = Fixture.Create<int>();
            var sendTime = Fixture.Create<DateTime?>();
            var reportId = Fixture.Create<int>();
            var reportScheduleId = Fixture.Create<int>();
            var status = Fixture.Create<string>();
            var failureReason = Fixture.Create<string>();
            var finishTime = Fixture.Create<DateTime?>();

            // Act
            reportQueue.ReportQueueID = reportQueueId;
            reportQueue.SendTime = sendTime;
            reportQueue.ReportID = reportId;
            reportQueue.ReportScheduleID = reportScheduleId;
            reportQueue.Status = status;
            reportQueue.FailureReason = failureReason;
            reportQueue.FinishTime = finishTime;

            // Assert
            reportQueue.ReportQueueID.ShouldBe(reportQueueId);
            reportQueue.SendTime.ShouldBe(sendTime);
            reportQueue.ReportID.ShouldBe(reportId);
            reportQueue.ReportScheduleID.ShouldBe(reportScheduleId);
            reportQueue.Status.ShouldBe(status);
            reportQueue.FailureReason.ShouldBe(failureReason);
            reportQueue.FinishTime.ShouldBe(finishTime);
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (FailureReason) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_FailureReason_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            reportQueue.FailureReason = Fixture.Create<string>();
            var stringType = reportQueue.FailureReason.GetType();

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

        #region General Getters/Setters : Class (ReportQueue) => Property (FailureReason) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_FailureReasonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFailureReason = "FailureReasonNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameFailureReason));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_FailureReason_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFailureReason = "FailureReason";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameFailureReason);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (FinishTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_FinishTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var reportQueue = Fixture.Create<ReportQueue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = reportQueue.GetType().GetProperty(propertyNameFinishTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(reportQueue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (FinishTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_FinishTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTimeNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameFinishTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_FinishTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameFinishTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            reportQueue.ReportID = Fixture.Create<int>();
            var intType = reportQueue.ReportID.GetType();

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

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_ReportIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportIDNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameReportID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportID = "ReportID";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameReportID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportQueueID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportQueueID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            reportQueue.ReportQueueID = Fixture.Create<int>();
            var intType = reportQueue.ReportQueueID.GetType();

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

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportQueueID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_ReportQueueIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportQueueID = "ReportQueueIDNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameReportQueueID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportQueueID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportQueueID = "ReportQueueID";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameReportQueueID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportScheduleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportScheduleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            reportQueue.ReportScheduleID = Fixture.Create<int>();
            var intType = reportQueue.ReportScheduleID.GetType();

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

        #region General Getters/Setters : Class (ReportQueue) => Property (ReportScheduleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_ReportScheduleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReportScheduleID = "ReportScheduleIDNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameReportScheduleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_ReportScheduleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReportScheduleID = "ReportScheduleID";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameReportScheduleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var reportQueue = Fixture.Create<ReportQueue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = reportQueue.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(reportQueue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ReportQueue) => Property (Status) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Status_Property_String_Type_Verify_Test()
        {
            // Arrange
            var reportQueue = Fixture.Create<ReportQueue>();
            reportQueue.Status = Fixture.Create<string>();
            var stringType = reportQueue.Status.GetType();

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

        #region General Getters/Setters : Class (ReportQueue) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var reportQueue  = Fixture.Create<ReportQueue>();

            // Act , Assert
            Should.NotThrow(() => reportQueue.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ReportQueue_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var reportQueue  = Fixture.Create<ReportQueue>();
            var propertyInfo  = reportQueue.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ReportQueue) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportQueue_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ReportQueue());
        }

        #endregion

        #region General Constructor : Class (ReportQueue) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportQueue_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfReportQueue = Fixture.CreateMany<ReportQueue>(2).ToList();
            var firstReportQueue = instancesOfReportQueue.FirstOrDefault();
            var lastReportQueue = instancesOfReportQueue.Last();

            // Act, Assert
            firstReportQueue.ShouldNotBeNull();
            lastReportQueue.ShouldNotBeNull();
            firstReportQueue.ShouldNotBeSameAs(lastReportQueue);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportQueue_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstReportQueue = new ReportQueue();
            var secondReportQueue = new ReportQueue();
            var thirdReportQueue = new ReportQueue();
            var fourthReportQueue = new ReportQueue();
            var fifthReportQueue = new ReportQueue();
            var sixthReportQueue = new ReportQueue();

            // Act, Assert
            firstReportQueue.ShouldNotBeNull();
            secondReportQueue.ShouldNotBeNull();
            thirdReportQueue.ShouldNotBeNull();
            fourthReportQueue.ShouldNotBeNull();
            fifthReportQueue.ShouldNotBeNull();
            sixthReportQueue.ShouldNotBeNull();
            firstReportQueue.ShouldNotBeSameAs(secondReportQueue);
            thirdReportQueue.ShouldNotBeSameAs(firstReportQueue);
            fourthReportQueue.ShouldNotBeSameAs(firstReportQueue);
            fifthReportQueue.ShouldNotBeSameAs(firstReportQueue);
            sixthReportQueue.ShouldNotBeSameAs(firstReportQueue);
            sixthReportQueue.ShouldNotBeSameAs(fourthReportQueue);
        }

        #endregion

        #region General Constructor : Class (ReportQueue) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ReportQueue_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var reportQueueId = -1;
            var reportId = -1;
            var status = "";
            var failureReason = "";

            // Act
            var reportQueue = new ReportQueue();

            // Assert
            reportQueue.ReportQueueID.ShouldBe(reportQueueId);
            reportQueue.SendTime.ShouldBeNull();
            reportQueue.ReportID.ShouldBe(reportId);
            reportQueue.Status.ShouldBe(status);
            reportQueue.FailureReason.ShouldBe(failureReason);
            reportQueue.FinishTime.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}