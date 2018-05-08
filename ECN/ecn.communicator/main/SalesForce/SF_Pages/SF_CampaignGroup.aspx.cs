using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.SF_Pages.Converters;
using ecn.communicator.main.Salesforce.Constants;
using ecn.communicator.main.Helpers;
using CommunicatorGroup = ECN_Framework_Entities.Communicator.Group;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_CampaignGroup : System.Web.UI.Page
    {
        private const int OneThousand = 1000;

        private Hashtable hUpdatedRecords = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE;
            Master.SubMenu = "Import";
            Master.Heading = "";

            if (!Page.IsPostBack)
            {
                try
                {
                    if (SF_Authentication.LoggedIn == true)
                    {

                        LoadLists();
                    }
                    else
                    {
                        pnlCampGroup.Visible = false;
                        ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                    }
                }
                catch (Exception ex)
                {
                    SF_Utilities.LogException(ex);
                }
            }
        }

        private void LoadLists()
        {
            List<SF_Campaign> listCampaigns = new List<SF_Campaign>();
            //listCampaigns = SF_Campaign.GetAll(SF_Authentication.Token.access_token).OrderBy(x => x.Name).ToList();

            List<ECN_Framework_Entities.Communicator.Folder> listFolder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderType == "GRP").OrderBy(x => x.FolderName).ToList();

            ddlECNFolder.DataSource = listFolder;
            ddlECNFolder.DataValueField = "FolderID";
            ddlECNFolder.DataTextField = "FolderName";
            ddlECNFolder.DataBind();
            ddlECNFolder.Items.Insert(0, new ListItem() { Text = "root", Value = "0", Selected = true });

            List<ECN_Framework_Entities.Communicator.Group> listECNGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderID == 0).OrderBy(x => x.GroupName).ToList();

            ddlExistingGroup.DataSource = listECNGroup;
            ddlExistingGroup.DataTextField = "GroupName";
            ddlExistingGroup.DataValueField = "GroupID";
            ddlExistingGroup.DataBind();
            ddlExistingGroup.Items.Insert(0, new ListItem() { Text = "--SELECT--", Value = "-1", Selected = true });

            //ddlSFCampaign.DataSource = listCampaigns;
            //ddlSFCampaign.DataValueField = "ID";
            //ddlSFCampaign.DataTextField = "Name";
            //ddlSFCampaign.DataBind();
            //ddlSFCampaign.Items.Insert(0, new ListItem() { Text = "--SELECT--", Value = "-1", Selected = true });


        }

        #region UI events
        protected void ddlSFCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSyncButton();

        }

        private void CheckSyncButton()
        {
            if (ddlSFCampaign.SelectedIndex > 0 && ((rblECNGroup.SelectedValue.ToLower().Equals("existing") && ddlExistingGroup.SelectedIndex > 0) || (rblECNGroup.SelectedValue.ToLower().Equals("new"))))
            {
                btnSyncGroup.Enabled = true;
            }
            else
            {
                btnSyncGroup.Enabled = false;
            }
        }

        protected void rblECNGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblECNGroup.SelectedValue.ToLower().Equals("new"))
            {

                ddlExistingGroup.Visible = false;
                lblExistingGroup.Visible = false;
                txtNewGroup.Visible = true;
                lblNewGroup.Visible = true;
            }
            else if (rblECNGroup.SelectedValue.ToLower().Equals("existing"))
            {
                ddlExistingGroup.SelectedIndex = -1;
                ddlExistingGroup.Visible = true;
                lblExistingGroup.Visible = true;
                txtNewGroup.Visible = false;
                lblNewGroup.Visible = false;
            }
            CheckSyncButton();
        }

        protected void btnSyncGroup_Click(object sender, EventArgs e)
        {
            if (ddlSFCampaign.SelectedIndex <= 0)
            {
                return;
            }

            hUpdatedRecords = new Hashtable();
            int groupId;
            int.TryParse(ddlExistingGroup.SelectedValue, out groupId);

            CommunicatorGroup ecnGroup;
            Hashtable hGdfFields;

            if (rblECNGroup.SelectedValue.Equals("new", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(txtNewGroup.Text))
                {
                    try
                    {
                        ecnGroup = CreateECNGroup(SF_Utilities.CleanStringSqlInjection(txtNewGroup.Text.Trim()));
                        if (ecnGroup.GroupID > 0)
                        {
                            hGdfFields = CreateUDF(ecnGroup.GroupID);
                        }
                        else
                        {
                            return;
                        }

                        var builder = new EmailBatchBuilder(
                            hGdfFields,
                            SF_Contact.GetCampaignMembers(SF_Authentication.Token.access_token, ddlSFCampaign.SelectedValue),
                            SF_Lead.GetCampaignMembers(SF_Authentication.Token.access_token, ddlSFCampaign.SelectedValue))
                            .ExcludeExtendedFields(true);

                        foreach (var batch in builder.Build())
                        {
                            try
                            {
                                UpdateToDB(ecnGroup.GroupID, batch.XmlProfileUdf, batch.XmlUdf);
                            }
                            catch (Exception ex)
                            {
                                SF_Utilities.LogException(ex);
                                ShowMessage("Import Unsuccessful", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SF_Utilities.LogException(ex);
                        ShowMessage("Error creating new group", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                        return;
                    }

                }
                else
                {
                    ShowMessage("Please enter a name for the new group", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                }

            }
            else if (rblECNGroup.SelectedValue.Equals("existing", StringComparison.InvariantCultureIgnoreCase))
            {
                if (ddlExistingGroup.SelectedIndex > 0)
                {
                    ecnGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupId, Master.UserSession.CurrentUser);

                    if (ecnGroup != null)
                    {
                        hGdfFields = CreateUDF(ecnGroup.GroupID);

                        var builder = new EmailBatchBuilder(
                                hGdfFields,
                                SF_Contact.GetCampaignMembers(SF_Authentication.Token.access_token, ddlSFCampaign.SelectedValue),
                                SF_Lead.GetCampaignMembers(SF_Authentication.Token.access_token, ddlSFCampaign.SelectedValue))
                            .ExcludeExtendedFields(true);

                        foreach (var batch in builder.Build())
                        {
                            try
                            {
                                UpdateToDB(ecnGroup.GroupID, batch.XmlProfileUdf, batch.XmlUdf);
                            }
                            catch (Exception ex)
                            {
                                SF_Utilities.LogException(ex);
                                ShowMessage("Import Unsuccessful", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                                return;
                            }
                        }
                    }

                }

                else
                {
                    ShowMessage("Please select an existing group to insert into", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                }
            }

            DisplayResults();
        }

        private void UpdateToDB(int groupID, string xmlProfile, string xmlUDF)
        {
            DataTable dtResults = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(Master.UserSession.CurrentUser, Master.UserSession.CurrentUser.CustomerID, groupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "HTML", "S", false, "", "Ecn.communicator.main.SalesForce.SF_Pages.SF_CampaignGroup.UpdateToDB");

            if (dtResults.Rows.Count > 0)
            {
                foreach (DataRow dr in dtResults.Rows)
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
        }

        protected void ddlExistingGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSyncButton();
        }

        protected void ddlECNFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            if (folderID > -1)
            {

                List<ECN_Framework_Entities.Communicator.Group> listECNGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderID(folderID, Master.UserSession.CurrentUser).Where(x => x.CustomerID == Master.UserSession.CurrentUser.CustomerID).OrderBy(x => x.GroupName).ToList();

                ddlExistingGroup.DataSource = listECNGroup;
                ddlExistingGroup.DataTextField = "GroupName";
                ddlExistingGroup.DataValueField = "GroupID";
                ddlExistingGroup.DataBind();
                ddlExistingGroup.Items.Insert(0, new ListItem() { Text = "--SELECT--", Value = "-1", Selected = true });
            }
        }
        #endregion

        private void DisplayResults()
        {
            var converter = new CampaignGroupActionConverter();
            var dtRecords = converter.ConvertToView(hUpdatedRecords);
            if (dtRecords != null)
                    {
                MessageLabel.Text = UserMessages.ImportResults;
                ResultsGrid.DataSource = dtRecords;
                ResultsGrid.DataBind();

                mpeResults.Show();
            }
        }

        private Hashtable getUDFsForList(int groupID, KMPlatform.Entity.User user)
        {
            Hashtable fields = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user);
            if (gdfList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    fields.Add("user_" + gdf.ShortName.ToLower(), gdf.GroupDataFieldsID);
                }
            }

            return fields;
        }

        private Hashtable CreateUDF(int groupID)
        {
            try
            {

                ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = groupID;
                gdf.IsPublic = "N";
                gdf.LongName = "SalesforceID";
                gdf.ShortName = "SFID";
                gdf.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                gdf.CreatedUserID = Master.UserSession.CurrentUser.UserID;

                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, Master.UserSession.CurrentUser);
            }
            catch (Exception ex)
            {
                //SF_Utilities.LogException(ex);
            }
            try
            {
                ECN_Framework_Entities.Communicator.GroupDataFields gdf2 = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf2.GroupID = groupID;
                gdf2.IsPublic = "N";
                gdf2.LongName = "Salesforce Type";
                gdf2.ShortName = "SFType";
                gdf2.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                gdf2.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf2, Master.UserSession.CurrentUser);
            }
            catch (Exception ex)
            {
                //SF_Utilities.LogException(ex);
            }

            Hashtable hGDFFields = getUDFsForList(groupID, Master.UserSession.CurrentUser);

            return hGDFFields;
        }

        private void ShowMessage(string msg, string title, Salesforce.Controls.Message.Message_Icon icon)
        {
            kmMsg.Show(msg, title, icon);
        }

        private CommunicatorGroup CreateECNGroup(string groupName)
        {
            CommunicatorGroup retGroup = new CommunicatorGroup();

            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            int newGroupID = -1;
            try
            {

                retGroup.GroupName = SF_Utilities.CleanStringSqlInjection(txtNewGroup.Text.Trim());
                retGroup.FolderID = folderID;
                retGroup.AllowUDFHistory = "Y";
                retGroup.OwnerTypeCode = "customer";
                retGroup.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                retGroup.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                retGroup.PublicFolder = 1;
                retGroup.IsSeedList = false;
                if (ECN_Framework_BusinessLayer.Communicator.Group.Exists(-1, retGroup.GroupName, retGroup.FolderID.Value, retGroup.CustomerID))
                {
                    ShowMessage("The group name entered is already in use.  Please enter a different group name", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                    return null;
                }

                retGroup.GroupID = ECN_Framework_BusinessLayer.Communicator.Group.Save(retGroup, Master.UserSession.CurrentUser);

                if (retGroup.GroupID < 0)
                {
                    //result = proxy.GetListByName("525ECEBE-28FC-4E85-9D7B-995FB3138509", txtNewGroup.Text.Trim());
                    //dox.LoadXml(result);
                    //nodeList = dox.GetElementsByTagName("ID");

                    //int.TryParse(nodeList.Item(0).InnerText, out newGroupID);
                    ShowMessage("Could not create new group", "ERROR", Salesforce.Controls.Message.Message_Icon.error);
                    return null;
                }
                retGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(retGroup.GroupID, Master.UserSession.CurrentUser);

            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                return retGroup = null;
            }
            return retGroup;
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

                List<SF_Campaign> listCamp = SF_Campaign.GetList(SF_Authentication.Token.access_token, "WHERE CreatedDate >= " + from.ToString("yyyy-MM-ddThh:mm:ssZ") + " and CreatedDate <= " + to.ToString("yyyy-MM-ddThh:mm:ssZ")).OrderBy(x => x.Name).ToList();


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
    }
}