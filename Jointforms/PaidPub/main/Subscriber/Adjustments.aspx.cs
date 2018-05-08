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
    public partial class Adjustments : System.Web.UI.Page
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

            DataTable dtsubscription = DataFunctions.GetDataTable("exec pharmalive_GetPaidSubscriberbyEmailAddress '" + txtEmailAddress.Text + "'");

            if (dtsubscription.Rows.Count > 0)
            {
                EmailID = Convert.ToInt32(dtsubscription.Rows[0]["EmailID"]);

                dlSubscribed.DataSource = dtsubscription;
                dlSubscribed.DataBind();

                pnlCurrentSubscriptions.Visible = true;
            }
            else
            {
                lblErrorMessage.Text = "Paid transaction not exists for this Email Address.";
                lblErrorMessage.Visible = true;
                return;
            }
        }

        public void btnAddAdjustment_Command(Object sender, CommandEventArgs e)
        {
            DataTable dtGroups = DataFunctions.GetDataTable("select top 1 g.groupID, g.groupname from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID  = gdf.groupdatafieldsID join groups g on g.groupID = gdf.groupID where emailID = " + EmailID + " and entryID = '" + e.CommandArgument.ToString()  + "'");

            if (dtGroups.Rows.Count > 0)
            {
                lblGroupID.Text = dtGroups.Rows[0]["groupID"].ToString();
                lblNewsletterName.Text = dtGroups.Rows[0]["groupname"].ToString();

                lblAdjEntryID.Text = e.CommandArgument.ToString();
                pnlAddAdjustment.Visible = true;
                drpAdjustmentType.ClearSelection();
                pnlDiscounts.Visible = false;
                pnlExpiration.Visible = false;
            }

        }

        public void dlSubscribed_ItemDataBound(Object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblAmountPaid = (Label)e.Item.FindControl("lblAmountPaid");
                Label lblTotalPaid = (Label)e.Item.FindControl("lblTotalPaid");

                GridView gva = (GridView)e.Item.FindControl("gvAdjustments");

                DataTable dtAdjustments = DataFunctions.GetDataTable("exec pharmalive_GetAdjustmentforTransaction " + EmailID + ",'" + dlSubscribed.DataKeys[e.Item.ItemIndex].ToString() + "'");

                gva.DataSource = dtAdjustments;
                gva.DataBind();

                double adjustments = 0;
                double totalpaid = Convert.ToDouble(lblAmountPaid.Text);

                foreach (DataRow dr in dtAdjustments.Rows)
                {
                       adjustments += (dr["AdjAmount"].ToString().Trim() == "" ? 0 : Convert.ToDouble(dr["AdjAmount"].ToString()));
                }

                lblTotalPaid.Text = String.Format("{0:C}", (totalpaid-adjustments));
            
            }
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataFunctions.Execute("exec pharmalive_SaveAdjustments " + EmailID + "," + lblGroupID.Text + ",'" + lblAdjEntryID.Text + "','" + drpAdjustmentType.SelectedItem.Value + "','" + txtAmount.Text + "','" + txtExpDate.Text + "','" + txtDesc.Text.Replace("'","''") + "'" );

                drpAdjustmentType.ClearSelection();

                lblAdjEntryID.Text = "";
                lblGroupID.Text = "";
                txtAmount.Text = "";
                txtExpDate.Text = "";
                txtDesc.Text = "";
                pnlAddAdjustment.Visible = false;
                btnSubmit_Click(sender, e);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddAdjustment.Visible = false;
            lblAdjEntryID.Text = "";
            lblGroupID.Text = "";
            txtAmount.Text = "";
            txtExpDate.Text = "";
            txtDesc.Text = "";
        }

        protected void drpAdjustmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpAdjustmentType.SelectedIndex > -1)
            {
                if (drpAdjustmentType.SelectedItem.Value.ToUpper() == "DISCOUNT")
                {
                    pnlDiscounts.Visible = true;
                    pnlExpiration.Visible = false;
                }
                else if (drpAdjustmentType.SelectedItem.Value.ToUpper() == "EXPIRATION")
                {
                    pnlDiscounts.Visible = false;
                    pnlExpiration.Visible = true;
                }
                else if (drpAdjustmentType.SelectedItem.Value.ToUpper() == "CANCEL")
                {
                    pnlDiscounts.Visible = false;
                    pnlExpiration.Visible = false;
                }
            }
            else
            {
                pnlDiscounts.Visible = false;
                pnlExpiration.Visible = false;
            }
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
