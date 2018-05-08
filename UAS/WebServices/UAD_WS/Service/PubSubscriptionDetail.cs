using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ProductSubscriptionDetail;
using EntityProductSubscriptionDetail = FrameworkUAD.Entity.ProductSubscriptionDetail;

namespace UAD_WS.Service
{
    public class PubSubscriptionDetail : FrameworkServiceBase, IPubSubscriptionDetail
    {
        private const string EntityName = "PubSubscriptionDetail";
        private const string EntityNameProductSubscriptionDetail = "ProductSubscriptionDetail";
        private const string MethodDeleteCodeSheetId = "DeleteCodeSheetID";
        private const string MethodSelect = "Select";
        private const string MethodProductSubscriptionDetailUpdateBulkSql = "ProductSubscriptionDetailUpdateBulkSql";
        private const string MethodSelectCount = "SelectCount";
        private const string MethodSelectPaging = "SelectPaging";

        /// <summary>
        /// Deletes a ProductSubscriptionDetail object based on the code sheet ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="codeSheetId">the code sheet ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> DeleteCodeSheetID(Guid accessKey, int codeSheetId, ClientConnections client)
        {
            var param = $" codeSheetID:{codeSheetId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteCodeSheetId,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(client, codeSheetId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ProductSubscriptionDetail objects based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubSubscriptionId">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductSubscriptionDetail objects</returns>
        public Response<List<EntityProductSubscriptionDetail>> Select(Guid accessKey, int pubSubscriptionId,  ClientConnections client)
        {
            var param = $" pubSubscriptionID:{pubSubscriptionId}";
            var model = new ServiceRequestModel<List<EntityProductSubscriptionDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(pubSubscriptionId, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a list of ProductSubscriptionDetail objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="list">the ProductSubscriptionDetail list to be saved</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<List<EntityProductSubscriptionDetail>> SaveBulkUpdate(Guid accessKey, ClientConnections client, List<EntityProductSubscriptionDetail> list)
        {
            var param = $"PubSubscriptionID:{list.Select(s => s.PubSubscriptionID).Distinct()}";
            var model = new ServiceRequestModel<List<EntityProductSubscriptionDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodProductSubscriptionDetailUpdateBulkSql,
                WorkerFunc = _ => new BusinessLogicWorker().ProductSubscriptionDetailUpdateBulkSql(client, list)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets the count of the ProductSubscriptionDetail objects that have the given product ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productId">the product ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SelectCount(Guid accessKey, int productId, ClientConnections client)
        {
            var param = $"productID:{productId}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCount,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCount(productId, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a certain amount of ProductSubscriptionDetail objects based on the current page, page size, product ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="page">the current page</param>
        /// <param name="pageSize">the page size</param>
        /// <param name="productId">the product ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductSubscriptionDetail objects</returns>
        public Response<List<EntityProductSubscriptionDetail>> SelectPaging(Guid accessKey, int page, int pageSize, int productId, ClientConnections client)
        {
            var param = $"page:{page} pageSize:{pageSize} productID:{productId}";
            var model = new ServiceRequestModel<List<EntityProductSubscriptionDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameProductSubscriptionDetail,
                AuthenticateMethod = MethodSelectPaging,
                WorkerFunc = _ => new BusinessLogicWorker().SelectPaging(page, pageSize, productId, client)
            };

            return GetResponse(model);
        }
    }    
}
