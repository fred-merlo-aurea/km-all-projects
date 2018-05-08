using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KMPS.MD.Objects;


namespace KMPS.MDAdmin
{
    public partial class MasterGroupsSort : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Master Groups";
            Master.SubMenu = "Master Groups Sort";
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if (!IsPostBack)
            {
                LoadMasterGroup();
            }
        }

        protected void LoadMasterGroup()
        {
            lstSourceFields.DataSource = KMPS.MD.Objects.MasterGroup.GetAll(Master.clientconnections);
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
                MasterGroup.DeleteCache(Master.clientconnections);

                using (SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections))
                {
                    conn.Open();
                    int count = 0;
                    foreach (ListItem li in lstSourceFields.Items)
                    {
                        count++;
                        SqlCommand cmdUpdate = new SqlCommand("update MasterGroups set SortOrder = " + count.ToString() + " where MasterGroupID = " + li.Value, conn);
                        //SqlCommand cmdInsert = new SqlCommand("INSERT INTO DisplaySetup( MagazineID, ResponseGroup) VALUES (" + drpMagazine.SelectedItem.Value + ",'" + li.Value + "')", conn);
                        cmdUpdate.ExecuteNonQuery();
                        cmdUpdate.Dispose();
                    }
                    conn.Close();
                }

                Response.Redirect("MasterGroups.aspx");
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("MasterGroups.aspx");
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