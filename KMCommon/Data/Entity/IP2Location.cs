using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class IP2Location
    {
        public IP2Location()
        {
            IPStart = -1;
            IPEnd = -1;
            Region = string.Empty;
            Country = string.Empty;
            State = string.Empty;
            City = string.Empty;
            Lat = string.Empty;
            Long = string.Empty;
            Zip = string.Empty;
            TimeZone = string.Empty;
        }
        #region Properties
        [DataMember]
        public long IPStart { get; set; }
        [DataMember]
        public long IPEnd { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Lat { get; set; }
        [DataMember]
        public string Long { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string TimeZone { get; set; }
        #endregion

        //public static IP2Location GetByIP(string IP)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_IP2Location_Select_ByIP";
        //    cmd.Parameters.AddWithValue("@IP", Convert.ToInt64(IP.Replace(".", "")));
        //    return Get(cmd);
        //}

        public static IP2Location GetByIP(long IP)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IP2Location_Select_ByIP";
            cmd.Parameters.AddWithValue("@IP", IP);
            return Get(cmd);
        }

        private static IP2Location Get(SqlCommand cmd)
        {
            IP2Location retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    retItem = new IP2Location();
                    var builder = DynamicBuilder<IP2Location>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                }
            }

            return retItem;
        }
    }
}
