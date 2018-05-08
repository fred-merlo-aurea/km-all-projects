using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Tools
{
    public partial class UserDataMaskSetUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "UserData Mask Setup";

            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                loadUser();
                lstSourceFields.DataSource = loadMaskField();
                lstSourceFields.DataBind();
                loadgrid();
            }
        }

        protected void loadUser()
        {
            List<KMPlatform.Entity.User> lusers = new List<KMPlatform.Entity.User>();

            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                lusers = KMPlatform.BusinessLogic.User.GetByClientGroupIDServiceCode(Master.UserSession.ClientGroupID, KMPlatform.Enums.Services.UAD);
            else
                lusers = KMPlatform.BusinessLogic.User.GetByClientIDServiceCode(Master.UserSession.ClientID, KMPlatform.Enums.Services.UAD);

            List<KMPlatform.Entity.User> lu = (from u in lusers
                                               where  !u.IsKMStaff
                                               orderby u.UserName 
                                               select u).ToList();

            drpUsers.DataSource = lu;
            drpUsers.DataBind();
            drpUsers.Items.Insert(0, new ListItem("--Select User-", "0"));
        }


        protected Dictionary<string, string> loadMaskField()
        {
            Dictionary<string, string> lstMaskFields = new Dictionary<string, string>();
            lstMaskFields.Add("FirstName", "FirstName");
            lstMaskFields.Add("LastName", "LastName");
            lstMaskFields.Add("Email", "Email");
            lstMaskFields.Add("Company", "Company");
            lstMaskFields.Add("Title", "Title");
            lstMaskFields.Add("Address", "Address");
            lstMaskFields.Add("Address2", "Address2");
            lstMaskFields.Add("Address3", "Address3");
            lstMaskFields.Add("City", "City");
            lstMaskFields.Add("State", "State");
            lstMaskFields.Add("Zip", "Zip");

            return lstMaskFields;
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
                        lblErrorMessage.Text = "Please select a Field.";
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

                    UserDataMask.Delete(Master.clientconnections, Convert.ToInt32(UserID));

                    if (lstDestFields.Items.Count > 0)
                    {
                        foreach (ListItem item in lstDestFields.Items)
                        {
                            UserDataMask u = new UserDataMask();
                            u.UserID = UserID;
                            u.MaskField = item.Value;
                            u.CreatedUserID = Master.LoggedInUser;
                            UserDataMask.Save(Master.clientconnections, u);
                        }
                    }

                    Response.Redirect("UserDataMaskSetup.aspx");
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
            Response.Redirect("UserDataMaskSetup.aspx");
        }

        protected void gvUserDataMask_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, Convert.ToInt32(gvUserDataMask.DataKeys[e.Row.RowIndex].Value.ToString()));
                Label lblMaskFields = (Label)e.Row.FindControl("lblMaskFields");
                lblMaskFields.Text = string.Join(",", udm.Select(u => u.MaskField));
            }
        }
        
        protected void gvUserDataMask_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void gvUserDataMask_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    UserDataMask.Delete(Master.clientconnections, Convert.ToInt32(e.CommandArgument.ToString()));
                    lstDestFields.Items.Clear();
                    drpUsers.Visible = true;
                    rfvusers.Enabled = true;
                    lblUserID.Visible = false;
                    lblUserID.Text = "0";
                    loadgrid();
                    lstSourceFields.DataSource = loadMaskField();
                    lstSourceFields.DataBind();
                    loadUser();
                }
            }
        }

        protected void gvUserDataMask_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserDataMask.PageIndex = e.NewPageIndex;
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

                Dictionary<string, string> AllMaskField = loadMaskField();
                List<UserDataMask> udm = UserDataMask.GetByUserID(Master.clientconnections, userID);

                foreach (var p in AllMaskField)
                {
                    if (!udm.Exists(x => x.MaskField == p.Value))
                        lstSourceFields.Items.Add(new ListItem(p.Value, p.Key.ToString()));
                    else
                        lstDestFields.Items.Add(new ListItem(p.Value, p.Key.ToString()));
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

            List<UserDataMask> ludm = UserDataMask.GetAll(Master.clientconnections);

            var query = (from a in lusers
                         join b in ludm
                         on a.UserID equals b.UserID
                         select a).Distinct();

            gvUserDataMask.DataSource = query.ToList();
            gvUserDataMask.DataBind();
        }
    }
}