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
using ecn.common.classes;
using System.Data.SqlClient;
using System.Configuration;

namespace ecn.accounts.main.reports
{
    public partial class channelreport : ECN_Framework.WebPageHelper
    {
        private DataSet ds = new DataSet();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;

            if (!IsPostBack)
            {
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                     ddlCustomerType.DataSource = ECN_Framework_BusinessLayer.Accounts.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.CustomerType, Master.UserSession.CurrentUser);
                    ddlCustomerType.DataBind();

                    ddlCustomerType.Items.Insert(0, new ListItem("----- Select Customer Type -----", ""));
                    ddlCustomerType_SelectedIndexChanged(sender, e);
                }
                else
                    Response.Redirect("/ecn.accounts/main/default.aspx");
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
            this.rptCustomerType.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptCustomerType_ItemDataBound);

        }
        #endregion

        protected void ddlCustomerType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadDs();
            rptCustomerType.DataSource = Distinct(ds.Tables[0], "CustomerType");
            rptCustomerType.DataBind();

        }

        private void LoadDs()
        {
            string connString = ConfigurationManager.AppSettings["connString"];

            SqlConnection dbConn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("rpt_CustomerUsagebyType", dbConn);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CustomerType", SqlDbType.VarChar));
            cmd.Parameters["@CustomerType"].Value = ddlCustomerType.SelectedValue;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dbConn.Open();
            da.Fill(ds);
            dbConn.Close();
        }

        private DataView BindChannel(string CustomerType)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "CustomerType='" + CustomerType + "'";
            dv.Sort = "BaseChannelName asc";
            return dv;
        }

        private DataView BindCustomer(string CustomerType, string ChannelID)
        {
            DataView dv = ds.Tables[1].DefaultView;
            dv.RowFilter = "CustomerType='" + CustomerType + "' and BaseChannelID='" + ChannelID + "'";
            dv.Sort = "CustomerName asc";
            return dv;
        }

        public DataTable Distinct(DataTable dt, string DistinctColumn)
        {
            DataTable dtclone = dt.Clone();
            DataView dv = dt.DefaultView;
            dv.Sort = DistinctColumn;
            string SelOld = string.Empty;
            for (int i = 0; i <= dv.Count - 1; i++)
            {
                if (SelOld != dv[i][DistinctColumn].ToString())
                {
                    DataRow drn = dtclone.NewRow();
                    for (int y = 0; y <= drn.ItemArray.Length - 1; y++)
                    {
                        drn[y] = dv[i][y];
                    }
                    SelOld = dv[i][DistinctColumn].ToString();
                    dtclone.Rows.Add(drn);
                }
            }
            return dtclone;
        }

        public void dlChannels_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            DataList dlChannels = (DataList)source;

            if (e.CommandName == "Select")
                dlChannels.SelectedIndex = e.Item.ItemIndex;
            else if (e.CommandName == "UnSelect")
                dlChannels.SelectedIndex = -1;

            Label lblcType = (Label)e.Item.FindControl("lblcType");

            LoadDs();
            dlChannels.DataSource = BindChannel(lblcType.Text);
            dlChannels.DataBind();
        }

        private void rptCustomerType_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblType = (Label)e.Item.FindControl("lblType");

                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "CustomerType='" + lblType.Text + "'";
                dv.Sort = "BaseChannelName asc";

                DataList dlChannels = (DataList)e.Item.FindControl("dlChannels");

                dlChannels.DataSource = dv;
                dlChannels.DataBind();
            }
        }

        public void dlChannels_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.SelectedItem)
            {
                DataList dlChannels = (DataList)e.Item.Parent;
                Label lblcType = (Label)e.Item.FindControl("lblcType");

                DataGrid dgCustomers = (DataGrid)e.Item.FindControl("dgCustomers");
                dgCustomers.DataSource = BindCustomer(lblcType.Text, dlChannels.DataKeys[e.Item.ItemIndex].ToString());
                dgCustomers.DataBind();
            }
        }

    }
}
