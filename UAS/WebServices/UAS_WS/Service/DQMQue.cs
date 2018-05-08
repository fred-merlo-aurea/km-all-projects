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
    public class DQMQue : ServiceBase, IDQMQue
    {
        /// <summary>
        /// Selects a list of DQMQue objects based on the client ID, its the object is part of the live or refresh server and if it is also part of ADMS or another API
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="isDemo">boolean if DQMQue object is part of live or refresh server</param>
        /// <param name="isADMS">boolean if DQMQue object is part of ADMS or other API</param>
        /// <returns>response.result will contain a list of DQMQue objects</returns>
        public Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, int clientID, bool isDemo, bool isADMS)
        {
            Response<List<FrameworkUAS.Entity.DQMQue>> response = new Response<List<FrameworkUAS.Entity.DQMQue>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " IsDemo:" + isDemo.ToString() + " IsADMS:" + isADMS.ToString(), "DQMQue", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DQMQue worker = new FrameworkUAS.BusinessLogic.DQMQue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientID, isDemo, isADMS);
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
        /// Selects a list of DQMQue objects based on if the object is part of the live or refresh server, if the object is part of ADMS or another API and if the object has been qued
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isDemo">boolean if DQMQue object is part of live or refresh server</param>
        /// <param name="isADMS">boolean if DQMQue object is part of ADMS or other API</param>
        /// <param name="isQued">boolean if the object has been qued</param>
        /// <returns>response.result will contain a list of DQMQue objects</returns>
        public Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, bool isDemo, bool isADMS, bool isQued = false)
        {
            Response<List<FrameworkUAS.Entity.DQMQue>> response = new Response<List<FrameworkUAS.Entity.DQMQue>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "IsDemo:" + isDemo.ToString() + " IsADMS:" + isADMS.ToString() + " IsQued:" + isQued.ToString(), "DQMQue", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DQMQue worker = new FrameworkUAS.BusinessLogic.DQMQue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(isDemo, isADMS, isQued);
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
        /// Selects a list of DQMQue objects based on if the object is part of the live or refresh server, 
        /// if the object is part of ADMS or another API, if the object has been qued and if the que has been completed
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="isDemo">boolean if the object is part of the live or refresh server</param>
        /// <param name="isADMS">boolean if the object is part of ADMS or another API</param>
        /// <param name="isQued">boolean if the object has been qued</param>
        /// <param name="isCompleted">boolean if the que has been completed</param>
        /// <returns>response.result will contain a list of DQMQue objects</returns>
        public Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, bool isDemo, bool isADMS, bool isQued = false, bool isCompleted = false)
        {
            Response<List<FrameworkUAS.Entity.DQMQue>> response = new Response<List<FrameworkUAS.Entity.DQMQue>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "IsDemo:" + isDemo.ToString() + " IsADMS:" + isADMS.ToString() + " IsQued:" + isQued.ToString() + " IsCompleted:" + isCompleted.ToString(), "DQMQue", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DQMQue worker = new FrameworkUAS.BusinessLogic.DQMQue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(isDemo, isADMS, isQued, isCompleted);
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
        /// Saves the DQMQue object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the DQMQue object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.DQMQue x)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.DQMQue>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "DQMQue", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.DQMQue worker = new FrameworkUAS.BusinessLogic.DQMQue();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result == true || response.Result == false)
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
