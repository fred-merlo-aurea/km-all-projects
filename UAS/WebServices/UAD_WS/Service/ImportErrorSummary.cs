using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ImportErrorSummary;
using EntityImportErrorSummary = FrameworkUAD.Object.ImportErrorSummary;

namespace UAD_WS.Service
{
    public class ImportErrorSummary : FrameworkServiceBase, IImportErrorSummary
    {
        private const string EntityName = "ImportErrorSummary";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of ImportErrorSummary objects based on the source file ID, the process code and the client
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

        /// <summary>
        /// Selects Import Erros from FileValidator based on the source file ID, process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ImportErrorSummary objects</returns>
        public Response<List<FrameworkUAD.Object.ImportErrorSummary>> FileValidatorSelect(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.ImportErrorSummary>> response = new Response<List<FrameworkUAD.Object.ImportErrorSummary>>();
            try
            {
                string param =  " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportErrorSummary", "FileValidatorSelect");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ImportErrorSummary worker = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.FileValidatorSelect(sourceFileID, processCode, client).ToList();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Creates an Error summary report
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <param name="clientArchiveDir">the client archive directory</param>
        /// <param name="clientName"></param>
        /// <returns>response.result will contain a string</returns>
        public Response<string> CreateDimensionErrorsSummaryReport(Guid accessKey, int sourceFileID, string processCode, string clientName, KMPlatform.Object.ClientConnections client, string clientArchiveDir)
        {
            Response<string> response = new Response<string>();
            try
            {
                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString() + " clientArchiveDir:" + clientArchiveDir;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportErrorSummary", "CreateDimensionErrorsSummaryReport");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ImportErrorSummary worker = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.CreateDimensionErrorsSummaryReport(sourceFileID, processCode, clientName, client, clientArchiveDir);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
