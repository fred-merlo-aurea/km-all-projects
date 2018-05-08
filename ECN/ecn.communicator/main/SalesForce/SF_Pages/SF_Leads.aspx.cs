using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce.Constants;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Extensions;
using ecn.communicator.main.Salesforce.SF_Pages.Converters;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_Leads : SalesForceBasePage
    {
        protected override string EcnCheckBoxId => "chkECNSelect";
        protected override string SfCheckBoxId => "chkSFSelect";
        protected override string EcnHeaderCheckBoxId => "chkECNSelectAll";
        protected override string SfHeaderCheckBoxId => "chkSFSelectAll";
        protected override GridView EcnGrid => dgECNLeads;
        protected override GridView SfGrid => dgSFLeads;
        protected override Message MessageControl => kmMsg;
        protected override string SfSortDirKey => "SFLeadSortDir";
        protected override string EcnSortDirKey => "ECNLeadSortDir";
        protected override string SfSortExpKey => "SFLeadSortExp";
        protected override string EcnSortExpKey => "ECNLeadSortExp";
        protected override EcnSfConverterBase ViewModelConverter => new EcnSfLeadConverter();

        #region properties
        public int KMCustomerID
        {
            get
            {
                return Master.UserSession.CurrentUser.CustomerID;
            }
        }
        private List<SF_Lead> SalesForce_Leads
        {
            get
            {
                if (Session["SF_Leads"] != null)
                    return (List<SF_Lead>)Session["SF_Leads"];
                else
                    return null;
            }
            set
            {
                Session["SF_Leads"] = value;
            }
        }
        private List<ECN_Framework_Entities.Communicator.Email> ECN_Leads
        {
            get
            {
                if (Session["ECN_Leads"] != null)
                    return (List<ECN_Framework_Entities.Communicator.Email>)Session["ECN_Leads"];
                else
                    return null;
            }
            set
            {
                Session["ECN_Leads"] = value;
            }
        }
        private List<SF_User> SF_UserList
        {
            get
            {
                if (Session["SF_Users"] != null)
                    return (List<SF_User>)Session["SF_Users"];
                else
                    return null;
            }
            set
            {
                Session["SF_Users"] = value;
            }
        }
        private static List<string> listECNCB;
        private static List<string> listSFCB;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Setup(Master);
            if (!Page.IsPostBack)
            {
                if (SF_Authentication.LoggedIn == true)
                {
                    SalesForce_Leads = new List<SF_Lead>();
                    ECN_Leads = new List<ECN_Framework_Entities.Communicator.Email>();
                    pnlLeads.Visible = true;
                    listECNCB = new List<string>();
                    listSFCB = new List<string>();
                    //LoadSFContacts();
                    LoadECNDropDowns(Master.UserSession.CurrentUser.CustomerID);
                    SF_UserList = SF_User.GetAll(SF_Authentication.Token.access_token);
                    //BindSearch();
                    GetSFTags();
                    //DataBindLists();
                    kmSearch.BindSearch(SF_Utilities.SFObject.Lead);
                }
                else
                {
                    pnlLeads.Visible = false;
                    ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                }
            }
        }

        #region DataBinding
        private void DataBindLists()
        {
            ECNSortExp = "";
            ECNSortDir = SortDirection.Ascending;
            SFSortDir = SortDirection.Ascending;
            SFSortExp = "";
            ECN_SelectedCount = 0;
            SF_SelectedCount = 0;

            ddlFilter.SelectedIndex = 0;

            divSFLeads.Visible = false;
            lblSFLeads.Visible = false;
            divECNLeads.Visible = false;
            lblECNLeads.Visible = false;
            ddlFilter.SelectedIndex = 0;
            if (ECN_Leads != null)
            {
                dgECNLeads.DataSource = ECN_Leads;
                dgECNLeads.DataBind();

                if (ECN_Leads.Count > 0)
                {
                    divECNLeads.Visible = true;
                    lblECNLeads.Visible = true;
                }
                else
                {
                    divECNLeads.Visible = false;
                    lblECNLeads.Visible = false;
                }
            }

            if (SalesForce_Leads != null)
            {
                dgSFLeads.DataSource = SalesForce_Leads;
                dgSFLeads.DataBind();

                if (SalesForce_Leads.Count > 0)
                {
                    divSFLeads.Visible = true;
                    lblSFLeads.Visible = true;
                }
                else
                {
                    divSFLeads.Visible = false;
                    lblSFLeads.Visible = false;
                }
            }

            ResetButtons();

        }

        private void DataBindECNLists()
        {
            dgECNLeads.DataSource = ECN_Leads;
            dgECNLeads.DataBind();

            if (ECN_Leads.Count > 0)
            {
                divECNLeads.Visible = true;
                lblECNLeads.Visible = true;
            }
            else
            {
                divECNLeads.Visible = false;
                lblECNLeads.Visible = false;
            }
            ResetButtons();
        }

        private void DataBindSFLists()
        {
            dgSFLeads.DataSource = SalesForce_Leads;
            dgSFLeads.DataBind();

            if (SalesForce_Leads.Count > 0)
            {
                divSFLeads.Visible = true;
                lblSFLeads.Visible = true;
            }
            else
            {
                divSFLeads.Visible = false;
                lblSFLeads.Visible = false;
            }
        }
        #endregion

        private bool CompareRecords(SF_Lead sf, ECN_Framework_Entities.Communicator.Email ecn)
        {
            if (!sf.FirstName.Equals(ecn.FirstName))
                return false;
            else if (!sf.LastName.Equals(ecn.LastName))
                return false;
            else if (!sf.City.Equals(ecn.City))
                return false;
            else if (!sf.PostalCode.Equals(ecn.Zip))
                return false;
            else if (!sf.State.Equals(ecn.State))
                return false;
            else if (!sf.Street.Trim().Equals((ecn.Address.Trim() + " " + ecn.Address2.Trim()).Trim()))
                return false;
            else if (!sf.MobilePhone.Equals(ecn.Mobile))
                return false;
            else if (!sf.Phone.Equals(ecn.Voice))
                return false;
            else
                return true;
        }

        #region Load Lists
        private void GetSFTags()
        {
            List<SF_TagDefinition> listTags = SF_TagDefinition.GetAll(SF_Authentication.Token.access_token);
            ddlSFTags.DataSource = listTags;
            ddlSFTags.DataValueField = "Name";
            ddlSFTags.DataTextField = "Name";
            ddlSFTags.DataBind();

            ddlSFTags.Items.Insert(0, new ListItem() { Text = "--SELECT--", Selected = true, Value = "-1" });
        }

        private void LoadECNDropDowns(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> listECNFolder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(customerID, Master.UserSession.CurrentUser).Where(x => x.FolderType == "GRP").OrderBy(x => x.FolderName).ToList();
            ddlECNFolder.DataSource = listECNFolder;
            ddlECNFolder.DataValueField = "FolderID";
            ddlECNFolder.DataTextField = "FolderName";
            ddlECNFolder.DataBind();

            ddlECNFolder.Items.Insert(0, new ListItem() { Text = "root", Selected = true, Value = "0" });

            List<ECN_Framework_Entities.Communicator.Group> listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(customerID, Master.UserSession.CurrentUser).Where(x => x.FolderID == 0).OrderBy(x => x.GroupName).ToList();

            ddlECNGroup.DataSource = listECNGroups;
            ddlECNGroup.DataValueField = "GroupID";
            ddlECNGroup.DataTextField = "GroupName";
            ddlECNGroup.DataBind();

            ddlECNGroup.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "0" });


        }
        #endregion

        #region Button events
        protected void imgbtnSFToECN_Click(object sender, ImageClickEventArgs e)
        {
            var ecnGroup = GetGroup(Master.UserSession.CurrentUser, txtNewGroup.Text, rblECNGroup.SelectedValue, ddlECNGroup.SelectedValue, ddlECNFolder.SelectedValue);
            if (ecnGroup == null)
            {
                return;
            }

            var hGDFFields = CreateUDF(ecnGroup.GroupID, Master.UserSession.CurrentUser);
            var udf = BuildUdf(SalesForce_Leads.Select(x => x.ToEntity()).ToArray(), hGDFFields);
            var xmlprofileUDF = udf.Item1;
            var xmlUDF = udf.Item2;

            var dtResults = ImportEmails(Master.UserSession.CurrentUser, ecnGroup.GroupID, xmlprofileUDF, xmlUDF);
            if (dtResults != null)
            {
                lblECNLeads.Text = ecnGroup.GroupName;
                DisplayResults(dtResults);
            }

            ECN_Leads = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(ecnGroup.GroupID, Master.UserSession.CurrentUser).ToList();
            DataBindLists();

            UncheckECN();
            UncheckSF();
            ResetButtons();
        }

        private void DisplayResults(DataTable dtResults)
        {
            var converter = new ActionConverter();
            var dtRecords = converter.ConvertToView(dtResults);
            if (dtRecords != null)
                    {
                MessageLabel.Text = UserMessages.ImportResults;
                ResultsGrid.DataSource = dtRecords;
                ResultsGrid.DataBind();
                mpeResults.Show();
            }
        }

        protected void imgbtnECNToSF_Click(object sender, ImageClickEventArgs e)
        {
            int SFInserts = 0;
            int SFUpdates = 0;
            int SFFails = 0;
            int SFSkips = 0;
            foreach (GridViewRow gvr in dgECNLeads.Rows)
            {
                CheckBox cb = (CheckBox)gvr.Cells[0].FindControl("chkECNSelect");
                if (cb != null && cb.Checked)
                {
                    if (cb.Attributes["Color"].ToString().Equals("GreyLight"))
                    {
                        SF_Lead tempCheck = SF_Lead.GetEmail(SF_Authentication.Token.access_token, gvr.Cells[1].Text);
                        //Check to see if email already exists, if it does then just add a tag to the record so there isn't a duplicate
                        if (string.IsNullOrEmpty(tempCheck.Email) || tempCheck.Email == "null")
                        {
                            SF_Lead sf = new SF_Lead();
                            string ECNEmail = gvr.Cells[1].Text;
                            ECN_Framework_Entities.Communicator.Email cc = ECN_Leads.First(x => x.EmailAddress == ECNEmail);
                            sf.Email = !string.IsNullOrEmpty(cc.EmailAddress.Trim()) ? cc.EmailAddress.Trim() : string.Empty;
                            sf.FirstName = !string.IsNullOrEmpty(cc.FirstName.Trim()) ? cc.FirstName.Trim() : string.Empty;
                            sf.LastName = !string.IsNullOrEmpty(cc.LastName.Trim()) ? cc.LastName.Trim() : string.Empty;
                            sf.City = !string.IsNullOrEmpty(cc.City.Trim()) ? cc.City.Trim() : string.Empty;
                            sf.PostalCode = !string.IsNullOrEmpty(cc.Zip.Trim()) ? cc.Zip.Trim() : string.Empty;
                            sf.State = !string.IsNullOrEmpty(cc.State.Trim()) ? cc.State.Trim() : string.Empty;
                            sf.Street = !string.IsNullOrEmpty((cc.Address + " " + cc.Address2).Trim()) ? (cc.Address + " " + cc.Address2).Trim() : string.Empty;
                            sf.MobilePhone = !string.IsNullOrEmpty(cc.Mobile.Trim()) ? cc.Mobile.Trim() : string.Empty;
                            sf.Phone = !string.IsNullOrEmpty(cc.Voice.Trim()) ? cc.Voice.Trim() : string.Empty;
                            sf.Company = !string.IsNullOrEmpty(cc.Company.Trim()) ? cc.Company.Trim() : string.Empty;

                            bool success = SF_Lead.Insert(SF_Authentication.Token.access_token, sf);
                            if (success)
                            {
                                sf = SF_Lead.GetSingle(SF_Authentication.Token.access_token, "WHERE Email = '" + sf.Email + "'");
                                SalesForce_Leads.Add(sf);
                                SFInserts++;
                            }
                            else
                            {
                                SFFails++;

                            }
                            if (ddlSFTags.SelectedIndex < 1 && success == true)
                            {
                                SF_LeadTag sfNewLeadTag = new SF_LeadTag();
                                sfNewLeadTag.ItemId = sf.Id;
                                sfNewLeadTag.Name = ddlECNGroup.SelectedItem.Text;
                                sfNewLeadTag.Type = "Public";
                                SF_LeadTag.Insert(SF_Authentication.Token.access_token, sfNewLeadTag);
                            }
                            else if (ddlSFTags.SelectedIndex > 0 && success == true)
                            {
                                SF_TagDefinition sfTagDef = SF_TagDefinition.GetAll(SF_Authentication.Token.access_token).First(x => x.Name == ddlSFTags.SelectedItem.Text);
                                SF_LeadTag sfOldLeadTag = SF_LeadTag.GetByTagName(SF_Authentication.Token.access_token, ddlSFTags.SelectedItem.Text).First();
                                sfOldLeadTag.ItemId = sf.Id;
                                sfOldLeadTag.Name = sfTagDef.Name;
                                sfOldLeadTag.Type = "Public";
                                SF_LeadTag.Insert(SF_Authentication.Token.access_token, sfOldLeadTag);

                            }
                        }
                        else
                        {
                            if (ddlSFTags.SelectedIndex < 1)
                            {
                                SF_LeadTag sfNewLeadTag = new SF_LeadTag();
                                sfNewLeadTag.ItemId = tempCheck.Id;
                                sfNewLeadTag.Name = ddlECNGroup.SelectedItem.Text;
                                sfNewLeadTag.Type = "Public";
                                var success = SF_LeadTag.Insert(SF_Authentication.Token.access_token, sfNewLeadTag);
                                if (success)
                                {
                                    SalesForce_Leads.Add(tempCheck);
                                }
                            }
                            else if (ddlSFTags.SelectedIndex > 0)
                            {
                                SF_TagDefinition sfTagDef = SF_TagDefinition.GetAll(SF_Authentication.Token.access_token).First(x => x.Name == ddlSFTags.SelectedItem.Text);
                                SF_LeadTag sfOldLeadTag = SF_LeadTag.GetByTagName(SF_Authentication.Token.access_token, ddlSFTags.SelectedItem.Text).First();
                                sfOldLeadTag.ItemId = tempCheck.Id;
                                sfOldLeadTag.Name = sfTagDef.Name;
                                sfOldLeadTag.Type = "Public";
                                var success = SF_LeadTag.Insert(SF_Authentication.Token.access_token, sfOldLeadTag);
                                if (success)
                                {
                                    SalesForce_Leads.Add(tempCheck);
                                }
                            }
                            SFSkips++;
                        }
                    }
                    else if (cb.Attributes["Color"].ToString().Equals("GreyDark"))
                    {
                        ECN_Framework_Entities.Communicator.Email cc = new ECN_Framework_Entities.Communicator.Email();
                        SF_Lead sf = new SF_Lead();

                        string ECNEmail = gvr.Cells[1].Text;

                        cc = ECN_Leads.First(x => x.EmailAddress == ECNEmail);
                        sf = SF_Lead.GetSingle(SF_Authentication.Token.access_token, "WHERE Email = '" + cc.EmailAddress.ToString() + "'");// SF_Contacts_List.First(x => x.Email == cc.Email);

                        sf.FirstName = cc.FirstName.Trim();
                        sf.LastName = cc.LastName.Trim();
                        sf.City = cc.City.Trim();
                        sf.PostalCode = cc.Zip.Trim();
                        sf.State = cc.State.Trim();
                        sf.Street = (cc.Address + " " + cc.Address2).Trim();
                        sf.MobilePhone = cc.Mobile.Trim();
                        sf.Phone = cc.Voice.Trim();

                        //sf.Name = (cc.FirstName + " " + cc.LastName).Trim();

                        bool success = SF_Lead.Update(SF_Authentication.Token.access_token, sf);
                        if (success)
                        {
                            SalesForce_Leads.Remove(SalesForce_Leads.First(x => x.Id == sf.Id));
                            SalesForce_Leads.Add(sf);
                            SFUpdates++;
                        }
                        else
                        {
                            SFFails++;
                        }
                    }
                }
            }

            DataTable dtResults = new DataTable();
            dtResults.Columns.Add("Action");
            dtResults.Columns.Add("Counts");
            dtResults.Columns.Add("sortOrder");

            DataRow dr;
            dr = dtResults.NewRow();
            dr["Action"] = "I";
            dr["Counts"] = SFInserts.ToString();
            dtResults.Rows.Add(dr);
            dr = dtResults.NewRow();
            dr["Action"] = "U";
            dr["Counts"] = SFUpdates.ToString();
            dtResults.Rows.Add(dr);
            dr = dtResults.NewRow();
            dr["Action"] = "S";
            dr["Counts"] = SFFails.ToString();
            dtResults.Rows.Add(dr);
            dr = dtResults.NewRow();
            dr["Action"] = "D";
            dr["Counts"] = SFSkips.ToString();
            dtResults.Rows.Add(dr);

            DisplayResults(dtResults);
            ResetButtons();
            DataBindLists();

        }

        protected void imgbtnCompare_Click(object sender, ImageClickEventArgs e)
        {
            var model = BuildViewModel(ECN_Leads, imgbtnCompare.CommandName, imgbtnCompare.CommandArgument);

            FillHidden(model, hfECNCustomerID, hfSFAccountID);
            Fill(lblECNAddress, lblSFAddress, rblAddress, model.Address);
            Fill(lblECNCity, lblSFCity, rblCity, model.City);
            Fill(lblECNState, lblSFState, rblState, model.State);
            Fill(lblECNZip, lblSFZip, rblZip, model.Zip);
            Fill(lblECNCellPhone, lblSFCellPhone, rblCellPhone, model.CellPhone);
            Fill(lblECNPhone, lblSFPhone, rblPhone, model.Phone);
            Fill(lblECNFirstName, lblSFFirstName, rblFirstName, model.FirstName);
            Fill(lblECNLastName, lblSFLastName, rblLastName, model.LastName);
            Fill(lblECNEmail, lblSFEmail, rblEmail, model.Email);

            mpeCompare.Show();
        }

        protected override LocationEntity GetLocationEntity(string token, string whereExpression)
        {
            return SF_Lead.GetSingle(token, whereExpression).ToEntity();
        }

        protected void btnSyncData_Click(object sender, EventArgs e)
        {
            int ECNid = Convert.ToInt32(hfECNCustomerID.Value);
            ECN_Framework_Entities.Communicator.Email c = ECN_Leads.First(x => x.EmailID == ECNid);
            string SFid = hfSFAccountID.Value;
            SF_Lead sf = SalesForce_Leads.First(x => x.Id.Equals(SFid.ToString()));


            if (rblFirstName.SelectedValue.Equals("SF"))
            {
                c.FirstName = sf.FirstName;
            }
            else if (rblFirstName.SelectedValue.Equals("ECN"))
            {
                sf.FirstName = c.FirstName;
            }

            if (rblLastName.SelectedValue.Equals("SF"))
            {
                c.LastName = sf.LastName;
            }
            else if (rblLastName.SelectedValue.Equals("ECN"))
            {
                sf.LastName = c.LastName;
            }

            if (rblEmail.SelectedValue.Equals("SF"))
            {
                c.EmailAddress = sf.Email;
            }
            else if (rblEmail.SelectedValue.Equals("ECN"))
            {
                sf.Email = c.EmailAddress;
            }

            if (rblAddress.SelectedValue.Equals("SF"))
            {
                c.Address = sf.Street;
                c.Address2 = string.Empty;
            }
            else if (rblAddress.SelectedValue.Equals("ECN"))
            {
                sf.Street = (c.Address + " " + c.Address2).Trim();
            }

            if (rblCity.SelectedValue.Equals("SF"))
            {
                c.City = sf.City;
            }
            else if (rblCity.SelectedValue.Equals("ECN"))
            {
                sf.City = c.City;
            }

            if (rblState.SelectedValue.Equals("SF"))
            {
                c.State = SF_Utilities.GetStateAbbr(sf.State);
            }
            else if (rblState.SelectedValue.Equals("ECN"))
            {
                sf.State = c.State;
            }

            if (rblZip.SelectedValue.Equals("SF"))
            {
                c.Zip = sf.PostalCode;
            }
            else if (rblZip.SelectedValue.Equals("ECN"))
            {
                sf.PostalCode = c.Zip;
            }

            if (rblCountry.SelectedValue.Equals("SF"))
            {
                c.Country = sf.Country;
            }
            else if (rblCountry.SelectedValue.Equals("ECN"))
            {
                sf.Country = c.Country;
            }

            if (rblPhone.SelectedValue.Equals("SF"))
            {
                c.Voice = sf.Phone;
            }
            else if (rblPhone.SelectedValue.Equals("ECN"))
            {
                sf.Phone = c.Voice;
            }

            if (rblCellPhone.SelectedValue.Equals("SF"))
            {
                c.Mobile = sf.MobilePhone;
            }
            else if (rblCellPhone.SelectedValue.Equals("ECN"))
            {
                sf.MobilePhone = c.Mobile;
            }

            ECN_Framework_BusinessLayer.Communicator.Email.Save(c);
            bool SFSuccess = false;
            SFSuccess = SF_Lead.Update(SF_Authentication.Token.access_token, sf);
            mpeCompare.Hide();
            if (SFSuccess == true)
            {
                ECN_Leads.Remove(ECN_Leads.First(x => x.EmailID == c.EmailID));
                ECN_Leads.Add(c);

                SalesForce_Leads.Remove(SalesForce_Leads.First(x => x.Id == sf.Id));
                SalesForce_Leads.Add(sf);

                DataBindLists();
                ResultsGrid.DataSource = null;
                ResultsGrid.DataBind();

                MessageLabel.Text = "Sync successful";
                mpeResults.Show();
            }
            else if (SFSuccess == false)
            {
                ResultsGrid.DataSource = null;
                ResultsGrid.DataBind();

                MessageLabel.Text = "Unable to sync to Salesforce";
                mpeResults.Show();
            }

            rblAddress.ClearSelection();
            rblCellPhone.ClearSelection();
            rblCity.ClearSelection();
            rblCountry.ClearSelection();
            rblEmail.ClearSelection();
            rblFirstName.ClearSelection();
            rblLastName.ClearSelection();
            rblPhone.ClearSelection();
            rblState.ClearSelection();
            rblZip.ClearSelection();

        }

        #endregion

        #region GridView Events
        protected void chkSFSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int totalChecked = 0;
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in dgSFLeads.Rows)
            {
                if (gvr.BackColor == Salesforce.Controls.KM_Colors.GreyLight)
                {
                    CheckBox cb = (CheckBox)gvr.FindControl("chkSFSelect");
                    if (cb != null)
                    {
                        cb.Checked = isChecked;
                        if (isChecked)
                            totalChecked++;
                    }
                }
            }

            UncheckECN();

            if (isChecked == true)
            {
                //cant do a compare with multi selection
                imgbtnCompare.Visible = false;

                //only thing we can do is SF to ECN
                if (rblECNGroup.SelectedValue.ToLower().Equals("new") || (rblECNGroup.SelectedValue.ToLower().Equals("existing") && ddlECNGroup.SelectedIndex > 0))
                {
                    imgbtnSFToECN.Visible = true;
                }
                else
                    imgbtnSFToECN.Visible = false;

                imgbtnECNToSF.Visible = false;
            }
            else
            {
                imgbtnCompare.Visible = false;
                imgbtnSFToECN.Visible = false;

                imgbtnECNToSF.Visible = false;
            }
            if (totalChecked == 0)
            {
                ResetButtons();
            }
            SF_SelectedCount = totalChecked;
        }

        protected void chkECNSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int totalChecked = 0;
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in dgECNLeads.Rows)
            {
                if (gvr.BackColor == Salesforce.Controls.KM_Colors.GreyLight)
                {
                    CheckBox cb = (CheckBox)gvr.FindControl("chkECNSelect");
                    if (cb != null)
                    {
                        cb.Checked = isChecked;
                        if (isChecked)
                            totalChecked++;
                    }
                }
            }

            UncheckSF();

            if (isChecked == true)
            {
                //cant do a compare with multi selection
                imgbtnCompare.Visible = false;

                //only thing we can do is ECN to SF
                imgbtnECNToSF.Visible = true;

                imgbtnSFToECN.Visible = false;
            }
            else
            {
                imgbtnCompare.Visible = false;
                imgbtnECNToSF.Visible = false;

                imgbtnSFToECN.Visible = false;
            }
            if (totalChecked == 0)
                ResetButtons();
            ECN_SelectedCount = totalChecked;
        }

        protected void chkECNSelectRow_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.ECN);
        }

        protected void chkSFSelectRow_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.SF);
        }

        protected void dgSFLeads_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HideHeaderCheckBox(e.Row, SfHeaderCheckBoxId, ddlFilter.SelectedValue);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SF_Lead current = (SF_Lead)e.Row.DataItem;
                CheckBox cbSFselect = (CheckBox)e.Row.FindControl("chkSFSelect");
                if (cbSFselect != null)
                    cbSFselect.Attributes.Add("Id", current.Id.ToString());
                if ((!current.OwnerId.Equals("null") || current.OwnerId.Length > 4) && SF_UserList.Exists(x => x.Id == current.OwnerId))
                {
                    Label lblSFOwner = (Label)e.Row.FindControl("lblSFLeadOwner");
                    lblSFOwner.Text = SF_UserList.First(x => x.Id == current.OwnerId).Alias;
                }
                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (ECN_Leads != null && ECN_Leads.Exists(x => x.EmailAddress.ToLower() == current.Email.ToLower()))
                {
                    ECN_Framework_Entities.Communicator.Email ecnContact = ECN_Leads.First(x => x.EmailAddress.ToLower() == current.Email.ToLower());
                    bool isSame = CompareRecords(current, ecnContact);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Lead is in SF and ECN but data is different";
                        if (cbSFselect != null)
                            cbSFselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueLight;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Lead is in SF and ECN and data is the same";
                        if (cbSFselect != null)
                            cbSFselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Lead is ONLY in SF";
                    if (cbSFselect != null)
                        cbSFselect.Attributes.Add("Color", "GreyLight");//only in this table
                }
            }
        }

        protected void dgECNLeads_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ddlFilter.SelectedValue.Equals("DiffData"))
                {
                    CheckBox chkECNSelectAll = (CheckBox)e.Row.FindControl("chkECNSelectAll");
                    if (chkECNSelectAll != null)
                    {
                        chkECNSelectAll.Visible = false;
                    }
                }
                else
                {
                    CheckBox chkECNSelectAll = (CheckBox)e.Row.FindControl("chkECNSelectAll");
                    if (chkECNSelectAll != null)
                    {
                        chkECNSelectAll.Visible = true;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.Email current = (ECN_Framework_Entities.Communicator.Email)e.Row.DataItem;
                CheckBox cbECNselect = (CheckBox)e.Row.FindControl("chkECNSelect");
                if (cbECNselect != null)
                    cbECNselect.Attributes.Add("EmailID", current.EmailID.ToString());

                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (SalesForce_Leads != null && SalesForce_Leads.Exists(x => x.Email.ToLower() == current.EmailAddress.ToLower()))
                {
                    SF_Lead sfAccount = SalesForce_Leads.First(x => x.Email.ToLower() == current.EmailAddress.ToLower());
                    bool isSame = CompareRecords(sfAccount, current);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Lead is in ECN and SF but data is different";
                        if (cbECNselect != null)
                            cbECNselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueLight;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Lead is in ECN and SF and data is the same";
                        if (cbECNselect != null)
                            cbECNselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Lead is ONLY in ECN";
                    if (cbECNselect != null)
                        cbECNselect.Attributes.Add("Color", "GreyLight");//only in this table
                }
            }
        }

        protected void dgSFLeads_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<SF_Lead> listToSort = new List<SF_Lead>();
            foreach (GridViewRow gvr in dgSFLeads.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow && gvr.Visible)
                {
                    SF_Lead sf = SalesForce_Leads.First(x => x.Id == dgSFLeads.DataKeys[gvr.RowIndex].Value.ToString());
                    listToSort.Add(sf);
                }

            }
            if (SFSortExp != null && SFSortExp.Equals(e.SortExpression))
            {
                if (SFSortDir == SortDirection.Ascending)
                {
                    SFSortDir = SortDirection.Descending;
                }
                else
                    SFSortDir = SortDirection.Ascending;
            }
            else
            {
                SFSortExp = e.SortExpression;
                SFSortDir = SortDirection.Ascending;
            }
            SalesForce_Leads = SF_Lead.Sort(listToSort, SFSortExp, SFSortDir);

            dgSFLeads.DataSource = SalesForce_Leads;
            dgSFLeads.DataBind();

            ResetButtons();
        }

        protected void dgECNLeads_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ECN_Framework_Entities.Communicator.Email> listToSort = new List<ECN_Framework_Entities.Communicator.Email>();
            foreach (GridViewRow gvr in dgECNLeads.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow && gvr.Visible)
                {
                    ECN_Framework_Entities.Communicator.Email em = ECN_Leads.First(x => x.EmailID == Convert.ToInt32(dgECNLeads.DataKeys[gvr.RowIndex].Value.ToString()));
                    listToSort.Add(em);
                }
            }
            if (ECNSortExp != null && ECNSortExp.Equals(e.SortExpression))
            {
                if (ECNSortDir == SortDirection.Ascending)
                {
                    ECNSortDir = SortDirection.Descending;
                }
                else
                    ECNSortDir = SortDirection.Ascending;
            }
            else
            {
                ECNSortExp = e.SortExpression;
                ECNSortDir = SortDirection.Ascending;
            }

            if (ECNSortExp.ToLower().Equals("firstname"))
            {
                if (ECNSortDir == SortDirection.Ascending)
                {
                    listToSort = listToSort.OrderBy(x => x.FirstName).ToList();
                }
                else
                    listToSort = listToSort.OrderByDescending(x => x.FirstName).ToList();
            }
            else if (ECNSortExp.ToLower().Equals("lastname"))
            {
                if (ECNSortDir == SortDirection.Ascending)
                {
                    listToSort = listToSort.OrderBy(x => x.LastName).ToList();
                }
                else
                    listToSort = listToSort.OrderByDescending(x => x.LastName).ToList();
            }
            else if (ECNSortExp.ToLower().Equals("email"))
            {
                if (ECNSortDir == SortDirection.Ascending)
                {
                    listToSort = listToSort.OrderBy(x => x.EmailAddress).ToList();
                }
                else
                    listToSort = listToSort.OrderByDescending(x => x.EmailAddress).ToList();
            }

            dgECNLeads.DataSource = listToSort;
            dgECNLeads.DataBind();

        }

        private void UncheckECN()
        {
            Uncheck(GroupType.ECN);
        }

        private void UncheckSF()
        {
            Uncheck(GroupType.SF);
        }

        #endregion

        #region Update Buttons

        protected override void UpdateButtons_Multi(GroupType grp)
        {
            //cant do a compare with multi selection
            //only thing we can do is SF to ECN or  ECN to SF
            imgbtnCompare.Visible = false;
            imgbtnSFToECN.Visible = grp.IsSf();
            imgbtnECNToSF.Visible = grp.IsEcn();
        }

        protected override void PopulateActionsOnRowColor(GroupType grp)
        {
            AddActionByColor(KM_Colors.GreyDark, UpdateButtonsOnGreyDark);
            AddActionByColor(KM_Colors.GreyLight, UpdateButtonsOnGreyLight);
            AddActionByColor(KM_Colors.BlueDark, (arg, g) => ResetButtons());
        }

        protected override void ResetButtons()
        {
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = false;
            imgbtnSFToECN.Visible = false;
        }

        private void UpdateButtonsOnGreyDark(string arg, GroupType grp)
        {
            imgbtnCompare.Visible = true;
            imgbtnCompare.CommandArgument = arg;
            imgbtnCompare.CommandName = GetCommandName(grp);
            imgbtnECNToSF.Visible = false;
            imgbtnSFToECN.Visible = false;
        }

        private void UpdateButtonsOnGreyLight(string arg, GroupType grp)
        {
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = grp.IsEcn();
            imgbtnSFToECN.Visible = grp.IsSf();
            if (grp.IsEcn())
            {
                imgbtnECNToSF.CommandArgument = arg;
            }
            else
            {
                imgbtnSFToECN.CommandArgument = arg;
            }
        }
        #endregion

        #region ECNGroups, Filter, Tags
        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string OnlySF = "OnlySF";
            const string OnlyECN = "OnlyECN";
            const string DiffData = "DiffData";
            const string All = "All";

            var chSFSelectAll = GetHeaderCheckboxAndShowIt(dgSFLeads, SfHeaderCheckBoxId);
            var chkECNSelectAll = GetHeaderCheckboxAndShowIt(dgECNLeads, EcnHeaderCheckBoxId);

            ShowControls(divSFLeads, lblSFLeads, divECNLeads, lblECNLeads);
            ResetButtons();
            UncheckECN();
            UncheckSF();

            switch (ddlFilter.SelectedValue)
            {
                case OnlySF:
                    HideControls(divECNLeads, lblECNLeads);
                    ShowSfByColor(KM_Colors.GreyLight);
                    break;
                case OnlyECN:
                    HideControls(divSFLeads, lblSFLeads);
                    ShowEcnByColor(KM_Colors.GreyLight);
                    break;
                case DiffData:
                    HideControls(chSFSelectAll, chkECNSelectAll);
                    ShowSfByColor(KM_Colors.GreyDark);
                    ShowEcnByColor(KM_Colors.GreyDark);
                    break;
                case All:
                DataBindLists();
                    break;
            }
        }

        protected void rblECNGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblECNGroup.SelectedValue.ToString().ToLower().Equals("new"))
            {
                ddlECNGroup.SelectedIndex = -1;
                ddlECNGroup.Visible = false;

                txtNewGroup.Visible = true;
                txtNewGroup.Text = string.Empty;
                divECNLeads.Visible = false;
                //LoadSFContacts();
                //DataBindSFLists();
            }
            else if (rblECNGroup.SelectedValue.ToString().ToLower().Equals("existing"))
            {

                ddlECNGroup.Visible = true;
                //divSFLeads.Visible = false;
                txtNewGroup.Visible = false;
            }
            ECN_Leads = new List<ECN_Framework_Entities.Communicator.Email>();
            DataBindLists();
            lblECNLeads.Visible = false;

            ResetButtons();
        }

        protected void ddlECNGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int GroupID = -1;
            int.TryParse(ddlECNGroup.SelectedValue.ToString(), out GroupID);
            if (GroupID > 0)
            {
                ECN_Leads = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(GroupID, Master.UserSession.CurrentUser);
                lblECNLeads.Text = ddlECNGroup.SelectedItem.Text;
                lblECNLeads.Visible = true;
                ddlFilter.SelectedIndex = 0;
                DataBindLists();
            }
            else
            {
                ECN_Leads = new List<ECN_Framework_Entities.Communicator.Email>();

                DataBindLists();
            }

        }

        protected void ddlSFTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSFTags.SelectedIndex > 0)
            {
                List<SF_LeadTag> listTags = SF_LeadTag.GetByTagName(SF_Authentication.Token.access_token, ddlSFTags.SelectedItem.Text).ToList();
                SalesForce_Leads = SF_Lead.GetByTagList(SF_Authentication.Token.access_token, listTags);
                DataBindLists();
            }
            else
            {
                SalesForce_Leads = new List<SF_Lead>();
                DataBindLists();
            }

        }


        protected void ddlECNFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            if (folderID > -1)
            {
                List<ECN_Framework_Entities.Communicator.Group> listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderID(folderID, Master.UserSession.CurrentUser).Where(x => x.CustomerID == Master.UserSession.CurrentUser.CustomerID).OrderBy(x => x.GroupName).ToList();

                ddlECNGroup.DataSource = listECNGroups;
                ddlECNGroup.DataValueField = "GroupID";
                ddlECNGroup.DataTextField = "GroupName";
                ddlECNGroup.DataBind();

                ddlECNGroup.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "0" });
            }
            ECN_Leads = new List<ECN_Framework_Entities.Communicator.Email>();
            DataBindLists();
        }
        #endregion

        #region Search
        private void BindSearch()
        {
            //gvSearchBy.DataSource = typeof(SF_Lead).GetProperties();
            //gvSearchBy.DataBind();
        }

        protected void btnGetQuery_Click(object sender, EventArgs e)
        {
            string query = kmSearch.GetQuery();
            SalesForce_Leads = SF_Lead.GetList(SF_Authentication.Token.access_token, query).Where(x => !string.IsNullOrEmpty(x.Email) && x.IsConverted == false).ToList();

            DataBindLists();
            ddlSFTags.SelectedIndex = 0;
        }

        protected void btnResetSearch_Click(object sender, EventArgs e)
        {
            kmSearch.ResetSearch();
            SalesForce_Leads = new List<SF_Lead>();
            ECN_Leads = new List<ECN_Framework_Entities.Communicator.Email>();
            ddlECNFolder.SelectedIndex = 0;
            ddlECNGroup.SelectedIndex = 0;
            ddlFilter.SelectedIndex = 0;
            ddlSFTags.SelectedIndex = 0;
            DataBindLists();
            lblECNLeads.Visible = false;
            lblSFLeads.Visible = false;
        }

        #endregion

        private void HideControls(params Control[] controls)
        {
            SetVisibility(false, controls);
        }

        private void ShowControls(params Control[] controls)
        {
            SetVisibility(true, controls);
        }

        private void SetVisibility(bool isVisible, params Control[] controls)
        {
            foreach (var item in controls)
            {
                if (item != null)
                {
                    item.Visible = isVisible;
                }
            }
        }

        private void ShowEcnByColor(Color color)
        {
            var visibleRows = GetRowCountByColorAndShowIt(dgECNLeads, color);
            SetVisibility(visibleRows > 0, divECNLeads, lblECNLeads);
        }

        private void ShowSfByColor(Color color)
        {
            var visibleRows = GetRowCountByColorAndShowIt(dgSFLeads, color);
            SetVisibility(visibleRows > 0, divSFLeads, lblSFLeads);
        }

        private int GetRowCountByColorAndShowIt(GridView grid, Color color)
        {
            var count = 0;
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.BackColor == color && row.RowType == DataControlRowType.DataRow)
                {
                    count++;
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }

            return count;
        }

        private CheckBox GetHeaderCheckboxAndShowIt(GridView grid, string id)
        {
            CheckBox checkbox = null;
            if (grid.HeaderRow != null)
            {
                checkbox = grid.HeaderRow.FindControl(id) as CheckBox;
                if (checkbox != null)
                {
                    checkbox.Visible = true;
                }
            }
            return checkbox;
        }
    }
}