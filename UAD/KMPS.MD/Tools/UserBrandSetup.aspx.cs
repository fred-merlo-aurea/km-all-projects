using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Tools
{
    public partial class UserBrandSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "User Brand Setup";

            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                loadUser();
                loadBrand();
                loadgrid();
            }
        }

        protected void loadUser()
        {
            List<KMPlatform.Entity.User> lusers = new List<KMPlatform.Entity.User>();

            if(KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                lusers = KMPlatform.BusinessLogic.User.GetByClientGroupIDServiceCode(Master.UserSession.ClientGroupID, KMPlatform.Enums.Services.UAD);
            else
                lusers = KMPlatform.BusinessLogic.User.GetByClientIDServiceCode(Master.UserSession.ClientID, KMPlatform.Enums.Services.UAD);

            List<UserBrand> luserbrand = UserBrand.getAll(Master.clientconnections);

            List<KMPlatform.Entity.User> lu = (from u in lusers
                                                where !(luserbrand.Any(x => x.UserID == u.UserID)) && !u.IsKMStaff
                                                select u).ToList();

            drpUsers.DataSource = lu;
            drpUsers.DataBind();
            drpUsers.Items.Insert(0, new ListItem("--Select User-", "0"));
        }


        protected void loadBrand()
        {
            lstSourceFields.DataSource = Brand.GetAll(Master.clientconnections);
            lstSourceFields.DataBind();
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    lstDestFields.Items.Add(lstSourceFields.Items[i]);
                    lstSourceFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnRemove_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstDestFields.Items.Count; i++)
            {
                if (lstDestFields.Items[i].Selected)
                {
                    lstSourceFields.Items.Add(lstDestFields.Items[i]);
                    lstDestFields.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                try
                {

                    if (lstDestFields.Items.Count == 0)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "Please select a Brand.";
                        return;
                    }

                    int UserID = 0;

                    if (drpUsers.Visible == true)
                    {
                        UserID = Convert.ToInt32(drpUsers.SelectedItem.Value);
                    }
                    else
                    {
                        UserID = Convert.ToInt32(lblUserID.Text);
                    }

                    UserBrand.Delete(Master.clientconnections, Convert.ToInt32(UserID));

                    if (lstDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstDestFields.Items)
                        {
                            UserBrand u = new UserBrand();
                            u.UserID = UserID;
                            u.BrandID = Convert.ToInt32(item.Value);
                            UserBrand.Save(Master.clientconnections, u);
                        }
                    }

                    Response.Redirect("UserBrandSetup.aspx");
                }
                catch (Exception ex)
                {
                    divError.Visible = true;
                    lblErrorMessage.Text = ex.Message;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserBrandSetup.aspx");
        }


        protected void gvUserBrand_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvUserBrand_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    UserBrand.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    lstDestFields.Items.Clear();
                    drpUsers.Visible = true;
                    rfvusers.Enabled = true;
                    lblUserID.Visible = false;
                    lblUserID.Text = "0";
                    loadgrid();
                    loadBrand();
                    loadUser();
                }
            }
        }

        protected void gvUserBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserBrand.PageIndex = e.NewPageIndex;
            loadgrid();
        }

        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                drpUsers.ClearSelection();
                lstSourceFields.Items.Clear();
                lstDestFields.Items.Clear();

                string[] args = e.CommandArgument.ToString().Split('/');
                int userID = Convert.ToInt32(args[0]);

                lblUsername.Text = args[1];
                lblUsername.Visible = true;
                lblUserID.Text = args[0];

                drpUsers.Visible = false;
                rfvusers.Enabled = false;

                List<UserBrand> ub = UserBrand.getByUserID(Master.clientconnections, userID);

                Dictionary<int, string> fields = new Dictionary<int, string>();

                List<Brand> brandlist = Brand.GetAll(Master.clientconnections);

                foreach (Brand b in brandlist)
                {
                    if (!ub.Exists(x => x.BrandID == b.BrandID))
                        lstSourceFields.Items.Add(new ListItem(b.BrandName, b.BrandID.ToString()));
                    else
                        fields.Add(b.BrandID, b.BrandName);
                }

                foreach (UserBrand u in ub)
                {
                    string brandname = fields.FirstOrDefault(x => x.Key == u.BrandID).Value;
                    lstDestFields.Items.Add(new ListItem(brandname, u.BrandID.ToString()));
                }
            }
        }


        protected void loadgrid()
        {
            List<KMPlatform.Entity.User> lusers = new List<KMPlatform.Entity.User>();

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                lusers = KMPlatform.BusinessLogic.User.GetByClientGroupIDServiceCode(Master.UserSession.ClientGroupID, KMPlatform.Enums.Services.UAD);
            else
                lusers = KMPlatform.BusinessLogic.User.GetByClientIDServiceCode(Master.UserSession.ClientID, KMPlatform.Enums.Services.UAD);

            List<UserBrand> luserbrand = UserBrand.getAll(Master.clientconnections);

            var query = (from a in lusers
                        join b in luserbrand
                        on a.UserID equals b.UserID
                        select a).Distinct();


            gvUserBrand.DataSource = query.ToList();
            gvUserBrand.DataBind();
        }

    }
}