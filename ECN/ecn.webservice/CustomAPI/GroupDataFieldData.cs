using System;
using System.Collections.Generic;
using System.Collections; 
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ecn.webservice.CustomAPI
{
    public class GroupDataFieldData
    {
        /// <summary>
        /// Get list of GroupDataField objects by SessionECN.sessionCustomer.customerID
        /// </summary>
        /// <returns>List of GroupDataField</returns>
        public static List<GroupDataField> GetData(int groupID)
        {
            List<GroupDataField> retList = new List<GroupDataField>();
            string sqlQuery = "SELECT * FROM GroupDatafields WHERE GroupID = " + groupID;
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection); 
            retList = ExcRdrList(rdr);
            conn.Close();
            return retList;
        }
        
        public static GroupDataField GetGroupDataFieldByGroupDataFieldsID(int groupDataFieldID, int groupID)  
        {
            GroupDataField retItem = new GroupDataField();
            List<GroupDataField> list = GetData(groupID);
            retItem = list.SingleOrDefault(x => x.GroupDataFieldsID == groupDataFieldID); 
            return retItem;
        }

        private static List<GroupDataField> ExcRdrList(SqlDataReader rdr)
        {
            List<GroupDataField> retList = new List<GroupDataField>(); 
            #region Reader
            while (rdr.Read())
            {
                GroupDataField retItem = new GroupDataField();
                int index;
                string name;

                name = "GroupDataFieldsID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.GroupDataFieldsID = Convert.ToInt32(rdr[index]);

                name = "ShortName";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.ShortName = Convert.ToString(rdr[index]).ToUpper();

                name = "LongName";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.LongName = Convert.ToString(rdr[index]);


                retList.Add(retItem);
            }
            #endregion

            return retList;
        }
        #region CRUD
        public static int Insert(GroupDataField gdf)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GroupDatafieldsInsert";
            cmd.Parameters.AddWithValue("@GroupID", gdf.GroupID);
            cmd.Parameters.AddWithValue("@ShortName", gdf.ShortName);
            cmd.Parameters.AddWithValue("@LongName", gdf.LongName);
            cmd.Parameters.AddWithValue("@SurveyID", gdf.SurveyID);  
            if(gdf.DataFieldSetID == null)
                cmd.Parameters.AddWithValue("@DatafieldSetID", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@DatafieldSetID", gdf.DataFieldSetID);
            cmd.Parameters.AddWithValue("@IsPublic", gdf.IsPublic);
            cmd.Parameters.AddWithValue("@IsPrimaryKey", gdf.IsPrimaryKey);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, conn));
        }
        #endregion

    }
}