using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Entities.Communicator;
using static ECN_Framework_BusinessLayer.Communicator.BlastSchedule;
using BusinessBlastSchedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule;

namespace ecn.communicator.main.blasts
{
    public partial class BlastScheduler : System.Web.UI.UserControl
    {
        //delegate declaration
        public delegate void TestChecked(bool isTestBlast);
        //event declaration
        public event TestChecked checkedHandler;

        //delegate declaration
        public delegate void Reset();
        //event declaration
        public event Reset resetHandler;

        private const string ErrorAllSelectedDays = 
            "Please make sure all selected days add up to less than or equal to {0}.";
        private const string ErrorInvalidNumber = "Please enter a valid number for each day checked.";
        private const string ErrorSeelctAnyDay = "Please select at least one day.";
        private const string ErrorAddValidNumberToSend = "Please enter a valid number to send.";
        private const string ErrorWrongWeekNumber = "Please enter a valid number for how many weeks.";
        private const string ErrorInvalidDay = "Please enter a valid day of the month.";

        private const string SplitManually = "Manually Split";
        private const string NumberTypePercent = "Percent";
        private const string RecurrenceDaily = "Daily";
        private const string RecurrenceWeekly = "Weekly";
        private const string RecurrenceMonthly = "Monthly";
        private const string SplitSingleDay = "Single Day";
        private const string SplitEvenly = "Evenly Split";
        private const string NumberToSendAll = "ALL";

        private const int MaxWekNumber = 52;
        private const int MaxDaysToSend = 31;
        private const int FakeMaxDaysToSend = 99;

        private const string ScheduleTypeSendNow = "Send Now";
        private const string ScheduleTypeOneTime = "Schedule One-Time";
        private const string ScheduleTypeRecurring = "Schedule Recurring";
        private const string NumberTypeNumber = "Number";
        private const string SplitTypeEvenly = "e";
        private const string SplitTypeMonth = "m";
        private const string FakeMonthNUmber = "99";

        private const string SchedulePeriodO = "o";
        private const string SchedulePeriodDay = "d";
        private const string SchedulePeriodWeek = "w";
        private const int NoIndex = -1;
        private const string BlastTypeAb = "ab";
        private const string BlastFrequencyReccuring = "RECURRING";
        private const int SetupInfoSendNowAmount100 = 100;
        private const string BlastFrequencyOneTime = "ONETIME";
        private const int FullWeek = 7;

        private static string _MaxSendAmount;
        private static string _MaxSendPercent;

        public bool CanTestBlast
        {
            get
            {
                if (Session["CanTestBlast"] == null)
                {
                    Session["CanTestBlast"] = true;
                }

                return (bool)Session["CanTestBlast"];
            }
            set
            {
                Session["CanTestBlast"] = value;
            }
        }
        public bool CanEmailPreview
        {
            get
            {
                if (Session["CanEmailPreview"] == null)
                {
                    Session["CanEmailPreview"] = true;
                }

                return (bool)Session["CanEmailPreview"];
            }
            set
            {
                Session["CanEmailPreview"] = value;
            }
        }
        public bool CanBlast
        {
            get
            {
                if (Session["CanBlast"] == null)
                {
                    Session["CanBlast"] = true;
                }

                return (bool)Session["CanBlast"];
            }
            set
            {
                Session["CanBlast"] = value;
            }
        }
        public bool CanScheduleBlast
        {
            get
            {
                if (Session["CanScheduleBlast"] == null)
                {
                    Session["CanScheduleBlast"] = true;
                }

                return (bool)Session["CanScheduleBlast"];
            }
            set
            {
                Session["CanScheduleBlast"] = value;
            }
        }
        public bool CanScheduleRecurringBlast
        {
            get
            {
                if (Session["CanScheduleRecurringBlast"] == null)
                {
                    Session["CanScheduleRecurringBlast"] = true;
                }

                return (bool)Session["CanScheduleRecurringBlast"];
            }
            set
            {
                Session["CanScheduleRecurringBlast"] = value;
            }
        }

        public string CampaignItemType
        {
            get
            {
                if (Session["CampaignItemType"] == null)
                {
                    Session["CampaignItemType"] = "regular";
                }

                return Session["CampaignItemType"].ToString();
            }
            set
            {
                Session["CampaignItemType"] = value;
            }
        }
        public bool CanSendAll
        {
            get
            {
                if (Session["CanSendAll"] == null)
                {
                    Session["CanSendAll"] = true;
                }

                return (bool)Session["CanSendAll"];
            }
            set
            {
                Session["CanSendAll"] = value;
            }
        }
        public int RequestBlastID
        {
            get
            {
                if (Session["RequestBlastID"] == null)
                {
                    Session["RequestBlastID"] = 0;
                }

                return (Int32)Session["RequestBlastID"];
            }
            set
            {
                Session["RequestBlastID"] = value;
            }
        }

        public int SourceBlastID
        {
            get
            {
                if (Session["SourceBlastID"] == null)
                {
                    Session["SourceBlastID"] = 0;
                }

                return (Int32)Session["SourceBlastID"];
            }
            set
            {
                Session["SourceBlastID"] = value;
            }
        }

        private IDictionary<int, Tuple<TextBox, CheckBox>> DayControls
        {
            get
            {
                var dayControls = new Dictionary<int, Tuple<TextBox, CheckBox>>
                                  {
                                      [6] = new Tuple<TextBox, CheckBox>(txtDay7, cbxDay7),
                                      [5] = new Tuple<TextBox, CheckBox>(txtDay6, cbxDay6),
                                      [4] = new Tuple<TextBox, CheckBox>(txtDay5, cbxDay5),
                                      [3] = new Tuple<TextBox, CheckBox>(txtDay4, cbxDay4),
                                      [2] = new Tuple<TextBox, CheckBox>(txtDay3, cbxDay3),
                                      [1] = new Tuple<TextBox, CheckBox>(txtDay2, cbxDay2),
                                      [0] = new Tuple<TextBox, CheckBox>(txtDay1, cbxDay1)
                                  };

                return dayControls;
            }
        }

        public void ResetSchedule(bool isTestBlast = false)
        {
            //SetupSendNow(isTestBlast);

            HideAllPanels();
            SetupInitialLoad(isTestBlast);
            Session["SourceBlastID"] = null;
            Session["RequestBlastID"] = null;
            rfvStartDate.Enabled = false;
            rfvStartTime.Enabled = false;
            rfvEndDate.Enabled = false;
            rfvWeeks.Enabled = false;
            rvWeeks.Enabled = false;
            rfvMonth.Enabled = false;
            rvMonth.Enabled = false;
        }

        public void LoadBlastSchedule()
        {
            var blastId = GetBlastId();

            if (blastId > 0)
            {
                var schedule = GetByBlastID(blastId, true);
                if (schedule != null)
                {
                    txtStartDate.Text = schedule.SchedStartDate;
                    txtStartTime.Text = schedule.SchedTime;
                    switch (schedule.Period)
                    {
                        case SchedulePeriodO:
                            ScheduleForPeriodO(schedule);
                            break;
                        case SchedulePeriodDay:
                            ScheduleForPeriodDay(schedule);
                            break;
                        case SchedulePeriodWeek:
                            ScheduleForPeriodWeek(schedule);
                            break;
                        case SplitTypeMonth:
                            ScheduleForPeriodMonth(schedule);
                            break;
                    }
                }
            }
        }

        private int GetBlastId()
        {
            var blastId = 0;
            if (RequestBlastID > 0)
            {
                blastId = RequestBlastID;
            }
            else if (SourceBlastID > 0)
            {
                blastId = SourceBlastID;
            }
            return blastId;
        }

