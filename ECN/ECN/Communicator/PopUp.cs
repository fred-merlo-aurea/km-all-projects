using System;
using System.Web;

namespace ecn.communicator.classes {
	
	
	
	public class PopUp {
		public PopUp() {
			//
			// TODO: Add constructor logic here
			//
		}

		public static string PopupMsg{
			get{	return HttpContext.Current.Session["msg"].ToString();	}
			set{	HttpContext.Current.Session.Add("msg", value);	}
		} 
	}
}
