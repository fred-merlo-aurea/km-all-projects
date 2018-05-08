using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ecn.publisher.main.Edition
{

	public partial class EditionLinks : System.Web.UI.Page
	{

		private int getEditionID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["eID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        private ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (getEditionID() > 0)
			{
                ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(getEditionID(),  UserSession.CurrentUser);

                ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, UserSession.CurrentUser);

                string subdomain = pb.PublicationCode;

				if (subdomain != string.Empty)
				{
					tbURL.Text = "http://" + subdomain.ToLower() + ".ecndigitaledition.com";
                    tbBlastURL.Text = "http://" + subdomain.ToLower() + ".ecndigitaledition.com/magazine.aspx?eid=" + getEditionID() + "&e=%%emailid%%&b=%%blastid%%"; 
				}
				else
				{
					tbURL.Text = "http://www.ecndigitaledition.com/magazine.aspx?eid=" + getEditionID(); 
					tbBlastURL.Text = "http://www.ecndigitaledition.com/magazine.aspx?eid=" + getEditionID()+ "&e=%%emailid%%&b=%%blastid%%"; 
				
				}
			}
		}
	}
}