        private void ScheduleForPeriodMonth(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            SetupRecurringMonthly();
            txtEndDate.Text = schedule.SchedEndDate;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            txtMonth.Enabled = true;
            txtMonth.Text = schedule.DaysList[0].DayToSend.ToString();
            cbLastDay.Checked = false;
            if (txtMonth.Text == FakeMonthNUmber)
            {
                cbLastDay.Checked = true;
                txtMonth.Enabled = false;
            }

            if (schedule.DaysList[0].IsAmount == null)
            {
                ddlNumberToSendType.SelectedIndex = NoIndex;
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }
            else
            {
                txtNumberToSend.Enabled = true;
                txtNumberToSend.Text = schedule.DaysList[0].Total.ToString();
                ddlNumberToSendType.SelectedIndex = NoIndex;
                if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
                {
                    ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypeNumber);
                }
                else
                {
                    ddlNumberToSendType.SelectedValue = NumberTypePercent;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypePercent);
                }
            }
        }

        private void ScheduleForPeriodWeek(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            txtEndDate.Text = schedule.SchedEndDate;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            ddlRecurrence.SelectedIndex = NoIndex;
            ddlRecurrence.SelectedValue = RecurrenceWeekly;

            ddlSplitType.SelectedIndex = NoIndex;
            if (!string.IsNullOrWhiteSpace(schedule.SplitType))
            {
                switch (schedule.SplitType)
                {
                    case SplitTypeMonth:
                        ddlSplitType.SelectedValue = SplitManually;
                        SetupRecurringWeeklyManuallySplit();
                        break;
                    case SplitTypeEvenly:
                        ddlSplitType.SelectedValue = SplitEvenly;
                        SetupRecurringWeeklyEvenlySplit();
                        break;
                    default:
                        ddlSplitType.SelectedValue = SplitManually;
                        SetupRecurringWeeklyManuallySplit();
                        break;
                }
            }
            else
            {
                ddlSplitType.SelectedValue = SplitManually;
                SetupRecurringWeeklyManuallySplit();
            }

            txtWeeks.Enabled = true;
            txtWeeks.Text = schedule.DaysList[0].Weeks.ToString();
            ddlNumberToSendType.SelectedIndex = NoIndex;

            if (schedule.SplitType == SplitTypeMonth)
            {
                if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
                {
                    ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                }
                else
                {
                    ddlNumberToSendType.SelectedValue = NumberTypePercent;
                }
            }

            LoadDaysAndAmounts(schedule.DaysList, schedule.SplitType);
        }

        private void ScheduleForPeriodDay(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            SetupRecurringSingleDay();
            txtEndDate.Text = schedule.SchedEndDate;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            if (schedule.DaysList == null || schedule.DaysList.Count == 0)
            {
                ddlNumberToSendType.SelectedIndex = NoIndex;
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }
            else
            {
                txtNumberToSend.Enabled = true;
                txtNumberToSend.Text = schedule.DaysList[0].Total.ToString();
                ddlNumberToSendType.SelectedIndex = NoIndex;
                if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
                {
                    ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypeNumber);
                }
                else
                {
                    ddlNumberToSendType.SelectedValue = NumberTypePercent;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypePercent);
                }
            }
        }

        private void ScheduleForPeriodO(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeOneTime;
            txtEndDate.Text = schedule.SchedEndDate;
            if (schedule.DaysList == null || schedule.DaysList.Count == 0)
            {
                SetupOneTime();
                ddlSplitType.SelectedIndex = NoIndex;
                ddlSplitType.SelectedValue = SplitSingleDay;
                ddlNumberToSendType.SelectedIndex = NoIndex;
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }
            else if (schedule.DaysList.Count == 1)
            {
                SelectForOneDay(schedule);
            }
            else if (schedule.DaysList.Count > 1)
            {
                SelectForManyDays(schedule);
            }
        }

        private void SelectForManyDays(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            if (string.Equals(schedule.SplitType, SplitTypeEvenly, StringComparison.OrdinalIgnoreCase))
            {
                SetupEvenlySplit();
                ddlSplitType.SelectedIndex = NoIndex;
                ddlSplitType.SelectedValue = SplitEvenly;
            }
            else
            {
                SetupManuallySplit();
                ddlSplitType.SelectedIndex = NoIndex;
                ddlSplitType.SelectedValue = SplitManually;
            }

            ddlNumberToSendType.SelectedIndex = NoIndex;
            if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
            {
                ddlNumberToSendType.SelectedValue = NumberTypeNumber;
            }
            else
            {
                ddlNumberToSendType.SelectedValue = NumberTypePercent;
            }

            LoadDaysAndAmounts(schedule.DaysList, schedule.SplitType);
        }

        private void SelectForOneDay(BlastSchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            if (schedule.DaysList[0].DayToSend == null)
            {
                SetupOneTime();
                ddlSplitType.SelectedIndex = NoIndex;
                ddlSplitType.SelectedValue = SplitSingleDay;
                txtNumberToSend.Enabled = true;
                txtNumberToSend.Text = schedule.DaysList[0].Total.ToString();
                ddlNumberToSendType.SelectedIndex = NoIndex;
                if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
                {
                    ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypeNumber);
                }
                else
                {
                    ddlNumberToSendType.SelectedValue = NumberTypePercent;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    SetMaxAmount(NumberTypePercent);
                }
            }
            else
            {
                if (schedule.SplitType.ToLower().Equals(SplitTypeEvenly))
                {
                    SetupEvenlySplit();
                    ddlSplitType.SelectedIndex = NoIndex;
                    ddlSplitType.SelectedValue = SplitEvenly;
                }
                else
                {
                    SetupManuallySplit();
                    ddlSplitType.SelectedIndex = NoIndex;
                    ddlSplitType.SelectedValue = SplitManually;
                }

                ddlNumberToSendType.SelectedIndex = NoIndex;
                if (Convert.ToBoolean(schedule.DaysList[0].IsAmount))
                {
                    ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                }
                else
                {
                    ddlNumberToSendType.SelectedValue = NumberTypePercent;
                }

                LoadDaysAndAmounts(schedule.DaysList, schedule.SplitType);
            }
        }

        private enum DaysOfTheWeek
        {
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday
        };

        private void FindDayControls(DayOfWeek dayofweek, int amount, string splitType = SplitTypeMonth)
        {
            if (ddlNumberToSendType.SelectedValue == NumberTypePercent)
            {
                SetMaxAmount(NumberTypePercent);
            }
            else
            {
                SetMaxAmount(NumberTypeNumber);
            }
            for (int i = 0; i < 7; i++)
            {
                if (cbxDay1.Text == dayofweek.ToString())
                {
                    cbxDay1.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay1.Enabled = true;
                        txtDay1.Text = amount.ToString();
                        rfvDay1.Enabled = true;
                        rvDay1.Enabled = true;
                    }
                    break;
                }
                if (cbxDay2.Text == dayofweek.ToString())
                {
                    cbxDay2.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay2.Enabled = true;
                        txtDay2.Text = amount.ToString();
                        rfvDay2.Enabled = true;
                        rvDay2.Enabled = true;
                    }
                    break;
                }
                if (cbxDay3.Text == dayofweek.ToString())
                {
                    cbxDay3.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay3.Enabled = true;
                        txtDay3.Text = amount.ToString();
                        rfvDay3.Enabled = true;
                        rvDay3.Enabled = true;
                    }
                    break;
                }
                if (cbxDay4.Text == dayofweek.ToString())
                {
                    cbxDay4.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay4.Enabled = true;
                        txtDay4.Text = amount.ToString();
                        rfvDay4.Enabled = true;
                        rvDay4.Enabled = true;
                    }
                    break;
                }
                if (cbxDay5.Text == dayofweek.ToString())
                {
                    cbxDay5.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay5.Enabled = true;
                        txtDay5.Text = amount.ToString();
                        rfvDay5.Enabled = true;
                        rvDay5.Enabled = true;
                    }
                    break;
                }
                if (cbxDay6.Text == dayofweek.ToString())
                {
                    cbxDay6.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay6.Enabled = true;
                        txtDay6.Text = amount.ToString();
                        rfvDay6.Enabled = true;
                        rvDay6.Enabled = true;
                    }
                    break;
                }
                if (cbxDay7.Text == dayofweek.ToString())
                {
                    cbxDay7.Checked = true;
                    if (splitType != SplitTypeEvenly)
                    {
                        txtDay7.Enabled = true;
                        txtDay7.Text = amount.ToString();
                        rfvDay7.Enabled = true;
                        rvDay7.Enabled = true;
                    }
                    break;
                }
            }
        }

        private void LoadDaysAndAmounts(List<ECN_Framework_Entities.Communicator.BlastScheduleDays> daysList, string splitType = SplitTypeMonth)
        {
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            //cbxDay1.Text = startDate.ToString("dddd");
            //cbxDay2.Text = startDate.AddDays(1).ToString("dddd");
            //cbxDay3.Text = startDate.AddDays(2).ToString("dddd");
            //cbxDay4.Text = startDate.AddDays(3).ToString("dddd");
            //cbxDay5.Text = startDate.AddDays(4).ToString("dddd");
            //cbxDay6.Text = startDate.AddDays(5).ToString("dddd");
            //cbxDay7.Text = startDate.AddDays(6).ToString("dddd");

            //cbxDay1.Checked = false;
            //cbxDay2.Checked = false;
            //cbxDay3.Checked = false;
            //cbxDay4.Checked = false;
            //cbxDay5.Checked = false;
            //cbxDay6.Checked = false;
            //cbxDay7.Checked = false;
            //txtDay1.Enabled = false;
            //txtDay2.Enabled = false;
            //txtDay3.Enabled = false;
            //txtDay4.Enabled = false;
            //txtDay5.Enabled = false;
            //txtDay6.Enabled = false;
            //txtDay7.Enabled = false;

            SetupDaysControls(false, false);

            rfvDay1.Enabled = false;
            rvDay1.Enabled = false;
            rfvDay2.Enabled = false;
            rfvDay2.Enabled = false;
            rfvDay3.Enabled = false;
            rvDay3.Enabled = false;
            rfvDay4.Enabled = false;
            rvDay4.Enabled = false;
            rfvDay5.Enabled = false;
            rvDay5.Enabled = false;
            rfvDay6.Enabled = false;
            rvDay6.Enabled = false;
            rfvDay7.Enabled = false;
            rvDay7.Enabled = false;

            foreach (ECN_Framework_Entities.Communicator.BlastScheduleDays days in daysList)
            {
                switch (Convert.ToInt32(days.DayToSend))
                {
                    case 0:
                        FindDayControls(DayOfWeek.Sunday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 1:
                        FindDayControls(DayOfWeek.Monday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 2:
                        FindDayControls(DayOfWeek.Tuesday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 3:
                        FindDayControls(DayOfWeek.Wednesday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 4:
                        FindDayControls(DayOfWeek.Thursday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 5:
                        FindDayControls(DayOfWeek.Friday, Convert.ToInt32(days.Total), splitType);
                        break;
                    case 6:
                        FindDayControls(DayOfWeek.Saturday, Convert.ToInt32(days.Total), splitType);
                        break;
                    default:
                        break;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _MaxSendAmount = "1000000";
            _MaxSendPercent = "100";
            CanTestBlast = CanTestBlast == false ? false : true;
            CanEmailPreview = CanEmailPreview == false ? false : true;
            CanBlast = CanBlast == false ? false : true;
            CanScheduleBlast = CanScheduleBlast == false ? false : true;
            CanSendAll = CanSendAll == false ? false : true;
            CanScheduleRecurringBlast = CanScheduleRecurringBlast == false ? false : true;


            if (CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) == null)
            {
                ddlScheduleType.Items.Clear();
                ddlScheduleType.Items.Add("--SELECT--");
                ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                ddlScheduleType.Items.Add(ScheduleTypeRecurring);
            }
            else if (!CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) != null)
            {
                ddlScheduleType.Items.Clear();
                ddlScheduleType.Items.Add("--SELECT--");
                ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                ddlScheduleType.Items.Add(ScheduleTypeOneTime);
            }

            if (!IsPostBack)
            {
                ddlScheduleType.Items.Clear();
                ddlScheduleType.Items.Add("--SELECT--");
                ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                if (CanScheduleRecurringBlast)
                {
                    ddlScheduleType.Items.Add(ScheduleTypeRecurring);
                }
                //SetupInitialLoad();
                rfvStartDate.Enabled = false;
                rfvStartTime.Enabled = false;
                rfvEndDate.Enabled = false;
                rfvWeeks.Enabled = false;
                rvWeeks.Enabled = false;
                rfvMonth.Enabled = false;
                rvMonth.Enabled = false;
                if (RequestBlastID > 0 || SourceBlastID > 0)
                {
                    LoadBlastSchedule();
                }

            }

            if (txtStartDate.Text.Length == 0 || txtStartTime.Text.Length == 0)
            {
                txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtStartTime.Text = DateTime.Now.AddMinutes(15).ToString("HH:mm:ss");
            }
            if (txtEndDate.Text.Length == 0)
            {
                txtEndDate.Text = DateTime.Now.AddYears(1).ToString("MM/dd/yyyy");
            }
            //DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            //cbxDay1.Text = startDate.ToString("dddd");
            //cbxDay2.Text = startDate.AddDays(1).ToString("dddd");
            //cbxDay3.Text = startDate.AddDays(2).ToString("dddd");
            //cbxDay4.Text = startDate.AddDays(3).ToString("dddd");
            //cbxDay5.Text = startDate.AddDays(4).ToString("dddd");
            //cbxDay6.Text = startDate.AddDays(5).ToString("dddd");
            //cbxDay7.Text = startDate.AddDays(6).ToString("dddd");

        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "jquery", "enabledtpicker();", true);
        }


        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    RegisterTextBoxForDatePicker(Page);
        //}


        //private void RegisterTextBoxForDatePicker(Page page)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "jquery", "enabledtpicker();", true);
        //}

        private void ShowAllPanels()
        {
            pnlTestBlast.Visible = true;
            pnlScheduleType.Visible = true;
            pnlRecurrence.Visible = true;
            pnlSplitType.Visible = true;
            pnlStart.Visible = true;
            pnlEnd.Visible = true;
            pnlNumberToSend.Visible = true;
            pnlNumberToSendType.Visible = true;

            pnlDays.Visible = true;
            pnlWeeks.Visible = true;
            pnlMonth.Visible = true;
            pnlEmailPreview.Visible = true;
            pnlErrorMessage.Visible = true;
            pnlTextBlast.Visible = true;
        }

        private void HideAllPanels()
        {
            pnlTestBlast.Visible = false;
            pnlScheduleType.Visible = false;
            pnlRecurrence.Visible = false;
            pnlSplitType.Visible = false;
            pnlStart.Visible = false;
            pnlEnd.Visible = false;
            pnlNumberToSend.Visible = false;
            pnlNumberToSendType.Visible = false;
            pnlDays.Visible = false;
            pnlWeeks.Visible = false;
            pnlMonth.Visible = false;
            pnlEmailPreview.Visible = false;
            pnlErrorMessage.Visible = false;
            pnlTextBlast.Visible = false;
        }

        private void DisableAllValidators()
        {
            rfvNumberToSend.Enabled = false;
            rvNumberToSend.Enabled = false;
            rfvMonth.Enabled = false;
            rvMonth.Enabled = false;
            rfvWeeks.Enabled = false;
            rvWeeks.Enabled = false;
            rfvDay1.Enabled = false;
            rvDay1.Enabled = false;
            rfvDay2.Enabled = false;
            rfvDay2.Enabled = false;
            rfvDay3.Enabled = false;
            rvDay3.Enabled = false;
            rfvDay4.Enabled = false;
            rvDay4.Enabled = false;
            rfvDay5.Enabled = false;
            rvDay5.Enabled = false;
            rfvDay6.Enabled = false;
            rvDay6.Enabled = false;
            rfvDay7.Enabled = false;
            rvDay7.Enabled = false;
        }

        private void SetupInitialLoad(bool isTestBlast = false, string campaignItemType = "regular")
        {

            if (isTestBlast)
            {

                HideAllPanels();
                pnlTestBlast.Visible = true;
                pnlTextBlast.Visible = true;
                Session["SelectedTestGroups_List"] = null;
                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreview))
                {
                    pnlEmailPreview.Visible = true;
                }
                isTestBlast = true;
                rblTestBlast.SelectedValue = "Yes";
                lblSocialMessage.Visible = true;
                if (checkedHandler != null)
                    checkedHandler(isTestBlast);
            }
            else
            {
                DisableAllValidators();
                HideAllPanels();
                pnlTestBlast.Visible = CanTestBlast;

                rblTestBlast.SelectedValue = "No";
                lblSocialMessage.Visible = false;
                rblTestBlast.Enabled = CanTestBlast;
                pnlScheduleType.Visible = true;
                ddlScheduleType.SelectedIndex = NoIndex;
                ddlScheduleType.SelectedValue = "--SELECT--";
                //pnlNumberToSend.Visible = true;

                //pnlNumberToSendType.Visible = true;


                if (CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) == null)
                {
                    ddlScheduleType.Items.Clear();
                    ddlScheduleType.Items.Add("--SELECT--");
                    ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                    ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                    ddlScheduleType.Items.Add(ScheduleTypeRecurring);
                }
                else if (!CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) != null)
                {
                    ddlScheduleType.Items.Clear();
                    ddlScheduleType.Items.Add("--SELECT--");
                    ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                    ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                }
            }
        }

        private void SetupSendNow(bool isTestBlast = false)
        {
            _MaxSendAmount = "1000000";
            _MaxSendPercent = "100";
            if (isTestBlast)
            {

                HideAllPanels();
                pnlTestBlast.Visible = true;
                pnlTextBlast.Visible = true;
                Session["SelectedTestGroups_List"] = null;
                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreview))
                {
                    pnlEmailPreview.Visible = true;
                }
                isTestBlast = true;
                rblTestBlast.SelectedValue = "Yes";
                lblSocialMessage.Visible = true;
                if (checkedHandler != null)
                    checkedHandler(isTestBlast);
            }
            else
            {
                DisableAllValidators();
                HideAllPanels();
                pnlTestBlast.Visible = CanTestBlast;

                rblTestBlast.SelectedValue = "No";
                lblSocialMessage.Visible = false;
                rblTestBlast.Enabled = CanTestBlast;
                pnlScheduleType.Visible = true;
                //ddlScheduleType.SelectedIndex = -1;
                //ddlScheduleType.SelectedValue = "Send Now";
                pnlNumberToSend.Visible = true;
                ddlNumberToSendType.SelectedIndex = NoIndex;

                SetupCanSendAll();

                if (CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) == null)
                {
                    ddlScheduleType.Items.Clear();
                    ddlScheduleType.Items.Add("--SELECT--");
                    ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                    ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                    ddlScheduleType.Items.Add(ScheduleTypeRecurring);
                }
                else if (!CanScheduleRecurringBlast && ddlScheduleType.Items.FindByText(ScheduleTypeRecurring) != null)
                {
                    ddlScheduleType.Items.Clear();
                    ddlScheduleType.Items.Add("--SELECT--");
                    ddlScheduleType.Items.Add(ScheduleTypeSendNow);
                    ddlScheduleType.Items.Add(ScheduleTypeOneTime);
                }
            }
        }

        private void SetupCanSendAll()
        {
            txtNumberToSend.Text = string.Empty;
            pnlNumberToSendType.Visible = true;
            ddlNumberToSendType.Items.Clear();
            ddlNumberToSendType.Items.Add(NumberTypePercent);
            ddlNumberToSendType.Items.Add(NumberTypeNumber);
            if (CanSendAll && !ddlNumberToSendType.Items.Contains(new ListItem(NumberToSendAll)))
            {
                ddlNumberToSendType.Items.Add(NumberToSendAll);
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }

            if (CanSendAll)
            {
                txtNumberToSend.Enabled = false;
                rfvNumberToSend.Enabled = false;
                rvNumberToSend.Enabled = false;
            }
            else
            {
                ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                txtNumberToSend.Enabled = true;
                rfvNumberToSend.Enabled = true;
                rvNumberToSend.Enabled = true;
                SetMaxAmount(NumberTypeNumber);
            }
        }

        private void SetupOneTime()
        {
            DisableAllValidators();
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeOneTime;
            pnlSplitType.Visible = true;
            ddlSplitType.Items.Clear();
            ddlSplitType.Items.Add(SplitSingleDay);
            if (!CampaignItemType.Equals("ab", StringComparison.OrdinalIgnoreCase)
                && !CampaignItemType.Equals("champion", StringComparison.OrdinalIgnoreCase))
            {
                ddlSplitType.Items.Add(SplitEvenly);
                ddlSplitType.Items.Add(SplitManually);
            }
            ddlSplitType.SelectedValue = SplitSingleDay;
            pnlStart.Visible = true;
            pnlNumberToSend.Visible = true;
            txtEndDate.Text = DateTime.Now.AddDays(FullWeek).ToShortDateString();

            SetupCanSendAll();
        }

        private void SetupEvenlySplit()
        {
            DisableAllValidators();
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeOneTime;
            pnlSplitType.Visible = true;
            ddlSplitType.Items.Clear();
            ddlSplitType.Items.Add(SplitSingleDay);
            ddlSplitType.Items.Add(SplitEvenly);
            ddlSplitType.Items.Add(SplitManually);
            ddlSplitType.SelectedValue = SplitEvenly;
            pnlStart.Visible = true;
            pnlDays.Visible = true;
            cbxDay1.Checked = true;
            cbxDay2.Checked = true;
            cbxDay3.Checked = true;
            cbxDay4.Checked = true;
            cbxDay5.Checked = true;
            cbxDay6.Checked = true;
            cbxDay7.Checked = true;
            txtDay1.Text = "";
            txtDay2.Text = "";
            txtDay3.Text = "";
            txtDay4.Text = "";
            txtDay5.Text = "";
            txtDay6.Text = "";
            txtDay7.Text = "";
            txtDay1.Enabled = false;
            txtDay2.Enabled = false;
            txtDay3.Enabled = false;
            txtDay4.Enabled = false;
            txtDay5.Enabled = false;
            txtDay6.Enabled = false;
            txtDay7.Enabled = false;
        }

        private void SetupManuallySplit()
        {
            DisableAllValidators();
            rfvDay1.Enabled = true;
            rvDay1.Enabled = true;
            rfvDay2.Enabled = true;
            rfvDay2.Enabled = true;
            rfvDay3.Enabled = true;
            rvDay3.Enabled = true;
            rfvDay4.Enabled = true;
            rvDay4.Enabled = true;
            rfvDay5.Enabled = true;
            rvDay5.Enabled = true;
            rfvDay6.Enabled = true;
            rvDay6.Enabled = true;
            rfvDay7.Enabled = true;
            rvDay7.Enabled = true;
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeOneTime;
            pnlSplitType.Visible = true;
            ddlSplitType.Items.Clear();
            ddlSplitType.Items.Add(SplitSingleDay);
            ddlSplitType.Items.Add(SplitEvenly);
            ddlSplitType.Items.Add(SplitManually);
            ddlSplitType.SelectedValue = SplitManually;
            pnlNumberToSendType.Visible = true;
            ddlNumberToSendType.Items.Clear();
            ddlNumberToSendType.Items.Add(NumberTypePercent);
            ddlNumberToSendType.Items.Add(NumberTypeNumber);
            ddlNumberToSendType.SelectedValue = NumberTypeNumber;
            pnlStart.Visible = true;
            pnlDays.Visible = true;
            SetupDaysControls(true, true);
            SetMaxAmount(NumberTypeNumber);
        }

        private void SetupRecurringSingleDay()
        {
            DisableAllValidators();
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            pnlRecurrence.Visible = true;
            ddlRecurrence.SelectedIndex = NoIndex;
            ddlRecurrence.SelectedValue = RecurrenceDaily;
            pnlStart.Visible = true;
            pnlEnd.Visible = true;
            pnlNumberToSend.Visible = true;
            txtNumberToSend.Text = "";
            pnlNumberToSendType.Visible = true;
            ddlNumberToSendType.Items.Clear();
            ddlNumberToSendType.Items.Add(NumberTypePercent);
            ddlNumberToSendType.Items.Add(NumberTypeNumber);
            txtEndDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            if (CanSendAll && !ddlNumberToSendType.Items.Contains(new ListItem(NumberToSendAll)))
            {
                ddlNumberToSendType.Items.Add(NumberToSendAll);
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }
            if (CanSendAll)
            {
                txtNumberToSend.Enabled = false;
                rfvNumberToSend.Enabled = false;
                rvNumberToSend.Enabled = false;
            }
            else
            {
                ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                txtNumberToSend.Enabled = true;
                rfvNumberToSend.Enabled = true;
                rvNumberToSend.Enabled = true;
                SetMaxAmount(NumberTypeNumber);
            }
        }

        private void SetupRecurringWeeklyEvenlySplit()
        {
            DisableAllValidators();
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            pnlRecurrence.Visible = true;
            ddlRecurrence.SelectedIndex = NoIndex;
            ddlRecurrence.SelectedValue = RecurrenceWeekly;
            pnlSplitType.Visible = true;
            ddlSplitType.Items.Clear();
            ddlSplitType.Items.Add(SplitEvenly);
            ddlSplitType.Items.Add(SplitManually);
            ddlSplitType.SelectedValue = SplitEvenly;
            pnlStart.Visible = true;
            pnlEnd.Visible = true;
            pnlWeeks.Visible = true;
            txtWeeks.Text = "2";
            pnlDays.Visible = true;

            SetupDaysControls(true, true);
        }

        private void SetupRecurringWeeklyManuallySplit()
        {
            DisableAllValidators();
            rfvDay1.Enabled = true;
            rvDay1.Enabled = true;
            rfvDay2.Enabled = true;
            rfvDay2.Enabled = true;
            rfvDay3.Enabled = true;
            rvDay3.Enabled = true;
            rfvDay4.Enabled = true;
            rvDay4.Enabled = true;
            rfvDay5.Enabled = true;
            rvDay5.Enabled = true;
            rfvDay6.Enabled = true;
            rvDay6.Enabled = true;
            rfvDay7.Enabled = true;
            rvDay7.Enabled = true;
            HideAllPanels();
            pnlScheduleType.Visible = true;
            ddlScheduleType.SelectedIndex = NoIndex;
            ddlScheduleType.SelectedValue = ScheduleTypeRecurring;
            pnlRecurrence.Visible = true;
            ddlRecurrence.SelectedIndex = NoIndex;
            ddlRecurrence.SelectedValue = RecurrenceWeekly;
            pnlSplitType.Visible = true;
            ddlSplitType.Items.Clear();
            ddlSplitType.Items.Add(SplitEvenly);
            ddlSplitType.Items.Add(SplitManually);
            ddlSplitType.SelectedValue = SplitManually;
            pnlNumberToSendType.Visible = true;
            ddlNumberToSendType.Items.Clear();
            ddlNumberToSendType.Items.Add(NumberTypePercent);
            ddlNumberToSendType.Items.Add(NumberTypeNumber);
            ddlNumberToSendType.SelectedValue = NumberTypeNumber;
            pnlStart.Visible = true;
            pnlEnd.Visible = true;
            pnlWeeks.Visible = true;
            txtWeeks.Text = "2";
            pnlDays.Visible = true;
            SetupDaysControls(true, true);
            SetMaxAmount(NumberTypeNumber);
        }

        private void SetupRecurringMonthly()
        {
            DisableAllValidators();
            HideAllPanels();
            pnlScheduleType.Visible = true;
            //ddlScheduleType.SelectedIndex = -1;
            //ddlScheduleType.SelectedValue = "Schedule Recurring";
            pnlRecurrence.Visible = true;
            ddlRecurrence.SelectedIndex = NoIndex;
            ddlRecurrence.SelectedValue = RecurrenceMonthly;
            pnlMonth.Visible = true;
            txtMonth.Text = "1";
            pnlStart.Visible = true;
            pnlEnd.Visible = true;
            pnlNumberToSend.Visible = true;
            txtNumberToSend.Text = "";
            pnlNumberToSendType.Visible = true;
            ddlNumberToSendType.Items.Clear();
            ddlNumberToSendType.Items.Add(NumberTypePercent);
            ddlNumberToSendType.Items.Add(NumberTypeNumber);
            if (CanSendAll && !ddlNumberToSendType.Items.Contains(new ListItem(NumberToSendAll)))
            {
                ddlNumberToSendType.Items.Add(NumberToSendAll);
                ddlNumberToSendType.SelectedValue = NumberToSendAll;
            }
            if (CanSendAll)
            {
                txtNumberToSend.Enabled = false;
                rfvNumberToSend.Enabled = false;
                rvNumberToSend.Enabled = false;
            }
            else
            {
                ddlNumberToSendType.SelectedValue = NumberTypeNumber;
                txtNumberToSend.Enabled = true;
                rfvNumberToSend.Enabled = true;
                rvNumberToSend.Enabled = true;
                SetMaxAmount(NumberTypeNumber);
            }
        }

        protected void rblTestBlast_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isTestBlast = false;
            if (rblTestBlast.SelectedValue.Equals("Yes"))
            {
                HideAllPanels();
                pnlTestBlast.Visible = true;
                pnlTextBlast.Visible = true;
                Session["SelectedTestGroups_List"] = null;

                if (KMPlatform.BusinessLogic.Client.HasServiceFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.PlatformClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreview))
                {
                    pnlEmailPreview.Visible = true;
                }
                isTestBlast = true;
                lblSocialMessage.Visible = true;

            }
            else
            {
                SetupInitialLoad();
                isTestBlast = false;
                pnlTextBlast.Visible = false;
                lblSocialMessage.Visible = false;

            }

            if (checkedHandler != null)
                checkedHandler(isTestBlast);
        }

        protected void ddlNumberToSendType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisableAllValidators();
            if (EqualsOrdinal(ddlNumberToSendType.SelectedValue, NumberTypePercent))
            {
                SetControlsForType(true);
            }
            else if (EqualsOrdinal(ddlNumberToSendType.SelectedValue, NumberTypeNumber))
            {
                SetControlsForType(false);
            }
            else
            {
                txtNumberToSend.Enabled = false;
            }
        }

        private void SetControlsForType(bool isNumber)
        {
            SetMaxAmount(isNumber ? NumberTypeNumber : NumberTypePercent);
            switch (ddlScheduleType.SelectedValue)
            {
                case ScheduleTypeSendNow:
                    txtNumberToSend.Enabled = true;
                    rfvNumberToSend.Enabled = true;
                    rvNumberToSend.Enabled = true;
                    if (isNumber)
                    {
                        rvNumberToSend.MaximumValue = _MaxSendAmount;
                    }
                    break;
                case ScheduleTypeOneTime:
                    if (EqualsOrdinal(ddlSplitType.SelectedValue, SplitSingleDay))
                    {
                        txtNumberToSend.Enabled = true;
                        rfvNumberToSend.Enabled = true;
                        rvNumberToSend.Enabled = true;
                        if (isNumber)
                        {
                            rvNumberToSend.MaximumValue = _MaxSendAmount;
                        }
                    }
                    else if (EqualsOrdinal(ddlSplitType.SelectedValue, SplitManually))
                    {
                        EnableValidatorsOnChecked();
                        if (isNumber)
                        {
                            SetMaximimValidatorsValueOnChecked();
                        }
                    }

                    break;
                case ScheduleTypeRecurring:
                    if (EqualsOrdinal(ddlRecurrence.SelectedValue, RecurrenceDaily) ||
                        EqualsOrdinal(ddlRecurrence.SelectedValue, RecurrenceMonthly))
                    {
                        txtNumberToSend.Enabled = true;
                        rfvNumberToSend.Enabled = true;
                        rvNumberToSend.Enabled = true;
                        rvNumberToSend.MaximumValue = isNumber ? _MaxSendAmount : _MaxSendPercent;
                    }
                    else if (EqualsOrdinal(ddlRecurrence.SelectedValue, RecurrenceWeekly))
                    {
                        if (EqualsOrdinal(ddlSplitType.SelectedValue, SplitManually))
                        {
                            EnableValidatorsOnChecked();
                            if (isNumber)
                            {
                                SetMaximimValidatorsValueOnChecked();
                            }
                        }
                    }
                    break;
            }
        }

        private bool EqualsOrdinal(string str0, string str1)
        {
            var isEqual = string.Equals(str0, str1, StringComparison.OrdinalIgnoreCase);
            return isEqual;
        }

        private void SetMaximimValidatorsValueOnChecked()
        {
            foreach (var checkValidators in GetCheckValidatersList())
            {
                if (checkValidators.CheckBox.Checked)
                {
                    checkValidators.RangeValidator.MaximumValue = _MaxSendAmount;
                }
            }
        }

        private void EnableValidatorsOnChecked()
        {
            foreach (var checkValidators in GetCheckValidatersList())
            {
                if (checkValidators.CheckBox.Checked)
                {
                    checkValidators.RequiredFieldValidator.Enabled = true;
                    checkValidators.RangeValidator.Enabled = true;
                }
            }
        }

        private void SetupDaysControls(bool cbxChecked = false, bool txtEnabled = false, List<ECN_Framework_Entities.Communicator.BlastScheduleDays> days = null)
        {
            List<CheckBox> cbxList = new List<CheckBox>();
            List<TextBox> txtList = new List<TextBox>();

            cbxList.AddRange(new CheckBox[] { cbxDay1, cbxDay2, cbxDay3, cbxDay4, cbxDay5, cbxDay6, cbxDay7 });
            txtList.AddRange(new TextBox[] { txtDay1, txtDay2, txtDay3, txtDay4, txtDay5, txtDay6, txtDay7 });

            foreach (CheckBox bx in cbxList)
            {
                bx.Checked = cbxChecked;
            }

            foreach (TextBox txt in txtList)
            {
                txt.Text = "";
                txt.Enabled = txtEnabled;
            }


        }

        private void SetMaxAmount(string type)
        {
            if (type == NumberTypePercent)
            {
                rvNumberToSend.MaximumValue = _MaxSendPercent;
                rvDay1.MaximumValue = _MaxSendPercent;
                rvDay2.MaximumValue = _MaxSendPercent;
                rvDay3.MaximumValue = _MaxSendPercent;
                rvDay4.MaximumValue = _MaxSendPercent;
                rvDay5.MaximumValue = _MaxSendPercent;
                rvDay6.MaximumValue = _MaxSendPercent;
                rvDay7.MaximumValue = _MaxSendPercent;
            }
            else
            {
                rvNumberToSend.MaximumValue = _MaxSendAmount;
                rvDay1.MaximumValue = _MaxSendAmount;
                rvDay2.MaximumValue = _MaxSendAmount;
                rvDay3.MaximumValue = _MaxSendAmount;
                rvDay4.MaximumValue = _MaxSendAmount;
                rvDay5.MaximumValue = _MaxSendAmount;
                rvDay6.MaximumValue = _MaxSendAmount;
                rvDay7.MaximumValue = _MaxSendAmount;
            }
        }

        protected void ddlScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlScheduleType.SelectedValue)
            {
                case ScheduleTypeSendNow:
                    SetupSendNow();
                    rfvStartDate.Enabled = false;
                    rfvStartTime.Enabled = false;
                    rfvEndDate.Enabled = false;
                    if (resetHandler != null)
                        resetHandler();
                    break;
                case ScheduleTypeOneTime:
                    SetupOneTime();
                    rfvStartDate.Enabled = true;
                    rfvStartTime.Enabled = true;
                    rfvEndDate.Enabled = false;
                    if (resetHandler != null)
                        resetHandler();
                    break;

                case ScheduleTypeRecurring:
                    SetupRecurringSingleDay();
                    rfvStartDate.Enabled = true;
                    rfvStartTime.Enabled = true;
                    rfvEndDate.Enabled = true;
                    if (resetHandler != null)
                        resetHandler();
                    break;
                case "--SELECT--":
                    SetupInitialLoad();
                    rfvStartDate.Enabled = false;
                    rfvStartTime.Enabled = false;
                    rfvEndDate.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        protected void ddlSplitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlScheduleType.SelectedValue)
            {
                case ScheduleTypeSendNow:
                    DisableAllValidators();
                    if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
                    {
                        rfvNumberToSend.Enabled = true;
                        rvNumberToSend.Enabled = true;
                        SetMaxAmount(ddlNumberToSendType.SelectedValue);
                    }
                    break;
                case ScheduleTypeOneTime:
                    switch (ddlSplitType.SelectedValue)
                    {
                        case SplitSingleDay:
                            SetupOneTime();
                            break;
                        case SplitEvenly:
                            SetupEvenlySplit();
                            break;
                        case SplitManually:
                            SetupManuallySplit();
                            break;
                        default:
                            break;
                    }
                    break;
                case ScheduleTypeRecurring:
                    if (ddlRecurrence.SelectedValue == RecurrenceWeekly)
                    {
                        switch (ddlSplitType.SelectedValue)
                        {
                            case SplitEvenly:
                                SetupRecurringWeeklyEvenlySplit();
                                break;
                            case SplitManually:
                                SetupRecurringWeeklyManuallySplit();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected void Days_CheckedChanged(object sender, EventArgs e)
        {
            DisableAllValidators();
            if (ddlSplitType.SelectedValue == SplitManually)
            {
                if (cbxDay1.Checked)
                {
                    txtDay1.Enabled = true;
                    rfvDay1.Enabled = true;
                    rvDay1.Enabled = true;
                }
                else
                {
                    txtDay1.Text = "";
                    txtDay1.Enabled = false;
                }
                if (cbxDay2.Checked)
                {
                    txtDay2.Enabled = true;
                    rfvDay2.Enabled = true;
                    rfvDay2.Enabled = true;
                }
                else
                {
                    txtDay2.Text = "";
                    txtDay2.Enabled = false;
                }
                if (cbxDay3.Checked)
                {
                    txtDay3.Enabled = true;
                    rfvDay3.Enabled = true;
                    rvDay3.Enabled = true;
                }
                else
                {
                    txtDay3.Text = "";
                    txtDay3.Enabled = false;
                }
                if (cbxDay4.Checked)
                {
                    txtDay4.Enabled = true;
                    rfvDay4.Enabled = true;
                    rvDay4.Enabled = true;
                }
                else
                {
                    txtDay4.Text = "";
                    txtDay4.Enabled = false;
                }
                if (cbxDay5.Checked)
                {
                    txtDay5.Enabled = true;
                    rfvDay5.Enabled = true;
                    rvDay5.Enabled = true;
                }
                else
                {
                    txtDay5.Text = "";
                    txtDay5.Enabled = false;
                }
                if (cbxDay6.Checked)
                {
                    txtDay6.Enabled = true;
                    rfvDay6.Enabled = true;
                    rvDay6.Enabled = true;
                }
                else
                {
                    txtDay6.Text = "";
                    txtDay6.Enabled = false;
                }
                if (cbxDay7.Checked)
                {
                    txtDay7.Enabled = true;
                    rfvDay7.Enabled = true;
                    rvDay7.Enabled = true;
                }
                else
                {
                    txtDay7.Text = "";
                    txtDay7.Enabled = false;
                }
            }
        }

        protected void ddlRecurrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlRecurrence.SelectedValue)
            {
                case RecurrenceDaily:
                    SetupRecurringSingleDay();
                    rfvWeeks.Enabled = false;
                    rvWeeks.Enabled = false;
                    rfvMonth.Enabled = false;
                    rvMonth.Enabled = false;
                    break;
                case RecurrenceWeekly:
                    SetupRecurringWeeklyEvenlySplit();
                    rfvWeeks.Enabled = true;
                    rvWeeks.Enabled = true;
                    rfvMonth.Enabled = false;
                    rvMonth.Enabled = false;
                    break;
                case RecurrenceMonthly:
                    SetupRecurringMonthly();
                    rfvWeeks.Enabled = false;
                    rvWeeks.Enabled = false;
                    rfvMonth.Enabled = true;
                    rvMonth.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        public void SetupWizard(bool isTestBlast = false)
        {
            _MaxSendAmount = "1000000";
            _MaxSendPercent = "100";
            SetupInitialLoad(isTestBlast);
            //HideAllPanels();
            rfvStartDate.Enabled = false;
            rfvStartTime.Enabled = false;
            rfvEndDate.Enabled = false;
            rfvWeeks.Enabled = false;
            rvWeeks.Enabled = false;
            rfvMonth.Enabled = false;
            rvMonth.Enabled = false;
        }

        public ECN_Framework_Entities.Communicator.BlastSetupInfo SetupSchedule(string blastType)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;

            pnlErrorMessage.Visible = false;
            List<string> errorList = ValidateSchedule();
            if (errorList.Count > 0)
            {
                pnlErrorMessage.Visible = true;
                blErrorMessage.DataSource = errorList;
                blErrorMessage.DataBind();
            }
            else
            {
                setupInfo = CreateSchedule(blastType);
            }
            return setupInfo;
        }

        public ECN_Framework_Entities.Communicator.BlastSetupInfo SetupUpdateSchedule(string blastType)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;
            if (RequestBlastID > 0)
            {
                pnlErrorMessage.Visible = false;
                List<string> errorList = ValidateSchedule();
                if (errorList.Count > 0)
                {
                    pnlErrorMessage.Visible = true;
                    blErrorMessage.DataSource = errorList;
                    blErrorMessage.DataBind();
                }
                else
                {
                    setupInfo = UpdateSchedule(blastType);
                }
            }
            return setupInfo;
        }

        private void CreateSendNow(ref ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo, string blastType)
        {
            setupInfo = new ECN_Framework_Entities.Communicator.BlastSetupInfo();
            setupInfo.ScheduleType = ScheduleTypeSendNow;
            if (rblTestBlast.SelectedValue.Equals("Yes"))
            {
                setupInfo.IsTestBlast = true;
                if (rblTextBlast.SelectedValue.ToLower().Equals("true"))
                    setupInfo.SendTextTestBlast = true;
                else
                    setupInfo.SendTextTestBlast = false;
            }
            else
            {
                setupInfo.IsTestBlast = false;
            }
            setupInfo.SendTime = DateTime.Now.AddSeconds(15);
            setupInfo.BlastFrequency = BlastFrequencyOneTime;

            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                setupInfo.SendNowAmount = Convert.ToInt32(txtNumberToSend.Text);
                if (ddlNumberToSendType.SelectedValue == NumberTypeNumber)
                {
                    setupInfo.SendNowIsAmount = true;
                }
                else
                {
                    setupInfo.SendNowIsAmount = false;
                }
            }
            else if (blastType.ToLower().Equals("ab") && ddlNumberToSendType.SelectedValue == NumberToSendAll)
            {
                setupInfo.SendNowIsAmount = false;
                setupInfo.SendNowAmount = 100;
            }
        }

        private void CreateOneTime(ref BlastSetupInfo setupInfo, string blastType)
        {
            var schedule = new BlastSchedule { SchedStartDate = txtStartDate.Text };

            var tempEnd = Convert.ToDateTime($"{txtStartDate.Text} {txtStartTime.Text}");
            tempEnd = tempEnd.AddDays(7);
            schedule.SchedEndDate = tempEnd.ToString("MM/dd/yyyy");

            schedule.Period = SchedulePeriodO;
            schedule.CreatedBy = ECNSession.CurrentSession().CurrentUser.UserID;
            schedule.UpdatedBy = ECNSession.CurrentSession().CurrentUser.UserID;
            schedule.SchedTime = txtStartTime.Text;
            var daysList = new List<BlastScheduleDays>();
            schedule.SplitType = getSplitType(ddlSplitType.Text);

            switch (ddlSplitType.SelectedValue)
            {
                case SplitSingleDay:
                    setupInfo = OneTimeCreateSplitSingleDay(setupInfo, blastType, daysList, schedule);
                    break;
                case SplitEvenly:
                    setupInfo = OneTimeCreateSplitEvenly(setupInfo, schedule, daysList);
                    break;
                case SplitManually:
                    setupInfo = OneTimeCreateSplitManually(setupInfo, schedule, daysList);
                    break;
            }
        }

        private BlastSetupInfo OneTimeCreateSplitEvenly(BlastSetupInfo setupInfo, BlastSchedule schedule, List<BlastScheduleDays> daysList)
        {
            const int totalPercent = 100;
            var totalDays = DayControls.Count(dayControl => dayControl.Value.Item2.Checked);
            var retDates = GetFirstDate();
            var firstDate = retDates[0];
            schedule.SchedEndDate = retDates[1].ToString("MM/dd/yyyy");
            var indivPercent = totalPercent / totalDays;
            var remainder = totalPercent % totalDays;
            var currentDay = 0;

            foreach (var dayControl in DayControls.Reverse())
            {
                if (!dayControl.Value.Item2.Checked)
                {
                    continue;
                }

                var days = new BlastScheduleDays { IsAmount = false };
                if (++currentDay == totalDays)
                {
                    days.Total = indivPercent + remainder;
                }
                else
                {
                    days.Total = indivPercent;
                }

                days.DayToSend = dayControl.Key;
                daysList.Add(days);
            }

            schedule.DaysList = daysList;
            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId <= 0)
            {
                return setupInfo;
            }

            setupInfo = CreateBlastSetupInfoOneTime(blastScheduleId);
            setupInfo.SendTime = Convert.ToDateTime($"{firstDate:MM/dd/yyyy} {txtStartTime.Text}");
            setupInfo.SendNowIsAmount = false;
            setupInfo.SendNowAmount = indivPercent;

            return setupInfo;
        }

        private BlastSetupInfo OneTimeCreateSplitManually(BlastSetupInfo setupInfo, BlastSchedule schedule, List<BlastScheduleDays> daysList)
        {
            var firstAmount = 0;
            var retDates = GetFirstDate();
            var firstDate = retDates[0];
            schedule.SchedEndDate = retDates[1].ToString("MM/dd/yyyy");

            foreach (var dayControl in DayControls)
            {
                if (!dayControl.Value.Item2.Checked)
                {
                    continue;
                }

                if ((int)firstDate.DayOfWeek == dayControl.Key)
                {
                    firstAmount = Convert.ToInt32(dayControl.Value.Item1.Text);
                }

                var days = new BlastScheduleDays
                {
                    IsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber,
                    Total = Convert.ToInt32(dayControl.Value.Item1.Text),
                    DayToSend = dayControl.Key
                };

                daysList.Add(days);
            }

            schedule.DaysList = daysList;
            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId <= 0)
            {
                return setupInfo;
            }

            setupInfo = CreateBlastSetupInfoOneTime(blastScheduleId);
            setupInfo.SendTime = Convert.ToDateTime($"{firstDate:MM/dd/yyyy} {txtStartTime.Text}");
            setupInfo.SendNowIsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber;
            setupInfo.SendNowAmount = firstAmount;

            return setupInfo;
        }

        private BlastSetupInfo OneTimeCreateSplitSingleDay(
            BlastSetupInfo setupInfo,
            string blastType,
            List<BlastScheduleDays> daysList,
            BlastSchedule schedule)
        {
            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                var days = new BlastScheduleDays
                {
                    Total = Convert.ToInt32(txtNumberToSend.Text),
                    IsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber
                };
                daysList.Add(days);
                schedule.DaysList = daysList;
            }
            else if (blastType.Equals(BlastTypeAb, StringComparison.OrdinalIgnoreCase)
                     && ddlNumberToSendType.SelectedValue == NumberToSendAll)
            {
                setupInfo.SendNowIsAmount = false;
                setupInfo.SendNowAmount = SetupInfoSendNowAmount100;
            }

            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId <= 0)
            {
                return setupInfo;
            }

            setupInfo = CreateBlastSetupInfoOneTime(blastScheduleId);
            CheckForNumberToSentAll(setupInfo);

            return setupInfo;
        }

        private List<DateTime> GetFirstDate()
        {
            DateTime? firstDate = null;

            DateTime startDate = new DateTime();
            DateTime.TryParse(txtStartDate.Text, out startDate);

            DateTime tempDate = new DateTime();
            // DateTime? endDate = new DateTime();
            DateTime.TryParse(txtEndDate.Text, out tempDate);
            DateTime? endDate = tempDate;


            int startDayOfWeek = (int)startDate.DayOfWeek;
            List<FindDate> dates = new List<FindDate>();
            //generic method to get check box data
            Action<CheckBox, int> addDate = (CheckBox cb, int dayofWeek) =>
                {
                    if (cb.Checked)
                    {
                        FindDate fd = new FindDate();
                        fd.Checked = true;
                        fd.DayOfWeek = dayofWeek;
                        dates.Add(fd);
                    }
                };
            //loop to run through Day Checkboxes and call above method
            int ind = 0;
            foreach (var cb in new CheckBox[] {cbxDay1, cbxDay2, cbxDay3, cbxDay4, cbxDay5, cbxDay6, cbxDay7 })
            {
                addDate(cb, ind++);
            }

            //loop through found dates and figure out first date and end date
            List<FindDate> datesForEnd = new List<FindDate>();
            foreach (FindDate fd in dates.OrderByDescending(x => x.DayOfWeek))
            {
                FindDate fdEnd = new FindDate();
                fdEnd.DayOfWeek = fd.DayOfWeek;
                if (!firstDate.HasValue || ((int)(firstDate.Value.Date - startDate.Date).TotalDays > 0
                                            && startDayOfWeek <= fd.DayOfWeek))
                {
                    if (startDayOfWeek > fd.DayOfWeek)
                    {
                        firstDate = startDate.AddDays((fd.DayOfWeek - startDayOfWeek) + 7);

                    }
                    else
                        firstDate = startDate.AddDays(fd.DayOfWeek - startDayOfWeek);
                    fdEnd.ActualDate = firstDate.Value;
                }
                else
                {
                    fdEnd.ActualDate = startDate.AddDays((fd.DayOfWeek - startDayOfWeek) + 7);
                }

                datesForEnd.Add(fdEnd);
            }

            endDate = datesForEnd.OrderByDescending(x => x.ActualDate).First().ActualDate;// datesForEnd.OrderBy(x => x.ActualDate).Last().ActualDate;

            List<DateTime> retDates = new List<DateTime>();
            retDates.Add(firstDate.Value);
            retDates.Add(endDate.Value);

            return retDates;

        }

        private string getSplitType(string splitTypeFull = "")
        {
            if (!string.IsNullOrEmpty(splitTypeFull))
            {
                switch (splitTypeFull)
                {
                    case SplitManually:
                        return SplitTypeMonth;
                        break;

                    case SplitEvenly:
                        return SplitTypeEvenly;
                        break;

                    case SplitSingleDay:
                        return "s";
                        break;

                    default: return String.Empty;
                }
            }
            return String.Empty;
        }

        private void CreateRecurring(ref BlastSetupInfo setupInfo, string blastType)
        {
            var schedule = CreateBlastSchedule();

            var daysList = new List<BlastScheduleDays>();
            switch (ddlRecurrence.SelectedValue)
            {
                case RecurrenceDaily:
                    setupInfo = CreateRecurrenceDaily(setupInfo, blastType, schedule, daysList);
                    break;
                case RecurrenceMonthly:
                    setupInfo = CreateReccerenceMonthly(setupInfo, blastType, schedule, daysList);
                    break;
                case RecurrenceWeekly:
                    setupInfo = CreateReccurenceWeekly(setupInfo, schedule, daysList);
                    break;
            }
        }

        private BlastSetupInfo CreateReccurenceWeekly(BlastSetupInfo setupInfo, BlastSchedule schedule, List<BlastScheduleDays> daysList)
        {
            schedule.Period = SchedulePeriodWeek;
            if (ddlSplitType.SelectedValue == SplitEvenly)
            {
                return CreateSplitEvenly(setupInfo, schedule, daysList);
            }

            if (ddlSplitType.SelectedValue == SplitManually)
            {
                CreateSplitManually(ref setupInfo, schedule, daysList);
            }

            return setupInfo;
        }

        private void CreateSplitManually(ref BlastSetupInfo setupInfo, BlastSchedule schedule, List<BlastScheduleDays> daysList)
        {
            var firstAmount = 0;
            var retDatesManual = GetFirstDate();
            var firstDate = retDatesManual[0];

            foreach (var dayControl in DayControls)
            {
                if (dayControl.Value.Item2.Checked)
                {
                    firstAmount = GetFirstAmount(
                        daysList,
                        firstDate,
                        firstAmount,
                        dayControl.Key,
                        dayControl.Value.Item1.Text);
                }
            }

            schedule.DaysList = daysList;
            schedule.SchedEndDate = txtEndDate.Text;
            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId > 0)
            {
                setupInfo = CreateBlastSetupInfoReccuringForAmount(blastScheduleId, firstDate, firstAmount);
            }
        }

        private BlastSetupInfo CreateBlastSetupInfoReccuringForAmount(
            int blastScheduleId,
            DateTime firstDate,
            int amount)
        {
            return new BlastSetupInfo
            {
                ScheduleType = ScheduleTypeRecurring,
                IsTestBlast = false,
                BlastScheduleID = blastScheduleId,
                BlastFrequency = BlastFrequencyReccuring,
                SendTime = Convert.ToDateTime($"{firstDate:MM/dd/yyyy} {txtStartTime.Text}"),
                SendNowIsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber,
                SendNowAmount = amount
            };
        }

        private int GetFirstAmount(
            ICollection<BlastScheduleDays> daysList,
            DateTime firstDate,
            int firstAmount,
            int daysToSend,
            string firstAmountText)
        {
            if ((int)firstDate.DayOfWeek == daysToSend)
            {
                firstAmount = Convert.ToInt32(firstAmountText);
            }

            var days = new BlastScheduleDays
            {
                IsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber,
                Total = Convert.ToInt32(firstAmountText),
                DayToSend = daysToSend,
                Weeks = Convert.ToInt32(txtWeeks.Text)
            };

            daysList.Add(days);

            return firstAmount;
        }

        private BlastSetupInfo CreateSplitEvenly(
            BlastSetupInfo setupInfo,
            BlastSchedule schedule,
            List<BlastScheduleDays> daysList)
        {
            var totalDays = 0;
            var dayBoxes = new List<CheckBox>() { cbxDay1, cbxDay2, cbxDay3, cbxDay4, cbxDay5, cbxDay6, cbxDay7 };
            foreach (var dayCheckBox in dayBoxes)
            {
                if (dayCheckBox.Checked)
                {
                    totalDays++;
                }
            }

            var retDatesEvenly = GetFirstDate();
            var firstDate = retDatesEvenly[0];
            schedule.SchedEndDate = txtEndDate.Text;
            var indivPercent = 100 / totalDays;
            var remainder = 100 % totalDays;
            var currentDay = 0;

            for (var dayIndex = 0; dayIndex < dayBoxes.Count; dayIndex++)
            {
                if (dayBoxes[dayIndex].Checked)
                {
                    currentDay = AddDaysToCurrentDay(
                        daysList,
                        currentDay,
                        totalDays,
                        indivPercent,
                        remainder,
                        dayIndex);
                }
            }

            schedule.DaysList = daysList;
            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            return blastScheduleId > 0
                       ? CreateBlastSetupInfoReccuringForAmount(blastScheduleId, firstDate, indivPercent)
                       : setupInfo;
        }

        private int AddDaysToCurrentDay(
            ICollection<BlastScheduleDays> daysList,
            int currentDay,
            int totalDays,
            int indivPercent,
            int remainder,
            int daysToSend)
        {
            var days = new BlastScheduleDays
            {
                IsAmount = false,
                Weeks = Convert.ToInt32(txtWeeks.Text)
            };

            if (++currentDay == totalDays)
            {
                days.Total = indivPercent + remainder;
            }
            else
            {
                days.Total = indivPercent;
            }

            days.DayToSend = daysToSend;
            daysList.Add(days);
            return currentDay;
        }

        private BlastSetupInfo CreateReccerenceMonthly(
            BlastSetupInfo setupInfo,
            string blastType,
            BlastSchedule schedule,
            List<BlastScheduleDays> daysList)
        {
            var setupBlastInfo = setupInfo;

            schedule.Period = SplitTypeMonth;
            var days = new BlastScheduleDays { DayToSend = Convert.ToInt32(txtMonth.Text) };
            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                days.Total = Convert.ToInt32(txtNumberToSend.Text);
                days.IsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber;
            }
            else if (blastType.Equals(BlastTypeAb, StringComparison.InvariantCultureIgnoreCase)
                     && ddlNumberToSendType.SelectedValue == NumberToSendAll)
            {
                setupBlastInfo.SendNowIsAmount = false;
                setupBlastInfo.SendNowAmount = SetupInfoSendNowAmount100;
            }

            daysList.Add(days);
            schedule.DaysList = daysList;
            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId > 0)
            {
                setupBlastInfo = new BlastSetupInfo
                {
                    ScheduleType = ScheduleTypeRecurring,
                    IsTestBlast = false,
                    BlastScheduleID = blastScheduleId,
                    BlastFrequency = BlastFrequencyReccuring
                };

                var startDate = Convert.ToDateTime($"{txtStartDate.Text} {txtStartTime.Text}");
                var selectedDate = startDate;
                if (txtMonth.Text == FakeMonthNUmber)
                {
                    selectedDate = Convert.ToDateTime(
                        $"{startDate.Month}/{DateTime.DaysInMonth(startDate.Year, startDate.Month)}/{startDate.Year} {txtStartTime.Text}");
                }
                else
                {
                    if (startDate.Day < Convert.ToInt32(txtMonth.Text))
                    {
                        var year = startDate.Year;
                        var month = startDate.Month;
                        selectedDate = Convert.ToInt32(txtMonth.Text) <= DateTime.DaysInMonth(year, month)
                                           ? Convert.ToDateTime($"{month}/{txtMonth.Text}/{year} {txtStartTime.Text}")
                                           : GetSelectedDate(month, year, selectedDate);
                    }
                    else if (startDate.Day > Convert.ToInt32(txtMonth.Text))
                    {
                        var year = startDate.Year;
                        var month = startDate.Month;
                        selectedDate = GetSelectedDate(month, year, selectedDate);
                    }
                }

                setupBlastInfo.SendTime = selectedDate;
                CheckForNumberToSentAll(setupBlastInfo);
            }

            return setupBlastInfo;
        }

        private DateTime GetSelectedDate(int month, int year, DateTime selectedDate)
        {
            for (var i = 1; i < 13; i++)
            {
                month++;
                if (month > 12)
                {
                    year++;
                    month = 1;
                }

                if (Convert.ToInt32(txtMonth.Text) <= DateTime.DaysInMonth(year, month))
                {
                    selectedDate = Convert.ToDateTime($"{month}/{txtMonth.Text}/{year} {txtStartTime.Text}");
                    break;
                }
            }

            return selectedDate;
        }

        private BlastSetupInfo CreateRecurrenceDaily(
            BlastSetupInfo setupInfo,
            string blastType,
            BlastSchedule schedule,
            List<BlastScheduleDays> daysList)
        {
            schedule.Period = SchedulePeriodDay;
            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                var days = new BlastScheduleDays
                {
                    Total = Convert.ToInt32(txtNumberToSend.Text),
                    IsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber
                };

                daysList.Add(days);
                schedule.DaysList = daysList;
            }
            else if (blastType.Equals(BlastTypeAb, StringComparison.InvariantCultureIgnoreCase)
                     && ddlNumberToSendType.SelectedValue == NumberToSendAll)
            {
                setupInfo.SendNowIsAmount = false;
                setupInfo.SendNowAmount = SetupInfoSendNowAmount100;
            }

            var blastScheduleId = RequestBlastID > 0
                                      ? Update(schedule, RequestBlastID)
                                      : Insert(schedule);

            if (blastScheduleId > 0)
            {
                var setupBlastInfo = CreateBlastSetupInfoReccuring(blastScheduleId);
                CheckForNumberToSentAll(setupBlastInfo);

                return setupBlastInfo;
            }

            return setupInfo;
        }

        private BlastSetupInfo CreateBlastSetupInfoReccuring(int blastScheduleId)
        {
            return CreateBlastSetupInfo(blastScheduleId, ScheduleTypeRecurring, BlastFrequencyReccuring);
        }

        private BlastSetupInfo CreateBlastSetupInfoOneTime(int blastScheduleId)
        {
            return CreateBlastSetupInfo(blastScheduleId, ScheduleTypeOneTime, BlastFrequencyOneTime);
        }

        private BlastSetupInfo CreateBlastSetupInfo(int blastScheduleId, string scheduleType, string blastFrequency)
        {
            return new BlastSetupInfo
                   {
                       ScheduleType = scheduleType,
                       IsTestBlast = false,
                       BlastScheduleID = blastScheduleId,
                       BlastFrequency = blastFrequency,
                       SendTime = Convert.ToDateTime($"{txtStartDate.Text} {txtStartTime.Text}")
                   };
        }

        private void CheckForNumberToSentAll(BlastSetupInfo setupInfo)
        {
            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                setupInfo.SendNowAmount = Convert.ToInt32(txtNumberToSend.Text);
                setupInfo.SendNowIsAmount = ddlNumberToSendType.SelectedValue == NumberTypeNumber;
            }
            else if (setupInfo.BlastType.Equals(BlastTypeAb, StringComparison.OrdinalIgnoreCase)
                     && ddlNumberToSendType.SelectedValue == NumberToSendAll)
            {
                setupInfo.SendNowIsAmount = false;
                setupInfo.SendNowAmount = SetupInfoSendNowAmount100;
            }
        }

        private BlastSchedule CreateBlastSchedule()
        {
            return new BlastSchedule
            {
                SchedStartDate = txtStartDate.Text,
                SchedEndDate = txtEndDate.Text,
                CreatedBy = ECNSession.CurrentSession().CurrentUser.UserID,
                UpdatedBy = ECNSession.CurrentSession().CurrentUser.UserID,
                SchedTime = txtStartTime.Text,
                SplitType = getSplitType(ddlSplitType.Text)
            };
        }

        private BlastSetupInfo CreateSchedule(string blastType)
        {
            BlastSetupInfo setupInfo = null;
            if (rblTestBlast.SelectedValue.ToLower().Equals("no"))
            {
                switch (ddlScheduleType.SelectedValue)
                {
                    case ScheduleTypeSendNow:
                        CreateSendNow(ref setupInfo, blastType);
                        break;
                    case ScheduleTypeOneTime:
                        CreateOneTime(ref setupInfo, blastType);
                        break;
                    case ScheduleTypeRecurring:
                        CreateRecurring(ref setupInfo, blastType);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                CreateSendNow(ref setupInfo, blastType);
            }

            return setupInfo;
        }

        private ECN_Framework_Entities.Communicator.BlastSetupInfo UpdateSchedule(string blastType)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;
            switch (ddlScheduleType.SelectedValue)
            {
                case ScheduleTypeOneTime:
                    CreateOneTime(ref setupInfo, blastType);
                    break;
                case ScheduleTypeRecurring:
                    CreateRecurring(ref setupInfo, blastType);
                    break;
                default:
                    break;
            }

            return setupInfo;
        }

        private void ValidateSendNow(ref List<string> errorList)
        {
            if (rblTestBlast.SelectedValue.Equals("No"))
            {
                if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
                {
                    int numberToSend = 0;
                    if (!int.TryParse(txtNumberToSend.Text, out numberToSend))
                    {
                        errorList.Add(ErrorAddValidNumberToSend);
                    }
                    else
                    {
                        if (ddlNumberToSendType.SelectedValue == NumberTypePercent && numberToSend > 100)
                        {
                            errorList.Add("Please enter a percentage less than or equal to 100.");
                        }
                    }
                }
            }
        }

        private void ValidateOneTime(List<string> errorList)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            ValidateStartDate(errorList);

            switch (ddlSplitType.SelectedValue)
            {
                case SplitSingleDay:
                    ValidateRecurringSelectedDaily(errorList);
                    break;
                case SplitEvenly:
                    if (!string.Equals(
                            ddlNumberToSendType.SelectedValue,
                            NumberToSendAll,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        if (NoDayChecked())
                        {
                            errorList.Add(ErrorSeelctAnyDay);
                        }
                    }
                    break;
                case SplitManually:
                    if (!string.Equals(
                            ddlNumberToSendType.SelectedValue,
                            NumberToSendAll,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        if (NoDayChecked())
                        {
                            errorList.Add(ErrorSeelctAnyDay);
                        }

                        ValidateDays(errorList);
                    }
                    break;
            }
        }

        private void ValidateRecurring(List<string> errorList)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            ValidateRecuringParseDates(errorList);

            switch (ddlRecurrence.SelectedValue)
            {
                case RecurrenceDaily:
                    ValidateRecurringSelectedDaily(errorList);
                    break;
                case RecurrenceWeekly:
                    ValidateRecurringSelectedWeekly(errorList);
                    break;
                case RecurrenceMonthly:
                    ValidateRecurringSelectedMonthly(errorList);
                    break;
            }
        }

        private void ValidateRecurringSelectedMonthly(ICollection<string> errorList)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                var numberToSend = 0;
                if (!int.TryParse(txtNumberToSend.Text, out numberToSend))
                {
                    errorList.Add(ErrorAddValidNumberToSend);
                }

                if (ddlNumberToSendType.SelectedValue == NumberTypePercent &&
                    numberToSend > Convert.ToInt32(_MaxSendPercent))
                {
                    errorList.Add(string.Format(ErrorAllSelectedDays, _MaxSendPercent));
                }
            }

            int dayToSend = 0;
            if ((!int.TryParse(txtMonth.Text, out dayToSend) || dayToSend > MaxDaysToSend) && 
                dayToSend != FakeMaxDaysToSend)
            {
                errorList.Add(ErrorInvalidDay);
            }
        }

        private void ValidateRecurringSelectedWeekly(ICollection<string> errorList)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            var howManyWeeks = 0;
            if (!int.TryParse(txtWeeks.Text, out howManyWeeks) || howManyWeeks > MaxWekNumber)
            {
                errorList.Add(ErrorWrongWeekNumber);
            }

            if (NoDayChecked())
            {
                errorList.Add(ErrorSeelctAnyDay);
            }

            ValidateManuallySplit(errorList);
        }

        private void ValidateManuallySplit(ICollection<string> errorList)
        {
            if (ddlSplitType.SelectedValue == SplitManually)
            {
                ValidateDays(errorList);
            }
        }

        private void ValidateDays(ICollection<string> errorList)
        {
            if (errorList == null)
            {
                throw new ArgumentNullException(nameof(errorList));
            }

            var totalPercent = 0;
            var showMessage = false;

            var dayCheckBoxes = GetCheckTextBoxValidationPairs();

            foreach (var controlPair in dayCheckBoxes)
            {
                ValidateDayCheckBoxes(
                    controlPair?.CheckBox,
                    controlPair?.TextBox, 
                    ref showMessage,
                    ref totalPercent);
            }

            if (showMessage)
            {
                errorList.Add(ErrorInvalidNumber);
            }

            if (ddlNumberToSendType.SelectedValue == NumberTypePercent &&
                totalPercent > Convert.ToInt32(_MaxSendPercent))
            {
                errorList.Add(string.Format(ErrorAllSelectedDays, _MaxSendPercent));
            }
        }

        private bool NoDayChecked() => !AnyDayChecked();

        private bool AnyDayChecked()
        {
            var anyDayChecked = GetCheckTextBoxValidationPairs()?.Any(p => (p?.CheckBox.Checked == true));
            return (anyDayChecked == true);
        }

        private IEnumerable<CheckTextBoxValidationPair> GetCheckTextBoxValidationPairs()
        {
            var dayCheckBoxes = new[]
            {
                new CheckTextBoxValidationPair(cbxDay1, txtDay1),
                new CheckTextBoxValidationPair(cbxDay2, txtDay2),
                new CheckTextBoxValidationPair(cbxDay3, txtDay3),
                new CheckTextBoxValidationPair(cbxDay4, txtDay4),
                new CheckTextBoxValidationPair(cbxDay5, txtDay5),
                new CheckTextBoxValidationPair(cbxDay6, txtDay6),
                new CheckTextBoxValidationPair(cbxDay7, txtDay7)
            };

            return dayCheckBoxes;
        }

        private IEnumerable<CheckValidaters> GetCheckValidatersList()
        {
            var checkValidatersList = new[]
            {
                new CheckValidaters(cbxDay1, rfvDay1, rvDay1),
                new CheckValidaters(cbxDay2, rfvDay2, rvDay2),
                new CheckValidaters(cbxDay3, rfvDay3, rvDay3),
                new CheckValidaters(cbxDay4, rfvDay4, rvDay4),
                new CheckValidaters(cbxDay5, rfvDay5, rvDay5),
                new CheckValidaters(cbxDay6, rfvDay6, rvDay6),
                new CheckValidaters(cbxDay7, rfvDay7, rvDay7)
            };

            return checkValidatersList;
        }

        private static void ValidateDayCheckBoxes(
            ICheckBoxControl chkBox,
            ITextControl txtBox,
            ref bool showMessage,
            ref int totalPercent)
        {
            if (chkBox == null)
            {
                throw new ArgumentNullException(nameof(chkBox));
            }

            if (chkBox.Checked)
            {
                if (txtBox == null)
                {
                    throw new ArgumentNullException(nameof(txtBox));
                }

                int day;
                if (!int.TryParse(txtBox.Text, out day))
                {
                    showMessage = true;
                }

                totalPercent += day;
            }
        }

        private void ValidateRecurringSelectedDaily(ICollection<string> errorList)
        {
            if (ddlNumberToSendType.SelectedValue != NumberToSendAll)
            {
                int numberToSend = 0;
                if (!int.TryParse(txtNumberToSend.Text, out numberToSend))
                {
                    errorList.Add(ErrorAddValidNumberToSend);
                }

                if (ddlNumberToSendType.SelectedValue == NumberTypePercent && numberToSend > Convert.ToInt32(_MaxSendPercent))
                {
                    errorList.Add("Please make sure all selected days add up to less than or equal to " + _MaxSendPercent +
                                  ".");
                }
            }
        }

        private void ValidateRecuringParseDates(List<string> errorList)
        {
            var tempStart = ValidateStartDate(errorList);

            ValidateEndDate(errorList, tempStart);
        }

        private void ValidateEndDate(ICollection<string> errorList, DateTime tempStart)
        {
            DateTime tempEnd;
            if (DateTime.TryParse(txtEndDate.Text + " " + txtStartTime.Text, out tempEnd))
            {
                if (tempEnd <= tempStart)
                {
                    errorList.Add("Please enter an end date that occurs after your start date and time.");
                }
            }
            else
            {
                errorList.Add("Please enter a valid end date and time.");
            }
        }

        private DateTime ValidateStartDate(ICollection<string> errorList)
        {
            DateTime tempStart;
            if (DateTime.TryParse(txtStartDate.Text + " " + txtStartTime.Text, out tempStart))
            {
                if (tempStart <= DateTime.Now)
                {
                    errorList.Add("Please enter a valid start date and time(current date/time is in the past).");
                }
            }
            else
            {
                errorList.Add("Please enter a valid start date and time.");
            }

            return tempStart;
        }

        private List<string> ValidateSchedule()
        {
            List<string> errorList = new List<string>();
            switch (ddlScheduleType.SelectedValue)
            {
                case ScheduleTypeSendNow:
                    ValidateSendNow(ref errorList);
                    break;
                case ScheduleTypeOneTime:
                    ValidateOneTime(errorList);
                    break;
                case ScheduleTypeRecurring:
                    ValidateRecurring(errorList);
                    break;
                default:
                    break;
            }
            return errorList;
        }

        protected void cbLastDay_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLastDay.Checked)
            {
                txtMonth.Text = FakeMonthNUmber;
                txtMonth.Enabled = false;
            }
            else
            {
                txtMonth.Enabled = true;
                txtMonth.Text = "";
            }
        }

    }
    public class FindDate
    {
        public int DayOfWeek { get; set; }
        public bool Checked { get; set; }
        public DateTime ActualDate { get; set; }

    }
}