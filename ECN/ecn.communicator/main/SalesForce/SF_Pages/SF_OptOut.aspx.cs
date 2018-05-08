using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using System.Configuration;
using System.Text;
using System.Data;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_OptOut : System.Web.UI.Page
    {
        #region Properties
        public int KMCustomerID
        {
            get
            {
                return Master.UserSession.CurrentUser.CustomerID;
            }
        }
        private List<SF_Contact> listSF
        {
            get
            {
                if (Session["SF_Contacts_OptOut"] != null)
                    return (List<SF_Contact>)Session["SF_Contacts_OptOut"];
                else
                    return null;
            }
            set
            {
                Session["SF_Contacts_OptOut"] = value;
            }
        }
        private List<SF_Lead> listSFLead
        {
            get
            {
                if (Session["SF_Leads_OptOut"] != null)
                    return (List<SF_Lead>)Session["SF_Leads_OptOut"];
                else
                    return null;
            }
            set
            {
                Session["SF_Leads_OptOut"] = value;
            }
        }
        //private List<ECN_Framework_Entities.Communicator.Email> listECN
        //{
        //    get
        //    {
        //        if (Session["ECN_OptOuts"] != null)
        //            return (List<ECN_Framework_Entities.Communicator.Email>)Session["ECN_OptOuts"];
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        Session["ECN_OptOuts"] = value;
        //    }
        //}
        private List<OptOutComp> listComp
        {
            get
            {
                if (Session["ECN_Email_OptOut"] != null)
                    return (List<OptOutComp>)Session["ECN_Email_OptOut"];
                else
                    return null;
            }
            set
            {
                Session["ECN_Email_OptOut"] = value;
            }
        }
        private static int pageNumber;
        private const int pageCount = 30;
        private DataTable dtECN
        {
            get
            {
                if (Session["dtECN"] != null)
                    return (DataTable)Session["dtECN"];
                else
                    return null;
            }
            set
            {
                Session["dtECN"] = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE;
            Master.SubMenu = "Sync";
            Master.Heading = "";
            if (!Page.IsPostBack)
            {
                if (SF_Authentication.LoggedIn == true)
                {
                    pnlOptOut.Visible = true;
                    LoadLists();
                    //DataBindLists();
                }
                else
                {
                    pnlOptOut.Visible = false;
                    ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                }

            }
        }

        private void ShowMessage(string msg, string title, Salesforce.Controls.Message.Message_Icon icon)
        {
            kmMessage.Show(msg, title, icon);
        }

        private void LoadLists()
        {
            //listSF = SF_Contact.GetAll(SF_Authentication.Token.access_token).Where(x => !string.IsNullOrEmpty(x.Email)).ToList();
            //listSFLead = SF_Lead.GetAll(SF_Authentication.Token.access_token).Where(x => !string.IsNullOrEmpty(x.Email)).ToList();

            List<ECN_Framework_Entities.Communicator.Folder> listECNFolder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(KMCustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderType == "GRP").OrderBy(x => x.FolderName).ToList();
            ddlECNFolder.DataSource = listECNFolder;
            ddlECNFolder.DataValueField = "FolderID";
            ddlECNFolder.DataTextField = "FolderName";
            ddlECNFolder.DataBind();
            ddlECNFolder.Items.Insert(0, new ListItem() { Text = "root", Selected = true, Value = "0" });

            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            if (folderID > -1)
            {
                List<ECN_Framework_Entities.Communicator.Group> listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(KMCustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderID == 0).OrderBy(x => x.GroupName).ToList();

                ddlECNGroups.DataSource = listECNGroups;
                ddlECNGroups.DataTextField = "GroupName";
                ddlECNGroups.DataValueField = "GroupID";
                ddlECNGroups.DataBind();

                ddlECNGroups.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "-1" });
            }
        }

        private void LoadEmailLists()
        {
            DataSet dsListEmails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentCustomer.CustomerID, Convert.ToInt32(ddlECNGroups.SelectedValue.ToString()), EmailsPager.CurrentPage, EmailsPager.PageSize, "");
            EmailsPager.RecordCount = Convert.ToInt32(dsListEmails.Tables[0].Rows[0][0].ToString());
            dtECN = dsListEmails.Tables[1];

            StringBuilder sbOptOut = new StringBuilder();
            List<string> lstEmails = new List<string>();
            sbOptOut.Append("WHERE Email in (");

            foreach (DataRow ooc in dtECN.Rows)
            {
                if (!lstEmails.Contains(ooc["EmailAddress"].ToString()))
                {
                    lstEmails.Add(ooc["EmailAddress"].ToString());
                    sbOptOut.Append("'" + ooc["EmailAddress"].ToString() + "',");
                }

            }

            sbOptOut.Append(")");
            listSF = SF_Contact.GetList(SF_Authentication.Token.access_token, sbOptOut.ToString().Replace(",)", ")"));
            listSFLead = SF_Lead.GetList(SF_Authentication.Token.access_token, sbOptOut.ToString().Replace(",)", ")")).Where(x => x.IsConverted == false).ToList();

            listComp = (from ecnCust in dtECN.AsEnumerable()
                        join sfCust in listSF on ecnCust["EmailAddress"].ToString().ToLower() equals sfCust.Email.ToLower()
                        select new OptOutComp { SFOptOut = sfCust.HasOptedOutOfEmail, SFId = sfCust.Id, Email = ecnCust["EmailAddress"].ToString(), ECNOptOut = ecnCust["SubscribeTypeCode"].ToString().Equals("U"), SF_Type = SF_Utilities.SFObject.Contact }).ToList();

            var listLeadComp = (from ecnCust in dtECN.AsEnumerable()
                                join sfLead in listSFLead on ecnCust["EmailAddress"].ToString().ToLower() equals sfLead.Email.ToLower()
                                select new OptOutComp { SFOptOut = sfLead.HasOptedOutOfEmail, SFId = sfLead.Id, Email = ecnCust["EmailAddress"].ToString(), ECNOptOut = ecnCust["SubscribeTypeCode"].ToString().Equals("U"), SF_Type = SF_Utilities.SFObject.Lead }).ToList();

            foreach (OptOutComp ooc in listLeadComp)
            {
                if (!listComp.Contains(ooc))
                {
                    listComp.Add(ooc);
                }
            }
            DataBindLists();
        }

        private void DataBindLists()
        {
            gvOptOut.DataSource = listComp;
            gvOptOut.DataBind();
            ddlFilter.SelectedIndex = 0;

            gvOptOut.Visible = true;
            divOptOut.Visible = true;
        }

        #region Grid View events
        protected void gvOptOut_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (e.SortExpression.ToLower().Equals("email"))
            {
                if (hfSortDirection.Value.ToString().Equals("Ascending"))
                {
                    listComp = listComp.OrderBy(x => x.Email).ToList();
                    hfSortDirection.Value = "Descending";
                }
                else if (hfSortDirection.Value.ToString().Equals("Descending"))
                {
                    listComp = listComp.OrderByDescending(x => x.Email).ToList();
                    hfSortDirection.Value = "Ascending";
                }

            }
            else if (e.SortExpression.ToLower().Equals("type"))
            {
                if (hfSortDirection.Value.ToString().Equals("Ascending"))
                {
                    listComp = listComp.OrderBy(x => x.SF_Type).ToList();
                    hfSortDirection.Value = "Descending";
                }
                else if (hfSortDirection.Value.ToString().Equals("Descending"))
                {
                    listComp = listComp.OrderByDescending(x => x.SF_Type).ToList();
                    hfSortDirection.Value = "Ascending";
                }
            }
            DataBindLists();
        }

        protected void gvOptOut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSF = (CheckBox)e.Row.FindControl("chkSFOptOut");
                CheckBox chkECN = (CheckBox)e.Row.FindControl("chkECNOptOut");

                OptOutComp current = (OptOutComp)e.Row.DataItem;
                SF_Lead sfLead = new SF_Lead();
                SF_Contact sfCont = new SF_Contact();
                DataRow ecnEmail = dtECN.AsEnumerable().First(x => x["EmailAddress"].ToString().ToLower() == current.Email.ToLower());
                if (current.SF_Type == SF_Utilities.SFObject.Lead)
                {
                    sfLead = listSFLead.First(x => x.Email.ToLower() == current.Email.ToLower());
                    chkSF.Attributes.Add("SFId", sfLead.Id);
                    chkSF.Attributes.Add("SFType", SF_Utilities.SFObject.Lead.ToString());
                }
                else if (current.SF_Type == SF_Utilities.SFObject.Contact)
                {
                    sfCont = listSF.First(x => x.Email.ToLower() == current.Email.ToLower());
                    chkSF.Attributes.Add("SFId", sfCont.Id);
                    chkSF.Attributes.Add("SFType", SF_Utilities.SFObject.Contact.ToString());
                }

                chkSF.Checked = current.SFOptOut;
                chkECN.Checked = current.ECNOptOut;
                chkECN.Attributes.Add("ECNId", ecnEmail["EmailID"].ToString());

                if (current.SFOptOut && current.ECNOptOut)
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueDark;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
                else if ((current.ECNOptOut && !current.SFOptOut) || (!current.ECNOptOut && current.SFOptOut))
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ForeColor = System.Drawing.Color.Black;

                }
                else if (!current.SFOptOut && !current.ECNOptOut)
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = System.Drawing.Color.Black;
                    chkECN.Visible = false;
                    chkSF.Visible = false;
                }
            }
        }

        #endregion

        #region UI Events
        protected void EmailsPager_IndexChanged(object sender, EventArgs e)
        {
            pageNumber = EmailsPager.CurrentPage;
            LoadEmailLists();
        }
        protected void ddlECNGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlECNGroups.SelectedIndex > 0)
            {

                DataSet dsListEmails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentCustomer.CustomerID, Convert.ToInt32(ddlECNGroups.SelectedValue.ToString()), 1, EmailsPager.PageSize, "");

                dtECN = dsListEmails.Tables[1];


                StringBuilder sbOptOut = new StringBuilder();
                List<string> lstEmails = new List<string>();
                sbOptOut.Append("WHERE Email in (");

                foreach (DataRow ooc in dtECN.Rows)
                {
                    if (!lstEmails.Contains(ooc["EmailAddress"].ToString()))
                    {
                        lstEmails.Add(ooc["EmailAddress"].ToString());
                        sbOptOut.Append("'" + ooc["EmailAddress"].ToString() + "',");
                    }

                }

                sbOptOut.Append(")");
                listSF = SF_Contact.GetList(SF_Authentication.Token.access_token, sbOptOut.ToString().Replace(",)", ")"));
                listSFLead = SF_Lead.GetList(SF_Authentication.Token.access_token, sbOptOut.ToString().Replace(",)", ")")).Where(x => x.IsConverted == false).ToList();

                listComp = (from ecnCust in dtECN.AsEnumerable()
                            join sfCust in listSF on ecnCust["EmailAddress"].ToString().ToLower() equals sfCust.Email.ToLower()
                            select new OptOutComp { SFOptOut = sfCust.HasOptedOutOfEmail, SFId = sfCust.Id, Email = ecnCust["EmailAddress"].ToString(), ECNOptOut = ecnCust["SubscribeTypeCode"].ToString().Equals("U"), SF_Type = SF_Utilities.SFObject.Contact }).ToList();

                var listLeadComp = (from ecnCust in dtECN.AsEnumerable()
                                    join sfLead in listSFLead on ecnCust["EmailAddress"].ToString().ToLower() equals sfLead.Email.ToLower()
                                    select new OptOutComp { SFOptOut = sfLead.HasOptedOutOfEmail, SFId = sfLead.Id, Email = ecnCust["EmailAddress"].ToString(), ECNOptOut = ecnCust["SubscribeTypeCode"].ToString().Equals("U"), SF_Type = SF_Utilities.SFObject.Lead }).ToList();

                foreach (OptOutComp ooc in listLeadComp)
                {
                    if (!listComp.Contains(ooc))
                    {
                        listComp.Add(ooc);
                    }
                }
                EmailsPager.RecordCount = listComp.Count;
                DataBindLists();
            }
            else
            {
                listComp = new List<OptOutComp>();
                DataBindLists();
            }
        }

        protected void btnSyncOptOut_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> ContactsToOptOut = new Dictionary<string, string>();
            Dictionary<string, string> LeadsToOptOut = new Dictionary<string, string>();
            if (ddlECNGroups.SelectedIndex > 0)
            {
                
                List<ECN_Framework_Entities.Communicator.EmailGroup> listEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByGroupID(Convert.ToInt32(ddlECNGroups.SelectedValue.ToString()), Master.UserSession.CurrentUser);
                foreach (ECN_Framework_Entities.Communicator.EmailGroup eg in listEmailGroup)
                {
                    if (eg.SubscribeTypeCode.Equals("U"))
                    {
                        ECN_Framework_Entities.Communicator.Email emailToOptOut = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(eg.EmailID, eg.GroupID, Master.UserSession.CurrentUser);
                        List<SF_Contact> listsfContact = SF_Contact.GetList(SF_Authentication.Token.access_token, "WHERE Email = '" + emailToOptOut.EmailAddress + "'");
                        List<SF_Lead> listSFLead = SF_Lead.GetList(SF_Authentication.Token.access_token, "WHERE Email = '" + emailToOptOut.EmailAddress + "' AND IsConverted = false");

                        foreach (SF_Contact sf in listsfContact)
                        {
                            if (!sf.HasOptedOutOfEmail)
                            {
                                if (!ContactsToOptOut.ContainsKey(sf.Id))
                                {
                                    ContactsToOptOut.Add(sf.Id, "true");
                                }
                                else
                                {
                                    ContactsToOptOut[sf.Id] = "true";
                                }

                                if(listComp.Exists(x => x.SFId.Equals(sf.Id)))
                                    listComp.First(x => x.SFId.Equals(sf.Id)).SFOptOut = true;
                            }
                        }

                        foreach (SF_Lead sf in listSFLead)
                        {
                            if (!sf.HasOptedOutOfEmail)
                            {
                                if (!LeadsToOptOut.ContainsKey(sf.Id))
                                {
                                    LeadsToOptOut.Add(sf.Id, "true");
                                }
                                else
                                {
                                    LeadsToOptOut[sf.Id] = "true";
                                }
                                if(listComp.Exists(x => x.SFId.Equals(sf.Id)))
                                    listComp.First(x => x.SFId.Equals(sf.Id)).SFOptOut = true;
                            }
                        }
                        
                        //ECN_Framework_Entities.Communicator.EmailGroup ecnEGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(ooc.Email, Convert.ToInt32(ddlECNGroups.SelectedValue.ToString()), Master.UserSession.CurrentUser);
                        //try
                        //{
                        //    if (chkECN.Checked == false && chkSF.Checked == true && ecnEGroup != null)
                        //    {

                        //        ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeSubscribers(ecnEGroup.GroupID, "'" + ooc.Email + "'", Master.UserSession.CurrentUser);
                        //        chkECN.Checked = true;
                        //        ooc.ECNOptOut = true;
                        //        listComp.Remove(ooc);
                        //        listComp.Add(ooc);
                        //        ECNCount++;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    SF_Utilities.LogException(ex);
                        //}

                    }
                }


                string contactBatchID = "";
                bool isContactJobDone = false;
                Dictionary<string, int> contactResults = new Dictionary<string, int>();
                contactResults.Add("success", 0);

                if (ContactsToOptOut.Count > 0)
                {
                    string contactOptOutJob = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.Contact);
                    if (!string.IsNullOrEmpty(contactOptOutJob))
                    {
                        string xmlForOptOut = SF_Contact.GetXMLForOptOutJob(ContactsToOptOut);
                        contactBatchID = SF_Job.AddBatch(SF_Authentication.Token.access_token, contactOptOutJob, xmlForOptOut);
                        SF_Job.Close(SF_Authentication.Token.access_token, contactOptOutJob);
                        if (!string.IsNullOrEmpty(contactBatchID))
                        {
                            while (!isContactJobDone)
                            {
                                System.Threading.Thread.Sleep(2000);
                                isContactJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, contactOptOutJob, contactBatchID);
                            }
                            contactResults["success"] = SF_Job.GetBatchResults(SF_Authentication.Token.access_token, contactOptOutJob, contactBatchID)["success"];
                        }
                        else
                        {
                            ShowMessage("Unable to process job, please ensure that the Salesforce Bulk API is enabled for your organization", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                            return;
                        }
                    }
                    else
                    {
                        ShowMessage("Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                        return;
                    }
                }

                Dictionary<string, int> leadResults = new Dictionary<string, int>();
                leadResults.Add("success", 0);
                if (LeadsToOptOut.Count > 0)
                {
                    string leadBatchID = "";
                    string leadOptOutJob = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.Lead);
                    bool isLeadOptOutDone = false;
                    if (!string.IsNullOrEmpty(leadOptOutJob))
                    {
                        string xmlForOptOut = SF_Lead.GetXMLForOptOutJob(LeadsToOptOut);
                        leadBatchID = SF_Job.AddBatch(SF_Authentication.Token.access_token, leadOptOutJob, xmlForOptOut);
                        SF_Job.Close(SF_Authentication.Token.access_token, leadOptOutJob);
                        if (!string.IsNullOrEmpty(leadBatchID))
                        {
                            while (!isLeadOptOutDone)
                            {
                                System.Threading.Thread.Sleep(2000);
                                isLeadOptOutDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, leadOptOutJob, leadBatchID);
                            }
                            leadResults["success"] = SF_Job.GetBatchResults(SF_Authentication.Token.access_token, leadOptOutJob, leadBatchID)["success"];
                        }
                        else
                        {
                            ShowMessage("Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                            return;
                        }
                    }
                    else
                    {
                        ShowMessage("Unable to create new job, please ensure that the Salesforce Bulk API is enabled for your organization", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                        return;
                    }
                }


                ShowMessage("Sync completed  <br /> " + (contactResults["success"] + leadResults["success"]).ToString() + " Salesforce records updated<br />", "Info", Salesforce.Controls.Message.Message_Icon.info);
                DataBindLists();
            }
            else
            {
                ShowMessage("Please select a group to sync", "ERROR", Salesforce.Controls.Message.Message_Icon.info);
            }
        }

        protected void ddlECNFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            if (folderID > -1)
            {
                List<ECN_Framework_Entities.Communicator.Group> listECNGroups = new List<ECN_Framework_Entities.Communicator.Group>();
                if (folderID == 0)
                {
                    listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderID == folderID).OrderBy(x => x.GroupName).ToList();
                }
                else
                {
                    listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderID(folderID, Master.UserSession.CurrentUser).OrderBy(x => x.GroupName).ToList();
                    listECNGroups = listECNGroups.Where(x => x.CustomerID == Master.UserSession.CurrentUser.CustomerID).ToList();
                }
                ddlECNGroups.DataSource = listECNGroups;
                ddlECNGroups.DataTextField = "GroupName";
                ddlECNGroups.DataValueField = "GroupID";
                ddlECNGroups.DataBind();

                ddlECNGroups.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "-1" });

                listComp = new List<OptOutComp>();
                DataBindLists();
            }
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalChecked = 0;
            if (ddlFilter.SelectedItem.Value.ToLower().Equals("both"))
            {
                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cbSF = (CheckBox)gvr.FindControl("chkSFOptOut");
                        CheckBox cbECN = (CheckBox)gvr.FindControl("chkECNOptOut");
                        if (cbSF.Checked && cbECN.Checked)
                        {
                            gvr.Visible = true;
                            totalChecked++;
                        }
                        else
                            gvr.Visible = false;
                    }
                }
            }
            else if (ddlFilter.SelectedItem.Value.ToLower().Equals("sf"))
            {
                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cbSF = (CheckBox)gvr.FindControl("chkSFOptOut");
                        CheckBox cbECN = (CheckBox)gvr.FindControl("chkECNOptOut");

                        if (cbSF.Checked && !cbECN.Checked)
                        {
                            gvr.Visible = true;
                            totalChecked++;
                        }
                        else
                            gvr.Visible = false;
                    }
                }
            }
            else if (ddlFilter.SelectedItem.Value.ToLower().Equals("ecn"))
            {
                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cbSF = (CheckBox)gvr.FindControl("chkSFOptOut");
                        CheckBox cbECN = (CheckBox)gvr.FindControl("chkECNOptOut");

                        if (cbECN.Checked && !cbSF.Checked)
                        {
                            gvr.Visible = true;
                            totalChecked++;
                        }
                        else
                            gvr.Visible = false;
                    }
                }
            }
            else if (ddlFilter.SelectedItem.Value.ToLower().Equals("none"))
            {
                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox cbSF = (CheckBox)gvr.FindControl("chkSFOptOut");
                        CheckBox cbECN = (CheckBox)gvr.FindControl("chkECNOptOut");

                        if (!cbECN.Checked && !cbSF.Checked)
                        {
                            gvr.Visible = true;
                            totalChecked++;
                        }
                        else
                            gvr.Visible = false;
                    }
                }
            }
            else if (ddlFilter.SelectedItem.Value.ToLower().Equals("all"))
            {
                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        gvr.Visible = true;
                        totalChecked++;
                    }
                }
            }

            if (totalChecked == 0)
            {
                gvOptOut.Visible = false;

            }
            else
            {
                gvOptOut.Visible = true;
            }
        }
        #endregion

        protected void chkSFOptOutAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = (CheckBox)sender;

            foreach (GridViewRow gvr in gvOptOut.Rows)
            {
                OptOutComp ooc = listComp.First(x => x.SFId == gvOptOut.DataKeys[gvr.RowIndex].Value.ToString());
                if (ooc.SFOptOut && !chkSelectAll.Checked)
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSFOptOut");
                    chkSelect.Checked = true;
                }
                else
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSFOptOut");
                    chkSelect.Checked = chkSelectAll.Checked;
                }
            }


        }

        protected void chkECNOptOutAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = (CheckBox)sender;

            foreach (GridViewRow gvr in gvOptOut.Rows)
            {
                OptOutComp ooc = listComp.First(x => x.SFId == gvOptOut.DataKeys[gvr.RowIndex].Value.ToString());
                if (ooc.ECNOptOut && !chkSelectAll.Checked)
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkECNOptOut");
                    chkSelect.Checked = true;
                }
                else
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkECNOptOut");
                    chkSelect.Checked = chkSelectAll.Checked;
                }
            }
        }

        protected void btnSyncSelected_Click(object sender, EventArgs e)
        {
            int SFcount = 0;
            int ECNcount = 0;
            if (ddlECNGroups.SelectedIndex > 0)
            {

                foreach (GridViewRow gvr in gvOptOut.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        bool success = false;

                        CheckBox chkSF = (CheckBox)gvr.FindControl("chkSFOptOut");
                        CheckBox chkECN = (CheckBox)gvr.FindControl("chkECNOptOut");
                        OptOutComp ooc = listComp.SingleOrDefault(x => x.SFId == gvOptOut.DataKeys[gvr.RowIndex].Value.ToString());
                        SF_Contact sfCont = new SF_Contact();
                        SF_Lead sfLead = new SF_Lead();
                        if ((chkECN.Checked == false && chkSF.Checked == true && !ooc.SFOptOut) || (chkSF.Checked == false && chkECN.Checked == true && !ooc.ECNOptOut) || ((chkECN.Checked && chkSF.Checked) && (!ooc.ECNOptOut || !ooc.SFOptOut)))
                        {

                            if (ooc.SF_Type == SF_Utilities.SFObject.Contact && chkSF.Checked && !ooc.SFOptOut)
                            {
                                if (listSF.Exists(x => x.Email.ToLower() == ooc.Email.ToLower()))
                                {
                                    sfCont = listSF.First(x => x.Email.ToLower() == ooc.Email.ToLower());
                                    sfCont.HasOptedOutOfEmail = true;
                                    success = SF_Contact.OptOut(SF_Authentication.Token.access_token, sfCont.Id);
                                }

                            }
                            else if (ooc.SF_Type == SF_Utilities.SFObject.Lead && chkSF.Checked && !ooc.SFOptOut)
                            {
                                if (listSFLead.Exists(x => x.Email.ToLower() == ooc.Email.ToLower()))
                                {
                                    sfLead = listSFLead.First(x => x.Email.ToLower() == ooc.Email.ToLower());
                                    sfLead.HasOptedOutOfEmail = true;
                                    success = SF_Lead.OptOut(SF_Authentication.Token.access_token, sfLead.Id);
                                }

                            }

                            if (success)
                            {
                                chkSF.Checked = true;
                                ooc.SFOptOut = true;
                                listComp.Remove(listComp.First(x => x.Email.ToLower() == ooc.Email.ToLower() && x.SFId == ooc.SFId));
                                listComp.Add(ooc);
                                SFcount++;
                            }

                            ECN_Framework_Entities.Communicator.EmailGroup ecnEGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(ooc.Email, Convert.ToInt32(ddlECNGroups.SelectedValue.ToString()), Master.UserSession.CurrentUser);
                            try
                            {
                                if (chkECN.Checked && !ooc.ECNOptOut && ecnEGroup != null)
                                {
                                    ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeSubscribers(ecnEGroup.GroupID, "'" + ooc.Email + "'", Master.UserSession.CurrentUser);
                                    chkECN.Checked = true;
                                    ooc.ECNOptOut = true;
                                    listComp.Remove(listComp.First(x => x.Email.ToLower() == ooc.Email.ToLower() && x.ECNId == ooc.ECNId));
                                    listComp.Add(ooc);
                                    ECNcount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                SF_Utilities.LogException(ex);
                            }
                        }
                    }
                }
                DisplayResults(SFcount, ECNcount);
                DataBindLists();
            }
            else
            {
                ShowMessage("Please select a group to sync", "ERROR", Salesforce.Controls.Message.Message_Icon.info);
            }
        }
        private void DisplayResults(int sfCount, int ecnCount)
        {
            lblResults.Text = "Sync Completed <br /> " + sfCount.ToString() + " Salesforce records updated.<br />" + ecnCount.ToString() + " ECN records updated.<br />";
            mpeResults.Show();
        }

    }
    public partial class OptOutComp
    {
        public bool SFOptOut { get; set; }
        public string SFId { get; set; }
        public string Email { get; set; }
        public bool ECNOptOut { get; set; }
        public int ECNId { get; set; }
        public SF_Utilities.SFObject SF_Type { get; set; }
    }

}