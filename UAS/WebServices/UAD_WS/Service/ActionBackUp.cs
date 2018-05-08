using UAD_WS.Interface;
using FrameworkUAS.Service;
using System;
using System.Linq;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ActionBackUp;

namespace UAD_WS.Service
{
    public class ActionBackUp : FrameworkServiceBase, IActionBackUp
    {
        private const string EntityName = "ActionBackUp";
        private const string MethodBulkInsert = "Bulk_Insert";
        private const string MethodRestore = "Restore";

        /// <summary>
        /// Backs up a Client's product info to an ActionBackUp object based on the product ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productID">the product ID</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Restore(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodRestore,
                WorkerFunc = _ => new BusinessLogicWorker().Restore(productID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Inserts a batch of of Client products by the product ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productID">the product ID</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Bulk_Insert(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodBulkInsert,
                WorkerFunc = _ => new BusinessLogicWorker().Bulk_Insert(productID, client)
            };

            return GetResponse(model);
        }
    }
}
