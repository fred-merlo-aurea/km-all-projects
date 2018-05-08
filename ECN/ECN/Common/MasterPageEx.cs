using System.Collections;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KMSite;

namespace ECN.Common
{
	public class MasterPageEx : MasterPage
	{
		protected Label lblHelpContent;
		protected Label lblHelpHeading;
		protected Label lblHeading;
		protected HtmlTableRow HideHeading;

        protected IKMAuthenticationManager KMAuthenticationManager { get; } = new KMAuthenticationManager();

        public string HelpContent
		{
			set
			{
				lblHelpContent.Text = value;
			}
		}

		public string HelpTitle
		{
			set
			{
				lblHelpHeading.Text = value;
			}
		}

		public string Heading
		{
			set
			{
				lblHeading.Text = value;
				if (lblHeading.Text != "")
				{
					HideHeading.Visible = true;
				}
			}
		}

		public string SubMenu { get; set; } = string.Empty;

        protected void EditProfileHandler()
        {
            var url = new StringBuilder(Request.Path);
            if (Request.QueryString.Count > 0)
            {
                url.Append($"?{Request.QueryString.ToString()}");
            }

            Response.Redirect($"/ecn.accounts/main/users/EditUserProfile.aspx?redirecturl={Server.UrlEncode(url.ToString())}", false);
        }

        protected void LogoutHandler()
        {
            foreach (DictionaryEntry cachedItem in Cache)
            {
                Cache.Remove(cachedItem.Key.ToString());
            }
            FormsAuthentication.SignOut();

            Response.Redirect("/EmailMarketing.Site/Login/Logout", false);
        }
    }
}
