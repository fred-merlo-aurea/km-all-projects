using System;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberMasterValue;

namespace UAD_WS.Service
{
    public class SubscriberMasterValue : FrameworkServiceBase, ISubscriberMasterValue
    {
        private const string EntityName = "SubscriberMasterValue";
        private const string MethodDeleteMasterId = "DeleteMasterID";

        /// <summary>
        /// Deletes  SubscriberMasterValue object selected by the master ID 
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
