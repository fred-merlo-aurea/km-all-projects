using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using ecn.common.classes;


namespace ecn.accounts.main.channels
 {


	public class DateRange {
		public DateRange(DateTime start, DateTime end) {
			_start = start;
			_end = end;
		}

		private DateTime _start;
		public DateTime Start {
			get {
				return (this._start);
			}
			set {
				this._start = value;
			}
		}

		private DateTime _end;
		public DateTime End {
			get {
				return (this._end);
			}
			set {
				this._end = value;
			}
		}
	}

}