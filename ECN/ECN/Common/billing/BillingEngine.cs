using System;

namespace ecn.common.classes.billing
{	
	public class BillingEngine : IBillingHistoryManager
	{		
		public BillingEngine(DateTime processDate, int days) : this(processDate, days, BillingGenerationSource.BillingEngine) {}

		public BillingEngine(DateTime processDate, int days, BillingGenerationSource source) {
			_processDate = processDate;
			_days = days;
			_source = source;
		}

		// Used for test purpose only
		public BillingEngine(IBillingHistoryManager historyManager, DateTime processDate, int days): this(processDate, days) {
			_billingHistoryManager = historyManager;
		}

		// Used for test purpose only
		public BillingEngine(IBillingHistoryManager historyManager, DateTime processDate, int days, BillingGenerationSource source): this(processDate, days, source) {
			_billingHistoryManager = historyManager;
		}

		private IBillingHistoryManager _billingHistoryManager = null;
		protected IBillingHistoryManager BillingHistoryManager {
			get {				
				if (_billingHistoryManager == null) {
					_billingHistoryManager = this;
				}
				return (this._billingHistoryManager);
			}		
		}


		private DateTime _processDate;
		public DateTime ProcessDate {
			get {
				return (this._processDate);
			}			
		}

		private int _days;
		public int Days {
			get {
				return (this._days);
			}			
		}

		private string _engineID;
		public string EngineID {
			get {
				return (this._engineID);
			}
			set {
				this._engineID = value;
			}
		}


		private BillingGenerationSource _source;

		public BillCollection CreateBills(Customer customer) {
			BillCollection bills = new BillCollection();

			foreach(Quote q in customer.Quotes) {				
				bills.Add(CreateBill(q));
			}
			return bills;
		}

		public Bill CreateBill(Quote quote) {
			Bill bill = quote.CreateBill(BillingHistoryManager, ProcessDate);								
			ClearBill(bill, ProcessDate, Days);
			if (bill != null) {
				bill.Source = _source;
			}
			return bill;
		}

		private void ClearBill(Bill bill,DateTime processDate, int days) {
			if (bill == null) {
				return;
			}
			for(int i = bill.CurrentBillItems.Count-1; i >=0 ; i--) {
				if (DoesBelongToBillingCycle(bill.CurrentBillItems[i], processDate, days)) {
					continue;
				}
				bill.CurrentBillItems.RemoveAt(i);
			}
		}

		private bool DoesBelongToBillingCycle(BillItem billItem, DateTime processDate, int days) {			
			if (billItem.StartDate > processDate.AddDays(days)) {			
				return false;
			}
			return true;
		}
		

		#region IBillingHistoryManager Members

		public BillItem GetLatestBillItem(QuoteItem quoteItem) {			
			return BillItem.GetLatestBillItemForQuotoItem(quoteItem.ID);
		}

		#endregion
	}
}
