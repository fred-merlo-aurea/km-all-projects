using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Reports
    {
        private static ECN_Framework_Entities.Communicator.Reports Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Reports retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Reports();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Reports>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.Reports> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Reports> retList = new List<ECN_Framework_Entities.Communicator.Reports>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Reports retItem = new ECN_Framework_Entities.Communicator.Reports();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Reports>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static List<ECN_Framework_Entities.Communicator.Reports> Get()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Select";
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Reports GetByReportID(int ReportID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Select_ReportID";
            cmd.Parameters.AddWithValue("@ReportID", ReportID);
            return Get(cmd);
        }
        public static ECN_Framework_Entities.Communicator.Reports GetByReportName(string reportName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Reports_Select_ReportName";
            cmd.Parameters.AddWithValue("@ReportName", reportName);
            return Get(cmd);
        }


    }
}
