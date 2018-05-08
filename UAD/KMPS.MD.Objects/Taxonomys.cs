using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Taxonomys
    {
        #region Properties
        [DataMember]
        public int masterID { get; set; }
        [DataMember]
        public string masterdesc { get; set; }
        [DataMember]
        public int subscriptions { get; set; }
        [DataMember]
        public int monthdt { get; set; }
        [DataMember]
        public int yeardt { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public DateTime MonthFirstDate { get; set; }
        #endregion

        public Taxonomys() { }

        #region Data
        public static List<Taxonomys> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Taxonomys> taxonomys = (List<Taxonomys>)CacheUtil.GetFromCache("TAXONOMYS", DatabaseName);

                if (taxonomys == null)
                {
                    taxonomys = GetData(clientconnection);

                    CacheUtil.AddToCache("TAXONOMYS", taxonomys, DatabaseName);
                }

                return taxonomys;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<Taxonomys> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            Taxonomys co = null;
            List<Taxonomys> coList = new List<Taxonomys>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_Taxonomys", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Taxonomys> builder = DynamicBuilder<Taxonomys>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    co = new Taxonomys();
                    co = builder.Build(rdr);
                    co.Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(Int32.Parse(rdr[4].ToString())) + " " + rdr[5].ToString();
                    co.MonthFirstDate = new DateTime(Int32.Parse(rdr[5].ToString()), Int32.Parse(rdr[4].ToString()), 1);
                    coList.Add(co);
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
            return coList;
        }
        #endregion
    }
}