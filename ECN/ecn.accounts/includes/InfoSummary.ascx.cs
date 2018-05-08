namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using System.Collections;
	using ecn.common.classes;
	using ecn.common.classes.billing;
    using ECN_Framework_BusinessLayer.Application;
	
	///		Summary description for InfoSummary.
	
	public partial class InfoSummary : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dltCustomers;

		private string CustomersSessionKey {
			get { return string.Format("Customers_Session_key_ForProcessInvoice_{0}", this.ID);}
		}

		protected ArrayList CustomersWithInvoice {
			get { return (ArrayList) Session[CustomersSessionKey];}
			set { Session[CustomersSessionKey] = value;}
		}		

		private BillCollection LoadUnPaidBillsByCustomerID(int customerID) {
			BillCollection allBills = Bill.GetBillsByCustomerID(customerID);
			foreach(Bill bill in allBills) {						
				bill.AddItems(BillItem.GetBillItemsByBillID(bill.ID));
			}
			BillCollection unpaidBills = new BillCollection();
			foreach(Bill bill in allBills) {
				if (bill.Status== BillStatusEnum.NotPaid || bill.Status == BillStatusEnum.PartiallyPaid) {
					unpaidBills.Add(bill);
				}
			}				
			return unpaidBills;
		}	

		private void CustomersDataBind() {
			dltCustomers.DataSource = CustomersWithInvoice;
			dltCustomers.DataBind();		
		}		

		private void ShowInvoice() {
			ArrayList customers = new ArrayList();
			BillCollection bills = LoadUnPaidBillsByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            Customer customer = Customer.GetCustomerByID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
			
			if (bills.Count > 0) {
				customer.Bills.AddRange(bills);
				customers.Add(customer);
			}
			
			CustomersWithInvoice = customers;		
			CustomersDataBind();
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//ShowInvoice();
		}

		public string Summary {
			set {
				ltlInfoSummary.Text = value;
			}
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
