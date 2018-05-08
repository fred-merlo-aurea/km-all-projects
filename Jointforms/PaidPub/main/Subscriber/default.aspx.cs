using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

using ecn.common.classes;
using ecn.communicator.classes;

namespace PaidPub.main.Subscriber
{
    public partial class _default : System.Web.UI.Page
    {

        private int EmailID
        {
            get
            {
                if (ViewState["EmailID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["EmailID"]);
            }

            set { ViewState["EmailID"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateEmailAddress(txtEmailAddress.Text))
            {
                lblErrorMessage.Text = "Please enter a valid emailaddress.";
                lblErrorMessage.Visible = true;
                return;
            }
            pnlCurrentSubscriptions.Visible = false;
            EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("select distinct e.emailID from emails e join emailgroups eg on e.emailID = eg.emailID join groups g on eg.groupID = g.groupID where e.customerID = " + Session["CustomerID"].ToString() + " and g.customerID= " + Session["CustomerID"].ToString() + " and subscribetypecode = 'S' and e.emailaddress = '" + txtEmailAddress.Text.Replace("'", "''") + "'"));

            if (EmailID > 0)
            {

                DataTable dteNewsletters = GetSubscriberNewsletters(EmailID);

                gvSubscribed.DataSource = dteNewsletters;
                gvSubscribed.DataBind();

                pnlCurrentSubscriptions.Visible = true;
            }
            else
            {
                lblErrorMessage.Text = "Email Address not exists.";
                lblErrorMessage.Visible = true;
                return;
            }
        }

        private DataTable GetSubscriberNewsletters(int EmailID)
        {
            DataTable dteNewsletters = DataFunctions.GetDataTable("select g.groupID, g.groupname, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join groups g on g.groupID =gdf.groupID join emailgroups eg on eg.emailID = edv.emailID and eg.groupID = g.groupID where edv.emailID = " + EmailID + " and shortname = 'PaidOrFree' and subscribetypecode='S' and g.groupID in (select GroupID from ecn_misc..CANON_PAIDPUB_eNewsletters where CustomerID = " + Session["CustomerID"].ToString() + ")");
            return dteNewsletters;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            drpsubscriptiontype.ClearSelection();
            txtAmountPaid.Text = "";
            txtPromoCode.Text = "";
            txtPassword.Text = "";
            pnlTrialperiod.Visible = false;
            pnlStartDate.Visible = false;
            pnlAmountPaid.Visible = false;
            pnlPassword.Visible = false;


            gvSubscribed.SelectedIndex = -1;
            pnlChange.Visible = false;
        }

        protected void btnSubmitStatus_Click(object sender, EventArgs e)
        {
            // First, set their password
            if (txtPassword.Text.Trim() != "")
                DataFunctions.ExecuteScalar("update emails set password = '" + txtPassword.Text.Replace("'", "''") + "' where emailid = " + EmailID.ToString());
            // Get the email group and UDF objects

            Emails eml = new Emails(EmailID);
            Groups group = new Groups(lblgroupID.Text);
            Hashtable UDFFields = getGroupdatafields(lblgroupID.Text);
            Hashtable UDFData = new Hashtable();

            // Then update their status depending on how their original status was
            if (drpsubscriptiontype.SelectedValue == "COMP")
            {
                group.AttachUDFToEmail(eml, UDFFields["PaidOrFree"].ToString(), "COMP");
                group.AttachUDFToEmail(eml, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
                group.AttachUDFToEmail(eml, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
            }
            else if (drpsubscriptiontype.SelectedValue == "FREE")
            {
                // Do we need to remove the paid records on conversion back to free?
                
                group.AttachUDFToEmail(eml, UDFFields["PaidOrFree"].ToString(), "FREE");
                group.AttachUDFToEmail(eml, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
                group.AttachUDFToEmail(eml, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
            }
            else if (drpsubscriptiontype.SelectedValue == "PAID")
            {
                string startdate = DateTime.Now.ToString("MM/dd/yyyy");
                string enddate = DateTime.Now.AddDays(-1).AddYears(Convert.ToInt32(rbYears.SelectedItem.Value)).ToString("MM/dd/yyyy");

                //double earnedamount = Convert.ToDouble(txtAmountPaid.Text) / (12 * Convert.ToInt32(rbYears.SelectedItem.Value));
                //double Deferredamount = Math.Round(Convert.ToDouble(txtAmountPaid.Text) - earnedamount, 2);

                // We have a new paid customer, let simulate the transaction code in the signup form
                UDFData.Add(UDFFields["startdate"].ToString(), startdate);
                UDFData.Add(UDFFields["enddate"].ToString(), enddate);
                UDFData.Add(UDFFields["amountpaid"].ToString(), txtAmountPaid.Text);

                UDFData.Add(UDFFields["earnedamount"].ToString(), "0");
                UDFData.Add(UDFFields["Deferredamount"].ToString(), "0");

                // do they have sent data? we may not need to add a UDF for this
                UDFData.Add(UDFFields["TotalSent"].ToString(), "0");

                // What should the promo code be?
                UDFData.Add(UDFFields["PromoCode"].ToString(), txtPromoCode.Text);

                // Do we need to edit the subtype?
                UDFData.Add(UDFFields["SubType"].ToString(), "New");

                // Do these fields work for the report?
                UDFData.Add(UDFFields["TransactionID"].ToString(), "00000");
                UDFData.Add(UDFFields["PaymentMethod"].ToString(), drpPaymentMethod.SelectedValue);
                UDFData.Add(UDFFields["CardType"].ToString(), "Manual");
                UDFData.Add(UDFFields["CardNumber"].ToString(), "Manual");

                string GUID = System.Guid.NewGuid().ToString();

                IDictionaryEnumerator en = UDFData.GetEnumerator();

                while (en.MoveNext())
                {
                    group.AttachUDFToEmail(eml, en.Key.ToString(), en.Value.ToString(), GUID);
                }

                // UDF Questions not needed
                //group.AttachUDFToEmail(eml, UDFFields["Business"].ToString(), user_Business.SelectedItem.Value);
                //group.AttachUDFToEmail(eml, UDFFields["Responsibility"].ToString(), user_Responsibility.SelectedItem.Value);

                group.AttachUDFToEmail(eml, UDFFields["PaidOrFree"].ToString(), "PAID");
                group.AttachUDFToEmail(eml, UDFFields["Effort_Code"].ToString(), txtPromoCode.Text);
                group.AttachUDFToEmail(eml, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
                group.AttachUDFToEmail(eml, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
            } 
            else if (drpsubscriptiontype.SelectedValue == "TRIAL")
            {

                // Do we need to remove the paid record on conversion back to free?
                string startdate = DateTime.Now.ToString("MM/dd/yyyy");
                string enddate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");

                //Increment the trial counter.
                int noofTrials = 1;

                try
                {
                    noofTrials = Convert.ToInt32(DataFunctions.ExecuteScalar("select datavalue from emaildatavalues where emailID = " + eml.ID() + " and groupdatafieldsID = (select groupdatafieldsID  from groupdatafields where groupID = " + group.ID() + " and shortname = 'NoofTrials')"));
                    noofTrials = noofTrials + 1;
                }
                catch
                {
                    noofTrials = 1;
                }

                group.AttachUDFToEmail(eml, UDFFields["NoofTrials"].ToString(), noofTrials.ToString());

                group.AttachUDFToEmail(eml, UDFFields["PaidOrFree"].ToString(), "TRIAL");
                group.AttachUDFToEmail(eml, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
                group.AttachUDFToEmail(eml, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
                UDFData.Add(UDFFields["startdate"].ToString(), startdate);
                UDFData.Add(UDFFields["enddate"].ToString(), enddate);

                string GUID = System.Guid.NewGuid().ToString();

                IDictionaryEnumerator en = UDFData.GetEnumerator();

                while (en.MoveNext())
                {
                    group.AttachUDFToEmail(eml, en.Key.ToString(), en.Value.ToString(), GUID);
                }
            }
            pnlChange.Visible = false;
            DataTable dteNewsletters = GetSubscriberNewsletters(EmailID);

            gvSubscribed.DataSource = dteNewsletters;
            gvSubscribed.DataBind();

            drpsubscriptiontype.ClearSelection();
            txtAmountPaid.Text = "";
            txtPromoCode.Text = "";
            txtPassword.Text = "";
            pnlTrialperiod.Visible = false;
            pnlStartDate.Visible = false;
            pnlAmountPaid.Visible = false;
            pnlPassword.Visible = false;

        }
        

        protected void gvSubscribed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "select")
            {
                pnlChange.Visible = true;
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                lblgroupID.Text = gvSubscribed.DataKeys[rowIndex].Value.ToString();

                GridViewRow gvr = gvSubscribed.Rows[rowIndex];

                lbleNewsletter.Text = gvr.Cells[0].Text;

                string curSubscriptionType = gvr.Cells[1].Text;
                lblcsType.Text = curSubscriptionType;

                drpsubscriptiontype.Items.Clear();


                drpsubscriptiontype.Items.Insert(0, new ListItem("FREE", "FREE"));
                drpsubscriptiontype.Items.Insert(0, new ListItem("PAID", "PAID"));
                drpsubscriptiontype.Items.Insert(0, new ListItem("COMP", "COMP"));
                drpsubscriptiontype.Items.Insert(0, new ListItem("TRIAL", "TRIAL"));
                drpsubscriptiontype.Items.Insert(0, new ListItem("SELECT", "SELECT"));

                drpsubscriptiontype.Items.Remove(curSubscriptionType);
            }
        }

        // Display or don't display selected index fields
        protected void drpsubscriptionOnChanged(object sender, EventArgs e)
        {
            if (drpsubscriptiontype.Items.FindByText("SELECT") != null)
            {
                drpsubscriptiontype.Items.Remove("SELECT");
            }
            if (drpsubscriptiontype.SelectedValue == "FREE")
            {
                pnlTrialperiod.Visible = false;
                pnlStartDate.Visible = false;
                pnlAmountPaid.Visible = false;
                pnlPassword.Visible = false;
            }
            else if (drpsubscriptiontype.SelectedValue == "TRIAL")
            {
                pnlTrialperiod.Visible = true;
                pnlStartDate.Visible = false;
                pnlAmountPaid.Visible = false;
                pnlPassword.Visible = true;
            }
            else if (drpsubscriptiontype.SelectedValue == "COMP")
            {
                pnlTrialperiod.Visible = false;
                pnlStartDate.Visible = false;
                pnlAmountPaid.Visible = false;
                pnlPassword.Visible = true;
            }
            else if (drpsubscriptiontype.SelectedValue == "PAID")
            {
                pnlTrialperiod.Visible = false;
                pnlStartDate.Visible = true;
                pnlAmountPaid.Visible = true;
                pnlPassword.Visible = true;
            }
        }

        public Hashtable getGroupdatafields(string groupID)
        {
            Hashtable cUDF = new Hashtable();

            DataTable dtUDF = DataFunctions.GetDataTable("select groupdatafieldsID, shortname from groupdatafields where groupID=" + groupID);

            foreach (DataRow dr in dtUDF.Rows)
            {
                cUDF.Add(dr["shortname"].ToString(), dr["groupdatafieldsID"].ToString());
            }
            return cUDF;
        }

        private bool ValidateEmailAddress(string emailaddress)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (emailaddress != null) return Regex.IsMatch(emailaddress, MatchEmailPattern);
            else return false;

        }
    }
}

