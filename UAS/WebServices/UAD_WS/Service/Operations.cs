using System;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Operations;

namespace UAD_WS.Service
{
    public class Operations : FrameworkServiceBase, IOperations
    {
        private const string EntityName = "Operations";
        private const string MethodRemovePubCode = "RemovePubCode";
        private const string MethodRemoveProcessCode = "RemoveProcessCode";
        private const string MethodQSourceValidation = "QSourceValidation";
        private const string MethodFileValidatorQSourceValidation = "FileValidator_QSourceValidation";

        /// <summary>
        /// Removes PubCode from UAD
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubCode">the pub code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> RemovePubCode(Guid accessKey, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" pubCode:{pubCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodRemovePubCode,
                WorkerFunc = _ => new BusinessLogicWorker().RemovePubCode(client, pubCode)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Removes ProcessCode from UAD
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> RemoveProcessCode(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $" processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodRemoveProcessCode,
                WorkerFunc = _ => new BusinessLogicWorker().RemoveProcessCode(client, processCode)
            };

            return GetResponse(model);
        }

        public Response<bool> QSourceValidation(Guid accessKey,  int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $"sourceFileID: {sourceFileID}  processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodQSourceValidation,
                WorkerFunc = _ => new BusinessLogicWorker().QSourceValidation(client, sourceFileID, processCode)
            };

            return GetResponse(model);
        }

        public Response<bool> FileValidator_QSourceValidation(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            var param = $"sourceFileID: {sourceFileID}  processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodFileValidatorQSourceValidation,
                WorkerFunc = _ => new BusinessLogicWorker().FileValidator_QSourceValidation(client, sourceFileID, processCode)
            };

            return GetResponse(model);
        }
    }
}
