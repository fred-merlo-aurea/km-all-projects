using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.IssueArchiveProductSubscriptionDetail;
using EntityIssueArchiveProductSubscriptionDetail = FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail;

namespace UAD_WS.Service
{
    public class IssueArchiveProductSubscriptionDetail : FrameworkServiceBase, IIssueArchiveProductSubscriptionDetail
    {
        private const string EntityName = "IssueArchiveProductSubscriptionDetail";
        private const string MethodSave = "Save";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSelectForUpdate = "SelectForUpdate";
        private const string MethodIssueArchiveProductSubscriptionDetailUpdateBulkSql = "IssueArchiveProductSubscriptionDetailUpdateBulkSql";

        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<EntityIssueArchiveProductSubscriptionDetail> list, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list, client)
            };

            return GetResponse(model);
        }

        public Response<bool> Save(Guid accessKey, EntityIssueArchiveProductSubscriptionDetail entity, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityIssueArchiveProductSubscriptionDetail>> SelectForUpdate(
            Guid accessKey,
            int productId,
            int issueId,
            List<int> subs,
            ClientConnections client)
        {
            var param = $"productId: {productId} issueId:{issueId}";
            var model = new ServiceRequestModel<List<EntityIssueArchiveProductSubscriptionDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForUpdate(productId, issueId, subs, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityIssueArchiveProductSubscriptionDetail>> SaveBulkUpdate(
            Guid accessKey,
            ClientConnections client,
            List<EntityIssueArchiveProductSubscriptionDetail> list)
        {
            var param = $"PubSubscriptionID:{list.Select(s => s.PubSubscriptionID).Distinct()}";
            var model = new ServiceRequestModel<List<EntityIssueArchiveProductSubscriptionDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodIssueArchiveProductSubscriptionDetailUpdateBulkSql,
                WorkerFunc = _ => new BusinessLogicWorker().IssueArchiveProductSubscriptionDetailUpdateBulkSql(client, list)
            };

            return GetResponse(model);
        }
    }
}
