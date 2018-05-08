using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.SqlTypes;
using System.Text;
using KMPS.MD.Objects;

namespace KMPS.MD.Administration
{
    public partial class PublicationsSetupCopy : KMPS.MD.Main.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Product Setup Copy";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            lblMessage.Text = "";
            SqlDataSourcePub.ConnectionString = DataFunctions.GetClientSqlConnection(Master.clientconnections).ConnectionString;
            SqlDataSourcePub2.ConnectionString = DataFunctions.GetClientSqlConnection(Master.clientconnections).ConnectionString;
        }

        //protected void SqlDataSourcePubs_DataBound(object sender, EventArgs e)
        //{
                 
        //}

        //protected void ddlFrom_SelectedIndexChanged(object sender, EventArgs e)
        //{
            
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SqlCommand cmdCopy = null;
            int fromPubID = 0;
            int toPubID = 0;
            try
            {
                fromPubID = Convert.ToInt32(ddlFrom.SelectedValue);
                toPubID = Convert.ToInt32(ddlTo.SelectedValue);
                cmdCopy = new SqlCommand("spCopyPubCodes", DataFunctions.GetClientSqlConnection(Master.clientconnections));
                cmdCopy.CommandType = CommandType.StoredProcedure;
                cmdCopy.Parameters.AddWithValue("@FromPubID", fromPubID);
                cmdCopy.Parameters.AddWithValue("@ToPubID", toPubID);
                cmdCopy.Parameters.AddWithValue("@UserID", Master.LoggedInUser);
                cmdCopy.Connection.Open();
                cmdCopy.ExecuteNonQuery();
                lblMessage.Text = "Copied product info from " + ddlFrom.SelectedItem.ToString() + " to " + ddlTo.SelectedItem.ToString();
            }
            catch (Exception Ex)
            {
                lblErrorMessage.Text = Ex.Message;
                divError.Visible = true;
            }
            finally
            {
                cmdCopy.Connection.Close();
            }
            
        }
    }
}