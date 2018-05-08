using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.AcsMailerInfo;
using EntityAcsMailerInfo = FrameworkUAD.Entity.AcsMailerInfo;

namespace UAD_WS.Service
{
    public class AcsMailerInfo : FrameworkServiceBase, IAcsMailerInfo
    {
        private const string EntityName = "AcsMailerInfo";
        private const string MethodSelect = "Select";
        private const string MethodSelectById = "SelectByID";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of AcsMailerInfo objects based on the Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a list of AcsMailerInfo</returns>
        public Response<List<EntityAcsMailerInfo>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityAcsMailerInfo>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a AcsMailerInfo object based on the Acs mailer info ID and the Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="acsMailerInfoID">the Acs mailer info ID</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contian a AcsMailerInfo object</returns>
        public Response<EntityAcsMailerInfo> SelectByID(Guid accessKey, int acsMailerInfoID, ClientConnections client)
        {
            var param = $"acsMailerInfoID:{acsMailerInfoID}";
            var model = new ServiceRequestModel<EntityAcsMailerInfo>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectById,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByID(acsMailerInfoID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the given AcsMailerInfo object with the Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityAcsMailerInfo"/> object</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityAcsMailerInfo entity, ClientConnections client)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }
    }
}
