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
    public partial class ProcessInvoice : ECN_Framework.WebPageHelper
	{
		protected System.Web.UI.WebControls.DataList dltBills;

		protected ArrayList CustomersWithInvoice {
			get { return (ArrayList) Session[CustomersSessionKey];}
			set { Session[CustomersSessionKey] = value;}
		}		

		private string CustomersSessionKey {
			get { return string.Format("Customers_Session_key_ForProcessInvoice_{0}", this.ID);}
		}
		

		#region Event Handler
		private int CurrentEditItemIndex {
			get {
				if ( Session[CurrentEditItemIndexSessionKey] == null) {
					 Session[CurrentEditItemIndexSessionKey] = -1;
				}
				return (int) Session[CurrentEditItemIndexSessionKey];}
			set { Session[CurrentEditItemIndexSessionKey] = value;}
		}

		private int CurrentEditBillID {
			get { 
				if (Session[CurrentEditBillIDSessionKey] == null) {
					Session[CurrentEditBillIDSessionKey] = -1;
				}
				return (int) Session[CurrentEditBillIDSessionKey];}
			set { Session[CurrentEditBillIDSessionKey] = value ;}
		}

		private string CurrentEditItemIndexSessionKey {
			get { return string.Format("CurrentEditItemIndexForProcessInvoice_{0}", ID);}
		}

		private string CurrentEditBillIDSessionKey {
			get { return string.Format("CurrentEditBillIDForProcessInvoice_{0}", ID);}
		}

		protected void Page_Load(object sender, System.EventArgs e) { 
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM; 

			if (!IsPostBack) {
				CustomersWithInvoice = null;
				ddlBaseChannels.DataSource = BaseChannel.GetBaseChannels();
				ddlBaseChannels.DataValueField = "ID";
				ddlBaseChannels.DataTextField = "Name";
				ddlBaseChannels.DataBind();

				CurrentEditBillID = -1;
				CurrentEditItemIndex = -1;
			}

			CustomersDataBind();
		}
		protected void btnShowInvoice_Click(object sender, System.EventArgs e) {
			ArrayList allCustomers = Customer.GetAllCustomersByChannelID(Convert.ToInt32(ddlBaseChannels.SelectedValue));
			ArrayList customers = new ArrayList();
			foreach(Customer customer in allCustomers) {
				BillCollection bills = LoadUnPaidBillsByCustomerID(customer.ID);				
				if (bills.Count > 0) {
					customer.Bills.AddRange(bills);
					customers.Add(customer);
				}
			}
			CustomersWithInvoice = customers;		
			CustomersDataBind();
		}				

		protected void dltBills_OnItemCreated(object sender, DataListItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer) {
				return;
			}

			DataGrid dgdBillItems = e.Item.FindControl("dgdBillItems") as DataGrid;
			Bill bill = e.Item.DataItem as Bill;
			
			if (bill == null) {
				return;
			}

			if (bill.ID == CurrentEditBillID) {			
				dgdBillItems.EditItemIndex = CurrentEditItemIndex;
			}
		}

		protected void dltBills_OnItemCommand(object sender, DataListCommandEventArgs e) {
            HttpRequestProcessor processor = new HttpRequestProcessor("QuoteDetail.aspx");
			switch(e.CommandName) {
				case "ViewQuote":					
					string[] IDs = e.CommandArgument.ToString().Split(',');
					if (IDs.Length != 2) {
						throw new ApplicationException("Can't find quote ID and customer ID to view quote.");
					}

                    //
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

		protected void dgdBillItems_OnItemCommand(object sender, DataGridCommandEventArgs e) {
			DataGrid dgdBillItems = sender as DataGrid;
			int billItemID = Convert.ToInt32(e.CommandArgument);
			BillItem item = GetBillItemByID(billItemID);
			Bill bill = GetBillByItemID(billItemID);
			switch (e.CommandName) {
				case "Edit":
					CurrentEditItemIndex = e.Item.ItemIndex;
					CurrentEditBillID = bill.ID;
					CustomersDataBind();
					break;
				case "Update":
					DropDownList ddl = e.Item.FindControl("ddlEditStatus") as DropDownList;
					
					item.Status = (BillItemStatusEnum) Convert.ToInt32(ddl.SelectedValue);
					item.Save();

					CurrentEditItemIndex = -1;
					CurrentEditBillID = -1;
					CustomersDataBind();
					break;
				case "Cancel":
					CurrentEditItemIndex = -1;
					CurrentEditBillID = -1;
					CustomersDataBind();				
					break;				
			}
		}
		
		protected void dgdBillItems_OnItemCreated(object sender, DataGridItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.EditItem) {
				DropDownList ddl = e.Item.FindControl("ddlEditStatus") as DropDownList;				 
				BillItem item = e.Item.DataItem as BillItem;
				ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(Convert.ToString( (int)  item.Status)));
			}
		}
		private void CustomersDataBind() {
			dltCustomers.DataSource = CustomersWithInvoice;
			dltCustomers.DataBind();		
		}		
		#endregion

		#region Private & Protected Methods
		protected BillCollection GetUnPaidBillsByCustomerID(int customerID) {
			foreach(Customer c in CustomersWithInvoice) {
				if (c.ID == customerID) {
					return c.Bills;
				}
			}
			throw new ApplicationException("There is no customer with ID " + customerID.ToString());
		}

		protected BillItemCollection GetBillItemsByBillID(int billID) {
			foreach(Customer c in CustomersWithInvoice) {
				foreach(Bill b in c.Bills) {					
					if (b.ID == billID) {
						return b.CurrentBillItems;
					}					
				}
			}
			return null;
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

		private BillItem GetBillItemByID(int billItemID) {
			foreach(Customer c in CustomersWithInvoice) {
				foreach(Bill b in c.Bills) {
					foreach(BillItem item in b.CurrentBillItems) {
						if (item.ID == billItemID) {
							return item;
						}
					}
				}
			}
			throw new ApplicationException("There is no bill item with ID " + billItemID.ToString());
		}

		private Bill GetBillByItemID(int billItemID) {
			foreach(Customer c in CustomersWithInvoice) {
				foreach(Bill b in c.Bills) {
					foreach(BillItem item in b.CurrentBillItems) {
						if (item.ID == billItemID) {
							return b;
						}
					}
				}
			}
			throw new ApplicationException("There is no bill with ID " + billItemID.ToString());
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion
	}
}
