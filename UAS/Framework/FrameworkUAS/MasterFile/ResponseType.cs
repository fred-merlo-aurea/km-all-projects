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
    public class ResponseType
    {
        /// <summary>
        /// fka ResponseGroup
        /// </summary>
        public ResponseType() { }
        #region Properties
        [DataMember]
        public int ResponseTypeID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public string ResponseTypeName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        #endregion

        public List<Response> ResponseList { get; set; }

        #region CRUD
        public static List<ResponseType> Select(string clientName, int PublicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Object.DBWorker.GetClientSqlConnection(clientName);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText =
                "SELECT ResponseGroupID as 'ResponseTypeID', PubID as 'PublicationID', ResponseGroupName as 'ResponseTypeName', DisplayName FROM ResponseGroups With(NoLock) WHERE PubID = " + PublicationID;

            return GetList(cmd);
        }
        private static List<ResponseType> GetList(SqlCommand cmd)
        {
            List<ResponseType> retList = new List<ResponseType>();

            using (SqlDataReader rdr = ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    ResponseType retItem = new ResponseType();
                    DynamicBuilder<ResponseType> builder = DynamicBuilder<ResponseType>.CreateBuilder(rdr);
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
