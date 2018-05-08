using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.accounts.classes;
using ecn.common.classes;
using ecn.common.classes.billing;
using ecn.communicator.classes;
using ecn.accounts.includes;

namespace ecn.accounts.main.leads
{
    public partial class _default : ECN_Framework.WebPageHelper
    {
        #region Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS;
            Master.SubMenu = "Dashboard";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "Leads Management System";
           
            if (!IsPostBack)
            {
                txtStartDate.Text = TheFirstDayOfTheMonth.ToShortDateString();
                txtEndDate.Text = TheLastDayOfTheMonth.ToShortDateString();
                LoadStaff();
                UpdateDashBoard();
                calCallDate.SelectedDate = DateTime.Now;
                calCallDate_SelectionChanged(null, null);
            }
        }


        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            UpdateDashBoard();
        }


        private void dgdLeads_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
            {
                return;
            }

            Label lblOpenDate = e.Item.FindControl("lblOpenDate") as Label;
            Label lblClickDate = e.Item.FindControl("lblClickDate") as Label;
            HyperLink lnkDemoDate = e.Item.FindControl("lnkDemoDate") as HyperLink;
            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            btnDelete.Attributes.Add("onclick", "return confirm('Are you really want to delete this lead?')");
            lnkDemoDate.Style.Add("Font", "normal");

            switch (e.Item.Cells[0].Text)
            {
                case "Decline":
                    e.Item.Style.Add("color", "red");
                    lnkDemoDate.Style.Add("color", "red");
                    break;
                case "Done":
                    e.Item.Style.Add("color", "green");
                    lnkDemoDate.Style.Add("color", "green");
                    break;
            }

            DateTime demoDate = Convert.ToDateTime(lnkDemoDate.Text);
            if (demoDate > DateTime.Now && DateTime.Now.AddDays(7) > demoDate)
            {
                e.Item.Style.Add("background-color", "yellow");
                lnkDemoDate.Style.Add("background-color", "yellow");
            }

            InterpretDateLabel(lblOpenDate);
            InterpretDateLabel(lblClickDate);
            InterpretDateLink(lnkDemoDate);
        }

        /// User1 -- Staff First Name
        /// User2 -- Url for demo
        /// User3 -- Conference Call number for demo
        /// User4 -- Meeting ID
        /// User5 -- Date a customer decides to sign up for a demo.
        /// User6 -- Staff ID
        /// UserEvent1Date -- Invite Send Date
        /// UserEvent2 -- Demo Status
        /// UserEvent2Date -- Demo Date

        protected void btnSendInvite_Click(object sender, System.EventArgs e)
        {
            lblErrorMessage.Visible = false;

            Emails email = new Emails();
            email.GetEmail(txtEmail.Text, LeadConfig.CustomerID);

            if (email.ID() > 0)
            {
                email = Emails.GetEmailByID(email.ID());
                lblErrorMessage.Visible = true;
                if (email.User6 != Staff.CurrentStaff.ID.ToString())
                {
                    lblErrorMessage.Text = string.Format("An invitation has been send to this customer on {0} by {1}.", email.UserEvent1Date, Staff.GetStaffByID(Convert.ToInt32(email.User6)).FirstName);
                    return;
                }

                lblErrorMessage.Text = string.Format("An invitation has been send to this customer on {0}. Another one is sent out again.", email.UserEvent1Date);
            }

            email.CustID = LeadConfig.CustomerID;
            email.Email = txtEmail.Text;
            email.FirstName = txtFirstName.Text;
            email.LastName = txtLastName.Text;
            email.Company = txtCompany.Text;
            email.Voice = txtPhone.Text;
            email.Notes = txtNote.Text;
            email.User1 = Staff.CurrentStaff.FirstName;
            email.User6 = Staff.CurrentStaff.ID.ToString();
            email.UserEvent1Date = DateTime.Now;
            email.Save();

            Groups group = new Groups(LeadConfig.DemoInviteGroupID);
            group.AttachEmail(email, "html", "S");

            if (chkSendEmail.Checked)
            {
                BlastSingle blastSingle = new BlastSingle(LeadConfig.BlastID, email.ID(), DateTime.Now);
                blastSingle.Save();
            }
            UpdateDashBoard();
        }


        private void dgdLeads_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            IsDesc = !IsDesc;
            SortingColumn = e.SortExpression;
            UpdateLeads();
        }


        private void dgdLeads_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            dgdLeads.CurrentPageIndex = e.NewPageIndex;
            dgdLeads.DataSource = UpdateLeads();
            dgdLeads.DataBind();
        }


        protected void btnClear_Click(object sender, System.EventArgs e)
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtNote.Text = string.Empty;
            lblErrorMessage.Visible = false;
        }


        private void dgdLeads_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    string sql = string.Format(@"delete from EmailGroups where GroupID = {0} and EmailID ={1};
if  not exists(select * from EmailGroups where EmailID = {1})
 delete from Emails where EmailID = {1}", LeadConfig.DemoInviteGroupID, e.CommandArgument);
                    DataFunctions.ExecuteScalar("communicator", sql);
                    UpdateDashBoard();
                    break;
            }
        }


        protected void btnUpdateCallCount_Click(object sender, System.EventArgs e)
        {
            CallRecord record = CallRecord.GetCallRecord(Convert.ToInt32(ddlStaff.SelectedValue), calCallDate.SelectedDate);

            if (record == null)
            {
                record = new CallRecord(Convert.ToInt32(ddlStaff.SelectedValue), calCallDate.SelectedDate, Convert.ToInt32(txtCallCount.Text));
            }
            else
            {
                record.CallCount = Convert.ToInt32(txtCallCount.Text);
            }

            record.Save();
            UpdateWeeklyView();
        }

        protected void calCallDate_SelectionChanged(object sender, System.EventArgs e)
        {
            CallRecord record = CallRecord.GetCallRecord(Convert.ToInt32(ddlStaff.SelectedValue), calCallDate.SelectedDate);

            if (record == null)
            {
                txtCallCount.Text = "0";
                return;
            }

            txtCallCount.Text = record.CallCount.ToString();
        }
        #endregion

        #region Properties
        private bool IsDesc
        {
            get
            {
                if (Session["SortingDirection"] == null)
                {
                    return true;
                }
                return Convert.ToBoolean(Session["SortingDirection"]);
            }
            set { Session["SortingDirection"] = value; }
        }

        private string SortingColumn
        {
            get
            {
                if (Session["SortingColumn"] == null)
                {
                    return "demodate";
                }
                return Convert.ToString(Session["SortingColumn"]);
            }
            set { Session["SortingColumn"] = value; }
        }

        private DateTime TheFirstDayOfTheMonth
        {
            get { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); }
        }
        private DateTime TheLastDayOfTheMonth
        {
            get { return TheFirstDayOfTheMonth.AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); }
        }

        private DateTime StartDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(txtStartDate.Text);
                }
                catch (Exception)
                {
                    txtStartDate.Text = DateTime.Now.ToShortDateString();
                    return DateTime.Now;
                }
            }
        }

        private DateTime EndDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(txtEndDate.Text);
                }
                catch (Exception)
                {
                    txtEndDate.Text = DateTime.Now.ToShortDateString();
                    return DateTime.Now;
                }
            }
        }
        #endregion

        private void LoadStaff()
        {
            if (Staff.CurrentStaff.Role == StaffRoleEnum.AccountManager) 
            {
                ddlStaff.DataSource = Staff.GetStaff();
                ddlStaff.DataTextField = "Email";
                ddlStaff.DataValueField = "ID";
                ddlStaff.DataBind();
                ddlStaff.SelectedIndex = ddlStaff.Items.IndexOf(ddlStaff.Items.FindByValue(Staff.CurrentStaff.ID.ToString()));
            }
            else
            {
                ddlStaff.Items.Add(new ListItem(Staff.CurrentStaff.Email, Staff.CurrentStaff.ID.ToString()));
                ddlStaff.SelectedIndex = 0;
            }
        }


        private void UpdateDashBoard()
        {
            ArrayList leads = UpdateLeads();
            lblMgrMessage.Text = string.Format("{0}~{1} MGR", StartDate.ToShortDateString(), EndDate.ToShortDateString());

            UpdateViewByDateRange(leads);
            UpdateWeeklyView();
        }


        private void UpdateWeeklyView()
        {
            ArrayList leads = new Lead().GetLeadsByStafffIDAndDateRange(Convert.ToInt32(ddlStaff.SelectedValue), TheFirstDayOfTheMonth, TheLastDayOfTheMonth);
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), end;
            while (start.DayOfWeek != DayOfWeek.Sunday)
            {
                start = start.AddDays(-1);
            }

            end = start.AddDays(6);
            ucLeadsWeeklyReport.Show(Convert.ToInt32(ddlStaff.SelectedValue), start, end, leads, false);
        }


        private ArrayList UpdateLeads()
        {
            ArrayList leads = new Lead().GetLeadsByStafffIDAndDateRange(Convert.ToInt32(ddlStaff.SelectedValue), StartDate, EndDate);
            SortLeads(leads);
            dgdLeads.DataSource = leads;
            dgdLeads.DataBind();
            return leads;
        }


        private void SortLeads(ArrayList leads)
        {
            IComparer sorter;
            switch (SortingColumn)
            {
                case "senddate":
                    sorter = new LeadSendDateComparer(IsDesc);
                    break;
                case "opendate":
                    sorter = new LeadOpenDateComparer(IsDesc);
                    break;
                case "clickdate":
                    sorter = new LeadClickDateComparer(IsDesc);
                    break;
                case "demodate":
                    sorter = new LeadDemoDateComparer(IsDesc);
                    break;
                case "company":
                    sorter = new LeadCompanyComparer(IsDesc);
                    break;
                default:
                    throw new ApplicationException(string.Format("Unknow sorting express: {0}", SortingColumn));
            }
            leads.Sort(sorter);
        }


        private void UpdateViewByDateRange(ArrayList leads)
        {
            int inviteCount = leads.Count, demoCount = 0;
            foreach (Lead lead in leads)
            {
                if (lead.DemoDate >= StartDate && lead.DemoDate <= EndDate)
                {
                    demoCount++;
                }
            }

            int qouteCount = Quote.GetQuoteNumberByStaffIDAndDateRange(Convert.ToInt32(ddlStaff.SelectedValue), StartDate, EndDate);
            lblInvitesCount.Text = inviteCount.ToString();
            lblDemoCount.Text = demoCount.ToString();
            lblQuotesCount.Text = qouteCount.ToString();

            double demoPercentage = (double)demoCount * 100 / inviteCount;
            lblDemoPercentage.Text = string.Format("{0:###.00}%", demoPercentage);

            double quotePercentage = (double)qouteCount * 100 / inviteCount;
            lblQuotePercentage.Text = string.Format("{0:###.00}%", quotePercentage);
        }


        private void InterpretDateLabel(Label dateLabel)
        {
            if (dateLabel.Text == "1/1/01 00:00")
            {
                dateLabel.Text = "N/A";
            }
        }


        private void InterpretDateLink(HyperLink link)
        {
            if (link.Text == "1/1/01 00:00")
            {
                link.Text = "N/A";
            }
        }

        protected string InterpretDateString(string dateString)
        {
            if (dateString == DateTime.MinValue.ToString())
            {
                return "No demo sign up date.";
            }

            return string.Format("The demo is signed up on {0}.", dateString);
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            this.dgdLeads.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgdLeads_ItemCommand);
            this.dgdLeads.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgdLeads_PageIndexChanged);
            this.dgdLeads.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgdLeads_SortCommand);
            this.dgdLeads.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgdLeads_ItemDataBound);

        }
        #endregion
    }
}
