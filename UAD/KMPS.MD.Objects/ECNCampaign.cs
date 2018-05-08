using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class ECNCampaign
    {
        public ECNCampaign() { }

        #region Properties
        [DataMember]
        public int ECNCampaignID { get; set; }
        [DataMember]
        public string ECNCampaignName  { get; set; }
        #endregion

        #region Data
        public static List<ECNCampaign> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<ECNCampaign> ecncampaigns = (List<ECNCampaign>)CacheUtil.GetFromCache("ECNCAMPAIGN", DatabaseName);

                if (ecncampaigns == null)
                {
                    ecncampaigns = GetData(clientconnection);

                    CacheUtil.AddToCache("ECNCAMPAIGN", ecncampaigns, DatabaseName);
                }

                return ecncampaigns;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<ECNCampaign> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<ECNCampaign> retList = new List<ECNCampaign>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from ECNCampaign order by ECNCampaignName", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ECNCampaign> builder = DynamicBuilder<ECNCampaign>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ECNCampaign x = builder.Build(rdr);
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
