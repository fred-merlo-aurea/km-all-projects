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
    public partial class FieldSettings : System.Web.UI.Page
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

        private int PFFieldID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PFFieldID"]); 
                }
                catch
                {
                    return 0;
                }
            }

        }

        private int PFID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PFID"]);
                }
                catch
                {
                    return 0;
                }
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlCommand cmdECNField = new SqlCommand("select PFF.PFFieldID,PSF.ECNFieldName from pubformfields pff join PubSubscriptionFields PSF on PFF.PSFieldID = PSF.PSFieldID where PFF.PFID = @PFID and isactive=1 and ControlType not in ('TEXTBOX','HIDDEN') and ECNFieldName not in ('STATE') and PFFieldID <> @PFFieldID");
                cmdECNField.CommandType = CommandType.Text;
                cmdECNField.Parameters.AddWithValue("@PFID",PFID);
                cmdECNField.Parameters.AddWithValue("@PFFieldID", PFFieldID);  
                ddlECNField.DataSource = DataFunctions.GetDataTable(cmdECNField);  
                ddlECNField.DataTextField = "ECNFieldName";
                ddlECNField.DataValueField = "PFFieldID";
                ddlECNField.DataBind();

                try
                {

                    int PFFReferenceID = Convert.ToInt32(DataFunctions.ExecuteScalar("select distinct PFFReferenceID from fieldsettings where PFFieldID =  " + PFFieldID));

                    if (PFFReferenceID > 0)
                    {
                        ddlECNField.ClearSelection();
                        ddlECNField.Items.FindByValue(PFFReferenceID.ToString()).Selected = true; 
                    }
                }
                catch
                { } 
            } 
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string strPFFieldIDs = string.Empty;

            foreach (GridViewRow gvr in grdFieldValues.Rows)
            {
                CheckBox chkSelected = (CheckBox)gvr.FindControl("chkSelected");

                if (chkSelected.Checked)
                    strPFFieldIDs += (strPFFieldIDs == string.Empty ? grdFieldValues.DataKeys[gvr.RowIndex].Value : "," + grdFieldValues.DataKeys[gvr.RowIndex].Value);

            }

            SqlCommand cmddfields = new SqlCommand("sp_SaveFieldSettings"); 
            cmddfields.CommandType = CommandType.StoredProcedure;
            cmddfields.CommandTimeout = 0;

            cmddfields.Parameters.Add(new SqlParameter("@PFFieldID", SqlDbType.Int)).Value = PFFieldID;
            cmddfields.Parameters.Add(new SqlParameter("@PFFReferenceID", SqlDbType.Int)).Value = ddlECNField.SelectedItem.Value; 
            cmddfields.Parameters.Add(new SqlParameter("@datavalue", SqlDbType.VarChar)).Value = strPFFieldIDs;
            cmddfields.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = jfsess.UserName();

            DataFunctions.Execute(cmddfields);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "self.parent.reloadpage();", true); 
        }
    
    }
}



