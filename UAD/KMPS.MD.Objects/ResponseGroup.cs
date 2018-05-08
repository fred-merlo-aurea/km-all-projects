using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;
using KMPlatform.Object;
using UADUtilities = FrameworkUAD.DataAccess.Utilities;

namespace KMPS.MD.Objects
{
    public class ResponseGroup
    {
        private const string ResponseGroupValidateDeleteOrInactiveCommandText = "e_ResponseGroup_Validate_DeleteorInActive";
        private const string ResponseGroupDeleteCommandText = "sp_ResponseGroups_Delete";
        private const string ResponseGroupIdKey = "@ResponseGroupId";
        private const string ProductIdKey = "@PubID";
        private const string CacheKeyResponseGroupPrefix = "RESPONSEGROUP_";
        private const string CacheKeyAllResponseGroupPrefix = "ALLRESPONSEGROUP_";

        private const string GetDataCommandText = "select ResponseGroupID, PubID, ResponseGroupName, DisplayName, DateCreated, DateUpdated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, isnull(IsActive, 1) as IsActive, WQT_ResponseGroupID, ResponseGroupTypeID from ResponseGroups where  pubID = @PubID and ISNULL(ResponseGroupTypeId,0) not in (select c.codeID from UAD_Lookup..CodeType ct join UAD_Lookup..Code c on ct.CodeTypeId = c.CodeTypeId where ct.CodeTypeName = 'Response Group'  and c.CodeName = 'Circ Only' ) order by ResponseGroupName";

        private const string PubIdParameterName = "@PubID";

        private const string GetAllDataCommandText = "select ResponseGroupID, PubID, ResponseGroupName, DisplayName, DateCreated, DateUpdated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, isnull(IsActive, 1) as IsActive, WQT_ResponseGroupID, ResponseGroupTypeID from ResponseGroups where  pubID = @PubID order by ResponseGroupName";

        #region Properties
        public int ResponseGroupID { get; set; }
        public int PubID { get; set; }
        public string ResponseGroupName { get; set; }
        public string DisplayName { get; set; }
        public bool IsMultipleValue { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int ResponseGroupTypeID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        #endregion

        #region Data
        public static bool ExistsByIDNamePubID(KMPlatform.Object.ClientConnections clientconnection, int PubID, string ResponseGroupName, int ResponseGroupID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * from ResponseGroups where PubID = @PubID and ResponseGroupName = @ResponseGroupName and ResponseGroupID <> @ResponseGroupID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupName", ResponseGroupName));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", ResponseGroupID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByName(KMPlatform.Object.ClientConnections clientconnection, string ResponseGroupName)
        {
            SqlCommand cmd = new SqlCommand("SELECT * from ResponseGroups where ResponseGroupName = @ResponseGroupName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupName", ResponseGroupName));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static List<ResponseGroup> GetByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<ResponseGroup> responsegroups = (List<ResponseGroup>)CacheUtil.GetFromCache("RESPONSEGROUP_" + pubID, DatabaseName);

                if (responsegroups == null)
                {
                    responsegroups = GetData(clientconnection, pubID).ToList();

                    CacheUtil.AddToCache("RESPONSEGROUP_" + pubID, responsegroups, DatabaseName);
                }

                return responsegroups;
            }
            else
            {
                return GetData(clientconnection, pubID).ToList();
            }
        }

        private static IList<ResponseGroup> GetData(ClientConnections clientConnections, int productId)
        {
            return GetResponseGroupsFromCommandText(clientConnections, productId, GetDataCommandText);
        }

        private static IList<ResponseGroup> GetResponseGroupsFromCommandText(ClientConnections clientConnections, int productId, string commandText)
        {
            List<ResponseGroup> retList;
            using (var sqlConnection = DataFunctions.GetClientSqlConnection(clientConnections))
            {
                using (var sqlCommand = DataFunctions.CreateTextSqlCommand(
                    commandText,
                    sqlConnection,
                    new[] { new SqlParameter(PubIdParameterName, productId) }))
                {
                    sqlConnection.Open();
                    using (var dataReader = sqlCommand.ExecuteReader())
                    {
                        retList = CreateResponseListFromBuilder(dataReader).ToList();
                    }
                }
            }

            return retList;
        }

        private static IEnumerable<ResponseGroup> CreateResponseListFromBuilder(IDataReader dataReader)
        {
            var retList = new List<ResponseGroup>();
            var builder = DynamicBuilder<ResponseGroup>.CreateBuilder(dataReader);
            while (dataReader.Read())
            {
                var responseGroup = builder.Build(dataReader);
                retList.Add(responseGroup);
            }

            return retList;
        }

