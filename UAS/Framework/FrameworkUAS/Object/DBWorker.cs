using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAS.Entity;
using KM.Common;
using KMPlatformDataFunctions = KMPlatform.DataAccess.DataFunctions;

namespace FrameworkUAS.Object
{
    public class DBWorker
    {
        private static string getPubIDAndCodesByClient = "SELECT PubID, PubCode FROM pubs";

        public static Dictionary<int, string> GetPubIDAndCodesByClient(string clientName)
        {
            var table = DataFunctions.GetDataTable(getPubIDAndCodesByClient, GetClientSqlConnection(clientName));

            Dictionary<int, string> pubData = new Dictionary<int, string>();
            foreach (DataRow dr in table.Rows)
            {
                pubData.Add(Convert.ToInt32(dr["PubID"]), Convert.ToString(dr["PubCode"]).Trim().ToUpper());
            }
            return pubData;
        }
        public static Dictionary<int, string> GetPubIDAndCodesByClient(KMPlatform.Entity.Client client)
        {
            var dt = DataFunctions.GetDataTable(getPubIDAndCodesByClient, GetClientSqlConnection(client));

            Dictionary<int, string> pubData = new Dictionary<int, string>();
            foreach (DataRow dr in dt.Rows)
            {
                pubData.Add(Convert.ToInt32(dr["PubID"]), Convert.ToString(dr["PubCode"].ToString().Trim().ToUpper()));
            }
            return pubData;
        }
        public static Dictionary<int, string> GetPubIDAndCodesByClient(KMPlatform.Object.ClientConnections client)
        {
            var dt = DataFunctions.GetDataTable(getPubIDAndCodesByClient, GetClientSqlConnection(client));

            Dictionary<int, string> pubData = new Dictionary<int, string>();
            foreach (DataRow dr in dt.Rows)
            {
                pubData.Add(Convert.ToInt32(dr["PubID"]), Convert.ToString(dr["PubCode"].ToString().Trim().ToUpper()));
            }
            return pubData;
        }
        public static int Save(int transformationId, int transformationTypeId, string transformationName, string transformationDescription, int clientId, int userID, bool isMapsPubCode = false, bool isLastStep = false)
        {
            FrameworkUAS.Entity.Transformation transformation = new Transformation();
            transformation.TransformationID = transformationId;
            transformation.TransformationTypeID = transformationTypeId;
            transformation.TransformationName = transformationName;
            transformation.TransformationDescription = transformationDescription;
            transformation.ClientID = clientId;
            transformation.IsActive = true;
            transformation.MapsPubCode = isMapsPubCode;
            transformation.LastStepDataMap = isLastStep;
            transformation.DateCreated = DateTime.Now;
            transformation.DateUpdated = DateTime.Now;
            transformation.CreatedByUserID = userID;
            transformation.UpdatedByUserID = userID;

            FrameworkUAS.BusinessLogic.Transformation tran = new BusinessLogic.Transformation();
            int res = tran.Save(transformation);

            return res;
        }

        public static SqlConnection GetClientSqlConnection(string clientName)
        {
            KMPlatform.BusinessLogic.Client cb = new KMPlatform.BusinessLogic.Client();
            List<KMPlatform.Entity.Client> allConnections = cb.Select();
            bool isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);
            bool isNetworkDeployed = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsNetworkDeployed"].ToString(), out isNetworkDeployed);
            string dbConn = string.Empty;

            if (isDemo == false)
            {
                dbConn = (from c in allConnections
                          where c.FtpFolder == clientName
                          select c.ClientLiveDBConnectionString).Single().ToString();
            }
            else
            {
                dbConn = (from c in allConnections
                          where c.FtpFolder == clientName
                          select c.ClientTestDBConnectionString).Single().ToString();
            }

            if (isNetworkDeployed == true)
                dbConn = dbConn.Replace("216.17", "10.10");


            return new SqlConnection(dbConn);
        }
        public static SqlConnection GetClientSqlConnection(KMPlatform.Entity.Client client)
        {
            var connection = KMPlatformDataFunctions.GetClientSqlConnection(client);
            return connection;
        }
        public static SqlConnection GetClientSqlConnection(KMPlatform.Object.ClientConnections client)
        {
            var connection = KMPlatformDataFunctions.GetClientSqlConnection(client);
            return connection;
        }
    }
}
