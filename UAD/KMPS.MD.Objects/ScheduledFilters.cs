using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace KMPS.MD.Objects
{
    public class ScheduledFilters
    {
        public ScheduledFilters()
        {
            SchedID = null;
            FilterID = null;
        }

        #region Properties
        public int? SchedID { get; set; }
        public int? FilterID { get; set; }
        #endregion

        #region Data
        public static List<ScheduledFilters> GetByScheduleID(int schedID)
        {
            List<ScheduledFilters> retList = new List<ScheduledFilters>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT * FROM ScheduledFilters WHERE SchedID = @SchedID", conn);
            cmd.Parameters.Add(new SqlParameter("@SchedID", schedID));
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                retList = ExcRdrList(rdr);
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

        private static List<ScheduledFilters> ExcRdrList(SqlDataReader rdr)
        {
            List<ScheduledFilters> retlist = new List<ScheduledFilters>();

            #region Reader
            while (rdr.Read())
            {
                ScheduledFilters retItem = new ScheduledFilters();
                int index;
                string name;

                #region Reader

                name = "SchedID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.SchedID = Convert.ToInt32(rdr[index].ToString());

                name = "FilterID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FilterID = Convert.ToInt32(rdr[index].ToString());

                retlist.Add(retItem);

                #endregion
            }
            #endregion

            return retlist;

        }
        #endregion
    }
}