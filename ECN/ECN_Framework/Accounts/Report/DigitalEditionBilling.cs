using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class DigitalEditionBilling
    {
        public DigitalEditionBilling() { }
        #region Properties
        public int BaseChannelID {get;set;}
        public string BaseChannelName {get;set;}
        public int CustomerID {get;set;}
        public string CustomerName {get;set;}
        public int PublicationID {get;set;}
        public string PublicationName {get;set;}
        public int EditionID {get;set;} 
        public string EditionName {get;set;}
        public string Status {get;set;}
        public int Pages {get;set;} 
		public string ActivatedDate {get;set;}
        public string ArchievedDate { get; set; }
        public string DeactivatedDate { get; set; }
        #endregion
        #region Data
        public static List<DigitalEditionBilling> Get(int month, int year)
        {
            List<DigitalEditionBilling> retList = new List<DigitalEditionBilling>();
            string sqlQuery = "rpt_DigitalEditionBilling";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<DigitalEditionBilling>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        DigitalEditionBilling x = builder.Build(rdr);
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
