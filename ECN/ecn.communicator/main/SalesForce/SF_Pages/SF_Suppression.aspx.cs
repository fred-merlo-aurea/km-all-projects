using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using System.Configuration;
using System.Data;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_Suppression1 : SessionStoragePage
    {
        private const string SortDirectionKey = "sortDir";
        private const string SortExpressionKey = "sortExp";
        #region properties
        public int KMCustomerID
        {
            get
            {
                return Master.UserSession.CurrentUser.CustomerID;
            }
        }
        private List<ECN_Framework_Entities.Communicator.Email> ECN_SuppList
        {
            get
            {
                if (Session["ECN_SuppList"] != null)
                    return (List<ECN_Framework_Entities.Communicator.Email>)Session["ECN_SuppList"];
                else
                    return null;
            }
            set
            {
                Session["ECN_SuppList"] = value;
            }
        }
        private List<SF_Contact> SF_SuppList
        {
            get
            {
                if (Session["SF_SuppList"] != null)
                    return (List<SF_Contact>)Session["SF_SuppList"];
                else
                    return null;
            }
            set
            {
                Session["SF_SuppList"] = value;
            }
        }
        private List<SuppList> listSupp
        {
            get
            {
                if (Session["ListSupp"] != null)
                    return (List<SuppList>)Session["ListSupp"];
                else
                    return null;
            }
            set
            {
                Session["ListSupp"] = value;
            }
        }
        private SortDirection sortDir
        {
            get
            {
                return Get(SortDirectionKey, SortDirection.Ascending);
            }

            set
            {
                Set(SortDirectionKey, value);
            }
        }
        private string sortExp
        {
            get
            {
                return Get<string>(SortExpressionKey, null);
            }

            set
            {
                Set(SortExpressionKey, value);
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
                    pnlSuppression.Visible = true;
                    listSupp = new List<SuppList>();
                    //LoadSuppressionList();
                    BindSearch();
                    
                    txtFrom.Text = DateTime.Now.AddDays(-90).ToShortDateString();
                    txtTo.Text = DateTime.Now.ToShortDateString();


                    List<ECN_Framework_Entities.Communicator.Campaign> campaignList =
                        ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    var resultCampaignList = (from src in campaignList
                                              orderby src.CampaignName
                                              select src).ToList();
                    drpCampaign.DataSource = resultCampaignList;
                    drpCampaign.DataBind();
                    drpCampaign.Items.Insert(0, new ListItem("-- All --", "0"));

                    List<KMPlatform.Entity.User> userList =
                    KMPlatform.BusinessLogic.User.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
                    var resultUserList = (from src in userList
                                          orderby src.UserName
                                          select src).ToList();
                    drpSentUser.DataSource = resultUserList;
                    drpSentUser.DataBind();
                    drpSentUser.Items.Insert(0, new ListItem("-- All --", "0"));
                }
                else
                {
                    pnlSuppression.Visible = false;
                    ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                }
            }
        }

        #region Load Lists
        private void LoadSuppressionList()
        {
            SF_SuppList = SF_Contact.GetAll(SF_Authentication.Token.access_token).ToList();
            //need currently logged in users CustomerID
            ECN_Framework_Entities.Communicator.Group suppGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            int subCount = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetSubscriberCount(suppGroup.GroupID, Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
            DataSet dsECNMaster = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentCustomer.CustomerID, suppGroup.GroupID, 1, subCount, "");
            DataTable dtECNMaster = dsECNMaster.Tables[1];
            ECN_SuppList = dtECNMaster.AsEnumerable().Select(row => new ECN_Framework_Entities.Communicator.Email()
            {
                EmailAddress = row["EmailAddress"].ToString(),
                EmailID = Convert.ToInt32(row["EmailID"].ToString())
            }).ToList();
            //ECN_SuppList = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(suppGroup.GroupID, Master.UserSession.CurrentUser).ToList();
            foreach (SF_Contact e in SF_SuppList)
            {
                if (!string.IsNullOrEmpty(e.Email))
                {
                    SuppList sl = new SuppList();
                    sl.Email = e.Email.Trim();
                    sl.SFId = e.Id;
                    sl.SFSupp = e.Master_Suppressed__c;

                    //ECN_Framework_Entities.Communicator.Email emCheck = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(sl.Email, Master.UserSession.CurrentCustomer.CustomerID);

                    if (ECN_SuppList.Exists(x => x.EmailAddress == e.Email))
                    {
                        sl.ECNId = ECN_SuppList.SingleOrDefault(x => x.EmailAddress == e.Email).EmailID;
                        sl.ECNSupp = true;
                    }
                    else
                    {
                        sl.ECNId = 0;
                        sl.ECNSupp = false;
                    }

                    listSupp.Add(sl);
                }
            }
            listSupp = listSupp.OrderBy(x => x.Email).ToList();
            DataBindList();
        }

        #endregion

        private void DataBindList()
        {
            ddlFilter.SelectedIndex = 0;
            gvSuppression.DataSource = listSupp;
            gvSuppression.DataBind();

            if (listSupp.Count > 0)
            {
                btnSyncSelected.Visible = true;
                gvSuppression.Visible = true;
            }
            else
            {
                btnSyncSelected.Visible = false;
                gvSuppression.Visible = false;
                
            }
        }

        private void ShowMessage(string msg, string title, Salesforce.Controls.Message.Message_Icon icon)
        {
            kmMessage.Show(msg, title, icon);
        }

        protected void gvSuppression_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSF = (CheckBox)e.Row.FindControl("chkSFSupp");
                CheckBox chkECN = (CheckBox)e.Row.FindControl("chkECNSupp");

                SuppList current = (SuppList)e.Row.DataItem;
                SF_Contact sfCont = new SF_Contact();
                ECN_Framework_Entities.Communicator.Email ecnEmail = ECN_SuppList.SingleOrDefault(x => x.EmailAddress.ToLower() == current.Email.ToLower());

                chkSF.Attributes.Add("SFId", current.SFId);
                chkECN.Attributes.Add("ECNId", current.ECNId.ToString());

                chkSF.Checked = current.SFSupp;
                chkECN.Checked = current.ECNSupp;
                if (chkSF.Checked)
                    chkSF.Enabled = false;
                if (chkECN.Checked)
                    chkECN.Enabled = false;

                if (current.ECNSupp && current.SFSupp)
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueDark;
                    e.Row.ForeColor = System.Drawing.Color.White;

                }
                else if ((current.ECNSupp && !current.SFSupp) || (!current.ECNSupp && current.SFSupp))
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ForeColor = System.Drawing.Color.Black;
                    if (current.ECNId == 0)
                    {
                        chkECN.Visible = false;
                    }

                }
                else if (!current.ECNSupp && !current.SFSupp)
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = System.Drawing.Color.Black;
                    if (current.ECNId == 0)
                        chkECN.Visible = false;
                }

            }
        }


        private void BindSearch()
        {
            kmSearch.BindSearch(SF_Utilities.SFObject.Contact);
        }
        #region UI Events
        protected void btnGetQuery_Click(object sender, EventArgs e)
        {
            string where = kmSearch.GetQuery();
            List<SF_Contact> search = SF_Contact.GetList(SF_Authentication.Token.access_token, where).Where(x => !string.IsNullOrEmpty(x.Email)).ToList();

            if (ECN_SuppList == null)
            {
                
                ECN_Framework_Entities.Communicator.Group suppGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
                int subCount = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetSubscriberCount(suppGroup.GroupID, Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
                DataSet dsECNMaster = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentCustomer.CustomerID, suppGroup.GroupID, 1, subCount, "");
                DataTable dtECNMaster = dsECNMaster.Tables[1];
                ECN_SuppList = dtECNMaster.AsEnumerable().Select(row => new ECN_Framework_Entities.Communicator.Email()
                {
                    EmailAddress = row["EmailAddress"].ToString(),
                    EmailID = Convert.ToInt32(row["EmailID"].ToString())
                }).ToList();
            }

            listSupp.Clear();
            

            //ECN_SuppList = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(suppGroup.GroupID, Master.UserSession.CurrentUser).ToList();
            foreach (SF_Contact con in search)
            {
                SuppList sl = new SuppList();
                sl.Email = con.Email.Trim();
                sl.SFId = con.Id;
                sl.SFSupp = con.Master_Suppressed__c;

                if (ECN_SuppList.Exists(x => x.EmailAddress == con.Email))
                {
                    sl.ECNId = ECN_SuppList.SingleOrDefault(x => x.EmailAddress == con.Email).EmailID;
                    sl.ECNSupp = true;
                }

                listSupp.Add(sl);
            }
            DataBindList();

            divSuppression.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            kmSearch.ResetSearch();
            //LoadSuppressionList();
            drpBlastType.SelectedIndex = 0;
            drpCampaign.SelectedIndex = 0;
            drpSearchCriteria.SelectedIndex = 0;
            drpSentUser.SelectedIndex = 0;
            txtFrom.Text = DateTime.Now.AddDays(-90).ToShortDateString();
            txtTo.Text = DateTime.Now.ToShortDateString();
            txtSearch.Text = string.Empty;
            if (ddlECNCampaign.Items.Count > 0)
                ddlECNCampaign.Items.Clear();

            ECN_SuppList = new List<ECN_Framework_Entities.Communicator.Email>();
            SF_SuppList = new List<SF_Contact>();
            listSupp = new List<SuppList>();
            DataBindList();
        }

        protected void ddlECNCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ECN_SuppList = new List<ECN_Framework_Entities.Communicator.Email>();
            listSupp = new List<SuppList>();
            if (ddlECNCampaign.SelectedIndex > 0)
            {
                int campaignItemID = -1;
                int.TryParse(ddlECNCampaign.SelectedValue.ToString(), out campaignItemID);

                List<ECN_Framework_Entities.Communicator.BlastAbstract> listBA = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(campaignItemID, Master.UserSession.CurrentUser, false);
                List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> listSuppressed = new List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>();
                foreach (ECN_Framework_Entities.Communicator.BlastAbstract ba in listBA)
                {
                    listSuppressed.AddRange(ECN_Framework_BusinessLayer.Activity.BlastActivityUnSubscribes.GetByBlastID(ba.BlastID).Where(x => x.UnsubscribeCodeID == 2).ToList());

                }

                foreach (ECN_Framework_Entities.Activity.BlastActivityUnSubscribes bas in listSuppressed)
                {
                    ECN_Framework_Entities.Communicator.Email ecnEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(bas.EmailID, Master.UserSession.CurrentUser);
                    ECN_SuppList.Add(ecnEmail);
                    SuppList sl = new SuppList();
                    sl.ECNId = bas.EmailID;
                    sl.ECNSupp = true;
                    sl.Email = ecnEmail.EmailAddress;
                    SF_Contact sf = SF_Contact.GetSingle(SF_Authentication.Token.access_token, "WHERE Email = '" + sl.Email + "'");
                    if (!string.IsNullOrEmpty(sf.Id))
                    {
                        sl.SFId = sf.Id;
                        sl.SFSupp = sf.Master_Suppressed__c;
                    }
                    listSupp.Add(sl);
                }

                DataBindList();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int? userID = null;
            if (Convert.ToInt32(drpSentUser.SelectedValue) > 0)
                userID = Convert.ToInt32(drpSentUser.SelectedValue);
            string campaignName = string.Empty;
            if (Convert.ToInt32(drpCampaign.SelectedValue) > 0)
                campaignName = drpCampaign.SelectedItem.ToString().Trim();
            string emailSubject = string.Empty;
            string groupName = string.Empty;
            string layoutName = string.Empty;
            int blastID = 0;
            string campaignItemName = string.Empty;
            if (drpSearchCriteria.SelectedValue.Equals("CampaignItem"))
            {
                campaignItemName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Subject"))
            {
                emailSubject = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Message"))
            {
                layoutName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("Group"))
            {
                groupName = txtSearch.Text;
            }
            else if (drpSearchCriteria.SelectedValue.Equals("BlastID"))
            {
                int.TryParse(txtSearch.Text, out blastID);
            }
            dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetSentCampaignItems(campaignName, campaignItemName, emailSubject, layoutName, groupName, blastID, Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text), userID, Convert.ToBoolean(drpBlastType.SelectedValue), Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            dt.DefaultView.Sort = "CampaignItemName";
            dt = dt.DefaultView.ToTable();

            ddlECNCampaign.DataSource = dt;
            ddlECNCampaign.DataValueField = "CampaignItemID";
            ddlECNCampaign.DataTextField = "CampaignItemName";
            ddlECNCampaign.DataBind();

            ddlECNCampaign.Items.Insert(0, new ListItem() { Text = "--SELECT--", Selected = true, Value = "-1" });
        }

        protected void btnSyncSuppression_Click(object sender, EventArgs e)
        {
            int SFMaster = 0;

            List<SF_Contact> contactsToMS = new List<SF_Contact>();
            //int ECNMaster = 0;
            List<SF_Contact> allSF_Contacts = SF_Contact.GetListForMS(SF_Authentication.Token.access_token, "WHERE Email != '' AND Master_Suppressed__c = FALSE");
            List<ECN_Framework_Entities.Communicator.Email> ecnMaster = new List<ECN_Framework_Entities.Communicator.Email>();
            ECN_Framework_Entities.Communicator.Group suppGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            int subCount = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetSubscriberCount(suppGroup.GroupID, Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
            DataSet dsECNMaster = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetBySearchStringPaging(Master.UserSession.CurrentCustomer.CustomerID, suppGroup.GroupID, 1, subCount, "");
            DataTable dtECNMaster = dsECNMaster.Tables[1];
            ecnMaster = dtECNMaster.AsEnumerable().Select(row => new ECN_Framework_Entities.Communicator.Email()
            {
                EmailAddress = row["EmailAddress"].ToString(),
                EmailID = Convert.ToInt32(row["EmailID"].ToString())
            }).ToList();
            foreach (ECN_Framework_Entities.Communicator.Email em in ecnMaster)
            {
                List<SF_Contact> listSFContact = allSF_Contacts.Where(x => x.Email.ToLower().Equals(em.EmailAddress.ToLower())).ToList();
                foreach (SF_Contact sf in listSFContact)
                {
                    if (!contactsToMS.Exists(x => x.Id.Equals(sf.Id)))
                    {
                        sf.Master_Suppressed__c = true;
                        contactsToMS.Add(sf);

                    }

                }
            }

            if (contactsToMS.Count > 0)
            {
                string contactJobID = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.Contact);
                bool isMSJobDone = false;
                Dictionary<string, int> MSResults = new Dictionary<string, int>();
                MSResults.Add("success", 0);
                int batchCount = 5000;
                int currentBatch = 0;
                bool doneBatching = false;
                List<string> batchIDs = new List<string>();
                if (!string.IsNullOrEmpty(contactJobID))
                {
                    while (!doneBatching)
                    {
                        if ((currentBatch * batchCount) < contactsToMS.Count)
                        {
                            string xmlForJob = SF_Contact.GetXMLForMSJob(contactsToMS.Skip(currentBatch * batchCount).Take(batchCount).ToList());
                            batchIDs.Add(SF_Job.AddBatch(SF_Authentication.Token.access_token, contactJobID, xmlForJob));
                        }
                        else
                        {
                            string xmlForJob = SF_Contact.GetXMLForMSJob(contactsToMS.Skip(currentBatch * batchCount).Take(contactsToMS.Count - (currentBatch * batchCount)).ToList());
                            batchIDs.Add(SF_Job.AddBatch(SF_Authentication.Token.access_token, contactJobID, xmlForJob));
                            doneBatching = true;
                        }
                    }

                    SF_Job.Close(SF_Authentication.Token.access_token, contactJobID);
                    if (batchIDs.Count > 0)
                    {
                        foreach (string s in batchIDs)
                        {
                            while (!isMSJobDone)
                            {
                                System.Threading.Thread.Sleep(2000);
                                isMSJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, contactJobID, s);
                            }
                            MSResults["success"] += SF_Job.GetBatchResults(SF_Authentication.Token.access_token, contactJobID, s)["success"];
                            isMSJobDone = false;
                        }
                    }
                    else
                    {
                        ShowMessage("Unable to process batch", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                        return;
                    }
                }
                else
                {
                    ShowMessage("Unable to create job, please ensure Salesforce Bulk API is enabled for your organization", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                    return;
                }


                lblSFResults.Text = "SF records suppressed: " + MSResults["success"].ToString();
                //lblECNResults.Text = "ECN records suppressed: " + ECNMaster.ToString();
                mpeResults.Show();
            }
            else
            {
                ShowMessage("No Contacts to Master Suppress", "INFO", Salesforce.Controls.Message.Message_Icon.info);
            }
        }

        #endregion

        protected void btnSyncSelected_Click(object sender, EventArgs e)
        {
            try
            {
                int SFMaster = 0;
                //int ECNMaster = 0;
                foreach (GridViewRow gvr in gvSuppression.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        SuppList sl = listSupp.First(x => x.SFId == gvSuppression.DataKeys[gvr.RowIndex].Value.ToString());
                        CheckBox chkSF = (CheckBox)gvr.FindControl("chkSFSupp");
                        CheckBox chkECN = (CheckBox)gvr.FindControl("chkECNSupp");

                        if ((chkSF.Checked && !sl.SFSupp) || (chkECN.Checked && !sl.ECNSupp))
                        {
                            if (!sl.SFSupp)
                            {
                                //SF_Contact sf = SF_Contact.GetSingle(SF_Authentication.Token.access_token, "WHERE Id = '" + sl.SFId + "'");
                                //sf.Master_Suppressed__c = true;

                                if (SF_Contact.MasterSuppress(SF_Authentication.Token.access_token, sl.SFId))
                                {
                                    sl.SFSupp = true;
                                    SFMaster++;
                                }
                            }

                            if (!sl.ECNSupp)
                            {
                                //try
                                //{
                                //    ECN_Framework_Entities.Communicator.Email eToSuppress = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(sl.Email, Master.UserSession.CurrentCustomer.CustomerID);
                                //    if (eToSuppress != null && eToSuppress.EmailID > 0)
                                //    {
                                //        ECN_Framework_BusinessLayer.Communicator.EmailGroup.AddToMasterSuppression(Master.UserSession.CurrentUser.CustomerID, eToSuppress.EmailID, Master.UserSession.CurrentUser);
                                //        sl.ECNSupp = true;
                                //        ECNMaster++;
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    SF_Utilities.LogException(ex);
                                //}
                            }
                        }

                    }
                }
                gvSuppression.DataSource = listSupp;
                gvSuppression.DataBind();

                lblSFResults.Text = "SF records suppressed: " + SFMaster.ToString();
                //lblECNResults.Text = "ECN records suppressed: " + ECNMaster.ToString();
                mpeResults.Show();
            }
            catch(Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "SF_Suppression.btn_SyncSelected_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                
            }
        }

        protected void gvSuppression_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<SuppList> tempSort = new List<SuppList>();
            foreach (GridViewRow gvr in gvSuppression.Rows)
            {
                if (gvr.Visible)
                {
                    SuppList sl = listSupp.First(x => x.SFId == gvSuppression.DataKeys[gvr.RowIndex].Value.ToString());
                    tempSort.Add(sl);
                }
            }
            if (Session["sortExp"] != null && sortExp.Equals(e.SortExpression))
            {
                if (sortDir == SortDirection.Ascending)
                {
                    sortDir = SortDirection.Descending;
                }
                else
                    sortDir = SortDirection.Ascending;
            }
            else
            {
                sortExp = e.SortExpression;
                sortDir = SortDirection.Ascending;
            }

            if (sortDir == SortDirection.Ascending)
                tempSort = tempSort.OrderBy(x => x.Email).ToList();
            else
                tempSort = tempSort.OrderByDescending(x => x.Email).ToList();

            gvSuppression.DataSource = tempSort;
            gvSuppression.DataBind();
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFilter.SelectedIndex > 0)
            {
                List<SuppList> tempFilter = new List<SuppList>();
                string filter = ddlFilter.SelectedValue.ToString();
                switch (filter)
                {
                    case "ecn":
                        foreach (SuppList sl in listSupp)
                        {
                            
                            if (!sl.SFSupp && sl.ECNSupp)
                            {
                                tempFilter.Add(sl);
                            }

                        }
                        break;
                    case "sf":
                        foreach (SuppList sl in listSupp)
                        {

                            if (sl.SFSupp && !sl.ECNSupp)
                            {
                                tempFilter.Add(sl);
                            }

                        }
                        break;
                    case "both":
                        foreach (SuppList sl in listSupp)
                        {

                            if (sl.SFSupp && sl.ECNSupp)
                            {
                                tempFilter.Add(sl);
                            }

                        }
                        break;
                    case "none":
                        foreach (SuppList sl in listSupp)
                        {
                            if (!sl.ECNSupp && !sl.SFSupp)
                            {
                                tempFilter.Add(sl);
                            }
                        }
                        break;
                }
                if (tempFilter.Count > 0)
                {
                    btnSyncSuppression.Enabled = true;
                    btnSyncSelected.Enabled = true;
                }
                else
                {
                    btnSyncSuppression.Enabled = false;
                    btnSyncSelected.Enabled = false;
                }
                gvSuppression.DataSource = tempFilter;
                gvSuppression.DataBind();
            }
            else
            {
                if (listSupp.Count > 0)
                {
                    btnSyncSuppression.Enabled = true;
                    btnSyncSelected.Enabled = true;
                }
                else
                {
                    btnSyncSuppression.Enabled = false;
                    btnSyncSelected.Enabled = false;
                }
                gvSuppression.DataSource = listSupp;
                gvSuppression.DataBind();
            }
        }
        
    }
    public partial class SuppList
    {
        public string SFId { get; set; }
        public int ECNId { get; set; }
        public string Email { get; set; }
        public bool SFSupp { get; set; }
        public bool ECNSupp { get; set; }
    }

}