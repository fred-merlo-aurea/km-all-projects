using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using System.IO;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Suppressed;

namespace UAD_WS.Service
{
    public class Suppressed : FrameworkServiceBase, ISuppressed
    {
        private const string EntityName = "Suppressed";
        private const string MethodPerformSuppression = "PerformSuppression";

        /// <summary>
        /// Saves and Inserts a list of 1000 Suppressed objects at a time through hardcode
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of Suppressed objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.Suppressed> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Suppressed", "SaveBulkSqlInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Suppressed worker = new FrameworkUAD.BusinessLogic.Suppressed();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkSqlInsert(list, client);
                    if (response.Result == true || response.Result == false)
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
        /// Suppresses 1000 SubscriberFinal objects at a time within the given list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of SubscriberFinal objects</param>
        /// <param name="client">the client object</param>
        /// <param name="sourceFileId">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="suppFile">the FileInfo object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> PerformSuppression(
            Guid accessKey,
            List<FrameworkUAD.Entity.SubscriberFinal> list,
            KMPlatform.Object.ClientConnections client,
            int sourceFileId,
            string processCode,
            FileInfo suppFile)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodPerformSuppression,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().PerformSuppression(list, client, sourceFileId, processCode, suppFile.Name);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
