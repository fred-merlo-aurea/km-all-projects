using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ArchivePubSubscriptionsExtension;
using EntityArchivePubSubscriptionsExtension = FrameworkUAD.Entity.ArchivePubSubscriptionsExtension;
using EntityPubSubscriptionAdHoc = FrameworkUAD.Object.PubSubscriptionAdHoc;

namespace UAD_WS.Service
{
    public class ArchivePubSubscriptionsExtension : FrameworkServiceBase, IArchivePubSubscriptionsExtension
    {
        private const string EntityName = "IssueArchiveProductSubscription";
        private const string MethodSelectForUpdate = "SelectForUpdate";
        private const string MethodSave = "Save";

        public Response<List<EntityArchivePubSubscriptionsExtension>> SelectForUpdate(
            Guid accessKey,
            int productId,
            int issueId,
            List<int> subs,
            ClientConnections client)
        {
            var param = $"productID: {productId} issueId:{issueId}";
            var model = new ServiceRequestModel<List<EntityArchivePubSubscriptionsExtension>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForUpdate(productId, issueId, subs, client)
            };

            return GetResponse(model);
        }

        public Response<List<EntityPubSubscriptionAdHoc>> GetArchiveAdhocs(
            Guid accessKey,
            int productId,
            int issueId,
            int pubSubId,
            ClientConnections client)
        {
            var param = $"productID: {productId} issueId:{issueId}";
            var model = new ServiceRequestModel<List<EntityPubSubscriptionAdHoc>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().GetArchiveAdhocs(client, pubSubId, productId, issueId)
            };

            return GetResponse(model);
        }

        public Response<bool> Save(
            Guid accessKey,
            List<EntityPubSubscriptionAdHoc> entities,
            int issueArchiveSubscriptionId,
            int pubId,
            ClientConnections client)
        {
            var param = $"issueArchiveSubscriptionID: {issueArchiveSubscriptionId} , pubID: {pubId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entities, issueArchiveSubscriptionId, pubId, client);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
