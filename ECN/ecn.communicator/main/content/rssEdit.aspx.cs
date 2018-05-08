using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.ServiceModel;
using System.Xml;

namespace ecn.communicator.main.content
{
    public partial class rssEdit : System.Web.UI.Page
    {
        private static ECN_Framework_Entities.Communicator.RSSFeed rssFeed = null;
        private int FeedID
        {
            get
            {
                if (Request.QueryString["feedID"] != null)
                {
                    try
                    {
                        return Convert.ToInt32(Request.QueryString["feedID"].ToString());
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
                else
                    return -1;

            }
            set
            {
                FeedID = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.SubMenu = "rss feeds";
            Master.Heading = "Content/Messages > RSS Feeds > Add/Edit RSS Feed";
            Master.HelpContent = "";
            Master.HelpTitle = "ADD/EDIT RSS FEED";
            phError.Visible = false;
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            if (FeedID > 0)
            {
                rssFeed = ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByID(FeedID);
                txtName.Enabled = false;
            }
            else
            {
                rssFeed = new ECN_Framework_Entities.Communicator.RSSFeed();
                txtName.Enabled = true;
            }

            txtName.Text = rssFeed.Name;
            txtURL.Text = rssFeed.URL;

            ddlStories.SelectedValue = rssFeed.StoriesToShow.HasValue ? rssFeed.StoriesToShow.Value.ToString() : "0";

        }

        protected void btnSaveFeed_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(txtURL.Text.Trim()))
                {


                    rssFeed.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                    rssFeed.IsDeleted = false;
                    rssFeed.Name = txtName.Text.Trim();
                    rssFeed.URL = txtURL.Text.Trim();

                    if (ddlStories.SelectedIndex > 0)
                    {
                        int stories = -1;
                        int.TryParse(ddlStories.SelectedValue.ToString(), out stories);
                        if (stories > 0)
                            rssFeed.StoriesToShow = stories;
                        else
                        {
                            throwECNException("Please specify the amount of stories to display");
                            return;
                        }

                    }
                    else
                    {
                        throwECNException("Please specify the amount of stories to display");
                        return;
                    }

                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.RSSFeed.Save(rssFeed, Master.UserSession.CurrentUser);
                        Response.Redirect("/ecn.communicator/main/content/rssList.aspx");
                    }
                    catch (ECNException ecn)
                    {
                        setECNError(ecn);
                    }


                }
                else
                {
                    throwECNException("Please enter a URL for the feed");
                    return;
                }
            }
            else
            {
                throwECNException("Please enter a name for the feed");
                return;
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.LandingPage, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }
    }
}