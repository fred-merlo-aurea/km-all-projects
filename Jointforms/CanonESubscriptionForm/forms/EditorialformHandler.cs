using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.IO;
using System.Net;

using ecn.common.classes;

namespace CanonESubscriptionForm.forms
{
    public class EditorialformHandler : System.Web.UI.Page
    {
        protected global::System.Web.UI.HtmlControls.HtmlForm frmsub1;
        protected global::System.Web.UI.WebControls.Panel pnlCurrent;
        protected global::System.Web.UI.WebControls.Panel pnlText1;
        protected global::System.Web.UI.WebControls.Panel pnlText2;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.Button btnEmailClick;
        protected global::System.Web.UI.WebControls.Panel pnlProfile;
        protected global::System.Web.UI.WebControls.TextBox fn;
        protected global::System.Web.UI.WebControls.TextBox ln;
        protected global::System.Web.UI.WebControls.TextBox t;
        protected global::System.Web.UI.WebControls.TextBox compname;
        protected global::System.Web.UI.WebControls.TextBox adr;
        protected global::System.Web.UI.WebControls.TextBox adr2;
        protected global::System.Web.UI.WebControls.TextBox city;
        protected global::System.Web.UI.WebControls.DropDownList state;
        protected global::System.Web.UI.WebControls.TextBox zc;
        protected global::System.Web.UI.WebControls.DropDownList ctry;
        protected global::System.Web.UI.WebControls.TextBox ph;
        protected global::System.Web.UI.WebControls.TextBox fax;
        protected global::System.Web.UI.WebControls.Panel pnlquestions;
        protected global::System.Web.UI.WebControls.Panel pnllogos;
        protected global::System.Web.UI.WebControls.Button btnSubmit;
        protected global::System.Web.UI.WebControls.DropDownList user_business;
        protected global::System.Web.UI.WebControls.DropDownList user_function;
        protected global::System.Web.UI.WebControls.Label lblErrorMessage;

        int SFGroupID = 0;
        int CustomerID = 0;
        string SubscriptionGroupIDs = string.Empty;

        private string Mag
        {
            get
            {
                try { return Request.QueryString["Mag"].ToString(); }
                catch { return string.Empty; }
            }
        }

        private string getPC
        {
            get
            {
                try { return Request.QueryString["PC"].ToString(); }
                catch { return "NNWEB"; }
            }
        }

        private string PubGroup
        {
            get
            {
                if (Mag.ToLower() == "appl" || Mag.ToLower() == "pbs")
                    return "group1";
                else if (Mag.ToLower() == "imm" || Mag.ToLower() == "mpw")
                    return "group2";
                else if (Mag.ToLower() == "mddi" || Mag.ToLower() == "mx")
                    return "group3";
                else if (Mag.ToLower() == "nutr") // || Mag.ToLower() == "cpcp"
                    return "group4";
                else if (Mag.ToLower() == "emd" || Mag.ToLower() == "cmdm" || Mag.ToLower() == "mdt")
                    return "group5";
                else if (Mag.ToLower() == "pmpn")
                    return "group6";
                else if (Mag.ToLower() == "ivd" || Mag.ToLower() == "mpmn" || Mag.ToLower() == "mem" || Mag.ToLower() == "mtp")
                    return "group7";

                return string.Empty;
            }
        }
 
        protected void Page_Load(object sender, EventArgs eargs)
        {
            if (ConfigurationManager.AppSettings["RedirectToJointForms"].ToString().ToLower().IndexOf(Mag.ToLower()) > -1)
            {
                string redirectURL = "http://eforms.kmpsgroup.com/jointforms/Forms/Subscription.aspx?pubcode=" + Mag.ToUpper();

                if (Mag.ToLower() == "pmpn")
                    redirectURL = "http://eforms.kmpsgroup.com/jointforms/Forms/Subscription.aspx?pubcode=phar";

                if (Mag.ToLower() == "mpw")
                    redirectURL = "http://eforms.kmpsgroup.com/jointforms/Forms/Subscription.aspx?pubcode=imm";

                try
                {
                    if (Request.QueryString["PC"].ToString().Length > 0)
                        redirectURL += "&promoCode=" + Request.QueryString["PC"].ToString();
                }
                catch { }
                Response.Redirect(redirectURL);
            }

            lblErrorMessage.Visible = false;
            btnEmailClick.Attributes.Add("onclick", "javascript:return validateEmail();");
            if (!IsPostBack)
            {
                pnlText1.Visible = true;
                pnlText2.Visible = true;
                btnSubmit.Enabled = false;

                if (PubGroup == string.Empty)
                {
                    lblErrorMessage.Text = "Click on the link from your email, to access this Site. Thank you.";
                    lblErrorMessage.Visible = true;
                    btnEmailClick.Visible = false;
                }
            }
        }

