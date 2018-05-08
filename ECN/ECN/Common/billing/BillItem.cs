using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes.license;

namespace ecn.common.classes.billing
{
	
	public enum BillItemStatusEnum { Pending, ChargedToCreditCard,InvoicePaid, Canceled, CustomerCredit, Failed }
	public class BillItem
	{		
		public BillItem(QuoteItem quoteItem) {
			QuoteItem = quoteItem;
			Quantity = quoteItem.Quantity;
			Rate = quoteItem.Rate * (1- quoteItem.DiscountRate); 

			if (quoteItem.IsCustomerCredit) {
				Status = BillItemStatusEnum.CustomerCredit;
			}
		}
		#region Properties
		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		private Bill _parent;
		public Bill Parent {
			get {
				return (this._parent);
			}
			set {
				this._parent = value;
			}
		}

		private QuoteItem _quoteItem;
		public QuoteItem QuoteItem {
			get {				
				return (this._quoteItem);
			}
			set {
				this._quoteItem = value;
			}
		}

		private DateTime _changedDate;
		public DateTime ChangedDate {
			get {
				return (this._changedDate);
			}
			set {				
				this._changedDate = value;
			}
		}


		// Still need Quantity and Rate to capture the snapshoot of the quote item.
		private long _quantity;
		public long Quantity {
			get {
				return (this._quantity);
			}
			set {
				this._quantity = value;
			}
		}

		private double _rate;
		public double Rate {
			get {
				return (this._rate);
			}
			set {
				this._rate = value;
			}
		}
		
		public double Total {
			get {
				return _rate * _quantity;
			}		
		}

		
		/// The good time for each item is different.
		
		private DateTime _startDate;
		public DateTime StartDate {
			get {
				return (this._startDate);
			}
			set {
				this._startDate = value;
			}
		}
		
		private DateTime _endDate;
		public DateTime EndDate {
			get {
				return (this._endDate);
			}
			set {
				this._endDate = value;
			}
		}

		private BillItemStatusEnum _status = BillItemStatusEnum.Pending;
		public BillItemStatusEnum Status {
			get {
				return (this._status);
			}
			set {
				if (_status == BillItemStatusEnum.CustomerCredit) {
					throw new InvalidOperationException("Status of bill item as customer credit is not allowed to change.");
				}
				if (value != _status) {
					ChangedDate = DateTime.Now;
				}
				this._status = value;
			}
		}

		public string Description {
			get { return QuoteItem.Description;}
		}

		private bool _isProcessedByLicenseEngine;
		public bool IsProcessedByLicenseEngine {
			get {
				return (this._isProcessedByLicenseEngine);
			}
			set {
				this._isProcessedByLicenseEngine = value;
			}
		}

		public string BillType {
			get { return QuoteItem.Parent.BillType;}
		}

		public string ItemCode {
			get { return QuoteItem.Code;}
		}

		private string _transactionID = "";
		public string TransactionID {
			get {
				return (this._transactionID);
			}
			set {
				this._transactionID = value;
			}
		}
		#endregion

		public DateTime CalculateEndDate() {
			if (QuoteItem.Frequency == FrequencyEnum.OneTime) {
				return DateTimeInterpreter.MaxValue;
			}

			if (QuoteItem.Frequency == FrequencyEnum.Annual) {
				return StartDate.AddYears(1).AddDays(-1);
			}

			if (QuoteItem.Frequency == FrequencyEnum.Monthly) {
				return StartDate.AddMonths(1).AddDays(-1);
			}

			if (QuoteItem.Frequency == FrequencyEnum.Quarterly) {
				return StartDate.AddMonths(3).AddDays(-1);
			}

			if (QuoteItem.Frequency == FrequencyEnum.BiWeekly) {
				return StartDate.AddDays(15).AddDays(-1);
			}

			throw new ApplicationException(string.Format("Unknown billing frequency '{0}'.", QuoteItem.Frequency));				
		}

		public DateTime CalculateStartDate(BillItem historyBillItem, DateTime billCreatedDate) {
			if (historyBillItem == null) {
				if (billCreatedDate > QuoteItem.Parent.StartDate) {
					return billCreatedDate;
				}

				return QuoteItem.Parent.StartDate;
			}

			DateTime nextStartDate = historyBillItem.EndDate.AddDays(1);

			if (billCreatedDate > nextStartDate) {
				return billCreatedDate;
			}

			return nextStartDate;			
		}


		#region License related Methods

