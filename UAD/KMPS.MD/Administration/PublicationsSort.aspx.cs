using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;


namespace KMPS.MD.Administration
{
    public partial class PublicationsSort : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Product Sort";
            divError.Visible = false;

            if (!IsPostBack)
            {
                LoadPubTypes();
                LoadPubs();
            }
        }

        protected void LoadPubTypes()
        {
            drpPubTypes.DataSource = PubTypes.GetAll(Master.clientconnections);
            drpPubTypes.DataBind();
        }

        protected void LoadPubs()
        {
            var pubsList = Pubs.GetByPubTypeID(Master.clientconnections, Convert.ToInt32(drpPubTypes.SelectedItem.Value));

            var pubsQuery = (from p in pubsList
                             orderby p.SortOrder ascending
                             select p);

            lstSourceFields.DataSource = pubsQuery.ToList();
            lstSourceFields.DataBind();
        }

        protected void drpPubTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPubs();
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

                    Pubs p = new Pubs();
                    p.PubID = Convert.ToInt32(li.Value);
                    p.SortOrder = count;
                    Pubs.SaveSortOrder(Master.clientconnections, p);
                }

                Response.Redirect("PublicationsSort.aspx");
            }
            catch (Exception ex)
            {
                divError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("PublicationsSort.aspx");
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