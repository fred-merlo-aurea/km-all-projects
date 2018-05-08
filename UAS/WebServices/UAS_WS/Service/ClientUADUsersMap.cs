using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.ClientUADUsersMap;
using EntityClientUADUsersMap = KMPlatform.Entity.ClientUADUsersMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientUADUsersMap : FrameworkServiceBase, IClientUADUsersMap
    {
        private const string EntityName = "ClientUADUsersMap";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";
        private const string MethodSelectClient = "SelectClient";
        private const string MethodSelectUser = "SelectUser";

        /// <summary>
        /// Selects a list of ClientUADUsersMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientUADUsersMap objects</returns>
        public Response<List<EntityClientUADUsersMap>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityClientUADUsersMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ClientUADUsersMap objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain a list of ClientUADUsersMap objects</returns>
        public Response<List<EntityClientUADUsersMap>> SelectClient(Guid accessKey, int clientID)
        {
            var param = $"ClientID:{clientID}";
            var model = new ServiceRequestModel<List<EntityClientUADUsersMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectClient,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().SelectClient(clientID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ClientUADUsersMap objects based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of ClientUADUsersMap objects</returns>
        public Response<List<EntityClientUADUsersMap>> SelectUser(Guid accessKey, int userID)
        {
            var param = $"UserID:{userID}";
            var model = new ServiceRequestModel<List<EntityClientUADUsersMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectUser,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().SelectUser(userID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a ClientUADUsersMap object based on the client ID and the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a ClientUADUsersMap object</returns>
        public Response<EntityClientUADUsersMap> Select(Guid accessKey, int clientID, int userID)
        {
            var param = $"ClientID:{clientID} UserID:{userID}";
            var model = new ServiceRequestModel<EntityClientUADUsersMap>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, userID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a ClientUADUsersMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityClientUADUsersMap"/> object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, EntityClientUADUsersMap entity)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a ClientUADUsersMap object using the client ID and user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, int clientID, int userID)
        {
            var param = $"ClientID:{clientID} UserID:{userID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().Save(clientID, userID)
            };

            return GetResponse(model);
        }
    }
}
