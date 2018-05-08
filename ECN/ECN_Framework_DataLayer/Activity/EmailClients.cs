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
    public class EmailClients
    {
        public static List<ECN_Framework_Entities.Activity.EmailClients> Get()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return GetData();
            }
            else
            {
                if (System.Web.HttpContext.Current.Cache["EMAILCLIENTS"] == null)
                    System.Web.HttpContext.Current.Cache.Add("EMAILCLIENTS", GetData(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);

                return (List<ECN_Framework_Entities.Activity.EmailClients>)System.Web.HttpContext.Current.Cache["EMAILCLIENTS"];

            }

        }

        private static List<ECN_Framework_Entities.Activity.EmailClients> GetData()
        {
            ECN_Framework_Entities.Activity.EmailClients ec = null;
            List<ECN_Framework_Entities.Activity.EmailClients> ecList = new List<ECN_Framework_Entities.Activity.EmailClients>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Emailclients where EmailClientID <> '15'";

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.EmailClients>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    ec = new ECN_Framework_Entities.Activity.EmailClients();
                    ec = builder.Build(rdr);
                    ecList.Add(ec);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return ecList;
        }
    }
}
