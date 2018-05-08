using System;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.Security;
using System.Configuration;
using ecn.common.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for MasterControl.
	/// </summary>
	public class MasterControl: System.Web.UI.UserControl
	{
		public ecn.wizard.Session WizardSession;
		private ECN_Framework.Common.SecurityCheck securityCheck = new ECN_Framework.Common.SecurityCheck();

		public MasterControl()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			WizardSession = ecn.wizard.Session.GetCurrentSession();
		}

		//common functions

		///     Property UrlBase is used to get the prefix for URLs.
		public static String UrlBase
		{
			get
			{
				return @"http://" + UrlSuffix + "/";; 
			}
		}

		private static string UrlSuffix
		{
			get
			{
				if(HttpContext.Current.Request.ApplicationPath != "/")
					return HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
				else
					return HttpContext.Current.Request.Url.Host;
			}
		}

		public int ChannelID
		{
			get
			{
				if (Context.User.Identity.IsAuthenticated)
					return Convert.ToInt32(securityCheck.ChannelID());
				else
				{
					if (Request["cID"] != null)
						return Convert.ToInt32(Request["cID"]);
					else
						return 2;
				}
			}
		}

		public int CustomerID
		{
			get
			{
				return Convert.ToInt32(securityCheck.CustomerID());
			}
		}

		public int UserID
		{
			get
			{
				return Convert.ToInt32(securityCheck.UserID());
			}
		}

	}

}
