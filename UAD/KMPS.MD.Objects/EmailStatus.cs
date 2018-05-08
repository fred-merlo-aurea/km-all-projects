using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class EmailStatus
    {
        public EmailStatus() { }

        #region Properties
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public string Status { get; set; }
        #endregion

        #region Data
        public static List<EmailStatus> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<EmailStatus> emailstatus = (List<EmailStatus>)CacheUtil.GetFromCache("EmailStatus", DatabaseName);

                if (emailstatus == null)
                {
                    emailstatus = GetData(clientconnection);

                    CacheUtil.AddToCache("EmailStatus", emailstatus, DatabaseName);
                }

                return emailstatus;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<EmailStatus> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<EmailStatus> retList = new List<EmailStatus>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from EmailStatus with (nolock) order by status", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<EmailStatus> builder = DynamicBuilder<EmailStatus>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    EmailStatus x = builder.Build(rdr);
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