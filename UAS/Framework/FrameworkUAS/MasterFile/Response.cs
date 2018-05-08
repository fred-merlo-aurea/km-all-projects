using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FrameworkUAS.DataAccess;
using KM.Common;

namespace FrameworkUAS.MasterFile
{
    [Serializable]
    [DataContract]
    public class Response
    {
        public Response() { }
        #region Properties
        [DataMember]
        public int ResponseID { get; set; }
        [DataMember]
        public int ResponseTypeID { get; set; }//fka ResponseGroup
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseTypeName { get; set; }
        #endregion

        #region CRUD
        public static List<Response> Select(string clientName, int PublicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Object.DBWorker.GetClientSqlConnection(clientName);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CodeSheetID as 'ResponseID', Responsevalue as 'ResponseCode', ResponseGroupID as 'ResponseTypeID', PubID as 'PublicationID', ResponseGroup as 'ResponseTypeName' FROM CodeSheet With(NoLock) WHERE PubID = " + PublicationID.ToString();

            return GetList(cmd);
        }
        private static List<Response> GetList(SqlCommand cmd)
        {
            List<Response> retList = new List<Response>();

            using (SqlDataReader rdr = ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Response retItem = new Response();
                    DynamicBuilder<Response> builder = DynamicBuilder<Response>.CreateBuilder(rdr);
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
        public static SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            cmd.CommandTimeout = 0;
            cmd.Connection.Open();

            SqlDataReader rdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (rdr != null && !rdr.HasRows)
                rdr = null;

            return rdr;
        }
        #endregion
    }
}
