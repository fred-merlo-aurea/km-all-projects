using KM.Common;
using KMPS_JF_Objects.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class NXTBook
    {
        public int NXTBookID { get; set; }
        public int ProductID { get; set; }
        public string APIKey { get; set; }
        
        public string Pubcode { get; set; }
        public string BookID { get; set; }
        public string BookURL { get; set; }
        public string SubscriptionID { get; set; }
        public DateTime? IssueDate { get; set; }

        public bool PostforDigitalSubscriber { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsUnlimited { get; set; }


        public NXTBook()
        { 

        }
        public static bool IsPostforDigitalSubscriber(string Pubcode)
        {

            SqlCommand cmd = new SqlCommand(string.Format("select case when count(NXTBookID) > 0 then 1 else 0 end from NXTBook  with (NOLOCK) where Pubcode = '{0}' and PostforDigitalSubscriber =1 and IsEnabled = 1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            return Convert.ToBoolean(DataFunctions.ExecuteScalar(cmd));

        }

        public static bool IsPostforPrintSubscriber(string Pubcode)
        {

            SqlCommand cmd = new SqlCommand(string.Format("select case when count(NXTBookID) > 0 then 1 else 0 end from NXTBook  with (NOLOCK) where Pubcode = '{0}' and PostforPrintSubscriber =1 and IsEnabled = 1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            return Convert.ToBoolean(DataFunctions.ExecuteScalar(cmd));

        }
        public static bool IsNXTBookAPIEnabled(string Pubcode)
        {

            SqlCommand cmd = new SqlCommand(string.Format("select case when count(NXTBookID) > 0 then 1 else 0 end from NXTBook  with (NOLOCK) where Pubcode = '{0}' and IsEnabled = 1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            return Convert.ToBoolean(DataFunctions.ExecuteScalar(cmd));

        }

        public static NXTBook GetbyProductID(int ProductID)
        {
            NXTBook nxtbook = null;

            if (CacheUtil.IsCacheEnabled())
            {
                nxtbook = (NXTBook) CacheUtil.GetFromCache("NXTBOOK_" + ProductID, "JOINTFORMS");

                if (nxtbook == null)
                {
                    nxtbook = GetData(ProductID);
                    CacheUtil.AddToCache("NXTBOOK_" + ProductID, nxtbook, "JOINTFORMS");
                }

                return nxtbook;
            }
            else
            {
                return GetData(ProductID);
            }
        }

        public static NXTBook GetData(int ProductID)
        {
            NXTBook nxtbook = new NXTBook();

            SqlCommand cmd = new SqlCommand(string.Format("select * from NXTBook  with (NOLOCK) where ProductID = {0} and IsEnabled = 1 ", ProductID));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                nxtbook.NXTBookID = Convert.ToInt32(dr["NXTBookID"]);
                nxtbook.ProductID = Convert.ToInt32(dr["ProductID"]);
                nxtbook.APIKey = dr["APIKey"].ToString();
                nxtbook.Pubcode = dr["Pubcode"].ToString();

                if (!dr.IsNull("BookID"))
                    nxtbook.BookID = dr["BookID"].ToString();

                if (!dr.IsNull("BookURL"))
                    nxtbook.BookURL = dr["BookURL"].ToString();

                nxtbook.SubscriptionID = dr["SubscriptionID"].ToString();

                if (!dr.IsNull("IssueDate"))
                    nxtbook.IssueDate = Convert.ToDateTime(dr["IssueDate"].ToString());
                else
                    nxtbook.IssueDate = null;

                nxtbook.PostforDigitalSubscriber = Convert.ToBoolean(dr["PostforDigitalSubscriber"]);

                nxtbook.IsEnabled = Convert.ToBoolean(dr["IsEnabled"]);
                nxtbook.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                if (!dr.IsNull("UpdatedDate"))
                    nxtbook.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"].ToString());
            }

            return nxtbook;
        }

        public static NXTBook GetbyProductCodeforDigital(string Pubcode)
        {
            NXTBook nxtbook = null;

            SqlCommand cmd = new SqlCommand(string.Format("select * from NXTBook  with (NOLOCK) where Pubcode = '{0}' and IsEnabled = 1 and PostforDigitalSubscriber =1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                nxtbook = new NXTBook();

                nxtbook.NXTBookID = Convert.ToInt32(dr["NXTBookID"]);
                nxtbook.ProductID = Convert.ToInt32(dr["ProductID"]);
                nxtbook.APIKey = dr["APIKey"].ToString();
                nxtbook.Pubcode = dr["Pubcode"].ToString();

                if (!dr.IsNull("BookID"))
                    nxtbook.BookID = dr["BookID"].ToString();

                if (!dr.IsNull("BookURL"))
                    nxtbook.BookURL = dr["BookURL"].ToString();

                nxtbook.SubscriptionID = dr["SubscriptionID"].ToString();
                if(!dr.IsNull("IsUnlimited"))
                {
                    nxtbook.IsUnlimited = (bool)dr["IsUnlimited"];
                }
                else
                {
                    nxtbook.IsUnlimited = false;
                }
                if (!dr.IsNull("IssueDate"))
                    nxtbook.IssueDate = Convert.ToDateTime(dr["IssueDate"].ToString());
                else
                    nxtbook.IssueDate = null;

                nxtbook.PostforDigitalSubscriber = Convert.ToBoolean(dr["PostforDigitalSubscriber"]);

                nxtbook.IsEnabled = Convert.ToBoolean(dr["IsEnabled"]);
                nxtbook.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                if (!dr.IsNull("UpdatedDate"))
                    nxtbook.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"].ToString()); ;

            }

            return nxtbook;
        }

        public static NXTBook GetbyProductCodeforPrint(string Pubcode)
        {
            NXTBook nxtbook = null;

            SqlCommand cmd = new SqlCommand(string.Format("select * from NXTBook  with (NOLOCK) where Pubcode = '{0}' and IsEnabled = 1 and PostforPrintSubscriber =1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            DataTable dt = DataFunctions.GetDataTable(cmd);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                nxtbook = new NXTBook();

                nxtbook.NXTBookID = Convert.ToInt32(dr["NXTBookID"]);
                nxtbook.ProductID = Convert.ToInt32(dr["ProductID"]);
                nxtbook.APIKey = dr["APIKey"].ToString();
                nxtbook.Pubcode = dr["Pubcode"].ToString();

                if (!dr.IsNull("BookID"))
                    nxtbook.BookID = dr["BookID"].ToString();

                if (!dr.IsNull("BookURL"))
                    nxtbook.BookURL = dr["BookURL"].ToString();

                nxtbook.SubscriptionID = dr["SubscriptionID"].ToString();
                if (!dr.IsNull("IsUnlimited"))
                {
                    nxtbook.IsUnlimited = (bool)dr["IsUnlimited"];
                }
                else
                {
                    nxtbook.IsUnlimited = false;
                }
                if (!dr.IsNull("IssueDate"))
                    nxtbook.IssueDate = Convert.ToDateTime(dr["IssueDate"].ToString());
                else
                    nxtbook.IssueDate = null;

                nxtbook.PostforDigitalSubscriber = Convert.ToBoolean(dr["PostforDigitalSubscriber"]);

                nxtbook.IsEnabled = Convert.ToBoolean(dr["IsEnabled"]);
                nxtbook.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                if (!dr.IsNull("UpdatedDate"))
                    nxtbook.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"].ToString()); ;

            }

            return nxtbook;
        }

        public static DateTime GetRecentIssueDatebyPubcode(string Pubcode)
        {

            SqlCommand cmd = new SqlCommand(string.Format("select max(IssueDate) from NXTBook  with (NOLOCK) where Pubcode = '{0}' and IsEnabled = 1 ", Pubcode));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;

            var issuedate = DataFunctions.ExecuteScalar(cmd);

            if (!issuedate.Equals(DBNull.Value))
            {
                return (DateTime)issuedate;
            }
            else
                throw new Exception("NXTBook.GetRecentIssueDatebyPubcode - Issue Date is null.");
        }
    }
}