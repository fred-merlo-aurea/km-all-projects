using System;
using System.Web.UI.WebControls;
using ECN_Framework;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEnums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.communicator.main.events
{
    public abstract class TriggerBase: WebPageHelper
    {
        public abstract PlaceHolder PhError { get; }
        public abstract MasterPages.Communicator EcnMaster { get; }
        public abstract RadioButtonList NoopRadioList { get; }
        public abstract TextBox NoopEmailFrom { get; }
        public abstract TextBox NoopReplyTo { get; }
        public abstract TextBox NoopEmailFromName { get; }
        public abstract TextBox NoopPeriod { get; }
        public abstract TextBox NoopTxtHours { get; }
        public abstract TextBox NoopTxtMinutes { get; }
        public abstract RequiredFieldValidator ValNoopEmailFrom { get; }
        public abstract RequiredFieldValidator ValNoopReplyTo { get; }
        public abstract RequiredFieldValidator ValNoopEmailFromName { get; }
        public abstract RequiredFieldValidator ValNoopPeriod { get; }
        public abstract RequiredFieldValidator ValNoopTxtHours { get; }
        public abstract RequiredFieldValidator ValNoopTxtMinutes { get; }
        public abstract RangeValidator RangeValNoopPeriod { get; }
        public abstract RangeValidator RangeValNoopTxtHours { get; }
        public abstract RangeValidator RangeValNoopTxtMinutes { get; }
        public abstract HiddenField EmailSubject { get; }
        public abstract HiddenField NoopEmailSubject { get; }
        public abstract TextBox EmailFromName { get; }
        public abstract TextBox EmailFrom { get; }
        public abstract TextBox ReplyTo { get; }
        public abstract Button CreateButton { get; }
        public abstract TextBox LayoutName { get; }
        public abstract DropDownList Criteria { get; }
        public abstract TextBox Period { get; }
        public abstract TextBox TxtHours { get; }
        public abstract TextBox TxtMinutes { get; }
        protected abstract void ClearNoopControls();

        private const string HelpTitleBlastBanner = "Blast Planner";
        private const string PeriodNone = "0";
        private const string SelectedValueNo = "N";
        private const string SelectedValueYes = "Y";
        private const string CreateButonCaption = "Create Trigger";
        protected int CustomerId;
        protected int UserId;

        protected void PageLoad()
        {
            PhError.Visible = false;
            EcnMaster.CurrentMenuCode = CommunicatorEnums.MenuCode.EVENTS;
            EcnMaster.HelpTitle = HelpTitleBlastBanner;

            CustomerId = EcnMaster.UserSession.CurrentUser.CustomerID;
            UserId = EcnMaster.UserSession.CurrentUser.UserID;

            if (!Page.IsPostBack)
            {
                NoopRadioList.SelectedValue = SelectedValueNo;
            }
        }

        protected virtual void SetNoopControlStatus(bool status)
        {
            NoopEmailFrom.Enabled = status;
            NoopReplyTo.Enabled = status;
            NoopEmailFromName.Enabled = status;
            NoopPeriod.Enabled = status;
            NoopTxtHours.Enabled = status;
            NoopTxtMinutes.Enabled = status;

            ValNoopEmailFrom.Enabled = status;
            ValNoopReplyTo.Enabled = status;
            ValNoopEmailFromName.Enabled = status;

            ValNoopPeriod.Enabled = status;
            ValNoopTxtHours.Enabled = status;
            ValNoopTxtMinutes.Enabled = status;
            RangeValNoopPeriod.Enabled = status;
            RangeValNoopTxtHours.Enabled = status;
            RangeValNoopTxtMinutes.Enabled = status;
        }

        protected void GetBlastFromControlsBase(Blast blast, string strSubject)
        {
            var strEmailFromName = StringFunctions.CleanString(EmailFromName.Text);
            var strEmailFrom = StringFunctions.Remove(EmailFrom.Text, StringFunctions.NonDomain());

            blast.CustomerID = CustomerId;
            blast.CreatedUserID = UserId;
            blast.EmailSubject = strSubject;
            blast.EmailFrom = strEmailFrom;
            blast.EmailFromName = strEmailFromName;
            blast.ReplyTo = StringFunctions.CleanString(ReplyTo.Text);
            blast.SendTime = DateTime.Now;
        }

        protected void LoadLayoutPlanBase(int layoutPlanId)
        {
            var layoutPlan = 
                BusinessCommunicator.LayoutPlans.GetByLayoutPlanID(layoutPlanId, EcnMaster.UserSession.CurrentUser);
            LayoutName.Text = layoutPlan.ActionName;

            Blast blast = BusinessCommunicator.Blast.GetByBlastID(
                layoutPlan.BlastID.Value, 
                EcnMaster.UserSession.CurrentUser, 
                false);

            EmailFrom.Text = blast.EmailFrom;
            ReplyTo.Text = blast.ReplyTo;
            EmailFromName.Text = blast.EmailFromName;
            EmailSubject.Value = blast.EmailSubject;

            var days = TimeSpan.FromDays((double)(layoutPlan.Period ?? 0M));
            Period.Text = days.Days.ToString();
            TxtHours.Text = days.Hours.ToString();
            TxtMinutes.Text = days.Minutes.ToString();

            var triggerPlan = BusinessCommunicator.TriggerPlans.GetByRefTriggerID(
                layoutPlan.BlastID.Value, 
                EcnMaster.UserSession.CurrentUser);

            if (triggerPlan != null)
            {
                NoopRadioList.SelectedValue = SelectedValueYes;
                SetNoopControlStatus(true);
                Blast trigblast = BusinessCommunicator.Blast.GetByBlastID(
                    triggerPlan.BlastID.Value, 
                    EcnMaster.UserSession.CurrentUser, 
                    false);

                NoopEmailFrom.Text = trigblast.EmailFrom;
                NoopReplyTo.Text = trigblast.ReplyTo;
                NoopEmailFromName.Text = trigblast.EmailFromName;
                NoopEmailSubject.Value = trigblast.EmailSubject;

                var trigTs = TimeSpan.FromDays((double)(triggerPlan.Period ?? 0M));
                NoopPeriod.Text = trigTs.Days.ToString();
                NoopTxtHours.Text = trigTs.Hours.ToString();
                NoopTxtMinutes.Text = trigTs.Minutes.ToString();
            }
            else
            {
                NoopRadioList.SelectedValue = SelectedValueNo;
                ClearNoopControls();
                SetNoopControlStatus(false);
            }
        }

        protected void ClearControlsBase()
        {
            CreateButton.Text = CreateButonCaption;

            LayoutName.Text = string.Empty;

            Criteria.SelectedIndex = 0;

            EmailFrom.Text = string.Empty;
            ReplyTo.Text = string.Empty;
            EmailFromName.Text = string.Empty;
            EmailSubject.Value = string.Empty;
            Period.Text = PeriodNone;
        }
    }
}
