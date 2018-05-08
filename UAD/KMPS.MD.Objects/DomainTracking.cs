using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class DomainTracking
    {
        public DomainTracking() { }
        #region Properties
        public int DomainTrackingID { get; set; }
        public string DomainName { get; set; }
        #endregion
        
        public static List<DomainTracking> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<DomainTracking> domaintacking = (List<DomainTracking>)CacheUtil.GetFromCache("DomainTracking", DatabaseName);

                if (domaintacking == null)
                {
                    domaintacking = GetData(clientconnection);

                    CacheUtil.AddToCache("DomainTracking", domaintacking, DatabaseName);
                }

                return domaintacking;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        #region Data
        private static List<DomainTracking> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<DomainTracking> retList = new List<DomainTracking>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from DomainTracking", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<DomainTracking> builder = DynamicBuilder<DomainTracking>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    DomainTracking x = builder.Build(rdr);
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