using System;
using System.Collections;
using ecn.common.classes;
using ecn.common.classes.billing;

namespace ecn.common.classes.license
{	
	public class LicenseEngine
	{
		public LicenseEngine(DateTime startDate)
		{
			_startDate = startDate;
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

		public ArrayList CreateLicenses(BillItemCollection billingItems) {
			ArrayList licenses = new ArrayList();
			foreach(BillItem item in billingItems) {
				licenses.AddRange(item.CreateLicenses());
			}
			return licenses;
		}		
	}
}
