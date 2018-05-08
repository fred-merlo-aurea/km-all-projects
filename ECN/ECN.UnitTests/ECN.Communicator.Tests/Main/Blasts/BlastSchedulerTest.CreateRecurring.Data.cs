using System;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KMPlatform.Entity;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class BlastSchedulerTest
    {
        private void CreateTestData1()
        {
            SetUpFakes();

            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = DateTime.Now.ToString();

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Daily", "Daily"));

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlRecurrence.Items.Add(new ListItem("", "Number"));

            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
        }

        private void CreateTestData2()
        {
            SetUpFakes();
            _testEntity.Session[SessionRequestBlastIDKey] = 0;

            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = DateTime.Now.ToString();

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Daily", "Daily"));

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("ALL", "ALL"));

            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
        }

        private void CreateTestData3()
        {
            SetUpFakes();
            _testEntity.Session[SessionRequestBlastIDKey] = 0;
            
            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = new DateTime(2018, 01, 01).ToString();

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Monthly", "Monthly") { Selected = true });

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("Number", "Number"));

            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
            Get<TextBox>(_privateTestObject, "txtMonth").Text = "1";
        }

        private void CreateTestData4()
        {
            SetUpFakes();
            _testEntity.Session[SessionRequestBlastIDKey] = 0;

            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = new DateTime(2018, 01, 01).ToString();

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Monthly", "Monthly") { Selected = true });

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("ALL", "ALL"));

            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
            Get<TextBox>(_privateTestObject, "txtMonth").Text = "10";
        }

        private void CreateTestData5()
        {
            SetUpFakes();
            _testEntity.Session[SessionRequestBlastIDKey] = 0;

            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = new DateTime(2018, 01, 01).ToString();
            Get<TextBox>(_privateTestObject, "txtEndDate").Text = new DateTime(2018, 06, 01).ToString();
            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
            Get<TextBox>(_privateTestObject, "txtWeeks").Text = "4";

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Weekly", "Weekly") { Selected = true });

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("ALL", "ALL"));

            var ddlSplitType = Get<DropDownList>(_privateTestObject, "ddlSplitType");
            ddlSplitType.Items.Add(new ListItem("Evenly Split", "Evenly Split"));

            for (int i = 7; i >= 1; i--)
            {
                Get<CheckBox>(_privateTestObject, $"cbxDay{i}").Checked = true;
            }
        }

        private void CreateTestData6()
        {
            SetUpFakes();
            _testEntity.Session[SessionRequestBlastIDKey] = 0;

            // Page Control SetUp
            var txtStartDate = Get<TextBox>(_privateTestObject, "txtStartDate");
            txtStartDate.Text = new DateTime(2018, 01, 01).ToString();
            Get<TextBox>(_privateTestObject, "txtEndDate").Text = new DateTime(2018, 06, 01).ToString();
            Get<TextBox>(_privateTestObject, "txtNumberToSend").Text = "1";
            Get<TextBox>(_privateTestObject, "txtWeeks").Text = "4";

            var ddlRecurrence = Get<DropDownList>(_privateTestObject, "ddlRecurrence");
            ddlRecurrence.Items.Add(new ListItem("Weekly", "Weekly") { Selected = true });

            var ddlNumberToSendType = Get<DropDownList>(_privateTestObject, "ddlNumberToSendType");
            ddlNumberToSendType.Items.Add(new ListItem("Number", "Number"));

            var ddlSplitType = Get<DropDownList>(_privateTestObject, "ddlSplitType");
            ddlSplitType.Items.Add(new ListItem("Manually Split", "Manually Split"));

            for (int i = 7; i >= 1; i--)
            {
                Get<CheckBox>(_privateTestObject, $"cbxDay{i}").Checked = true;
                Get<TextBox>(_privateTestObject, $"txtDay{i}").Text = $"{i}";
            }
        }

        private void SetUpFakes()
        {
            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User() { UserID = 1 };
            ShimECNSession.CurrentSession = () => shimECNSession.Instance;
            _testEntity.Session[SessionRequestBlastIDKey] = 1;
            ShimBlastSchedule.InsertBlastSchedule = (schedule) =>
            {
                _savedSchedule = schedule;
                _isScheduleInserted = true;
                return 1;
            };
            ShimBlastSchedule.UpdateBlastScheduleInt32 = (schedule, id) =>
            {
                _savedSchedule = schedule;
                _isScheduleUpdated = true;
                return id;
            };
        }
    }
}
