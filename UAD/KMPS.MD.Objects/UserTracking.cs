using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class UserTracking
    {
        public UserTracking() { }

        #region Properties
        [DataMember]
        public int UserTrackingID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public DateTime ActivityDateTime { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string BrowserInfo { get; set; }
        [DataMember]
        public int PlatformID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public Enums.Platforms Platform { get; set; }
        [DataMember]
        public Enums.Clients Client { get; set; }
        [DataMember]
        public string UserName { get; set; }
        #endregion

        #region Data
        public static List<UserTracking> GetByDate(KMPlatform.Object.ClientConnections clientconnection, string StartDate, string EndDate)
        {
            List<UserTracking> retList = new List<UserTracking>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_UserTracking_Select_ByDate", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", StartDate);
            cmd.Parameters.AddWithValue("@ToDate", EndDate);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<UserTracking> builder = DynamicBuilder<UserTracking>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    UserTracking x = builder.Build(rdr);
                    x.Client = (Enums.Clients)x.ClientID;
                    x.Platform = (Enums.Platforms)x.PlatformID;
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

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, UserTracking u)
        {
            SqlCommand cmd = new SqlCommand("e_UserTracking_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", u.UserID));
            cmd.Parameters.Add(new SqlParameter("@Activity", u.Activity));
            cmd.Parameters.Add(new SqlParameter("@IPAddress", u.IPAddress));
            cmd.Parameters.Add(new SqlParameter("@BrowserInfo", u.BrowserInfo));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion
    }
}
