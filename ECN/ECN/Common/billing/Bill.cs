using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;

namespace ecn.common.classes.billing
{	
	public enum BillingGenerationSource { BillingEngine, Administrators }
	public enum BillStatusEnum { NotPaid, PartiallyPaid, Paid, Canceled}
	public class Bill
	{
		public Bill(Customer customer) {
			_customer = customer;
			_billItems = new BillItemCollection();
		}

		#region Properties

		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private Customer _customer;
		public Customer Customer {
			get {
				return (this._customer);
			}
			set {
				this._customer = value;
			}
		}		

		private Quote _quote;
		public Quote Quote {
			get {
				return (this._quote);
			}
			set {
				this._quote = value;
			}
		}

		public string Code {
			get { return string.Format("B{0}_{1}_{2}{3}{4}_{5}",Customer.BaseChannelID.ToString("0000"), Customer.ID.ToString("000000"),CreatedDate.Month.ToString("00"), CreatedDate.Day.ToString("00"), CreatedDate.Year, ID==-1?"New":ID.ToString());}
		}

        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get
            {
                return (this._createdDate);
            }
            set
            {
                this._createdDate = value;
            }
        }

		private BillItemCollection _billItems;
		public BillItemCollection CurrentBillItems {
			get {
				return (this._billItems);
			}
		}		

		private BillingGenerationSource _source = BillingGenerationSource.Administrators;
		public BillingGenerationSource Source {
			get {
				return (this._source);
			}
			set {
				this._source = value;
			}
		}

		private bool _isSyncToQb;
		public bool IsSyncToQb {
			get {
				return (this._isSyncToQb);
			}
			set {
				this._isSyncToQb = value;
			}
		}


		public double Total {
			get {
				double ret = 0;
				foreach(BillItem item in this.CurrentBillItems) {
					if (item.QuoteItem.IsCustomerCredit) {
						continue;
					}
					ret += item.Total;
				}
				return ret;
			}
		}

		public BillStatusEnum Status {
			get { 
				
				if (CanceledItemCount == CurrentBillItems.Count) {
					return BillStatusEnum.Canceled;
				}

				int unPaidItemCount = 0;
				foreach(BillItem item in CurrentBillItems) {
					if (item.Status == BillItemStatusEnum.Pending) {
						unPaidItemCount ++;
					}				
				}			

				if (unPaidItemCount == 0) {
					return BillStatusEnum.Paid;
				}

				return unPaidItemCount == CurrentBillItems.Count?BillStatusEnum.NotPaid:BillStatusEnum.PartiallyPaid;
			}
		}

		public int CanceledItemCount {
			get {
				int canceledItemCount = 0;
				foreach(BillItem item in CurrentBillItems) {
					if (item.Status == BillItemStatusEnum.Canceled) {
						canceledItemCount ++;
					}
				}
				return canceledItemCount;
			}
		}

		public int CustomerID {
			get { return Customer.ID;}
		}

		public int QuoteID {
			get { return Quote.QuoteID;}
		}

		#endregion

		public void AddItem(BillItem item) {
			if (item == null) {
				return;
			}
			_billItems.Add(item);
			if (item.ID <= 0) {
				item.ChangedDate = this.CreatedDate;
			}
			item.Parent = this;
		}

		public void AddItems(BillItemCollection items) {
			foreach(BillItem item in items) {
				AddItem(item);
			}
		}


		#region Database Methods
		// No update for bill table.
		public void Save(int userID) {
			if (this.CurrentBillItems.Count == 0) {
				return;
			}
			SqlConnection connection = new SqlConnection(DataFunctions.GetConnectionString());
            SqlCommand command = new SqlCommand("INSERT INTO [Bills] (CustomerID, QuoteID, CreatedDate, Source, CreatedUserID, UpdatedDate, IsDeleted) VALUES (@customerID, @quoteID, @createdDate, @source, @userid, getdate(), 0);SELECT @@IDENTITY;", connection);
			command.Parameters.Add("@customerID", SqlDbType.Int,4).Value = this.Customer.ID;
			command.Parameters.Add("@quoteID", SqlDbType.Int,4).Value = this.Quote.QuoteID;
            command.Parameters.Add("@createdDate", SqlDbType.DateTime, 8).Value = this.CreatedDate;
			command.Parameters.Add("@source", SqlDbType.TinyInt, 1).Value = this.Source;
            command.Parameters.Add("@userid", SqlDbType.Int, 4).Value = userID;

			try {
				connection.Open();				

				if (this.ID <= 0) {
					command.Prepare();
					this.ID = Convert.ToInt32( command.ExecuteScalar() );
				}
				
				foreach(BillItem item in this.CurrentBillItems) {   
					item.Save(connection);	
				}					
			} finally {
				connection.Close();
				connection.Dispose();
				command.Dispose();
			}
		}

