using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
using System.Configuration;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class RegionGroup
    {
        public RegionGroup() { }

        #region Properties
        [DataMember]
        public int RegionGroupID { get; set; }
        [DataMember]
        public string RegionGroupName { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion

        #region Data
        public static List<RegionGroup> GetAll()
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<RegionGroup> RegionGroups = (List<RegionGroup>)CacheUtil.GetFromCache("UL_REGIONGROUP", DatabaseName);

                if (RegionGroups == null)
                {
                    RegionGroups = GetData();

                    CacheUtil.AddToCache("UL_RegionGroup", RegionGroups, DatabaseName);
                }

                return RegionGroups;
            }
            else
            {
                return GetData();
            }
        }

        private static List<RegionGroup> GetData()
        {
            List<RegionGroup> retList = new List<RegionGroup>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UAD_Lookup"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select RegionGroupID, RegionGroupName from RegionGroup  with (nolock) order by RegionGroupName asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<RegionGroup> builder = DynamicBuilder<RegionGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    RegionGroup x = builder.Build(rdr);

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
