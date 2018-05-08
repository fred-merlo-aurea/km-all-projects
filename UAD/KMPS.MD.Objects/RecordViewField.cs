using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class RecordViewField
    {
        public RecordViewField() { }

        #region Properties
        [DataMember]
        public int RecordViewFieldID { get; set; }
        [DataMember]
        public int MasterGroupID { get; set; }
        [DataMember]
        public int SubscriptionsExtensionMapperID { get; set; }
        #endregion

        #region Data
        public static List<RecordViewField> getAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<RecordViewField> retList = new List<RecordViewField>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from RecordViewField", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<RecordViewField> builder = DynamicBuilder<RecordViewField>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    RecordViewField x = builder.Build(rdr);
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

        public static List<RecordViewField> getSubscriptionsExtensionMapper(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<RecordViewField> retList = getAll(clientconnection).FindAll(x => x.SubscriptionsExtensionMapperID != 0);
            return retList;
        }

        public static List<RecordViewField> getMasterGroup(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<RecordViewField> retList = getAll(clientconnection).FindAll(x => x.MasterGroupID != 0);
            return retList;
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, RecordViewField rvf)
        {
            SqlCommand cmd = new SqlCommand("e_RecordViewField_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", rvf.MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionsExtensionMapperID", rvf.SubscriptionsExtensionMapperID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection)
        {
            SqlCommand cmd = new SqlCommand("e_RecordViewField_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}
