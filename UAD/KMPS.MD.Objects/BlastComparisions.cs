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
    public class BlastComparisions
    {
        public BlastComparisions() { }

        #region Properties
        [DataMember]
        public string ActionTypeCode { get; set; }
        [DataMember]
        public string BlastID { get; set; }
        [DataMember]
        public int DistinctCount { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public int TotalSent { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public float Perc { get; set; }
        #endregion

        #region Data
        public static List<BlastComparisions> GetData(string xmlgroups, int blasts)
        {
            List<BlastComparisions> bcList = new List<BlastComparisions>();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ecnActivity"].ConnectionString);
            BlastComparisions bcobject = null;
            SqlCommand cmd = new SqlCommand("sp_GetBlastComparision_Group", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupsIDs", xmlgroups);
            cmd.Parameters.AddWithValue("@blastsnum", blasts);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<BlastComparisions> builder = DynamicBuilder<BlastComparisions>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    bcobject = new BlastComparisions();
                    bcobject = builder.Build(rdr);
                    bcList.Add(bcobject);
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
            return bcList;
        }
        #endregion
    }
}