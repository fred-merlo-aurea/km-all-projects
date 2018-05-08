using System;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.AcsShippingDetail;
using EntityAcsShippingDetail = FrameworkUAD.Entity.AcsShippingDetail;

namespace UAD_WS.Service
{
    public class AcsShippingDetail : FrameworkServiceBase, IAcsShippingDetail
    {
        private const string EntityName = "AcsShippingDetail";
        private const string MethodSave = "Save";

        /// <summary>
        /// Saves the given AcsShippingDetail object with the Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="acsShippingDetail">the AcsShippingDetail object</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityAcsShippingDetail acsShippingDetail, ClientConnections client)
        {
            var param = new JsonFunctions().ToJson(acsShippingDetail);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(acsShippingDetail, client)
            };

            return GetResponse(model);
        }
    }
}
