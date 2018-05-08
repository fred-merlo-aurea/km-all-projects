using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.blasts.Fakes;
using ecn.communicator.main.blasts;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Application;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using ECNEntitiesFakes = ECN_Framework_Entities.Communicator.Fakes;
using PageHelper = ECN.Tests.Helpers.PageHelper;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    ///     Unit Tests for <see cref="ecn.communicator.main.blasts.BlastScheduler"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public partial class BlastSchedulerTest : PageHelper
    {
        private const string MaxSendPercent = "50";
        private const string day1 = "day1";
        private const string day2 = "day2";
        private const string day3 = "day3";
        private const string day4 = "day4";
        private const string day5 = "day5";
        private const string day6 = "day6";
        private const string day7 = "day7";
        private const string txtDaySampleValue = "5";
        private const string CreateRecurringMethodName = "CreateRecurring";
        private const string SessionRequestBlastIDKey = "RequestBlastID";
        private const string CreateOneTime = "CreateOneTime";
        private string _setupType = string.Empty;
        private BlastScheduler _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _shimsObject;
        private bool _isScheduleInserted;
        private bool _isScheduleUpdated;
        private BlastSchedule _savedSchedule;
        private IDisposable _context;
        private TextBox _txtStartDate;
        private TextBox _txtEndDate;
        private TextBox _txtStartTime;
        private TextBox _txtNumberToSend;
        private TextBox _txtMonth;
        private TextBox _txtWeeks;
        private TextBox _txtDay1;
        private TextBox _txtDay2;
        private TextBox _txtDay3;
        private TextBox _txtDay4;
        private TextBox _txtDay5;
        private TextBox _txtDay6;
        private TextBox _txtDay7;
        private DropDownList _ddlRecurrence;
        private DropDownList _ddlNumberToSendType;
        private DropDownList _ddlSplitType;
        private DropDownList _ddlScheduleType;
        private CheckBox _cbxDay1;
        private CheckBox _cbxDay2;
        private CheckBox _cbxDay3;
        private CheckBox _cbxDay4;
        private CheckBox _cbxDay5;
        private CheckBox _cbxDay6;
        private CheckBox _cbxDay7;
        private RequiredFieldValidator _rfvMonth;
        private RequiredFieldValidator _rfvWeeks;
        private RequiredFieldValidator _rfvDay1;
        private RequiredFieldValidator _rfvDay2;
        private RequiredFieldValidator _rfvDay3;
        private RequiredFieldValidator _rfvDay4;
        private RequiredFieldValidator _rfvDay5;
        private RequiredFieldValidator _rfvDay6;
        private RequiredFieldValidator _rfvDay7;
        private RequiredFieldValidator _rfvNumberToSend;
        private RangeValidator _rvMonth;
        private RangeValidator _rvWeeks;
        private RangeValidator _rvDay1;
        private RangeValidator _rvDay2;
        private RangeValidator _rvDay3;
        private RangeValidator _rvDay4;
        private RangeValidator _rvDay5;
        private RangeValidator _rvDay6;
        private RangeValidator _rvDay7;
        private Panel _pnlTestBlast;
        private Panel _pnlScheduleType;
        private Panel _pnlRecurrence;
        private Panel _pnlSplitType;
        private Panel _pnlStart;
        private Panel _pnlEnd;
        private Panel _pnlNumberToSend;
        private Panel _pnlNumberToSendType;
        private Panel _pnlDays;
        private Panel _pnlWeeks;
        private Panel _pnlMonth;
        private Panel _pnlEmailPreview;
        private Panel _pnlErrorMessage;
        private Panel _pnlTextBlast;
        private RangeValidator _rvNumberToSend;
        private BlastScheduler _blastScheduler;
        private Type _blastSchedulerType;
        private BlastSetupInfo _setupInfo;
        private BlastSchedule _resultingSchedule;
        private BlastSetupInfo _resultingSetup;
        private int _requestBlastID;
        private string _blastType;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _testEntity = new BlastScheduler();
            _privateTestObject = new PrivateObject(_testEntity);
            _shimsObject = ShimsContext.Create();
            InitializeAllControls(_testEntity);
            _isScheduleInserted = false;
            _isScheduleUpdated = false;
            _savedSchedule = null;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void SetupSendNow_WhenTestBlast_TestBlastPanelIsVisible()
        {
            // Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);
            var isTestBlast = true;
            var methodArgs = new object[] { isTestBlast };

            // Act
            CallMethod(
                _blastSchedulerType,
                "SetupSendNow",
                methodArgs,
                _blastScheduler);

            // Assert
            var testPanelShown = (GetField(_blastScheduler, "pnlTestBlast") as Panel).Visible;
            var textBlastShown = (GetField(_blastScheduler, "pnlTextBlast") as Panel).Visible;
            _blastScheduler.ShouldSatisfyAllConditions(
                () => testPanelShown.ShouldBeTrue(),
                () => textBlastShown.ShouldBeTrue());
        }

        [Test]
        public void SetupSendNow_WhenNotTestBlast_TestBlastPanelIsVisible()
        {
            // Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);
            var isTestBlast = false;
            var methodArgs = new object[] { isTestBlast };
            var numberToSendType = "Number";
            var recurringItem = new ListItem("Schedule Recurring");
            // Act
            CallMethod(
                _blastSchedulerType,
                "SetupSendNow",
                methodArgs,
                _blastScheduler);

            // Assert
            var numberType = (GetField(_blastScheduler, "ddlNumberToSendType") as DropDownList).SelectedValue;
            var scheduleTypeList = (GetField(_blastScheduler, "ddlScheduleType") as DropDownList).Items;
            _blastScheduler.ShouldSatisfyAllConditions(
                () => numberType.ShouldBe(numberToSendType),
                () => scheduleTypeList.Contains(recurringItem).ShouldBeTrue());
        }

        [Test]
        public void SetupSendNow_WhenCanSendAllIsTrue_SendTypeIsSetToAll()
        {
            // Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);
            var isTestBlast = false;
            var methodArgs = new object[] { isTestBlast };
            var numberToSendType = "ALL";
            _blastScheduler.CanSendAll = true;

            // Act
            CallMethod(
                _blastSchedulerType,
                "SetupSendNow",
                methodArgs,
                _blastScheduler);

            // Assert
            var numberType = (GetField(_blastScheduler, "ddlNumberToSendType") as DropDownList).SelectedValue;
            var numberTextBoxEnabled = (GetField(_blastScheduler, "txtNumberToSend") as TextBox).Enabled;
            _blastScheduler.ShouldSatisfyAllConditions(
                () => numberType.ShouldBe(numberToSendType),
                () => numberTextBoxEnabled.ShouldBeFalse());
        }

        [Test]
        public void SetupSendNow_WhenCanScheduleRecurringBlastIsFalse_ScheduleListDoenNotContainRecurringItem()
        {
            // Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);
            var isTestBlast = false;
            var methodArgs = new object[] { isTestBlast };
            var recurringItem = new ListItem("Schedule Recurring");
            _blastScheduler.CanScheduleRecurringBlast = false;
            _ddlScheduleType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Schedule Recurring",
                Text = "Schedule Recurring"
            });

            // Act
            CallMethod(
                _blastSchedulerType,
                "SetupSendNow",
                methodArgs,
                _blastScheduler);

            // Assert
            var scheduleTypeList = (GetField(_blastScheduler, "ddlScheduleType") as DropDownList).Items;
            scheduleTypeList.Contains(recurringItem).ShouldBeFalse();
        }

        [TestCase("txtDay1")]
        [TestCase("txtDay2")]
        [TestCase("txtDay3")]
        [TestCase("txtDay4")]
        [TestCase("txtDay5")]
        [TestCase("txtDay6")]
        [TestCase("txtDay7")]
        public void Days_CheckedChanged_WithManuallySplit_SpecificDayIsEnabled(string weekDayTextBox)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Manually Split",
                Enabled = true,
                Value = "Manually Split"
            });
            var methodArgs = new object[] { null, EventArgs.Empty };
            var checkBoxValue = string.Format("cbx{0}", weekDayTextBox.Substring(3));
            SetField(_blastScheduler, weekDayTextBox, new TextBox
            {
                Enabled = false
            });
            SelectSingleWeekDay(checkBoxValue);

            // Act
            CallMethod(
                _blastSchedulerType,
                "Days_CheckedChanged",
                methodArgs,
                _blastScheduler);

            // Assert
            var weekDayEnabled = (GetField(_blastScheduler, weekDayTextBox) as TextBox).Enabled;
            weekDayEnabled.ShouldBeTrue();
        }

        [TestCase(DayOfWeek.Saturday, 100, "txtDay1")]
        [TestCase(DayOfWeek.Sunday, 23, "txtDay2")]
        [TestCase(DayOfWeek.Monday, 26, "txtDay3")]
        [TestCase(DayOfWeek.Thursday, 33, "txtDay4")]
        [TestCase(DayOfWeek.Wednesday, 66, "txtDay5")]
        [TestCase(DayOfWeek.Thursday, 77, "txtDay6")]
        [TestCase(DayOfWeek.Friday, 12, "txtDay7")]
        public void FindDayControls_WithNumberTypeAndGivenAmount_TextBoxForSpecificDayContainsAmount(DayOfWeek weekDay, int amountToSend, string textBox)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Number",
                Enabled = true,
                Value = "Number"
            });
            var methodArgs = new object[] { weekDay, amountToSend, "m" };
            var checkBoxValue = string.Format("cbx{0}", textBox.Substring(3));
            SetField(_blastScheduler, checkBoxValue, new CheckBox()
            {
                Text = weekDay.ToString()
            });

            // Act
            CallMethod(
                _blastSchedulerType,
                "FindDayControls",
                methodArgs,
                _blastScheduler);

            // Assert
            var daysAmount = (GetField(_blastScheduler, textBox) as TextBox).Text;
            daysAmount.ShouldBe(amountToSend.ToString());
        }

        [TestCase("Daily", "other", false)]
        [TestCase("Monthly", "other", false)]
        [TestCase("Weekly", "other", false)]
        public void CreateOneTime_WithNumberToSendTypeIsNumber_DaysListIsAmountIsChanged(string recurrence, string numberToSendType, bool scheduleDaysListIsAmount)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            var methodArgs = new object[] { _setupInfo, "ab" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var isAmount = _resultingSchedule.DaysList.TrueForAll(x => x.IsAmount == scheduleDaysListIsAmount);
            isAmount.ShouldBeTrue();
        }

        [TestCase("Daily", "ALL", false)]
        [TestCase("Monthly", "ALL", false)]
        public void CreateOneTime_WithNumberToSendTypeIsALL_SendNowIsNotAmount(string recurrence, string numberToSendType, bool scheduleDaysListIsAmount)
        {
            // Arrange
            Initialize(CreateOneTime);
            SetSessionVariable("RequestBlastID", 0);
            _setupInfo.BlastType = "ab";
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            var methodArgs = new object[] { _setupInfo, "ab" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var isAmount = _resultingSetup.SendNowIsAmount ?? true;
            isAmount.ShouldBeFalse();
        }

        [TestCase("Daily")]
        [TestCase("Monthly")]
        public void CreateOneTime_WhenScheduleExists_UpdatesSchedule(string recurrence)
        {
            // Arrange
            Initialize(CreateOneTime);
            var scheduleUpdated = false;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            ShimBlastScheduler.AllInstances.RequestBlastIDGet = (x) =>
            {
                return 1;
            };
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (x, y) =>
            {
                scheduleUpdated = true;
                _resultingSchedule = x;
                return 1;
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            scheduleUpdated.ShouldBeTrue();
        }

        [TestCase("Daily")]
        [TestCase("Monthly")]
        public void CreateOneTime_WhenScheduleNotExists_InsertsSchedule(string recurrence)
        {
            // Arrange
            Initialize(CreateOneTime);
            var scheduleInserted = false;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            ShimBlastScheduler.AllInstances.RequestBlastIDGet = (x) =>
            {
                return 0;
            };
            ShimBlastSchedule.InsertBlastSchedule = (x) =>
            {
                scheduleInserted = true;
                _resultingSchedule = x;
                return 1;
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            scheduleInserted.ShouldBeTrue();
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateOneTime_WhenWeeklyAndEvenlySplit_SelectedDaysAreAddedToScheduleDaysList(params string[] selectedDays)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Evenly Split",
                Enabled = true,
                Value = "Evenly Split"
            });
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            var scheduleDaysCount = _resultingSchedule.DaysList.Count;
            scheduleDaysCount.ShouldBe(totalDaysSelected);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateOneTime_WhenWeeklyAndEvenlySplit_SelectedDaysHaveSimilarTotalButLastDayMightHaveGreaterTotal(params string[] selectedDays)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Evenly Split",
                Enabled = true,
                Value = "Evenly Split"
            });
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            BlastScheduleDays schLastDay = _resultingSchedule.DaysList.Last();
            List<BlastScheduleDays> schDaysExceptLast = _resultingSchedule.DaysList.GetRange(0, selectedDays.Length - 1).ToList();
            if (schDaysExceptLast.Count == 0)
            {
                schDaysExceptLast.Add(schLastDay);
            }
            BlastScheduleDays schOtherDay = schDaysExceptLast.First();
            var schLastDayTotal = schLastDay.Total ?? 0;
            var schOtherDaysTotal = schOtherDay.Total ?? 0;
            var otherDaysHaveSimilarTotal = false;
            var withDifferentTotal = schDaysExceptLast.Where(x => x.Total != schOtherDaysTotal).ToList().Count;
            if (withDifferentTotal > 0)
            {
                otherDaysHaveSimilarTotal = false;
            }
            else
            {
                otherDaysHaveSimilarTotal = true;
            }
            otherDaysHaveSimilarTotal.ShouldBeTrue();
            schLastDayTotal.ShouldBeGreaterThanOrEqualTo(schOtherDaysTotal);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateOneTime_WhenWeeklyAndManualySplit_SelectedDaysTotalHaveGivenValues(params string[] selectedDays)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Manually Split",
                Enabled = true,
                Value = "Manually Split"
            });
            var rnd = new Random();
            var day1Value = (rnd.Next(1, 100)).ToString();
            _txtDay1.Text = day1Value;
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            BlastScheduleDays schFirstDay = _resultingSchedule.DaysList.Last();
            var schFirstDayTotal = schFirstDay.Total.ToString();
            schFirstDayTotal.ShouldBe(_txtDay1.Text);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateOneTime_WhenWeeklyAndManualySplitAndNumberToSendTypeIsNotNumber_SelectedDaysIsNotAmount(params string[] selectedDays)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Manually Split",
                Enabled = true,
                Value = "Manually Split"
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "notNumber",
                Enabled = true,
                Value = "notNumber"
            });
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            var schDaysIsAmount = _resultingSchedule.DaysList.Any(x => x.IsAmount == true);
            schDaysIsAmount.ShouldBeFalse();
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateOneTime_WhenSingleDaySplitAndNumberToSendTypeIsNotNumber_SelectedDaysIsNotAmount(params string[] selectedDays)
        {
            // Arrange
            Initialize(CreateOneTime);
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Single Day",
                Enabled = true,
                Value = "Single Day"
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "notNumber",
                Enabled = true,
                Value = "notNumber"
            });
            var methodArgs = new object[] { _setupInfo, "ab" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            var schDaysIsAmount = _resultingSchedule.DaysList.Any(x => x.IsAmount == true);
            schDaysIsAmount.ShouldBeFalse();
        }

        [Test]
        public void CreateRecurring_WhenCalled_BlastScheduleInfoHasSuppliedValues()
        {
            // Arrange
            Initialize();
            var scheduleInsertedOrUpdated = false;
            _txtStartDate.Text = "01/01/2018";
            _txtEndDate.Text = "01/31/2018";
            _txtStartTime.Text = "12:00 AM";
            _txtNumberToSend.Text = "10";
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Daily"
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Number"
            });
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (x, y) =>
            {
                scheduleInsertedOrUpdated = true;
                _resultingSchedule = x;
                return 1;
            };
            ShimBlastSchedule.InsertBlastSchedule = (x) =>
            {
                scheduleInsertedOrUpdated = true;
                _resultingSchedule = x;
                return 1;
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            _resultingSchedule.CreatedBy.ShouldBe(1);
            _resultingSchedule.SchedStartDate.ShouldBe(_txtStartDate.Text);
            _resultingSchedule.SchedEndDate.ShouldBe(_txtEndDate.Text);
            _resultingSchedule.SchedTime.ShouldBe(_txtStartTime.Text);
            _resultingSetup.ShouldNotBeNull();
            _resultingSetup.ScheduleType.ShouldNotBeNullOrWhiteSpace();
            scheduleInsertedOrUpdated.ShouldBeTrue();
        }


        [TestCase("Daily", "Number", true)]
        [TestCase("Daily", "other", false)]
        [TestCase("Monthly", "Number", true)]
        [TestCase("Monthly", "other", false)]
        [TestCase("Weekly", "other", false)]
        public void CreateRecurring_WithNumberToSendTypeIsNumber_DaysListIsAmountIsChanged(string recurrence, string numberToSendType, bool scheduleDaysListIsAmount)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            var methodArgs = new object[] { _setupInfo, "ab" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var isAmount = _resultingSchedule.DaysList.TrueForAll(x => x.IsAmount == scheduleDaysListIsAmount);
            isAmount.ShouldBeTrue();
        }

        [TestCase("Daily", "ALL", false)]
        [TestCase("Monthly", "ALL", false)]
        public void CreateRecurring_WithNumberToSendTypeIsALL_SendNowIsNotAmount(string recurrence, string numberToSendType, bool scheduleDaysListIsAmount)
        {
            // Arrange
            Initialize();
            SetSessionVariable("RequestBlastID", 0);
            _setupInfo.BlastType = "ab";
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            var methodArgs = new object[] { _setupInfo, "ab" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var isAmount = _resultingSetup.SendNowIsAmount ?? true;
            isAmount.ShouldBeFalse();
        }

        [TestCase("Daily", "Number", true)]
        [TestCase("Daily", "other", false)]
        [TestCase("Monthly", "Number", true)]
        [TestCase("Monthly", "other", false)]
        public void CreateRecurring_IfScheduleSaved_SetupInfoIsAmountChanges(string recurrence, string numberToSendType, bool sendNowIsAmount)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });

            ShimBlastSetupInfo.Constructor = x => new BlastSetupInfo()
            {
                BlastType = "AB"
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            _resultingSetup.SendNowIsAmount.ShouldBe(sendNowIsAmount);
        }

        [TestCase("Daily")]
        [TestCase("Monthly")]
        public void CreateRecurring_WhenScheduleExists_UpdatesSchedule(string recurrence)
        {
            // Arrange
            Initialize();
            var scheduleUpdated = false;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            ShimBlastScheduler.AllInstances.RequestBlastIDGet = (x) =>
            {
                return 1;
            };
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (x, y) =>
            {
                scheduleUpdated = true;
                _resultingSchedule = x;
                return 1;
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            scheduleUpdated.ShouldBeTrue();
        }

        [TestCase("Daily")]
        [TestCase("Monthly")]
        public void CreateRecurring_WhenScheduleNotExists_InsertsSchedule(string recurrence)
        {
            // Arrange
            Initialize();
            var scheduleInserted = false;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            ShimBlastScheduler.AllInstances.RequestBlastIDGet = (x) =>
            {
                return 0;
            };
            ShimBlastSchedule.InsertBlastSchedule = (x) =>
            {
                scheduleInserted = true;
                _resultingSchedule = x;
                return 1;
            };
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            scheduleInserted.ShouldBeTrue();
        }

        [TestCase("Monthly", "99")]
        public void CreateRecurring_WhenMonthIsNotGiven_SetupInfoSendTimeEqualsLastDateOfMonth(string recurrence, string month)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = "01/15/2017";
            _txtStartTime.Text = "02:15 AM";
            _txtMonth.Text = month;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var result = Convert.ToDateTime("01/31/2017 02:15 AM");
            _resultingSetup.SendTime.ShouldBe(result);
        }

        [TestCase("Monthly", "08", "06/15/2017", "07/08/2017")]
        [TestCase("Monthly", "09", "07/15/2017", "08/09/2017")]
        [TestCase("Monthly", "10", "10/15/2017", "11/10/2017")]
        public void CreateRecurring_WhenGivenMonthIsSmallerThanStartDateDay_MostProbablyItIsDayOfNextMonth(string recurrence, string month, string startDate, string setupDate)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = startDate;
            _txtStartTime.Text = "02:15 AM";
            _txtMonth.Text = month;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var result = Convert.ToDateTime(setupDate + " 02:15 AM");
            _resultingSetup.SendTime.ShouldBe(result);
        }

        [TestCase("Monthly", "08", "06/06/2017", "06/08/2017")]
        [TestCase("Monthly", "09", "07/07/2017", "07/09/2017")]
        [TestCase("Monthly", "31", "10/02/2017", "10/31/2017")]
        public void CreateRecurring_WhenGivenMonthIsGreaterThanStartDateDay_MostProbablyItIsDayOfCurrentMonthOr(string recurrence, string month, string startDate, string setupDate)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = startDate;
            _txtStartTime.Text = "02:15 AM";
            _txtMonth.Text = month;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrence
            });
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            _resultingSetup = (BlastSetupInfo)methodArgs[0];
            var result = Convert.ToDateTime(setupDate + " 02:15 AM");
            _resultingSetup.SendTime.ShouldBe(result);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateRecurring_WhenWeeklyAndEvenlySplit_SelectedDaysAreAddedToScheduleDaysList(params string[] selectedDays)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Evenly Split",
                Enabled = true,
                Value = "Evenly Split"
            });
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            var scheduleDaysCount = _resultingSchedule.DaysList.Count;
            scheduleDaysCount.ShouldBe(totalDaysSelected);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateRecurring_WhenWeeklyAndEvenlySplit_SelectedDaysHaveSimilarTotalButLastDayMightHaveGreaterTotal(params string[] selectedDays)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Evenly Split",
                Enabled = true,
                Value = "Evenly Split"
            });
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            BlastScheduleDays schLastDay = _resultingSchedule.DaysList.Last();
            List<BlastScheduleDays> schDaysExceptLast = _resultingSchedule.DaysList.GetRange(0, selectedDays.Length - 1).ToList();
            if (schDaysExceptLast.Count == 0)
            {
                schDaysExceptLast.Add(schLastDay);
            }
            BlastScheduleDays schOtherDay = schDaysExceptLast.First();
            var schLastDayTotal = schLastDay.Total ?? 0;
            var schOtherDaysTotal = schOtherDay.Total ?? 0;
            var otherDaysHaveSimilarTotal = false;
            var withDifferentTotal = schDaysExceptLast.Where(x => x.Total != schOtherDaysTotal).ToList().Count;
            if (withDifferentTotal > 0)
            {
                otherDaysHaveSimilarTotal = false;
            }
            else
            {
                otherDaysHaveSimilarTotal = true;
            }
            otherDaysHaveSimilarTotal.ShouldBeTrue();
            schLastDayTotal.ShouldBeGreaterThanOrEqualTo(schOtherDaysTotal);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateRecurring_WhenWeeklyAndManualySplit_SelectedDaysTotalHaveGivenValues(params string[] selectedDays)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Manually Split",
                Enabled = true,
                Value = "Manually Split"
            });
            Random rnd = new Random();
            var day1Value = (rnd.Next(1, 100)).ToString();
            _txtDay1.Text = day1Value;
            var totalDaysSelected = selectedDays.Length;
            var lastDay = selectedDays.Last();
            SetSelectedDays(selectedDays);
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            BlastScheduleDays schFirstDay = _resultingSchedule.DaysList.Last();
            var schFirstDayTotal = schFirstDay.Total.ToString();
            schFirstDayTotal.ShouldBe(_txtDay1.Text);
        }

        [TestCase("day1")]
        [TestCase("day1", "day2")]
        [TestCase("day1", "day2", "day3")]
        [TestCase("day1", "day2", "day3", "day4")]
        [TestCase("day1", "day2", "day3", "day4", "day5")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6")]
        [TestCase("day1", "day2", "day3", "day4", "day5", "day6", "day7")]
        public void CreateRecurring_WhenWeeklyAndManualySplitAndNumberToSendTypeIsNotNumber_SelectedDaysIsNotAmount(params string[] selectedDays)
        {
            // Arrange
            Initialize();
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = "Weekly"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Manually Split",
                Enabled = true,
                Value = "Manually Split"
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "notNumber",
                Enabled = true,
                Value = "notNumber"
            });
            var methodArgs = new object[] { _setupInfo, "blastType" };

            // Act
            CallMethod(
                _blastSchedulerType,
                "CreateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            var schDaysIsAmount = _resultingSchedule.DaysList.Any(x => x.IsAmount == true);
            schDaysIsAmount.ShouldBeFalse();
        }

        [TestCase("Percent", "Send Now", "other")]
        [TestCase("Percent", "Schedule One-Time", "Single Day")]
        [TestCase("Number", "Send Now", "other")]
        [TestCase("Number", "Schedule One-Time", "Single Day")]
        public void ddlNumberToSendType_SelectedIndexChanged_WhenScheduleTypeIsSendNowOrOneTime_TextBoxForNumberInputIsEnabled(string numberToSendType, string scheduleType, string splitType)
        {
            // Arrange
            Initialize();
            DisableAllValidators();
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlScheduleType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = scheduleType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ddlNumberToSendType_SelectedIndexChanged",
                methodArgs,
                _blastScheduler);

            // Assert
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _rfvNumberToSend.Enabled.ShouldBeTrue(),
                () => _rvNumberToSend.Enabled.ShouldBeTrue());
        }

        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day1")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day2")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day3")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day4")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day5")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day6")]
        [TestCase("Percent", "Schedule One-Time", "Manually Split", "day7")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day1")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day2")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day3")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day4")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day5")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day6")]
        [TestCase("Number", "Schedule One-Time", "Manually Split", "day7")]
        public void ddlNumberToSendType_SelectedIndexChanged_WhenSplitManualy_SelectedDaysInputBecomesRequired(string numberToSendType, string scheduleType, string splitType, string selectedDay)
        {
            // Arrange
            Initialize();
            SelectDay(selectedDay);
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlScheduleType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = scheduleType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ddlNumberToSendType_SelectedIndexChanged",
                methodArgs,
                _blastScheduler);

            // Assert
            var isRequiredSelectedDayValue = GetRequiredFieldValidatorForDay(selectedDay);
            isRequiredSelectedDayValue.ShouldBeTrue();
        }

        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day1", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day2", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day3", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day4", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day5", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day6", "Weekly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "day7", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day1", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day2", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day3", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day4", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day5", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day6", "Weekly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "day7", "Weekly")]
        public void ddlNumberToSendType_SelectedIndexChanged_WhenSplitManualyWithWeeklyRecurrence_SelectedDaysInputBecomesRequired(string numberToSendType, string scheduleType, string splitType, string selectedDay, string recurrenceType)
        {
            // Arrange
            Initialize();
            SelectDay(selectedDay);
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlScheduleType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = scheduleType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ddlNumberToSendType_SelectedIndexChanged",
                methodArgs,
                _blastScheduler);

            // Assert
            var isRequiredSelectedDayValue = GetRequiredFieldValidatorForDay(selectedDay);
            isRequiredSelectedDayValue.ShouldBeTrue();
        }

        [TestCase("Percent", "Schedule Recurring", "Manually Split", "Monthly")]
        [TestCase("Percent", "Schedule Recurring", "Manually Split", "Daily")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "Monthly")]
        [TestCase("Number", "Schedule Recurring", "Manually Split", "Daily")]
        public void ddlNumberToSendType_SelectedIndexChanged_WhenSplitManualyWithMonthlyOrDailyRecurrence_SelectedDaysInputBecomesRequired(string numberToSendType, string scheduleType, string splitType, string recurrenceType)
        {
            // Arrange
            Initialize();
            DisableAllValidators();
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlScheduleType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = scheduleType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var methodArgs = new object[] { null, EventArgs.Empty };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ddlNumberToSendType_SelectedIndexChanged",
                methodArgs,
                _blastScheduler);

            // Assert
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _rfvNumberToSend.Enabled.ShouldBeTrue(),
                () => _rvNumberToSend.Enabled.ShouldBeTrue());
        }

        [TestCase("Manually Split", "Number", "past", "Please enter a valid start date and time(current date/time is in the past).")]
        [TestCase("Manually Split", "Number", "invalidDate", "Please enter a valid start date and time.")]
        public void ValidateOneTime_WhenStartDateIsPastOrInvalid_ErrorListContainsErrors(string splitType, string numberToSendType, string dateString, string errorString)
        {
            // Arrange
            Initialize();
            if (dateString == "future")
            {
                _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            }
            if (dateString == "invalidDate")
            {
                _txtStartDate.Text = "invalidDate";
            }
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void ValidateOneTime_WhenNumberToSendIsInvalid_ErrorListContainsError()
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var splitType = "Single Day";
            var numberToSendType = "Percent";
            var errorString = "Please enter a valid number to send.";
            _txtNumberToSend.Text = "invalidNumber";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void ValidateOneTime_WhenNumberToSendIsGreaterTheanMaxSendPercent_ErrorListContainsErrors()
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var splitType = "Single Day";
            var numberToSendType = "Percent";
            var errorString = $"Please make sure all selected days add up to less than or equal to {MaxSendPercent}.";
            _txtNumberToSend.Text = "99";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Evenly Split")]
        [TestCase("Manually Split")]
        public void ValidateOneTime_WhenEvenlyOrManuallySplitAndNoDayIsSelected_ErrorListContainsErrors(string splitType)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            UncheckAllDays();
            var numberToSendType = "Percent";
            var errorString = "Please select at least one day.";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void ValidateOneTime_WhenValidNumbersEnteredAndTotalPercentageIsLessThanMaxSendPercent_ErrorListIsEmpty()
        {
            // Arrange
            Initialize();
            SelectAllDaysWithValidValues();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var splitType = "Manually Split";
            var numberToSendType = "Percent";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Count.ShouldBe(0);
        }

        [Test]
        public void ValidateOneTime_WhenInvalidNumbersEntered_ErrorListContainsError()
        {
            // Arrange
            Initialize();
            SelectAllDaysWithInvalidValues();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var splitType = "Manually Split";
            var numberToSendType = "Percent";
            var errorString = "Please enter a valid number for each day checked.";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void ValidateOneTime_WhenTotalPercentageIsGreaterThanMaxSendPercent_ErrorListContainsError()
        {
            // Arrange
            Initialize();
            SelectAllDaysWithValidValues();
            _txtDay7.Text = "1000";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var splitType = "Manually Split";
            var numberToSendType = "Percent";
            var errorString = "Please make sure all selected days add up to less than or equal to " + MaxSendPercent + ".";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateOneTime",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Manually Split", "Number", "past", "Please enter a valid start date and time(current date/time is in the past).")]
        [TestCase("Manually Split", "Number", "invalidDate", "Please enter a valid start date and time.")]
        public void ValidateRecurring_WhenStartDateIsPastOrInvalid_ErrorListContainsErrors(string splitType, string numberToSendType, string dateString, string errorString)
        {
            // Arrange
            Initialize();
            if (dateString == "future")
            {
                _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            }
            if (dateString == "invalidDate")
            {
                _txtStartDate.Text = "invalidDate";
            }
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Manually Split", "Number", "wrongEndDate", "Please enter an end date that occurs after your start date and time.")]
        [TestCase("Manually Split", "Number", "invalidDate", "Please enter a valid end date and time.")]
        public void ValidateRecurring_WhenEndDateOccoursBeforeStartDate_ErrorListContainsErrors(string splitType, string numberToSendType, string dateString, string errorString)
        {
            // Arrange
            Initialize();
            if (dateString == "wrongEndDate")
            {
                _txtStartDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
                _txtEndDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            }
            if (dateString == "invalidDate")
            {
                _txtEndDate.Text = "invalidDate";
            }
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Daily", "Manually Split", "Percent")]
        [TestCase("Monthly", "Manually Split", "Percent")]
        public void ValidateRecurring_WhenNumberToSendIsInvalid_ErrorListContainsErrors(string recurrenceType, string splitType, string numberToSendType)
        {
            // Arrange
            Initialize();
            _txtNumberToSend.Text = "invalid Number";
            var errorString = "Please enter a valid number to send.";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Daily", "Manually Split", "Percent")]
        [TestCase("Monthly", "Manually Split", "Percent")]
        [TestCase("Weekly", "Manually Split", "Percent")]
        public void ValidateRecurring_WhenNumberToSendIsGreaterThanMaxLimit_ErrorListContainsErrors(string recurrenceType, string splitType, string numberToSendType)
        {
            // Arrange
            Initialize();
            _txtNumberToSend.Text = "1000";
            var errorString = "Please make sure all selected days add up to less than or equal to " + MaxSendPercent + ".";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Monthly", "Manually Split", "Percent")]
        public void ValidateRecurring_WhenMonthIsInvalid_ErrorListContainsErrors(string recurrenceType, string splitType, string numberToSendType)
        {
            // Arrange
            Initialize();
            _txtNumberToSend.Text = "40";
            _txtMonth.Text = "32";
            var errorString = "Please enter a valid day of the month.";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Weekly", "Manually Split", "Percent")]
        public void ValidateRecurring_WhenInvalidNumberOfWeeks_ErrorListContainsErrors(string recurrenceType, string splitType, string numberToSendType)
        {
            // Arrange
            Initialize();
            _txtWeeks.Text = "55";
            var errorString = "Please enter a valid number for how many weeks.";
            _txtNumberToSend.Text = "40";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [TestCase("Weekly", "Manually Split", "Percent")]
        public void ValidateRecurring_WhenNoDayIsSelected_ErrorListContainsErrors(string recurrenceType, string splitType, string numberToSendType)
        {
            // Arrange
            Initialize();
            UncheckAllDays();
            _txtWeeks.Text = "50";
            var errorString = "Please select at least one day.";
            _txtNumberToSend.Text = "40";
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void ValidateRecurring_WhenValidNumbersEnteredAndTotalPercentageIsLessThanMaxSendPercent_ErrorListIsEmpty()
        {
            // Arrange
            Initialize();
            SelectAllDaysWithValidValues();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            var recurrenceType = "Weekly";
            var splitType = "Manually Split";
            var numberToSendType = "Percent";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Count.ShouldBe(0);
        }

        [Test]
        public void ValidateRecurring_WhenInvalidNumbersEntered_ErrorListContainsError()
        {
            // Arrange
            Initialize();
            var recurrenceType = "Weekly";
            SelectAllDaysWithInvalidValues();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            _txtEndDate.Text = DateTime.Now.AddMonths(2).ToString("MM/dd/yyyy");
            var splitType = "Manually Split";
            var numberToSendType = "Percent";
            var errorString = "Please enter a valid number for each day checked.";
            _txtNumberToSend.Text = "15";
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = numberToSendType
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = splitType
            });
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Value = recurrenceType
            });
            var errorList = new List<string>();
            var methodArgs = new object[] { errorList };

            // Act
            CallMethod(
                _blastSchedulerType,
                "ValidateRecurring",
                methodArgs,
                _blastScheduler);

            // Assert
            errorList.Contains(errorString).ShouldBeTrue();
        }

        [Test]
        public void LoadBlastSchedule_WhenScheduleDaysListIsEmpty_SplitTypeIsSetToSingleDay()
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            SetProperty(blastScheduleObject, "Period", "o");
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            _ddlSplitType.SelectedValue.ShouldBe("Single Day");
        }

        [TestCase(true, "Number")]
        [TestCase(false, "Percent")]
        public void LoadBlastSchedule_WhenScheduleDaysListContainsOneItemAndDaysToSendIsNull_SplitTypeIsSetToSingleDayTextNumberToSendIsSetToDaysTotal(bool schObjIsAmount, string ddlNumberToSendType)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "DayToSend", null);
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "Period", "o");
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            var textNumberSetToDaysTotal = (bool)(_txtNumberToSend.Text == blastScheduleObject.DaysList[0].Total.ToString());
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _ddlSplitType.SelectedValue.ShouldBe("Single Day"),
                () => textNumberSetToDaysTotal.ShouldBeTrue(),
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _ddlNumberToSendType.SelectedValue.ShouldBe(ddlNumberToSendType));
        }

        [TestCase(true, "Number", "e", "Evenly Split")]
        [TestCase(false, "Percent", "m", "Manually Split")]
        public void LoadBlastSchedule_WhenScheduleDaysListContainsOneItemAndDaysToSendHasValue_SplitTypeIsSetl(bool schObjIsAmount, string ddlNumberToSendType, string schSplitType, string ddlSplitType)
        {
            // Arrange
            Initialize();
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "Period", "o");
            SetProperty(blastScheduleObject, "SplitType", schSplitType);
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert ddlSplitType
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _ddlSplitType.SelectedValue.ShouldBe(ddlSplitType),
                () => _ddlSplitType.SelectedValue.ShouldBe(ddlSplitType),
                () => _ddlNumberToSendType.SelectedValue.ShouldBe(ddlNumberToSendType));
        }

        [TestCase(true, "Number", "e", "Evenly Split", 1)]
        [TestCase(true, "Number", "e", "Evenly Split", 2)]
        [TestCase(false, "Percent", "m", "Manually Split", 1)]
        [TestCase(false, "Percent", "m", "Manually Split", 2)]
        public void LoadBlastSchedule_WhenScheduleDaysListContainsDays_SplitTypeIsSettxtNumberToSendIsEnabled(bool schObjIsAmount, string ddlNumberToSendType, string schSplitType, string ddlSplitType, int daysToAdd)
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "o");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            for (var i = 0; i < daysToAdd; i++)
            {
                blastScheduleDaysList.Add(blastScheduleDaysObject);
            }
            SetProperty(blastScheduleObject, "SplitType", schSplitType);
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert ddlSplitType
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _ddlSplitType.SelectedValue.ShouldBe(ddlSplitType),
                () => _ddlSplitType.SelectedValue.ShouldBe(ddlSplitType),
                () => _ddlNumberToSendType.SelectedValue.ShouldBe(ddlNumberToSendType));
        }

        [TestCase(true, "Percent", "e", "Evenly Split", 0)]
        [TestCase(true, "Number", "e", "Evenly Split", 1)]
        [TestCase(true, "Number", "e", "Evenly Split", 2)]
        [TestCase(false, "Percent", "m", "Manually Split", 1)]
        [TestCase(false, "Percent", "m", "Manually Split", 2)]
        public void LoadBlastSchedule_WithDailyScheduleBlast_ddlNumberToSendTypeIsSettxtNumberToSendIsEnabled(bool schObjIsAmount, string ddlNumberToSendType, string schSplitType, string ddlSplitType, int daysToAdd)
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "d");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            for (var i = 0; i < daysToAdd; i++)
            {
                blastScheduleDaysList.Add(blastScheduleDaysObject);
            }
            SetProperty(blastScheduleObject, "SplitType", schSplitType);
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();
            // Assert ddlSplitType
            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _txtNumberToSend.Enabled.ShouldBeTrue(),
                () => _ddlNumberToSendType.SelectedValue.ShouldBe(ddlNumberToSendType));
        }

        [TestCase(false, "Percent", "m", "Manually Split")]
        [TestCase(false, "Percent", "e", "Evenly Split")]
        [TestCase(false, "Percent", "me", "Manually Split")]
        [TestCase(false, "Percent", "", "Manually Split")]
        public void LoadBlastSchedule_WithWeeklyScheduleBlast__ddlSplitTypeIsSetAndtxtWeeksIsEnabled(bool schObjIsAmount, string ddlNumberToSendType, string schSplitType, string ddlSplitType)
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "w");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "SplitType", schSplitType);
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();
            // Assert ddlSplitType

            _txtNumberToSend.ShouldSatisfyAllConditions(
                () => _ddlSplitType.SelectedValue.ShouldBe(ddlSplitType),
                () => _txtWeeks.Enabled.ShouldBeTrue());
        }

        [TestCase(true, "Number", "m", "Manually Split")]
        [TestCase(false, "Percent", "m", "Manually Split")]
        public void _LoadBlastSchedule_WithWeeklyScheduleBlast_ddlNumberToSendTypeIsSettxtNumberToSendIsEnabled(bool schObjIsAmount, string ddlNumberToSendType, string schSplitType, string ddlSplitType)
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "w");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)schObjIsAmount);
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "SplitType", schSplitType);
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            _ddlNumberToSendType.SelectedValue.ShouldBe(ddlNumberToSendType);
        }

        [Test]
        public void LoadBlastSchedule_WithMonthlyScheduleBlastWhenTextMonthNotSet_txtMonthIsDisabled()
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "m");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", (bool?)true);
            blastScheduleDaysObject.DayToSend = 99;
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "SplitType", "dummyString");
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            _txtMonth.Enabled.ShouldBeFalse();
        }

        [Test]
        public void LoadBlastSchedule_WithMonthlyScheduleWhenIsAmountIsNull_txtMonthIsDisabled()
        {
            // Arrange
            Initialize();
            object isAmmount = null;
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "m");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", isAmmount);
            blastScheduleDaysObject.DayToSend = 99;
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "SplitType", "dummyString");
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            _ddlNumberToSendType.SelectedValue.ShouldBe("Percent");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void LoadBlastSchedule_WithMonthlyScheduleWhenIsAmountIsNotNull__rfvNumberToSendIsEnabled(bool? isAmmount)
        {
            // Arrange
            Initialize();
            var blastScheduleObject = CreateInstance(typeof(BlastSchedule));
            SetProperty(blastScheduleObject, "Period", "m");
            _txtStartDate.Text = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var blastScheduleDaysObject = CreateInstance(typeof(BlastScheduleDays));
            SetProperty(blastScheduleDaysObject, "IsAmount", isAmmount);
            blastScheduleDaysObject.DayToSend = 99;
            var blastScheduleDaysList = new List<BlastScheduleDays>();
            blastScheduleDaysList.Add(blastScheduleDaysObject);
            SetProperty(blastScheduleObject, "SplitType", "dummyString");
            SetProperty(blastScheduleObject, "DaysList", blastScheduleDaysList);
            SetProperty(blastScheduleObject, "SchedStartDate", DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"));
            ShimBlastSchedule.GetByBlastIDInt32Boolean = (x, y) => blastScheduleObject;

            // Act
            _blastScheduler.LoadBlastSchedule();

            // Assert
            _rfvNumberToSend.Enabled.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_Test()
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);

            _blastScheduler.CanScheduleBlast = true;

            ShimBlastScheduler.AllInstances.LoadBlastSchedule = (instance) => { };

            //Act
            _privateTestObject.Invoke("Page_Load", new object[] { new object(), EventArgs.Empty });

            //Assert
            _txtStartDate.Text.ShouldBe("01/01/2018");
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetupInitialLoad_Test(bool isTestBlast)
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_blastScheduler);
            HideAllPanelsShim();

            _blastScheduler.CanScheduleRecurringBlast = true;
            var parameters = new object[] { isTestBlast, "regular" };

            //Act
            _privateTestObject.Invoke("SetupInitialLoad", parameters);

            //Assert            
            _ddlScheduleType.Visible.ShouldBeTrue();
        }

        [Test]
        [TestCase("Yes")]
        [TestCase("No")]
        public void CreateSendNow_Test(string selectedValue)
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_testEntity);

            string blastType = "ab";

            var parameters = new object[] { _setupInfo, blastType };

            _txtNumberToSend.Text = "1234";
            ReflectionHelper.SetField(_testEntity, "txtNumberToSend", _txtNumberToSend);

            var rblTestBlast = new RadioButtonList();
            rblTestBlast.Items.Add(new ListItem(selectedValue, selectedValue));
            rblTestBlast.SelectedValue = selectedValue;
            ReflectionHelper.SetField(_testEntity, "rblTestBlast", rblTestBlast);

            //Act
            _privateTestObject.Invoke("CreateSendNow", parameters);

            //Assert            
            _setupInfo.SendNowIsAmount.ShouldBe(true);
        }

        [Test]
        [TestCase("Send Now", "50")]
        [TestCase("Schedule One-Time", "75")]
        [TestCase("Schedule Recurring", "")]
        public void DdlSplitType_SelectedIndexChanged_Test(string selectedValue, string expected)
        {
            var ddlScheduleType = new DropDownList();
            var ddlSplitType = new DropDownList();
            var ddlRecurrence = new DropDownList();
            var ddlNumberToSendType = new DropDownList();
            var rvNumberToSend = new RangeValidator();

            try
            {
                //Arrange
                Initialize(CreateOneTime);
                InitializeControls(_testEntity);

                ddlScheduleType.Items.Insert(0, new ListItem(selectedValue, selectedValue));

                ddlSplitType.Items.Insert(0, new ListItem("Single Day", "Single Day"));
                ddlSplitType.Items.Insert(0, new ListItem("Evenly Split", "Evenly Split"));
                ddlSplitType.Items.Insert(0, new ListItem("Manually Split", "Manually Split"));
                ddlSplitType.SelectedValue = "Manually Split";

                ddlRecurrence.Items.Insert(0, new ListItem("Weekly", "Weekly"));

                ddlNumberToSendType.Items.Insert(0, new ListItem("Percent", "Percent"));

                DdlSplitType_SelectedIndexChanged_Test_SetFields(ddlScheduleType, ddlSplitType, ddlRecurrence, ddlNumberToSendType, rvNumberToSend);

                SetupManuallySplitShim(expected, rvNumberToSend);

                var parameters = new object[] { new object(), EventArgs.Empty };

                //Act
                _privateTestObject.Invoke("ddlSplitType_SelectedIndexChanged", parameters);

                //Assert            
                rvNumberToSend.MaximumValue.ShouldBe(expected);
            }
            finally
            {
                ddlScheduleType.Dispose();
                ddlSplitType.Dispose();
                ddlRecurrence.Dispose();
                ddlNumberToSendType.Dispose();
                rvNumberToSend.Dispose();
            }
        }

        [Test]
        [TestCase("Send Now")]
        [TestCase("Schedule One-Time")]
        [TestCase("Schedule Recurring")]
        [TestCase("--SELECT--")]
        public void DdlScheduleType_SelectedIndexChanged_Test(string selectedValue)
        {
            var ddlScheduleType = new DropDownList();
            try
            {
                //Arrange
                Initialize(CreateOneTime);
                InitializeControls(_testEntity);
                SetupDdlScheduleType_SelectedIndexChanged_Test_Shims();

                ddlScheduleType.Items.Insert(0, new ListItem(selectedValue, selectedValue));
                ReflectionHelper.SetField(_testEntity, "ddlScheduleType", ddlScheduleType);

                var parameters = new object[] { new object(), EventArgs.Empty };

                //Act
                _privateTestObject.Invoke("ddlScheduleType_SelectedIndexChanged", parameters);

                //Assert            
                _setupType.ShouldBe(selectedValue);
            }
            finally
            {
                ddlScheduleType.Dispose();
            }
        }

        [Test]
        [TestCase("Daily")]
        [TestCase("Weekly")]
        [TestCase("Monthly")]
        [TestCase("Default")]
        public void DdlRecurrence_SelectedIndexChanged_Test(string selectedValue)
        {
            var ddlRecurrence = new DropDownList();
            try
            {
                //Arrange
                Initialize(CreateOneTime);
                InitializeControls(_testEntity);
                DdlRecurrence_SelectedIndexChanged_Test_Shims();

                ddlRecurrence.Items.Insert(0, new ListItem(selectedValue, selectedValue));
                ReflectionHelper.SetField(_testEntity, "ddlRecurrence", ddlRecurrence);

                _setupType = selectedValue;

                var parameters = new object[] { new object(), EventArgs.Empty };

                //Act
                _privateTestObject.Invoke("ddlRecurrence_SelectedIndexChanged", parameters);

                //Assert            
                _setupType.ShouldBe(selectedValue);
            }
            finally
            {
                ddlRecurrence.Dispose();
            }
        }

        [Test]
        [TestCase("Yes", true)]
        [TestCase("No", false)]
        public void RblTestBlast_SelectedIndexChanged_Test(string selectedValue, bool expected)
        {
            var rblTestBlast = new RadioButtonList();

            try
            {
                //Arrange
                Initialize(CreateOneTime);
                InitializeControls(_testEntity);
                InitializeDummyShims();

                rblTestBlast.Items.Add(new ListItem(selectedValue, selectedValue));
                rblTestBlast.SelectedValue = selectedValue;

                _pnlTextBlast.Visible = !expected;

                ReflectionHelper.SetField(_testEntity, "pnlTextBlast", _pnlTextBlast);
                ReflectionHelper.SetField(_testEntity, "rblTestBlast", rblTestBlast);

                var parameters = new object[] { new object(), EventArgs.Empty };

                //Act
                _privateTestObject.Invoke("rblTestBlast_SelectedIndexChanged", parameters);

                //Assert  
                _pnlTextBlast.Visible.ShouldBe(expected);
            }
            finally
            {
                rblTestBlast.Dispose();
            }
        }

        [Test]
        public void ShowAllPanels_Test()
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_testEntity);

            _pnlTextBlast.Visible = false;

            ReflectionHelper.SetField(_testEntity, "pnlTextBlast", _pnlTextBlast);

            //Act
            _privateTestObject.Invoke("ShowAllPanels");

            //Assert            
            _pnlTextBlast.Visible.ShouldBeTrue();
        }

        [Test]
        public void ResetSchedule_Test()
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_testEntity);
            InitializeDummyShims();

            _rvMonth.Enabled = true;
            ReflectionHelper.SetField(_testEntity, "rvMonth", _rvMonth);

            //Act
            _privateTestObject.Invoke("ResetSchedule", false);

            //Assert            
            _rvMonth.Enabled.ShouldBeFalse();
        }

        [Test]
        public void SetupWizard_Test()
        {
            //Arrange
            Initialize(CreateOneTime);
            InitializeControls(_testEntity);
            InitializeDummyShims();

            _rvMonth.Enabled = true;
            ReflectionHelper.SetField(_testEntity, "rvMonth", _rvMonth);

            //Act
            _privateTestObject.Invoke("SetupWizard", false);

            //Assert            
            _rvMonth.Enabled.ShouldBeFalse();
        }

        [Test]
        [TestCase("", false)]
        [TestCase("Error", true)]
        public void SetupUpdateSchedule_Test(string error, bool expected)
        {
            var ddlScheduleType = new DropDownList();

            try
            {
                //Arrange
                Initialize(CreateOneTime);
                InitializeControls(_testEntity);
                InitializeDummyShims();

                var blastType = "ab";

                _pnlErrorMessage.Visible = false;
                ddlScheduleType.Items.Insert(0, new ListItem("Schedule Recurring", "Schedule Recurring"));

                ReflectionHelper.SetField(_testEntity, "pnlErrorMessage", _pnlErrorMessage);
                ReflectionHelper.SetField(_testEntity, "ddlScheduleType", ddlScheduleType);

                ShimBlastScheduler.AllInstances.ValidateSchedule = (instance) =>
                {
                    var errorList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        errorList.Add(error);
                    }

                    return errorList;
                };

                //Act
                var result = _testEntity.SetupUpdateSchedule(blastType);

                //Assert            
                _pnlErrorMessage.Visible.ShouldBe(expected);
            }
            finally
            {
                ddlScheduleType.Dispose();
            }
        }

        private void InitializeDummyShims()
        {
            ShimBlastScheduler.AllInstances.HideAllPanels = (instace) => { };
            ShimBlastScheduler.AllInstances.SetupInitialLoadBooleanString = (BlastScheduler instance, bool value1, string value2) => { };
            ShimBaseDataBoundControl.AllInstances.DataBind = (instance) => { };
            ShimBlastScheduler.AllInstances.CreateRecurringBlastSetupInfoRefString = (BlastScheduler instance, ref BlastSetupInfo setupInfo, string stringValue) =>
            {
                setupInfo = _setupInfo;
            };
        }

        private void DdlRecurrence_SelectedIndexChanged_Test_Shims()
        {
            ShimBlastScheduler.AllInstances.SetupRecurringSingleDay = (instace) =>
            {
                _setupType = "Daily";
            };

            ShimBlastScheduler.AllInstances.SetupRecurringWeeklyEvenlySplit = (instance) =>
            {
                _setupType = "Weekly";
            };

            ShimBlastScheduler.AllInstances.SetupRecurringMonthly = (instance) =>
            {
                _setupType = "Monthly";
            };
        }

        private static void SetupManuallySplitShim(string expected, RangeValidator rvNumberToSend)
        {
            ShimBlastScheduler.AllInstances.SetupManuallySplit = (instance) => { rvNumberToSend.MaximumValue = expected; };
        }

        private void SetupDdlScheduleType_SelectedIndexChanged_Test_Shims()
        {
            ShimBlastScheduler.AllInstances.SetupOneTime = (instance) =>
            { _setupType = "Schedule One-Time"; };

            ShimBlastScheduler.AllInstances.SetupSendNowBoolean = (BlastScheduler instance, bool boolValue) =>
            { _setupType = "Send Now"; };

            ShimBlastScheduler.AllInstances.SetupRecurringSingleDay = (instance) =>
            { _setupType = "Schedule Recurring"; };

            ShimBlastScheduler.AllInstances.SetupInitialLoadBooleanString = (BlastScheduler instance, bool boolValue, string campaignType) =>
            { _setupType = "--SELECT--"; };
        }

        private void DdlSplitType_SelectedIndexChanged_Test_SetFields(DropDownList ddlScheduleType, DropDownList ddlSplitType, DropDownList ddlRecurrence, DropDownList ddlNumberToSendType, RangeValidator rvNumberToSend)
        {
            ReflectionHelper.SetField(_testEntity, "ddlScheduleType", ddlScheduleType);
            ReflectionHelper.SetField(_testEntity, "ddlSplitType", ddlSplitType);
            ReflectionHelper.SetField(_testEntity, "ddlRecurrence", ddlRecurrence);
            ReflectionHelper.SetField(_testEntity, "ddlNumberToSendType", ddlNumberToSendType);
            ReflectionHelper.SetField(_testEntity, "rvNumberToSend", rvNumberToSend);
        }

        private void HideAllPanelsShim()
        {
            ShimBlastScheduler.AllInstances.HideAllPanels = (instance) =>
            {
                _ddlScheduleType.Visible = true;
            };
        }

        private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private void CreateShims(string method = "default")
        {
            ShimUserControl.AllInstances.SessionGet = x => { return HttpContext.Current.Session; };
            ECNEntitiesFakes.ShimBlastSetupInfo.AllInstances.BlastTypeGet = (x) => { return "ab"; };
            ShimECNSession.CurrentSession = () =>
            {
                User user = CreateInstance(typeof(User));
                user.UserID = 1;
                ECNBusiness.ECNSession ecnSession = CreateInstance(typeof(ECNBusiness.ECNSession));
                SetField(ecnSession, "CurrentUser", user);
                SetField(ecnSession, "CustomerID", 1);
                SetField(ecnSession, "BaseChannelID", 1);
                return ecnSession;
            };
            ShimAuthenticationTicket.getTicket = () =>
            {
                AuthenticationTicket authTkt = CreateInstance(typeof(AuthenticationTicket));
                SetField(authTkt, "CustomerID", 1);
                return authTkt;
            };
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            ShimBlastScheduler.AllInstances.getSplitTypeString = (x, y) => { return "m"; };
            ShimBlastSchedule.Constructor = (x) => new BlastSchedule()
            {
                Period = ","
            };
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (x, y) =>
            {
                _resultingSchedule = x;
                return 1;
            };
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (x, y) =>
            {
                _resultingSchedule = x;
                return 1;
            };
            ShimBlastSchedule.InsertBlastSchedule = (x) => { return 1; };
            if (method == "default")
            {
                ShimBlastScheduler.AllInstances.GetFirstDate = (x) =>
                {
                    DateTime dateTime = CreateInstance(typeof(DateTime));
                    SetField(dateTime, "DayOfWeek", 6);
                    List<DateTime> dateTimeList = new List<DateTime>()
                {
                    dateTime
                };
                    return dateTimeList;
                };
            }
        }

        private void Initialize(string method = "default")
        {
            CreateShims(method);
            _txtStartDate = new TextBox();
            _txtEndDate = new TextBox();
            _txtStartTime = new TextBox();
            _txtMonth = new TextBox();
            _txtNumberToSend = new TextBox();
            _txtWeeks = new TextBox();
            _txtDay1 = new TextBox();
            _txtDay2 = new TextBox();
            _txtDay3 = new TextBox();
            _txtDay4 = new TextBox();
            _txtDay5 = new TextBox();
            _txtDay6 = new TextBox();
            _txtDay7 = new TextBox();
            _cbxDay1 = new CheckBox();
            _cbxDay2 = new CheckBox();
            _cbxDay3 = new CheckBox();
            _cbxDay4 = new CheckBox();
            _cbxDay5 = new CheckBox();
            _cbxDay6 = new CheckBox();
            _cbxDay7 = new CheckBox();
            _rfvMonth = new RequiredFieldValidator();
            _rfvWeeks = new RequiredFieldValidator();
            _rfvDay1 = new RequiredFieldValidator();
            _rfvDay2 = new RequiredFieldValidator();
            _rfvDay3 = new RequiredFieldValidator();
            _rfvDay4 = new RequiredFieldValidator();
            _rfvDay5 = new RequiredFieldValidator();
            _rfvDay6 = new RequiredFieldValidator();
            _rfvDay7 = new RequiredFieldValidator();
            _rfvNumberToSend = new RequiredFieldValidator();
            _rvMonth = new RangeValidator();
            _rvWeeks = new RangeValidator();
            _rvDay1 = new RangeValidator();
            _rvDay2 = new RangeValidator();
            _rvDay3 = new RangeValidator();
            _rvDay4 = new RangeValidator();
            _rvDay5 = new RangeValidator();
            _rvDay6 = new RangeValidator();
            _rvDay7 = new RangeValidator();
            _rvNumberToSend = new RangeValidator();
            _blastSchedulerType = typeof(BlastScheduler);
            _blastScheduler = CreateInstance(typeof(BlastScheduler));
            _setupInfo = CreateInstance(typeof(BlastSetupInfo));
            _resultingSchedule = new BlastSchedule();
            _resultingSetup = new BlastSetupInfo();
            _ddlRecurrence = new DropDownList();
            _ddlNumberToSendType = new DropDownList();
            _ddlSplitType = new DropDownList();
            _ddlScheduleType = new DropDownList();
            SetDefaults();
        }

        private void SetDefaults()
        {
            SetField(_blastScheduler, "_MaxSendPercent", MaxSendPercent);
            _requestBlastID = 1;
            _blastType = "ab";
            _txtStartDate.Text = "01/01/2018";
            _txtEndDate.Text = "01/31/2018";
            _txtStartTime.Text = "12:00 AM";
            _txtMonth.Text = "1";
            _txtNumberToSend.Text = "10";
            _txtWeeks.Text = "4";
            _txtDay1.Text = "99";
            _txtDay2.Text = "10";
            _txtDay3.Text = "75";
            _txtDay4.Text = "62";
            _txtDay5.Text = "51";
            _txtDay6.Text = "40";
            _txtDay7.Text = "88";
            _cbxDay1.Checked = true;
            _cbxDay2.Checked = true;
            _cbxDay3.Checked = true;
            _cbxDay4.Checked = true;
            _cbxDay5.Checked = true;
            _cbxDay6.Checked = true;
            _cbxDay7.Checked = true;
            _ddlRecurrence.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Daily",
                Enabled = true,
                Value = "Daily"
            });
            _ddlNumberToSendType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Number",
                Enabled = true,
                Value = "Number"
            });
            _ddlSplitType.Items.Insert(0, new ListItem()
            {
                Selected = true,
                Text = "Evenly Split",
                Enabled = true,
                Value = "Evenly Split"
            });
            SetField(_blastScheduler, "requestBlastID", 1);
            SetField(_blastScheduler, "blastType", _blastType);
            SetField(_blastScheduler, "txtStartDate", _txtStartDate);
            SetField(_blastScheduler, "txtEndDate", _txtEndDate);
            SetField(_blastScheduler, "txtStartTime", _txtStartTime);
            SetField(_blastScheduler, "txtMonth", _txtMonth);
            SetField(_blastScheduler, "txtNumberToSend", _txtNumberToSend);
            SetField(_blastScheduler, "txtWeeks", _txtWeeks);
            SetField(_blastScheduler, "cbxDay1", _cbxDay1);
            SetField(_blastScheduler, "cbxDay2", _cbxDay2);
            SetField(_blastScheduler, "cbxDay3", _cbxDay3);
            SetField(_blastScheduler, "cbxDay4", _cbxDay4);
            SetField(_blastScheduler, "cbxDay5", _cbxDay5);
            SetField(_blastScheduler, "cbxDay6", _cbxDay6);
            SetField(_blastScheduler, "cbxDay7", _cbxDay7);
            SetField(_blastScheduler, "txtDay1", _txtDay1);
            SetField(_blastScheduler, "txtDay2", _txtDay2);
            SetField(_blastScheduler, "txtDay3", _txtDay3);
            SetField(_blastScheduler, "txtDay4", _txtDay4);
            SetField(_blastScheduler, "txtDay5", _txtDay5);
            SetField(_blastScheduler, "txtDay6", _txtDay6);
            SetField(_blastScheduler, "txtDay7", _txtDay7);
            SetField(_blastScheduler, "rvMonth", _rvMonth);
            SetField(_blastScheduler, "rvWeeks", _rvWeeks);
            SetField(_blastScheduler, "rvDay1", _rvDay1);
            SetField(_blastScheduler, "rvDay2", _rvDay2);
            SetField(_blastScheduler, "rvDay3", _rvDay3);
            SetField(_blastScheduler, "rvDay4", _rvDay4);
            SetField(_blastScheduler, "rvDay5", _rvDay5);
            SetField(_blastScheduler, "rvDay6", _rvDay6);
            SetField(_blastScheduler, "rvDay7", _rvDay7);
            SetField(_blastScheduler, "rfvNumberToSend", _rfvNumberToSend);
            SetField(_blastScheduler, "rfvMonth", _rfvMonth);
            SetField(_blastScheduler, "rfvWeeks", _rfvWeeks);
            SetField(_blastScheduler, "rfvDay1", _rfvDay1);
            SetField(_blastScheduler, "rfvDay2", _rfvDay2);
            SetField(_blastScheduler, "rfvDay3", _rfvDay3);
            SetField(_blastScheduler, "rfvDay4", _rfvDay4);
            SetField(_blastScheduler, "rfvDay5", _rfvDay5);
            SetField(_blastScheduler, "rfvDay6", _rfvDay6);
            SetField(_blastScheduler, "rfvDay7", _rfvDay7);
            SetField(_blastScheduler, "rvNumberToSend", _rvNumberToSend);
            SetField(_blastScheduler, "", _rvDay3);
            SetField(_blastScheduler, "", _rvDay3);
            SetField(_blastScheduler, "", _rvDay3);
            SetField(_blastScheduler, "", _rvDay3);
            SetField(_blastScheduler, "ddlScheduleType", _ddlScheduleType);
            SetField(_blastScheduler, "ddlRecurrence", _ddlRecurrence);
            SetField(_blastScheduler, "ddlNumberToSendType", _ddlNumberToSendType);
            SetField(_blastScheduler, "ddlSplitType", _ddlSplitType);
            SetSessionVariable("RequestBlastID", 12);
            SetField(_blastScheduler, "MaxSendPercent", MaxSendPercent);
            InitializePanelsForBlastSchedule();
        }

        private void SetField(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetField(obj, fieldName, value);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetField(obj, fieldName);
        }

        private void SetSelectedDays(string[] selectedDays)
        {
            if (!selectedDays.Contains("day1"))
            {
                _cbxDay1.Checked = false;
            }
            if (!selectedDays.Contains("day2"))
            {
                _cbxDay2.Checked = false;
            }
            if (!selectedDays.Contains("day3"))
            {
                _cbxDay3.Checked = false;
            }
            if (!selectedDays.Contains("day4"))
            {
                _cbxDay4.Checked = false;
            }
            if (!selectedDays.Contains("day5"))
            {
                _cbxDay5.Checked = false;
            }
            if (!selectedDays.Contains("day6"))
            {
                _cbxDay6.Checked = false;
            }
            if (!selectedDays.Contains("day7"))
            {
                _cbxDay7.Checked = false;
            }
        }

        private void DisableAllValidators()
        {
            _txtNumberToSend.Enabled = false;
            _rfvNumberToSend.Enabled = false;
            _rvNumberToSend.Enabled = false;
            _rfvMonth.Enabled = false;
            _rvMonth.Enabled = false;
            _rfvWeeks.Enabled = false;
            _rvWeeks.Enabled = false;
            _rfvDay1.Enabled = false;
            _rvDay1.Enabled = false;
            _rfvDay2.Enabled = false;
            _rfvDay2.Enabled = false;
            _rfvDay3.Enabled = false;
            _rvDay3.Enabled = false;
            _rfvDay4.Enabled = false;
            _rvDay4.Enabled = false;
            _rfvDay5.Enabled = false;
            _rvDay5.Enabled = false;
            _rfvDay6.Enabled = false;
            _rvDay6.Enabled = false;
            _rfvDay7.Enabled = false;
            _rvDay7.Enabled = false;
        }

        private void SelectDay(string dayName)
        {
            if (dayName == day1)
            {
                _cbxDay1.Checked = true;
                _rfvDay1.Enabled = false;
                _rvDay1.Enabled = false;
            }
            if (dayName == day2)
            {
                _cbxDay2.Checked = true;
                _rfvDay2.Enabled = false;
                _rvDay2.Enabled = false;
            }
            if (dayName == day3)
            {
                _cbxDay3.Checked = true;
                _rfvDay3.Enabled = false;
                _rvDay3.Enabled = false;
            }
            if (dayName == day4)
            {
                _cbxDay4.Checked = true;
                _rfvDay4.Enabled = false;
                _rvDay4.Enabled = false;
            }
            if (dayName == day5)
            {
                _cbxDay5.Checked = true;
                _rfvDay5.Enabled = false;
                _rvDay5.Enabled = false;
            }
            if (dayName == day6)
            {
                _cbxDay6.Checked = true;
                _rfvDay6.Enabled = false;
                _rvDay6.Enabled = false;
            }
            if (dayName == day7)
            {
                _cbxDay7.Checked = true;
                _rfvDay7.Enabled = false;
                _rvDay7.Enabled = false;
            }
        }

        private bool GetRequiredFieldValidatorForDay(string dayName)
        {
            if (dayName == "day1")
            {
                return _rfvDay1.Enabled;
            }
            if (dayName == "day2")
            {
                return _rfvDay2.Enabled;
            }
            if (dayName == "day3")
            {
                return _rfvDay3.Enabled;
            }
            if (dayName == "day4")
            {
                return _rfvDay4.Enabled;
            }
            if (dayName == "day5")
            {
                return _rfvDay5.Enabled;
            }
            if (dayName == "day6")
            {
                return _rfvDay6.Enabled;
            }
            if (dayName == "day7")
            {
                return _rfvDay7.Enabled;
            }
            else
            {
                return false;
            }
        }

        private void UncheckAllDays()
        {
            _cbxDay1.Checked = false;
            _cbxDay2.Checked = false;
            _cbxDay3.Checked = false;
            _cbxDay4.Checked = false;
            _cbxDay5.Checked = false;
            _cbxDay6.Checked = false;
            _cbxDay7.Checked = false;
        }

        private void SelectAllDaysWithValidValues()
        {
            _cbxDay1.Checked = true;
            _cbxDay2.Checked = true;
            _cbxDay3.Checked = true;
            _cbxDay4.Checked = true;
            _cbxDay5.Checked = true;
            _cbxDay6.Checked = true;
            _cbxDay7.Checked = true;
            _txtDay1.Text = txtDaySampleValue;
            _txtDay2.Text = txtDaySampleValue;
            _txtDay3.Text = txtDaySampleValue;
            _txtDay4.Text = txtDaySampleValue;
            _txtDay5.Text = txtDaySampleValue;
            _txtDay6.Text = txtDaySampleValue;
            _txtDay7.Text = txtDaySampleValue;
        }

        private void SelectAllDaysWithInvalidValues()
        {
            _cbxDay1.Checked = true;
            _cbxDay2.Checked = true;
            _cbxDay3.Checked = true;
            _cbxDay4.Checked = true;
            _cbxDay5.Checked = true;
            _cbxDay6.Checked = true;
            _cbxDay7.Checked = true;
            _txtDay1.Text = "a";
            _txtDay2.Text = "b";
            _txtDay3.Text = "c";
            _txtDay4.Text = "d";
            _txtDay5.Text = "e";
            _txtDay6.Text = "f";
            _txtDay7.Text = "g";
        }

        private void SetSessionVariable(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }

        private void SetProperty(dynamic instance, string propertyName, dynamic propertyValue)
        {
            ReflectionHelper.SetProperty(instance, propertyName, propertyValue);
        }

        private dynamic GetProperty(dynamic instance, string propertyName)
        {
            return ReflectionHelper.GetPropertyValue(instance, propertyName);
        }

        private void InitializePanelsForBlastSchedule()
        {
            _pnlTestBlast = new Panel();
            _pnlScheduleType = new Panel();
            _pnlRecurrence = new Panel();
            _pnlSplitType = new Panel();
            _pnlStart = new Panel();
            _pnlEnd = new Panel();
            _pnlNumberToSend = new Panel();
            _pnlNumberToSendType = new Panel();
            _pnlDays = new Panel();
            _pnlWeeks = new Panel();
            _pnlMonth = new Panel();
            _pnlEmailPreview = new Panel();
            _pnlErrorMessage = new Panel();
            _pnlTextBlast = new Panel();
            SetField(_blastScheduler, "pnlTestBlast", _pnlTestBlast);
            SetField(_blastScheduler, "pnlScheduleType", _pnlScheduleType);
            SetField(_blastScheduler, "pnlRecurrence", _pnlRecurrence);
            SetField(_blastScheduler, "pnlSplitType", _pnlSplitType);
            SetField(_blastScheduler, "pnlStart", _pnlStart);
            SetField(_blastScheduler, "pnlEnd", _pnlEnd);
            SetField(_blastScheduler, "pnlNumberToSend", _pnlNumberToSend);
            SetField(_blastScheduler, "pnlNumberToSendType", _pnlNumberToSendType);
            SetField(_blastScheduler, "pnlDays", _pnlDays);
            SetField(_blastScheduler, "pnlWeeks", _pnlWeeks);
            SetField(_blastScheduler, "pnlMonth", _pnlMonth);
            SetField(_blastScheduler, "pnlEmailPreview", _pnlEmailPreview);
            SetField(_blastScheduler, "pnlErrorMessage", _pnlErrorMessage);
            SetField(_blastScheduler, "pnlTextBlast", _pnlTextBlast);
            SetField(_blastScheduler, "cbLastDay", new CheckBox());
        }

        private void SelectSingleWeekDay(string checkBoxValue)
        {
            var tempCheckBoxId = string.Format("{0}", checkBoxValue.Substring(0, 6));
            for (var i = 1; i < 8; i++)
            {
                var currentCheckBoxId = string.Format("{0}{1}", tempCheckBoxId, i);
                if (currentCheckBoxId != checkBoxValue)
                {
                    SetField(_blastScheduler, currentCheckBoxId, new CheckBox()
                    {
                        Checked = false
                    });
                }
                else
                {
                    SetField(_blastScheduler, currentCheckBoxId, new CheckBox()
                    {
                        Checked = true
                    });
                }
            }
        }

        private void InitializeControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is UserControl)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to set field value: {0}", ex);
                    }
                }
            }
        }
    }
}