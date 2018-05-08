using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class MasterCodeSheetSort : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Master Groups";
            Master.SubMenu = "Master Code Sheet Sort";

            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                List<MasterGroup> mgroup = MasterGroup.GetAll(Master.clientconnections);
                var query = (from mg in mgroup orderby mg.SortOrder ascending select mg);

                drpMasterGroups.DataSource = query.ToList();
                drpMasterGroups.DataBind();
                loadListBox();
                           Master.Menu = "Master Groups";
                Master.SubMenu = "Master Code Sheet Sort";}
        }
        protected void drpMasterGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadListBox();
        }

        protected void loadListBox()
        {
            lstSourceFields.DataSource = MasterCodeSheet.GetByMasterGroupID(Master.clientconnections, Convert.ToInt32(drpMasterGroups.SelectedItem.Value));
            lstSourceFields.DataBind();
        }

        protected void btnUp_Click(Object sender, EventArgs e)
        {
            for (int i = 0; i < lstSourceFields.Items.Count; i++)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    if (i > 0 && !lstSourceFields.Items[i - 1].Selected)
                    {
                        ListItem bottom = lstSourceFields.Items[i];
                        lstSourceFields.Items.Remove(bottom);
                        lstSourceFields.Items.Insert(i - 1, bottom);
                        lstSourceFields.Items[i - 1].Selected = true;
                    }
                }
            }
        }

        protected void btndown_Click(Object sender, EventArgs e)
        {
            int startindex = lstSourceFields.Items.Count - 1;

            for (int i = startindex; i > -1; i--)
            {
                if (lstSourceFields.Items[i].Selected)
                {
                    if (i < startindex && !lstSourceFields.Items[i + 1].Selected)
                    {
                        ListItem bottom = lstSourceFields.Items[i];
                        lstSourceFields.Items.Remove(bottom);
                        lstSourceFields.Items.Insert(i + 1, bottom);
                        lstSourceFields.Items[i + 1].Selected = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListItem li in lstSourceFields.Items)
                {
                    count++;
                    MasterCodeSheet.update(Master.clientconnections, count, Convert.ToInt32(li.Value));
                }

                Response.Redirect("MasterCodeSheetSort.aspx");
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MasterCodeSheetSort.aspx");
        }

        protected void ibSort_Click(object sender, EventArgs e)
        {
            List<ListItem> lst = new List<ListItem>(lstSourceFields.Items.Cast<ListItem>());

            lst = lst.OrderBy(x => x.Text).ToList<ListItem>();

            lstSourceFields.Items.Clear();
            lstSourceFields.Items.AddRange(lst.ToArray<ListItem>());
        }
    }
}