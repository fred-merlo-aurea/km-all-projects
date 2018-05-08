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
    public partial class billDetail : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM;
            int billID = 0;

            try
            {
                billID = Convert.ToInt32(HttpRequestProcessor.Decrypt(Request["BillID"]));
            }
            catch (Exception)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Invalid Bill ID.";
                return;
            }

            if (billID == 0)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Invalid Bill ID.";
                return;
            }

            Bill bill = Bill.GetBillByID(billID);
            bill.AddItems(BillItem.GetBillItemsByBillID(billID));
            DisplayDateAndNumber(bill);
            DisplayBillAddress(bill);

            dgdBillItems.DataSource = bill.CurrentBillItems;
            dgdBillItems.DataBind();

            lblTotal.Text = string.Format("{0:c}", bill.Total);
        }

        private void DisplayDateAndNumber(Bill bill)
        {
            lblDate.Text = bill.CreatedDate.ToShortDateString();
            lblBillCode.Text = bill.Code;
        }

        private void DisplayBillAddress(Bill bill)
        {
            lblCustomerName.Text = bill.Quote.Customer.Name;
            lblAddress.Text = bill.Quote.Customer.BillingContact.StreetAddress;
            lblCityStateZip.Text = string.Format("{0} , {1} {2}", bill.Quote.Customer.BillingContact.City, bill.Quote.Customer.BillingContact.State, bill.Quote.Customer.BillingContact.Zip);
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

        }
        #endregion
    }
}
