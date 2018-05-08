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
using ecn.common.classes;

namespace CanonESubscriptionForm.forms
{
    public partial class PharmaLive_signup : System.Web.UI.Page
    {
        private string EmailAddress
        {
            get
            {
                try
                {
                    return Request.QueryString["emailaddress"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        private string PromoCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pc"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


       private string EmailSource
        {
            get
            {
                try
                {
                    return Request.QueryString["EmailSource"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        } 

        int  SFGroupID = 0;
        int CustomerID = 0;
        string SubscriptionGroupIDs = string.Empty;

        protected void Page_Load(object sender, EventArgs eargs)
        {
            btnSubmit.Attributes.Add("OnClick", "javascript:return validateForm();");

            if (!IsPostBack)
            {
                loadstate();
               
                if (EmailSource.ToLower() == "f2f")
                {
                    txtpromo.Text = String.Format("X{0}{1}04",DateTime.Now.Year.ToString().Substring(3),DateTime.Now.Month.ToString("d2"));
                }
                else
                {
                 txtpromo.Text = PromoCode;
                }
            }

            if (EmailAddress != string.Empty)
            {
                getSmartFormDetails();
                loadform();
            }
        }

        protected void loadform()
        {
            DataTable dtEmail = DataFunctions.GetDataTable("select e.EmailID,isnull(EmailAddress	,'') as EmailAddress,isnull(FirstName,'') as FirstName,isnull(LastName,'') as LastName,isnull(Title,'') as Title,isnull(Company,'') as Company,isnull(Address,'') as Address,isnull(Address2,'') as Address2,isnull(City,'') as City,isnull(State,'') as State,isnull(Zip,'') as Zip,isnull(Country,'') as Country,isnull(Voice,'') as Voice,isnull(Fax,'') as Fax from emails e where e.emailaddress = '" + EmailAddress.Replace("'", "''") + "' and customerID = " + CustomerID);

            if (dtEmail.Rows.Count > 0)
            {
                txtemail.Text = EmailAddress;

                if (dtEmail.Rows[0]["FirstName"].ToString() != string.Empty)
                    txtfirstname.Text = dtEmail.Rows[0]["FirstName"].ToString();

                if (dtEmail.Rows[0]["LastName"].ToString() != string.Empty)
                    txtlastname.Text = dtEmail.Rows[0]["LastName"].ToString();

                if (dtEmail.Rows[0]["Title"].ToString() != string.Empty)
                    txttitle.Text = dtEmail.Rows[0]["Title"].ToString();

                if (dtEmail.Rows[0]["Company"].ToString() != string.Empty)
                    txtcompany.Text = dtEmail.Rows[0]["Company"].ToString();
                if (dtEmail.Rows[0]["Address"].ToString() != string.Empty)
                    txtaddress.Text = dtEmail.Rows[0]["Address"].ToString();
                if (dtEmail.Rows[0]["Address2"].ToString() != string.Empty)
                    txtaddress2.Text = dtEmail.Rows[0]["Address2"].ToString();
                if (dtEmail.Rows[0]["City"].ToString() != string.Empty)
                    txtcity.Text = dtEmail.Rows[0]["City"].ToString();

                hidState.Value = dtEmail.Rows[0]["State"].ToString();


                try
                {
                    if (dtEmail.Rows[0]["State"].ToString() != string.Empty)
                    {
                        drpstate.ClearSelection();
                        drpstate.Items.FindByValue(dtEmail.Rows[0]["State"].ToString()).Selected = true;
                    }
                }
                catch { }

                if (dtEmail.Rows[0]["Zip"].ToString() != string.Empty)
                    txtzip.Text = dtEmail.Rows[0]["Zip"].ToString();
                try
                {
                    if (dtEmail.Rows[0]["Country"].ToString() != string.Empty)
                        drpcountry.ClearSelection();
                    drpcountry.Items.FindByValue(dtEmail.Rows[0]["Country"].ToString()).Selected = true;
                }
                catch { }

                if (dtEmail.Rows[0]["Voice"].ToString() != string.Empty)
                    txtphone.Text = dtEmail.Rows[0]["Voice"].ToString();

                if (dtEmail.Rows[0]["Fax"].ToString() != string.Empty)
                    txtfax.Text = dtEmail.Rows[0]["Fax"].ToString();

                loadUDFValues(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));
                loadSubscribedGroups(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));

                txtemail.Enabled = false;
            }
           
            btnSubmit.Enabled = true;
        }

        private void getSmartFormDetails()
        {
            string smartFormSql = " SELECT g.customerID, sf.groupID, sf.SubscriptionGroupIDs FROM SmartFormsHistory sf join groups g on sf.groupID = g.groupID WHERE SmartFormID= " + ConfigurationManager.AppSettings["pharma_SFID"].ToString();

            DataTable dt = DataFunctions.GetDataTable(smartFormSql);
            DataRow dr = dt.Rows[0];

            SFGroupID = Convert.ToInt32(dr["GroupID"]);
            CustomerID = Convert.ToInt32(dr["CustomerID"]);

            if (dr.IsNull("SubscriptionGroupIDs"))
                SubscriptionGroupIDs = SFGroupID.ToString();
            else
                SubscriptionGroupIDs = dr["GroupID"].ToString() + (dr["SubscriptionGroupIDs"].ToString().Trim() == string.Empty ? "" : "," + dr["SubscriptionGroupIDs"].ToString().Trim());
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
                        hidBusiness.Value = dr["datavalue"].ToString();
                        try
                        {
                            user_Business.ClearSelection();
                            user_Business.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
                        }
                        catch { }
                    }
                    else if (dr["shortname"].ToString().ToLower() == "responsibility")
                    {
                        hidResponsibility.Value = dr["datavalue"].ToString();
                        user_Responsibility.ClearSelection();
                        user_Responsibility.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
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

            drpstate.Items.Add(item);

        }
    }
}
