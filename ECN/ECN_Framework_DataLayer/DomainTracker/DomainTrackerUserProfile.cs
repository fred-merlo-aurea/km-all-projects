using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerUserProfile
    {
        public static ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile GetByEmailAddress(string EmailAddress, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerUserProfile_GetByEmailAddress";
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            return Get(cmd);
        }

        public static bool Exists(string EmailAddress, int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerUserProfile_Exists";
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", EmailAddress));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile domainTrackerUserProfile)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerUserProfile_Save";
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", domainTrackerUserProfile.EmailAddress));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", domainTrackerUserProfile.BaseChannelID));
            if (domainTrackerUserProfile.ProfileID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)domainTrackerUserProfile.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)domainTrackerUserProfile.CreatedUserID ?? DBNull.Value));

            cmd.Parameters.AddWithValue("@IsKnown", domainTrackerUserProfile.IsKnown);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()).ToString());
        }

        private static ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (!retItem.IsKnown.HasValue)
                            retItem.IsKnown = true;
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> retList = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.DomainTracker.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile retItem = new ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile();
                    var builder = DynamicBuilder<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            if (!retItem.IsKnown.HasValue)
                                retItem.IsKnown = true;
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> GetByDomainTrackerID(int domainTrackerID, int? CurrentPage, int? PageSize, string StartDate, string EndDate, string Email, string TypeFilter, string sortColumn, string sortDirection, string PageUrl)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerUserProfile_Select_DomainTrackerID";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            if (CurrentPage.HasValue)
                cmd.Parameters.AddWithValue("@CurrentPage", CurrentPage);
            if (PageSize.HasValue)
                cmd.Parameters.AddWithValue("@PageSize", PageSize);
            if (!String.IsNullOrEmpty(StartDate))
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
            if (!String.IsNullOrEmpty(EndDate))
                cmd.Parameters.AddWithValue("@EndDate", plusOneDayToDate(EndDate));
            if (!String.IsNullOrEmpty(Email))
                cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@TypeFilter", TypeFilter);
            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
            if (!String.IsNullOrEmpty(PageUrl))
                cmd.Parameters.AddWithValue("@PageUrl", PageUrl);

            return GetList(cmd);
        }

        public static DataTable GetByDomainTrackerID_Paging(int domainTrackerID, int? CurrentPage, int? PageSize, string StartDate, string EndDate, string Email)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainTrackerUserProfile_Select_Paging";
            cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
            if (CurrentPage.HasValue)
                cmd.Parameters.AddWithValue("@CurrentPage", CurrentPage);
            if (PageSize.HasValue)
                cmd.Parameters.AddWithValue("@PageSize", PageSize);
            if (!String.IsNullOrEmpty(StartDate))
                cmd.Parameters.AddWithValue("@StartDate", StartDate);
            if (!String.IsNullOrEmpty(EndDate))
                cmd.Parameters.AddWithValue("@EndDate", plusOneDayToDate(EndDate));
            if (!String.IsNullOrEmpty(Email))
                cmd.Parameters.AddWithValue("@Email", Email);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.DomainTracker.ToString());
        }

        private static string plusOneDayToDate(string date)
        {
            DateTime myDate = DateTime.Parse(date);
            myDate = myDate.AddDays(1);
            return myDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        //public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> GetUserProfilesByEmailaddress(int domainTrackerID, string emailaddress)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_DomainTrackerUserProfile_Select_DomainTrackerID_ByEmail";
        //    cmd.Parameters.AddWithValue("@DomainTrackerID", domainTrackerID);
        //    cmd.Parameters.AddWithValue("@Email", emailaddress);
        //    return GetList(cmd);
        //}
    }
}
