using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Newsletter_UDF : System.Web.UI.Page
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

        private int NewsLetterID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["NewsLetterID"]);
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
                    btnCancel.Visible = false;

                    SqlCommand cmdNewsLetter = new SqlCommand("select DisplayName, ECNGroupID, ECNDefaultGroupID from PubNewsletters pn join Publications p on pn.pubID = p.pubID where pn.PubID = @PubID and NewsletterID = @NewsLetterID");
                    cmdNewsLetter.CommandType = CommandType.Text;  
                    cmdNewsLetter.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));
                    cmdNewsLetter.Parameters.Add(new SqlParameter("@NewsLetterID", NewsLetterID.ToString()));
                    DataTable dt = DataFunctions.GetDataTable(cmdNewsLetter); 

                    if (dt.Rows.Count > 0)
                    {
                        BoxPanel2.Title = "Manage UDFs for " + dt.Rows[0]["DisplayName"].ToString() + " Newsletter:";
                        GroupID = Convert.ToInt32(dt.Rows[0]["ECNGroupID"].ToString());

                        SqlCommand cmddrpPubUDFs = new SqlCommand("select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupID = @groupID  and IsDeleted=0 order by shortname");                        
                        cmddrpPubUDFs.Parameters.Add(new SqlParameter("@groupID",dt.Rows[0]["ECNDefaultGroupID"].ToString()));                          
                        drpPubUDFs.DataSource = DataFunctions.GetDataTable("communicator", cmddrpPubUDFs); 
                        drpPubUDFs.DataTextField = "Shortname"; 
                        drpPubUDFs.DataValueField = "groupdatafieldsID"; 
                        drpPubUDFs.DataBind();

                    }
                    else
                    {
                        lblMessage.Text = "Invalid Newsletter ID";
                        btnAddUDF.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdNewsletterUDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "UDFEdit")
                {
                    drpPubUDFs.Enabled = false;
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    hfldGDFId.Value = e.CommandArgument.ToString();

                    txtShortName.Text = row.Cells[1].Text.Replace("%", "");
                    txtLongName.Text = row.Cells[0].Text;
                    chkIsPublic.Checked = (row.Cells[2].Text == "Y" ? true : false);
                    BoxPanel1.Title = "Edit UDF";
                    btnAddUDF.Text = "UPDATE";
                    btnCancel.Visible = true;

                }
                else if (e.CommandName == "UDFDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    hfldGDFId.Value = e.CommandArgument.ToString();
                    SqlCommand delGrpf = new SqlCommand("Delete from groupdatafields where groupdatafieldsID=@hfldGDFId and IsDeleted=0 ");
                    delGrpf.CommandType = CommandType.Text; 
                    delGrpf.Parameters.Add(new SqlParameter("@hfldGDFId",hfldGDFId.Value));                     
                    DataFunctions.ExecuteScalar("communicator", delGrpf); 
                    ClearData();

                    grdNewsletterUDF.DataBind();
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
                grdNewsletterUDF.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            drpPubUDFs.Enabled = true;
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
            drpPubUDFs.Enabled = true;
            drpPubUDFs.ClearSelection();
        }

        protected void SqlDataSourceGDF_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@GroupID"].Value = GroupID;
        }

        protected void drpPubUDFs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpPubUDFs.SelectedIndex > -1)  
            {
                SqlCommand cmddrpPubUDFs = new SqlCommand("select groupdatafieldsID, ShortName, LongName, IsPublic from groupdatafields where groupdatafieldsID = @pubudfs and IsDeleted=0 ");   
                cmddrpPubUDFs.Parameters.AddWithValue("@pubudfs",drpPubUDFs.SelectedItem.Value);                
                DataTable dt = DataFunctions.GetDataTable("communicator", cmddrpPubUDFs);   

                if (dt.Rows.Count > 0)
                {
                    txtShortName.Text =  dt.Rows[0]["ShortName"].ToString();
                    txtLongName.Text =  dt.Rows[0]["LongName"].ToString();
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
