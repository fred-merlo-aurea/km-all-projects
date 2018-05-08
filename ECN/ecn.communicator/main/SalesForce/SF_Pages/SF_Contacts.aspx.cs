using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public partial class SF_Contacts : SalesForceBasePage
    {
        protected override string EcnCheckBoxId => "chkECNSelect";
        protected override string SfCheckBoxId => "chkSFSelect";
        protected override string EcnHeaderCheckBoxId => "chkECNSelectAll";
        protected override string SfHeaderCheckBoxId => "chkSFSelectAll";
        protected override GridView EcnGrid => gvECNContacts;
        protected override GridView SfGrid => gvSFContacts;
        protected override Message MessageControl => kmMsg;
        protected override string SfSortDirKey => "SFSortDir";
        protected override string EcnSortDirKey => "ECNSortDir";
        protected override string SfSortExpKey => "SFSortExp";
        protected override string EcnSortExpKey => "ECNSortExp";
        protected override EcnSfConverterBase ViewModelConverter => new EcnSfContactConverter();

        #region properties
        public int KMCustomerID
        {
            get
            {
                return Master.UserSession.CurrentUser.CustomerID;
            }
        }
        private List<SF_Contact> SF_Contacts_List
        {
            get
            {
                if (Session["SF_Contacts"] != null)
                    return (List<SF_Contact>)Session["SF_Contacts"];
                else
                    return null;
            }
            set
            {
                Session["SF_Contacts"] = value;
            }
        }
        private List<ECN_Framework_Entities.Communicator.Email> ECN_Contacts
        {
            get
            {
                if (Session["ECN_Contacts"] != null)
                    return (List<ECN_Framework_Entities.Communicator.Email>)Session["ECN_Contacts"];
                else
                    return null;
            }
            set
            {
                Session["ECN_Contacts"] = value;
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

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Setup(Master);
            if (!Page.IsPostBack)
            {
                try
                {
                    if (SF_Authentication.LoggedIn == true)
                    {
                        pnlContact.Visible = true;
                        SF_UserList = SF_User.GetAll(SF_Authentication.Token.access_token);
                        SFSortExp = string.Empty;
                        SFSortDir = SortDirection.Ascending;
                        ECNSortDir = SortDirection.Ascending;
                        ECNSortExp = string.Empty;

                        ECN_Contacts = new List<ECN_Framework_Entities.Communicator.Email>();
                        SF_Contacts_List = new List<SF_Contact>();
                        //LoadSFContacts();
                        //LoadECNContacts(142);
                        BindSearch();
                        //DataBindLists();
                        List<ECN_Framework_Entities.Communicator.Folder> listECNFolder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderType == "GRP").OrderBy(x => x.FolderName).ToList();
                        ddlECNFolder.DataSource = listECNFolder;
                        ddlECNFolder.DataValueField = "FolderID";
                        ddlECNFolder.DataTextField = "FolderName";
                        ddlECNFolder.DataBind();
                        ddlECNFolder.Items.Insert(0, new ListItem() { Text = "root", Selected = true, Value = "0" });

                        List<ECN_Framework_Entities.Communicator.Group> listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser).Where(x => x.FolderID == 0).OrderBy(x => x.GroupName).ToList();
                        ddlECNGroup.DataSource = listECNGroups;
                        ddlECNGroup.DataTextField = "GroupName";
                        ddlECNGroup.DataValueField = "GroupID";
                        ddlECNGroup.DataBind();

                        ddlECNGroup.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "0" });
                    }
                    else
                    {
                        pnlContact.Visible = false;
                        ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                    }
                }
                catch (Exception ex)
                {
                    SF_Utilities.LogException(ex);
                }
            }
        }

        private bool CompareRecords(SF_Contact SFContact, ECN_Framework_Entities.Communicator.Email ECNContact)
        {
            if (!SFContact.FirstName.Equals(ECNContact.FirstName))
                return false;
            else if (!SFContact.LastName.Equals(ECNContact.LastName))
                return false;
            else if (!SFContact.MailingCity.Equals(ECNContact.City))
                return false;
            else if (!SFContact.MailingPostalCode.Equals(ECNContact.Zip))
                return false;
            else if (!SFContact.MailingState.Equals(ECNContact.State))
                return false;
            else if (!SFContact.MailingStreet.Trim().Equals(((ECNContact.Address.Trim() + " " + ECNContact.Address2.Trim()).Trim()).Trim()))
                return false;
            else if (!SFContact.MobilePhone.Equals(ECNContact.Mobile))
                return false;
            else if (!SFContact.Phone.Equals(ECNContact.Voice))
                return false;
            else
                return true;
        }

        #region Load Lists
        private void LoadSFContacts()
        {
            SF_Contacts_List = SF_Contact.GetAll(SF_Authentication.Token.access_token).Where(x => x.Email.Trim().Length > 0).ToList();
        }

        private void LoadECNContacts(int groupID)
        {
            //if (ECN_Contacts == null)
            //ECN_Contacts = ECN_Framework_DataLayer.Accounts.CustomerContact.GetByCustomerID(customerID).ToList();

            ECN_Contacts = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(groupID, Master.UserSession.CurrentUser);
            lblECNContacts.Visible = true;


        }

        private void DataBindLists()
        {
            ECNSortExp = "";
            ECNSortDir = SortDirection.Ascending;
            SFSortDir = SortDirection.Ascending;
            SFSortExp = "";
            ECN_SelectedCount = 0;
            SF_SelectedCount = 0;

            ddlFilter.SelectedIndex = 0;

            divSFContacts.Visible = false;
            lblSFContacts.Visible = false;
            divECNContacts.Visible = false;
            lblECNContacts.Visible = false;

            if (SF_Contacts_List != null)
            {
                gvSFContacts.DataSource = SF_Contacts_List;
                gvSFContacts.DataBind();
                gvSFContacts.SelectedIndex = -1;

                if (SF_Contacts_List.Count > 0)
                {
                    divSFContacts.Visible = true;
                    lblSFContacts.Visible = true;
                }



            }

            if (ECN_Contacts != null)
            {
                gvECNContacts.DataSource = ECN_Contacts;
                gvECNContacts.DataBind();
                gvECNContacts.SelectedIndex = -1;

                if (ECN_Contacts.Count > 0)
                {
                    divECNContacts.Visible = true;
                    lblECNContacts.Visible = true;
                }
            }

            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = false;
            imgbtnSFToECN.Visible = false;
        }

        #endregion

        #region Insert/Update/Compare image buttons
        protected void imgbtnSFToECN_Click(object sender, ImageClickEventArgs e)
        {
            var ecnGroup = GetGroup(Master.UserSession.CurrentUser, txtECNGroup.Text, rblECNGroup.SelectedValue, ddlECNGroup.SelectedValue, ddlECNFolder.SelectedValue);
            if (ecnGroup == null)
            {
                return;
            }

            var hGDFFields = CreateUDF(ecnGroup.GroupID, Master.UserSession.CurrentUser);
            var udf = BuildUdf(SF_Contacts_List.Select(x => x.ToEntity()).ToArray(), hGDFFields);
            var xmlprofileUDF = udf.Item1;
            var xmlUDF = udf.Item2;

            var dtResults = ImportEmails(Master.UserSession.CurrentUser, ecnGroup.GroupID, xmlprofileUDF, xmlUDF);
            if (dtResults != null)
            {
                lblECNContacts.Text = ecnGroup.GroupName;
                DisplayResults(dtResults);
            }

            var listNewGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser).OrderBy(x => x.GroupName).ToList();
            ddlECNGroup.DataSource = listNewGroup;
            ddlECNGroup.DataValueField = "GroupID";
            ddlECNGroup.DataTextField = "GroupName";
            ddlECNGroup.DataBind();
            ddlECNGroup.Items.Insert(0, new ListItem() { Text = "--SELECT--", Value = "-1" });
            ddlECNGroup.SelectedValue = ecnGroup.GroupID.ToString();

            ECN_Contacts = ECN_Framework_BusinessLayer.Communicator.Email.GetByGroupID(ecnGroup.GroupID, Master.UserSession.CurrentUser).ToList();

            DataBindLists();
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
            foreach (GridViewRow gvr in gvECNContacts.Rows)
            {
                CheckBox cb = (CheckBox)gvr.Cells[0].FindControl("chkECNSelect");
                if (cb != null && cb.Checked)
                {
                    if (cb.Attributes["Color"].ToString().Equals("GreyLight"))
                    {


                        string id = gvr.Cells[1].Text;
                        SF_Contact sf = SF_Contact.GetSingle(SF_Authentication.Token.access_token, "WHERE Email = '" + id.ToString() + "'");
                        if (string.IsNullOrEmpty(sf.Email) || sf.Email == "null")
                        {
                            sf = new SF_Contact();
                            ECN_Framework_Entities.Communicator.Email cc = ECN_Contacts.First(x => x.EmailAddress == id);
                            sf.Email = cc.EmailAddress;
                            sf.FirstName = cc.FirstName;
                            sf.LastName = cc.LastName;
                            sf.MailingCity = cc.City;
                            sf.MailingPostalCode = cc.Zip;
                            sf.MailingState = cc.State;
                            sf.MailingStreet = (cc.Address + " " + cc.Address2).Trim();
                            sf.MobilePhone = cc.Mobile;
                            sf.Phone = cc.Voice;

                            bool success = SF_Contact.Insert(SF_Authentication.Token.access_token, sf);
                            if (success)
                            {
                                SF_Contacts_List.Add(sf);
                                SFInserts++;

                            }
                            else
                            {
                                SFFails++;
                            }
                        }
                        else
                        {
                            //SF_Contacts_List.Add(sf);
                            SFSkips++;
                        }
                    }
                    else if (cb.Attributes["Color"].ToString().Equals("GreyDark"))
                    {
                        ECN_Framework_Entities.Communicator.Email cc = new ECN_Framework_Entities.Communicator.Email();
                        SF_Contact sf = new SF_Contact();

                        string ECNEmail = gvr.Cells[1].Text;

                        cc = ECN_Contacts.First(x => x.EmailAddress == ECNEmail);
                        sf = SF_Contact.GetSingle(SF_Authentication.Token.access_token, "WHERE Email = '" + cc.EmailAddress.ToString() + "'");// SF_Contacts_List.First(x => x.Email == cc.Email);

                        sf.FirstName = cc.FirstName;
                        sf.LastName = cc.LastName;
                        sf.MailingCity = cc.City;
                        sf.MailingPostalCode = cc.Zip;
                        sf.MailingState = cc.State;
                        sf.MailingStreet = (cc.Address + " " + cc.Address2).Trim();
                        sf.MobilePhone = cc.Mobile;
                        sf.Phone = cc.Voice;

                        //sf.Name = (cc.FirstName + " " + cc.LastName).Trim();

                        bool success = SF_Contact.Update(SF_Authentication.Token.access_token, sf);
                        if (success)
                        {
                            SF_Contacts_List.Remove(SF_Contacts_List.First(x => x.Id == sf.Id));
                            SF_Contacts_List.Add(sf);
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
            var model = BuildViewModel(ECN_Contacts, imgbtnCompare.CommandName, imgbtnCompare.CommandArgument);

            FillHidden(model, hfECNContactID, hfSFLeadID);
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
            return SF_Contact.GetSingle(token, whereExpression).ToEntity();
        }
        #endregion

        #region GridView events
        protected void gvECNContacts_Sorting(object sender, GridViewSortEventArgs e)
        {
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
            List<ECN_Framework_Entities.Communicator.Email> sortList = new List<ECN_Framework_Entities.Communicator.Email>();
            foreach (GridViewRow gvr in gvECNContacts.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow && gvr.Visible == true)
                {

                    ECN_Framework_Entities.Communicator.Email em = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(gvr.Cells[1].Text.Trim(), Master.UserSession.CurrentCustomer.CustomerID);// (ECN_Framework_Entities.Communicator.Email)gvr.DataItem;
                    sortList.Add(em);
                }
            }
            switch (e.SortExpression.ToLower())
            {
                case "email":
                    if (ECNSortDir == SortDirection.Ascending)
                        sortList = sortList.OrderBy(x => x.EmailAddress).ToList();
                    else
                        sortList = sortList.OrderByDescending(x => x.EmailAddress).ToList();
                    break;

                case "firstname":
                    if (ECNSortDir == SortDirection.Ascending)
                        sortList = sortList.OrderBy(x => x.FirstName).ToList();
                    else
                        sortList = sortList.OrderByDescending(x => x.FirstName).ToList();
                    break;

                case "lastname":
                    if (ECNSortDir == SortDirection.Ascending)
                        sortList = sortList.OrderBy(x => x.LastName).ToList();
                    else
                        sortList = sortList.OrderByDescending(x => x.LastName).ToList();
                    break;

                case "state":
                    if (ECNSortDir == SortDirection.Ascending)
                        sortList = sortList.OrderBy(x => x.State).ToList();
                    else
                        sortList = sortList.OrderByDescending(x => x.State).ToList();
                    break;
            }
            gvECNContacts.DataSource = sortList;
            gvECNContacts.DataBind();
            ResetButtons();
        }

        protected void gvSFContacts_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["SFSortExp"] != null && SFSortExp.Equals(e.SortExpression))
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
            List<SF_Contact> listSort = new List<SF_Contact>();
            foreach (GridViewRow gvr in gvSFContacts.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow && gvr.Visible == true)
                {
                    SF_Contact sfc = SF_Contacts_List.First(x => x.Email == gvr.Cells[1].Text.Trim());
                    listSort.Add(sfc);
                }
            }
            listSort = SF_Contact.Sort(listSort, SFSortExp, SFSortDir);

            gvSFContacts.DataSource = listSort;
            gvSFContacts.DataBind();
            ResetButtons();
        }

        protected void dgSFContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HideHeaderCheckBox(e.Row, SfHeaderCheckBoxId, ddlFilter.SelectedValue);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SF_Contact current = (SF_Contact)e.Row.DataItem;
                CheckBox cbSFselect = (CheckBox)e.Row.FindControl("chkSFSelect");
                if (cbSFselect != null)
                    cbSFselect.Attributes.Add("Id", current.Id.ToString());
                if ((!current.OwnerId.Equals("null") || current.OwnerId.Length > 4) && SF_UserList.Exists(x => x.Id == current.OwnerId))
                {
                    Label lblSFOwner = (Label)e.Row.FindControl("lblSFOwner");
                    lblSFOwner.Text = SF_UserList.First(x => x.Id == current.OwnerId).Alias;
                }
                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (ECN_Contacts != null && ECN_Contacts.Exists(x => x.EmailAddress.ToLower() == current.Email.ToLower()))
                {
                    ECN_Framework_Entities.Communicator.Email ecnContact = ECN_Contacts.First(x => x.EmailAddress.ToLower() == current.Email.ToLower());
                    bool isSame = CompareRecords(current, ecnContact);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Contact is in SF and ECN but data is different";
                        if (cbSFselect != null)
                            cbSFselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Contact is in SF and ECN and data is the same";
                        if (cbSFselect != null)
                            cbSFselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Contact is ONLY in SF";
                    if (cbSFselect != null)
                        cbSFselect.Attributes.Add("Color", "GreyLight");//only in this table
                }
            }
        }

        protected void dgECNContacts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ddlFilter.SelectedValue.Equals("DiffData"))
                {
                    CheckBox cbECNSelectAll = (CheckBox)e.Row.FindControl("chkECNSelectAll");
                    if (cbECNSelectAll != null)
                        cbECNSelectAll.Visible = false;
                }
                else
                {
                    CheckBox cbECNSelectAll = (CheckBox)e.Row.FindControl("chkECNSelectAll");
                    if (cbECNSelectAll != null)
                        cbECNSelectAll.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.Email current = (ECN_Framework_Entities.Communicator.Email)e.Row.DataItem;
                CheckBox cbECNselect = (CheckBox)e.Row.FindControl("chkECNSelect");
                if (cbECNselect != null)
                    cbECNselect.Attributes.Add("CustomerID", current.CustomerID.ToString());

                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (SF_Contacts_List != null && SF_Contacts_List.Exists(x => x.Email.ToLower() == current.EmailAddress.ToLower()))
                {
                    SF_Contact sfAccount = SF_Contacts_List.First(x => x.Email.ToLower() == current.EmailAddress.ToLower());
                    bool isSame = CompareRecords(sfAccount, current);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Contact is in ECN and SF but data is different";
                        if (cbECNselect != null)
                            cbECNselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Contact is in ECN and SF and data is the same";
                        if (cbECNselect != null)
                            cbECNselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Contact is ONLY in ECN";
                    if (cbECNselect != null)
                        cbECNselect.Attributes.Add("Color", "GreyLight");//only in this table
                }
            }
        }
        #endregion

        #region UI Events
        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ECNCount = 0;
            int SFCount = 0;
            divECNContacts.Visible = true;
            divSFContacts.Visible = true;
            lblECNContacts.Visible = true;
            lblSFContacts.Visible = true;
            UncheckSF();
            UncheckECN();
            ResetButtons();
            CheckBox chSFSelectAll = new CheckBox();
            CheckBox chkECNSelectAll = new CheckBox();
            if (gvSFContacts.HeaderRow != null)
            {
                chSFSelectAll = (CheckBox)gvSFContacts.HeaderRow.FindControl("chkSFSelectAll");
                if (chSFSelectAll != null)
                    chSFSelectAll.Visible = true;
            }
            if (gvECNContacts.HeaderRow != null)
            {
                chkECNSelectAll = (CheckBox)gvECNContacts.HeaderRow.FindControl("chkECNSelectAll");
                if (chkECNSelectAll != null)
                    chkECNSelectAll.Visible = true;

            }



            if (ddlFilter.SelectedValue.Equals("OnlySF"))
            {
                divECNContacts.Visible = false;
                lblECNContacts.Visible = false;

                foreach (GridViewRow gvr in gvSFContacts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyLight)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        SFCount++;
                    }
                }
            }
            else if (ddlFilter.SelectedValue.Equals("OnlyECN"))
            {
                divSFContacts.Visible = false;
                lblSFContacts.Visible = false;

                foreach (GridViewRow gvr in gvECNContacts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyLight)
                        gvr.Visible = false;
                    else
                    {
                        ECNCount++;
                        gvr.Visible = true;

                    }
                }
            }
            else if (ddlFilter.SelectedValue.Equals("DiffData"))
            {
                if (chkECNSelectAll != null)
                    chkECNSelectAll.Visible = false;
                if (chSFSelectAll != null)
                    chSFSelectAll.Visible = false;

                foreach (GridViewRow gvr in gvECNContacts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyDark)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        ECNCount++;
                    }
                }
                foreach (GridViewRow gvr in gvSFContacts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyDark)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        SFCount++;
                    }
                }

            }
            else if (ddlFilter.SelectedValue.Equals("All"))
            {
                foreach (GridViewRow gvr in gvECNContacts.Rows)
                {
                    gvr.Visible = true;
                    ECNCount++;
                }
                foreach (GridViewRow gvr in gvSFContacts.Rows)
                {
                    gvr.Visible = true;
                    SFCount++;
                }
            }

            if (ECNCount > 0)
            {
                divECNContacts.Visible = true;
                lblECNContacts.Visible = true;
            }
            else
            {
                divECNContacts.Visible = false;
                lblECNContacts.Visible = false;
            }
            if (SFCount > 0)
            {
                divSFContacts.Visible = true;
                lblSFContacts.Visible = true;
            }
            else
            {
                divSFContacts.Visible = false;
                lblSFContacts.Visible = false;
            }
        }

        protected void btnSyncData_Click(object sender, EventArgs e)
        {
            int ECNid = Convert.ToInt32(hfECNContactID.Value);
            ECN_Framework_Entities.Communicator.Email c = ECN_Contacts.First(x => x.EmailID == ECNid);
            string SFid = hfSFLeadID.Value;
            SF_Contact sf = SF_Contacts_List.First(x => x.Id.Equals(SFid.ToString()));


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
                c.Address = sf.MailingStreet;
                c.Address2 = string.Empty;
            }
            else if (rblAddress.SelectedValue.Equals("ECN"))
            {
                sf.MailingStreet = (c.Address.Trim() + " " + c.Address2.Trim()).Trim();
            }

            if (rblCity.SelectedValue.Equals("SF"))
            {
                c.City = sf.MailingCity;
            }
            else if (rblCity.SelectedValue.Equals("ECN"))
            {
                sf.MailingCity = c.City;
            }

            if (rblState.SelectedValue.Equals("SF"))
            {
                c.State = SF_Utilities.GetStateAbbr(sf.MailingState);
            }
            else if (rblState.SelectedValue.Equals("ECN"))
            {
                sf.MailingState = c.State;
            }

            if (rblZip.SelectedValue.Equals("SF"))
            {
                c.Zip = sf.MailingPostalCode;
            }
            else if (rblZip.SelectedValue.Equals("ECN"))
            {
                sf.MailingPostalCode = c.Zip;
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

            try
            {
                ECN_Framework_BusinessLayer.Communicator.Email.Save(c);
                ECN_Contacts.Remove(ECN_Contacts.First(x => x.CustomerID == c.CustomerID));
                ECN_Contacts.Add(c);

                SF_Contact.Update(SF_Authentication.Token.access_token, sf);
                SF_Contacts_List.Remove(SF_Contacts_List.First(x => x.Id == sf.Id));
                SF_Contacts_List.Add(sf);

                DataBindLists();
                MessageLabel.Text = "Update successful";
                mpeResults.Show();
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
            }
        }

        protected void ddlECNGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            int groupID = -1;
            int.TryParse(ddlECNGroup.SelectedValue.ToString(), out groupID);

            if (groupID > 0)
            {
                LoadECNContacts(groupID);
                lblECNContacts.Text = ddlECNGroup.SelectedItem.Text;
                lblECNContacts.Visible = true;
                DataBindLists();


            }
            else
            {
                ECN_Contacts = new List<ECN_Framework_Entities.Communicator.Email>();
                DataBindLists();
            }

        }

        protected void rblECNGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblECNGroup.SelectedValue.ToLower().Equals("existing"))
            {
                ddlECNGroup.Visible = true;
                txtECNGroup.Visible = false;
                //divSFContacts.Visible = false;
                //lblSFContacts.Visible = false;

            }
            else if (rblECNGroup.SelectedValue.ToLower().Equals("new"))
            {
                ddlECNGroup.Visible = false;
                txtECNGroup.Visible = true;
                txtECNGroup.Text = string.Empty;
                divECNContacts.Visible = false;
                //divSFContacts.Visible = true;

                //lblSFContacts.Visible = true;
                lblECNContacts.Visible = false;
            }
            if (ECN_Contacts != null)
                ECN_Contacts = new List<ECN_Framework_Entities.Communicator.Email>();
            ddlECNGroup.SelectedIndex = 0;
            ddlECNFolder.SelectedIndex = 0;
            DataBindLists();
            ResetButtons();

            UpdateContactPanel();
        }


        protected void ddlECNFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            int folderID = -1;
            int.TryParse(ddlECNFolder.SelectedValue.ToString(), out folderID);
            if (folderID > -1)
            {

                List<ECN_Framework_Entities.Communicator.Group> listECNGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderID(folderID, Master.UserSession.CurrentUser).Where(x => x.CustomerID == Master.UserSession.CurrentUser.CustomerID).OrderBy(x => x.GroupName).ToList();
                ddlECNGroup.DataSource = listECNGroups;
                ddlECNGroup.DataTextField = "GroupName";
                ddlECNGroup.DataValueField = "GroupID";
                ddlECNGroup.DataBind();

                ddlECNGroup.Items.Insert(0, new ListItem() { Selected = true, Text = "--SELECT--", Value = "0" });

                ECN_Contacts = new List<ECN_Framework_Entities.Communicator.Email>();
                DataBindLists();
            }
        }
        #endregion

        #region CheckBox events
        protected void chkSFSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int totalChecked = 0;
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in gvSFContacts.Rows)
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
                {
                    imgbtnSFToECN.Visible = false;
                }
                imgbtnECNToSF.Visible = false;
            }
            else
            {
                imgbtnCompare.Visible = false;
                imgbtnSFToECN.Visible = false;

                imgbtnECNToSF.Visible = false;
            }

            UpdateContactPanel();
            SF_SelectedCount = totalChecked;
        }

        private void UncheckECN()
        {
            Uncheck(GroupType.ECN);
        }

        private void UncheckSF()
        {
            Uncheck(GroupType.SF);
        }

        protected void chkSFSelect_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.SF);
        }

        private void UpdateContactPanel()
        {
            upContacts.Update();
        }

        protected void chkECNSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int totalChecked = 0;
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in gvECNContacts.Rows)
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

                //only thing we can do is SF to ECN

                imgbtnECNToSF.Visible = true;

                imgbtnSFToECN.Visible = false;
            }
            else
            {
                imgbtnCompare.Visible = false;
                imgbtnECNToSF.Visible = false;

                imgbtnSFToECN.Visible = false;
            }

            ECN_SelectedCount = totalChecked;
        }

        protected void chkECNSelect_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.ECN);
        }
        #endregion

        #region Search
        private void BindSearch()
        {
            kmSearch.BindSearch(SF_Utilities.SFObject.Contact);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string where = kmSearch.GetQuery();

            SF_Contacts_List = SF_Contact.GetList(SF_Authentication.Token.access_token, where).Where(x => !string.IsNullOrEmpty(x.Email)).ToList();

            DataBindLists();

            divSFContacts.Visible = true;
            lblSFContacts.Visible = true;
        }
        protected void btnSearchReset_Click(object sender, EventArgs e)
        {
            kmSearch.ResetSearch();
            SF_Contacts_List = new List<SF_Contact>();
            ECN_Contacts = new List<ECN_Framework_Entities.Communicator.Email>();
            DataBindLists();
            lblECNContacts.Visible = false;
            lblSFContacts.Visible = false;

            ddlECNFolder.SelectedIndex = 0;
            ddlECNGroup.SelectedIndex = 0;

        }
        #endregion

        protected override void PopulateActionsOnRowColor(GroupType grp)
        {
            if (grp.IsSf() && IsSelectionValid())
            {
                AddActionByColor(KM_Colors.GreyDark, UpdateButtonsOnGreyDark);
                AddActionByColor(KM_Colors.GreyLight, UpdateButtonsOnGreyLight);
                AddActionByColor(KM_Colors.BlueDark, (arg, g) => ResetButtons());
            }
            if (grp.IsEcn())
            {
                AddActionByColor(KM_Colors.GreyDark, UpdateButtonsOnGreyDark);
                AddActionByColor(KM_Colors.GreyLight, UpdateButtonsOnGreyLight);
                AddActionByColor(KM_Colors.BlueDark, (arg, g) => ResetButtons());
            }
        }

        protected override void UpdateButtons_Multi(GroupType grp)
        {
            //cant do a compare with multi selection
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = grp.IsEcn();

            if (grp.IsSf())
            {
                //only thing we can do is SF to ECN
                if (IsSelectionValid())
                {
                    imgbtnSFToECN.Visible = true;
                }
            }
            else
            {
                imgbtnSFToECN.Visible = false;
            }
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
            imgbtnSFToECN.CommandArgument = arg;
            imgbtnECNToSF.CommandArgument = arg;
        }

        private bool IsSelectionValid()
        {
            const string newValue = "new";
            const string existingValue = "existing";

            return rblECNGroup.SelectedValue.Equals(newValue, StringComparison.OrdinalIgnoreCase) || (rblECNGroup.SelectedValue.Equals(existingValue, StringComparison.OrdinalIgnoreCase) && ddlECNGroup.SelectedIndex > 0);
        }
    }
}