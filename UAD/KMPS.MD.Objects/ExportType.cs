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
    public class ExportType
    {
        public ExportType() { }

        #region Properties
        [DataMember]
        public int ExportTypeID { get; set; }
        [DataMember]
        public string Type { get; set; }
        #endregion

        #region Data
        public static List<ExportType> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<ExportType> retList = new List<ExportType>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_ExportType_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ExportType> builder = DynamicBuilder<ExportType>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    ExportType x = builder.Build(rdr);
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