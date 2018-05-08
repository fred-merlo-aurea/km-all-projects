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
    public class ResponseOptions
    {
        private static ECN_Framework_Entities.Collector.ResponseOptions Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.ResponseOptions retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.ResponseOptions();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.ResponseOptions>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.ResponseOptions> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Collector.ResponseOptions> retList = new List<ECN_Framework_Entities.Collector.ResponseOptions>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.ResponseOptions retItem = new ECN_Framework_Entities.Collector.ResponseOptions();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.ResponseOptions>.CreateBuilder(rdr);
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
        
        public static List<ECN_Framework_Entities.Collector.ResponseOptions> GetByQuestionID(int QuestionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from response_options with (NOLOCK) where QuestionID =@QuestionID";
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            return GetList(cmd);
        }
    }
}
