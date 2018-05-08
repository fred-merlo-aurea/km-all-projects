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
using ecn.common.classes.billing;

namespace ecn.accounts.main.billingSystem
{
    public partial class BillingHistory : ECN_Framework.WebPageHelper
    {
        protected System.Web.UI.WebControls.DataList dltBills;

        protected ArrayList CustomersWithBills
        {
            get { return (ArrayList)Session[CustomersSessionKey]; }
            set { Session[CustomersSessionKey] = value; }
        }

        private string CustomersSessionKey
        {
            get { return string.Format("Customers_Session_key_{0}", this.ID); }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM; 

            if (!IsPostBack)
            {
                LoadChannelAndCustomer();
                startDate.Date = DateTime.Now.AddDays(-30);
                endDate.Date = DateTime.Now;
            }
        }

        #region Data Loading methods
        private void LoadChannelAndCustomer()
        {
            ddlChannels.DataSource = BaseChannel.GetBaseChannels();
            ddlChannels.DataTextField = "Name";
            ddlChannels.DataValueField = "ID";
            ddlChannels.DataBind();

            if (ddlChannels.Items.Count > 0)
            {
                ddlChannels.SelectedIndex = 0;
                ddlChannels_SelectedIndexChanged(null, null);
            }
        }

        #endregion

        #region Event Handler

        protected void ddlChannels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlCustomers.DataSource = Customer.GetAllCustomersByChannelID(Convert.ToInt32(ddlChannels.SelectedValue));
            ddlCustomers.DataTextField = "Name";
            ddlCustomers.DataValueField = "ID";
            ddlCustomers.DataBind();
        }

        protected void btnView_Click(object sender, System.EventArgs e)
        {   
            ArrayList allCustomers = new ArrayList();
            if (chkShowAll.Checked)
            {
                allCustomers = Customer.GetAllCustomersByChannelID(Convert.ToInt32(ddlChannels.SelectedValue));
            }
            else if (ddlCustomers.SelectedValue.Trim().Length > 0)
            {
                allCustomers.Add(Customer.GetCustomerByID(Convert.ToInt32(ddlCustomers.SelectedValue)));
            }

            ArrayList customers = new ArrayList();
            foreach (Customer customer in allCustomers)
            {
                BillCollection bills = LoadBillsByCustomerID(customer.ID);
                if (bills.Count > 0)
                {
                    customer.Bills.AddRange(bills);
                    customers.Add(customer);
                }
            }

            CustomersWithBills = customers;
            dltCustomers.DataSource = CustomersWithBills;
            dltCustomers.DataBind();
        }   

        protected void dltBills_OnItemCommand(object sender, DataListCommandEventArgs e)
        {
            HttpRequestProcessor processor = new HttpRequestProcessor("QuoteDetail.aspx");

            switch (e.CommandName)
            {
                case "ViewQuote":
                    string[] IDs = e.CommandArgument.ToString().Split(',');
                    if (IDs.Length != 2)
                    {
                        throw new ApplicationException("Can't find quote ID and customer ID to view quote.");
                    }

                    //HttpRequestProcessor processor = new HttpRequestProcessor("QuoteDetail.aspx");
                    //processor.Add("CustomerID", Convert.ToInt32(IDs[0]));
                    //processor.Add("QuoteID", Convert.ToInt32(IDs[1]));

                    //Server.Transfer(processor.EncryptedHttpRequest);

                    Response.Redirect(string.Format("QuoteDetail.aspx?CustomerID={0}&QuoteID={1}", Convert.ToInt32(IDs[0]), Convert.ToInt32(IDs[1])));

                    break;
                case "ShowBill":
                    int billID = Convert.ToInt32(e.CommandArgument);
                    processor = new HttpRequestProcessor("billDetail.aspx");
                    processor.Add("BillID", billID);

                    Server.Transfer(processor.EncryptedHttpRequest);
                    break;
            }
        }

        protected BillCollection GetBillsByCustomerID(int customerID)
        {
            foreach (Customer c in CustomersWithBills)
            {
                if (c.ID == customerID)
                {
                    return c.Bills;
                }
            }
            throw new ApplicationException("There is no customer with ID " + customerID.ToString());
        }

        private BillCollection LoadBillsByCustomerID(int customerID)
        {
            BillCollection allBills = Bill.GetBillsByCustomerID(customerID, startDate.Date, endDate.Date, ddlBillType.SelectedValue);
            foreach (Bill bill in allBills)
            {
                bill.AddItems(BillItem.GetBillItemsByBillID(bill.ID));
            }
            return allBills;
        }
        #endregion

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

        }
        #endregion
    }
}
