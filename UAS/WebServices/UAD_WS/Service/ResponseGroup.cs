using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ResponseGroup;
using EntityResponseGroup = FrameworkUAD.Entity.ResponseGroup;

namespace UAD_WS.Service
{
    public class ResponseGroup : FrameworkServiceBase, IResponseGroup
    {
        private const string EntityName = "ResponseGroup";
        private const string MethodDelete = "Delete";
        private const string MethodCopy = "Copy";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of ResponseGroup objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ResponseGroup objects</returns>
        public Response<List<EntityResponseGroup>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityResponseGroup>>
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
        /// Selects a list of ResponseGroup objects based on the client and the pub ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="pubID">the pub ID</param>
        /// <returns>response.result will contain a list of ResponseGroup objects</returns>
        public Response<List<EntityResponseGroup>> Select(Guid accessKey, ClientConnections client, int pubID)
        {
            var param = $"PubID:{pubID}";
            var model = new ServiceRequestModel<List<EntityResponseGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(pubID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the ResponseGroup object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the ResponseGroup object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, ClientConnections client, EntityResponseGroup entity)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity, client);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Copies the information over from a ResponseGroup selected by the response group ID to a xml file of the given pub ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="responseGroupID">the response group ID</param>
        /// <param name="destPubsXML">the destination xml files pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Copy(Guid accessKey, int responseGroupID, string destPubsXML, KMPlatform.Object.ClientConnections client)
        {
            var param = $" ResponseGroupID:{responseGroupID} XML:{destPubsXML}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCopy,
                WorkerFunc = _ => new BusinessLogicWorker().Copy(client, responseGroupID, destPubsXML)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a ResponseGroup object for the client based on the response group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="responseGroupID">the response group ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" ResponseGroupID:{responseGroupID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete
            };

            return GetResponse(model);
        }
    }
}
