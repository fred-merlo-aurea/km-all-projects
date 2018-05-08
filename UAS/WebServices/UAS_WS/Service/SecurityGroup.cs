using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.SecurityGroup;
using EntitySecurityGroup = KMPlatform.Entity.SecurityGroup;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityGroup : FrameworkServiceBase, ISecurityGroup
    {
        private const string EntityName = "SecurityGroup";
        private const string MethodSave = "Save";
        private const string MethodSecurityGroupNameExists = "SecurityGroupNameExists";
        private const string MethodSelect = "Select";
        private const string MethodSelectActiveForClientGroup = "SelectActiveForClientGroup";

        /// <summary>
        /// Selects a list of SecurityGroup objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeServices">boolean to include the services with the SecurityGroup object</param>
        /// <returns>response.result will contain a list of SecurityGroup objects</returns>
        public Response<List<KMPlatform.Entity.SecurityGroup>> Select(Guid accessKey, bool includeServices = true)
        {
            Response<List<KMPlatform.Entity.SecurityGroup>> response = new Response<List<KMPlatform.Entity.SecurityGroup>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SecurityGroup", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.SecurityGroup worker = new KMPlatform.BusinessLogic.SecurityGroup();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.AMS_Select(includeServices);
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
                LogError(accessKey, ex, "SecurityGroup", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of SecurityGroup objects available to the client group by the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <param name="includeServices">boolean to include the services for the SecurityGroup object</param>
        /// <returns>response.result will contain a list of SecurityGroup objects</returns>
        public Response<List<KMPlatform.Entity.SecurityGroup>> SelectForClientGroup(Guid accessKey, int clientGroupID, bool includeServices = true)
        {
            Response<List<KMPlatform.Entity.SecurityGroup>> response = new Response<List<KMPlatform.Entity.SecurityGroup>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "clientGroupID:" + clientGroupID.ToString(), "SecurityGroup", "SelectForClientGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.SecurityGroup worker = new KMPlatform.BusinessLogic.SecurityGroup();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.AMS_SelectForClientGroup(clientGroupID, includeServices);
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
                LogError(accessKey, ex, "SecurityGroup", "SelectForClientGroup");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of active SecurityGroup objects available to the client group by the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupId">the client group ID</param>
        /// <param name="includeServices">boolean to include the services for the SecurityGroup object</param>
        /// <returns>response.result will contain a list of SecurityGroup objects</returns>
        public Response<List<EntitySecurityGroup>> SelectActiveForClientGroup(Guid accessKey, int clientGroupId, bool includeServices = true)
        {
            var param = $"clientGroupID:{clientGroupId}";
            var model = new ServiceRequestModel<List<EntitySecurityGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectActiveForClientGroup,
                /* The worker call was commented out in original code.
                  WorkerFunc = _ => new BusinessLogicWorker().AMS_SelectActiveForClientGroup(clientGroupID, includeServices)
                  */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a SecurityGroup object based on the user ID and the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userId">the user ID</param>
        /// <param name="clientId">the client ID</param>
        /// <param name="isKMUser">the client ID</param>
        /// <param name="includeServices">boolean to include the service for the SecurityGroup object</param>
        /// <returns>response.result will contain a SecurityGroup object</returns>
        public Response<EntitySecurityGroup> Select(Guid accessKey, int userId, int clientId, bool isKMUser, bool includeServices = true)
        {
            var model = new ServiceRequestModel<EntitySecurityGroup>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(userId, clientId, isKMUser, includeServices)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a security group name exists
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="name">the security group name</param>
        /// <param name="clientGroupId">the client group ID</param>
        /// <returns>response.result will contian a boolean</returns>
        public Response<bool> SecurityGroupNameExists(Guid accessKey, string name, int clientGroupId)
        {
            var param = $"SecurityGroupName:{name}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSecurityGroupNameExists,
                /* The worker call was commented out in original code.
                  WorkerFunc = _ => new BusinessLogicWorker().SecurityGroupNameExists(name, clientGroupId)
                  */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.SecurityGroup securityGroup, int clientGroupID, int userID)
        {
            if (securityGroup == null)
            {
                throw new ArgumentNullException(nameof(securityGroup));
            }

            var param = new UtilityJsonFunctions().ToJson(securityGroup);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(securityGroup);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
