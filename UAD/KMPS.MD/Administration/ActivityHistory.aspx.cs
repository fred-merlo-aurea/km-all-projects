using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class ActivityHistory : System.Web.UI.Page
    {
        private string SortField
        {
            get
            {
                return ViewState["SortField"].ToString();
            }
            set
            {
                ViewState["SortField"] = value;
            }
        }

        private string SortDirection
        {
            get
            {
                return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Reports";
            divError.Visible = false;
            lblErrorMessage.Text = string.Empty;
            
            if (!IsPostBack)
            {
                txtSDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                txtEDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

                LoadUser();

                SortField = "ACTIVITYDATETIME";
                SortDirection = "DESC";

                LoadGrid();
            }
        }

        protected void LoadUser()
        {
            List<KMPlatform.Entity.User> lEusers = KMPlatform.BusinessLogic.User.GetByClientGroupID(Master.UserSession.ClientGroupID);

            var query = lEusers.GroupBy(member => member.UserID).Select(x => x.OrderBy(y => y.UserName).First()).OrderBy(z=>z.UserName);

            drpUsers.DataSource = query;
            drpUsers.DataBind();
            drpUsers.Items.Insert(0, new ListItem("--Select User-", "0"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void gvActivityHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == SortField)
            {
                SortDirection = (SortDirection.ToUpper() == "ASC" ? "DESC" : "ASC");
            }
            else
            {
                SortField = e.SortExpression;
                SortDirection = "ASC";
            }

            LoadGrid();
        }

        protected void LoadGrid()
        {
            gvActivityHistory.DataSource = null;
            gvActivityHistory.DataBind();

            List<UserTracking> lusertracking = UserTracking.GetByDate(Master.clientconnections, txtSDate.Text, txtEDate.Text);

            List<KMPlatform.Entity.User> lEusers = KMPlatform.BusinessLogic.User.GetByClientGroupID(Master.UserSession.ClientGroupID);

            lEusers = lEusers.GroupBy(member => member.UserID).Select(x => x.OrderBy(y => y.UserName).First()).OrderBy(z => z.UserName).ToList();

            List<UserTracking> lut = (from u in lusertracking
                                      join eu in lEusers on u.UserID equals eu.UserID
                                      select new UserTracking()
                                      {   Activity = u.Activity,
                                          ActivityDateTime = u.ActivityDateTime,
                                          BrowserInfo = u.BrowserInfo,
                                          ClientID = u.ClientID,
                                          Client = u.Client,
                                          IPAddress = u.IPAddress,
                                          Platform = u.Platform,
                                          PlatformID = u.PlatformID,
                                          UserID = u.UserID,
                                          UserName = eu.UserName,
                                          UserTrackingID = u.UserTrackingID
                                      }).ToList();

            if (Convert.ToInt32(drpUsers.SelectedItem.Value) > 0)
            {
                 lut = lut.FindAll(u => u.UserID == Convert.ToInt32(drpUsers.SelectedItem.Value));
            }

            if (lut != null && lut.Count > 0)
            {
                List<UserTracking> lst = null;

                switch (SortField.ToUpper())
                {
                    case "USERNAME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.UserName).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.UserName).ToList();
                        break;

                    case "ACTIVITY":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.Activity).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.Activity).ToList();
                        break;

                    case "ACTIVITYDATETIME":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.ActivityDateTime).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.ActivityDateTime).ToList();
                        break;

                    case "IPADDRESS":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.IPAddress).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.IPAddress).ToList();
                        break;

                    case "BROWSERINFO":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.BrowserInfo).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.BrowserInfo).ToList();
                        break;

                    case "PLATFORM":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.Platform).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.Platform).ToList();
                        break;

                    case "CLIENT":
                        if (SortDirection.ToUpper() == "ASC")
                            lst = lut.OrderBy(o => o.Client).ToList();
                        else
                            lst = lut.OrderByDescending(o => o.Client).ToList();
                        break;
                }

                gvActivityHistory.DataSource = lst;
                gvActivityHistory.DataBind();
            }
        }

        protected void gvActivityHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivityHistory.PageIndex = e.NewPageIndex;
            LoadGrid();
        }
    }
}