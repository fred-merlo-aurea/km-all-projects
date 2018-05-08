using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.content
{
    public partial class rssList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.SubMenu = "rss feeds";
            Master.Heading = "Content/Messages > RSS Feeds";
            Master.HelpContent = "";
            Master.HelpTitle = "Manage RSS Feeds";
            if (!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.RSSFeed, KMPlatform.Enums.Access.View))
                {
                    LoadData();
                    if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.RSSFeed, KMPlatform.Enums.Access.Edit))
                    {
                        gvRSSFeeds.Columns[3].Visible = true;
                        btnAddNewFeed.Visible = true;
                    }
                    else
                    {
                        gvRSSFeeds.Columns[3].Visible = false;
                        btnAddNewFeed.Visible = false;
                    }

                    if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.RSSFeed, KMPlatform.Enums.Access.Delete))
                    {
                        gvRSSFeeds.Columns[4].Visible = true;
                    }
                    else
                    {
                        gvRSSFeeds.Columns[4].Visible = false;
                    }

                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
            }
        }

        private void LoadData()
        {
            List<ECN_Framework_Entities.Communicator.RSSFeed> listRSS = ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID);
            gvRSSFeeds.DataSource = listRSS;
            gvRSSFeeds.DataBind();
        }

        protected void btnAddNewFeed_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/content/rssEdit.aspx");
        }

        protected void gvRSSFeeds_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().ToLower().Equals("deleterss"))
            {
                int feedID = -1;
                int.TryParse(e.CommandArgument.ToString(), out feedID);

                if (feedID > 0)
                {
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.RSSFeed.Delete(ECN_Framework_BusinessLayer.Communicator.RSSFeed.GetByID(feedID), Master.UserSession.CurrentUser);
                        LoadData();
                    }
                    catch (ECN_Framework_Common.Objects.ECNException ecn)
                    {
                        setECNError(ecn);
                        return;
                    }
                }
                
            }
        }

        protected void gvRSSFeeds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.RSSFeed rss = (ECN_Framework_Entities.Communicator.RSSFeed)e.Row.DataItem;

                Label Stories = (Label)e.Row.FindControl("lblStories");
                HyperLink imgbtnEdit = (HyperLink)e.Row.FindControl("hlEdit");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeleteRSS");

                Stories.Text = rss.StoriesToShow.HasValue ? rss.StoriesToShow.Value.ToString() : "All";
                imgbtnEdit.NavigateUrl = "/ecn.communicator/main/content/rssEdit.aspx?feedID=" + rss.FeedID.ToString();
                imgbtnDelete.CommandArgument = rss.FeedID.ToString();
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