using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberOriginal;
using EntitySubscriberOriginal = FrameworkUAD.Entity.SubscriberOriginal;

namespace UAD_WS.Service
{
    public class SubscriberOriginal : FrameworkServiceBase, ISubscriberOriginal
    {
        private const string EntityName = "SubscriberOriginal";
        private const string MethodSaveBulkUpdate = "SaveBulkUpdate";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSaveBulkInsert = "SaveBulkInsert";
        private const string MethodSelect = "Select";
        private const string MethodSelectForFileAudit = "SelectForFileAudit";
        private const string MethodSave = "Save";
        private const string NullString = "NULL";

        /// <summary>
        /// Selects a list of SubscriberOriginal objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberOriginal objects</returns>
        public Response<List<EntitySubscriberOriginal>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberOriginal>>
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
        /// Selects a list of SubscriberOriginal objects based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberOriginal objects</returns>
        public Response<List<EntitySubscriberOriginal>> Select(Guid accessKey, string processCode, ClientConnections client)
        {
            var param = $" processCode:{processCode}";
            var model = new ServiceRequestModel<List<EntitySubscriberOriginal>>
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
        /// Selects a list of SubscriberOriginal objects based on the source file ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberOriginal objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntitySubscriberOriginal>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(sourceFileID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberOriginal objects based on the process code, the source file ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberOriginal objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntitySubscriberOriginal>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(processCode, sourceFileID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberOriginal objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberOriginal objects</returns>
        public Response<List<EntitySubscriberOriginal>> SelectForFileAudit(
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
            var model = new ServiceRequestModel<List<EntitySubscriberOriginal>>
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
        /// Saves the SubscriberOriginal object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntitySubscriberOriginal"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriberOriginal entity, ClientConnections client)
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
        /// Saves a list of SubscriberOriginal objects and updates the SubscriberOriginal database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberOriginal list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkUpdate,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkUpdate(list, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves and Inserts the list of SubscriberOriginal objects into the SubscriberOriginal database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberOriginal list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkInsert(list, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves and Inserts the list of SubscriberOriginal objects into the SubscriberOriginal database table manually without Sql script
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberOriginal list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list, client)
            };

            return GetResponse(model);
        }
    }
}
