using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.MasterCodeSheet;
using EntityMasterCodeSheet = FrameworkUAD.Entity.MasterCodeSheet;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class MasterCodeSheet : FrameworkServiceBase, IMasterCodeSheet
    {
        private const string EntityName = "MasterCodeSheet";
        private const string MethodSelect = "Select";
        private const string MethodSelectMasterGroupId = "SelectMasterGroupID";
        private const string MethodSave = "Save";
        private const string MethodImportSubscriber = "ImportSubscriber";
        private const string MethodDeleteMasterId = "DeleteMasterID";

        /// <summary>
        /// Selects a list of MaseterCodeSheet objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of MasterCodeSheet objects</returns>
        public Response<List<FrameworkUAD.Entity.MasterCodeSheet>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityMasterCodeSheet>>
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
        /// Selects a list of MasterCodeSheet objects based on the master group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="masterGroupID">the master group ID</param>
        /// <returns>response.result will contain a list of MasterCodeSheet objects</returns>
        public Response<List<FrameworkUAD.Entity.MasterCodeSheet>> SelectMasterGroupID(Guid accessKey, KMPlatform.Object.ClientConnections client, int masterGroupID)
        {
            var param = $"MasterGroupID:{masterGroupID}";
            var model = new ServiceRequestModel<List<EntityMasterCodeSheet>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectMasterGroupId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectMasterGroupID(client, masterGroupID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the MasterCodeSheet object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the MasterCodeSheet object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.MasterCodeSheet x, KMPlatform.Object.ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(x, client);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Imports the information from the xml document to the MasterCodeSheet object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="masterID">the master ID</param>
        /// <param name="xDoc">the xml document</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> ImportSubscriber(Guid accessKey, int masterID, XDocument xDoc, KMPlatform.Object.ClientConnections client)
        {
            var param = $" MasterID:{masterID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodImportSubscriber,
                WorkerFunc = _ => new BusinessLogicWorker().ImportSubscriber(masterID, xDoc, client)
            };

            return GetResponse(model);
        }
        
        /// <summary>
        /// Deletes a MasterCodeSheet object based on the master ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="masterID">the master ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> DeleteMasterID(Guid accessKey, int masterID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" masterID:{masterID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteMasterId,
                WorkerFunc = _ => new BusinessLogicWorker().DeleteMasterID(client, masterID)
            };

            return GetResponse(model);
        }
    }    
}
