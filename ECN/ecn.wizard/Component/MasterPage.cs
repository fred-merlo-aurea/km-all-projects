using System;
using System.Web;
using System.Web.UI;
using System.Security;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Security;
using System.Xml;
using System.Configuration;
using ecn.wizard;
using ecn.common.classes;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for MasterPage.
	/// </summary>
	public class MasterPage: System.Web.UI.Page
	{
		// This is a reference to the form that this page contains.  If the
		// page contains no form, the control points to the page itself.
		private Control ctlForm;
		private const string HeaderPath ="~/UserControls/Header.ascx";
		private const string StatusBarPath ="~/UserControls/StatusBar.ascx";
		private const string FooterPath ="~/UserControls/Footer.ascx";

		public ecn.wizard.Session WizardSession;
		private ECN_Framework.Common.SecurityCheck securityCheck = new ECN_Framework.Common.SecurityCheck();

		//=====================================================================
		// Properties

		public string PageTitle
		{
			get { return (string)ViewState["PageTitle"]; }
			set { ViewState["PageTitle"] = value; }
		}


		public string PageDescription
		{
			get
			{return (string)ViewState["PageDesc"];}
			set { ViewState["PageDesc"] = value; }
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

		public string PageKeywords
		{
			get
			{return (string)ViewState["PageKeywords"];}
			set { ViewState["PageKeywords"] = value; }
		}


		/// <summary>
		/// This property is used to set or get a reference to the form
		/// control that appears on the page as indicated by the presence
		/// of a &lt;form&gt; tag.
		/// </summary>
		/// <value>If not explicitly set, it defaults to the form on
		/// the page or the page itself if there isn't one.
		/// <p/>Derived classes can use this property to access the form
		/// in order to insert additional controls into it.  For example,
		/// the derived <see cref="MenuPage"/> classes use it to insert the
		/// supporting table structure and the menu control around the form
		/// that makes up the page's content.</value>
		/// <exception cref="System.ArgumentException">
		/// This property must be set to an
		/// <see cref="System.Web.UI.HtmlControls.HtmlForm"/> or a
		/// <see cref="System.Web.UI.Page"/> object or it will throw an
		/// exception.</exception>
		public Control PageForm
		{
			get { return ctlForm; }
			set
			{
				// Must be derived from one of these types
				if(value is System.Web.UI.HtmlControls.HtmlForm ||
					value is System.Web.UI.Page)
					ctlForm = value;
				else
					throw new ArgumentException(
						"PageForm must be set to an HtmlForm or Page object");
			}
		}


		//=====================================================================
		// Methods, etc

		public MasterPage()
		{
			ctlForm = this;
		}

		/// <summary>
		/// OnInit is overridden to locate the form control on the page and
		/// set the <see cref="PageForm"/> property to it.  For postbacks, it
		/// will also retrieve the value of the dirty flag and store it in
		/// the <see cref="Dirty"/> property.
		/// </summary>
		/// <remarks>If you override this method in a derived class, you
		/// must call this version too.</remarks>
		/// <param name="e">Event arguments</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// Find the form on this page if there is one
			foreach(Control ctlItem in Controls)
				if(ctlItem is System.Web.UI.HtmlControls.HtmlForm)
				{
					ctlForm = ctlItem;
					break;
				}

//			if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
				WizardSession = ecn.wizard.Session.GetCurrentSession();
		}



		protected virtual void RenderHeader(HtmlTextWriter writer)
		{
			StringBuilder strHeader = new StringBuilder(1024);

			// Write out the stock header
			strHeader.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" + 
				"<html>\n<head>\n" +
				"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>\n");

			if(this.PageTitle != null)
				strHeader.AppendFormat("<title>{0}</title>\n", this.PageTitle);

			strHeader.AppendFormat("<meta name='GENERATOR' content='{0} Class'>\n", GetType().BaseType);

			// Output the application-specific style sheet if specified
			strHeader.AppendFormat("<link rel='stylesheet' type='text/css' href='{0}'>", UrlBase + "style.css");

			strHeader.Append("</head>\n<body>");
			
			writer.Write(strHeader.ToString());
			
			// Insert other header tags from derived classes (if any)
			RenderCustomHeaderFooter();


		}

		protected virtual void RenderCustomHeaderFooter()
		{
			this.PageForm.Controls.AddAt(0, LoadControl(HeaderPath));

			if (Context.User.Identity.IsAuthenticated)
				this.PageForm.Controls.AddAt(1, LoadControl(StatusBarPath));

			this.PageForm.Controls.Add(LoadControl(FooterPath));
		}

		protected virtual void RenderFooter(HtmlTextWriter writer)
		{
			writer.Write("\n</body>\n</html>\n");
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
	
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			RenderHeader(writer);

			base.Render(writer);
			
			RenderFooter(writer);
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

	}
}
