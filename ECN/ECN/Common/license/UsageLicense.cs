using System;

namespace ecn.common.classes.license
{
	
	
	
	public class UsageLicense {
		public UsageLicense(long quantity, long usedCount, DateTime start, DateTime end) {			
			_quantity = quantity;
			_usedCount = usedCount;
			_startDate = start;
			_endDate = end;
		}			

		private long _quantity;
		public long Quantity {
			get {
				return (this._quantity);
			}
			set {
				this._quantity = value;
			}
		}

		private long _usedCount;
		public long UsedCount {
			get {
				return (this._usedCount);
			}
			set {
				this._usedCount = value;
			}
		}

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
	}
}
