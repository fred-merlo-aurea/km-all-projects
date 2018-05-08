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
    public class ClientGroupUserMap : ServiceBase, IClientGroupUserMap
    {
        /// <summary>
        /// Selects a list of ClientGroupUserMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientGroupUserMap objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupUserMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "", "ClientGroupUserMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupUserMap worker = new FrameworkUAS.BusinessLogic.ClientGroupUserMap();
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
        /// Selects a list of ClientGroupUserMap objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a list of ClientGroupUserMap objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> SelectForClientGroup(Guid accessKey, int clientGroupID)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupUserMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "clientGroupID:" + clientGroupID.ToString(), "ClientGroupUserMap", "SelectForClientGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupUserMap worker = new FrameworkUAS.BusinessLogic.ClientGroupUserMap();
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
        /// Selects a list of ClientGroupUserMap objects based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of ClientGroupUserMap objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> SelectForUser(Guid accessKey, int userID)
        {
            Response<List<FrameworkUAS.Entity.ClientGroupUserMap>> response = new Response<List<FrameworkUAS.Entity.ClientGroupUserMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "userID:" + userID.ToString(), "ClientGroupUserMap", "SelectForUser");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupUserMap worker = new FrameworkUAS.BusinessLogic.ClientGroupUserMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForUser(userID);
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
        /// Saves a ClientGroupUserMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientGroupUserMap object</param>
        /// <returns>resposne.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.ClientGroupUserMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.ClientGroupUserMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupUserMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientGroupUserMap worker = new FrameworkUAS.BusinessLogic.ClientGroupUserMap();
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
