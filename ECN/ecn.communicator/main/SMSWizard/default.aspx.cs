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
using ecn.common.classes;

namespace ecn.communicator.main.SMSWizard
{
    public partial class _default : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            if (!IsPostBack)
            {
                ViewState["p_SortField"] = "DateCreated";
                ViewState["p_SortDirection"] = "DESC";

                ViewState["sc_SortField"] = "Sendtime";
                ViewState["sc_SortDirection"] = "DESC";

                ViewState["s_SortField"] = "Sendtime";
                ViewState["s_SortDirection"] = "DESC";

                btnimg1.Visible = true;
                btnimg2.Visible = true;

                //if (!(KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)))
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {

                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View))
                    {
                        throw new NotImplementedException("NOT IMPLEMENTED");
                    }
                    //if (!(KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") && KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "createblast")) || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "approvalblast"))
                    //{
                    //    btnimg1.Visible = false;
                    //}

                    //if (!KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "contentpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "ecnwiz"))
                    //{
                    //    btnimg2.Visible = false;
                    //}
                   
                }

                loadGrids();
            }
        }

        private void loadGrids()
        {
            loadPendingCampaigns();
            loadScheduledCampaigns();
            loadSentCampaigns();
        }

        private void loadPendingCampaigns()
        {

            string sqlQuery = "select WizardID, WizardName, DateCreated, 'WizardID=' + Convert(varchar,WizardID) + '&chID=" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "&cuID=" + Master.UserSession.CurrentUser.CustomerID + "' as WizardURL from Wizard where BlastType='sms' and StatusCode='pending' and UserID = " + Master.UserSession.CurrentUser.UserID;

            DataTable dt = DataFunctions.GetDataTable(sqlQuery);

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["p_SortField"].ToString() + ' ' + ViewState["p_SortDirection"].ToString();

            dgPendingCampaigns.DataSource = dv;
            dgPendingCampaigns.DataBind();

            PendingPager.RecordCount = dt.Rows.Count;

            if (PendingPager.RecordCount > 0)
                PendingPager.Visible = true;
            else
                PendingPager.Visible = false;
        }

        private void loadScheduledCampaigns()
        {

            string sqlQuery = "select w.WizardName, b.BlastID, b.Sendtime from  Wizard w join blasts b on w.blastID = b.blastID where b.BlastType='sms' and b.StatusCode='pending' and b.UserID = " + Master.UserSession.CurrentUser.UserID;

            DataTable dt = DataFunctions.GetDataTable(sqlQuery);

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["sc_SortField"].ToString() + ' ' + ViewState["sc_SortDirection"].ToString();

            dgScheduledCampaigns.DataSource = dv;
            dgScheduledCampaigns.DataBind();
            SchedulePager.RecordCount = dt.Rows.Count;

            if (SchedulePager.RecordCount > 0)
                SchedulePager.Visible = true;
            else
                SchedulePager.Visible = false;
        }

        private void loadSentCampaigns()
        {
            string sqlQuery = "select WizardID, WizardName, Sendtime, w.blastID from Wizard w join blasts b on w.blastID = b.blastID where b.BlastType='sms' and b.statuscode='sent' and w.StatusCode='completed' and w.UserID = " + Master.UserSession.CurrentUser.UserID + " order by sendtime desc ";

            DataTable dt = DataFunctions.GetDataTable(sqlQuery);

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["s_SortField"].ToString() + ' ' + ViewState["s_SortDirection"].ToString();

            dgSentCampaigns.DataSource = dv;
            dgSentCampaigns.DataBind();

            SentPager.RecordCount = dt.Rows.Count;

            if (SentPager.RecordCount > 0)
                SentPager.Visible = true;
            else
                SentPager.Visible = false;

        }

        public void dgPendingCampaigns_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                dgPendingCampaigns.CurrentPageIndex = 0;
                PendingPager.CurrentPage = 1;
                PendingPager.CurrentIndex = 0;

                DataFunctions.Execute("delete from wizard where wizardID = " + dgPendingCampaigns.DataKeys[e.Item.ItemIndex]);

                loadGrids();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

        public void dgPendingCampaigns_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            ImageButton btnDelete = new ImageButton();
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                btnDelete = (ImageButton)e.Item.FindControl("btnDelete");
                btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete this message?')");
            }
        }

        private void dgPendingCampaigns_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToLower() == ViewState["p_SortField"].ToString().ToLower())
            {
                switch (ViewState["p_SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["p_SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["p_SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["p_SortField"] = e.SortExpression;
                ViewState["p_SortDirection"] = "ASC";
            }

            loadGrids();
        }

        public void dgScheduledCampaigns_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            try
            {
                //DataFunctions.Execute("delete from blasts where blastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);
                //DataFunctions.Execute("delete from blastplans where blastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);

                //We don't want to delete any blast record physically - ashok 10/05/07 after the incident with F51 customer.
                DataFunctions.Execute("if exists (SELECT BlastID FROM Blasts WHERE BlastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex] + " AND StatusCode = 'sent' AND SuccessTotal > 0) UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex] + " ELSE UPDATE Blasts SET StatusCode = 'deleted' WHERE blastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);
                DataFunctions.Execute("UPDATE BlastPlans SET BlastID = -BlastID WHERE BlastID = " + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);

                System.Data.SqlClient.SqlCommand cmddelactivity = new System.Data.SqlClient.SqlCommand("spDeleteEmailActivityByBlastID");
                cmddelactivity.CommandType = CommandType.StoredProcedure;
                cmddelactivity.Parameters.AddWithValue("@blastID", dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);
                DataFunctions.Execute("activity", cmddelactivity);

                //DataFunctions.Execute("DELETE FROM EmailActivityLog  WHERE BlastID=" + dgScheduledCampaigns.DataKeys[e.Item.ItemIndex]);

                dgScheduledCampaigns.CurrentPageIndex = 0;
                SchedulePager.CurrentPage = 1;
                SchedulePager.CurrentIndex = 0;

                loadGrids();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

        public void dgScheduledCampaigns_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            ImageButton btnDelete = new ImageButton();
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                btnDelete = (ImageButton)e.Item.FindControl("btnDelete1");
                //btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete this message?')");
                btnDelete.Attributes.Add("onclick", "return confirm('Blast titled \"" + e.Item.Cells[0].Text.ToString().Replace("'", "") + "\" will be DELETED !!" + "\\n" + "\\n" + "Are you sure you want to contine? This process CANNOT be undone.');");

            }
        }

        private void dgScheduledCampaigns_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToLower() == ViewState["sc_SortField"].ToString().ToLower())
            {
                switch (ViewState["sc_SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["sc_SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["sc_SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["sc_SortField"] = e.SortExpression;
                ViewState["sc_SortDirection"] = "ASC";
            }

            loadGrids();

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
            this.btnimg1.Click += new System.Web.UI.ImageClickEventHandler(this.btnimg1_Click);
            this.btnimg2.Click += new System.Web.UI.ImageClickEventHandler(this.btnimg2_Click);
            this.dgPendingCampaigns.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgPendingCampaigns_SortCommand);
            this.dgScheduledCampaigns.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgScheduledCampaigns_SortCommand);
            this.dgSentCampaigns.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgSentCampaigns_SortCommand);

        }
        #endregion

        private void btnimg1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/smswizard/setupcampaign.aspx?chID=" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "&cuID=" + Master.UserSession.CurrentUser.CustomerID);
        }

        private void btnimg2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/content/");
        }

     

        private void dgSentCampaigns_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToLower() == ViewState["s_SortField"].ToString().ToLower())
            {
                switch (ViewState["s_SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["s_SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["s_SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["s_SortField"] = e.SortExpression;
                ViewState["s_SortDirection"] = "ASC";
            }

            loadGrids();
        }

        protected void Pager_IndexChanged(object sender, System.EventArgs e)
        {
            loadGrids();
        }

    }
}
