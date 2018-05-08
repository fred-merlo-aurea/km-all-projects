using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.IssueArchiveProductSubscription;
using EntityIssueArchiveProductSubscription = FrameworkUAD.Entity.IssueArchiveProductSubscription;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class IssueArchiveProductSubscription : FrameworkServiceBase, IIssueArchiveProductSubscription
    {
        private const string EntityName = "IssueArchiveProductSubscription";
        private const string MethodSave = "Save";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSelectCount = "SelectCount";
        private const string MethodSelectForUpdate = "SelectForUpdate";
        private const string MethodSelectPaging = "SelectPaging";
        private const string MethodSelectForIssue = "SelectForIssue";

        /// <summary>
        /// Selects a list of IssueArchiveProductSubscription based on the issue ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueID">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueArchiveProductSubscription objects</returns>
        public Response<List<EntityIssueArchiveProductSubscription>> SelectIssue(Guid accessKey, int issueID, ClientConnections client)
        {
            var param = $"IssueID:{issueID}";
            var model = new ServiceRequestModel<List<EntityIssueArchiveProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForIssue,
                WorkerFunc = _ => new BusinessLogicWorker().SelectIssue(issueID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a certain amount of IssueArchiveProductSubscription objects into a list based on the page, page size, issue ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="page">the current page</param>
        /// <param name="pageSize">the page size</param>
        /// <param name="issueID">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueArchiveProductSubscription objects</returns>
        public Response<List<EntityIssueArchiveProductSubscription>> SelectPaging(
            Guid accessKey,
            int page,
            int pageSize,
            int issueID,
            ClientConnections client)
        {
            var param = $"page:{page} pageSize:{pageSize} issueID:{issueID}";
            var model = new ServiceRequestModel<List<EntityIssueArchiveProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectPaging,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPaging(page, pageSize, issueID, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityIssueArchiveProductSubscription>> SelectForUpdate(
            Guid accessKey,
            int productID,
            int issueId,
            List<int> subs,
            ClientConnections client)
        {
            var param = $"productID: {productID} issueId:{issueId}";
            var model = new ServiceRequestModel<List<EntityIssueArchiveProductSubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForUpdate(productID, issueId, subs, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects the count of how many IssueArchiveProductSubscription objects there are
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueID">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SelectCount(Guid accessKey, int issueID, ClientConnections client)
        {
            var param = $"issueID:{issueID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCount,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCount(issueID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a list of IssueProductSubscription objects for the client in a batch size of 1000 for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the IssueArchiveProductSubsciption list to be saved</param>
        /// <param name="client"></param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<EntityIssueArchiveProductSubscription> list, ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the IssueArchiveProductSubscription object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the IssueArchiveProductSubscription object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(x, client)
            };

            return GetResponse(model);
        }

        public Response<int> SaveAll(Guid accessKey, FrameworkUAD.Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().SaveAll(x, client)
            };

            return GetResponse(model);
        }
    }
}
