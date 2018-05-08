using System;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriptionDetail;

namespace UAD_WS.Service
{
    public class SubscriptionDetail : FrameworkServiceBase, ISubscriptionDetail
    {
        private const string EntityName = "SubscriptionDetail";
        private const string MethodDeleteMasterId = "DeleteMasterID";

        /// <summary>
        /// Deletes a SubscriptionDetail object based on the master ID for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="masterId">the master ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> DeleteMasterID(Guid accessKey, int masterId, ClientConnections client)
        {
            var param = $" masterID:{masterId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteMasterId,
                WorkerFunc = _ => new BusinessLogicWorker().DeleteMasterID(client, masterId)
            };

            return GetResponse(model);
        }
    }
}
