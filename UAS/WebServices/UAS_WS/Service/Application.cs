using System;
using System.Collections.Generic;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using EntityApplication = KMPlatform.Entity.Application;
using BusinessLogicWorker = KMPlatform.BusinessLogic.Application;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Application : FrameworkServiceBase, IApplication
    {
        private const string EntityName = "Application";
        private const string MethodSelect = "Select";
        private const string MethodSelectForUser = "SelectForUser";
        private const string MethodSelectForSecurityGroup = "SelectForSecurityGroup";
        private const string MethodSelectForService = "SelectForService";
        private const string MethodSelectOnlyEnabledForService = "SelectOnlyEnabledForService";
        private const string MethodSearch = "Search";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of Application objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> Select(Guid accessKey)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Application objects based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> SelectForUser(Guid accessKey, int userID)
        {
            var param = $"UserID:{userID}";
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUser,
                WorkerFunc = request => new BusinessLogicWorker().SelectForUser(userID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Application objects based on the security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> SelectForSecurityGroup(Guid accessKey, int securityGroupID)
        {
            var param = $"SecurityGroupID:{securityGroupID}";
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForSecurityGroup,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().SelectForSecurityGroup(securityGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Application objects based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> SelectForService(Guid accessKey, int serviceID)
        {
            var param = $"ServiceID:{serviceID}";
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForService,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForService(serviceID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Application objects that are only enabled based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> SelectOnlyEnabledForService(Guid accessKey, int serviceID)
        {
            var param = $"ServiceID:{serviceID}";
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectOnlyEnabledForService,
                WorkerFunc = _ => new BusinessLogicWorker().SelectOnlyEnabledForService(serviceID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Response<List<KMPlatform.Entity.Application>> SelectOnlyEnabledForServiceUserID(Guid accessKey, int serviceID, int userID)
        {
            var param = $"ServiceID:{serviceID}, UserID: {userID}";
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectOnlyEnabledForService,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().SelectOnlyEnabledForService(serviceID, userID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches for a Application object of a certain search value in the specified list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="searchValue">the search value</param>
        /// <param name="searchList">the specified search list</param>
        /// <param name="isActive">boolean if the Application object is active</param>
        /// <returns>response.result will contain a list of Application objects</returns>
        public Response<List<KMPlatform.Entity.Application>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.Application> searchList, bool? isActive = null)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntityApplication>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSearch,
                WorkerFunc = _ => new BusinessLogicWorker().Search(searchValue, searchList, isActive)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a Application object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Application object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityApplication entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var param = new UtilityJsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
