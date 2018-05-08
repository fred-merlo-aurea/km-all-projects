using ecn.common.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Diag = System.Diagnostics;

namespace KM.Framework.Web.WebForms.EmailProfile
{
    public abstract class EmailProfileUDFHistoryBaseControl : EmailProfileBaseControl
    {
        protected abstract DataGrid gridUDFHistoryData { get; }

        protected string GetDataFieldSetName(string dataFieldSetId)
        {
            var dataFieldSetName = string.Empty;
            try
            {
                var sql = string.Format("SELECT Name FROM DataFieldSets WHERE DataFieldSetID = {0}", dataFieldSetId);
                dataFieldSetName = DataFunctions.ExecuteScalar(sql).ToString();
            }
            catch
            {
                ShowMessageLabel("<br>ERROR: DataFieldSetID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            }
            return dataFieldSetName;
        }

        protected string GetCustomerId(string groupId)
        {
            var customerId = string.Empty;
            try
            {
                var sql = string.Format("SELECT CustomerID FROM Groups WHERE GroupID = {0}", groupId);
                customerId = DataFunctions.ExecuteScalar(sql).ToString();
            }
            catch
            {
                customerId = "0";
                ShowMessageLabel(string.Format("<br>ERROR: CustomerID does not Exist for the GroupID:{0}. Please click on the 'Profile' link in the email message that you received", groupId));
            }
            return customerId;
        }

        protected void LoadUDFHistoryData(string emailId, string groupId, string dataFieldSetId, string customerId)
        {
            var dbConnection = new SqlConnection(DataFunctions.connStr);
            var udfDataListCommand = new SqlCommand("sp_GetUDFDataValues", dbConnection);
            udfDataListCommand.CommandTimeout = 100;
            udfDataListCommand.CommandType = CommandType.StoredProcedure;

            //--GroupID
            int numericGroupId;
            int.TryParse(groupId, out numericGroupId);
            udfDataListCommand.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            udfDataListCommand.Parameters["@GroupID"].Value = numericGroupId;

            //--CustomerID
            int numericCustomerId;
            int.TryParse(customerId, out numericCustomerId);
            udfDataListCommand.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            udfDataListCommand.Parameters["@CustomerID"].Value = numericCustomerId;

            //--UDF EmailID
            udfDataListCommand.Parameters.Add(new SqlParameter("@UDFEmailID", SqlDbType.VarChar));
            udfDataListCommand.Parameters["@UDFEmailID"].Value = emailId;

            //--DataFieldSetID
            int numericDataFieldSetId;
            int.TryParse(dataFieldSetId, out numericDataFieldSetId);
            udfDataListCommand.Parameters.Add(new SqlParameter("@DatafieldSetID", SqlDbType.Int));
            udfDataListCommand.Parameters["@DatafieldSetID"].Value = numericDataFieldSetId;

            var dataAdapter = new SqlDataAdapter(udfDataListCommand);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "sp_GetUDFDataValues");
            dbConnection.Close();

            try
            {
                var historyData = dataSet.Tables[0];
                historyData.Columns.Remove("EmailID");
                gridUDFHistoryData.DataSource = historyData.DefaultView;
                gridUDFHistoryData.DataBind();
            }
            catch (Exception ex)
            {
                Diag.Trace.TraceError("Error occured while fetching UDF History Data\nError Message: {0}", ex.Message);
            }
        }
    }
}
