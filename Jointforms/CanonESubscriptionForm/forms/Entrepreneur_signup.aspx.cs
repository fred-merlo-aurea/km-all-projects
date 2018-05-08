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
    public partial class Entrepreneur_signup : System.Web.UI.Page
    {
        int SFGroupID = 0;
        int CustomerID = 0;
        string SubscriptionGroupIDs = string.Empty;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.Attributes.Add("OnClick", "javascript:return validateForm();");

            if (!IsPostBack)
            {
                if (EmailAddress != string.Empty)
                {
                    getSmartFormDetails();
                    loadform();
                }
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

                if (dtEmail.Rows[0]["Zip"].ToString() != string.Empty)
                    txtzip.Text = dtEmail.Rows[0]["Zip"].ToString();
               
                loadSubscribedGroups(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));

                txtemail.Enabled = false;
            }

            btnSubmit.Enabled = true;
        }

        private void getSmartFormDetails()
        {
            string smartFormSql = " SELECT g.customerID, sf.groupID, sf.SubscriptionGroupIDs FROM SmartFormsHistory sf join groups g on sf.groupID = g.groupID WHERE SmartFormID= " + ConfigurationManager.AppSettings["Entrepreneur_SFID"].ToString();

            DataTable dt = DataFunctions.GetDataTable(smartFormSql);
            DataRow dr = dt.Rows[0];

            SFGroupID = Convert.ToInt32(dr["GroupID"]);
            CustomerID = Convert.ToInt32(dr["CustomerID"]);

            if (dr.IsNull("SubscriptionGroupIDs"))
                SubscriptionGroupIDs = SFGroupID.ToString();
            else
            {
                SubscriptionGroupIDs = dr["GroupID"].ToString() + (dr["SubscriptionGroupIDs"].ToString().Trim() == string.Empty ? "" : "," + dr["SubscriptionGroupIDs"].ToString().Trim());
                //SubscriptionGroupIDs = (dr["SubscriptionGroupIDs"].ToString().Trim() == string.Empty ? "" : "," + dr["SubscriptionGroupIDs"].ToString().Trim());
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
    }
}
