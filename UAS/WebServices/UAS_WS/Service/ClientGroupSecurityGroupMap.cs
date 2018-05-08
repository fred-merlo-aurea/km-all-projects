using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientGroupSecurityGroupMap : ServiceBase, IClientGroupSecurityGroupMap
    {
        /// <summary>
        /// Selects a list of ClientGroupSecurityGroupMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientGroupSecurityGroupMap</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupSecurityGroupMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap worker = new FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of ClientGroupSecurityGroupMap objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a list of ClientGroupSecurityGroupMap objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> SelectForClientGroup(Guid accessKey, int clientGroupID)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>>();
            try
            {
                string param = "clientGroupID: " + clientGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupSecurityGroupMap", "SelectForClientGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap worker = new FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForClientGroup(clientGroupID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of ClientGroupSecurityGroupMap objects based on the security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <returns>response.result will contain a list of ClientGroupSecurityGroupMap objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> SelectForSecurityGroup(Guid accessKey, int securityGroupID)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>>();
            try
            {
                string param = "securityGroupID: " + securityGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupSecurityGroupMap", "SelectForSecurityGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap worker = new FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForSecurityGroup(securityGroupID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a ClientGroupSecurityGroupMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientGroupSecurityGroupMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.ClientGroupSecurityGroupMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupSecurityGroupMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap worker = new FrameworkUAS.BusinessLogic.ClientGroupSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
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
    }
}
