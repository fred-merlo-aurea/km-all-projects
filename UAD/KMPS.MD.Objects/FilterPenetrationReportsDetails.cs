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
    [Serializable]
    [DataContract]
    public class FilterPenetrationReportsDetails
    {
        public FilterPenetrationReportsDetails() { }

        #region Properties
        [DataMember]
        public int FilterPenetrationReportsDetailsID { get; set; }
        [DataMember]
        public int ReportID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        #endregion

        #region Data
        public static List<FilterPenetrationReportsDetails> getByFilterID(KMPlatform.Object.ClientConnections clientconnection, int ReportID)
        {
            List<FilterPenetrationReportsDetails> retList = new List<FilterPenetrationReportsDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterPenetrationReportsDetails where ReportID = @ReportID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@ReportID", ReportID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<FilterPenetrationReportsDetails> builder = DynamicBuilder<FilterPenetrationReportsDetails>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterPenetrationReportsDetails x = builder.Build(rdr);

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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterPenetrationReportsDetails fprd)
        {
            SqlCommand cmd = new SqlCommand("e_FilterPenetrationReportsDetails_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportID", fprd.ReportID);
            cmd.Parameters.AddWithValue("@FilterID", fprd.FilterID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion
    }
}
