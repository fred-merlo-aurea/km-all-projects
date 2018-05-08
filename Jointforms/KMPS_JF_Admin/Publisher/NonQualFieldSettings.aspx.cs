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
    public partial class NonQualFieldSettings : System.Web.UI.Page
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
            try
            {
                if (!IsPostBack)
                {
                    SqlCommand cmdgrdFieldValuesNonQual = new SqlCommand("select psfd.DataText, psfd.DataValue, case when psfd.datavalue = fs.datavalue then 'Y' else 'N' end as 'IsSelected' from PubSubscriptionFieldData psfd join pubformfields pff on pff.PSFieldID = psfd.PSFieldID left outer join NonQualSettings fs on fs.PFFieldID = pff.PFFieldID and pff.PFID=@PFID and fs.datavalue = psfd.datavalue where PFF.PFFieldID=@PFFieldID"); 
                    cmdgrdFieldValuesNonQual.CommandType = CommandType.Text;
                    cmdgrdFieldValuesNonQual.Parameters.Add(new SqlParameter("@PFID", PFID));  
                    cmdgrdFieldValuesNonQual.Parameters.Add(new SqlParameter("@PFFieldID", PFFieldID)); 
                    grdFieldValuesNonQual.DataSource = DataFunctions.GetDataTable(cmdgrdFieldValuesNonQual); 
                    grdFieldValuesNonQual.DataBind();
                }
            }
            catch { } 
          
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string strPFFieldIDs = string.Empty;

            foreach (GridViewRow gvr in grdFieldValuesNonQual.Rows)
            {
                CheckBox chkNonQualSelected = (CheckBox)gvr.FindControl("chkNonQualSelected");

                if (chkNonQualSelected.Checked)
                    strPFFieldIDs += (strPFFieldIDs == string.Empty ? grdFieldValuesNonQual.DataKeys[gvr.RowIndex].Value : "," + grdFieldValuesNonQual.DataKeys[gvr.RowIndex].Value);

            }

            SqlCommand cmddfields = new SqlCommand("sp_SaveNonQualFieldSettings"); 
            cmddfields.CommandType = CommandType.StoredProcedure;
            cmddfields.CommandTimeout = 0;

            cmddfields.Parameters.Add(new SqlParameter("@PFFieldID", SqlDbType.Int)).Value = PFFieldID;
            cmddfields.Parameters.Add(new SqlParameter("@datavalue", SqlDbType.VarChar)).Value = strPFFieldIDs;
            cmddfields.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = jfsess.UserName();

            DataFunctions.Execute(cmddfields);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "self.parent.reloadpage();", true);
        } 
    }
}
