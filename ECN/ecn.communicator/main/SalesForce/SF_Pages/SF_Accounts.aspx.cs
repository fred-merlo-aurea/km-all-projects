using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ecn.communicator.main.Salesforce.Entity;
using ecn.communicator.main.Salesforce.Extensions;
using ecn.communicator.main.Salesforce.Controls;
using ecn.communicator.main.Salesforce.SF_Pages.Converters;

namespace ecn.communicator.main.Salesforce.SF_Pages
{
    public partial class SF_Accounts : SalesForceBasePage
    {
        protected override string EcnCheckBoxId => "cbECNselect";
        protected override string SfCheckBoxId => "cbSFselect";
        protected override string EcnHeaderCheckBoxId => "cbECNselectALL";
        protected override string SfHeaderCheckBoxId => "cbSFselectALL";
        protected override GridView EcnGrid => gvECNAccounts;
        protected override GridView SfGrid => gvSFAccounts;
        protected override Message MessageControl => kmMsg;
        protected override string SfSortDirKey => "SFAccountSortDir";
        protected override string EcnSortDirKey => "ECNAccountSortDir";
        protected override string SfSortExpKey => "SFAccountSortExp";
        protected override string EcnSortExpKey => "ECNAccountSortExp";

        #region properties
        private List<SF_Account> SF_Account_List
        {
            get
            {
                if (Session["SF_Accounts"] != null)
                    return (List<SF_Account>)Session["SF_Accounts"];
                else
                    return null;
            }
            set
            {
                Session["SF_Accounts"] = value;
            }
        }
        private List<ECN_Framework_Entities.Accounts.Customer> ECN_Customer_List
        {
            get
            {
                if (Session["ECN_Customers"] != null)
                    return (List<ECN_Framework_Entities.Accounts.Customer>)Session["ECN_Customers"];
                else
                    return null;
            }
            set
            {
                Session["ECN_Customers"] = value;
            }
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Setup(Master);
            if (!Page.IsPostBack)
            {
                if (SF_Authentication.LoggedIn == true)
                {
                    pnlGrids.Visible = true;
                    LoadSF_Accounts();
                    LoadECN_Customers();

                    DataBindLists();
                    BindSearch();
                }
                else
                {
                    pnlGrids.Visible = false;
                    ShowMessage("You must first log into Salesforce to use this page", "Salesforce Login Required", Salesforce.Controls.Message.Message_Icon.error);
                }
            }
        }
        #endregion

        #region Grid Databinding
        private void DataBindLists()
        {

            gvECNAccounts.DataSource = ECN_Customer_List;
            gvECNAccounts.DataBind();

            gvSFAccounts.DataSource = SF_Account_List;
            gvSFAccounts.DataBind();

            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = false;

            if (ECN_Customer_List.Count > 0)
            {
                gvECNAccounts.Visible = true;
                lblECNAccounts.Visible = true;
            }
            else
            {
                gvECNAccounts.Visible = false;
                lblECNAccounts.Visible = false;
            }

            if (SF_Account_List.Count > 0)
            {
                gvSFAccounts.Visible = true;
                lblSFAccounts.Visible = true;
            }
            else
            {
                gvSFAccounts.Visible = false;
                lblSFAccounts.Visible = false;
            }

            ddlFilter.SelectedIndex = 0;
        }
        private void LoadSF_Accounts()
        {
            //SF_Account_List = SF_Account.GetList(SF_Authentication.Token.access_token, "WHERE Name like 'J%'");
            SF_Account_List = SF_Account.GetAll(SF_Authentication.Token.access_token).OrderBy(x => x.Name).ToList();
        }
        private void LoadECN_Customers()
        {
            LoadBaseChannels();
            LoadChannels();
            PopulateCustomersList();
        }

