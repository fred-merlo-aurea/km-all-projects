using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using ECN_Framework.Common;
using System.Collections;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ecn.communicator.admin
{
    public partial class mergeprofiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
            if (!IsPostBack)
            {
                List<ECN_Framework_Entities.Accounts.BaseChannel> baseChannelList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                drpBaseChannel.DataTextField = "BaseChannelName";
                drpBaseChannel.DataValueField = "BaseChannelID";
                var result = (from src in baseChannelList
                              orderby src.BaseChannelName
                              select src).ToList();
                drpBaseChannel.DataSource = result;
                drpBaseChannel.DataBind();

                toggleControls(false);
            }
        }
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void toggleControls(bool visible)
        {
            lblMergeAuto.Visible = visible;
            lblMergeManual.Visible = visible;
            lblMergeAll.Visible = visible;
            gvAllCustomer.Visible = visible;
            gvMergeCustomer.Visible = visible;
            gvReplaceCustomer.Visible = visible;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                phError.Visible = false;
                toggleControls(true);
                DataTable dtAllCustomers = new DataTable();
                dtAllCustomers.Columns.Add("Customer");
                dtAllCustomers.Columns.Add("CustomerID");
                dtAllCustomers.AcceptChanges();

                DataTable dtReplacedProfiles_Customers = new DataTable();
                dtReplacedProfiles_Customers.Columns.Add("Customer");
                dtReplacedProfiles_Customers.Columns.Add("CustomerID");
                dtReplacedProfiles_Customers.AcceptChanges();

                DataTable dtMergeProfiles_Customers = new DataTable();
                dtMergeProfiles_Customers.Columns.Add("Customer");
                dtMergeProfiles_Customers.Columns.Add("CustomerID");
                dtMergeProfiles_Customers.Columns.Add("OldEmailID");
                dtMergeProfiles_Customers.Columns.Add("NewEmailID");
                dtMergeProfiles_Customers.AcceptChanges();

                List<ECN_Framework_Entities.Accounts.Customer> customerList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(drpBaseChannel.SelectedValue));
                foreach (ECN_Framework_Entities.Accounts.Customer cust in customerList)
                {
                    bool fromExists = ECN_Framework_BusinessLayer.Communicator.Email.Exists(txtFromEmailAddress.Text.Trim(), cust.CustomerID);
                    bool toExists = ECN_Framework_BusinessLayer.Communicator.Email.Exists(txtToEmailAddress.Text.Trim(), cust.CustomerID);
                    if (toExists == true && fromExists == true)
                    {
                        ECN_Framework_Entities.Communicator.Email oldEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(txtFromEmailAddress.Text.Trim(), cust.CustomerID);
                        ECN_Framework_Entities.Communicator.Email newEmail = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(txtToEmailAddress.Text.Trim(), cust.CustomerID, oldEmail.EmailID, Master.UserSession.CurrentUser);
                        DataRow dr = dtMergeProfiles_Customers.NewRow();
                        dr["Customer"] = cust.CustomerName;
                        dr["CustomerID"] = cust.CustomerID;
                        dr["OldEmailID"] = oldEmail.EmailID;
                        dr["NewEmailID"] = newEmail.EmailID;
                        dtMergeProfiles_Customers.Rows.Add(dr);
                    }
                    else if (toExists == false && fromExists == true)
                    {
                        ECN_Framework_Entities.Communicator.Email oldEmailID = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(txtFromEmailAddress.Text.Trim(), cust.CustomerID);
                        oldEmailID.EmailAddress = txtToEmailAddress.Text;
                        ECN_Framework_BusinessLayer.Communicator.Email.Save(oldEmailID);
                        DataRow dr = dtReplacedProfiles_Customers.NewRow();
                        dr["Customer"] = cust.CustomerName;
                        dr["CustomerID"] = cust.CustomerID;
                        dtReplacedProfiles_Customers.Rows.Add(dr);
                    }
                    DataRow drAllCustomers = dtAllCustomers.NewRow();
                    drAllCustomers["Customer"] = cust.CustomerName;
                    drAllCustomers["CustomerID"] = cust.CustomerID;
                    dtAllCustomers.Rows.Add(drAllCustomers);
                }
                gvAllCustomer.DataSource = dtAllCustomers;
                gvAllCustomer.DataBind();
                gvMergeCustomer.DataSource = dtMergeProfiles_Customers;
                gvMergeCustomer.DataBind();
                gvReplaceCustomer.DataSource = dtReplacedProfiles_Customers;
                gvReplaceCustomer.DataBind();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
            catch (Exception exc)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(exc, "ecn.communicator.admin.MergeProfiles.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/error.aspx", true);
            }
        }

        protected void gvMergeCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int CustomerID = Convert.ToInt32(e.CommandArgument.ToString());
            string redirectURL = "/ecn.communicator/main/lists/mergeProfiles.aspx?";
            foreach (GridViewRow gvr in gvMergeCustomer.Rows)
            {
                if (Convert.ToInt32(((Label)gvr.FindControl("lblCustomerID")).Text) == CustomerID)
                {
                    int customerID = Convert.ToInt32(((Label)gvr.FindControl("lblCustomerID")).Text);
                    int oldEmailID = Convert.ToInt32(((Label)gvr.FindControl("lblOldEmailID")).Text);
                    int newEmailID = Convert.ToInt32(((Label)gvr.FindControl("lblNewEmailID")).Text);
                    redirectURL = redirectURL + "oldemailid=" + oldEmailID.ToString() + "&newemailid=" + newEmailID.ToString() + "&customerID=" + customerID.ToString();
                    break;
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + redirectURL + "','_blank')", true);
        }
    }
}