using System;
using Core_AMS.Utilities;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.CodeSheetMasterCodeSheetBridge;
using EntityCodeSheetMasterCodeSheetBridge = FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge;

namespace UAD_WS.Service
{
    public class CodeSheetMasterCodeSheetBridge : FrameworkServiceBase, ICodeSheetMasterCodeSheetBridge
    {
        private const string EntityName = "CodeSheetMasterCodeSheetBridge";
        private const string MethodDeleteCodeSheetId = "DeleteCodeSheetID";
        private const string MethodDeleteMasterId = "DeleteMasterID";
        private const string MethodSave = "Save";

        /// <summary>
        /// Saves the CodeSheetMasterCodeSheetBridge object based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityCodeSheetMasterCodeSheetBridge"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityCodeSheetMasterCodeSheetBridge entity, KMPlatform.Object.ClientConnections client)
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

        /// <summary>
        /// Deletes the CodeSheetMasterCodeSheetBridge object based on the code sheet ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeSheetID">the code sheet ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> DeleteCodeSheetID(Guid accessKey, int codeSheetID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" codeSheetID:{codeSheetID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteCodeSheetId,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(client, codeSheetID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a CodeSheetMasterCodeSheetBridge object based on the master ID and the client
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
