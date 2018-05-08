using System;
using ecn.common.classes.billing;

namespace ecn.common.classes.license {
	public class EmailUsageLicense : LicenseBase {
		public EmailUsageLicense(QuoteItem quoteItem, long quantity, long usedCount, DateTime start, DateTime end) : base(quoteItem) {			
			_usage = new UsageLicense(quantity, usedCount, start, end);
		}			

		private UsageLicense _usage;
		public UsageLicense Usage {
			get {
				return (this._usage);
			}			
		}		

//		public EmailUsageLicense GetEmailUsageLicenseByBillItemID(int billItemID) {
//			string sql = string.Format(@"select cl.* from BillItems bi join CustomerLicense cl on bi.QuoteItemID = cl.QuoteItemID and DateDiff(day, bi.StartDate, cl.AddDate) <= 1 and DateDiff(day, bi.StartDate, cl.AddDate) >= 0
//				where BillItemID = {0}",billItemID);
//		}

		#region Overriden Methods
		public override void Enable() {
			SetUsageLicense(this.Usage, "emailblock10k", true);
		}

		public override void Disable() {
			SetUsageLicense(this.Usage, "emailblock10k", false);
		}		
		#endregion
	}
}
