using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KMPS_JF_Objects.Objects;
using System.Text;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_AutoSubscription : System.Web.UI.Page
    {
        public int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubId"].ToString());
                }
                catch { return 0; }
            }
        }

        JFSession jfsess = new JFSession();
        Publication pub = null;        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pub = Publication.GetPublicationbyID(PubID, "");
            }
            catch { }


            try
            {
                if (!IsPostBack)
                {
                    SqlCommand PublisherList = new SqlCommand();
                    SqlDataSourcePCustomerConnect.SelectCommand = "select * from Customer where CustomerId in (" + jfsess.AllowedCustoemerIDs() + ")";
                    SqlDataSourcePCustomerConnect.Select(DataSourceSelectArguments.Empty);
                    LoadAvailableGroups(0);
                    LoadSelectedGroups(0);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadAvailableGroups(int customerID)
        {
            SqlCommand cmdAvailableGroups = new SqlCommand("spGetAvailableGroupsForPublication");
            cmdAvailableGroups.CommandType = CommandType.StoredProcedure;

            string customerIDs = string.Empty;

            if (customerID == 0)
                customerIDs = jfsess.AllowedCustoemerIDs();
            else
                customerIDs = customerID.ToString();

            cmdAvailableGroups.Parameters.AddWithValue("@pubID", pub.PubID.ToString());
            cmdAvailableGroups.Parameters.AddWithValue("@customerIDs", customerIDs);

            DataTable dtAvailableGroups = DataFunctions.GetDataTable(cmdAvailableGroups);
            lstAvailableGroups.DataSource = dtAvailableGroups;
            lstAvailableGroups.DataTextField = "GroupName";
            lstAvailableGroups.DataValueField = "GroupID";
            lstAvailableGroups.DataBind();
        }

        private void LoadSelectedGroups(int customerID)
        {
            SqlCommand cmdSelectedGroups = new SqlCommand("spGetSelectedGroupsForPublication");
            cmdSelectedGroups.CommandType = CommandType.StoredProcedure;
            string customerIDs = string.Empty;

            if (customerID == 0)
                customerIDs = jfsess.AllowedCustoemerIDs();
            else
                customerIDs = customerID.ToString();

            cmdSelectedGroups.Parameters.AddWithValue("@pubID", pub.PubID.ToString());
            cmdSelectedGroups.Parameters.AddWithValue("@customerIDs", customerIDs);
            DataTable dtAvailableGroups = DataFunctions.GetDataTable(cmdSelectedGroups);
            lstSelectedGroups.DataSource = dtAvailableGroups;
            lstSelectedGroups.DataTextField = "GroupName";
            lstSelectedGroups.DataValueField = "GroupID";
            lstSelectedGroups.DataBind();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAvailableGroups(Convert.ToInt32(ddlCustomer.SelectedItem.Value));
            LoadSelectedGroups(Convert.ToInt32(ddlCustomer.SelectedItem.Value));
        }

        protected void btnAddSelectedGroups_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstAvailableGroups, lstSelectedGroups, false);
        }

        protected void btnRemoveSelectedGroups_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstSelectedGroups, lstAvailableGroups, false);
        }

        private void MoveSelectedItems(ListBox source, ListBox target, bool moveAllItems)
        {

            for (int i = source.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = source.Items[i];
                if (item.Selected)
                {
                    target.Items.Add(item);
                    item.Selected = false;
                    source.Items.Remove(item);                    
                }
            }          
        }

        private void SaveAutoSubscriptionGroup()
        {
            string strAutoPubs = string.Empty;

            foreach (ListItem li in lstSelectedGroups.Items)
                strAutoPubs += li.Value + ",";

            SqlCommand cmdAutoSubs = new SqlCommand("spSavePubAutoSubscription");
            cmdAutoSubs.CommandType = CommandType.StoredProcedure;
            cmdAutoSubs.Parameters.AddWithValue("@pubID", pub.PubID.ToString());
            cmdAutoSubs.Parameters.AddWithValue("@pubAutoIDs", strAutoPubs.TrimEnd(','));
            
            if(Convert.ToInt32(ddlCustomer.SelectedItem.Value) == 0)
                cmdAutoSubs.Parameters.AddWithValue("@CustomerIDs", jfsess.AllowedCustoemerIDs());    
            else
                cmdAutoSubs.Parameters.AddWithValue("@CustomerIDs", ddlCustomer.SelectedItem.Value);

            DataFunctions.Execute(cmdAutoSubs);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveAutoSubscriptionGroup();
        }
    }
}