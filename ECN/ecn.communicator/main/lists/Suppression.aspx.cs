using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using ecn.communicator.CommonControls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists
{
    public partial class Suppression : ECN_Framework.WebPageHelper
    {
        private const string SubscribeTypeAll = " 'S', 'U', 'D', 'P', 'B', 'M','A','F','?','E' ";
        int _pagerCurrentPage = 1;
        //bool _byDate = false;
        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }

        //public bool ByDate
        //{
        //    set { _byDate = value; }
        //    get { return _byDate; }
        //}

        int MasterRecordCount = 0;
        int DomainRecordCount = 0;
        int NoThresholdRecordCount = 0;
        int GlobalRecordCount = 0;

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "Suppression";
            Master.Heading = "Groups > Suppression";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            pnlDomainSuppressionMsg.Visible = false;
            lblDomainSuppressionMsg.Text = "";
            lblMasterSuppressionMsg.Text = "";
            pnlMasterSuppressionMsg.Visible = false;

            //if (false == KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))

            //MASTER SUPRESSION TAB
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)
                || KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit)
                || KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View))
            {
                TabMasterSuppression.Visible = true;
            }
            else
            {
                TabMasterSuppression.Visible = false;
            }

            //DOMAIN SUPRESSION TAB
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)
                || KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSuppression, KMPlatform.Enums.Access.View))
            {
                TabDomainSuppression.Visible = true;
            }
            else
            {
                TabDomainSuppression.Visible = false;
            }

            //CHANNEL SUPRESSION TAB & NO THRESHOLD TAB
            if (KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser)
                || KM.Platform.User.IsSystemAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
                TabChannelSuppression.Visible = true;
                TabNoThreshold.Visible = true;
                pnlDomainSuppFor.Visible = true;
            }
            else
            {
                TabChannelSuppression.Visible = false;
                TabNoThreshold.Visible = false;
                pnlDomainSuppFor.Visible = false;
            
            }
            //GLOBAL SUPPRESSION TAB
            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                TabGlobalSuppression.Visible = false;
            }

            chID_Hidden.Value = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            custID_Hidden.Value = Master.UserSession.CurrentUser.CustomerID.ToString();

            if (!(Page.IsPostBack))
            {

                List<ECN_Framework_Entities.Communicator.Group> groupList =
                ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
                List<ECN_Framework_Entities.Communicator.Group> result = (from src in groupList
                                                                          where src.MasterSupression == 1
                                                                          select src).ToList();

                int groupID = result[0].GroupID;
                grpID_Hidden.Value = groupID.ToString();
                ViewState["searchFilterVS"] = "";
                //ByDate = true;
               loadSuppressionGroupGrid();
               
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DomainSuppression, KMPlatform.Enums.Access.Edit))
                {
                    pnlDomainSuppFor.Visible = false;
                    pnlDomainSuppAdd.Visible = false;
                    gvDomainSuppression.Columns[2].Visible = false;
                    gvDomainSuppression.Columns[3].Visible = false;
                }
                else
                {
                    pnlDomainSuppFor.Visible = true;
                    pnlDomainSuppAdd.Visible = true;
                    gvDomainSuppression.Columns[2].Visible = true;
                    gvDomainSuppression.Columns[3].Visible = true;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                {
                    gvSuppressionGroup.Columns[6].Visible = false;
                }
                else 
                {
                    gvSuppressionGroup.Columns[6].Visible = true;
                }

                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    gvSuppressionGroup.Columns[7].Visible = false;
                    gvChannelMasterSuppression.Columns[2].Visible = false;
                }
                else
                {
                    gvSuppressionGroup.Columns[7].Visible = true;
                    gvChannelMasterSuppression.Columns[2].Visible = true;
                }
            }
        }

        protected void TabContainer_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer.ActiveTabIndex == 0)
            {
                loadSuppressionGroupGrid();
            }
            else if (TabContainer.ActiveTabIndex == 2)
            {
                loadChannelMasterSuppressionGrid();
            }
            else if (TabContainer.ActiveTabIndex == 1)
            {
                loadDomainSuppressionGrid();
            }
            else if (TabContainer.ActiveTabIndex == 3)
            {
                loadNoThresholdEmailsGrid();
            }
            else if (TabContainer.ActiveTabIndex == 4)
            {
                loadGlobalMasterSuppressionGrid();
            }
        }

        #region Domain Suppression Tab

        private void loadDomainSuppressionGrid()
        {
            List<ECN_Framework_Entities.Communicator.DomainSuppression> domainSuppressionList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();

            
            if (pnlDomainSuppFor.Visible)
            {
                domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain(txtsearchDomain.Text.Replace("'", "''"), Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
            }
            else
            {
                domainSuppressionList = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomain(txtsearchDomain.Text.Replace("'", "''"), Master.UserSession.CurrentCustomer.CustomerID, null, Master.UserSession.CurrentUser);
            }
            DomainRecordCount = domainSuppressionList.Count;
            gvDomainSuppression.DataSource = domainSuppressionList;
            gvDomainSuppression.DataBind();
        }

        public void btnAddDomainSuppression_click(object sender, System.EventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Communicator.DomainSuppression domainSuppression = new ECN_Framework_Entities.Communicator.DomainSuppression();
                if (rbType.SelectedItem.Value == "Channel")
                {
                    domainSuppression.BaseChannelID = Convert.ToInt32(Master.UserSession.CurrentBaseChannel.BaseChannelID);
                }
                else
                {
                    domainSuppression.CustomerID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID);
                }
                domainSuppression.DomainSuppressionID = Convert.ToInt32(lblDomainSuppressionID.Text);
                domainSuppression.IsActive = true;
                domainSuppression.Domain = txtDomain.Text;
                if (Convert.ToInt32(lblDomainSuppressionID.Text) > 0)
                {
                    domainSuppression.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                }
                else
                    domainSuppression.CreatedUserID = Master.UserSession.CurrentUser.UserID;

                ECN_Framework_BusinessLayer.Communicator.DomainSuppression.Save(domainSuppression, Master.UserSession.CurrentUser);

                loadDomainSuppressionGrid();
                txtDomain.Text = "";
                rbType.SelectedValue = "Customer";
                lblDomainSuppressionID.Text = "0";
                btnAddDomainSuppression.Text = "Add Domain Suppression";
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        public void btnCancel_click(object sender, System.EventArgs e)
        {
            txtDomain.Text = "";
            rbType.SelectedValue = "Customer";
            lblDomainSuppressionID.Text = "0";
            btnAddDomainSuppression.Text = "Add Domain Suppression";
        }

        protected void gvDomainSuppression_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvDomainSuppression.PageIndex = e.NewPageIndex;
            }
            loadDomainSuppressionGrid();
        }

        protected void gvDomainSuppression_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.DomainSuppression.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
            loadDomainSuppressionGrid();
        }

        protected void gvDomainSuppression_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvDomainSuppression_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int DomainSuppressionID = Convert.ToInt32(gvDomainSuppression.DataKeys[e.NewEditIndex].Values["DomainSuppressionID"].ToString());
            ECN_Framework_Entities.Communicator.DomainSuppression domainSupression = ECN_Framework_BusinessLayer.Communicator.DomainSuppression.GetByDomainSuppressionID(DomainSuppressionID, Master.UserSession.CurrentUser);

            txtDomain.Text = domainSupression.Domain;
            if (domainSupression.BaseChannelID != null)
            {
                rbType.SelectedValue = "Channel";
            }
            else
            {
                rbType.SelectedValue = "Customer";
            }
            btnAddDomainSuppression.Text = "Edit Domain Suppression";
            lblDomainSuppressionID.Text = DomainSuppressionID.ToString();
        }

        protected void gvDomainSuppression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsDomain = (Label)e.Row.FindControl("lblTotalRecordsDomain");
                lblTotalRecordsDomain.Text = DomainRecordCount.ToString();

                Label lblTotalNumberOfPagesDomain = (Label)e.Row.FindControl("lblTotalNumberOfPagesDomain");
                lblTotalNumberOfPagesDomain.Text = gvDomainSuppression.PageCount.ToString();

                TextBox txtGoToPageDomain = (TextBox)e.Row.FindControl("txtGoToPageDomain");
                txtGoToPageDomain.Text = (gvDomainSuppression.PageIndex + 1).ToString();
            }
        }

        protected void GoToPageDomain_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvDomainSuppression.PageCount)
            {
                this.gvDomainSuppression.PageIndex = pageNumber - 1;
            }
            else
            {
                gvDomainSuppression.PageIndex = 0;
            }
            loadDomainSuppressionGrid();
        }

        protected void btnsearchDomain_Click(object sender, EventArgs e)
        {
            loadDomainSuppressionGrid();
        }

        #endregion

        #region No Threshhold Tab

        public void btnAddNoThresholdEmails_Click(object sender, System.EventArgs e)
        {
            int emailsAdded = 0;
            string emailAddressToAdd = emailAddresses.Text;
            StringBuilder xmlInsert = new StringBuilder();
            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            DateTime startDateTime = DateTime.Now;

            Hashtable hUpdatedRecords = new Hashtable();

            if (emailAddressToAdd.Length > 0)
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                while (st.HasMoreTokens())
                {
                    xmlInsert.Append("<ea>" + st.NextToken().Trim() + "</ea>");
                }

                xmlInsert.Append("</XML>");
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsToNoThreshold(Master.UserSession.CurrentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());

                if (emailRecordsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in emailRecordsDT.Rows)
                    {
                        if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                            hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                        else
                        {
                            int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                            hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                        }
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Action");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Action"] = "Total Records in the File";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Action"] = "New";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Action"] = "Changed";
                            row["sortOrder"] = 3;
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Action"] = "Duplicate(s)";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Action"] = "Skipped";
                            row["sortOrder"] = 5;
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Action"] = "&nbsp;";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Action"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";

                    ResultsGrid.DataSource = dv;
                    ResultsGrid.DataBind();
                    ResultsGrid.Visible = true;
                    importResultsPNL.Visible = true;
                    MessageLabel.Visible = false;

                    loadNoThresholdEmailsGrid();
                }
                emailAddresses.Text = "";

            }
            else
            {
                ResultsGrid.Visible = false;
                MessageLabel.Visible = true;
                MessageLabel.Text = "<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>";
            }
        }

        protected void exportEmailsBTN_Click(object sender, EventArgs e)
        {
            string newline = "";
            string clientID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_NoThreshold_Emails.CSV";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + clientID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            string outfileName = txtoutFilePath + fileName;
            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }
            TextWriter txtfile = File.AppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList =
            ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, txtNoThresholdEmails.Text.Replace("'", "''"), Master.UserSession.CurrentUser);

            var result = (from src in channelNoThresholdList
                          orderby src.EmailAddress
                          group src by src.EmailAddress into grp
                          select new
                          {
                              EmailAddress = grp.Key,
                              DateAdded = grp.Max(t => t.CreatedDate)
                          }).ToList();

            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    newline = "";
                    newline += result[i].EmailAddress + ", " + result[i].DateAdded.ToString();
                    txtfile.WriteLine(newline);
                }
            }
            txtfile.Close();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private void loadNoThresholdEmailsGrid()
        {
            List<ECN_Framework_Entities.Communicator.ChannelNoThresholdList> channelNoThresholdList =
             ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, txtNoThresholdEmails.Text.Replace("'", "''"), Master.UserSession.CurrentUser);

            var result = (from src in channelNoThresholdList
                          orderby src.EmailAddress
                          group src by src.EmailAddress into g
                          select new
                          {
                              EmailAddress = g.Key,
                              DateAdded = g.Max(t => t.CreatedDate)
                          }).ToList();

            if (result.Count > 0)
            {
                exportEmailsBTN.Visible = true;
                NoThresholdRecordCount = result.Count;
                gvNoThreshold.DataSource = result;
                gvNoThreshold.DataBind();
            }
            else
            {
                gvNoThreshold.DataSource = null;
                gvNoThreshold.DataBind();
                exportEmailsBTN.Visible = false;
            }
        }

        protected void gvNoThreshold_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsNoThreshold = (Label)e.Row.FindControl("lblTotalRecordsNoThreshold");
                lblTotalRecordsNoThreshold.Text = NoThresholdRecordCount.ToString();

                Label lblTotalNumberOfPagesNoThreshold = (Label)e.Row.FindControl("lblTotalNumberOfPagesNoThreshold");
                lblTotalNumberOfPagesNoThreshold.Text = gvNoThreshold.PageCount.ToString();

                TextBox txtGoToPageNoThreshold = (TextBox)e.Row.FindControl("txtGoToPageNoThreshold");
                txtGoToPageNoThreshold.Text = (gvNoThreshold.PageIndex + 1).ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteBtn = e.Row.FindControl("deleteEmailBTN") as LinkButton;
                deleteBtn.Attributes.Add("onclick", "return confirm('Email Address \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" will be removed from the No Threshold List!!" + "\\n" + "This process will enable \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" to be suppressed by Threshold Suppression for campaigns you have scheduled / will be sending in the future." + "\\n" + "\\n" + "Are you sure you want to contine?This process CANNOT be undone.');");
            }
            return;
        }

        protected void gvNoThreshold_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "DELETEEMAIL":
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.Delete(Master.UserSession.CurrentBaseChannel.BaseChannelID, e.CommandArgument.ToString().Replace("'", "''"), Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                    break;
            }
            loadNoThresholdEmailsGrid();
        }

        protected void gvNoThreshold_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void GoToPageNoThreshold_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvNoThreshold.PageCount)
            {
                this.gvNoThreshold.PageIndex = pageNumber - 1;
            }
            else
            {
                gvNoThreshold.PageIndex = 0;
            }
            loadNoThresholdEmailsGrid();
        }


        protected void gvNoThreshold_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvNoThreshold.PageIndex = e.NewPageIndex;
            }
            loadNoThresholdEmailsGrid();
        }

        protected void btnsearchNoThreshold_Click(object sender, EventArgs e)
        {
            loadNoThresholdEmailsGrid();
        }

        #endregion

        #region Global Suppression Tab

        public void btnAddGlobalSuppressionEmails_Click(object sender, System.EventArgs e)
        {

            int emailsAdded = 0;
            string emailAddressToAdd = txtemailAddressesGlobal.Text;
            StringBuilder xmlInsert = new StringBuilder();
            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            DateTime startDateTime = DateTime.Now;

            Hashtable hUpdatedRecords = new Hashtable();

            if (emailAddressToAdd.Length > 0)
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                while (st.HasMoreTokens())
                {
                    xmlInsert.Append("<ea>" + st.NextToken().Trim() + "</ea>");
                }

                xmlInsert.Append("</XML>");
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsToGlobalMS(Master.UserSession.CurrentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
                if (emailRecordsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in emailRecordsDT.Rows)
                    {
                        if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                            hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                        else
                        {
                            int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                            hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                        }
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Action");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Action"] = "Total Records in the File";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Action"] = "New";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Action"] = "Changed";
                            row["sortOrder"] = 3;
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Action"] = "Duplicate(s)";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Action"] = "Skipped";
                            row["sortOrder"] = 5;
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Action"] = "&nbsp;";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Action"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";
                    dgResultsGlobal.DataSource = dv;
                    dgResultsGlobal.DataBind();
                    dgResultsGlobal.Visible = true;
                    pnlimportResultsGlobal.Visible = true;
                    lblMessageGlobal.Visible = false;
                    txtemailAddressesGlobal.Text = "";

                    loadGlobalMasterSuppressionGrid();
                }

            }
            else
            {
            }
        }

        protected void btnexportEmailsGlobal_Click(object sender, EventArgs e)
        {
            string newline = "";
            string clientID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_MasterSuppressedGlobal_Emails.CSV";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + clientID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            string outfileName = txtoutFilePath + fileName;

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");
            List<ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList> globalMasterSuppressionList_List =
            ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress(txtEmailsGlobal.Text.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (globalMasterSuppressionList_List.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GlobalMasterSuppressionList GlobalMasterSuppressionList in globalMasterSuppressionList_List)
                {
                    newline = "";
                    newline += GlobalMasterSuppressionList.EmailAddress + ", " + GlobalMasterSuppressionList.CreatedDate;
                    txtfile.WriteLine(newline);
                }
            }
            txtfile.Close();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private void loadGlobalMasterSuppressionGrid()
        {
            DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.GetByEmailAddress_Paging(ChannelMasterPager.CurrentPage, ChannelMasterPager.PageSize, txtEmailsGlobal.Text, Master.UserSession.CurrentUser);
            DataTable emailstable = emailsListDS.Tables[1];
            DataTable dt = emailsListDS.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                GlobalMasterPager.RecordCount = Convert.ToInt32(dr[0]);
            }

            if (GlobalMasterPager.RecordCount > 0)
            {
                btnexportEmailsGlobal.Visible = true;
            }
            else
            {
                btnexportEmailsGlobal.Visible = false;
            }

            gvGlobalMasterSuppression.DataSource = emailstable;
            gvGlobalMasterSuppression.DataBind();
        }

        protected void GlobalMasterPager_IndexChanged(object sender, System.EventArgs e)
        {
            loadGlobalMasterSuppressionGrid();
        }

        protected void gvGlobalMasterSuppression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteBtn = e.Row.FindControl("deleteEmailBTN") as LinkButton;
                deleteBtn.Attributes.Add("onclick", "return confirm('Email Address \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" will be removed from the Global Suppression List!" + "\\n" + "This process will enable \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" to start receiving the campaigns that you have scheduled / will be sending in the future." + "\\n" + "\\n" + "Are you sure you want to continue? This process CANNOT be undone.');");
            }
            return;
        }

        protected void gvGlobalMasterSuppression_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "DELETEEMAIL":
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.GlobalMasterSuppressionList.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                    break;
            }
            loadGlobalMasterSuppressionGrid();
        }

        protected void gvGlobalMasterSuppression_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnsearchEmailsGlobal_Click(object sender, EventArgs e)
        {
            loadGlobalMasterSuppressionGrid();
        }

        protected void GoToPageGlobalMaster_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvGlobalMasterSuppression.PageCount)
            {
                this.gvGlobalMasterSuppression.PageIndex = pageNumber - 1;
            }
            else
            {
                gvGlobalMasterSuppression.PageIndex = 0;
            }
            loadGlobalMasterSuppressionGrid();
        }

        protected void gvGlobalMasterSuppression_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvGlobalMasterSuppression.PageIndex = e.NewPageIndex;
            }
            loadGlobalMasterSuppressionGrid();
        }

        #endregion

        #region Channel Suppression Tab

        public void btnAddChannelSuppressionEmails_Click(object sender, System.EventArgs e)
        {

            int emailsAdded = 0;
            string emailAddressToAdd = txtemailAddresses.Text;
            StringBuilder xmlInsert = new StringBuilder();
            xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            DateTime startDateTime = DateTime.Now;

            Hashtable hUpdatedRecords = new Hashtable();

            if (emailAddressToAdd.Length > 0)
            {
                emailAddressToAdd = emailAddressToAdd.Replace("\r\n", ",");
                emailAddressToAdd = emailAddressToAdd.Replace("\n", ",");
                StringTokenizer st = new StringTokenizer(emailAddressToAdd, ',');

                while (st.HasMoreTokens())
                {
                    xmlInsert.Append("<ea>" + st.NextToken().Trim() + "</ea>");
                }

                xmlInsert.Append("</XML>");
                DataTable emailRecordsDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsToCS(Master.UserSession.CurrentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID, xmlInsert.ToString());
                if (emailRecordsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in emailRecordsDT.Rows)
                    {
                        if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                            hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                        else
                        {
                            int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                            hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                        }
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Action");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Action"] = "Total Records in the File";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Action"] = "New";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Action"] = "Changed";
                            row["sortOrder"] = 3;
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Action"] = "Duplicate(s)";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Action"] = "Skipped";
                            row["sortOrder"] = 5;
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Action"] = "&nbsp;";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Action"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";

                    dgResults.DataSource = dv;
                    dgResults.DataBind();
                    dgResults.Visible = true;
                    pnlimportResults.Visible = true;
                    lblMessage.Visible = false;
                    txtemailAddresses.Text = "";

                    loadChannelMasterSuppressionGrid();
                }

            }
            else
            {
                dgResults.Visible = false;
                lblMessage.Visible = true;
                lblMessage.Text = "<font face=verdana size=2 color=#000000>&nbsp;" + emailsAdded.ToString() + " rows updated/inserted </font>";
            }
        }

        protected void btnexportEmails_Click(object sender, EventArgs e)
        {
            string newline = "";
            string clientID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
            string fileName = clientID + "_MasterSuppressed_Emails.CSV";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + clientID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            string outfileName = txtoutFilePath + fileName;

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);
            txtfile.WriteLine("EmailAddress, DateAdded");
            List<ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList> channelMasterSuppressionList_List =
            ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(Master.UserSession.CurrentBaseChannel.BaseChannelID, txtEmails.Text.Replace("'", "''"), Master.UserSession.CurrentUser);
            if (channelMasterSuppressionList_List.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.ChannelMasterSuppressionList ChannelMasterSuppressionList in channelMasterSuppressionList_List)
                {
                    newline = "";
                    newline += ChannelMasterSuppressionList.EmailAddress + ", " + ChannelMasterSuppressionList.CreatedDate;
                    txtfile.WriteLine(newline);
                }
            }
            txtfile.Close();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }

        private void loadChannelMasterSuppressionGrid()
        {
            DataSet emailsListDS = ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress_Paging(Master.UserSession.CurrentBaseChannel.BaseChannelID, ChannelMasterPager.CurrentPage, ChannelMasterPager.PageSize, txtEmails.Text, Master.UserSession.CurrentUser);
            DataTable emailstable = emailsListDS.Tables[1];
            DataTable dt = emailsListDS.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                ChannelMasterPager.RecordCount = Convert.ToInt32(dr[0]);
            }

            if (ChannelMasterPager.RecordCount > 0)
            {
                btnexportEmails.Visible = true;
            }
            else
            {
                btnexportEmails.Visible = false;
            }

            gvChannelMasterSuppression.DataSource = emailstable;
            gvChannelMasterSuppression.DataBind();
        }

        protected void ChannelMasterPager_IndexChanged(object sender, System.EventArgs e)
        {
            loadChannelMasterSuppressionGrid();
        }

        protected void gvChannelMasterSuppression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteBtn = e.Row.FindControl("deleteEmailBTN") as LinkButton;
                deleteBtn.Attributes.Add("onclick", "return confirm('Email Address \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" will be removed from the Master Suppression List!" + "\\n" + "This process will enable \"" + e.Row.Cells[0].Text.ToString().Replace("'", "") + "\" to start receiving the campaigns that you have scheduled / will be sending in the future." + "\\n" + "\\n" + "Are you sure you want to continue? This process CANNOT be undone.');");
            }
            return;
        }

        protected void gvChannelMasterSuppression_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "DELETEEMAIL":
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.Delete(Master.UserSession.CurrentBaseChannel.BaseChannelID, Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                    }
                    break;
            }
            loadChannelMasterSuppressionGrid();
        }

        protected void gvChannelMasterSuppression_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnsearchEmails_Click(object sender, EventArgs e)
        {
            loadChannelMasterSuppressionGrid();
        }

        protected void GoToPageChannelMaster_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvChannelMasterSuppression.PageCount)
            {
                this.gvChannelMasterSuppression.PageIndex = pageNumber - 1;
            }
            else
            {
                gvChannelMasterSuppression.PageIndex = 0;
            }
            loadChannelMasterSuppressionGrid();
        }

        protected void gvChannelMasterSuppression_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvChannelMasterSuppression.PageIndex = e.NewPageIndex;
            }
            loadChannelMasterSuppressionGrid();
        }

        #endregion

        #region  master Suppression Tab

        private void loadSuppressionGroupGrid()
        {
            DataSet emailsListDS = null;
            //if (ByDate)
            //{

            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            DateTime.TryParse(DateFromFilter.Text, out fromDate);
            DateTime.TryParse(DateToFilter.Text, out toDate);
            emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentUser.CustomerID, Convert.ToInt32(grpID_Hidden.Value), EmailsPager.CurrentPage, EmailsPager.PageSize, fromDate, toDate, chkRecentActivity.Checked, ViewState["searchFilterVS"].ToString());
            //}
            //else
            //{
            //    emailsListDS = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentUser.CustomerID, Convert.ToInt32(grpID_Hidden.Value), EmailsPager.CurrentPage, EmailsPager.PageSize, ViewState["searchFilterVS"].ToString());
            //}
            DataTable emailstable = emailsListDS.Tables[1];
            DataTable dt = emailsListDS.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                EmailsPager.RecordCount = Convert.ToInt32(dr[0]);
            }

            MasterRecordCount = emailstable.Rows.Count;

            gvSuppressionGroup.DataSource = emailstable;
            gvSuppressionGroup.DataBind();

            if (emailstable.Rows.Count > 0)
            {
                DownloadPanel.Visible = true;
            }
            else
            {
                DownloadPanel.Visible = false;
            }

            //if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email))
        }

        protected void gvSuppressionGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailGroup.DeleteFromMasterSuppressionGroup(Convert.ToInt32(grpID_Hidden.Value), Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                }
                loadSuppressionGroupGrid();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void gvSuppressionGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvSuppressionGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void GoToPageMaster_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvSuppressionGroup.PageCount)
            {
                this.gvSuppressionGroup.PageIndex = pageNumber - 1;
            }
            else
            {
                gvSuppressionGroup.PageIndex = 0;
            }
            loadSuppressionGroupGrid();
        }

        protected void EmailsPager_IndexChanged(object sender, System.EventArgs e)
        {
            loadSuppressionGroupGrid();
        }

        protected void EmailFilterButton_Click(object sender, EventArgs e)
        {
            bool isValid = true;
            var dateValidator = new DateValidator();
            if (chkRecentActivity.Checked)
            {
                isValid = dateValidator.ValidateDates(DateFromFilter, DateToFilter, 2, phError, lblErrorMessage, true);
            }
            else
            {
                isValid = dateValidator.ValidateDates(DateFromFilter, DateToFilter, 2, phError, lblErrorMessage);
            }

            if (isValid)
            {
                EmailsPager.CurrentPage = 1;
                EmailsPager.CurrentIndex = 0;
                if (searchString != null)
                {
                    ViewState["searchFilterVS"] = searchString;
                    loadSuppressionGroupGrid();
                } 
            }
        }

        public string searchString
        {
            get
            {
                string filter = "";
                //ByDate = true;
                string subscribeType = SubscribeTypeFilter.SelectedItem.Value.ToString();
                string emailAdd = StringFunctions.CleanString(EmailFilter.Text);
                emailAdd = emailAdd.Replace("_", "[_]").Replace("'","''");
                string searchEmailLike = SearchEmailLike.SelectedItem.Value.ToString();

                if (emailAdd.Length > 0)
                {
                    if (searchEmailLike.Equals("like"))
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "%'";
                    }
                    else if (searchEmailLike.Equals("equals"))
                    {
                        filter = " AND EmailAddress like '" + emailAdd + "'";
                    }
                    else if (searchEmailLike.Equals("ends"))
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "'";
                    }
                    else if (searchEmailLike.Equals("starts"))
                    {
                        filter = " AND EmailAddress like '" + emailAdd + "%'";
                    }
                    else
                    {
                        filter = " AND EmailAddress like '%" + emailAdd + "%'";
                    }
                }

                if (!(subscribeType.Equals("*")))
                {
                    filter += " AND SubscribeTypeCode = '" + subscribeType + "'";
                }

                //if (chkRecentActivity.Checked)
                //{
                //    filter += " AND (eg.CreatedOn < (SELECT top 1 MAX(bac.ClickTime) FROM ECN_Activity..BlastActivityClicks bac with(nolock) where bac.EmailID = eg.EmailID) OR eg.CreatedOn < (SELECT top 1 MAX(bao.OpenTime) FROM ECN_Activity..BlastActivityOpens bao with(nolock) where bao.EmailID = eg.EmailID))";
                //    filter += " AND ((ECN_Activity..BlastActivityClicks.ClickTime BETWEEN '" + fromDate + "' AND '" + toDate + "') OR (ECN_Activity..BlastActivityOpens.OpenTime BETWEEN '" + fromDate + "' AND '" + toDate + "'))";
                //}

                return filter;
            }
        }
        #endregion

        protected void DownloadFilteredEmailsButton_Click(object sender, EventArgs e)
        {
            int FilterID = 0;
            string subscribeType = SubscribeTypeFilter.SelectedItem.Value;
            string emailAddr = EmailFilter.Text;
            string searchType = SearchEmailLike.Text;
            string channelID = chID_Hidden.Value.ToString();
            string customerID = custID_Hidden.Value.ToString();
            string groupID = grpID_Hidden.Value.ToString();
            string downloadType = FilteredDownloadType.SelectedItem.Value;

            string profFilter = "ProfilePlusStandalone";

            string delimiter;
            var filter = PopulateFilter(downloadType, emailAddr, searchType, FilterID, SubscribeTypeAll, ref subscribeType, out delimiter);
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            DateTime.TryParse(DateFromFilter.Text, out fromDate);
            DateTime.TryParse(DateToFilter.Text, out toDate);

            DataTable emailProfilesDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetGroupEmailProfilesWithUDF(Convert.ToInt32(groupID), Convert.ToInt32(customerID), subscribeType, fromDate, toDate, chkRecentActivity.Checked, filter, profFilter);
            string OSFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/downloads/");
            String tfile = customerID + "-" + groupID + "emails" + downloadType;
            string outfileName = OSFilePath + tfile;

            PopulateResponse(OSFilePath, outfileName, downloadType, emailProfilesDT, delimiter, tfile, new HttpResponseWrapper(Response));
        }

        protected DateTime StartOfWeek(DateTime dt)
        {
            DateTime date = new DateTime();
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    date = dt;
                    break;
                case DayOfWeek.Monday:
                    date = dt.AddDays(-1.0d);
                    break;
                case DayOfWeek.Tuesday:
                    date = dt.AddDays(-2.0d);
                    break;
                case DayOfWeek.Wednesday:
                    date = dt.AddDays(-3.0d);
                    break;
                case DayOfWeek.Thursday:
                    date = dt.AddDays(-4.0d);
                    break;
                case DayOfWeek.Friday:
                    date = dt.AddDays(-5.0d);
                    break;
                case DayOfWeek.Saturday:
                    date = dt.AddDays(-6.0d);
                    break;
            }
            return date;
        }

        protected DateTime EndOfWeek(DateTime dt)
        {
            DateTime date = new DateTime();
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    date = dt.AddDays(6.0d);
                    break;
                case DayOfWeek.Monday:
                    date = dt.AddDays(5.0d);
                    break;
                case DayOfWeek.Tuesday:
                    date = dt.AddDays(4.0d);
                    break;
                case DayOfWeek.Wednesday:
                    date = dt.AddDays(3.0d);
                    break;
                case DayOfWeek.Thursday:
                    date = dt.AddDays(2.0d);
                    break;
                case DayOfWeek.Friday:
                    date = dt.AddDays(1.0d);
                    break;
                case DayOfWeek.Saturday:
                    date = dt;
                    break;
            }
            return date;
        }
    }
}