        private void LoadBaseChannels()
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            var result = (from src in bcList
                          orderby src.BaseChannelName
                          select new
                          {
                              BaseChannelID = src.BaseChannelID,
                              BaseChannelName = src.BaseChannelName
                          }).ToList();
            ddlBaseChannel.DataSource = result;
            ddlBaseChannel.DataValueField = "BaseChannelID";
            ddlBaseChannel.DataTextField = "BaseChannelName";
            ddlBaseChannel.DataBind();
            ddlBaseChannel.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
        }

        private void LoadChannels()
        {
            List<ECN_Framework_Entities.Accounts.Channel> lChannels = ECN_Framework_BusinessLayer.Accounts.Channel.GetByBaseChannelID(int.Parse(ddlBaseChannel.SelectedValue));

            var channels = from c in lChannels
                           orderby c.ChannelName
                           select new { c.ChannelID, ChannelName = c.ChannelName + " (" + c.ChannelTypeCode + ")" };

            ddlProductLine.DataSource = channels;
            ddlProductLine.DataValueField = "ChannelID";
            ddlProductLine.DataTextField = "ChannelName";
            ddlProductLine.DataBind();

            foreach (ListItem li in ddlProductLine.Items)
            {
                if (li.Text.EndsWith("communicator)"))
                {
                    ddlProductLine.Items.FindByText(li.Text).Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region Insert/Update/Compare button events
        private bool CompareRecords(SF_Account sf, ECN_Framework_Entities.Accounts.Customer ecn)
        {
            if (!sf.BillingCity.Trim().ToLower().Equals(ecn.City.Trim().ToLower()))
                return false;
            else if (!sf.BillingPostalCode.Trim().Equals(ecn.Zip.Trim()))
                return false;
            else if (!sf.BillingState.Trim().ToLower().Equals(ecn.State.Trim().ToLower()))
                return false;
            else if (!sf.BillingStreet.Trim().ToLower().Equals(ecn.Address.Trim().ToLower()))
                return false;
            else if (!sf.Fax.Trim().Equals(ecn.Fax.Trim()))
                return false;
            else if (!sf.Phone.Trim().Equals(ecn.Phone.Trim()))
                return false;
            else
                return true;
        }

        protected void imgbtnCompare_Click(object sender, ImageClickEventArgs e)
        {
            ResetSelection();

            var items = GetItems();
            var customer = items.Item1;
            var account = items.Item2;
            var ecnAddress = GetAddress(customer);
            var sfAddress = GetAddress(account);

            var converter = new EcnSfAccountConverter();
            var model = converter.Convert(customer, account, ecnAddress, sfAddress);

            FillHidden(model, hfECNCustomerID, hfSFAccountID);
            Fill(lblECNAddress, lblSFAddress, rblAddress, model.Address);
            Fill(lblECNCity, lblSFCity, rblCity, model.City);
            Fill(lblECNState, lblSFState, rblState, model.State);
            Fill(lblECNZip, lblSFZip, rblZip, model.Zip);
            Fill(lblECNCountry, lblSFCountry, rblCountry, model.Country);
            Fill(lblECNPhone, lblSFPhone, rblPhone, model.Phone);
            Fill(lblECNFax, lblSFFax, rblFax, model.Fax);
            Fill(lblECNName, lblSFName, rblName, model.Name);

            mpeCompare.Show();
        }

        protected void imgbtnECNToSF_Click(object sender, ImageClickEventArgs e)
        {
            int success = 0;
            int fail = 0;
            int total = 0;
            bool successful = false;
            foreach (GridViewRow gvr in gvECNAccounts.Rows)
            {

                CheckBox chkECN = (CheckBox)gvr.FindControl("cbECNselect");
                if (chkECN != null && chkECN.Checked)
                {
                    total++;
                    if (gvr.BackColor.Equals(Salesforce.Controls.KM_Colors.GreyLight))
                    {
                        SF_Account sf = new SF_Account();
                        int id = Convert.ToInt32(chkECN.Attributes["CustomerID"].ToString());
                        ECN_Framework_Entities.Accounts.Customer c = ECN_Customer_List.First(x => x.CustomerID == id);

                        sf.BillingCity = c.City;
                        sf.BillingCountry = c.Country;
                        sf.BillingPostalCode = c.Zip;
                        sf.BillingState = c.State;
                        sf.BillingStreet = c.Address;
                        sf.Fax = c.Fax;
                        sf.Name = c.CustomerName;
                        sf.Phone = c.Phone;

                        bool succ = SF_Account.Insert(SF_Authentication.Token.access_token, sf);
                        if (succ)
                        {
                            SF_Account_List.Add(sf);
                            success++;
                            successful = true;
                        }
                        else
                            fail++;
                    }
                    else if (gvr.BackColor.Equals(Salesforce.Controls.KM_Colors.GreyDark))
                    {
                        int id = Convert.ToInt32(chkECN.Attributes["CustomerID"].ToString());
                        ECN_Framework_Entities.Accounts.Customer c = ECN_Customer_List.First(x => x.CustomerID == id);
                        SF_Account sf = SF_Account_List.First(x => x.Name == c.CustomerName);

                        sf.BillingCity = c.City;
                        sf.BillingCountry = c.Country;
                        sf.BillingPostalCode = c.Zip;
                        sf.BillingState = c.State;
                        sf.BillingStreet = c.Address;
                        sf.Fax = c.Fax;
                        sf.Name = c.CustomerName;
                        sf.Phone = c.Phone;

                        bool succ = SF_Account.Update(SF_Authentication.Token.access_token, sf);
                        if (succ)
                        {
                            success++;
                            SF_Account_List.Remove(SF_Account_List.First(x => x.Id == sf.Id));
                            SF_Account_List.Add(sf);
                            successful = true;
                        }
                        else
                            fail++;
                    }
                }
            }

            if (successful)
                DisplayResults(success.ToString() + " records updated.<br />" + fail.ToString() + " records failed to update.");
            else
                DisplayResults(success.ToString() + " records updated.<br />" + fail.ToString() + " records failed to update.");

            DataBindLists();
            imgbtnECNToSF.Visible = false;
        }

        private void DisplayResults(string message)
        {
            lblMessage.Text = message;
            mpeResults.Show();
        }

        protected void btnSyncData_Click(object sender, EventArgs e)
        {
            int ECNid = Convert.ToInt32(hfECNCustomerID.Value);
            ECN_Framework_Entities.Accounts.Customer c = ECN_Customer_List.First(x => x.CustomerID == ECNid);
            c.BillingContact = ECN_Framework_BusinessLayer.Accounts.Contact.GetByCustomerID(c.CustomerID, Master.UserSession.CurrentUser);
            c.GeneralContant = ECN_Framework_BusinessLayer.Accounts.Contact.GetByCustomerID(c.CustomerID, Master.UserSession.CurrentUser);
            string SFid = hfSFAccountID.Value;
            SF_Account sf = SF_Account_List.First(x => x.Id.Equals(SFid.ToString()));

            //sf.BillingLatitude = Address_SF.Latitude;
            //sf.BillingLongitude = Address_SF.Longitude;

            if (rblName.SelectedValue.Equals("SF"))
            {
                c.CustomerName = sf.Name;
            }
            else if (rblName.SelectedValue.Equals("ECN"))
            {
                sf.Name = c.CustomerName;
            }

            if (rblAddress.SelectedValue.Equals("SF"))
            {
                c.Address = sf.BillingStreet.Trim();
                c.GeneralContant.StreetAddress = sf.BillingStreet.Trim();
            }
            else if (rblAddress.SelectedValue.Equals("ECN"))
            {
                sf.BillingStreet = c.Address;
            }

            if (rblCity.SelectedValue.Equals("SF"))
            {
                c.City = sf.BillingCity.Trim();
                c.GeneralContant.City = sf.BillingCity.Trim();
            }
            else if (rblCity.SelectedValue.Equals("ECN"))
            {
                sf.BillingCity = c.City;
            }

            if (rblState.SelectedValue.Equals("SF"))
            {
                c.State = sf.BillingState;
                c.GeneralContant.State.Trim();
            }
            else if (rblState.SelectedValue.Equals("ECN"))
            {
                sf.BillingState = c.State;
            }

            if (rblZip.SelectedValue.Equals("SF"))
            {
                c.Zip = sf.BillingPostalCode.Trim();
                c.GeneralContant.Zip = sf.BillingPostalCode.Trim();
            }
            else if (rblZip.SelectedValue.Equals("ECN"))
            {
                sf.BillingPostalCode = c.Zip;
            }

            if (rblCountry.SelectedValue.Equals("SF"))
            {
                c.Country = sf.BillingCountry.Trim();
                c.GeneralContant.Country = sf.BillingCountry.Trim();
            }
            else if (rblCountry.SelectedValue.Equals("ECN"))
            {
                sf.BillingCountry = c.Country;
            }

            if (rblPhone.SelectedValue.Equals("SF"))
            {
                c.Phone = sf.Phone.Trim();
                c.GeneralContant.Phone = sf.Phone.Trim();
            }
            else if (rblPhone.SelectedValue.Equals("ECN"))
            {
                sf.Phone = c.Phone;
            }

            if (rblFax.SelectedValue.Equals("SF"))
            {
                c.Fax = sf.Fax.Trim();
                c.GeneralContant.Fax = sf.Fax.Trim();
            }
            else if (rblFax.SelectedValue.Equals("ECN"))
            {
                sf.Fax = c.Fax;
            }

            imgbtnCompare.Visible = false;
            ddlFilter.SelectedIndex = 0;
            c.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            c.UpdatedDate = DateTime.Now;

            c.BillingContact.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

            c.GeneralContant.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            c.GeneralContant.StreetAddress = c.Address.Trim();
            try
            {
                int resultID = ECN_Framework_BusinessLayer.Accounts.Customer.Save(c, Master.UserSession.CurrentUser);
                ECN_Customer_List.Remove(ECN_Customer_List.First(x => x.CustomerID == c.CustomerID));
                ECN_Customer_List.Add(c);

                bool succ = SF_Account.Update(SF_Authentication.Token.access_token, sf);
                SF_Account_List.Remove(SF_Account_List.First(x => x.Id == sf.Id));
                SF_Account_List.Add(sf);

                DataBindLists();
                UncheckECN();
                UncheckSF();
                mpeCompare.Hide();
                if (resultID > 0 && succ)
                {
                    DisplayResults("Update Successful");
                }
                else
                {
                    DisplayResults("Update Failed");
                }
            }
            catch (Exception ex)
            {
                SF_Utilities.LogException(ex);
                DisplayResults("Update Failed.  Support has been notified.");
            }
        }

        #endregion

        #region Grid Events
        protected void gvSFAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SF_Account current = (SF_Account)e.Row.DataItem;
                CheckBox cbSFselect = (CheckBox)e.Row.FindControl("cbSFselect");
                if (cbSFselect != null)
                    cbSFselect.Attributes.Add("Id", current.Id.ToString());

                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (ECN_Customer_List.Exists(x => x.CustomerName == current.Name))
                {
                    ECN_Framework_Entities.Accounts.Customer ecnContact = ECN_Customer_List.First(x => x.CustomerName == current.Name);
                    bool isSame = CompareRecords(current, ecnContact);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Account is in SF and ECN but data is different";
                        if (cbSFselect != null)
                            cbSFselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueLight;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Account is in SF and ECN and data is the same";
                        if (cbSFselect != null)
                            cbSFselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Account is ONLY in SF";
                    if (cbSFselect != null)
                    {
                        cbSFselect.Attributes.Add("Color", "GreyLight");//only in this table
                        cbSFselect.Visible = false;
                    }
                }
            }
        }
        protected void cbSFselectALL_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in gvSFAccounts.Rows)
            {
                if (gvr.BackColor == Salesforce.Controls.KM_Colors.GreyLight)
                {
                    CheckBox cb = (CheckBox)gvr.FindControl("cbSFselect");
                    if (cb != null)
                        cb.Checked = isChecked;
                }
            }