        protected void btnEmailClick_Click(object sender, EventArgs eargs)
        {
            DataTable dtEmail = DataFunctions.GetDataTable("select e.EmailID,isnull(EmailAddress	,'') as EmailAddress,isnull(FirstName,'') as FirstName,isnull(LastName,'') as LastName,isnull(Title,'') as Title,isnull(Company,'') as Company,isnull(Address,'') as Address,isnull(Address2,'') as Address2,isnull(City,'') as City,isnull(State,'') as State,isnull(Zip,'') as Zip,isnull(Country,'') as Country,isnull(Voice,'') as Voice,isnull(Fax,'') as Fax from emails e join emailgroups eg on e.emailID = eg.emailID where e.emailaddress = '" + txtEmail.Text.Replace("'", "''") + "' and customerID = " + ConfigurationManager.AppSettings["canon_CustomerID"]);
            loadstate();
            if (dtEmail.Rows.Count > 0)
            {
                if (dtEmail.Rows[0]["FirstName"].ToString() != string.Empty)
                    fn.Text = dtEmail.Rows[0]["FirstName"].ToString();

                if (dtEmail.Rows[0]["LastName"].ToString() != string.Empty)
                    ln.Text = dtEmail.Rows[0]["LastName"].ToString();

                if (dtEmail.Rows[0]["Title"].ToString() != string.Empty)
                    t.Text = dtEmail.Rows[0]["Title"].ToString();

                if (dtEmail.Rows[0]["Company"].ToString() != string.Empty)
                    compname.Text = dtEmail.Rows[0]["Company"].ToString();
                if (dtEmail.Rows[0]["Address"].ToString() != string.Empty)
                    adr.Text = dtEmail.Rows[0]["Address"].ToString();
                if (dtEmail.Rows[0]["Address2"].ToString() != string.Empty)
                    adr2.Text = dtEmail.Rows[0]["Address2"].ToString();
                if (dtEmail.Rows[0]["City"].ToString() != string.Empty)
                    city.Text = dtEmail.Rows[0]["City"].ToString();

                try
                {
                    if (dtEmail.Rows[0]["State"].ToString() != string.Empty)
                    {
                        state.ClearSelection();
                        state.Items.FindByValue(dtEmail.Rows[0]["State"].ToString()).Selected = true;
                    }
                }
                catch { }

                if (dtEmail.Rows[0]["Zip"].ToString() != string.Empty)
                    zc.Text = dtEmail.Rows[0]["Zip"].ToString();
                try
                {
                    if (dtEmail.Rows[0]["Country"].ToString() != string.Empty)
                        ctry.ClearSelection();
                    ctry.Items.FindByValue(dtEmail.Rows[0]["Country"].ToString()).Selected = true;
                }
                catch { }

                if (dtEmail.Rows[0]["Voice"].ToString() != string.Empty)
                    ph.Text = dtEmail.Rows[0]["Voice"].ToString();

                if (dtEmail.Rows[0]["Fax"].ToString() != string.Empty)
                    fax.Text = dtEmail.Rows[0]["Fax"].ToString();
                
                getSmartFormDetails();
                loadUDFValues(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));
                loadSubscribedGroups(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));
            }
            pnlText1.Visible = false;
            pnlText2.Visible = false;
            pnlCurrent.Visible = true;
            pnlProfile.Visible = true;
            pnlquestions.Visible = true;
            pnllogos.Visible = true;
            btnEmailClick.Visible = false;
            txtEmail.Enabled = false;
            btnSubmit.Enabled = true;
        }

        private void getSmartFormDetails()
        {
            string smartFormSql = " SELECT g.customerID, sf.groupID, sf.SubscriptionGroupIDs FROM SmartFormsHistory sf join groups g on sf.groupID = g.groupID WHERE SmartFormID= " + ConfigurationManager.AppSettings["canon_" + PubGroup + "_SFID"].ToString();

            DataTable dt = DataFunctions.GetDataTable(smartFormSql);
            DataRow dr = dt.Rows[0];

            SFGroupID = Convert.ToInt32(dr["GroupID"]);
            CustomerID = Convert.ToInt32(dr["CustomerID"]);

            if (dr.IsNull("SubscriptionGroupIDs"))
                SubscriptionGroupIDs = SFGroupID.ToString();
            else
                SubscriptionGroupIDs = SubscriptionGroupIDs == string.Empty ? dr["SubscriptionGroupIDs"].ToString().Trim() : "," + dr["SubscriptionGroupIDs"].ToString().Trim();
        }

        private void loadUDFValues(int EmailID)
        {
            DataTable dtUDF = DataFunctions.GetDataTable("select distinct shortname, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + EmailID + " and groupID in (" + SubscriptionGroupIDs + ")");

            foreach (DataRow dr in dtUDF.Rows)
            {
                try
                {
                    if (dr["shortname"].ToString().ToLower() == "business")
                    {
                        try
                        {
                            user_business.ClearSelection();
                            user_business.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
                        }
                        catch { }
                    }
                    else if (dr["shortname"].ToString().ToLower() == "function")
                    {
                        try
                        {
                            user_function.ClearSelection();
                            user_function.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
                        }
                        catch { }
                    }
                }
                catch
                {
                }
            }
        }

        private void loadSubscribedGroups(int EmailID)
        {
            DataTable dtSubscribedGroups = DataFunctions.GetDataTable("select GroupID from emailgroups where subscribetypecode = 'S' and emailID = " + EmailID + " and groupID in (" + SubscriptionGroupIDs + ")");

            foreach (DataRow dr in dtSubscribedGroups.Rows)
            {
                try
                {
                    HtmlInputCheckBox c = (HtmlInputCheckBox)Page.FindControl("g_" + dr["GroupID"].ToString());
                    c.Checked = true;
                }
                catch { }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                getSmartFormDetails();

                string PostParams = string.Empty;

                PostParams = "?c=" + System.Configuration.ConfigurationManager.AppSettings["canon_CustomerID"].ToString();
                PostParams += "&f=html&s=S&SFmode=manage";
                PostParams += "&sfID=" + System.Configuration.ConfigurationManager.AppSettings["canon_" + PubGroup + "_SFID"].ToString();
                PostParams += "&e=" + txtEmail.Text;
                PostParams += "&fn=" + fn.Text;
                PostParams += "&ln=" + ln.Text;
                PostParams += "&t=" + t.Text;
                PostParams += "&n=" + fn.Text + " " + ln.Text;
                PostParams += "&compname=" + compname.Text;
                PostParams += "&adr=" + adr.Text;
                PostParams += "&adr2=" + adr2.Text;
                PostParams += "&city=" + city.Text;
                PostParams += "&state=" + state.SelectedItem.Value;
                PostParams += "&zc=" + zc.Text;
                PostParams += "&ctry=" + ctry.SelectedItem.Value;
                PostParams += "&ph=" + ph.Text;
                PostParams += "&fax=" + fax.Text;
                PostParams += "&user_business=" + user_business.SelectedItem.Value;
                PostParams += "&user_function=" + user_function.SelectedItem.Value;
                PostParams += "&user_demo5=Y";
                PostParams += "&user_SubSrc=" + getPC;
                
                string[] groups = SubscriptionGroupIDs.Split(',');
                for (int j = 0; j < groups.Length; j++)
                {
                    try
                    {
                        HtmlInputCheckBox c = (HtmlInputCheckBox)Page.FindControl("g_" + groups[j]);
                        if (c.Checked)
                            PostParams += "&g_" + groups[j] + "=y";
                        else
                            PostParams += "&g_" + groups[j] + "=n";    
                    }
                    catch { }
                }

                //HTTP Post.
                HttpPost(PostParams);

                Response.Redirect("Editorialform_Thankyou.aspx");
            }
            catch (WebException ex)
            {
               lblErrorMessage.Visible = true;
               lblErrorMessage.Text = ex.Message;
            }
        }

        private void HttpPost(string postparams)
        {
            //Response.Write(String.Format(ConfigurationManager.AppSettings["ECN_ActivityEngine_MultiGroupSubscribe_Path"].ToString(),postparams));

            WebRequest webRequest = WebRequest.Create(String.Format(ConfigurationManager.AppSettings["ECN_ActivityEngine_MultiGroupSubscribe_Path"].ToString(), postparams));
            webRequest.Method = "GET";
            WebResponse WebResp = webRequest.GetResponse();
        } 

        #region State Dropdown
        private void loadstate()
        {
            addlistitem("", "Select a State", "");
            addlistitem("AK", "Alaska", "USA");
            addlistitem("AL", "Alabama", "USA");
            addlistitem("AR", "Arkansas", "USA");
            addlistitem("AZ", "Arizona", "USA");
            addlistitem("CA", "California", "USA");
            addlistitem("CO", "Colorado", "USA");
            addlistitem("CT", "Connecticut", "USA");
            addlistitem("DC", "Washington D.C.", "USA");
            addlistitem("DE", "Delaware", "USA");
            addlistitem("FL", "Florida", "USA");
            addlistitem("GA", "Georgia", "USA");
            addlistitem("HI", "Hawaii", "USA");
            addlistitem("IA", "Iowa", "USA");
            addlistitem("ID", "Idaho", "USA");
            addlistitem("IL", "Illinois", "USA");
            addlistitem("IN", "Indiana", "USA");
            addlistitem("KS", "Kansas", "USA");
            addlistitem("KY", "Kentucky", "USA");
            addlistitem("LA", "Louisiana", "USA");
            addlistitem("MA", "Massachusetts", "USA");
            addlistitem("MD", "Maryland", "USA");
            addlistitem("ME", "Maine", "USA");
            addlistitem("MI", "Michigan", "USA");
            addlistitem("MN", "Minnesota", "USA");
            addlistitem("MO", "Missourri", "USA");
            addlistitem("MS", "Mississippi", "USA");
            addlistitem("MT", "Montana", "USA");
            addlistitem("NC", "North Carolina", "USA");
            addlistitem("ND", "North Dakota", "USA");
            addlistitem("NE", "Nebraska", "USA");
            addlistitem("NH", "New Hampshire", "USA");
            addlistitem("NJ", "New Jersey", "USA");
            addlistitem("NM", "New Mexico", "USA");
            addlistitem("NV", "Nevada", "USA");
            addlistitem("NY", "New York", "USA");
            addlistitem("OH", "Ohio", "USA");
            addlistitem("OK", "Oklahoma", "USA");
            addlistitem("OR", "Oregon", "USA");
            addlistitem("PA", "Pennsylvania", "USA");
            addlistitem("PR", "Puerto Rico", "USA");
            addlistitem("RI", "Rhode Island", "USA");
            addlistitem("SC", "South Carolina", "USA");
            addlistitem("SD", "South Dakota", "USA");
            addlistitem("TN", "Tennessee", "USA");
            addlistitem("TX", "Texas", "USA");
            addlistitem("UT", "Utah", "USA");
            addlistitem("VA", "Virginia", "USA");
            addlistitem("VT", "Vermont", "USA");
            addlistitem("WA", "Washington", "USA");
            addlistitem("WI", "Wisconsin", "USA");
            addlistitem("WV", "West Virginia", "USA");
            addlistitem("WY", "Wyoming", "USA");
            addlistitem("AB", "Alberta", "Canada");
            addlistitem("BC", "British Columbia", "Canada");
            addlistitem("MB", "Manitoba", "Canada");
            addlistitem("NB", "New Brunswick", "Canada");
            addlistitem("NF", "New Foundland", "Canada");
            addlistitem("NS", "Nova Scotia", "Canada");
            addlistitem("ON", "Ontario", "Canada");
            addlistitem("PE", "Prince Edward Island", "Canada");
            addlistitem("QC", "Quebec", "Canada");
            addlistitem("SK", "Saskatchewan", "Canada");
            addlistitem("YT", "Yukon Territories", "Canada");
            addlistitem("OT", "Other", "Foreign");
        }

        private void addlistitem(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            state.Items.Add(item);

        }
        #endregion
    }
}
