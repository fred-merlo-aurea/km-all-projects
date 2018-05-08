using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common.Functions;

namespace KM.Common.Entity
{
    [Serializable]
    [DataContract]
    public class Application
    {
        public Application()
        {

        }

        #region Properties
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string FromEmailAddress { get; set; }
        #endregion

        public static Application GetByApplicationID(int applicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Application_Select_ApplicationID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            return Get(cmd);
        }

        public static Application GetByApplicationName(string applicationName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Application_Select_ApplicationName";
            cmd.Parameters.AddWithValue("@ApplicationName", applicationName);
            return Get(cmd);
        }
        public static Application GetByApplicationName(Applications applicationName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Application_Select_ApplicationName";
            cmd.Parameters.AddWithValue("@ApplicationName", applicationName.ToString());
            return Get(cmd);
        }

        public static List<Application> GetByUserID(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Application_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            return GetList(cmd);
        }

        private static List<Application> GetList(SqlCommand cmd)
        {
            List<Application> retList = new List<Application>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Application retItem = new Application();
                    var builder = DynamicBuilder<Application>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                }
            }

            return retList;
        }

        private static Application Get(SqlCommand cmd)
        {
            Application retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    retItem = new Application();
                    var builder = DynamicBuilder<Application>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                }
            }

            return retItem;
        }

        public enum Applications
        {
            ECN_Communicator = 1,
            ECN_Blast_Engine = 2,
            ECN_Activity_Engine = 3,
            GoodDriver_Android = 7,
            GoodDonor_PC = 8,
            GoodDonor_Tools = 9,
            CallCenterImport_Apogee = 10,
            CallCenterImport_Savers = 11,
            AddressValidator = 12,
            CarrierRouteUpdate = 13,
            DMailOrderActual = 14,
            GarageSaleScrape = 15,
            GD_ECN_Sync = 16,
            GeoCodeDonorAddress = 17,
            RouteOptimization = 18,
            GD_Services = 19,
            GoodDonor_Admin_Site = 20,
            GoodDonor_Public_Site = 21,
            Charity_Public_Site = 22,
            Mobile_GoodDonor = 23,
            Mobile_Public = 24,
            GoodDonor_DataMine = 25,
            ECN_FBL = 26,
            ECN_Web_Services = 28,
            MAF_Web_Services = 43
        }
    }
}
