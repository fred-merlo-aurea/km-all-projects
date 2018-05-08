using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberArchive;
using EntitySubscriberArchive = FrameworkUAD.Entity.SubscriberArchive;

namespace UAD_WS.Service
{
    public class SubscriberArchive : FrameworkServiceBase, ISubscriberArchive
    {
        private const string EntityName = "SubscriberArchive";
        private const string MethodSaveBulkInsert = "SaveBulkInsert";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";
        private const string MethodSelectForFileAudit = "SelectForFileAudit";
        private const string NullString = "NULL";

        /// <summary>
        /// Selects a list of SubscriberArchive based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberArchive objects</returns>
        public Response<List<EntitySubscriberArchive>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberArchive>>
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
        /// Selects a list of SubscriberArchive objects based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberArchive objects</returns>
        public Response<List<EntitySubscriberArchive>> Select(Guid accessKey, string processCode, ClientConnections client)
        {
            var param = $" ProcessCode:{processCode}";
            var model = new ServiceRequestModel<List<EntitySubscriberArchive>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(processCode, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberArchive objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberArchive objects</returns>
        public Response<List<EntitySubscriberArchive>> SelectForFileAudit(
            Guid accessKey,
            string processCode,
            int sourceFileID,
            DateTime? startDate,
            DateTime? endDate,
            ClientConnections client)
        {
            var start = startDate?.ToString() ?? NullString;
            var end = endDate?.ToString() ?? NullString;
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID} startDate:{start} endDate:{end}";
            var model = new ServiceRequestModel<List<EntitySubscriberArchive>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForFileAudit,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForFileAudit(processCode, sourceFileID, startDate, endDate, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the SubscriberArchive object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntitySubscriberArchive"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriberArchive entity, KMPlatform.Object.ClientConnections client)
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
        /// Saves a list of SubscriberArchive objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberArchive list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberArchive> list, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkInsert(list, client)
            };

            return GetResponse(model);
        }
    }
}
