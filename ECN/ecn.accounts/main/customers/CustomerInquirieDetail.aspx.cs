using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.common.classes;
using ecn.common.classes.license;
using ECN_Framework;
using System.Linq;

namespace ecn.accounts.main.customers
{
    public partial class CustomerInquirieDetail : WebPageHelper
    {


        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "customer Inquiries";
            Master.Heading = "Customer Inquiries";
            Master.HelpContent = "";
            Master.HelpTitle = "Customer Inquiries";

            if (!IsPostBack)
            {
                LoadChannelAndCustomer();
            }
        }

        protected ECN_Framework_Entities.Accounts.Customer CurrentCustomer
        {
            get { return (ECN_Framework_Entities.Accounts.Customer)Session["CurrentCustomer"]; }
            set { Session["CurrentCustomer"] = value; }
        }

        protected ArrayList CurrentCustomerInquiries
        {
            get
            {
                if (Session["CustomerInquiries"] == null)
                {
                    return new ArrayList();
                }
                return (ArrayList)Session["CustomerInquiries"];
            }
            set { Session["CustomerInquiries"] = value; }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            this.dltCustomerInquiries.ItemCreated += new System.Web.UI.WebControls.DataListItemEventHandler(this.dltCustomerInquiries_ItemCreated);
            this.dltCustomerInquiries.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dltCustomerInquiries_ItemCommand);

        }
        #endregion

        protected void ddlChannels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlCustomers.DataSource = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(ddlChannels.SelectedValue));
            ddlCustomers.DataTextField = "CustomerName";
            ddlCustomers.DataValueField = "CustomerID";
            ddlCustomers.DataBind();
            ResetCustomer();
            BindCustomerInquiries();
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ResetCustomer();
            BindCustomerInquiries();
        }

        private void ResetCustomer()
        { 
            if (ddlCustomers.SelectedValue.Trim().Length > 0) 
            {
                CurrentCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(Convert.ToInt32(ddlCustomers.SelectedValue),false);
                CurrentCustomerInquiries = CustomerInquirie.GetInquiriesByCustomerID(CurrentCustomer.CustomerID);
                lblErrorMessage.Visible = false;
            }
        }

        private void LoadChannelAndCustomer()
        {
            ddlChannels.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll().OrderBy(x=>x.BaseChannelName);
            ddlChannels.DataTextField = "BaseChannelName";
            ddlChannels.DataValueField = "BaseChannelID";
            ddlChannels.DataBind();

            if (ddlChannels.Items.Count > 0)
            {
                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    ddlChannels.Enabled = false;
                    ddlChannels.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
                }

                ddlChannels_SelectedIndexChanged(null, null);
            }
        }

        private void BindCustomerInquiries()
        {
            dltCustomerInquiries.DataSource = CurrentCustomerInquiries;
            dltCustomerInquiries.DataBind();
            btnAdd.Enabled = true;
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            ServiceLicense license = ServiceLicense.GetServiceLicenseByCustomerID(ServiceTypeEnum.ClientInquirie, CurrentCustomer.CustomerID);

            if (license == null)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "This customer doesn't have customer inquirie license. Check if this customer have annual tech quote.";
                return;
            }

            CustomerInquirie newInquirie = new CustomerInquirie(CurrentCustomer, "", "", DateTime.Now);
            newInquirie.InquirieLicense = license;

            CurrentCustomerInquiries.Add(newInquirie);
            dltCustomerInquiries.EditItemIndex = CurrentCustomerInquiries.Count - 1;
            btnAdd.Enabled = false;
            BindCustomerInquiries();
        }

        private void dltCustomerInquiries_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            CustomerInquirie inquirie = CurrentCustomerInquiries[e.Item.ItemIndex] as CustomerInquirie;
            switch (e.CommandName)
            {
                case "Edit":
                    dltCustomerInquiries.EditItemIndex = e.Item.ItemIndex;
                    BindCustomerInquiries();
                    break;
                case "Update":
                    dltCustomerInquiries.EditItemIndex = -1;
                    btnAdd.Enabled = true;
                    TextBox txtFirstName = GetTextBox(e.Item, "txtFirstName");
                    TextBox txtLastName = GetTextBox(e.Item, "txtLastName");
                    TextBox txtNotes = GetTextBox(e.Item, "txtNotes");
                    DropDownList ddl = e.Item.FindControl("ddlCustomerServiceRep") as DropDownList;
                    inquirie.FirstName = txtFirstName.Text;
                    inquirie.LastName = txtLastName.Text;
                    inquirie.Notes = txtNotes.Text;
                    inquirie.CustomerServiceStaff = new Staff(Convert.ToInt32(ddl.SelectedValue));
                    inquirie.Save();
                    BindCustomerInquiries();
                    break;
                case "Cancel":
                    dltCustomerInquiries.EditItemIndex = -1;
                    if (btnAdd.Enabled)
                    {
                        BindCustomerInquiries();
                        return;
                    }

                    CurrentCustomerInquiries.RemoveAt(CurrentCustomerInquiries.Count - 1);
                    btnAdd.Enabled = true;
                    BindCustomerInquiries();
                    break;
                case "Delete":
                    inquirie.Delete();
                    BindCustomerInquiries();
                    break;
            }
        }

        private void dltCustomerInquiries_ItemCreated(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
            {
                return;
            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox txtFirstName = GetTextBox(e.Item, "txtFirstName");
                TextBox txtLastName = GetTextBox(e.Item, "txtLastName");
                TextBox txtNotes = GetTextBox(e.Item, "txtNotes");

                DropDownList ddl = e.Item.FindControl("ddlCustomerServiceRep") as DropDownList;
                ddl.DataSource = Staff.GetStaffByRole(StaffRoleEnum.CustomerService);
                ddl.DataTextField = "FirstName";
                ddl.DataValueField = "ID";
                ddl.DataBind();

                CustomerInquirie inquirie = (CustomerInquirie)CurrentCustomerInquiries[e.Item.ItemIndex];
                txtFirstName.Text = inquirie.FirstName;
                txtLastName.Text = inquirie.LastName;
                txtNotes.Text = inquirie.Notes;

                return;
            }

            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?')");
        }

        private TextBox GetTextBox(DataListItem item, string name)
        {
            return (TextBox)item.FindControl(name);
        }


    }
}