		public void SetQbSyncFlag(bool isSync, int userID) {
			SqlConnection connection = new SqlConnection(DataFunctions.GetConnectionString());
			SqlCommand command = new SqlCommand("UPDATE [Bills] Set IsSyncToQb = @isSyncToQb, UpdatedUserID=@UserID, UpdatedDate=GetDate() where BillID = @billID",connection);
			command.Parameters.Add("@isSyncToQb", SqlDbType.Bit,1).Value = isSync;
			command.Parameters.Add("@billID", SqlDbType.Int,4).Value = this.ID;
            command.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;	

			try {
				connection.Open();						
				command.Prepare();				
				command.ExecuteNonQuery();				
			} finally {
				connection.Close();
				connection.Dispose();
				command.Dispose();
			}
		}
		#endregion

		#region Static Database Methods
		
		public static BillCollection GetBillsByCustomerID(int customerID) {
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [Bills] where CustomerID = " + customerID.ToString() + " and IsDeleted = 0");
			BillCollection bills = new BillCollection();

			Customer parent = new Customer(customerID);
			foreach(DataRow row in dt.Rows) {				
				bills.Add(GetBill(row, parent));
			}
			return bills;
		}

		public static BillCollection GetBillsByCustomerID(int customerID, DateTime start, DateTime end, string billType) {
            string sql = string.Format("SELECT b.* from [Bills] b join [Quote] q on b.QuoteID = q.QuoteID and q.IsDeleted = 0 where b.IsDeleted = 0 and b.CustomerID = {0} AND (b.CreatedDate >= '{1}' or b.UpdatedDate >= '{1}') AND (b.CreatedDate <= '{2}' or b.UpdatedDate <= '{2}') ", customerID, start, end);
			
			if (billType != "Both") {
				sql = sql + " AND q.BillType = '" + billType + "'";
			}

			DataTable dt = DataFunctions.GetDataTable(sql);
			BillCollection bills = new BillCollection();

			Customer parent = new Customer(customerID);
			foreach(DataRow row in dt.Rows) {				
				bills.Add(GetBill(row, parent));
			}
			return bills;
		}


		public static BillCollection GetBillsByDateRange(DateTime start, DateTime end) {
            string sql = string.Format("SELECT b.* from [Bills] b join [Quote] q on b.QuoteID = q.QuoteID and q.IsDeleted = 0 where b.IsDeleted = 0 and (b.CreatedDate >= '{0}' or b.UpdatedDate >= '{0}') AND (b.CreatedDate <= '{1}' or b.UpdatedDate <= '{1}') ", start, end);
			
			DataTable dt = DataFunctions.GetDataTable(sql);
			BillCollection bills = new BillCollection();
			 
			foreach(DataRow row in dt.Rows) {	
				Customer parent = new Customer(Convert.ToInt32(row["CustomerID"]));
				bills.Add(GetBill(row, parent));
			}
			return bills;
		}

		public static Bill GetBillByID(int billID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from [Bills] where BillID = " + billID.ToString() + " and IsDeleted = 0 ");
			if (dt.Rows.Count == 0) {
				return null;
			}

			return GetBill(dt.Rows[0], new Customer(Convert.ToInt32(dt.Rows[0]["CustomerID"])));
		}

		private static Bill GetBill(DataRow row, Customer parent) {
			Bill bill = new Bill(parent);
			bill.ID = Convert.ToInt32(row["BillID"]);
			bill.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
			bill.Source = (BillingGenerationSource) Convert.ToInt16(row["Source"]);
			bill.Quote = new Quote(Convert.ToInt32(row["QuoteID"]),parent);
			bill.IsSyncToQb = Convert.ToBoolean(row["IsSyncToQB"]);
			return bill;
		}
		#endregion
	}

	public class BillCollection : CollectionBase {
		public void Add(Bill bill) {
			if (bill == null) {
				return;
			}
			this.InnerList.Add(bill);
		}

		public void AddRange(BillCollection bills) {
			foreach(Bill bill in bills) {
				Add(bill);
			}
		}

		public Bill this[int index] {
			get { return (Bill) this.InnerList[index];}
			set { this.InnerList[index] = value;}
		}
	}
}
