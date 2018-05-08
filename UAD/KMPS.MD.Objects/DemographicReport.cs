using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class DemographicReport
    {
        #region Properties
        public int ID { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
        public string Desc1 { get; set; }
        public int Count { get; set; }
        public int Mail { get; set; }
        public int Email { get; set; }
        public int Phone { get; set; }
        public int Mail_Phone { get; set; }
        public int Email_Phone { get; set; }
        public int Mail_Email { get; set; }
        public int All_Records { get; set; }
        #endregion

        #region Data
        public static List<DemographicReport> GetMasterDemographicData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int masterGroup, string description, int brandID, string filters, bool isRecencyView )
        {
            if (System.Web.HttpContext.Current == null)
            {
                return GetData(clientconnection, queries, masterGroup, description, brandID, isRecencyView);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session[filters + "MasterDemographicData" + masterGroup] == null)
                    System.Web.HttpContext.Current.Session.Add(filters + "MasterDemographicData" + masterGroup, GetData(clientconnection, queries, masterGroup, description, brandID, isRecencyView));

                return (List<DemographicReport>)System.Web.HttpContext.Current.Session[filters + "MasterDemographicData" + masterGroup];
            }
        }

        public static List<DemographicReport> GetData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int masterGroup, string description, int brandID, bool isRecencyView)
        {
            return GetDataInternal(clientconnection, "sp_Subscriber_MasterCodesheet_counts", queries, masterGroup, description, brandID, isRecencyView);           
        }

        public static List<DemographicReport> GetMasterDemographicDataWithPermission(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int masterGroup, string description, int brandID, string filters, bool isRecencyView)
        {
            if (System.Web.HttpContext.Current == null)
            {
                return GetDataWithPermission(clientconnection, queries, masterGroup, description, brandID, isRecencyView);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session[filters + "MasterDemographicDataWithPermission" + masterGroup] == null)
                    System.Web.HttpContext.Current.Session.Add(filters + "MasterDemographicDataWithPermission" + masterGroup, GetDataWithPermission(clientconnection, queries, masterGroup, description, brandID, isRecencyView));

                return (List<DemographicReport>)System.Web.HttpContext.Current.Session[filters + "MasterDemographicDataWithPermission" + masterGroup];
            }
        }

        public static List<DemographicReport> GetDataWithPermission(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int masterGroup, string description, int brandID, bool isRecencyView)
        {                       
            return GetDataInternal(clientconnection, "sp_Subscriber_MasterCodesheet_counts_With_Permissions", queries, masterGroup, description, brandID, isRecencyView);
        }

        public static List<DemographicReport> GetProductDemographicData(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int responseGroupID, string description, string filters, int pubID)
        {
            if (System.Web.HttpContext.Current == null)
            {
                return Get(clientconnection, queries, responseGroupID, description, pubID);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session[filters + "ProductDemographicData" + responseGroupID] == null)
                    System.Web.HttpContext.Current.Session.Add(filters + "ProductDemographicData" + responseGroupID, Get(clientconnection, queries, responseGroupID, description, pubID));

                return (List<DemographicReport>)System.Web.HttpContext.Current.Session[filters + "ProductDemographicData" + responseGroupID];
            }
        }

        public static List<DemographicReport> Get(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int responseGroupID, string description, int pubID)
        {
            return GetInternal(
               clientconnection,
               "sp_Subscriber_Codesheet_counts",
               queries,
               responseGroupID,
               description,
               pubID);      
        }

        public static List<DemographicReport> GetProductDemographicDataWithPermission(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int responseGroupID, string description, string filters, int pubID)
        {
            if (System.Web.HttpContext.Current == null)
            {
                return GetWithPermission(clientconnection, queries, responseGroupID, description, pubID);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session[filters + "ProductDemographicDataWithPermission" + responseGroupID] == null)
                    System.Web.HttpContext.Current.Session.Add(filters + "ProductDemographicDataWithPermission" + responseGroupID, GetWithPermission(clientconnection, queries, responseGroupID, description, pubID));

                return (List<DemographicReport>)System.Web.HttpContext.Current.Session[filters + "ProductDemographicDataWithPermission" + responseGroupID];
            }
        }

        public static List<DemographicReport> GetWithPermission(KMPlatform.Object.ClientConnections clientconnection, StringBuilder queries, int responseGroupID, string description, int pubID)
        {
            return GetInternal(
                clientconnection,
                "sp_Subscriber_Codesheet_counts_With_Permissions", 
                queries, 
                responseGroupID,
                description,
                pubID);
        }

        private static List<DemographicReport> GetInternal(
            KMPlatform.Object.ClientConnections clientconnection, 
            string storedProcedure, 
            StringBuilder queries,
            int responseGroupID,
            string description, 
            int pubID)
        {
            var connection = DataFunctions.GetClientSqlConnection(clientconnection);
            var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
            command.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = queries.ToString();
            command.Parameters.Add(new SqlParameter("@ResponseGroupID", SqlDbType.Int)).Value = responseGroupID;
            command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = description;
            command.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = pubID;
            
            return FetchDemographicsReports(command);
        }

        private static List<DemographicReport> GetDataInternal(
            KMPlatform.Object.ClientConnections clientconnection, 
            string storedProcedure, 
            StringBuilder queries,
            int masterGroup,
            string description, 
            int brandID, 
            bool isRecencyView)
        {
            var conn = DataFunctions.GetClientSqlConnection(clientconnection);
            var command = new SqlCommand(storedProcedure, conn)
            {
                CommandTimeout = 0,
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = queries.ToString();
            command.Parameters.Add(new SqlParameter("@masterGroup", SqlDbType.Int)).Value = masterGroup;
            command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = description;
            command.Parameters.Add(new SqlParameter("@BrandID", SqlDbType.Int)).Value = brandID;
            command.Parameters.Add(new SqlParameter("@IsRecencyView", SqlDbType.Bit)).Value = isRecencyView;
            
            return FetchDemographicsReports(command);
        }

        private static List<DemographicReport> FetchDemographicsReports(SqlCommand command)
        {
            var returnList = new List<DemographicReport>();
            try
            {
                command.Connection.Open();
                var reader = command.ExecuteReader();
                var builder = DynamicBuilder<DemographicReport>.CreateBuilder(reader);

                while (reader.Read())
                {
                    returnList.Add(builder.Build(reader));
                }
            }
            finally
            {
                command.Connection.Close();
            }

            return returnList;
        }
        #endregion
    }
}
