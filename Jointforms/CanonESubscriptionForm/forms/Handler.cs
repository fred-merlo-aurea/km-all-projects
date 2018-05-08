using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ecn.common.classes;
namespace CanonESubscriptionForm.forms
{
        public class Handler : System.Web.UI.Page
        {
            protected global::System.Web.UI.HtmlControls.HtmlForm frmsub1;
            protected global::System.Web.UI.WebControls.Panel pnlCurrent;
            protected global::System.Web.UI.WebControls.TextBox txtemail;
            protected global::System.Web.UI.WebControls.Button btnEmailClick;
            protected global::System.Web.UI.WebControls.Panel pnlProfile;
            protected global::System.Web.UI.WebControls.TextBox txtfirstname;
            protected global::System.Web.UI.WebControls.TextBox txtlastname;
            protected global::System.Web.UI.WebControls.TextBox txttitle;
            protected global::System.Web.UI.WebControls.TextBox txtcompany;
            protected global::System.Web.UI.WebControls.TextBox txtaddress;
            protected global::System.Web.UI.WebControls.TextBox txtaddress2;
            protected global::System.Web.UI.WebControls.TextBox txtcity;
            protected global::System.Web.UI.WebControls.DropDownList drpstate;
            protected global::System.Web.UI.WebControls.TextBox txtzip;
            protected global::System.Web.UI.WebControls.DropDownList drpcountry;
            protected global::System.Web.UI.WebControls.TextBox txtphone;
            protected global::System.Web.UI.WebControls.TextBox txtfax;
            protected global::System.Web.UI.WebControls.Panel pnlquestions;
            protected global::System.Web.UI.WebControls.Panel pnllogos;
            protected global::System.Web.UI.WebControls.Button btnSubmit;

            protected void Page_Load(object sender, EventArgs eargs)
            {
                //Response.Write(Request.QueryString.ToString());
                Response.Redirect("Editorialform.aspx?" + Request.QueryString.ToString());
                //btnSubmit.Enabled = false;
                //btnSubmit.Attributes.Add("OnClick", "javascript:return validateForm();");
                //btnEmailClick.Attributes.Add("onclick", "javascript:return validateEmail();");
                ////btnEmailClick.Attributes.Add("style", "visibility:hidden");
                //// string eventHandler = ClientScript.GetPostBackEventReference(this.btnEmailClick, "");
                //// this.txtemail.Attributes.Add("onblur", "if (validateEmail()) {__doPostBack('btnEmailClick','');}");

                /////if (!IsPostBack)
            }

            protected void btnEmailClick_Click(object sender, EventArgs eargs)
            {
                DataTable dtEmail = DataFunctions.GetDataTable("select e.EmailID,isnull(EmailAddress	,'') as EmailAddress,isnull(FirstName,'') as FirstName,isnull(LastName,'') as LastName,isnull(Title,'') as Title,isnull(Company,'') as Company,isnull(Address,'') as Address,isnull(Address2,'') as Address2,isnull(City,'') as City,isnull(State,'') as State,isnull(Zip,'') as Zip,isnull(Country,'') as Country,isnull(Voice,'') as Voice,isnull(Fax,'') as Fax from emails e join emailgroups eg on e.emailID = eg.emailID where e.emailaddress = '" + txtemail.Text.Replace("'", "''") + "' and customerID = " + ConfigurationManager.AppSettings["CustomerID"] + " and groupID = " + ConfigurationManager.AppSettings["GroupID"]);
                loadstate();
                if (dtEmail.Rows.Count > 0)
                {
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

                    
                }
                pnlCurrent.Visible = true;
                pnlProfile.Visible = true;
                pnlquestions.Visible = true;
                pnllogos.Visible = true;
                btnEmailClick.Visible = false;
                txtemail.Enabled = false;
                btnSubmit.Enabled = true;
            }

            private void loadUDFValues(int EmailID)
            {
                DataTable dtUDF = DataFunctions.GetDataTable("select distinct shortname, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + EmailID + " and groupID = " + ConfigurationManager.AppSettings["GroupID"]);

                foreach (DataRow dr in dtUDF.Rows)
                {
                    try
                    {
                        HtmlInputCheckBox hicb = (HtmlInputCheckBox)FindControl("user_" + dr["shortname"].ToString().ToLower());
                        if (dr["datavalue"].ToString().ToLower() == "y")
                            hicb.Checked = true;
                        else
                            hicb.Checked = false;
                    }
                    catch
                    {
                        //Response.Write(dr["shortname"].ToString().ToLower() + " - " + ex.Message  + "<BR>");
                    }
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
