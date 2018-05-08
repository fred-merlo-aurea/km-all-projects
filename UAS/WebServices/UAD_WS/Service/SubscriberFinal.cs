using System;
using System.Collections.Generic;
using System.Globalization;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberFinal;
using EntitySubscriberFinal = FrameworkUAD.Entity.SubscriberFinal;

namespace UAD_WS.Service
{
    public class SubscriberFinal : FrameworkServiceBase, ISubscriberFinal
    {
        private const string EntityName = "SubscriberFinal";
        private const string NullString = "NULL";
        private const string MethodSaveDqmClean = "SaveDQMClean";
        private const string MethodSetOneMaster = "SetOneMaster";
        private const string MethodSetMissingMaster = "SetMissingMaster";
        private const string MethodAddressSearch = "AddressSearch";
        private const string MethodSelectByAddressValidation = "SelectByAddressValidation";
        private const string MethodSelectForFileAudit = "SelectForFileAudit";
        private const string MethodSaveBulkUpdate = "SaveBulkUpdate";
        private const string MethodSaveBulkInsert = "SaveBulkInsert";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of SubscriberFinal objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberFinal objects</returns>
        public Response<List<EntitySubscriberFinal>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberFinal>>
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
        /// Selects a list of SubscriberFinal objects based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberFinal objects</returns>
        public Response<List<EntitySubscriberFinal>> Select(Guid accessKey, string processCode, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberFinal>>
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
        /// Selects a list of SubscriberFinal objects based on the process code, source file ID, the client and if the latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="isLatLonValid">boolean if the latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberFinal objects</returns>
        public Response<List<EntitySubscriberFinal>> SelectByAddressValidation(
            Guid accessKey,
            KMPlatform.Object.ClientConnections client,
            string processCode,
            int sourceFileID,
            bool isLatLonValid)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberFinal>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $" processCode:{processCode} sourceFileID:{sourceFileID} isLatLonValid:{isLatLonValid}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, processCode, sourceFileID, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberFinal objects based on the client and if the latitude/longitude is valid
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="isLatLonValid">boolean if the latitude/longitude is valid</param>
        /// <returns>response.result will contain a list of SubscriberFinal objects</returns>
        public Response<List<EntitySubscriberFinal>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberFinal>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $" isLatLonValid:{isLatLonValid}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAddressValidation,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAddressValidation(client, isLatLonValid)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SubscriberFinal objects based on the process code, the source file ID, start date, end date and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberFinal objects</returns>
        public Response<List<EntitySubscriberFinal>> SelectForFileAudit(
            Guid accessKey,
            string processCode,
            int sourceFileID,
            DateTime? startDate,
            DateTime? endDate,
            KMPlatform.Object.ClientConnections client)
        {
            var start = startDate?.ToString(CultureInfo.InvariantCulture) ?? NullString;
            var end = endDate?.ToString(CultureInfo.InvariantCulture) ?? NullString;
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID} startDate:{start} endDate:{end}";
            var model = new ServiceRequestModel<List<EntitySubscriberFinal>>
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
        /// Saves a list of SubscriberFinal objects and updates them to the SubscriberDemographicFinal database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberFinal list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkUpdate(Guid accessKey, List<EntitySubscriberFinal> list, KMPlatform.Object.ClientConnections client)
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
        /// Saves a list of SubscriberFinal objects and Inserts them into the SubscriberFinal database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the SubscriberFinal list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkInsert(Guid accessKey, List<EntitySubscriberFinal> list, KMPlatform.Object.ClientConnections client)
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
        /// Saves the SubscriberFinal object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntitySubscriberFinal"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriberFinal entity, KMPlatform.Object.ClientConnections client)
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
        /// Saves the SubscriberFinal object by the process code and inserts non duplicate records to the SubscriberDemographicFinal database table for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <param name="fileType">the process code</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveDQMClean(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode, string fileType)
        {
            var param = $" sourceFileID:{sourceFileID} processCode:{processCode}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveDqmClean,
                WorkerFunc = _ => new BusinessLogicWorker().SaveDQMClean(client, processCode, fileType)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Sets that SubscriberFinal is missing the master group for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SetMissingMaster(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSetMissingMaster,
                WorkerFunc = _ => new BusinessLogicWorker().SetMissingMaster(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Sets SubscriberFinal to one Master group for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SetOneMaster(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSetOneMaster,
                WorkerFunc = _ => new BusinessLogicWorker().SetOneMaster(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches for SubscriberFinal objects by the given address information
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="address">the address</param>
        /// <param name="mailstop">the mailstop</param>
        /// <param name="city">the city</param>
        /// <param name="state">the state</param>
        /// <param name="zip">the zip code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> AddressSearch(Guid accessKey, string address, string mailstop, string city, string state, string zip, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodAddressSearch,
                WorkerFunc = _ => new BusinessLogicWorker().AddressSearch(address, mailstop, city, state, zip, client)
            };

            return GetResponse(model);
        }
    }
}