		public ArrayList CreateLicenses() {
			ArrayList licenses = new ArrayList();
			EmailUsageLicense el = this.CreateEmailUsageLicense();
			if (el != null) {
				el.BillItem = this;
				licenses.Add(el);
			}

			ProductLicense pl = this.CreateProductLicense();
			if (pl != null) {	
				pl.BillItem = this;
				licenses.Add(pl);
			}
				
			licenses.AddRange(this.CreateProductFeatureLicenses());
			licenses.AddRange(this.CreateServiceLicenses());
			return licenses;
		}

		public EmailUsageLicense CreateEmailUsageLicense() {
			if (this.QuoteItem.LicenseType == LicenseTypeEnum.EmailBlock) {
				return new EmailUsageLicense(QuoteItem, Quantity, 0, StartDate, EndDate);
			}

			return null;
		}		

		public ProductLicense CreateProductLicense() {
			if (QuoteItem.Products.Count == 0) {
				return null;
			}

			ProductLicense pl = new ProductLicense(QuoteItem);			
			return pl;
		}

		public ArrayList CreateProductFeatureLicenses() {
			ArrayList features = new ArrayList();
			foreach(ProductFeature feature in QuoteItem.ProductFeatures) {
				ProductFeatureLicense pfl = new ProductFeatureLicense(QuoteItem, feature);
				pfl.BillItem = this;
				features.Add(pfl);
			}
			return features;
		}

		public ArrayList CreateServiceLicenses() {
			ArrayList serviceLicenses = new ArrayList();
			foreach(Service service in QuoteItem.Services) {
				ServiceLicense sl = new ServiceLicense(QuoteItem,service, StartDate, EndDate);
				sl.BillItem = this;
				serviceLicenses.Add(sl);
			}
			return serviceLicenses;
		}
		#endregion

		#region Database Methods 
		public void Save(SqlConnection connection) {

			SqlCommand insertCommand = new SqlCommand("INSERT INTO BillItems (BillID, QuoteItemID, ChangedDate, Quantity, Rate, StartDate, EndDate, Status, TransactionID) VALUES (@billID, @quoteItemID, @changedDate, @Quantity, @rate, @startDate, @endDate, @status, @transactionID);SELECT @@IDENTITY;", connection);

			if (this.ID <= 0) {
				insertCommand.Parameters.Add("@billID", SqlDbType.Int, 4).Value = this.Parent.ID;
				insertCommand.Parameters.Add("@quoteItemID", SqlDbType.Int, 4).Value = this.QuoteItem.ID;
				insertCommand.Parameters.Add("@changedDate", SqlDbType.DateTime, 8).Value = this.ChangedDate;
				insertCommand.Parameters.Add("@quantity", SqlDbType.Int, 4).Value = this.Quantity;
				insertCommand.Parameters.Add("@Rate", SqlDbType.Float,8).Value = this.Rate;
				insertCommand.Parameters.Add("@startDate", SqlDbType.DateTime, 8).Value = this.StartDate;
				insertCommand.Parameters.Add("@endDate", SqlDbType.DateTime, 8).Value = this.EndDate;
				insertCommand.Parameters.Add("@status", SqlDbType.TinyInt, 1).Value = this.Status;
				insertCommand.Parameters.Add("@transactionID", SqlDbType.VarChar, 100).Value = this.TransactionID;
				

				try {
					insertCommand.Prepare();
					this.ID = Convert.ToInt32( insertCommand.ExecuteScalar());
					return;
				} finally {
					insertCommand.Dispose();
				}
			}

			SqlCommand updateCommand = new SqlCommand("UPDATE BillItems SET Status = @status, IsProcessedByLicenseEngine = @processed, ChangedDate = @changedDate, TransactionID = @transactionID where BillItemID = @billItemID", connection);
			updateCommand.Parameters.Add("@changedDate", SqlDbType.DateTime, 8).Value = this.ChangedDate;
			updateCommand.Parameters.Add("@status", SqlDbType.TinyInt,1).Value = this.Status;
			updateCommand.Parameters.Add("@processed", SqlDbType.Bit,1).Value = this.IsProcessedByLicenseEngine;
			updateCommand.Parameters.Add("@transactionID", SqlDbType.VarChar, 100).Value = this.TransactionID;
			updateCommand.Parameters.Add("@billItemID", SqlDbType.Int, 4).Value = this.ID;			

			try {
				updateCommand.Prepare();
				updateCommand.ExecuteScalar();
			} finally {
				updateCommand.Dispose();
			}
		}

		public void Save() {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			try {
				conn.Open();
				Save(conn);
			} finally {
				conn.Close();
				conn.Dispose();
			}	
		}

		public void SetProcessedFlag(bool isProcess) {
			DataFunctions.Execute(string.Format("UPDATE BillItems SET IsProcessedByLicenseEngine = {0} where BillItemID = {1}", isProcess?1:0, ID));
		}
		#endregion

