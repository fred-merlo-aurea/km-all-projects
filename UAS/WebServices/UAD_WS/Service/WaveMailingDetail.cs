using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.WaveMailingDetail;
using EntityWaveMailingDetail = FrameworkUAD.Entity.WaveMailingDetail;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class WaveMailingDetail : FrameworkServiceBase, IWaveMailingDetail
    {
        private const string EntityName = "WaveMailingDetail";
        private const string MethodSave = "Save";
        private const string MethodSelectForIssue = "SelectForIssue";
        private const string MethodUpdateOriginalSubInfo = "UpdateOriginalSubInfo";

        /// <summary>
        /// Selects an Issue based on the issue ID from wave mailing details
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueId">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of WaveMailingDetail objects</returns>
        public Response<List<EntityWaveMailingDetail>> SelectIssue(Guid accessKey, int issueId, ClientConnections client)
        {
            var param = $"IssueID:{issueId}";
            var model = new ServiceRequestModel<List<EntityWaveMailingDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForIssue,
                WorkerFunc = request => new BusinessLogicWorker().SelectIssue(issueId, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Response<bool> UpdateOriginalSubInfo(Guid accessKey, int productId, int userId, ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateOriginalSubInfo,
                WorkerFunc = request => new BusinessLogicWorker().UpdateOriginalSubInfo(productId, userId, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a WaveMailingDetail object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="detail">the WaveMailingDetail object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityWaveMailingDetail detail, ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(detail);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(detail, client);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
