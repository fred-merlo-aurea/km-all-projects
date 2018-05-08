using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class AudienceEngagementReport
    {
        public AudienceEngagementReport() { }

        #region Properties
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string SubscriberType { get; set; }
        [DataMember]
        public int Counts { get; set; }
        [DataMember]
        public decimal Percents { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion

        #region Data
        public static List<AudienceEngagementReport> Get(int GroupID, int clickpercentage, int days, string download, string downloadType)
        {
            List<AudienceEngagementReport> retList = new List<AudienceEngagementReport>();

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnActivity"].ConnectionString);
            SqlCommand cmd = new SqlCommand("spAudienceEngagementReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            cmd.Parameters.AddWithValue("@ClickPercentage", clickpercentage);
            cmd.Parameters.AddWithValue("@Days", days);
            cmd.Parameters.AddWithValue("@Download", download);
            cmd.Parameters.AddWithValue("@DownloadType", downloadType);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<AudienceEngagementReport> builder = DynamicBuilder<AudienceEngagementReport>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    AudienceEngagementReport x = builder.Build(rdr);
                    x.Percents = Math.Round(decimal.Parse(rdr["Percents"].ToString()),2);
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