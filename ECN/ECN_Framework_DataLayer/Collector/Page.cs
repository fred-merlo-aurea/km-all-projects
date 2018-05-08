using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Collector
{
    [Serializable]
    [DataContract]
    public class Page
    {
        public static int Save(ECN_Framework_Entities.Collector.Page item)
        {
            SqlCommand cmd = new SqlCommand("sp_SaveSurveyPages");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = item.PageID;

            cmd.Parameters.Add("@SurveyID", SqlDbType.Int);
            cmd.Parameters["@SurveyID"].Value = item.SurveyID;

            cmd.Parameters.Add("@PageHeader", SqlDbType.VarChar);
            cmd.Parameters["@PageHeader"].Value = item.PageHeader;

            cmd.Parameters.Add("@PageDesc", SqlDbType.VarChar);
            cmd.Parameters["@PageDesc"].Value = item.PageDesc;

            cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
            cmd.Parameters["@PageNumber"].Value = item.Number;

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()).ToString());

        }

        public static List<ECN_Framework_Entities.Collector.Page> GetBySurveyID(int SurveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Page with (NOLOCK)  where SurveyID=@SurveyID order by number";
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Collector.Page Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.Page retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.Page();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Page>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.Page> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Collector.Page> retList = new List<ECN_Framework_Entities.Collector.Page>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.Page retItem = new ECN_Framework_Entities.Collector.Page();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Page>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Reorder(int PageID, string Position, int ToPageID)
        {
            SqlCommand cmd = new SqlCommand("sp_ReOrderPage");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;

            cmd.Parameters.Add("@Position", SqlDbType.Char);
            cmd.Parameters["@Position"].Value = Position;

            cmd.Parameters.Add("@ToPageID", SqlDbType.Int);
            cmd.Parameters["@ToPageID"].Value = ToPageID;
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static ECN_Framework_Entities.Collector.Page GetByPageID(int PageID)
        {
            SqlCommand cmd = new SqlCommand("select * from Page with (NOLOCK) where PageID=@PageID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;
            return Get(cmd);
        }

        public static void Delete(int PageID)
        {
            SqlCommand cmd = new SqlCommand("sp_DeletePage");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static bool BranchFromExists(int PageID)
        {
            SqlCommand cmd = new SqlCommand("if exists (select top 1 SurveyID FROM Surveys WITH (NOLOCK) " +
                              "WHERE CustomerID = @CustomerID and SurveyName=@SurveyName and SurveyID<> @SurveyID) select 1 else select 0");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static bool BranchToExists(int PageID)
        {
            SqlCommand cmd = new SqlCommand("if exists (select top 1 PageID from SurveyBranching WITH (NOLOCK) where PageID=@PageID) select 1 else select 0");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }
    }
}
