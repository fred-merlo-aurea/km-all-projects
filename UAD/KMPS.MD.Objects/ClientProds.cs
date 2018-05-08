using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Globalization;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class ClientProds
    {
        #region Properties       
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public int SubscriberCount { get; set; }
        #endregion

        public ClientProds()
        {
        }

        #region Data
        public static List<ClientProds> Get(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID1, int PubTypeID2, int PubTypeID3)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<ClientProds> ClientProds = (List<ClientProds>)CacheUtil.GetFromCache("CLIENTPROD", DatabaseName);

                if (ClientProds == null)
                {
                    ClientProds = GetData(clientconnection, PubTypeID1, PubTypeID2, PubTypeID3);

                    CacheUtil.AddToCache("CLIENTPROD", ClientProds, DatabaseName);
                }

                return ClientProds;
            }
            else
            {
                return GetData(clientconnection, PubTypeID1, PubTypeID2, PubTypeID3);
            }
        }

        private static List<ClientProds> GetData(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID1, int PubTypeID2, int PubTypeID3)
        {
            ClientProds cp = null;
            List<ClientProds> cpList = new List<ClientProds>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_ClientProd", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PubTypeID1", PubTypeID1);
            cmd.Parameters.AddWithValue("@PubTypeID2", PubTypeID2);
            cmd.Parameters.AddWithValue("@PubTypeID3", PubTypeID3);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ClientProds> builder = DynamicBuilder<ClientProds>.CreateBuilder(rdr);
                
                while (rdr.Read())
                {
                    cp = new ClientProds();
                    cp = builder.Build(rdr);
                    cpList.Add(cp);
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

            return cpList;
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("CLIENTPROD", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("CLIENTPROD", DatabaseName);
                }
            }
        }
        #endregion
    }
}