using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Globalization;
using KM.Common;

namespace KMPS.MD.Objects.Dashboard
{
    [Serializable]
    [DataContract]
    public class SubscriberScores
    {
        #region Properties       
        [DataMember]
        public string range { get; set; }
        [DataMember]
        public int subscribercount { get; set; }
        #endregion

        public SubscriberScores()
        {
        }

        #region Data
        public static List<SubscriberScores> Get(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<SubscriberScores> subscriberscores = (List<SubscriberScores>)CacheUtil.GetFromCache(string.Format("SUBSCRIBERSCORES_{0}", brandID), DatabaseName);

                if (subscriberscores == null)
                {
                    subscriberscores = GetData(clientconnection, brandID);

                    CacheUtil.AddToCache(string.Format("SUBSCRIBERSCORES_{0}", brandID), subscriberscores, DatabaseName);
                }

                return subscriberscores;
            }
            else
            {
                return GetData(clientconnection, brandID);
            }
        }

        private static List<SubscriberScores> GetData(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            SubscriberScores sc = null;
            List<SubscriberScores> scList = new List<SubscriberScores>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SubscriberScores", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@brandID", brandID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriberScores> builder = DynamicBuilder<SubscriberScores>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    sc = new SubscriberScores();
                    sc = builder.Build(rdr);
                    scList.Add(sc);
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

            return scList;
        }
        #endregion
    }
}