            if (isChecked == true)
            {
                //cant do a compare with multi selection
                imgbtnCompare.Visible = false;


                imgbtnECNToSF.Visible = false;
            }
            else
            {
                imgbtnCompare.Visible = false;


                imgbtnECNToSF.Visible = false;
            }


        }

        protected void cbSFselect_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.SF);
        }

        protected void gvECNAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Accounts.Customer current = (ECN_Framework_Entities.Accounts.Customer)e.Row.DataItem;
                CheckBox cbECNselect = (CheckBox)e.Row.FindControl("cbECNselect");
                if (cbECNselect != null)
                    cbECNselect.Attributes.Add("CustomerID", current.CustomerID.ToString());

                //SF_Account current = SF_Account_List.First(x => x.Id == gvSFAccounts.DataKeys[gvr.RowIndex].ToString());
                if (SF_Account_List.Exists(x => x.Name == current.CustomerName))
                {
                    SF_Account sfAccount = SF_Account_List.First(x => x.Name == current.CustomerName);
                    bool isSame = CompareRecords(sfAccount, current);
                    if (!isSame)
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyDark;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Account is in ECN and SF but data is different";
                        if (cbECNselect != null)
                            cbECNselect.Attributes.Add("Color", "GreyDark");//in both tables but have differnt data
                    }
                    else
                    {
                        e.Row.BackColor = Salesforce.Controls.KM_Colors.BlueLight;
                        e.Row.ForeColor = System.Drawing.Color.White;
                        e.Row.ToolTip = "Account is in ECN and SF and data is the same";
                        if (cbECNselect != null)
                            cbECNselect.Visible = false;//in both tables and data is the same
                    }
                }
                else
                {
                    e.Row.BackColor = Salesforce.Controls.KM_Colors.GreyLight;
                    e.Row.ForeColor = Salesforce.Controls.KM_Colors.GreyDark;
                    e.Row.ToolTip = "Account is ONLY in ECN";
                    if (cbECNselect != null)
                        cbECNselect.Attributes.Add("Color", "GreyLight");//only in this table
                }
            }
        }
        protected void cbECNselectALL_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox s = (CheckBox)sender;
            bool isChecked = s.Checked;
            foreach (GridViewRow gvr in gvECNAccounts.Rows)
            {
                if (gvr.BackColor == Salesforce.Controls.KM_Colors.GreyLight)
                {
                    CheckBox cb = (CheckBox)gvr.FindControl("cbECNselect");
                    if (cb != null)
                        cb.Checked = isChecked;
                }
            }

            if (isChecked == true)
            {
                //cant do a compare with multi selection
                imgbtnCompare.Visible = false;

                //only thing we can do is SF to ECN
                imgbtnECNToSF.Visible = true;


            }
            else
            {
                imgbtnCompare.Visible = false;
                imgbtnECNToSF.Visible = false;


            }
        }

        protected void cbECNselect_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            UpdateControlsOnCheck(checkbox, GroupType.ECN);
        }

        private void UncheckECN()
        {
            Uncheck(GroupType.ECN);
        }

        private void UncheckSF()
        {
            Uncheck(GroupType.SF);
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SFAccounts = 0;
            int ECNAccounts = 0;
            gvECNAccounts.Visible = true;
            gvSFAccounts.Visible = true;
            imgbtnECNToSF.Visible = false;
            imgbtnCompare.Visible = false;
            UncheckECN();
            UncheckSF();

            imgbtnECNToSF.Visible = false;
            imgbtnCompare.Visible = false;

            if (ddlFilter.SelectedValue.Equals("OnlySF"))
            {
                gvECNAccounts.Visible = false;

                foreach (GridViewRow gvr in gvSFAccounts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyLight)
                    {
                        gvr.Visible = false;
                    }
                    else
                    {
                        gvr.Visible = true;
                        SFAccounts++;
                    }
                }
            }
            else if (ddlFilter.SelectedValue.Equals("OnlyECN"))
            {
                gvSFAccounts.Visible = false;

                foreach (GridViewRow gvr in gvECNAccounts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyLight)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        ECNAccounts++;
                    }
                }
            }
            else if (ddlFilter.SelectedValue.Equals("DiffData"))
            {
                foreach (GridViewRow gvr in gvECNAccounts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyDark)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        ECNAccounts++;
                    }
                }
                foreach (GridViewRow gvr in gvSFAccounts.Rows)
                {
                    if (gvr.BackColor != Salesforce.Controls.KM_Colors.GreyDark)
                        gvr.Visible = false;
                    else
                    {
                        gvr.Visible = true;
                        SFAccounts++;
                    }
                }
            }
            else if (ddlFilter.SelectedValue.Equals("All"))
            {
                foreach (GridViewRow gvr in gvECNAccounts.Rows)
                {
                    gvr.Visible = true;
                    ECNAccounts++;
                }
                foreach (GridViewRow gvr in gvSFAccounts.Rows)
                {
                    gvr.Visible = true;
                    SFAccounts++;
                }
            }

            if (ECNAccounts > 0)
            {
                gvECNAccounts.Visible = true;
                lblECNAccounts.Visible = true;
            }
            else
            {
                gvECNAccounts.Visible = false;
                lblECNAccounts.Visible = false;
            }

            if (SFAccounts > 0)
            {
                gvSFAccounts.Visible = true;
                lblSFAccounts.Visible = true;
            }
            else
            {
                gvSFAccounts.Visible = false;
                lblSFAccounts.Visible = false;
            }
        }

        protected void gvSFAccounts_Sorting(object sender, GridViewSortEventArgs e)
        {
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

            SF_Account_List = SF_Account.Sort(SF_Account_List, SFSortExp, SFSortDir).ToList();
            gvSFAccounts.DataSource = SF_Account_List;
            gvSFAccounts.DataBind();
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = false;

        }
        protected void gvECNAccounts_Sorting(object sender, GridViewSortEventArgs e)
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

            ECN_Customer_List = ECN_Sort.Sort_Customers(ECN_Customer_List, ECNSortExp, ECNSortDir).ToList();
            gvECNAccounts.DataSource = ECN_Customer_List;
            gvECNAccounts.DataBind();
        }

        #endregion

        #region Search
        private void BindSearch()
        {
            kmSearch.BindSearch(SF_Utilities.SFObject.Account);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string where = kmSearch.GetQuery();
            ddlFilter.SelectedIndex = 0;
            List<SF_Account> search = SF_Account.GetList(SF_Authentication.Token.access_token, where).ToList();
            gvSFAccounts.DataSource = search;
            gvSFAccounts.DataBind();

            divSFAccounts.Visible = true;
        }
        protected void btnSearchReset_Click(object sender, EventArgs e)
        {
            kmSearch.ResetSearch();
            SF_Account_List = new List<SF_Account>();
            ECN_Customer_List = new List<ECN_Framework_Entities.Accounts.Customer>();
            DataBindLists();
        }
        #endregion

        protected void ddlBaseChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChannels();
            LoadCustomers();

        }

        protected void ddlProductLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            PopulateCustomersList();
            DataBindLists();
        }

        private IEnumerable<ECN_Framework_Entities.Accounts.Customer> FilterByProductLineAndOrderByName(IEnumerable<ECN_Framework_Entities.Accounts.Customer> lCustomers, int id)
        {
            return lCustomers.Where(c => c.CommunicatorChannelID == id ||
                                         c.CollectorChannelID == id ||
                                         c.CreatorChannelID == id ||
                                         c.PublisherChannelID == id ||
                                         c.CharityChannelID == id)
                            .OrderBy(x => x.CustomerName);
        }

        private void PopulateCustomersList()
        {
            var baseChannelId = 0;
            var productLineId = 0;
            if (int.TryParse(ddlBaseChannel?.SelectedItem?.Value, out baseChannelId) &&
                int.TryParse(ddlProductLine?.SelectedItem?.Value, out productLineId))
            {
                var lCustomers = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(baseChannelId);
                ECN_Customer_List = FilterByProductLineAndOrderByName(lCustomers, productLineId).ToList();
            }
        }

        protected override void PopulateActionsOnRowColor(GroupType grp)
        {
            AddActionByColor(KM_Colors.GreyDark, UpdateButtonsOnGreyDark);
            AddActionByColor(KM_Colors.GreyLight, UpdateButtonsOnGreyLight);
            AddActionByColor(KM_Colors.BlueLight, (arg, g) => ResetButtons());
        }

        protected override void UpdateButtons_Multi(GroupType grp)
        {
            //cant do a compare with multi selection
            imgbtnCompare.Visible = false;
            //only thing we can do is ECN to SF
            imgbtnECNToSF.Visible = grp.IsEcn();
        }

        protected override void ResetButtons()
        {
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = false;
        }

        private void UpdateButtonsOnGreyDark(string arg, GroupType grp)
        {
            imgbtnCompare.Visible = true;
            imgbtnCompare.CommandArgument = arg;
            imgbtnCompare.CommandName = GetCommandName(grp);
            imgbtnECNToSF.Visible = false;
        }

        private void UpdateButtonsOnGreyLight(string arg, GroupType grp)
        {
            imgbtnCompare.Visible = false;
            imgbtnECNToSF.Visible = grp.IsEcn();
            if (grp.IsEcn())
            {
                imgbtnECNToSF.CommandArgument = arg;
            }
        }

        private Tuple<ECN_Framework_Entities.Accounts.Customer, SF_Account> GetItems()
        {
            var customer = new ECN_Framework_Entities.Accounts.Customer();
            var account = new SF_Account();
            const string ecnCommand = "ecn";

            if (imgbtnCompare.CommandName.Equals(ecnCommand, StringComparison.OrdinalIgnoreCase))
            {
                var id = Convert.ToInt32(imgbtnCompare.CommandArgument);
                customer = ECN_Customer_List.First(x => x.CustomerID == id);
                account = SF_Account_List.First(x => x.Name.Equals(customer.CustomerName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                var id = imgbtnCompare.CommandArgument;
                account = SF_Account_List.First(x => x.Id.Equals(id));
                customer = ECN_Customer_List.First(x => x.CustomerName.Equals(account.Name, StringComparison.OrdinalIgnoreCase));
            }

            return new Tuple<ECN_Framework_Entities.Accounts.Customer, SF_Account>(customer, account);
        }

        private AddressValidator.AddressLocation GetAddress(ECN_Framework_Entities.Accounts.Customer customer)
        {
            var addressLocation = new AddressValidator.AddressLocation
            {
                City = customer.City,
                IsValid = false,
                Latitude = 0,
                Longitude = 0,
                PostalCode = customer.Zip,
                Region = customer.State,
                Street = customer.Address,
                ValidationMessage = string.Empty
            };

            var valid = addressLocation.GetValidAddress();
            if (valid != null)
            {
                Address_ECN = valid;
            }
            return Address_ECN;
        }

        private AddressValidator.AddressLocation GetAddress(SF_Account account)
        {
            var addressLocation = new AddressValidator.AddressLocation
            {
                City = account.BillingCity,
                IsValid = false,
                Latitude = 0,
                Longitude = 0,
                PostalCode = account.BillingPostalCode,
                Region = account.BillingState,
                Street = account.BillingStreet,
                ValidationMessage = string.Empty
            };

            var valid = addressLocation.GetValidAddress();
            if (valid != null)
            {
                Address_SF = valid;
            }

            return Address_SF;
        }

        private void ResetSelection()
        {
            rblAddress.SelectedIndex = -1;
            rblCity.SelectedIndex = -1;
            rblCountry.SelectedIndex = -1;
            rblFax.SelectedIndex = -1;
            rblName.SelectedIndex = -1;
            rblPhone.SelectedIndex = -1;
            rblState.SelectedIndex = -1;
            rblZip.SelectedIndex = -1;
        }
    }
}