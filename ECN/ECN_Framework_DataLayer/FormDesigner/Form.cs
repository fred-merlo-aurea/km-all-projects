using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.FormDesigner
{
    [Serializable]
    public class Form
    {
        public static bool ActiveByGroup(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Form_ActiveByGroup";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString())) > 0 ? true : false;
        }

        public static bool ActiveByGDF(int groupDataFieldsID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Form_ActiveByGDF";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupDataFieldsID", groupDataFieldsID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString())) > 0 ? true : false;
        }
        public static ECN_Framework_Entities.FormDesigner.Form GetByForm_Seq_ID(int BaseChannelID, int Form_Seq_ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Form_Select_Form_Seq_ID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@FormSeqID", Form_Seq_ID);
            return Get(cmd);
        }
        
        public static ECN_Framework_Entities.FormDesigner.Form GetByFormID_NoAccessCheck(int Form_Seq_ID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Form_Select_FormID";
            cmd.Parameters.AddWithValue("@FormSeqID", Form_Seq_ID);
            return Get(cmd);
        }
        private static ECN_Framework_Entities.FormDesigner.Form Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.FormDesigner.Form retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.FormDesigner.Form();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.Form>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
        public static DataSet GetBySearchStringPaging(int BaseChannelID, int CustomerID, string FormType, string FormStatus, string FormName, string SearchCriteria, int Active, int PageNumber, int PageSize, string sortDirection, string sortColumn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetFormsList";
                cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
                if (CustomerID != -1)
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                if (FormType.Trim().Length >0)
                    cmd.Parameters.AddWithValue("@FormType", FormType);
                if (FormStatus.Trim().Length > 0)
                    cmd.Parameters.AddWithValue("@FormStatus", FormStatus);
                if (FormName.Trim().Length > 0)
                    cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@SearchCriteria", SearchCriteria);
                cmd.Parameters.AddWithValue("@Active", Active);
                cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                cmd.Parameters.AddWithValue("@PageSize", PageSize);
                cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
                cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
                return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            }
            catch (Exception e)
            {
                string m = e.Message;
                return new DataSet();
            }

        }
    }
}
