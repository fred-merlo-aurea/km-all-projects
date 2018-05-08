using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class KMLogoClickReport
    {
        public KMLogoClickReport() { }
        #region Properties
        public string BaseChannelName {get;set;}
		public string CustomerName {get;set;} 
		public string EmailSubject {get;set;} 
		public DateTime SendTime {get;set;} 
		public string FirstName {get;set;}
		public string LastName {get;set;}
		public string EmailAddress {get;set;}
        public string Voice { get; set; }
        #endregion
        #region Data
        //rpt_KMLogoClickReport
        //@fromdt varchar(10),
	//@todt varchar(10)
        public static List<KMLogoClickReport> Get(DateTime fromDate, DateTime toDate)
        {
            List<KMLogoClickReport> retList = new List<KMLogoClickReport>();
            string sqlQuery = "rpt_KMLogoClickReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdt", fromDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@todt", toDate.ToShortDateString());

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<KMLogoClickReport>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        KMLogoClickReport x = builder.Build(rdr);
                        retList.Add(x);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
        #endregion
    }
}
