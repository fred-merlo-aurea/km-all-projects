using System;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class BlastSchedulerTest
    {
        [Test]
        public void CreateRecurring_WhenRecurrenceTypeDailyAndRequestBlastIdExists_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData1();
            var blastSetupInfo = new BlastSetupInfo();
            var blastType = string.Empty;
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(DateTime.Now.ToShortDateString()),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("d"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeFalse(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(DateTime.Now.Date));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeDailyAndSingleNumberSendRequestBlastIdExists_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData1();
            var blastSetupInfo = new BlastSetupInfo();
            var blastType = string.Empty;
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };
            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("Number", "Number"));

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(DateTime.Now.ToShortDateString()),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("d"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeTrue(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(DateTime.Now.Date));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeDailyAndRequestBlastIdNotExists_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData2();
            var blastSetupInfo = new BlastSetupInfo();
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(DateTime.Now.ToShortDateString()),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("d"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.ShouldBeNull(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(DateTime.Now.Date));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeDailyAndRequestBlastIdExistsAndAllNumberToSend_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData2();
            _testEntity.Session[SessionRequestBlastIDKey] = 1;
            var blastSetupInfo = new BlastSetupInfo
            {
                BlastType = "ab"
            };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(DateTime.Now.ToShortDateString()),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("d"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.ShouldBeNull(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(DateTime.Now.Date));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeMonthlyRequestBlastIdExistsAndBlastTypeSet_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData3();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            _testEntity.Session[SessionRequestBlastIDKey] = 1;
            var blastSetupInfo = new BlastSetupInfo
            {
                BlastType = "ab"
            };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("m"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeTrue(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(1),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(1),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeMonthlyAndRequestBlastIdExistsAndAllNumberToSend_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData4();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("m"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.ShouldBeNull(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(10),
                () => _savedSchedule.DaysList[0].Total.ShouldBeNull(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(new DateTime(2018, 01, 10).Date));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeMonthlyAndMonthsMoreAndAllNumberToSend_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData4();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };
            Get<TextBox>(_privateTestObject, "txtMonth").Text = "33";

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("m"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.ShouldBeNull(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.
                ShouldBe(Convert.ToInt32(Get<TextBox>(_privateTestObject, "txtMonth").Text)),
                () => _savedSchedule.DaysList[0].Total.ShouldBeNull(),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeMonthlyAndMonthsMoreAndNoNumberToSend_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData4();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };
            Get<TextBox>(_privateTestObject, "txtMonth").Text = "33";
            Get<DropDownList>(_privateTestObject, "ddlNumberToSendType").Items.Clear();

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldBeNullOrWhiteSpace(),
                () => _savedSchedule.Period.ShouldBe("m"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(1),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeFalse(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.
                ShouldBe(Convert.ToInt32(Get<TextBox>(_privateTestObject, "txtMonth").Text)),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(1),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeWeeklyAndEvenlySlit_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData5();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };
            
            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldContain(Get<TextBox>(_privateTestObject, "txtEndDate").Text),
                () => _savedSchedule.Period.ShouldBe("w"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(7),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeFalse(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(0),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(14),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeWeeklyAndEvenlySlitAndBlasTIdExists_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData5();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            _testEntity.Session[SessionRequestBlastIDKey] = 1;
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldContain(Get<TextBox>(_privateTestObject, "txtEndDate").Text),
                () => _savedSchedule.Period.ShouldBe("w"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(7),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeFalse(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(0),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(14),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeWeeklyAndManualSlit_InsertsScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData6();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeFalse();
            _isScheduleInserted.ShouldBeTrue();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldContain(Get<TextBox>(_privateTestObject, "txtEndDate").Text),
                () => _savedSchedule.Period.ShouldBe("w"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(7),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeTrue(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(6),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(7),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

        [Test]
        public void CreateRecurring_WhenRecurrenceTypeWeeklyAndManualSlitAndAllNumberToSendAndBlastIdExists_UpdatesScheduleAndSetUpInfo()
        {
            // Arrange
            CreateTestData6();
            var sendDate = new DateTime(2018, 01, 01).ToShortDateString();
            _testEntity.Session[SessionRequestBlastIDKey] = 1;
            var blastSetupInfo = new BlastSetupInfo { BlastType = "ab" };
            var blastType = "ab";
            var param = new object[]
            {
                blastSetupInfo,
                blastType
            };
            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("ALL", "ALL") { Selected = true });

            // Act
            _privateTestObject.Invoke(CreateRecurringMethodName, param);

            // Assert
            _isScheduleUpdated.ShouldBeTrue();
            _isScheduleInserted.ShouldBeFalse();
            _savedSchedule.ShouldNotBeNull();
            _savedSchedule.ShouldSatisfyAllConditions(
                () => _savedSchedule.SchedStartDate.ShouldContain(sendDate),
                () => _savedSchedule.SchedEndDate.ShouldContain(Get<TextBox>(_privateTestObject, "txtEndDate").Text),
                () => _savedSchedule.Period.ShouldBe("w"),
                () => _savedSchedule.ErrorList.Count.ShouldBe(0),
                () => _savedSchedule.DaysList.Count.ShouldBe(7),
                () => _savedSchedule.DaysList[0].IsAmount.Value.ShouldBeFalse(),
                () => _savedSchedule.DaysList[0].DayToSend.Value.ShouldBe(6),
                () => _savedSchedule.DaysList[0].Total.Value.ShouldBe(7),
                () => _savedSchedule.CreatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID),
                () => _savedSchedule.UpdatedBy.ShouldBe(ECNSession.CurrentSession().CurrentUser.UserID));
            var refSetUpInfo = param[0] as BlastSetupInfo;
            refSetUpInfo.ShouldNotBeNull();
            refSetUpInfo.ShouldSatisfyAllConditions(
                () => refSetUpInfo.BlastFrequency.ShouldBe("RECURRING"),
                () => refSetUpInfo.BlastScheduleID.ShouldBe(1),
                () => refSetUpInfo.BlastType.ShouldBeNullOrWhiteSpace(),
                () => refSetUpInfo.ScheduleType.ShouldBe("Schedule Recurring"),
                () => refSetUpInfo.SendTime.Value.Date.ShouldBe(Convert.ToDateTime(sendDate)));
        }

    }
}
