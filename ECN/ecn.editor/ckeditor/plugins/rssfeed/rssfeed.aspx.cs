using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.editor.ckeditor.plugins.rssfeed
{
    public partial class rssfeed : System.Web.UI.Page
    {
        string _RSSFeeds = "";

        public string RSSFeeds
        {
            get
            {
                return _RSSFeeds;
            }
        }

        public string getCustomerID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID.ToString();
                else
                    return Request.QueryString["cuID"].ToString();
            }
            catch
            {
                return Request.QueryString["cuID"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int custID = Convert.ToInt32(getCustomerID());

            if (!Page.IsPostBack)
            {
                List<ECN_Framework_Entities.Communicator.RSSFeed> listRSSFeeds = ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByCustomerID(Convert.ToInt32(getCustomerID()));
                _RSSFeeds = "<option value=''>--SELECT--</option>";
                foreach (ECN_Framework_Entities.Communicator.RSSFeed rss in listRSSFeeds)
                {
                    _RSSFeeds += "<option value='ECN.RSSFEED." + rss.Name + ".ECN.RSSFEED'>" + rss.Name + "</option>";
                }
            }
        }
    }
}