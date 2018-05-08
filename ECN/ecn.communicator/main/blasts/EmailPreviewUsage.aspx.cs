using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace ecn.communicator.main.blastsmanager
{
    public partial class EmailPreviewUsage : ECN_Framework.WebPageHelper
    {   
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS; 
                Master.SubMenu="";
                Master.Heading = "Email Preview Usage";
                Master.HelpContent = "";
                Master.HelpTitle = "Email Preview Usage";	

                ViewState["SortField"] = "CustomerName";
                ViewState["SortDirection"] = "ASC";

                ViewState["UsageDetailsSortField"] = "BlastID";
                ViewState["UsageDetailsSortDirection"] = "ASC";

                List<ECN_Framework_Entities.Accounts.Customer> customerList = new List<ECN_Framework_Entities.Accounts.Customer>();
               
                if (KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    customerList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);
                    drpCustomer.DataSource = customerList;
                    drpCustomer.DataTextField = "CustomerName";
                    drpCustomer.DataValueField = "CustomerID";
                    drpCustomer.DataBind();
                    drpCustomer.Items.Insert(0, new ListItem("----- All Customer -----", "0"));
                }
                else if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreviewUsageReport, KMPlatform.Enums.Access.View))
                {
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser, false);
                    customerList.Add(customer);
                    drpCustomer.Enabled = false;
                    drpCustomer.DataSource = customerList;
                    drpCustomer.DataTextField = "CustomerName";
                    drpCustomer.DataValueField = "CustomerID";
                    drpCustomer.DataBind();
                    

                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
 


                
                
                drpMonth.Items.FindByValue(DateTime.Today.Month.ToString()).Selected = true;

                int currentYear = DateTime.Today.Year;
                drpYear.Items.Insert(0, new ListItem(currentYear.ToString(), currentYear.ToString()));
                drpYear.Items.Insert(0, new ListItem((currentYear - 1).ToString(), (currentYear - 1).ToString()));
                drpYear.Items.Insert(0, new ListItem((currentYear - 2).ToString(), (currentYear - 2).ToString()));
                drpYear.Items.FindByValue(DateTime.Today.Year.ToString()).Selected = true;

                loadGrid();
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            loadGrid();
        }

        protected void grdEmailPreviewUsage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdEmailPreviewUsage.PageIndex = e.NewPageIndex;
            }
            loadGrid();
        }

        private void loadGrid()
        {
            pnlUsageDetails.Visible = false;

            DataTable dt = new DataTable();

            if (Convert.ToInt32(drpCustomer.SelectedItem.Value) == 0)
            {
                dt = ECN_Framework_BusinessLayer.Communicator.EmailPreview.GetUsageByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Convert.ToInt32(drpMonth.SelectedItem.Value), Convert.ToInt32(drpYear.SelectedItem.Value));
            }
            else
            {
                dt = ECN_Framework_BusinessLayer.Communicator.EmailPreview.GetUsage(Convert.ToInt32(drpCustomer.SelectedItem.Value), Convert.ToInt32(drpMonth.SelectedItem.Value), Convert.ToInt32(drpYear.SelectedItem.Value));
            }

            //foreach(DataRow dr in dt.Rows)
            //{
            //    dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
            //}

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            grdEmailPreviewUsage.DataSource = dv;

            try
            {
                grdEmailPreviewUsage.DataBind();
            }
            catch
            {
                grdEmailPreviewUsage.PageIndex = 0;
                grdEmailPreviewUsage.DataBind();
            }


        }

        protected void ActiveGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "DESC";
            }
            loadGrid(); 
        }

        protected void grdEmailPreviewUsage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                pnlUsageDetails.Visible = true;
                hfCustomerID.Value = e.CommandArgument.ToString();
                loadGridUsageDetails();
            }
        }

        private void loadGridUsageDetails()
        {
            DataTable dt = new DataTable();

            dt = ECN_Framework_BusinessLayer.Communicator.EmailPreview.GetUsageDetails(Convert.ToInt32(hfCustomerID.Value), Convert.ToInt32(drpMonth.SelectedItem.Value), Convert.ToInt32(drpYear.SelectedItem.Value));

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            grdUsageDetails.DataSource = dv;

            try
            {
                grdUsageDetails.DataBind();
            }
            catch
            {
                grdUsageDetails.PageIndex = 0;
                grdUsageDetails.DataBind();
            }

            int PreviewUsageCanBeUsed = 100;
            int PreviewUsageUsed = dt.Rows.Count;
            lblPreviewUsage.Text = PreviewUsageUsed.ToString();
            lblPreviewUsageCanBeUsed.Text = PreviewUsageCanBeUsed.ToString();
            lblPreviewUsageAvailable.Text = (PreviewUsageCanBeUsed - PreviewUsageUsed).ToString();
            capacityBar.Style.Add("WIDTH", PreviewUsageUsed + "%");
            capacityBarArrow.Style.Add("WIDTH", PreviewUsageUsed + "%");
        }

        protected void grdUsageDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdUsageDetails.PageIndex = e.NewPageIndex;
            }
            loadGridUsageDetails();
        }

        protected void grdUsageDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["UsageDetailsSortField"].ToString())
            {
                switch (ViewState["UsageDetailsSortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["UsageDetailsSortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["UsageDetailsSortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["UsageDetailsSortField"] = e.SortExpression;
                ViewState["UsageDetailsSortDirection"] = "DESC";
            }
            loadGridUsageDetails();
        }
    }
}