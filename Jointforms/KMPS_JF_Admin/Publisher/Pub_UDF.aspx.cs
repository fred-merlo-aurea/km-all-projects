using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient; 

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_UDF : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

        private int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubId"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private int GroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["GroupID"]);
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BoxPanel2.Title = "Manage UDFs for " + Request.QueryString["PubName"] + ":";

                    SqlCommand cmdGroupID = new SqlCommand("select ECNDefaultGroupID from publications where PubID = @PubID");  
                    cmdGroupID.CommandType = CommandType.Text; 
                    cmdGroupID.Parameters.Add(new SqlParameter("@PubID",PubID.ToString()));  
                    GroupID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmdGroupID));      

                    SqlCommand cmddrpUDFs = new SqlCommand("select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupID = " + ConfigurationManager.AppSettings["DefaultUDFGroupID"].ToString() + " and IsDeleted=0 order by shortname");  
                    cmddrpUDFs.CommandType = CommandType.Text;                     
                    drpUDFs.DataSource = DataFunctions.GetDataTable("communicator", cmddrpUDFs); 
                    drpUDFs.DataTextField = "Shortname"; 
                    drpUDFs.DataValueField = "groupdatafieldsID";  
                    drpUDFs.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                btnAddUDF.Visible = false;
            }
        }

        protected void grdPublisherUDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "UDFEdit")
                {
                    drpUDFs.Enabled = false;

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    hfldGDFId.Value = e.CommandArgument.ToString();

                    txtShortName.Text = row.Cells[1].Text.Replace("%", "");
                    txtLongName.Text = row.Cells[0].Text;
                    chkIsPublic.Checked = (row.Cells[2].Text == "Y" ? true : false);
                    BoxPanel1.Title = "Edit UDF";
                    btnAddUDF.Text = "SAVE";

                }
                else if (e.CommandName == "UDFDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    hfldGDFId.Value = e.CommandArgument.ToString();

                    SqlCommand cmdUDFDelete = new SqlCommand("update groupdatafields set IsDeleted=1 where groupdatafieldsID = @hfldGDFId");
                    cmdUDFDelete.Parameters.Add(new SqlParameter("@hfldGDFId", hfldGDFId.Value));
                    DataFunctions.ExecuteScalar("communicator", cmdUDFDelete); 
                    ClearData();
                    grdPublisherUDF.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnAddUDF_Click(object sender, EventArgs e)
        {
            try
            {
                ECNUpdate.SaveUDF(hfldGDFId.Value == "" ? 0 : Convert.ToInt32(hfldGDFId.Value), GroupID, txtShortName.Text, txtLongName.Text, chkIsPublic.Checked?true:false);
                ClearData();
                grdPublisherUDF.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            drpUDFs.Enabled = true;
            ClearData();
        }
        private void ClearData()
        {
            hfldGDFId.Value = "";
            txtShortName.Text = "";
            txtLongName.Text = "";
            chkIsPublic.Checked = false;
            BoxPanel1.Title = "Add UDF";
            btnAddUDF.Text = "SAVE";
            drpUDFs.Enabled = true;
            drpUDFs.ClearSelection();
        }

        protected void SqlDataSourceGDF_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@GroupID"].Value = GroupID;
        }

        protected void drpUDFs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpUDFs.SelectedIndex > -1)
            {
                SqlCommand cmddrpUDFs = new SqlCommand("select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupdatafieldsID = @groupdatafieldsID and IsDeleted=0");
                cmddrpUDFs.CommandType = CommandType.Text;
                cmddrpUDFs.Parameters.Add(new SqlParameter("@groupdatafieldsID", drpUDFs.SelectedItem.Value)); 
                DataTable dt = DataFunctions.GetDataTable("communicator",cmddrpUDFs);  

                if (dt.Rows.Count > 0)
                {
                    txtShortName.Text = dt.Rows[0]["ShortName"].ToString();
                    txtLongName.Text = dt.Rows[0]["LongName"].ToString();
                    chkIsPublic.Checked = dt.Rows[0]["IsPublic"].ToString() == "Y" ? true : false;
                }
            }
            else
            {
                ClearData();
            }

        }
    }
}
