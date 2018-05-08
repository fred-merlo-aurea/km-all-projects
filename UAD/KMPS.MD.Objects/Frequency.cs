using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace KMPS.MD.Objects
{
    [Serializable]
    public class Frequency
    {
        #region Properties
        public int FrequencyID { get; set; }
        public string FrequencyName { get; set; }
        public int Issues { get; set; }
        #endregion

        #region Data
        public static List<Frequency> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Frequency> frequency = (List<Frequency>)CacheUtil.GetFromCache("FREQUENCY", DatabaseName);

                if (frequency == null)
                {
                    frequency = GetData(clientconnection);

                    CacheUtil.AddToCache("FREQUENCY", frequency, DatabaseName);
                }

                return frequency;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<Frequency> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Frequency> retList = new List<Frequency>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from Frequency", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Frequency> builder = DynamicBuilder<Frequency>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Frequency x = builder.Build(rdr);
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