		#region Static Database Methods 
		public static BillItemCollection GetBillItemsByBillID(int billID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from BillItems where BillID = " + billID.ToString());
			BillItemCollection items = new BillItemCollection();

			Bill parent = Bill.GetBillByID(billID);
			foreach(DataRow row in dt.Rows) {				
				items.Add(GetBillItem(row, parent));
			}
			return items;
		}

		public static BillItem GetLatestBillItemForQuotoItem(int quoteItemID) {
			// DataTable dt = DataFunctions.GetDataTable("SELECT top 1 * FROM BillItems bi JOIN Bills b on bi.BillID = b.BillID Where bi.QuoteItemID = " + quoteItemID.ToString() + " order by b.ID DESC;");
			DataTable dt = DataFunctions.GetDataTable("SELECT top 1 * FROM BillItems Where QuoteItemID = " + quoteItemID.ToString() + " order by EndDate DESC;");
			if (dt.Rows.Count == 0) {
				return null;
			}
		
			return GetBillItem(dt.Rows[0], Bill.GetBillByID(Convert.ToInt32(dt.Rows[0]["BillID"])) );
		}

		public static BillItemCollection GetPendingCreditCardBillItems() {
			DataTable dt = DataFunctions.GetDataTable(string.Format(@"select bi.* from Quote q join QuoteItem qi on q.QuoteID = qi.QuoteID 
join BillItems bi on qi.QuoteItemID = bi.QuoteItemID
where q.BillType = 'CreditCard' and qi.RecurringProfileID <> '' and bi.Status <> {0}", (int)BillItemStatusEnum.ChargedToCreditCard));
			BillItemCollection billItems = new BillItemCollection();
			Hashtable parents = new Hashtable();

			foreach(DataRow row in dt.Rows) {
				int billID = Convert.ToInt32(row["BillID"]);
				Bill parent = parents[billID] as Bill;
				if (parent == null) {
					parent = Bill.GetBillByID(billID);
					parents.Add(billID, parent);
				}
				billItems.Add(GetBillItem(row, parent));
			}
			return billItems;
		}

		
		/// Get Bill items which should enable its license. (Paid by credit, paid by invoice, customer credit)
		
		/// <param name="shouldEnable"></param>
		/// <returns></returns>
		public static BillItemCollection GetNewBillItems(bool shouldEnable) {
			string sql =  shouldEnable?string.Format("Select * from BillItems where IsProcessedByLicenseEngine = 0 and Status != {0} and Status != {1}",		
				(int) BillItemStatusEnum.Canceled, (int) BillItemStatusEnum.Pending):
				string.Format("Select * from BillItems where IsProcessedByLicenseEngine = 0 and (Status = {0} or Status = {1})",		
				(int) BillItemStatusEnum.Canceled, (int) BillItemStatusEnum.Pending);

			DataTable dt = DataFunctions.GetDataTable(sql);
			BillItemCollection items = new BillItemCollection();
			
			foreach(DataRow row in dt.Rows) {				
				Bill parent = Bill.GetBillByID(Convert.ToInt32(row["BillID"]));
				items.Add(GetBillItem(row, parent));
			}

			return items;
			
		}

		private static BillItem GetBillItem(DataRow row, Bill parent) {
			int quoteID = Convert.ToInt32(row["QuoteItemID"]);
			QuoteItem quoteItem = QuoteItem.GetQuoteItemByID(quoteID);
			BillItem item = new BillItem(quoteItem);
			item.ID = Convert.ToInt32(row["BillItemID"]);
			item.Parent = parent;
			item.ChangedDate = Convert.ToDateTime(row["ChangedDate"]);
			item.Quantity = Convert.ToInt64(row["Quantity"]);
			item.Rate = Convert.ToDouble(row["Rate"]);
			item.StartDate = Convert.ToDateTime(row["StartDate"]);
			item.EndDate = Convert.ToDateTime(row["EndDate"]);
			item.TransactionID = Convert.ToString(row["TransactionID"]);			
			
			// Reset CustomerCredit Status will trigger an exception.
			if (!quoteItem.IsCustomerCredit) {
				item.Status = (BillItemStatusEnum) Convert.ToInt16(row["Status"]);
			}

			item.IsProcessedByLicenseEngine = Convert.ToBoolean(row["IsProcessedByLicenseEngine"]);
			return item;
		}

		#endregion
	}

	public class BillItemCollection : CollectionBase {
		public void Add(BillItem item) {
			this.InnerList.Add(item);
		}

		public BillItem this[int index] {
			get { return (BillItem) this.InnerList[index];}
			set { this.InnerList[index] = value;}
		}
	}
}
