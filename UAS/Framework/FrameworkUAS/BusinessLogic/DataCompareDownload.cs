using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareDownload
    {
        public List<Entity.DataCompareDownload> SelectForClient(int clientId)
        {
            List<Entity.DataCompareDownload> x = null;
            x = DataAccess.DataCompareDownload.SelectForClient(clientId).ToList();
            foreach (Entity.DataCompareDownload r in x)
                SetCustomProperties(r);
            return x;
        }
        public List<Entity.DataCompareDownload> SelectForUser(int userId)
        {
            List<Entity.DataCompareDownload> x = null;
            x = DataAccess.DataCompareDownload.SelectForUser(userId).ToList();
            foreach (Entity.DataCompareDownload r in x)
                SetCustomProperties(r);
            return x;
        }
        public List<Entity.DataCompareDownload> SelectForSourceFile(int sourceFileId)
        {
            List<Entity.DataCompareDownload> x = null;
            x = DataAccess.DataCompareDownload.SelectForSourceFile(sourceFileId).ToList();
            foreach (Entity.DataCompareDownload r in x)
                SetCustomProperties(r);
            return x;
        }
        public List<Entity.DataCompareDownload> SelectForView(int dcViewId)
        {
            List<Entity.DataCompareDownload> x = null;
            x = DataAccess.DataCompareDownload.SelectForView(dcViewId).ToList();
            foreach (Entity.DataCompareDownload r in x)
                SetCustomProperties(r);
            return x;
        }
        private void SetCustomProperties(Entity.DataCompareDownload dcr)
        {
            DataCompareDownloadCostDetail cWorker = new DataCompareDownloadCostDetail();
            dcr.CostDetail = cWorker.Select(dcr.DcDownloadId);
        }
        public int Save(Entity.DataCompareDownload x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.DcDownloadId = DataAccess.DataCompareDownload.Save(x);
                scope.Complete();
            }

            return x.DcDownloadId;
        }

        //public Entity.DataCompareDownload CreateResult(int dataCompareId)
        //{
        //    Entity.DataCompareDownload x = null;
        //    x = DataAccess.DataCompareDownload.CreateResult(dataCompareId);

        //    return x;
        //}

        #region Old DC Methods
        //#region LikeDemo
        //private void CreateLikeDemoTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, string likeClause, string matchTarget, string tableName,
        //                                    string ProcessCode, int productId, int brandId, int marketId)
        //{
        //    DataCompareUserLikeCriteria ulcWorker = new DataCompareUserLikeCriteria();
        //    int likeCriteriaCount = ulcWorker.SelectCount(dcResultQueId);

        //    DataAccess.DataCompareResult.CreateLikeDemoTables(client, dcResultQueId, likeCriteriaCount, likeClause, matchTarget, tableName, ProcessCode, productId, brandId, marketId);
        //}
        //private void InsertLikeDemoDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    DataAccess.DataCompareResult.InsertLikeDemoDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //}
        //public System.Data.DataTable SelectLikeDemoData(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string likeClause, string matchTarget,
        //                                                string fileName, string tableName, string processCode, int productId, int brandId, int marketId, int clientId, int parentClientId)
        //{
        //    System.Data.DataTable x = null;
        //    try
        //    {
        //        if (matchTarget.Equals("Product"))
        //        {
        //            likeClause = likeClause.Replace("s.", "ps.");
        //            likeClause = ResetFilters(likeClause);
        //        }

        //        CreateLikeDemoTables(client, dcResultQueId, likeClause, matchTarget, tableName, processCode, productId, brandId, marketId);
        //        InsertLikeDemoDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //        x = DataAccess.DataCompareResult.SelectLikeDemoData(client, tableName);
        //    }
        //    catch(Exception ex)
        //    {
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Data_Compare;
        //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
        //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SelectLikeDemoData", app, string.Empty);
        //    }
        //    return x;
        //}
        //#endregion
        //#region LikeProfile
        //private void CreateLikeProfileTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, string likeClause, string matchTarget, string tableName,
        //                                    string ProcessCode, int productId, int brandId, int marketId)
        //{
        //    DataCompareUserLikeCriteria ulcWorker = new DataCompareUserLikeCriteria();
        //    int likeCriteriaCount = ulcWorker.SelectCount(dcResultQueId);

        //    DataAccess.DataCompareResult.CreateLikeProfileTables(client, dcResultQueId, likeCriteriaCount, likeClause, matchTarget, tableName, ProcessCode, productId, brandId, marketId);
        //}
        //private void InsertLikeProfileDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    DataAccess.DataCompareResult.InsertLikeProfileDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //}
        //public System.Data.DataTable SelectLikeProfileData(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string likeClause, string matchTarget,
        //                                                string fileName, string tableName, string processCode, int productId, int brandId, int marketId, int clientId, int parentClientId)
        //{
        //    System.Data.DataTable x = null;
        //    try
        //    {
        //        if (matchTarget.Equals("Product"))
        //        {
        //            likeClause = likeClause.Replace("s.", "ps.");
        //            likeClause = ResetFilters(likeClause);
        //        }

        //        CreateLikeProfileTables(client, dcResultQueId, likeClause, matchTarget, tableName, processCode, productId, brandId, marketId);
        //        InsertLikeProfileDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //        x = DataAccess.DataCompareResult.SelectLikeProfileData(client, tableName);
        //    }
        //    catch (Exception ex)
        //    {
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Data_Compare;
        //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
        //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SelectLikeProfileData", app, string.Empty);
        //    }
        //    return x;
        //}
        //#endregion
        //#region MatchDemo
        //private void CreateMatchingDemoTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, string matchClause, string matchTarget, string tableName,
        //                                   string ProcessCode, int productId, int brandId, int marketId)
        //{
        //    DataCompareUserMatchCriteria ulcWorker = new DataCompareUserMatchCriteria();
        //    int matchCriteriaCount = ulcWorker.SelectCount(dcResultQueId);

        //    DataAccess.DataCompareResult.CreateMatchingDemoTables(client, dcResultQueId, matchCriteriaCount, matchClause, matchTarget, tableName, ProcessCode, productId, brandId, marketId);
        //}
        //private void InsertMatchingDemoDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    DataAccess.DataCompareResult.InsertMatchingDemoDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //}
        //public System.Data.DataTable SelectMatchingDemoData(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string matchClause, string matchTarget,
        //                                                string fileName, string tableName, string processCode, int productId, int brandId, int marketId, int clientId, int parentClientId)
        //{
        //    System.Data.DataTable x = null;
        //    try
        //    {
        //        if (matchTarget.Equals("Product"))
        //        {
        //            matchClause = matchClause.Replace("s.", "ps.");
        //            matchClause = ResetFilters(matchClause);
        //        }

        //        CreateMatchingDemoTables(client, dcResultQueId, matchClause, matchTarget, tableName, processCode, productId, brandId, marketId);
        //        InsertMatchingDemoDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //        x = DataAccess.DataCompareResult.SelectMatchingDemoData(client, tableName);
        //    }
        //    catch (Exception ex)
        //    {
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Data_Compare;
        //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
        //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SelectMatchingDemoData", app, string.Empty);
        //    }
        //    return x;
        //}
        //#endregion
        //#region MatchProfile
        //private void CreateMatchingProfileTables(KMPlatform.Object.ClientConnections client, int dcResultQueId, string matchClause, string matchTarget, string tableName,
        //                                    string ProcessCode, int productId, int brandId, int marketId)
        //{
        //    DataCompareUserMatchCriteria ulcWorker = new DataCompareUserMatchCriteria();
        //    int matchCriteriaCount = ulcWorker.SelectCount(dcResultQueId);

        //    DataAccess.DataCompareResult.CreateMatchingProfileTables(client, dcResultQueId, matchCriteriaCount, matchClause, matchTarget, tableName, ProcessCode, productId, brandId, marketId);
        //}
        //private void InsertMatchingProfileDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, int clientId, int parentClientId)
        //{
        //    DataAccess.DataCompareResult.InsertMatchingProfileDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //}
        //public System.Data.DataTable SelectMatchingProfileData(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string matchClause, string matchTarget,
        //                                                string fileName, string tableName, string processCode, int productId, int brandId, int marketId, int clientId, int parentClientId)
        //{
        //    System.Data.DataTable x = null;
        //    try
        //    {
        //        if (matchTarget.Equals("Product"))
        //        {
        //            matchClause = matchClause.Replace("s.", "ps.");
        //            matchClause = ResetFilters(matchClause);
        //        }

        //        CreateMatchingProfileTables(client, dcResultQueId, matchClause, matchTarget, tableName, processCode, productId, brandId, marketId);
        //        InsertMatchingProfileDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName, clientId, parentClientId);
        //        x = DataAccess.DataCompareResult.SelectMatchingProfileData(client, tableName);
        //    }
        //    catch (Exception ex)
        //    {
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Data_Compare;
        //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
        //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SelectMatchingProfileData", app, string.Empty);
        //    }
        //    return x;
        //}
        //#endregion
        //#region NoData
        //private void CreateNoDataTable(KMPlatform.Object.ClientConnections client, int dcResultQueId, string tableName, string ProcessCode)
        //{
        //    DataAccess.DataCompareResult.CreateNoDataTable(client, dcResultQueId, tableName, ProcessCode);
        //}
        //private void InsertNoDataDetail(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName)
        //{
        //    DataAccess.DataCompareResult.InsertNoDataDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName);
        //}
        //public System.Data.DataTable SelectNoDataData(KMPlatform.Object.ClientConnections client, int dcResultQueId, int dataCompareResultId, string fileName, string tableName, string processCode)
        //{
        //    System.Data.DataTable x = null;
        //    try
        //    {
        //        CreateNoDataTable(client, dcResultQueId, tableName, processCode);
        //        InsertNoDataDetail(client, dcResultQueId, dataCompareResultId, fileName, tableName);
        //        x = DataAccess.DataCompareResult.SelectNoDataData(client, tableName);
        //    }
        //    catch (Exception ex)
        //    {
        //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Data_Compare;
        //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
        //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
        //        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SelectNoDataData", app, string.Empty);
        //    }
        //    return x;
        //}
        //#endregion
        //public System.Data.DataTable CreateSummaryReportFile(int dataCompareResultId, string fileName, string processCode)
        //{
        //    System.Data.DataTable x = null;
        //    x = DataAccess.DataCompareResult.CreateSummaryReportFile(dataCompareResultId, fileName, processCode);

        //    return x;
        //}
        #endregion
    }
}
