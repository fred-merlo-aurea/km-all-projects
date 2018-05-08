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
using System.Web.UI.DataVisualization.Charting;
using Kalitte.Dashboard.Framework.Types;
using Kalitte.Dashboard.Framework;
using Kalitte.Dashboard.Framework.Providers;
using System.Threading;
using System.Web.Security;
using System.Collections.Generic;
using KMPS.MD.Objects;

namespace KMPS.MD.Main
{
    public partial class Dashboard : KMPS.MD.Main.WebPageHelper
    {
        protected System.Web.UI.WebControls.DropDownList ExplodedPointList;
        protected System.Web.UI.WebControls.DropDownList HoleSizeList;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            Master.Menu = "Dashboard";

            DashboardFramework.ChangeProvider(DataFunctions.GetSubDomain().ToLower());

            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("mBody");
            body.Attributes.Add("onload", "doOnLoad()");
            body.Attributes.Add("onunload", "doUnLoad()");
            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Dashboard, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                //Theme

                //foreach (DashboardTheme dhbT in Enum.GetValues(typeof(DashboardTheme)))
                //{
                //    drpKTheme.Items.Add(new ListItem(dhbT.ToString(), dhbT.ToString()));
                //}

                //object theme = Session["selectedKTheme"];
                //if (theme != null)
                //    drpKTheme.Items.FindByValue(theme.ToString()).Selected = true;
                //else
                //    drpKTheme.Items.FindByValue(DashboardTheme.Kalitte.ToString()).Selected = true;


                int brandID = 0;

                if (Request.QueryString["brandID"] != null)
                {
                    brandID = int.Parse(Request.QueryString["brandID"].ToString());
                }

                List<Brand> lbrand = new List<Brand>();
                bool IsBrandAssignedUser = false;

                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    lbrand = Brand.GetByUserID(Master.clientconnections, Master.LoggedInUser);

                    if (lbrand.Count > 0)
                    {
                        IsBrandAssignedUser = true;

                        if (brandID == 0)
                        {
                            Response.Redirect("Dashboard.aspx?brandID=" + lbrand[0].BrandID.ToString(), true);
                        }
                    }
                }

                if (lbrand.Count == 0)
                {
                    lbrand = Brand.GetAll(Master.clientconnections);
                }

                Brand brand = null;

                if (brandID > 0)
                {
                    brand = lbrand.Find(x => x.BrandID == brandID);

                    if (brand == null)
                        Response.Redirect("../SecurityAccessError.aspx");
                }

                if (lbrand.Count > 0)
                {
                    pnlBrand.Visible = true;

                    drpBrand.Visible = true;
                    drpBrand.DataSource = lbrand;
                    drpBrand.DataBind();

                    if (!IsBrandAssignedUser)
                        drpBrand.Items.Insert(0, new ListItem("--Select Brand--", "0"));
                    else
                        drpBrand.Items[0].Selected = true;

                    if (brandID >= 0)
                    {
                        drpBrand.ClearSelection();
                        drpBrand.Items.FindByValue(brandID.ToString()).Selected = true;

                        if (brandID > 0)
                        {
                            imglogo.ImageUrl = "../Images/logo/" + Master.UserSession.CustomerID + "/" + brand.Logo;
                            imglogo.Visible = true;
                        }
                    }
                }
                else
                {
                    pnlBrand.Visible = false;
                }

