using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class RecurrenceType
    {
        public RecurrenceType() { }

        #region Properties
        [DataMember]
        public int RecurrenceTypeID { get; set; }
        [DataMember]
        public string Type { get; set; }
        #endregion

        #region Data
        public static List<RecurrenceType> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<RecurrenceType> retList = new List<RecurrenceType>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_RecurrenceType_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<RecurrenceType> builder = DynamicBuilder<RecurrenceType>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    RecurrenceType x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }
        #endregion
    }
}