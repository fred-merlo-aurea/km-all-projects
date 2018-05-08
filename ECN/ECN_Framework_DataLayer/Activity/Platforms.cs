using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity
{
    [Serializable]
    public class Platforms
    {
        public static List<ECN_Framework_Entities.Activity.Platforms> Get()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return GetData();
            }
            else
            {
                if (System.Web.HttpContext.Current.Cache["PLATFORMS"] == null)
                    System.Web.HttpContext.Current.Cache.Add("PLATFORMS", GetData(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);

                return (List<ECN_Framework_Entities.Activity.Platforms>)System.Web.HttpContext.Current.Cache["PLATFORMS"];

            }

        }

        private static List<ECN_Framework_Entities.Activity.Platforms> GetData()
        {
            ECN_Framework_Entities.Activity.Platforms pform = null;
            List<ECN_Framework_Entities.Activity.Platforms> pformList = new List<ECN_Framework_Entities.Activity.Platforms>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Platforms where PlatformID <>'5'";

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Platforms>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    pform = new ECN_Framework_Entities.Activity.Platforms();
                    pform = builder.Build(rdr);
                    pformList.Add(pform);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return pformList;
        }
    }
}