                try
                {
                    List<DashboardInstance> dashboardsOfUser;
                    dashboardsOfUser = DashboardFramework.GetDashboards(((FormsIdentity)User.Identity).Name);
                    if (dashboardsOfUser.Count == 0)
                    {
                        ctlCreateDashboard();
                        dashboardsOfUser = DashboardFramework.GetDashboards(((FormsIdentity)User.Identity).Name);
                    }
                    //DashboardSurface1.DashboardToolbarPrepare += new Kalitte.Dashboard.Framework.DashboardToolbarPrepareHandler(DashboardSurface1_DashboardToolbarPrepare);
                    DashboardSurface1.WidgetTypeMenuPrepare += new Kalitte.Dashboard.Framework.WidgetTypeMenuPrepareEventHandler(surface_WidgetTypeMenuPrepare);
                    DashboardSurface1.DashboardKey = dashboardsOfUser[0].InstanceKey.ToString();
                    DashboardSurface1.DataBind();
                }
                catch
                {

                }
            }

        }

        protected void OnInit(object sender, EventArgs e)
        {
            base.OnInit(e);

            DashboardFramework.ChangeProvider(DataFunctions.GetSubDomain().ToLower());

            //object theme = Session["selectedKTheme"];
            //if (theme != null)
            //    Kalitte.Dashboard.Framework.ScriptManager.GetInstance(this.Page).Theme = (DashboardTheme)Enum.Parse(typeof(DashboardTheme), theme.ToString());
            //else
            //    Kalitte.Dashboard.Framework.ScriptManager.GetInstance(this.Page).Theme = DashboardTheme.Kalitte;
        }


        protected void surface_WidgetTypeMenuPrepare(object sender, Kalitte.Dashboard.Framework.WidgetTypeMenuPrepareEventArgs e)
        {
            try
            {
                List<DashboardInstance> dashboardsOfUser;
                dashboardsOfUser = DashboardFramework.GetDashboards(((FormsIdentity)User.Identity).Name);

                List<WidgetInstance> listadded = DashboardFramework.GetWidgetInstances(dashboardsOfUser[0].InstanceKey.ToString());
                var widgets = DashboardFramework.GetAllWidgetTypes();

                foreach (var item in widgets)
                {
                    if (!item.Group.Equals("Inactive"))
                    {
                        bool ispresent = false;
                        foreach (var widgetint in listadded)
                        {
                            if (widgetint.Type.InstanceKey.Equals(item.InstanceKey))
                            {
                                ispresent = true;
                                break;
                            }
                        }

                        if (!ispresent)
                            e.List.Add(new WidgetTypeMenuItemData(item));
                        ispresent = false;
                    }
                }
            }
            catch
            {

            }
        }

        protected void ctlCreateDashboard()
        {
            Guid dashboardId = Guid.NewGuid();
            CreateNewDashboard(dashboardId);
        }

        protected void DashboardSurface1_DashboardToolbarPrepare(object sender, DashboardToolbarPrepareEventArgs e)
        {
            //if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            //{
            //    e.Toolbar.AddItem(new DashboardToolbarSeperator("ctlSeperator1"));
            //    DashboardToolbarButton btnEdit = new DashboardToolbarButton("ctlEditDashboard")
            //    {
            //        Text = "Edit Dashboard",
            //        Icon = WidgetIcon.PageEdit,
            //        CommandName = "EditDashboards",
            //        Hint = "<b>Edit Dashboard</b><br/>Click to edit this dashboard.",
            //        MaskMessage = "Loading dashboard editor ..."
            //    };
            //    btnEdit.Arguments.Add("key", e.Instance.InstanceKey.ToString());
            //    e.Toolbar.AddItem(btnEdit);
            //}
        }

        protected void DashboardSurface1_DashboardCommand(object sender, DashboardCommandArgs e)
        {
            if (e.Command.CommandName == "EditDashboards")
            {
                Response.Redirect(string.Format("EditDashboard.aspx?d={0}", e.Command.Arguments["key"]));
            }

        }

        protected DashboardInstance CreateNewDashboard(Guid id)
        {
            DashboardInstance newDashboard = new DashboardInstance();

            try
            {
                newDashboard.Username = ((FormsIdentity)User.Identity).Name;
                newDashboard.Title = "";
                newDashboard.InstanceKey = id;
                newDashboard.ViewMode = DashboardViewMode.CurrentPage;

                DashboardSectionInstance row1 = new DashboardSectionInstance(newDashboard.InstanceKey, 0, Guid.NewGuid());
                row1.Columns.Add(new DashboardColumn(row1.InstanceKey, 0, 50));
                row1.Columns.Add(new DashboardColumn(row1.InstanceKey, 1, 50));
                //DashboardSectionInstance row2 = new DashboardSectionInstance(newDashboard.InstanceKey, 1, Guid.NewGuid());
                //row2.Columns.Add(new DashboardColumn(row2.InstanceKey, 0, 33));
                //row2.Columns.Add(new DashboardColumn(row2.InstanceKey, 1, 33));
                //row2.Columns.Add(new DashboardColumn(row2.InstanceKey, 2, 34));

                newDashboard.Rows.Add(row1);
                //newDashboard.Rows.Add(row2);

                DashboardFramework.CreateDashboard(newDashboard);
            }
            catch
            {
            }

            return newDashboard;
        }

        protected void DashboardSurface1_WidgetAdded(object sender, WidgetEventArgs e)
        {
            redirecttoSamePage();
        }

        protected void DashboardSurface1_OnAfterWidgetCommand(object sender, WidgetCommandArgs e)
        {
            if (e.Command.CommandName == "closeDone")
            {
                redirecttoSamePage();
            }
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx?brandID=" + drpBrand.SelectedItem.Value, true);
        }

        //protected void drpKTheme_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["selectedKTheme"] = drpKTheme.SelectedItem.Value;

        //    Response.Redirect("Dashboard.aspx?brandID=" + drpBrand.SelectedItem.Value, true);
        //}



        private void redirecttoSamePage()
        {
            if (Request.QueryString["brandID"] != null)
            {
                int brandID = int.Parse(Request.QueryString["brandID"].ToString());
                Response.Redirect("Dashboard.aspx?brandID=" + brandID, true);
            }
            else
                Response.Redirect("Dashboard.aspx", true);
        }
    }
}
