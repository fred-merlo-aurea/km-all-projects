using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class DataCompareDownload
    {
        public static List<Entity.DataCompareDownload> SelectForClient(int clientId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownload_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            return GetList(cmd);
        }
        public static List<Entity.DataCompareDownload> SelectForUser(int userId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownload_Select_UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);
            return GetList(cmd);
        }
        public static List<Entity.DataCompareDownload> SelectForSourceFile(int sourceFileId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownload_Select_SourceFileId";
            cmd.Parameters.AddWithValue("@SourceFileId", sourceFileId);
            return GetList(cmd);
        }
        public static List<Entity.DataCompareDownload> SelectForView(int dcViewId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownload_Select_DcViewId";
            cmd.Parameters.AddWithValue("@dcViewId", dcViewId);
            return GetList(cmd);
        }
        public static Entity.DataCompareDownload Get(SqlCommand cmd)
        {
            Entity.DataCompareDownload retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.DataCompareDownload();
                        DynamicBuilder<Entity.DataCompareDownload> builder = DynamicBuilder<Entity.DataCompareDownload>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static List<Entity.DataCompareDownload> GetList(SqlCommand cmd)
        {
            List<Entity.DataCompareDownload> retList = new List<Entity.DataCompareDownload>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.DataCompareDownload retItem = new Entity.DataCompareDownload();
                        DynamicBuilder<Entity.DataCompareDownload> builder = DynamicBuilder<Entity.DataCompareDownload>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.DataCompareDownload x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DataCompareDownload_Save";
            cmd.Parameters.Add(new SqlParameter("@DcDownloadId", x.DcDownloadId));
            cmd.Parameters.Add(new SqlParameter("@DcViewId", x.DcViewId));
            cmd.Parameters.Add(new SqlParameter("@WhereClause", x.WhereClause));
            //cmd.Parameters.Add(new SqlParameter("@ProfileColumns", x.ProfileColumns));
            //cmd.Parameters.Add(new SqlParameter("@DimensionColumns", x.DimensionColumns));
            cmd.Parameters.Add(new SqlParameter("@DcTypeCodeId", x.DcTypeCodeId));
            cmd.Parameters.Add(new SqlParameter("@ProfileCount", x.ProfileCount));
            cmd.Parameters.Add(new SqlParameter("@TotalItemCount", x.TotalItemCount));
            cmd.Parameters.Add(new SqlParameter("@TotalBilledCost", x.TotalBilledCost));
            cmd.Parameters.Add(new SqlParameter("@TotalThirdPartyCost", x.TotalThirdPartyCost));
            cmd.Parameters.Add(new SqlParameter("@IsPurchased", x.IsPurchased));
            cmd.Parameters.Add(new SqlParameter("@PurchasedByUserId", x.PurchasedByUserId));
            cmd.Parameters.Add(new SqlParameter("@PurchasedDate", (object) x.PurchasedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PurchasedCaptcha", x.PurchasedCaptcha));
            cmd.Parameters.Add(new SqlParameter("@IsBilled", x.IsBilled));
            cmd.Parameters.Add(new SqlParameter("@BilledDate", (object) x.BilledDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DownloadFileName ", x.DownloadFileName));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserId ", x.CreatedByUserId));
            cmd.Parameters.Add(new SqlParameter("@DateCreated ", x.DateCreated));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        #region old code comment out 10/31/2016
        //#region Shared
        //private static string SelectStandardList(int dcResultQueId, string demoCodeTypeName, string dcOptionName, string matchTarget)
        //{
        //    string list = string.Empty;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_SelectStandardList";
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DemoCodeTypeName", demoCodeTypeName);
        //    cmd.Parameters.AddWithValue("@DcOptionName", dcOptionName);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    list = DataFunctions.ExecuteScalar(cmd).ToString();

        //    KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
        //    alWrk.SaveNote("DC Standard List: " + list, "DataCompareResult.SelectStandardList", KMPlatform.BusinessLogic.Enums.Applications.Data_Compare, dcResultQueId, "DCResultQueId: " + dcResultQueId.ToString() + ", demoCodeTypeName: " + demoCodeTypeName + ", dcOptionName: " + dcOptionName + ", matchTarget: " + matchTarget);

        //    return list;
        //}
        //private static string SelectPremiumList(int dcResultQueId, string demoCodeTypeName, string dcOptionName, string matchTarget)
        //{
        //    string list = string.Empty;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_SelectPremiumList";
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DemoCodeTypeName", demoCodeTypeName);
        //    cmd.Parameters.AddWithValue("@DcOptionName", dcOptionName);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    list = DataFunctions.ExecuteScalar(cmd).ToString();
        //    KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
        //    alWrk.SaveNote("DC Premium List: " + list, "DataCompareResult.SelectPremiumList", KMPlatform.BusinessLogic.Enums.Applications.Data_Compare, dcResultQueId, "DCResultQueId: " + dcResultQueId.ToString() + ", demoCodeTypeName: " + demoCodeTypeName + ", dcOptionName: " + dcOptionName + ", matchTarget: " + matchTarget);

        //    return list;
        //}
        //private static string SelectCustomList(int dcResultQueId, string demoCodeTypeName, string dcOptionName, string matchTarget)
        //{
        //    string list = string.Empty;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_SelectCustomList";
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DemoCodeTypeName", demoCodeTypeName);
        //    cmd.Parameters.AddWithValue("@DcOptionName", dcOptionName);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    list = DataFunctions.ExecuteScalar(cmd).ToString();
        //    KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
        //    alWrk.SaveNote("DC Custom List: " + list, "DataCompareResult.SelectCustomList", KMPlatform.BusinessLogic.Enums.Applications.Data_Compare, dcResultQueId, "DCResultQueId: " + dcResultQueId.ToString() + ", demoCodeTypeName: " + demoCodeTypeName + ", dcOptionName: " + dcOptionName + ", matchTarget: " + matchTarget);

        //    return list;
        //}
        //#endregion
        //#region LikeDemo
        //public static void CreateLikeDemoTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, int matchCriteriaCount, string matchClause, string matchTarget, string tableName,
        //                                    string processCode, int productId, int brandId, int marketId)
        //{
        //    string standardList = SelectStandardList(dcResultQueId, "Demographic Standard Attributes", "Like Demographics", matchTarget);
        //    string premiumList = SelectPremiumList(dcResultQueId, "Demographic Premium Attributes", "Like Demographics", matchTarget);
        //    string customList = SelectCustomList(dcResultQueId, "Demographic Custom Attributes", "Like Demographics", matchTarget);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoCreateTables";
        //    cmd.Parameters.AddWithValue("@MatchCriteriaCount", matchCriteriaCount);
        //    cmd.Parameters.AddWithValue("@MatchClause", matchClause);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    cmd.Parameters.AddWithValue("@standardList", standardList);
        //    cmd.Parameters.AddWithValue("@premiumList", premiumList);
        //    cmd.Parameters.AddWithValue("@customList", customList);
        //    cmd.Parameters.AddWithValue("@ProductId", productId);
        //    cmd.Parameters.AddWithValue("@BrandId", brandId);
        //    cmd.Parameters.AddWithValue("@MarketId", marketId);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, processCode));
        //    }
        //}
        //private static int SelectLikeDemoStandardCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoSelectStandCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectLikeDemoPremiumCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoSelectPremCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectLikeDemoCustomCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoSelectCustCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //public static void InsertLikeDemoDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    int stdDemoCount = SelectLikeDemoStandardCount(client, tableName);
        //    int premDemoCount = SelectLikeDemoPremiumCount(client, tableName);
        //    int custDemoCount = SelectLikeDemoCustomCount(client, tableName);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoInsertDetail";
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@stdDemoCount", stdDemoCount);
        //    cmd.Parameters.AddWithValue("@premDemoCount", premDemoCount);
        //    cmd.Parameters.AddWithValue("@custDemoCount", custDemoCount);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@parentClientId", parentClientId);
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, "InsertLikeDemoDetail"));
        //    }
        //}
        //public static System.Data.DataTable SelectLikeDemoData(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeDemoSelectData";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    x = DataFunctions.GetDataTable(cmd);

        //    return x;
        //}
        //#endregion
        //#region LikeProfile
        //public static void CreateLikeProfileTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, int likeCriteriaCount, string likeClause, string target, string tableName,
        //                                    string processCode, int productId, int brandId, int marketId)
        //{
        //    string standardList = SelectStandardList(dcResultQueId, "Profile Standard Attributes", "Like Profiles", target);
        //    string premiumList = SelectPremiumList(dcResultQueId, "Profile Premium Attributes", "Like Profiles", target);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileCreateTables";
        //    cmd.Parameters.AddWithValue("@LikeCriteriaCount", likeCriteriaCount);
        //    cmd.Parameters.AddWithValue("@LikeClause", likeClause);
        //    cmd.Parameters.AddWithValue("@Target", target);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    cmd.Parameters.AddWithValue("@standardList", standardList);
        //    cmd.Parameters.AddWithValue("@premiumList", premiumList);
        //    cmd.Parameters.AddWithValue("@ProductId", productId);
        //    cmd.Parameters.AddWithValue("@BrandId", brandId);
        //    cmd.Parameters.AddWithValue("@MarketId", marketId);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, processCode));
        //    }
        //}
        //private static int SelectLikeProfileStandardCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileSelectStandCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectLikeProfilePremiumCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileSelectPremCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectLikeProfileTotalRecordCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileSelectTotalRecordCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //public static void InsertLikeProfileDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    int stdProfCount = SelectLikeProfileStandardCount(client, tableName);
        //    int premProfCount = SelectLikeProfilePremiumCount(client, tableName);
        //    int totalProfCount = SelectLikeProfileTotalRecordCount(client, tableName);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileInsertDetail";
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@stdProfCount", stdProfCount);
        //    cmd.Parameters.AddWithValue("@premProfCount", premProfCount);
        //    cmd.Parameters.AddWithValue("@totalProfCount", totalProfCount);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@parentClientId", parentClientId);
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, "InsertLikeProfileDetail"));
        //    }
        //}
        //public static System.Data.DataTable SelectLikeProfileData(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_LikeProfileSelectData";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    x = DataFunctions.GetDataTable(cmd);

        //    return x;
        //}
        //#endregion
        //#region MatchDemo
        //public static void CreateMatchingDemoTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, int matchCriteriaCount, string matchClause, string matchTarget, string tableName,
        //                                    string processCode, int productId, int brandId, int marketId)
        //{
        //    string standardList = SelectStandardList(dcResultQueId, "Demographic Standard Attributes", "Matching Demographics", matchTarget);
        //    string premiumList = SelectPremiumList(dcResultQueId, "Demographic Premium Attributes", "Matching Demographics", matchTarget);
        //    string customList = SelectCustomList(dcResultQueId, "Demographic Custom Attributes", "Matching Demographics", matchTarget);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoCreateTables";
        //    cmd.Parameters.AddWithValue("@MatchCriteriaCount", matchCriteriaCount);
        //    cmd.Parameters.AddWithValue("@MatchClause", matchClause);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    cmd.Parameters.AddWithValue("@standardList", standardList);
        //    cmd.Parameters.AddWithValue("@premiumList", premiumList);
        //    cmd.Parameters.AddWithValue("@customList", customList);
        //    cmd.Parameters.AddWithValue("@ProductId", productId);
        //    cmd.Parameters.AddWithValue("@BrandId", brandId);
        //    cmd.Parameters.AddWithValue("@MarketId", marketId);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, processCode));
        //    }
        //}
        //private static int SelectMatchingDemoStandardCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoSelectStandCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectMatchingDemoPremiumCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoSelectPremCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectMatchingDemoCustomCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoSelectCustCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //public static void InsertMatchingDemoDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    int stdDemoCount = SelectMatchingDemoStandardCount(client, tableName);
        //    int premDemoCount = SelectMatchingDemoPremiumCount(client, tableName);
        //    int custDemoCount = SelectMatchingDemoCustomCount(client, tableName);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoInsertDetail";
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@stdDemoCount", stdDemoCount);
        //    cmd.Parameters.AddWithValue("@premDemoCount", premDemoCount);
        //    cmd.Parameters.AddWithValue("@custDemoCount", custDemoCount);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@parentClientId", parentClientId);
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, "InsertMatchingDemoDetail"));
        //    }
        //}
        //public static System.Data.DataTable SelectMatchingDemoData(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchDemoSelectData";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    x = DataFunctions.GetDataTable(cmd);

        //    return x;
        //}
        //#endregion
        //#region MatchProfile
        //public static void CreateMatchingProfileTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, int matchCriteriaCount, string matchClause, string matchTarget, string tableName,
        //                                    string processCode, int productId, int brandId, int marketId)
        //{
        //    string standardList = SelectStandardList(dcResultQueId, "Profile Standard Attributes", "Matching Profiles", matchTarget);
        //    string premiumList = SelectPremiumList(dcResultQueId, "Profile Premium Attributes", "Matching Profiles", matchTarget);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchProfileCreateTables";
        //    cmd.Parameters.AddWithValue("@MatchCriteriaCount", matchCriteriaCount);
        //    cmd.Parameters.AddWithValue("@MatchClause", matchClause);
        //    cmd.Parameters.AddWithValue("@MatchTarget", matchTarget);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    cmd.Parameters.AddWithValue("@standardList", standardList);
        //    cmd.Parameters.AddWithValue("@premiumList", premiumList);
        //    cmd.Parameters.AddWithValue("@ProductId", productId);
        //    cmd.Parameters.AddWithValue("@BrandId", brandId);
        //    cmd.Parameters.AddWithValue("@MarketId", marketId);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, processCode));
        //    }
        //}
        //private static int SelectMatchingProfileStandardCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchProfileSelectStandCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectMatchingProfilePremiumCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchProfileSelectPremCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //private static int SelectMatchingProfileTotalRecordCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchingProfileSelectTotalRecordCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //public static void InsertMatchingProfileDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    int stdProfCount = SelectMatchingProfileStandardCount(client, tableName);
        //    int premProfCount = SelectMatchingProfilePremiumCount(client, tableName);
        //    int totalProfCount = SelectMatchingProfileTotalRecordCount(client, tableName);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchProfileInsertDetail";
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@stdProfCount", stdProfCount);
        //    cmd.Parameters.AddWithValue("@premProfCount", premProfCount);
        //    cmd.Parameters.AddWithValue("@totalProfCount", totalProfCount);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Parameters.AddWithValue("@clientId", clientId);
        //    cmd.Parameters.AddWithValue("@parentClientId", parentClientId);
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, "InsertMatchingProfileDetail"));
        //    }
        //}
        //public static System.Data.DataTable SelectMatchingProfileData(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_MatchProfileSelectData";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    x = DataFunctions.GetDataTable(cmd);

        //    return x;
        //}
        //#endregion
        //#region NoData
        //private static string SelectNoDataList(int dcResultQueId)
        //{
        //    string list = string.Empty;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_NoDataSelectList";
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    list = DataFunctions.ExecuteScalar(cmd).ToString();
        //    return list;
        //}
        //public static void CreateNoDataTable(KMPlatform.Object.ClientConnections client, int dcResultQueId, string tableName, string processCode)
        //{
        //    string list = SelectNoDataList(dcResultQueId);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_NoDataCreateTable";
        //    cmd.Parameters.AddWithValue("@list", list);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, processCode));
        //    }
        //}
        //private static int SelectNoDataCount(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    int count = 0;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_NoDataSelectCnt";
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    count = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        //    return count;
        //}
        //public static void InsertNoDataDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName)
        //{
        //    int ndCount = SelectNoDataCount(client, tableName);

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_NoDataInsertDetail";
        //    cmd.Parameters.AddWithValue("@dcResultQueId", dcResultQueId);
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@ndCount", ndCount);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());

        //    try
        //    {
        //    DataFunctions.ExecuteNonQuery(cmd);
        //}
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
        //        fl.Save(new FrameworkUAS.Entity.FileLog(-1, 0, msg, "InsertNoDataDetail"));
        //    }
        //}
        //public static System.Data.DataTable SelectNoDataData(KMPlatform.Object.ClientConnections client, string tableName)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dc_NoDataSelectData";
        //    cmd.Connection = DataFunctions.GetClientSqlConnection(client);
        //    cmd.Parameters.AddWithValue("@TableName", tableName);
        //    x = DataFunctions.GetDataTable(cmd);

        //    return x;
        //}
        //#endregion
        //public static System.Data.DataTable CreateSummaryReportFile(int dataCompareResultId, string fileName, string processCode)
        //{
        //    System.Data.DataTable x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "dt_DataCompareResult_CreateSummaryReportFile";
        //    cmd.Connection = DataFunctions.GetSqlConnection(DataFunctions.ConnectionString.UAS.ToString());
        //    cmd.Parameters.AddWithValue("@DataCompareResultId", dataCompareResultId);
        //    cmd.Parameters.AddWithValue("@FileName", fileName);
        //    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
        //    x = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.UAS.ToString());

        //    return x;
        //}
        #endregion
    }
}
