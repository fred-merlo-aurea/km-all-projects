using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.CodeSheet;
using EntityCodeSheet = FrameworkUAD.Entity.CodeSheet;

namespace UAD_WS.Service
{
    public class CodeSheet : FrameworkServiceBase, ICodeSheet
    {
        private const string EntityName = "CodeSheet";
        private const string MethodCodeSheetValidation = "CodeSheetValidation";
        private const string MethodDeleteCodeSheetId = "DeleteCodeSheetID";
        private const string MethodFileValidatorCodeSheetValidation = "FileValidator_CodeSheetValidation";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of CodeSheet objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of CodeSheet objects</returns>
        public Response<List<EntityCodeSheet>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityCodeSheet>>
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
        /// Selects a list of CodeSheet objects based on the pub ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubID">the pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of CodeSheet objects</returns>
        public Response<List<EntityCodeSheet>> Select(Guid accessKey, int pubID, ClientConnections client)
        {
            var param = $" pubID:{pubID}";
            var model = new ServiceRequestModel<List<EntityCodeSheet>>
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
        /// Saves a CodeSheet object based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityCodeSheet"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>resposne.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityCodeSheet entity, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a CodeSheet object  based on the code sheet ID and the client
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
        /// Validates the CodeSheet object based on the source file ID, the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeSheetValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeSheetValidation,
                WorkerFunc = _ => new BusinessLogicWorker().CodeSheetValidation(sourceFileID, processCode, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes the CodeSheet from SubscriberDemographicTransformed, SubscriberDemographicInvalid, SubscriberInvalid, 
        /// SubscriberTransformed and ImportError based on the source file ID, the client and the process code
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> CodeSheetValidation_Delete(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCodeSheetValidation,
                WorkerFunc = _ => new BusinessLogicWorker().CodeSheetValidation_Delete(sourceFileID, processCode, client)
            };

            return GetResponse(model);
        }

        public Response<bool> FileValidator_CodeSheetValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodFileValidatorCodeSheetValidation,
                WorkerFunc = _ => new BusinessLogicWorker().FileValidator_CodeSheetValidation(sourceFileID, processCode, client)
            };

            return GetResponse(model);
        }
    }
}
