using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_CampaignActivity : System.Web.UI.Page
    {
        private const string ClickedStatus = "clicked";
        private const string MasterSuppressedStatus = "master suppressed";
        private const string UnsubscribedStatus = "unsubscribed";

        private readonly string[] NotValidStatuses = new string[]
        {
            ClickedStatus,
            MasterSuppressedStatus,
            UnsubscribedStatus
        };

        #region properties
        private List<SF_Campaign> listCamp;
        private Dictionary<string, string> MasterList;
        private Dictionary<string, string> OptOutList;

        private Dictionary<string, string> ToMasterSuppressList;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE;
            Master.SubMenu = "Export";
            Master.Heading = "";
            if (!Page.IsPostBack)
            {
                try
                {
                    if (SF_Authentication.LoggedIn)
                    {

                    }
                    else
                    {
                        pnlCampaignActivity.Visible = false;
                        ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                    }
                }
                catch (Exception ex)
                {

                    SF_Utilities.LogException(ex);
                }
            }
        }

        #region UI Events
        protected void btnSyncCampaign_Click(object sender, EventArgs e)
        {
            if (ddlSFCampaign.SelectedIndex > 0 && ddlECNCampItem.SelectedIndex > 0)
            {
                Dictionary<string, int> Results = new Dictionary<string, int>();
                MasterList = new Dictionary<string, string>();
                OptOutList = new Dictionary<string, string>();
                ToMasterSuppressList = new Dictionary<string, string>();

                List<SF_CampaignMember> listCampMember = SF_CampaignMember.GetList(SF_Authentication.Token.access_token, "WHERE CampaignId = '" + ddlSFCampaign.SelectedValue.ToString() + "'").ToList();
                List<SF_CampaignMemberStatus> liststat = SF_CampaignMemberStatus.GetAll(SF_Authentication.Token.access_token).ToList();
                MemberStatusCheck(ddlSFCampaign.SelectedValue.ToString());

                ActivityResults ar = new ActivityResults();
                ar.SyncSuccess = false;


                int ECNcampaignItemID = -1;
                int.TryParse(ddlECNCampItem.SelectedValue.ToString(), out ECNcampaignItemID);

                List<ECN_Framework_Entities.Communicator.BlastAbstract> blastDT = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(ECNcampaignItemID, Master.UserSession.CurrentUser, false);
                //System.Data.DataTable reportingDT = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDataByCampaignItemID();
                List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> listUnsubs = new List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>();
                List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> listSupp = new List<ECN_Framework_Entities.Activity.BlastActivitySuppressed>();
                List<ECN_Framework_Entities.Activity.BlastActivityOpens> listOpens = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();
                List<ECN_Framework_Entities.Activity.BlastActivityClicks> listClicks = new List<ECN_Framework_Entities.Activity.BlastActivityClicks>();
                List<ECN_Framework_Entities.Activity.BlastActivityBounces> listBounces = new List<ECN_Framework_Entities.Activity.BlastActivityBounces>();


                foreach (ECN_Framework_Entities.Communicator.BlastAbstract bAbstract in blastDT)
                {
                    listUnsubs = ECN_Framework_BusinessLayer.Activity.BlastActivityUnSubscribes.GetByBlastID(bAbstract.BlastID).ToList();
                    //listSupp.AddRange(ECN_Framework_DataLayer.Activity.BlastActivitySuppressed.GetByBlastID(ba.BlastID).ToList());
                    listOpens = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(bAbstract.BlastID).ToList();
                    listClicks = ECN_Framework_BusinessLayer.Activity.BlastActivityClicks.GetByBlastID(bAbstract.BlastID).ToList();
                    listBounces = ECN_Framework_BusinessLayer.Activity.BlastActivityBounces.GetByBlastID(bAbstract.BlastID).ToList();

                    try
                    {
                        ar = TransferOpens(listOpens, listCampMember, bAbstract.GroupID.Value, ar);
                        if (!ar.SyncSuccess)
                            break;
                        ar = TransferClicks(listClicks, listCampMember, bAbstract.GroupID.Value, ar);
                        if (!ar.SyncSuccess)
                            break;
                        ar = TransferUnsubs(listUnsubs, listCampMember, bAbstract.GroupID.Value, ar);
                        if (!ar.SyncSuccess)
                            break;
                        ar = TransferBounces(listBounces, listCampMember, bAbstract.GroupID.Value, ar);
                        if (!ar.SyncSuccess)
                            break;


                        Results.Add("success", 0);
                        bool isCampaignMemberJobDone = false;
                        string jobID = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.CampaignMember);
                        List<string> batchIDs = new List<string>();
                        string xmlForJob = "";
                        int batchCount = 5000;
                        int currentBatch = 0;
                        bool doneBatching = false;
                        Dictionary<string, string> batch = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(jobID))
                        {
                            
                            while (!doneBatching)
                            {
                                if ((currentBatch * batchCount) < MasterList.Count)
                                {
                                    xmlForJob = SF_CampaignMember.GetXMLForUpdateJob((Dictionary<string, string>)MasterList.Skip(batchCount * currentBatch).Take(batchCount));
                                    batchIDs.Add(SF_Job.AddBatch(SF_Authentication.Token.access_token, jobID, xmlForJob));
                                    currentBatch++;
                                }
                                else
                                {
                                    xmlForJob = SF_CampaignMember.GetXMLForUpdateJob((Dictionary<string, string>)MasterList.Skip(batchCount * currentBatch).Take(MasterList.Count - (batchCount * currentBatch)));
                                    batchIDs.Add(SF_Job.AddBatch(SF_Authentication.Token.access_token, jobID, xmlForJob));
                                    currentBatch++;
                                    doneBatching = true;
                                }
                            }
                            
                            SF_Job.Close(SF_Authentication.Token.access_token, jobID);

                            foreach (string s in batchIDs)
                            {
                                while (!isCampaignMemberJobDone)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    isCampaignMemberJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, jobID, s);
                                }
                                Results["success"] += SF_Job.GetBatchResults(SF_Authentication.Token.access_token, jobID, s)["success"];
                                isCampaignMemberJobDone = false;
                            }
                            ar.SyncSuccess = true;
                        }
                        else
                        {
                            ar.ErrorMessage += "Unable to create job, please make sure Salesforce Bulk API is enabled for your organization. ";
                            ar.SyncSuccess = false;

                        }

                        Results.Add("mastersuppress", 0);
                        if (ToMasterSuppressList.Count > 0)
                        {
                            bool isMasterSuppressJobDone = false;
                            string xmlForMSJob = SF_Utilities.GetXMLForMasterSuppressJob(ToMasterSuppressList);
                            string jobMSID = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.CampaignMember);

                            if (!string.IsNullOrEmpty(jobID))
                            {
                                string batchID = SF_Job.AddBatch(SF_Authentication.Token.access_token, jobID, xmlForJob);
                                bool isJobClosed = SF_Job.Close(SF_Authentication.Token.access_token, jobID);

                                while (!isMasterSuppressJobDone)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    isMasterSuppressJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, jobMSID, batchID);
                                }
                                Results["mastersuppress"] = SF_Job.GetBatchResults(SF_Authentication.Token.access_token, jobMSID, batchID)["success"];
                                ar.SyncSuccess = true;
                            }
                            else
                            {
                                ar.ErrorMessage += "Unable to Master Suppress Contacts. ";
                                ar.SyncSuccess = false;
                            }
                        }


                        Results.Add("optout", 0);
                        if (OptOutList.Count > 0)
                        {

                            //Have to opt out contacts and leads separately
                            //Contacts
                            bool isOptOutContactsJobDone = false;
                            Dictionary<string, string> ContactsOptOut = (Dictionary<string, string>)OptOutList.Where(x => x.Value.Equals("contact"));
                            string xmlForOPTOUTContactsJob = SF_Utilities.GetXMLForOptOutJob(ContactsOptOut);
                            string jobContactsOPTOUTID = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.Contact);

                            if (!string.IsNullOrEmpty(jobContactsOPTOUTID))
                            {
                                string batchID = SF_Job.AddBatch(SF_Authentication.Token.access_token, jobContactsOPTOUTID, xmlForOPTOUTContactsJob);
                                bool isJobClosed = SF_Job.Close(SF_Authentication.Token.access_token, jobContactsOPTOUTID);

                                while (!isOptOutContactsJobDone)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    isOptOutContactsJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, jobContactsOPTOUTID, batchID);
                                }
                                Results["optout"] = SF_Job.GetBatchResults(SF_Authentication.Token.access_token, jobContactsOPTOUTID, batchID)["success"];
                                ar.SyncSuccess = true;

                            }
                            else
                            {
                                ar.ErrorMessage += "Unable to Opt-out Contacts. ";
                            }

                            //Leads
                            bool isOptOutLeadsJobDone = false;
                            Dictionary<string, string> LeadsOptOut = (Dictionary<string, string>)OptOutList.Where(x => x.Value.Equals("lead"));
                            string xmlForOPTOUTLeadsJob = SF_Utilities.GetXMLForOptOutJob(LeadsOptOut);
                            string jobLeadsOPTOUTID = SF_Job.Create(SF_Authentication.Token.access_token, "update", SF_Utilities.SFObject.Lead);

                            if (!string.IsNullOrEmpty(jobLeadsOPTOUTID))
                            {
                                string batchID = SF_Job.AddBatch(SF_Authentication.Token.access_token, jobLeadsOPTOUTID, xmlForOPTOUTLeadsJob);
                                bool isJobClosed = SF_Job.Close(SF_Authentication.Token.access_token, jobLeadsOPTOUTID);

                                while (!isOptOutLeadsJobDone)
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    isOptOutLeadsJobDone = SF_Job.GetBatchState(SF_Authentication.Token.access_token, jobLeadsOPTOUTID, batchID);
                                }
                                Results["optout"] += SF_Job.GetBatchResults(SF_Authentication.Token.access_token, jobLeadsOPTOUTID, batchID)["success"];
                                ar.SyncSuccess = true;
                            }
                            else
                            {
                                ar.ErrorMessage += "Unable to Opt-out Leads. ";
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        SF_Utilities.LogException(ex);
                        if (!string.IsNullOrEmpty(ar.ErrorMessage))
                        {
                            ShowMessage(ar.ErrorMessage, "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                        }
                        else
                        {
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SF_CampaignActivity.TransferActivity", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommom_Application"].ToString()));
                            ShowMessage("Error transferring email activity.  Support has been notified.", "ERROR", Salesforce.Controls.Message.Message_Icon.error);

                        }
                        break;
                    }
                }


                if (ar.SyncSuccess)
                {

                    lblBounceTotal.Text = ar.BounceTotal.ToString();

                    lblClickTotal.Text = ar.ClickTotal.ToString();

                    lblOpenTotal.Text = ar.OpenTotal.ToString();

                    lblUnsubTotal.Text = ar.UnsubTotal.ToString();
                    lblUnsubSuccessCount.Text = Results["optout"].ToString();
                    lblMSSuccessCount.Text = Results["mastersuppress"].ToString();
                    lblMSTotal.Text = ar.MSTotal.ToString();
                    lblSuccessTotal.Text = Results["success"].ToString();
                    mpeResults.Show();

                    txtECNFrom.Text = string.Empty;
                    txtECNTo.Text = string.Empty;
                    txtSFFrom.Text = string.Empty;
                    txtSFTo.Text = string.Empty;
                    ddlECNCampItem.Items.Clear();
                    ddlSFCampaign.Items.Clear();
                }
                else
                {
                    ShowMessage(ar.ErrorMessage, "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                }

            }
            else if (ddlECNCampItem.SelectedIndex > 0 && ddlSFCampaign.SelectedIndex < 1)
            {
                kmMsg.Show("Please select a Salesforce campaign to sync to", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
            }
            else if (ddlECNCampItem.SelectedIndex < 1 && ddlSFCampaign.SelectedIndex > 0)
            {
                kmMsg.Show("Please select an ECN campaign item to sync from", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
            }
            else
            {
                kmMsg.Show("Please select campaigns to sync", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
            }
        }


        #endregion

        #region transfer activities
        private ActivityResults TransferUnsubs(List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> listUnsubs, List<SF_CampaignMember> listCampMember, int groupID, ActivityResults ar)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> tempList = new List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes>();
            listUnsubs = listUnsubs.OrderBy(x => x.UnsubscribeCodeID).ToList();
            foreach (ECN_Framework_Entities.Activity.BlastActivityUnSubscribes bau in listUnsubs)
            {
                if (!tempList.Exists(x => x.EmailID == bau.EmailID))
                {
                    tempList.Add(bau);
                }
            }

            ar.UnsubTotal += tempList.Where(x => x.UnsubscribeCodeID != 2).Count();
            ar.MSTotal += tempList.Where(x => x.UnsubscribeCodeID == 2).Count();

            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDV = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();
            List<ECN_Framework_Entities.Communicator.EmailDataValues> listEDV = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();

            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFID = new ECN_Framework_Entities.Communicator.GroupDataFields();
            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFType = new ECN_Framework_Entities.Communicator.GroupDataFields();
            try
            {
                gdfSFID = listGDV.First(x => x.ShortName.ToLower().Equals("sfid"));
                gdfSFType = listGDV.First(x => x.ShortName.ToLower().Equals("sftype"));
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                ar.SyncSuccess = false;
                ar.ErrorMessage = "Could not sync email activity.  Required Salesforce UDFs are missing";
                return ar;
            }

            foreach (ECN_Framework_Entities.Activity.BlastActivityUnSubscribes bau in tempList)
            {
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFID = listEDV.First(x => x.GroupDataFieldsID == gdfSFID.GroupDataFieldsID && x.EmailID == bau.EmailID);
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFType = listEDV.First(x => x.GroupDataFieldsID == gdfSFType.GroupDataFieldsID && x.EmailID == bau.EmailID);

                if (edvSFType.DataValue.ToLower().Equals("contact"))
                {
                    try
                    {
                        //Code to update SF_Contact
                        //Dont want to unsubscribe contact unless master suppressed
                        SF_CampaignMember sfCM = listCampMember.First(x => x.ContactId == edvSFID.DataValue.ToString());
                        if (bau.UnsubscribeCodeID == 2)
                        {
                            try
                            {
                                if (MasterList.ContainsKey(sfCM.Id))
                                {
                                    MasterList[sfCM.Id] = "Master Suppressed";
                                }
                                else
                                {
                                    MasterList.Add(sfCM.Id, "Master Suppressed");
                                }
                                //Need to mark the Contact as Master_Suppress__c
                                if (ToMasterSuppressList.ContainsKey(sfCM.ContactId))
                                {
                                    ToMasterSuppressList[sfCM.ContactId] = "true";
                                }
                                else
                                {
                                    ToMasterSuppressList.Add(sfCM.ContactId, "true");
                                }
                            }
                            catch (Exception ex)
                            {
                                SF_Utilities.LogException(ex);
                            }
                        }
                        else
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                MasterList[sfCM.Id] = "Unsubscribed";
                            }
                            else
                                MasterList.Add(sfCM.Id, "Unsubscribed");

                            if (OptOutList.ContainsKey(sfCM.ContactId))
                                OptOutList[sfCM.ContactId] = "Contact";
                            else
                                OptOutList.Add(sfCM.ContactId, "Contact");
                        }

                    }
                    catch (Exception ex) { SF_Utilities.LogException(ex); }
                }
                else if (edvSFType.DataValue.ToLower().Equals("lead"))
                {
                    try
                    {
                        //Code to update SF_Lead
                        SF_Lead.OptOut(SF_Authentication.Token.access_token, edvSFID.ToString());
                        SF_CampaignMember sfCM = listCampMember.First(x => x.LeadId == edvSFID.DataValue.ToString());
                        if (!sfCM.Status.ToLower().Equals("unsubscribed"))
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                MasterList[sfCM.Id] = "Unsubscribed";
                            }
                            else
                                MasterList.Add(sfCM.Id, "Unsubscribed");

                            if (OptOutList.ContainsKey(sfCM.LeadId))
                            {
                                OptOutList[sfCM.LeadId] = "Lead";
                            }
                            else
                                OptOutList.Add(sfCM.LeadId, "Lead");
                        }
                    }
                    catch (Exception ex) { SF_Utilities.LogException(ex); }
                }
            }
            ar.SyncSuccess = true;
            return ar;
        }

        private ActivityResults TransferOpens(List<ECN_Framework_Entities.Activity.BlastActivityOpens> listOpens, List<SF_CampaignMember> listCampMember, int groupID, ActivityResults ar)
        {

            List<ECN_Framework_Entities.Activity.BlastActivityOpens> tempList = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();
            foreach (ECN_Framework_Entities.Activity.BlastActivityOpens bao in listOpens)
            {
                if (!tempList.Exists(x => x.EmailID == bao.EmailID))
                {
                    tempList.Add(bao);
                }
            }

            ar.OpenTotal += tempList.Count;

            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDV = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();
            List<ECN_Framework_Entities.Communicator.EmailDataValues> listEDV = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();

            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFID = new ECN_Framework_Entities.Communicator.GroupDataFields();
            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFType = new ECN_Framework_Entities.Communicator.GroupDataFields();
            try
            {
                gdfSFID = listGDV.First(x => x.ShortName.ToLower().Equals("sfid"));
                gdfSFType = listGDV.First(x => x.ShortName.ToLower().Equals("sftype"));
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                ar.SyncSuccess = false;
                ar.ErrorMessage = "Could not sync email activity.  Required Salesforce UDFs are missing";
                return ar;
            }

            foreach (ECN_Framework_Entities.Activity.BlastActivityOpens bao in tempList)
            {
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFID = listEDV.First(x => x.GroupDataFieldsID == gdfSFID.GroupDataFieldsID && x.EmailID == bao.EmailID);
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFType = listEDV.First(x => x.GroupDataFieldsID == gdfSFType.GroupDataFieldsID && x.EmailID == bao.EmailID);

                if (edvSFType.DataValue.ToLower().Equals("contact"))
                {
                    try
                    {
                        SF_CampaignMember sfCM = listCampMember.First(x => x.ContactId == edvSFID.DataValue.ToString());
                        if (sfCM.Status == null || (!sfCM.Status.ToString().ToLower().Equals("opened") || !sfCM.Status.ToString().ToLower().Equals("unsubscribed") || !sfCM.Status.ToString().ToLower().Equals("master suppressed") || !sfCM.Status.ToString().ToLower().Equals("clicked")))
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                MasterList[sfCM.Id] = "Opened";
                            }
                            else
                            {
                                MasterList.Add(sfCM.Id, "Opened");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SF_Utilities.LogException(ex);
                    }
                }
                else if (edvSFType.DataValue.ToLower().Equals("lead"))
                {
                    try
                    {
                        SF_CampaignMember sfCM = listCampMember.First(x => x.LeadId == edvSFID.DataValue.ToString());
                        if (sfCM.Status == null || (!sfCM.Status.ToString().ToLower().Equals("opened") || !sfCM.Status.ToString().ToLower().Equals("unsubscribed") || !sfCM.Status.ToString().ToLower().Equals("master suppressed") || !sfCM.Status.ToString().ToLower().Equals("clicked")))
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                MasterList[sfCM.Id] = "Opened";
                            }
                            else
                            {
                                MasterList.Add(sfCM.Id, "Opened");
                            }
                        }
                    }
                    catch (Exception ex) { SF_Utilities.LogException(ex); }
                }
            }
            ar.SyncSuccess = true;
            return ar;
        }

        private ActivityResults TransferClicks(List<ECN_Framework_Entities.Activity.BlastActivityClicks> listClicks, List<SF_CampaignMember> listCampMember, int groupID, ActivityResults ar)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityClicks> tempList = new List<ECN_Framework_Entities.Activity.BlastActivityClicks>();

            foreach (ECN_Framework_Entities.Activity.BlastActivityClicks bac in listClicks)
            {
                if (!tempList.Exists(x => x.EmailID == bac.EmailID))
                {
                    tempList.Add(bac);
                }
            }
            ar.ClickTotal += tempList.Count;

            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDV = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();
            List<ECN_Framework_Entities.Communicator.EmailDataValues> listEDV = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();

            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFID = new ECN_Framework_Entities.Communicator.GroupDataFields();
            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFType = new ECN_Framework_Entities.Communicator.GroupDataFields();

            try
            {
                gdfSFID = listGDV.First(x => x.ShortName.ToLower().Equals("sfid"));
                gdfSFType = listGDV.First(x => x.ShortName.ToLower().Equals("sftype"));
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                ar.SyncSuccess = false;
                ar.ErrorMessage = "Could not sync email activity.  Required Salesforce UDFs are missing";
                return ar;
            }

            foreach (ECN_Framework_Entities.Activity.BlastActivityClicks bac in tempList)
            {
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFID = listEDV.First(x => x.GroupDataFieldsID == gdfSFID.GroupDataFieldsID && x.EmailID == bac.EmailID);
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFType = listEDV.First(x => x.GroupDataFieldsID == gdfSFType.GroupDataFieldsID && x.EmailID == bac.EmailID);

                var campaignMember = GetCampaignMember(listCampMember, edvSFType.DataValue, edvSFID.DataValue);
                if (campaignMember != null)
                {
                    AddClickedValueToMasterList(campaignMember);
                }
            }
            ar.SyncSuccess = true;
            return ar;
        }

        private ActivityResults TransferBounces(List<ECN_Framework_Entities.Activity.BlastActivityBounces> listBounces, List<SF_CampaignMember> listCampMember, int groupID, ActivityResults ar)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityBounces> tempList = new List<ECN_Framework_Entities.Activity.BlastActivityBounces>();
            foreach (ECN_Framework_Entities.Activity.BlastActivityBounces bab in listBounces)
            {
                if (!tempList.Exists(x => x.EmailID == bab.EmailID))
                {
                    tempList.Add(bab);
                }
            }
            ar.BounceTotal += listBounces.Count;

            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDV = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();
            List<ECN_Framework_Entities.Communicator.EmailDataValues> listEDV = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupID(groupID, Master.UserSession.CurrentUser).ToList();

            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFID = new ECN_Framework_Entities.Communicator.GroupDataFields();
            ECN_Framework_Entities.Communicator.GroupDataFields gdfSFType = new ECN_Framework_Entities.Communicator.GroupDataFields();
            try
            {
                gdfSFID = listGDV.First(x => x.ShortName.ToLower().Equals("sfid"));
                gdfSFType = listGDV.First(x => x.ShortName.ToLower().Equals("sftype"));
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                ar.SyncSuccess = false;
                ar.ErrorMessage = "Could not sync email activity.  Required Salesforce UDFs are missing";
                return ar;
            }
            foreach (ECN_Framework_Entities.Activity.BlastActivityBounces bab in tempList)
            {
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFID = listEDV.First(x => x.GroupDataFieldsID == gdfSFID.GroupDataFieldsID && x.EmailID == bab.EmailID);
                ECN_Framework_Entities.Communicator.EmailDataValues edvSFType = listEDV.First(x => x.GroupDataFieldsID == gdfSFType.GroupDataFieldsID && x.EmailID == bab.EmailID);

                if (edvSFType.DataValue.ToLower().Equals("contact"))
                {
                    try
                    {
                        SF_CampaignMember sfCM = listCampMember.First(x => x.ContactId == edvSFID.DataValue.ToString());
                        if (sfCM.Status == null || (!sfCM.Status.ToLower().Equals("hardbounce") && !sfCM.Status.ToLower().Equals("softbounce")))
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                if (bab.BounceCode.ToLower().Equals("hardbounce"))
                                    MasterList[sfCM.Id] = "Hardbounce";
                                else if (bab.BounceCode.ToLower().Equals("softbounce"))
                                    MasterList[sfCM.Id] = "Softbounce";
                            }
                            else
                            {
                                if (bab.BounceCode.ToLower().Equals("hardbounce"))
                                    MasterList.Add(sfCM.Id, "Hardbounce");
                                else if (bab.BounceCode.ToLower().Equals("softbounce"))
                                    MasterList.Add(sfCM.Id, "Softbounce");
                            }
                        }
                    }
                    catch (Exception ex) { SF_Utilities.LogException(ex); }
                }
                else if (edvSFType.DataValue.ToLower().Equals("lead"))
                {
                    try
                    {
                        SF_CampaignMember sfCM = listCampMember.First(x => x.LeadId == edvSFID.DataValue.ToString());
                        if (sfCM.Status == null || (!sfCM.Status.ToLower().Equals("hardbounce") && !sfCM.Status.ToLower().Equals("softbounce")))
                        {
                            if (MasterList.ContainsKey(sfCM.Id))
                            {
                                if (bab.BounceCode.ToLower().Equals("hardbounce"))
                                    MasterList[sfCM.Id] = "Hardbounce";
                                else if (bab.BounceCode.ToLower().Equals("softbounce"))
                                    MasterList[sfCM.Id] = "Softbounce";
                            }
                            else
                            {
                                if (bab.BounceCode.ToLower().Equals("hardbounce"))
                                    MasterList.Add(sfCM.Id, "Hardbounce");
                                else if (bab.BounceCode.ToLower().Equals("softbounce"))
                                    MasterList.Add(sfCM.Id, "Softbounce");
                            }
                        }
                    }
                    catch (Exception ex) { SF_Utilities.LogException(ex); }
                }
            }
            ar.SyncSuccess = true;
            return ar;
        }
        #endregion

        #region SF Member status
        private void MemberStatusCheck(string campaignID)
        {
            List<SF_CampaignMemberStatus> listCampMemberStatus = SF_CampaignMemberStatus.GetList(SF_Authentication.Token.access_token, "WHERE CampaignId = '" + campaignID + "'").ToList();
            List<SF_CampaignMemberStatus> listAll = SF_CampaignMemberStatus.GetAll(SF_Authentication.Token.access_token).ToList();
            if (!listCampMemberStatus.Exists(x => x.Label == "Clicked"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = true;
                bool success = CreateNewMemberStatus(temp, campaignID, "Clicked", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }

            }

            if (!listCampMemberStatus.Exists(x => x.Label == "Opened"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = true;
                bool success = CreateNewMemberStatus(temp, campaignID, "Opened", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }
            }

            if (!listCampMemberStatus.Exists(x => x.Label == "Hardbounce"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = false;
                bool success = CreateNewMemberStatus(temp, campaignID, "Hardbounce", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }
            }

            if (!listCampMemberStatus.Exists(x => x.Label == "Softbounce"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = false;
                bool success = CreateNewMemberStatus(temp, campaignID, "Softbounce", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }
            }

            if (!listCampMemberStatus.Exists(x => x.Label == "Master Suppressed"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = true;
                bool success = CreateNewMemberStatus(temp, campaignID, "Master Suppressed", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }
            }

            if (!listCampMemberStatus.Exists(x => x.Label == "Unsubscribed"))
            {
                SF_CampaignMemberStatus temp = new SF_CampaignMemberStatus();
                temp.HasResponded = true;
                bool success = CreateNewMemberStatus(temp, campaignID, "Unsubscribed", listCampMemberStatus.Last().SortOrder + 1);

                if (success)
                {
                    listCampMemberStatus.Add(temp);
                }
            }
        }

        private bool CreateNewMemberStatus(SF_CampaignMemberStatus sfCMS, string campaignID, string label, int sortOrder)
        {

            sfCMS.CampaignId = campaignID;
            sfCMS.Label = label;
            sfCMS.SortOrder = sortOrder;
            sfCMS.IsDefault = false;
            sfCMS.IsDeleted = false;
            try
            {
                bool success = SF_CampaignMemberStatus.Insert(SF_Authentication.Token.access_token, sfCMS);
                return success;
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                return false;
            }
        }
        #endregion

        private void ShowMessage(string msg, string title, Salesforce.Controls.Message.Message_Icon icon)
        {
            kmMsg.Show(msg, title, icon);

        }

        protected void btnECNDateFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtECNFrom.Text.Trim()) && !string.IsNullOrEmpty(txtECNTo.Text.Trim()))
            {
                DateTime from = new DateTime();
                DateTime to = new DateTime();
                DateTime.TryParse(txtECNFrom.Text.Trim(), out from);
                DateTime.TryParse(txtECNTo.Text.Trim(), out to);
                to.AddDays(1);
                DataTable dtECN = new DataTable();
                dtECN = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetSentCampaignItems("", "", "", "", "", -1, from, to, null, false, Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
                dtECN.DefaultView.Sort = "CampaignItemName";
                dtECN = dtECN.DefaultView.ToTable();

                ddlECNCampItem.DataSource = dtECN;
                ddlECNCampItem.DataTextField = "CampaignItemName";
                ddlECNCampItem.DataValueField = "CampaignItemID";
                ddlECNCampItem.DataBind();
                ddlECNCampItem.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "-1" });
            }
            else
            {

            }
        }

        protected void btnSFDateFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSFFrom.Text.Trim()) && !string.IsNullOrEmpty(txtSFTo.Text.Trim()))
            {
                DateTime from = new DateTime();
                DateTime to = new DateTime();
                DateTime.TryParse(txtSFFrom.Text.Trim(), out from);
                DateTime.TryParse(txtSFTo.Text.Trim(), out to);
                to = to.AddDays(1);

                listCamp = SF_Campaign.GetList(SF_Authentication.Token.access_token, "WHERE CreatedDate >= " + from.ToString("yyyy-MM-ddThh:mm:ssZ") + " and CreatedDate <= " + to.ToString("yyyy-MM-ddThh:mm:ssZ")).OrderBy(x => x.Name).ToList();


                ddlSFCampaign.DataSource = listCamp;
                ddlSFCampaign.DataTextField = "Name";
                ddlSFCampaign.DataValueField = "ID";
                ddlSFCampaign.DataBind();
                ddlSFCampaign.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "-1" });
            }
            else
            {

            }
        }

        private SF_CampaignMember GetCampaignMember(List<SF_CampaignMember> members, string memberType, string id)
        {
            const string Contact = "contact";
            const string Lead = "lead";

            if (memberType.Equals(Contact, StringComparison.OrdinalIgnoreCase))
            {
                return members.FirstOrDefault(x => x.ContactId == id);
            }
            else if (memberType.Equals(Lead, StringComparison.OrdinalIgnoreCase))
            {
                return members.FirstOrDefault(x => x.LeadId == id);
            }

            return null;
        }

        private void AddClickedValueToMasterList(SF_CampaignMember member)
        {
            const string ClickedValue = "Clicked";

            try
            {
                if (member.Status == null || !NotValidStatuses.Contains(member.Status.ToLower()))
                {
                    if (MasterList.ContainsKey(member.Id))
                    {
                        MasterList[member.Id] = ClickedValue;
                    }
                    else
                    {
                        MasterList.Add(member.Id, ClickedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
            }
        }
    }
    public partial class SF_Suppression
    {
        public string SFID { get; set; }
        public string Email { get; set; }
        public int ECNID { get; set; }
        public string SFType { get; set; }
        public bool ECNSupp { get; set; }
        public bool SFSupp { get; set; }
    }
    public partial class ActivityResults
    {
        public int UnsubTotal { get; set; }
        public int UnsubSuccess { get; set; }
        public int BounceTotal { get; set; }
        public int BounceSuccess { get; set; }
        public int OpenTotal { get; set; }
        public int OpenSuccess { get; set; }
        public int ClickTotal { get; set; }
        public int ClickSuccess { get; set; }
        public int MSTotal { get; set; }
        public int MSSuccess { get; set; }
        public bool SyncSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

}