        public static IList<ResponseGroup> GetAllByPubID(ClientConnections clientConnections, int productId)
        {
            if (!CacheUtil.IsCacheEnabled())
            {
                return GetAllData(clientConnections, productId);
            }

            var databaseName = DataFunctions.GetDBName(clientConnections);

            return CacheUtil.GetFromCache(
                CacheKeyAllResponseGroup(productId),
                databaseName, 
                () => GetAllData(clientConnections, productId));
        }

        private static IList<ResponseGroup> GetAllData(ClientConnections clientConnections, int productId)
        {
            return GetResponseGroupsFromCommandText(clientConnections, productId, GetAllDataCommandText);
        }

        public static List<ResponseGroup> GetActiveByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            var responseGroupList = GetByPubID(clientconnection, pubID);

            var Query = (from r in responseGroupList
                                    where r.IsActive == true 
                                    select r);
            return Query.ToList();
        }

        public static ResponseGroup GetByResponseGroupID(KMPlatform.Object.ClientConnections clientconnection, int responseGroupID)
        {
            ResponseGroup retItem = new ResponseGroup();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from ResponseGroups where ResponseGroupID = @ResponseGroupID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ResponseGroup> builder = DynamicBuilder<ResponseGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
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

            return retItem;
        }

        public static List<ResponseGroup> GetAcitiveByPubIDForFilterSchedule(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            List<ResponseGroup> retList = new List<ResponseGroup>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select ResponseGroupID, PubID, ResponseGroupName, DisplayName, DateCreated, DateUpdated, CreatedByUserID, DisplayOrder, IsMultipleValue, IsRequired, isnull(IsActive, 1) as IsActive, WQT_ResponseGroupID, ResponseGroupTypeID from ResponseGroups where  pubID = @PubID order by ResponseGroupName", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ResponseGroup> builder = DynamicBuilder<ResponseGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ResponseGroup x = builder.Build(rdr);
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

        public static void DeleteCache(ClientConnections clientConnections, int productId)
        {
            if (!CacheUtil.IsCacheEnabled())
            {
                return;
            }

            var databaseName = DataFunctions.GetDBName(clientConnections);

            CacheUtil.SafeRemoveFromCache(CacheKeyResponseGroup(productId), databaseName);
            CacheUtil.SafeRemoveFromCache(CacheKeyAllResponseGroup(productId), databaseName);
        }

        private static string CacheKeyAllResponseGroup(int productId)
        {
            return $"{CacheKeyAllResponseGroupPrefix}{productId}";
        }

        private static string CacheKeyResponseGroup(int productId)
        {
            return $"{CacheKeyResponseGroupPrefix}{productId}";
        }

        #endregion

        #region CRUD

        public static int Save(KMPlatform.Object.ClientConnections clientconnection, ResponseGroup cs)
        {
            DeleteCache(clientconnection, cs.PubID);

            SqlCommand cmd = new SqlCommand("sp_ResponseGroups_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", cs.ResponseGroupID));
            cmd.Parameters.Add(new SqlParameter("@PubID", cs.PubID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupName", cs.ResponseGroupName));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", cs.DisplayName));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)cs.CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)cs.DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)cs.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)cs.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsMultipleValue", (object)cs.IsMultipleValue ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsRequired", (object)cs.IsRequired ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)cs.IsActive ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupTypeID", cs.ResponseGroupTypeID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Copy(KMPlatform.Object.ClientConnections clientconnection, int srcResponseGroupID, string destPubsXML, string destPubIDs, int userID)
        {
            SqlCommand cmd = new SqlCommand("spCopyResponseGroups");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@srcResponseGroupID", srcResponseGroupID));
            cmd.Parameters.Add(new SqlParameter("@destPubsXML", destPubsXML));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));

            CodeSheet.DeleteCache(clientconnection, srcResponseGroupID);

            string[] str = destPubIDs.Split(',');

            foreach (string s in str)
            {
                DeleteCache(clientconnection, Convert.ToInt32(s));
            }

            Pubs.DeleteCache(clientconnection);
        }

        public static NameValueCollection ValidationForDeleteorInActive(ClientConnections clientConnections, int responseGroupId, int productId)
        {
            var result = UADUtilities.ValidateDeleteOrInActive(
                clientConnections,
                ResponseGroupValidateDeleteOrInactiveCommandText,
                GetCommandParameter(responseGroupId, productId),
                FilterExportScheduleProcessor.ProcessDataReader);

            return result;
        }

        public static void Delete(ClientConnections clientConnections, int responseGroupId, int productId)
        {
            DeleteCache(clientConnections, productId);
            UADUtilities.Delete(
                clientConnections, 
                ResponseGroupDeleteCommandText, 
                new[] { new SqlParameter(ResponseGroupIdKey, responseGroupId) });
        }

        private static IEnumerable<SqlParameter> GetCommandParameter(int responseGroupId, int productId)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter(ResponseGroupIdKey, responseGroupId),
                new SqlParameter(ProductIdKey, productId)
            };
        }
        #endregion
    }
}