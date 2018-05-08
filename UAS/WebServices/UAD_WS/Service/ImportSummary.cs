using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ImportErrorSummary;
using EntityImportErrorSummary = FrameworkUAD.Object.ImportErrorSummary;

namespace UAD_WS.Service
{
    public class ImportSummary : FrameworkServiceBase, IImportSummary
    {
        private const string EntityName = "ImportErrorSummary";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of ImportErrorSummary objects based on the source file Id, process code and the client objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileId">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ImportErrorSummary objects</returns>
        public Response<List<EntityImportErrorSummary>> Select(Guid accessKey, int sourceFileId, string processCode, ClientConnections client)
        {
            var param = $" processCode:{processCode} sourceFileID:{sourceFileId}";
            var model = new ServiceRequestModel<List<EntityImportErrorSummary>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(sourceFileId, processCode, client)
            };

            return GetResponse(model);
        }
    }
}
