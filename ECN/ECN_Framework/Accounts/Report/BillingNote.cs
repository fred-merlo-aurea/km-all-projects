using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class BillingNote
    {
        public BillingNote() { }
        #region Properties
        public int BaseChannelID {get;set;}
        public string BaseChannelName  {get;set;}
        public int CustomerID {get;set;}
        public string CustomerName {get;set;}
        public string Notes {get;set;}
        public string UpdatedBy {get;set;}
        public DateTime DateUpdated { get; set; }
        #endregion
        #region Data
        private static List<BillingNote> GetCache()
        {
            List<BillingNote> retList = new List<BillingNote>();
            if (System.Web.HttpContext.Current.Cache["BillingNote"] == null)
            {

                retList = GetAllFromDataBase();
                System.Web.HttpContext.Current.Cache.Add("BillingNote", retList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1440), System.Web.Caching.CacheItemPriority.Normal, null);
                return retList;
            }
            else
            {
                retList = (List<BillingNote>)System.Web.HttpContext.Current.Cache.Get("BillingNote");
                return retList;
            }
        }

        private static List<BillingNote> GetAllFromDataBase()
        {
            List<BillingNote> retList = new List<BillingNote>();
            string sqlQuery = "rpt_BillingNotes";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<BillingNote>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        BillingNote x = builder.Build(rdr);
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
        public static List<BillingNote> GetAll()
        {
            return GetCache().ToList();
        }
        public static List<BillingNote> GetForCustomer(int customerID)
        {
            return GetCache().Where(x => x.CustomerID == customerID).ToList();
        }
        #endregion
    }
}
